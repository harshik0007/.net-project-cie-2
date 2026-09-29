using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class SupportCenter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void cvName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string name = args.Value.Trim();

            args.IsValid =
                name.Length >= 3 &&
                Regex.IsMatch(name, @"^[a-zA-Z ]+$");
        }

        protected void cvEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string email = args.Value.Trim();

            args.IsValid = Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
            );
        }

        protected void cvCategory_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrWhiteSpace(args.Value);
        }

        protected void cvPriority_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrWhiteSpace(args.Value);
        }

        protected void cvSubject_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string subject = args.Value.Trim();

            args.IsValid =
                subject.Length >= 5 &&
                subject.Length <= 100;
        }

        protected void cvMessage_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string message = args.Value.Trim();

            args.IsValid =
                message.Length >= 20 &&
                message.Length <= 1000;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid)
            {
                return;
            }

            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string category = ddlCategory.SelectedValue;
            string priority = ddlPriority.SelectedValue;
            string subject = txtSubject.Text.Trim();
            string message = txtMessage.Text.Trim();

            // ADO.NET database submission will be added later.

            lblSuccess.Text = "Your support request has been submitted successfully.";
            lblSuccess.Visible = true;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtEmail.Text = "";
            ddlCategory.SelectedIndex = 0;
            ddlPriority.SelectedIndex = 0;
            txtSubject.Text = "";
            txtMessage.Text = "";
            lblSuccess.Visible = false;
        }
    }
}