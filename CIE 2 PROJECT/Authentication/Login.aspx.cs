using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.IsAuthenticated)
                Response.Redirect(ResolveUrl(Context.User.IsInRole("Client") ? "~/Client/ClientHome.aspx" : "~/Freelancer/FreelancerHome.aspx"), false);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Page.Validate();
            lblLoginMessage.Text = string.Empty;
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                IUserService users = ServiceFactory.CreateUserService();
                LoginAccount account = users.GetLoginAccount(txtUsername.Text.Trim());
                if (account == null || !PasswordHasher.Verify(txtPassword.Text, account.PasswordHash))
                {
                    lblLoginMessage.Text = "Email/username or password is incorrect.";
                    return;
                }

                AuthenticatedUser.SignIn(account.UserID, account.Role);
                FlashMessageStore.Success("Welcome back. You are signed in.");
                string destination = account.Role == "Client"
                    ? "~/Client/ClientHome.aspx"
                    : "~/Freelancer/FreelancerHome.aspx";
                Response.Redirect(destination, false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Login failed: " + ex);
                lblLoginMessage.Text = ex is InvalidOperationException
                    ? ex.Message
                    : "We could not sign you in right now. Check the database connection and try again.";
            }
        }

        protected void cvPassword_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            string password = args.Value;

            if (string.IsNullOrWhiteSpace(password))
            {
                args.IsValid = false;
                return;
            }

            if (password.Length < 6)
            {
                args.IsValid = false;
                return;
            }

            args.IsValid = true;
        }
    }
}
