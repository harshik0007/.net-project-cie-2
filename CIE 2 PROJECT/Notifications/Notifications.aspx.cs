using System;

namespace CIE_2_PROJECT
{
    public partial class NotificationsPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnAll_Click(object sender, EventArgs e)
        {
            pnlAllNotifications.Visible = true;
            pnlUnreadNotifications.Visible = false;

            btnAll.CssClass = "notification-tab active-tab";
            btnUnread.CssClass = "notification-tab";
        }

        protected void btnUnread_Click(object sender, EventArgs e)
        {
            pnlAllNotifications.Visible = false;
            pnlUnreadNotifications.Visible = true;

            btnAll.CssClass = "notification-tab";
            btnUnread.CssClass = "notification-tab active-tab";
        }

        protected void btnMarkAllRead_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "All notifications marked as read.";
            lblMessage.CssClass = "page-message success-message";
        }
    }
}