using System;

using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Models;
using CIE_2_PROJECT.Services;

namespace CIE_2_PROJECT
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Reset the server controls on every request so a consumed message
            // cannot reappear from control view state on a later postback.
            pnlFlashMessage.Visible = false;
            pnlFlashMessage.CssClass = "site-flash-message site-flash-success";
            pnlFlashMessage.Attributes["role"] = "status";
            pnlFlashMessage.Attributes["aria-live"] = "polite";
            flashMessageIcon.Attributes["class"] = "fa-solid fa-circle-check";
            litFlashMessage.Text = string.Empty;
            lnkDismissFlash.HRef = ResolveUrl(Request.AppRelativeCurrentExecutionFilePath) + Request.Url.Query;

            FlashMessageNotice flashMessage = FlashMessageStore.Consume();
            if (flashMessage != null)
            {
                pnlFlashMessage.Visible = true;
                pnlFlashMessage.CssClass = flashMessage.IsError
                    ? "site-flash-message site-flash-error"
                    : "site-flash-message site-flash-success";
                pnlFlashMessage.Attributes["role"] = flashMessage.IsError ? "alert" : "status";
                pnlFlashMessage.Attributes["aria-live"] = flashMessage.IsError ? "assertive" : "polite";
                flashMessageIcon.Attributes["class"] = flashMessage.IsError
                    ? "fa-solid fa-circle-exclamation"
                    : "fa-solid fa-circle-check";
                litFlashMessage.Text = flashMessage.Message;
            }

            string path = Request.AppRelativeCurrentExecutionFilePath;
            string role = AuthenticatedUser.Role;
            bool isClientPage = string.Equals(role, "Client", StringComparison.OrdinalIgnoreCase);

            freelancerNav.Visible = !isClientPage;
            clientNav.Visible = isClientPage;

            freelancerProfile.Visible = !isClientPage;
            clientProfile.Visible = isClientPage;

            if (isClientPage)
            {
                brandLink.HRef = ResolveUrl("~/Client/ClientHome.aspx");
                clientProfile.HRef = ResolveUrl("~/Client/Profile.aspx");
            }
            else
            {
                brandLink.HRef = ResolveUrl("~/Freelancer/FreelancerHome.aspx");
                freelancerProfile.HRef = ResolveUrl("~/Freelancer/Profile.aspx");
            }

            try
            {
                User currentUser = ServiceFactory.CreateUserService().GetUser(
                    AuthenticatedUser.UserID);
                string displayName = currentUser == null ? string.Empty : currentUser.FullName;
                if (isClientPage)
                    litClientNavName.Text = displayName;
                else
                    litFreelancerNavName.Text = displayName;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceWarning("Navigation profile name could not be loaded: " + ex.Message);
                if (isClientPage)
                    litClientNavName.Text = "Client";
                else
                    litFreelancerNavName.Text = "Freelancer";
            }

            try
            {
                int userId = AuthenticatedUser.UserID;
                int unreadNotifications = NotificationService.GetUnreadCount(userId);
                notificationBadge.Visible = unreadNotifications > 0;
                litNotificationCount.Text = unreadNotifications > 99 ? "99+" : unreadNotifications.ToString();

                int unreadMessages = ServiceFactory.CreateMessagingService().GetUnreadCount(userId);
                messageBadge.Visible = unreadMessages > 0;
                litMessageCount.Text = unreadMessages > 99 ? "99+" : unreadMessages.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceWarning("Unread navigation counts could not be loaded: " + ex.Message);
                notificationBadge.Visible = false;
                messageBadge.Visible = false;
            }
        }

        protected void logoutLink_Click(object sender, EventArgs e)
        {
            AuthenticatedUser.SignOut();
            Response.Redirect(ResolveUrl("~/Landing/Landing.aspx"), false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
