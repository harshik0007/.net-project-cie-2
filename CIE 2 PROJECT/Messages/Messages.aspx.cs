using System;

namespace CIE_2_PROJECT
{
    public partial class MessagesPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRahul_Click(object sender, EventArgs e)
        {
            SetActiveConversation(btnRahul);
        }

        protected void btnPriya_Click(object sender, EventArgs e)
        {
            SetActiveConversation(btnPriya);
        }

        protected void btnNeha_Click(object sender, EventArgs e)
        {
            SetActiveConversation(btnNeha);
        }

        private void SetActiveConversation(
            System.Web.UI.WebControls.LinkButton selectedButton)
        {
            btnRahul.CssClass = "conversation-item";
            btnPriya.CssClass = "conversation-item";
            btnNeha.CssClass = "conversation-item";

            selectedButton.CssClass =
                "conversation-item active-conversation";
        }

        protected void cvMessage_ServerValidate(
            object source,
            System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            string message = args.Value.Trim();

            args.IsValid =
                message.Length >= 1 &&
                message.Length <= 1000;
        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid)
            {
                return;
            }

            lblMessage.Text = "Message sent successfully.";
            lblMessage.CssClass =
                "action-message success-message";

            txtMessage.Text = "";
        }
    }
}