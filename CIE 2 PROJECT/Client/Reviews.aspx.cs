using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class ClientReviews : Page
    {
        private sealed class ReviewView
        {
            public string Reviewer { get; set; }
            public string ReviewerProfileUrl { get; set; }
            public int Rating { get; set; }
            public string CreatedAt { get; set; }
            public string ReviewText { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadReviews();
        }

        private void LoadReviews()
        {
            try
            {
                IReviewService reviewService = ServiceFactory.CreateReviewService();
                IUserService userService = ServiceFactory.CreateUserService();
                int clientId = GetClientId();
                User client = userService.GetUser(clientId);
                if (client == null || !string.Equals(client.Role, "Client", StringComparison.OrdinalIgnoreCase))
                {
                    lblMessage.Text = "This client profile could not be found.";
                    lblMessage.Visible = true;
                    pnlEmpty.Visible = false;
                    return;
                }

                string viewerRole = GetViewerRole();
                lnkBackProfile.NavigateUrl = "Profile.aspx?id=" + clientId.ToString(CultureInfo.InvariantCulture) +
                    "&viewer=" + viewerRole;
                IList<Review> reviews = reviewService.GetReviewsForUser(clientId);
                List<ReviewView> rows = new List<ReviewView>();
                foreach (Review review in reviews)
                {
                    User reviewer = userService.GetUser(review.ReviewerID);
                    rows.Add(new ReviewView
                    {
                        Reviewer = reviewer == null ? "User #" + review.ReviewerID : reviewer.FullName,
                        ReviewerProfileUrl = "ViewFreelancerProfile.aspx?id=" +
                            review.ReviewerID.ToString(CultureInfo.InvariantCulture) + "&viewer=" + viewerRole,
                        Rating = review.Rating,
                        CreatedAt = review.CreatedAt.ToString("dd MMM yyyy", CultureInfo.InvariantCulture),
                        ReviewText = review.ReviewText
                    });
                }
                lblAverageRating.Text = reviewService.GetAverageRating(clientId)
                    .ToString("0.0", CultureInfo.InvariantCulture);
                lblReviewCount.Text = reviewService.GetReviewCount(clientId)
                    .ToString(CultureInfo.InvariantCulture);
                rptReviews.DataSource = rows;
                rptReviews.DataBind();
                pnlEmpty.Visible = rows.Count == 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Client review history failed to load: " + ex);
                lblMessage.Text = "Unable to load reviews right now.";
                lblMessage.Visible = true;
                pnlEmpty.Visible = false;
            }
        }

        private int GetClientId()
        {
            string value = Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(value))
                return TestUserContext.ClientID;

            int clientId;
            if (!int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out clientId) || clientId <= 0)
                throw new ArgumentException("A valid client profile is required.");
            return clientId;
        }

        private string GetViewerRole()
        {
            string viewer = Request.QueryString["viewer"];
            return string.Equals(viewer, "freelancer", StringComparison.OrdinalIgnoreCase)
                ? "freelancer" : "client";
        }
    }
}
