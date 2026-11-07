using System;
using System.Web.UI;

namespace CIE_2_PROJECT
{
    public partial class ResetPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(ResolveUrl("~/Authentication/ForgotPassword.aspx"), false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
