using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void cvNewPassword_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            string password = args.Value;

            bool hasMinimumLength = password.Length >= 8;
            bool hasLetter = Regex.IsMatch(password, @"[A-Za-z]");
            bool hasNumber = Regex.IsMatch(password, @"[0-9]");
            bool hasSymbol = Regex.IsMatch(password, @"[^A-Za-z0-9]");

            args.IsValid =
                hasMinimumLength &&
                hasLetter &&
                hasNumber &&
                hasSymbol;
        }

        protected void cvConfirmPassword_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            string password = txtNewPassword.Text;
            string confirmPassword = args.Value;

            args.IsValid =
                password == confirmPassword;
        }

        protected void btnResetPassword_Click(
            object sender,
            EventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid)
            {
                return;
            }

            string newPassword = txtNewPassword.Text;

            // Password update logic will be added later.
        }
    }
}