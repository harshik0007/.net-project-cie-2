using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class ClientProfile : Page
    {
        private sealed class ClientReviewItem
        {
            public string ReviewerName { get; set; }
            public string ReviewerProfileUrl { get; set; }
            public string CreatedDate { get; set; }
            public string Rating { get; set; }
            public string ReviewText { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Form != null)
                Page.Form.Enctype = "multipart/form-data";
            if (!IsPostBack)
                BindProfile();
        }

        protected void btnEditProfile_Click(object sender, EventArgs e)
        {
            int profileUserId = GetProfileUserId();
            if (profileUserId != TestUserContext.ClientID)
            {
                ShowProfileError("Only the profile owner can edit these details.");
                return;
            }

            User user = ServiceFactory.CreateUserService().GetUser(profileUserId);
            if (user == null)
            {
                ShowProfileError("The client profile could not be found.");
                return;
            }

            txtFullName.Text = user.FullName;
            txtLocation.Text = user.Location ?? string.Empty;
            txtBio.Text = user.Bio ?? string.Empty;
            txtEducation.Text = user.Education ?? string.Empty;
            txtOrganization.Text = user.OrganizationName ?? string.Empty;
            txtContactEmail.Text = user.ContactEmail ?? user.Email;
            txtPhoneNumber.Text = user.PhoneNumber ?? string.Empty;
            txtPortfolioUrl.Text = user.PortfolioUrl ?? string.Empty;
            txtPaymentAccountLabel.Text = user.PaymentAccountLabel ?? string.Empty;
            SelectPaymentMethod(user.PreferredPaymentMethod);
            pnlEditProfile.Visible = true;
            litEditMessage.Text = string.Empty;
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            string newProfileImage = null;
            bool profileSaved = false;
            try
            {
                int profileUserId = GetProfileUserId();
                if (profileUserId != TestUserContext.ClientID)
                    throw new InvalidOperationException("Only the profile owner can edit these details.");

                User user = ServiceFactory.CreateUserService().GetUser(profileUserId);
                if (user == null)
                    throw new InvalidOperationException("The client profile could not be found.");

                string profileImage = user.ProfileImage;
                if (fuProfilePhoto.HasFile)
                {
                    newProfileImage = ProfileImageStorage.Save(fuProfilePhoto.PostedFile, Server.MapPath("~/"));
                    profileImage = newProfileImage;
                }

                ServiceFactory.CreateUserService().UpdateProfile(
                    profileUserId,
                    txtFullName.Text,
                    txtBio.Text,
                    txtLocation.Text,
                    user.Skills,
                    txtEducation.Text,
                    txtContactEmail.Text,
                    txtPhoneNumber.Text,
                    txtOrganization.Text,
                    txtPortfolioUrl.Text,
                    ddlPreferredPaymentMethod.SelectedValue,
                    txtPaymentAccountLabel.Text,
                    profileImage);

                profileSaved = true;
                FlashMessageStore.Success("Your profile was updated successfully.");
                Response.Redirect("Profile.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (ArgumentException ex)
            {
                litEditMessage.Text = ex.Message;
                pnlEditProfile.Visible = true;
            }
            catch (InvalidOperationException ex)
            {
                litEditMessage.Text = ex.Message;
                pnlEditProfile.Visible = true;
            }
            catch
            {
                litEditMessage.Text = "Profile could not be saved. Please try again.";
                pnlEditProfile.Visible = true;
            }
            finally
            {
                if (!profileSaved && !string.IsNullOrWhiteSpace(newProfileImage))
                    ProfileImageStorage.Delete(newProfileImage, Server.MapPath("~/"));
            }
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            pnlEditProfile.Visible = false;
            litEditMessage.Text = string.Empty;
        }

        private void BindProfile()
        {
            try
            {
                int profileUserId = GetProfileUserId();
                bool isOwnProfile = profileUserId == TestUserContext.ClientID;
                User user = ServiceFactory.CreateUserService().GetUser(profileUserId);
                if (user == null)
                {
                    ShowProfileError("The client profile could not be found.");
                    return;
                }

                if (!string.Equals(user.Role, "Client", StringComparison.OrdinalIgnoreCase))
                {
                    ShowProfileError("This profile is not a client profile.");
                    return;
                }

                IList<Project> projects = ServiceFactory.CreateProjectService()
                    .GetClientProjects(profileUserId);
                IList<Payment> payments = isOwnProfile
                    ? ServiceFactory.CreatePaymentService().GetClientPaymentHistory(profileUserId)
                    : new List<Payment>();
                IList<Review> reviews = ServiceFactory.CreateReviewService()
                    .GetReviewsForUser(profileUserId);

                litInitials.Text = GetInitials(user.FullName);
                litFullName.Text = user.FullName;
                litRole.Text = user.Role;
                litLocation.Text = DisplayOrDefault(user.Location, "Location not added");
                litMemberSince.Text = user.CreatedAt.ToString("MMMM yyyy", CultureInfo.InvariantCulture);
                litAboutName.Text = user.FullName;
                litAboutLocation.Text = DisplayOrDefault(user.Location, "Not provided");
                litBio.Text = DisplayOrDefault(user.Bio, "Add a short introduction to tell freelancers about your projects.");
                litEducation.Text = DisplayOrDefault(user.Education, "Education not added yet");
                litOrganization.Text = DisplayOrDefault(user.OrganizationName, "Not added yet");
                litEmail.Text = user.Email;
                litUsername.Text = "@" + user.Username;
                litAccountRole.Text = user.Role;
                SetProfileImage(user.ProfileImage);
                btnEditProfile.Visible = isOwnProfile;
                pnlAccountInfo.Visible = isOwnProfile;
                pnlPaymentSummary.Visible = isOwnProfile;
                pnlSecurityCard.Visible = isOwnProfile;
                pnlPrivateContactAndPayment.Visible = isOwnProfile;
                pnlProfileExtraGrid.CssClass = isOwnProfile
                    ? "profile-grid profile-extra-grid"
                    : "profile-grid profile-extra-grid profile-extra-single";
                btnEditContactPayment.Visible = isOwnProfile;
                btnAddEducation.Visible = isOwnProfile &&
                    (string.IsNullOrWhiteSpace(user.Education) ||
                     string.IsNullOrWhiteSpace(user.OrganizationName) ||
                     string.IsNullOrWhiteSpace(user.PortfolioUrl));
                litContactEmailValue.Text = DisplayOrDefault(user.ContactEmail, "Not added yet");
                litPhoneNumberValue.Text = DisplayOrDefault(user.PhoneNumber, "Not added yet");
                litPreferredPaymentMethod.Text = DisplayOrDefault(user.PreferredPaymentMethod, "Not set");
                litPaymentAccountLabel.Text = DisplayOrDefault(user.PaymentAccountLabel, "Not added yet");
                SetPortfolioLink(user.PortfolioUrl);
                pnlEditProfile.Visible = isOwnProfile && pnlEditProfile.Visible;
                lnkAllClientReviews.NavigateUrl = "Reviews.aspx?id=" +
                    profileUserId.ToString(CultureInfo.InvariantCulture) + "&viewer=" + GetViewerRole();

                int completedProjects = projects.Count(project => project.Status == WorkflowStatuses.ProjectCompleted);
                int activeProjects = projects.Count(project => project.Status == WorkflowStatuses.ProjectInProgress);
                litProjectsPosted.Text = projects.Count.ToString(CultureInfo.InvariantCulture);
                litProjectsCompleted.Text = completedProjects.ToString(CultureInfo.InvariantCulture);
                litActiveProjects.Text = activeProjects.ToString(CultureInfo.InvariantCulture);

                decimal released = SumPayments(payments, WorkflowStatuses.PaymentReleased);
                decimal held = SumPayments(payments, WorkflowStatuses.PaymentHeld);
                int paidCount = payments.Count(payment =>
                    payment.PaymentStatus == WorkflowStatuses.PaymentReleased ||
                    payment.PaymentStatus == WorkflowStatuses.PaymentHeld);
                litTotalSpent.Text = FormatCurrency(released);
                litReleasedAmount.Text = FormatCurrency(released);
                litEscrowHeld.Text = FormatCurrency(held);
                litPaymentCount.Text = paidCount.ToString(CultureInfo.InvariantCulture);

                decimal averageRating = reviews.Count == 0 ? 0 :
                    reviews.Average(review => (decimal)review.Rating);
                int reviewCount = reviews.Count;
                litAverageRating.Text = averageRating.ToString("0.0", CultureInfo.InvariantCulture);
                litRatingStars.Text = RenderStars(averageRating);
                litReviewCount.Text = reviewCount.ToString(CultureInfo.InvariantCulture);
                litCompletedCount.Text = completedProjects.ToString(CultureInfo.InvariantCulture);
                litReviewCountSummary.Text = reviewCount.ToString(CultureInfo.InvariantCulture);

                IUserService userService = ServiceFactory.CreateUserService();
                List<ClientReviewItem> reviewItems = new List<ClientReviewItem>();
                foreach (Review review in reviews.Take(3))
                {
                    User reviewer = userService.GetUser(review.ReviewerID);
                    reviewItems.Add(new ClientReviewItem
                    {
                        ReviewerName = reviewer == null ? "Freelancer #" + review.ReviewerID : reviewer.FullName,
                        ReviewerProfileUrl = "ViewFreelancerProfile.aspx?id=" +
                            review.ReviewerID.ToString(CultureInfo.InvariantCulture) + "&viewer=" + GetViewerRole(),
                        CreatedDate = review.CreatedAt.ToString("dd MMM yyyy", CultureInfo.InvariantCulture),
                        Rating = review.Rating.ToString(CultureInfo.InvariantCulture),
                        ReviewText = string.IsNullOrWhiteSpace(review.ReviewText)
                            ? "No written feedback was included."
                            : review.ReviewText
                    });
                }
                rptClientReviews.DataSource = reviewItems;
                rptClientReviews.DataBind();
                pnlNoClientReviews.Visible = reviewItems.Count == 0;

                rptRecentProjects.DataSource = projects.Take(3).Select(project => new ProjectProfileItem
                {
                    Title = project.Title,
                    Category = project.Category,
                    Status = project.Status,
                    StatusClass = GetStatusClass(project.Status),
                    Url = "ProjectDetails.aspx?id=" + project.ProjectID.ToString(CultureInfo.InvariantCulture)
                }).ToList();
                rptRecentProjects.DataBind();
                pnlNoProjects.Visible = projects.Count == 0;

                pnlProfileContent.Visible = true;
                pnlProfileError.Visible = false;
            }
            catch (Exception ex)
            {
                ShowProfileError(DatabaseErrorMessageHelper.GetUserMessage(
                    ex, "We could not load the profile right now. Please refresh and try again."));
            }
        }

        private int GetProfileUserId()
        {
            string value = Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(value))
                return TestUserContext.ClientID;

            int userId;
            if (!int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out userId) || userId <= 0)
                throw new ArgumentException("A valid client profile is required.");
            return userId;
        }

        private string GetViewerRole()
        {
            string viewer = Request.QueryString["viewer"];
            return string.Equals(viewer, "freelancer", StringComparison.OrdinalIgnoreCase)
                ? "freelancer" : "client";
        }

        private void SelectPaymentMethod(string value)
        {
            ddlPreferredPaymentMethod.ClearSelection();
            ListItem item = ddlPreferredPaymentMethod.Items.FindByValue(value ?? string.Empty);
            if (item == null && !string.IsNullOrWhiteSpace(value))
                item = ddlPreferredPaymentMethod.Items.FindByValue("Other");
            if (item != null)
                item.Selected = true;
        }

        private void SetPortfolioLink(string value)
        {
            Uri url;
            bool valid = Uri.TryCreate(value, UriKind.Absolute, out url) &&
                (url.Scheme == Uri.UriSchemeHttp || url.Scheme == Uri.UriSchemeHttps);
            lnkPortfolio.Visible = valid;
            if (!valid)
                return;

            lnkPortfolio.NavigateUrl = url.AbsoluteUri;
            lnkPortfolio.Attributes["rel"] = "noopener noreferrer";
        }

        private void ShowProfileError(string message)
        {
            litProfileError.Text = message;
            pnlProfileError.Visible = true;
            pnlProfileContent.Visible = false;
        }

        private static decimal SumPayments(IEnumerable<Payment> payments, string status)
        {
            return payments.Where(payment => payment.PaymentStatus == status)
                .Sum(payment => payment.ClientTotalAmount);
        }

        private static string FormatCurrency(decimal amount)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-IN"), "₹{0:N0}", amount);
        }

        private static string DisplayOrDefault(string value, string fallback)
        {
            return string.IsNullOrWhiteSpace(value) ? fallback : value;
        }

        private static string GetInitials(string fullName)
        {
            string[] parts = (fullName ?? string.Empty).Split(
                new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return "?";
            if (parts.Length == 1)
                return parts[0].Substring(0, 1).ToUpperInvariant();
            return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpperInvariant();
        }

        private void SetProfileImage(string storedPath)
        {
            string path = (storedPath ?? string.Empty).Trim().Replace('\\', '/');
            bool isSafeLocalPath = path.StartsWith("~/Uploads/ProfileImages/", StringComparison.OrdinalIgnoreCase) &&
                path.IndexOf("..", StringComparison.Ordinal) < 0 &&
                path.IndexOf("://", StringComparison.Ordinal) < 0;

            imgProfileImage.Visible = isSafeLocalPath;
            pnlAvatarInitials.Visible = !isSafeLocalPath;
            if (isSafeLocalPath)
                imgProfileImage.ImageUrl = ResolveUrl(path);
        }

        private static string RenderStars(decimal rating)
        {
            int filled = (int)Math.Round(rating, MidpointRounding.AwayFromZero);
            if (filled < 0) filled = 0;
            if (filled > 5) filled = 5;
            return string.Concat(Enumerable.Range(1, 5).Select(index =>
                index <= filled
                    ? "<i class=\"fa-solid fa-star\"></i>"
                    : "<i class=\"fa-regular fa-star\"></i>"));
        }

        private static string GetStatusClass(string status)
        {
            if (status == WorkflowStatuses.ProjectInProgress)
                return "status-progress";
            if (status == WorkflowStatuses.ProjectCompleted)
                return "status-completed";
            if (status == WorkflowStatuses.ProjectCancelled)
                return "status-cancelled";
            return "status-open";
        }

        private sealed class ProjectProfileItem
        {
            public string Title { get; set; }
            public string Category { get; set; }
            public string Status { get; set; }
            public string StatusClass { get; set; }
            public string Url { get; set; }
        }
    }
}
