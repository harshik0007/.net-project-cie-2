using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class SignUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.IsAuthenticated)
                Response.Redirect(ResolveUrl(Context.User.IsInRole("Client") ? "~/Client/ClientHome.aspx" : "~/Freelancer/FreelancerHome.aspx"), false);

            if (!IsPostBack)
            {
                string requestedRole = Request.QueryString["role"];
                if (string.Equals(requestedRole, "Client", StringComparison.OrdinalIgnoreCase))
                    rbClient.Checked = true;
                else if (string.Equals(requestedRole, "Freelancer", StringComparison.OrdinalIgnoreCase))
                    rbFreelancer.Checked = true;
            }

            if (Page.Form != null)
                Page.Form.Enctype = "multipart/form-data";
            if (!IsPostBack && Request.QueryString["error"] == "duplicate")
                lblSignupMessage.Text = "That username or email is already registered. Try signing in or choose another.";
        }

        protected void cvFullName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string fullName = args.Value.Trim();

            args.IsValid =
                fullName.Length >= 3 &&
                fullName.Length <= 100 &&
                Regex.IsMatch(fullName, @"^[a-zA-Z ]+$");
        }

        protected void cvUsername_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string username = args.Value.Trim();

            args.IsValid =
                username.Length >= 3 &&
                username.Length <= 50 &&
                Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$");
        }

        protected void cvEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string email = args.Value.Trim();

            args.IsValid =
                email.Length <= 100 &&
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
            lblSignupMessage.Text = string.Empty;

            if (!Page.IsValid)
            {
                return;
            }

            string fullName = txtFullName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string role = rbFreelancer.Checked ? "Freelancer" : "Client";
            string profileImage = null;
            try
            {
                IUserService users = ServiceFactory.CreateUserService();
                if (users.UsernameOrEmailExists(username, email))
                {
                    lblSignupMessage.Text = "That username or email is already registered. Try signing in or choose another.";
                    return;
                }

                if (fuProfilePhoto.HasFile)
                    profileImage = ProfileImageStorage.Save(fuProfilePhoto.PostedFile, Server.MapPath("~/"));

                PendingFreelancerRegistration previous = Session[PendingRegistrationKey] as PendingFreelancerRegistration;
                if (previous != null && !string.IsNullOrWhiteSpace(previous.ProfileImage))
                    ProfileImageStorage.Delete(previous.ProfileImage, Server.MapPath("~/"));
                Session.Remove(PendingRegistrationKey);

                string passwordHash = PasswordHasher.Hash(txtPassword.Text);
                if (role == "Freelancer")
                {
                    Session[PendingRegistrationKey] = new PendingFreelancerRegistration
                    {
                        FullName = fullName,
                        Username = username,
                        Email = email,
                        PhoneNumber = phone,
                        PasswordHash = passwordHash,
                        ProfileImage = profileImage
                    };
                    Response.Redirect("~/Authentication/FreelancerSkills.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                int userId = users.CreateAccount(fullName, username, email, passwordHash,
                    role, phone, profileImage, null);
                AuthenticatedUser.SignIn(userId, role);
                FlashMessageStore.Success("Your client account was created successfully.");
                Response.Redirect("~/Client/ClientHome.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (SqlException ex)
            {
                if (!string.IsNullOrWhiteSpace(profileImage))
                    ProfileImageStorage.Delete(profileImage, Server.MapPath("~/"));
                System.Diagnostics.Trace.TraceError("Account registration SQL error: " + ex);
                lblSignupMessage.Text = ex.Number == 2601 || ex.Number == 2627
                    ? "That username or email is already registered. Try signing in or choose another."
                    : "We could not create your account. Check the database connection and try again.";
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(profileImage))
                    ProfileImageStorage.Delete(profileImage, Server.MapPath("~/"));
                System.Diagnostics.Trace.TraceError("Account registration failed: " + ex);
                lblSignupMessage.Text = ex is ArgumentException || ex is InvalidOperationException
                    ? ex.Message
                    : "We could not create your account. Please check the form and try again.";
            }
        }

        private const string PendingRegistrationKey = "PendingFreelancerRegistration";
    }
}
