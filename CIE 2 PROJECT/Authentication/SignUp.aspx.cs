using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class SignUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void cvFullName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string fullName = args.Value.Trim();

            args.IsValid =
                fullName.Length >= 3 &&
                Regex.IsMatch(fullName, @"^[a-zA-Z ]+$");
        }

        protected void cvUsername_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string username = args.Value.Trim();

            args.IsValid =
                username.Length >= 3 &&
                Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$");
        }

        protected void cvEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string email = args.Value.Trim();

            args.IsValid =
                Regex.IsMatch(
                    email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
                );
        }

        protected void cvPassword_ServerValidate(object source, ServerValidateEventArgs args)
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

        protected void cvConfirmPassword_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string password = txtPassword.Text;
            string confirmPassword = args.Value;

            args.IsValid = password == confirmPassword;
        }

        protected void cvRole_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid =
                rbFreelancer.Checked ||
                rbClient.Checked;
        }

        protected void cvPhone_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string phone = args.Value.Trim();

            args.IsValid = Regex.IsMatch(
                phone,
                @"^[0-9]{10}$"
            );
        }

        protected void cvTerms_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = chkTerms.Checked;
        }

        protected void btnCreateAccount_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid)
            {
                return;
            }

            string fullName = txtFullName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string phone = txtPhone.Text.Trim();

            string role;

            if (rbFreelancer.Checked)
            {
                role = "Freelancer";
            }
            else
            {
                role = "Client";
            }

            // Database registration logic will be added later.
        }
    }
}