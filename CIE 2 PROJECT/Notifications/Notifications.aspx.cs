using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Models;
using CIE_2_PROJECT.Services;

namespace CIE_2_PROJECT
{
    public partial class NotificationsPage : Page
    {
        private sealed class NotificationItem
        {
            public int NotificationID { get; set; }
            public bool IsRead { get; set; }
            public string ReadClass { get; set; }
            public string Title { get; set; }
            public string MessageText { get; set; }
            public string CreatedText { get; set; }
            public string IconClass { get; set; }
            public string IconName { get; set; }
            public string TargetUrl { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindNotifications(IsUnreadFilter());
        }

        protected void btnMarkAllRead_Click(object sender, EventArgs e)
        {
            try
            {
                NotificationService.MarkAllRead(AuthenticatedUser.UserID);
                bool unreadOnly = IsUnreadFilter();
                FlashMessageStore.Success("All notifications have been marked as read.");
                Response.Redirect("Notifications.aspx" + (unreadOnly ? "?filter=unread" : string.Empty), false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Marking notifications as read failed: " + ex);
                bool unreadOnly = IsUnreadFilter();
                FlashMessageStore.Error("Notifications could not be updated. Please try again.");
                Response.Redirect("Notifications.aspx" + (unreadOnly ? "?filter=unread" : string.Empty), false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void rptNotifications_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!string.Equals(e.CommandName, "MarkRead", StringComparison.Ordinal) &&
                !string.Equals(e.CommandName, "Delete", StringComparison.Ordinal))
                return;

            int notificationId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument, CultureInfo.InvariantCulture), out notificationId) ||
                notificationId <= 0)
                return;

            int userId = AuthenticatedUser.UserID;
            if (string.Equals(e.CommandName, "Delete", StringComparison.Ordinal))
            {
                try
                {
                    NotificationService.DeleteForUser(userId, notificationId);
                    bool unreadOnly = IsUnreadFilter();
                    FlashMessageStore.Success("Notification deleted.");
                    Response.Redirect("Notifications.aspx" + (unreadOnly ? "?filter=unread" : string.Empty), false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("Deleting a notification failed: " + ex);
                    bool unreadOnly = IsUnreadFilter();
                    FlashMessageStore.Error("The notification could not be deleted. Please try again.");
                    Response.Redirect("Notifications.aspx" + (unreadOnly ? "?filter=unread" : string.Empty), false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                return;
            }

            Notification notification = NotificationService.GetForUser(userId, notificationId);
            if (notification == null)
                return;

            NotificationService.MarkRead(userId, notificationId);
            lblMessage.Text = "Notification marked as read.";
            lblMessage.Visible = true;
            BindNotifications(IsUnreadFilter());
        }

        private void BindNotifications(bool unreadOnly)
        {
            int userId = AuthenticatedUser.UserID;
            IList<Notification> notifications = NotificationService.GetForUser(userId, unreadOnly);
            List<NotificationItem> items = new List<NotificationItem>();
            foreach (Notification notification in notifications)
            {
                string iconClass;
                string iconName;
                GetIcon(notification.Type, out iconClass, out iconName);
                items.Add(new NotificationItem
                {
                    NotificationID = notification.NotificationID,
                    IsRead = notification.IsRead,
                    ReadClass = notification.IsRead ? "read" : "unread",
                    Title = GetTitle(notification.Type),
                    MessageText = notification.MessageText,
                    CreatedText = notification.CreatedAt.ToString("dd MMM yyyy, h:mm tt", CultureInfo.InvariantCulture),
                    IconClass = iconClass,
                    IconName = iconName,
                    TargetUrl = ResolveUrl(GetTargetUrl(notification))
                });
            }

            rptNotifications.DataSource = items;
            rptNotifications.DataBind();
            pnlEmpty.Visible = items.Count == 0;
            litEmptyTitle.Text = unreadOnly ? "You’re all caught up" : "No notifications yet";
            litEmptyText.Text = unreadOnly
                ? "New unread updates will appear here."
                : "Updates about proposals, work, messages, payments, and reviews will appear here.";
            litUnreadCount.Text = NotificationService.GetUnreadCount(userId).ToString(CultureInfo.InvariantCulture);
            btnAll.CssClass = unreadOnly ? "notification-tab" : "notification-tab active-tab";
            btnUnread.CssClass = unreadOnly ? "notification-tab active-tab" : "notification-tab";
        }

        private bool IsUnreadFilter()
        {
            return string.Equals(Request.QueryString["filter"], "unread", StringComparison.OrdinalIgnoreCase);
        }

        private string GetTargetUrl(Notification notification)
        {
            string role = AuthenticatedUser.Role;
            if (notification.Type == "NewMessage")
                return "~/Messages/Messages.aspx";
            if (notification.Type == "ReviewReceived")
                return role == "Client" ? "~/Client/Profile.aspx" : "~/Freelancer/Profile.aspx";

            if (role == "Client")
            {
                if (notification.Type == "NewProposal" && notification.ProjectID.HasValue)
                    return "~/Client/ProjectProposal.aspx?projectId=" + notification.ProjectID.Value.ToString(CultureInfo.InvariantCulture);
                if (notification.Type == "WorkSubmitted" && notification.ProjectID.HasValue)
                    return "~/Client/ReviewSubmittedWork.aspx?projectId=" + notification.ProjectID.Value.ToString(CultureInfo.InvariantCulture);
                return "~/Client/MyProjects.aspx";
            }

            if (notification.Type == "PaymentReleased")
                return "~/Freelancer/WorkHistory.aspx";
            if (notification.Type == "ProposalRejected")
                return "~/Freelancer/MyProposals.aspx?filter=rejected";
            if (notification.Type == "ProposalAccepted" || notification.Type == "PaymentFunded" ||
                notification.Type == "RevisionRequested" || notification.Type == "WorkApproved" ||
                notification.Type == "ProjectCancelled")
                return "~/Freelancer/ActiveWork.aspx";
            return "~/Freelancer/MyProposals.aspx";
        }

        private static string GetTitle(string type)
        {
            switch (type)
            {
                case "NewProposal": return "New proposal received";
                case "ProposalAccepted": return "Proposal accepted";
                case "ProposalRejected": return "Proposal not selected";
                case "PaymentFunded": return "Contract funded";
                case "PaymentReleased": return "Payment released";
                case "WorkSubmitted": return "Work submitted";
                case "RevisionRequested": return "Revision requested";
                case "WorkApproved": return "Work approved";
                case "ReviewReceived": return "New rating and review";
                case "NewMessage": return "New message";
                case "ProjectCancelled": return "Project cancelled";
                default: return "Project update";
            }
        }

        private static void GetIcon(string type, out string iconClass, out string iconName)
        {
            if (type == "NewMessage")
            {
                iconClass = "message-notification";
                iconName = "fa-regular fa-envelope";
            }
            else if (type == "PaymentReleased" || type == "PaymentFunded")
            {
                iconClass = "payment-notification";
                iconName = "fa-solid fa-wallet";
            }
            else if (type == "NewProposal" || type == "ProposalAccepted" || type == "ProposalRejected")
            {
                iconClass = "proposal-icon";
                iconName = "fa-solid fa-file-signature";
            }
            else if (type == "ReviewReceived")
            {
                iconClass = "review-notification";
                iconName = "fa-solid fa-star";
            }
            else if (type == "WorkSubmitted" || type == "RevisionRequested" || type == "WorkApproved")
            {
                iconClass = "work-notification";
                iconName = "fa-solid fa-folder-open";
            }
            else
            {
                iconClass = "project-notification";
                iconName = "fa-solid fa-circle-info";
            }
        }
    }
}
