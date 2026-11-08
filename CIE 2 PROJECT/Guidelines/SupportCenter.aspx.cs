using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class SupportCenter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Keep ticket submission unavailable on both the initial render and postbacks.
            txtName.Enabled = false;
            txtEmail.Enabled = false;
            ddlCategory.Enabled = false;
            ddlPriority.Enabled = false;
            txtSubject.Enabled = false;
            txtMessage.Enabled = false;
            btnClear.Enabled = false;
            btnSubmit.Enabled = false;

            rfvName.Enabled = false;
            cvName.Enabled = false;
            rfvEmail.Enabled = false;
            cvEmail.Enabled = false;
            cvCategory.Enabled = false;
            cvPriority.Enabled = false;
            rfvSubject.Enabled = false;
            cvSubject.Enabled = false;
            rfvMessage.Enabled = false;
            cvMessage.Enabled = false;
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

    }
}
