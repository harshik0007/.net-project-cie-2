using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class ViewClientProfile : Page
    {
        private sealed class ProjectItem
        {
            public string Title { get; set; }
            public string Category { get; set; }
            public string Status { get; set; }
            public string StatusClass { get; set; }
        }

        private sealed class ReviewItem
        {
            public string ReviewerName { get; set; }
            public string ReviewerProfileUrl { get; set; }
            public string CreatedDate { get; set; }
            public string Rating { get; set; }
            public string ReviewText { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindProfile();
        }

        private void BindProfile()
        {
            int userId;
            if (!int.TryParse(Request.QueryString["id"], NumberStyles.None,
                CultureInfo.InvariantCulture, out userId) || userId <= 0)
            {
                ShowError("Choose a valid client profile to view.");
                return;
            }

            try
            {
                User user = ServiceFactory.CreateUserService().GetUser(userId);
                if (user == null || !string.Equals(user.Role, "Client", StringComparison.OrdinalIgnoreCase))
                {
                    ShowError("This client profile could not be found.");
                    return;
                }

                litFullName.Text = user.FullName;
                litUsername.Text = user.Username;
                litInitials.Text = GetInitials(user.FullName);
                litLocation.Text = Display(user.Location, "Location not shared");
                litMemberSince.Text = user.CreatedAt.ToString("MMMM yyyy", CultureInfo.InvariantCulture);
                litBio.Text = Display(user.Bio, "This client has not added an introduction yet.");
                litOrganization.Text = Display(user.OrganizationName, "Not provided");
                SetProfileImage(user.ProfileImage);
                SetContactEmail(user.ContactEmail, user.Email);
                SetPortfolio(user.PortfolioUrl);

                IList<Project> projects = ServiceFactory.CreateProjectService().GetClientProjects(userId);
                List<Project> completed = projects.Where(project =>
                    project.Status == WorkflowStatuses.ProjectCompleted).ToList();
                List<ProjectItem> projectItems = projects
                    .OrderByDescending(project => project.CreatedAt)
                    .Take(5)
                    .Select(project => new ProjectItem
                    {
                        Title = project.Title,
                        Category = Display(project.Category, "Project"),
                        Status = project.Status,
                        StatusClass = GetStatusClass(project.Status)
                    }).ToList();
                rptProjects.DataSource = projectItems;
                rptProjects.DataBind();
                pnlNoProjects.Visible = projectItems.Count == 0;
                litProjectCount.Text = projects.Count.ToString(CultureInfo.InvariantCulture);
                litActiveProjects.Text = projects.Count(project =>
                    project.Status == WorkflowStatuses.ProjectOpen ||
                    project.Status == WorkflowStatuses.ProjectInProgress)
                    .ToString(CultureInfo.InvariantCulture);
                litCompletedProjects.Text = completed.Count.ToString(CultureInfo.InvariantCulture);
                litCompletedProjectsSide.Text = litCompletedProjects.Text;

                IList<Review> reviews = ServiceFactory.CreateReviewService().GetReviewsForUser(userId);
                decimal average = reviews.Count == 0 ? 0m : reviews.Average(review => (decimal)review.Rating);
                litAverageRating.Text = average.ToString("0.0", CultureInfo.InvariantCulture);
                litStars.Text = RenderStars(average);
                litReviewCount.Text = reviews.Count.ToString(CultureInfo.InvariantCulture);
                string messageUrl = ResolveUrl("~/Messages/Messages.aspx?with=" + userId.ToString(CultureInfo.InvariantCulture));
                lnkSendMessage.NavigateUrl = messageUrl;
                lnkContactMessage.NavigateUrl = messageUrl;
                var userService = ServiceFactory.CreateUserService();
                List<ReviewItem> reviewItems = new List<ReviewItem>();
                foreach (Review review in reviews.Take(3))
                {
                    User reviewer = userService.GetUser(review.ReviewerID);
                    string reviewerName = reviewer == null ? "Freelancer" : reviewer.FullName;
                    reviewItems.Add(new ReviewItem
                    {
                        ReviewerName = reviewerName,
                        ReviewerProfileUrl = "Profile.aspx?id=" + review.ReviewerID.ToString(CultureInfo.InvariantCulture),
                        CreatedDate = review.CreatedAt.ToString("dd MMM yyyy", CultureInfo.InvariantCulture),
                        Rating = review.Rating.ToString(CultureInfo.InvariantCulture),
                        ReviewText = Display(review.ReviewText, "No written comment was provided.")
                    });
                }
                rptReviews.DataSource = reviewItems;
                rptReviews.DataBind();
                pnlNoReviews.Visible = reviewItems.Count == 0;
                lnkAllReviews.NavigateUrl = "Reviews.aspx?id=" + userId.ToString(CultureInfo.InvariantCulture) + "&viewer=freelancer";
                pnlProfile.Visible = true;
                pnlProfileError.Visible = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Public client profile failed to load: " + ex);
                ShowError(DatabaseErrorMessageHelper.GetUserMessage(
                    ex, "We could not load this client profile. Please refresh and try again."));
            }
        }

        private void SetProfileImage(string storedPath)
        {
            string path = (storedPath ?? string.Empty).Trim().Replace('\\', '/');
            if (path.StartsWith("~/Uploads/ProfileImages/", StringComparison.OrdinalIgnoreCase) &&
                path.IndexOf("..", StringComparison.Ordinal) < 0)
            {
                imgProfile.ImageUrl = ResolveUrl(path);
                imgProfile.Visible = true;
                pnlInitials.Visible = false;
            }
            else
            {
                imgProfile.Visible = false;
                pnlInitials.Visible = true;
            }
        }

        private void SetContactEmail(string contactEmail, string accountEmail)
        {
            string email = string.IsNullOrWhiteSpace(contactEmail) ? accountEmail : contactEmail;
            if (string.IsNullOrWhiteSpace(email))
            {
                lnkContactEmail.Visible = false;
                return;
            }
            email = email.Trim();
            lnkContactEmail.Text = Server.HtmlEncode(email);
            lnkContactEmail.NavigateUrl = "mailto:" + Uri.EscapeDataString(email);
        }

        private void SetPortfolio(string value)
        {
            Uri url;
            if (!Uri.TryCreate(value, UriKind.Absolute, out url) ||
                (url.Scheme != Uri.UriSchemeHttp && url.Scheme != Uri.UriSchemeHttps))
            {
                portfolioContactRow.Visible = false;
                return;
            }
            lnkPortfolio.NavigateUrl = url.AbsoluteUri;
            lnkPortfolio.Attributes["rel"] = "noopener noreferrer";
            portfolioContactRow.Visible = true;
        }

        private void ShowError(string message)
        {
            litProfileError.Text = message;
            pnlProfileError.Visible = true;
            pnlProfile.Visible = false;
        }

        private static string Display(string value, string fallback)
        {
            return string.IsNullOrWhiteSpace(value) ? fallback : value;
        }

        private static string GetStatusClass(string status)
        {
            if (status == WorkflowStatuses.ProjectCompleted) return "status-completed";
            if (status == WorkflowStatuses.ProjectInProgress) return "status-progress";
            if (status == WorkflowStatuses.ProjectCancelled) return "status-cancelled";
            return "status-open";
        }

        private static string GetInitials(string fullName)
        {
            string[] names = (fullName ?? string.Empty).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (names.Length == 0) return "?";
            return names.Length == 1 ? names[0].Substring(0, 1).ToUpperInvariant() :
                (names[0].Substring(0, 1) + names[names.Length - 1].Substring(0, 1)).ToUpperInvariant();
        }

        private static string RenderStars(decimal rating)
        {
            int fullStars = (int)Math.Round(rating, MidpointRounding.AwayFromZero);
            fullStars = Math.Max(0, Math.Min(5, fullStars));
            return new string('★', fullStars) + new string('☆', 5 - fullStars);
        }
    }
}
