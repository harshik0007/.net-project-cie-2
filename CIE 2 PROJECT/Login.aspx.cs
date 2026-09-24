using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (username.Length < 3)
            {
                return;
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