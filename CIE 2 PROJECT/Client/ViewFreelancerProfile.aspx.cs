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
    public partial class ViewFreelancerProfile : Page
    {
        private sealed class WorkItem
        {
            public string Title { get; set; }
            public string ClientName { get; set; }
            public string ClientProfileUrl { get; set; }
            public string CompletedDate { get; set; }
            public string Amount { get; set; }
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
                ShowError("Choose a valid freelancer profile to view.");
                return;
            }

            try
            {
                User user = ServiceFactory.CreateUserService().GetUser(userId);
                if (user == null || !string.Equals(user.Role, "Freelancer", StringComparison.OrdinalIgnoreCase))
                {
                    ShowError("This freelancer profile could not be found.");
                    return;
                }

                litFullName.Text = user.FullName;
                litUsername.Text = user.Username;
                litInitials.Text = GetInitials(user.FullName);
                litLocation.Text = Display(user.Location, "Location not shared");
                litMemberSince.Text = user.CreatedAt.ToString("MMMM yyyy", CultureInfo.InvariantCulture);
                litBio.Text = Display(user.Bio, "This freelancer has not added an introduction yet.");
                litEducation.Text = Display(user.Education, "Not provided");
                litOrganization.Text = Display(user.OrganizationName, "Not provided");
                SetProfileImage(user.ProfileImage);
                SetContactEmail(user.ContactEmail, user.Email);
                SetPortfolio(user.PortfolioUrl);

                List<string> skills = (user.Skills ?? string.Empty)
                    .Split(new[] { ',', ';', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(skill => skill.Trim())
                    .Where(skill => skill.Length > 0)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
                rptSkills.DataSource = skills;
                rptSkills.DataBind();
                pnlNoSkills.Visible = skills.Count == 0;

                IList<Contract> contracts = ServiceFactory.CreateContractService()
                    .GetForFreelancerHistory(userId);
                List<Contract> completed = contracts
                    .Where(contract => contract.ApprovalStatus == WorkflowStatuses.ContractCompleted)
                    .OrderByDescending(contract => contract.CompletedAt ?? contract.CreatedAt)
                    .ToList();
                var projectService = ServiceFactory.CreateProjectService();
                var userService = ServiceFactory.CreateUserService();
                IList<Payment> freelancerPayments = ServiceFactory.CreatePaymentService()
                    .GetFreelancerPaymentHistory(userId);
                int slowWorkerCount = FreelancerPerformanceHelper.GetSlowWorkerBadgeCount(
                    contracts, projectService, freelancerPayments);
                pnlSlowWorkerBadge.Visible = slowWorkerCount > 0;
                litSlowWorkerCount.Text = slowWorkerCount.ToString(CultureInfo.InvariantCulture);
                List<WorkItem> workItems = new List<WorkItem>();
                foreach (Contract contract in completed.Take(5))
                {
                    Project project = projectService.GetProject(contract.ProjectID);
                    User client = userService.GetUser(contract.ClientID);
                    Payment completedPayment = freelancerPayments.FirstOrDefault(payment =>
                        payment.ContractID == contract.ContractID &&
                        payment.PaymentStatus == WorkflowStatuses.PaymentReleased);
                    string clientName = client == null ? "Client" : client.FullName;
                    workItems.Add(new WorkItem
                    {
                        Title = project == null ? "Completed project" : project.Title,
                        ClientName = clientName,
                        ClientProfileUrl = "Profile.aspx?id=" + contract.ClientID.ToString(CultureInfo.InvariantCulture),
                        CompletedDate = (contract.CompletedAt ?? contract.CreatedAt).ToString("MMM yyyy", CultureInfo.InvariantCulture),
                        Amount = FormatCurrency(completedPayment == null
                            ? contract.AgreedAmount : completedPayment.FreelancerNetAmount)
                    });
                }
                rptWorkHistory.DataSource = workItems;
                rptWorkHistory.DataBind();
                pnlNoWorkHistory.Visible = workItems.Count == 0;

                IList<Review> reviews = ServiceFactory.CreateReviewService().GetReviewsForUser(userId);
                decimal average = reviews.Count == 0 ? 0m : reviews.Average(review => (decimal)review.Rating);
                litAverageRating.Text = average.ToString("0.0", CultureInfo.InvariantCulture);
                litStars.Text = RenderStars(average);
                litReviewCount.Text = reviews.Count.ToString(CultureInfo.InvariantCulture);
                string messageUrl = ResolveUrl("~/Messages/Messages.aspx?with=" + userId.ToString(CultureInfo.InvariantCulture));
                lnkSendMessage.NavigateUrl = messageUrl;
                lnkContactMessage.NavigateUrl = messageUrl;
                litCompletedProjects.Text = completed.Count.ToString(CultureInfo.InvariantCulture);
                litActiveProjects.Text = contracts.Count(contract =>
                    contract.ApprovalStatus != WorkflowStatuses.ContractCompleted &&
                    contract.ApprovalStatus != WorkflowStatuses.ContractCancelled)
                    .ToString(CultureInfo.InvariantCulture);
                litReleasedEarnings.Text = FormatCurrency(freelancerPayments
                    .Where(payment => payment.PaymentStatus == WorkflowStatuses.PaymentReleased ||
                        (payment.PaymentStatus == WorkflowStatuses.PaymentRefunded && payment.FreelancerPayoutAmount > 0m))
                    .Sum(payment => payment.PaymentStatus == WorkflowStatuses.PaymentReleased
                        ? payment.FreelancerNetAmount : payment.FreelancerPayoutAmount));

                List<ReviewItem> reviewItems = new List<ReviewItem>();
                foreach (Review review in reviews.Take(3))
                {
                    User reviewer = userService.GetUser(review.ReviewerID);
                    string reviewerName = reviewer == null ? "Client" : reviewer.FullName;
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
                lnkAllReviews.NavigateUrl = "Reviews.aspx?id=" + userId.ToString(CultureInfo.InvariantCulture) + "&viewer=client";
                pnlProfile.Visible = true;
                pnlProfileError.Visible = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Public freelancer profile failed to load: " + ex);
                ShowError(DatabaseErrorMessageHelper.GetUserMessage(
                    ex, "We could not load this freelancer profile. Please refresh and try again."));
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
            lnkPortfolio.Text = "Visit website";
            portfolioContactRow.Visible = true;
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

        private static string FormatCurrency(decimal amount)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-IN"), "₹{0:N0}", amount);
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
            return new string('★', Math.Max(0, Math.Min(5, fullStars))) +
                new string('☆', 5 - Math.Max(0, Math.Min(5, fullStars)));
        }
    }
}
