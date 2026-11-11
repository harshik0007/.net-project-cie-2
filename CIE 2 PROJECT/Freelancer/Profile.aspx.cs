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
    public partial class Profile : Page
    {
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
            if (profileUserId != TestUserContext.FreelancerID)
            {
                ShowProfileError("Only the profile owner can edit these details.");
                return;
            }

            User user = ServiceFactory.CreateUserService().GetUser(profileUserId);
            if (user == null)
            {
                ShowProfileError("The freelancer profile could not be found.");
                return;
            }

            txtFullName.Text = user.FullName;
            txtLocation.Text = user.Location ?? string.Empty;
            txtBio.Text = user.Bio ?? string.Empty;
            txtSkills.Text = user.Skills ?? string.Empty;
            txtEducation.Text = user.Education ?? string.Empty;
            txtOrganization.Text = user.OrganizationName ?? string.Empty;
            txtContactEmail.Text = user.ContactEmail ?? user.Email;
            txtPhoneNumber.Text = user.PhoneNumber ?? string.Empty;
            txtPortfolioUrl.Text = user.PortfolioUrl ?? string.Empty;
            txtPaymentAccountLabel.Text = user.PaymentAccountLabel ?? string.Empty;
            SelectPaymentMethod(user.PreferredPaymentMethod);
            litEditMessage.Text = string.Empty;
            pnlEditProfile.Visible = true;
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            string newProfileImage = null;
            bool profileSaved = false;
            try
            {
                int profileUserId = GetProfileUserId();
                if (profileUserId != TestUserContext.FreelancerID)
                    throw new InvalidOperationException("Only the profile owner can edit these details.");

                User user = ServiceFactory.CreateUserService().GetUser(profileUserId);
                if (user == null)
                    throw new InvalidOperationException("The freelancer profile could not be found.");

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
                    txtSkills.Text,
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
                IUserService userService = ServiceFactory.CreateUserService();
                IContractService contractService = ServiceFactory.CreateContractService();
                IReviewService reviewService = ServiceFactory.CreateReviewService();
                IProjectService projectService = ServiceFactory.CreateProjectService();

                bool isOwnProfile = profileUserId == TestUserContext.FreelancerID;
                User user = userService.GetUser(profileUserId);
                if (user == null)
                {
                    ShowProfileError("The freelancer profile could not be found.");
                    return;
                }

                if (!string.Equals(user.Role, "Freelancer", StringComparison.OrdinalIgnoreCase))
                {
                    ShowProfileError("This profile is not a freelancer profile.");
                    return;
                }

                IList<Contract> contracts = contractService
                    .GetForFreelancerHistory(profileUserId);
                IList<Review> reviews = reviewService
                    .GetReviewsForUser(profileUserId);
                IList<Payment> payments = ServiceFactory.CreatePaymentService()
                    .GetFreelancerPaymentHistory(profileUserId);

                litInitials.Text = GetInitials(user.FullName);
                litFullName.Text = user.FullName;
                litRole.Text = user.Role;
                litLocation.Text = DisplayOrDefault(user.Location, "Location not added");
                litMemberSince.Text = user.CreatedAt.ToString("MMMM yyyy", CultureInfo.InvariantCulture);
                litBio.Text = DisplayOrDefault(user.Bio, "Add a short introduction so clients can get to know you.");
                litEducation.Text = DisplayOrDefault(user.Education, "Education not added yet");
                litOrganization.Text = DisplayOrDefault(user.OrganizationName, "Not added yet");
                litEmail.Text = user.Email;
                litUsername.Text = user.Username;
                litContactLocation.Text = DisplayOrDefault(user.Location, "Not provided");
                litJoinedDate.Text = user.CreatedAt.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                btnEditProfile.Visible = isOwnProfile;
                pnlPrivateContact.Visible = isOwnProfile;
                pnlProfileSecurity.Visible = isOwnProfile;
                pnlEditProfile.Visible = isOwnProfile && pnlEditProfile.Visible;
                btnEditContactPayment.Visible = isOwnProfile;
                btnAddEducation.Visible = isOwnProfile &&
                    (string.IsNullOrWhiteSpace(user.Education) ||
                     string.IsNullOrWhiteSpace(user.OrganizationName) ||
                     string.IsNullOrWhiteSpace(user.PortfolioUrl));
                litContactEmail.Text = DisplayOrDefault(user.ContactEmail, "Contact email not added");
                litPhoneNumber.Text = DisplayOrDefault(user.PhoneNumber, "Phone not added");
                litPreferredPaymentMethod.Text = DisplayOrDefault(user.PreferredPaymentMethod, "Payout method not set");
                litPaymentAccountLabel.Text = DisplayOrDefault(user.PaymentAccountLabel, "Payout account label not added");
                SetPortfolioLink(user.PortfolioUrl);
                string reviewsUrl = "Reviews.aspx?id=" + profileUserId.ToString(CultureInfo.InvariantCulture) +
                    "&viewer=" + GetViewerRole();
                lnkAllReviews.NavigateUrl = reviewsUrl;
                lnkAllReviewsBottom.NavigateUrl = reviewsUrl;

                List<Payment> earnedPayments = payments
                    .Where(payment => payment.PaymentStatus == WorkflowStatuses.PaymentReleased ||
                        (payment.PaymentStatus == WorkflowStatuses.PaymentRefunded && payment.FreelancerPayoutAmount > 0m))
                    .ToList();
                litReleasedEarnings.Text = FormatCurrency(earnedPayments.Sum(payment =>
                    payment.PaymentStatus == WorkflowStatuses.PaymentReleased
                        ? payment.FreelancerNetAmount : payment.FreelancerPayoutAmount));
                litReleasedPaymentCount.Text = earnedPayments.Count.ToString(CultureInfo.InvariantCulture);

                SetProfileImage(user.ProfileImage);

                List<string> skills = (user.Skills ?? string.Empty)
                    .Split(new[] { ',', ';', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(skill => skill.Trim())
                    .Where(skill => skill.Length > 0)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
                rptSkills.DataSource = skills;
                rptSkills.DataBind();
                pnlNoSkills.Visible = skills.Count == 0;

                List<Contract> completedContracts = contracts
                    .Where(contract => contract.ApprovalStatus == WorkflowStatuses.ContractCompleted)
                    .OrderByDescending(contract => contract.CompletedAt ?? contract.CreatedAt)
                    .ToList();
                int activeContracts = contracts.Count(contract =>
                    contract.ApprovalStatus != WorkflowStatuses.ContractCompleted &&
                    contract.ApprovalStatus != WorkflowStatuses.ContractCancelled);
                int slowWorkerCount = FreelancerPerformanceHelper.GetSlowWorkerBadgeCount(
                    contracts, projectService, payments);
                pnlSlowWorkerBadge.Visible = slowWorkerCount > 0;
                litSlowWorkerCount.Text = slowWorkerCount.ToString(CultureInfo.InvariantCulture);

                List<WorkHistoryItem> workItems = new List<WorkHistoryItem>();
                foreach (Contract contract in completedContracts.Take(5))
                {
                    Project project = projectService.GetProject(contract.ProjectID);
                    User client = userService.GetUser(contract.ClientID);
                    Payment completedPayment = payments.FirstOrDefault(payment =>
                        payment.ContractID == contract.ContractID &&
                        payment.PaymentStatus == WorkflowStatuses.PaymentReleased);
                    workItems.Add(new WorkHistoryItem
                    {
                        Title = project == null ? "Project #" + contract.ProjectID : project.Title,
                        ClientName = client == null ? "Client #" + contract.ClientID : client.FullName,
                        ClientProfileUrl = "ViewClientProfile.aspx?id=" +
                            contract.ClientID.ToString(CultureInfo.InvariantCulture) + "&viewer=" + GetViewerRole(),
                        CompletedDate = (contract.CompletedAt ?? contract.CreatedAt)
                            .ToString("MMM yyyy", CultureInfo.InvariantCulture),
                        Amount = FormatCurrency(completedPayment == null
                            ? contract.AgreedAmount : completedPayment.FreelancerNetAmount)
                    });
                }
                rptWorkHistory.DataSource = workItems;
                rptWorkHistory.DataBind();
                pnlNoWorkHistory.Visible = workItems.Count == 0;

                decimal averageRating = reviews.Count == 0 ? 0 :
                    reviews.Average(review => (decimal)review.Rating);
                int positiveReviews = reviews.Count(review => review.Rating >= 4);
                litAverageRating.Text = averageRating.ToString("0.0", CultureInfo.InvariantCulture);
                litAverageRatingStat.Text = litAverageRating.Text;
                litRatingStars.Text = RenderStars(averageRating);
                litReviewCount.Text = reviews.Count.ToString(CultureInfo.InvariantCulture);
                litCompletedProjects.Text = completedContracts.Count.ToString(CultureInfo.InvariantCulture);
                litPositiveReviews.Text = positiveReviews.ToString(CultureInfo.InvariantCulture);
                litActiveContracts.Text = activeContracts.ToString(CultureInfo.InvariantCulture);
                litReputationStatus.Text = GetReputationStatus(reviews.Count, averageRating);

                int ratingPercent = (int)Math.Round(averageRating * 20, MidpointRounding.AwayFromZero);
                if (ratingPercent < 0) ratingPercent = 0;
                if (ratingPercent > 100) ratingPercent = 100;
                litRatingPercent.Text = ratingPercent.ToString(CultureInfo.InvariantCulture) + "%";
                pnlRatingFill.Style["width"] = ratingPercent.ToString(CultureInfo.InvariantCulture) + "%";

                List<ReviewItem> reviewItems = new List<ReviewItem>();
                foreach (Review review in reviews.Take(3))
                {
                    User reviewer = userService.GetUser(review.ReviewerID);
                    string reviewerName = reviewer == null ? "Client #" + review.ReviewerID : reviewer.FullName;
                    reviewItems.Add(new ReviewItem
                    {
                        ReviewerName = reviewerName,
                        ReviewerProfileUrl = "ViewClientProfile.aspx?id=" +
                            review.ReviewerID.ToString(CultureInfo.InvariantCulture) + "&viewer=" + GetViewerRole(),
                        Initials = GetInitials(reviewerName),
                        Rating = review.Rating.ToString(CultureInfo.InvariantCulture),
                        CreatedDate = review.CreatedAt.ToString("dd MMM yyyy", CultureInfo.InvariantCulture),
                        ReviewText = DisplayOrDefault(review.ReviewText, "No written comment was provided.")
                    });
                }
                rptReviews.DataSource = reviewItems;
                rptReviews.DataBind();
                pnlNoReviews.Visible = reviewItems.Count == 0;

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
                return TestUserContext.FreelancerID;

            int profileUserId;
            if (!int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out profileUserId) || profileUserId <= 0)
                throw new ArgumentException("A valid freelancer profile is required.");
            return profileUserId;
        }

        private string GetViewerRole()
        {
            string viewer = Request.QueryString["viewer"];
            return string.Equals(viewer, "client", StringComparison.OrdinalIgnoreCase)
                ? "client" : "freelancer";
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

        private void SetProfileImage(string storedPath)
        {
            string path = (storedPath ?? string.Empty).Trim().Replace('\\', '/');
            bool isSafeLocalPath = path.StartsWith("~/Uploads/ProfileImages/", StringComparison.OrdinalIgnoreCase) &&
                path.IndexOf("..", StringComparison.Ordinal) < 0 &&
                path.IndexOf("://", StringComparison.Ordinal) < 0;

            imgProfileImage.Visible = isSafeLocalPath;
            pnlInitials.Visible = !isSafeLocalPath;
            if (isSafeLocalPath)
                imgProfileImage.ImageUrl = ResolveUrl(path);
        }

        private void ShowProfileError(string message)
        {
            litProfileError.Text = message;
            pnlProfileError.Visible = true;
            pnlProfileContent.Visible = false;
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

        private static string GetReputationStatus(int reviewCount, decimal averageRating)
        {
            if (reviewCount == 0)
                return "No reviews yet";
            if (averageRating >= 4.5m)
                return "Highly rated";
            if (averageRating >= 3.5m)
                return "Well rated";
            return "Building reputation";
        }

        private sealed class WorkHistoryItem
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
            public string Initials { get; set; }
            public string Rating { get; set; }
            public string CreatedDate { get; set; }
            public string ReviewText { get; set; }
        }
    }
}
