using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void cvEmail_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            string email = args.Value.Trim();

            args.IsValid =
                Regex.IsMatch(
                    email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
                );
        }

        protected void btnReset_Click(
            object sender,
            EventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid)
            {
                return;
            }

            string email = txtEmail.Text.Trim();

            // Password reset logic will be added later.
        }
    }
}