using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class SecurePayment : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void cvUpi_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string upi = args.Value.Trim();

            args.IsValid = Regex.IsMatch(
                upi,
                @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+$"
            );
        }

        protected void btnMakePayment_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid)
            {
                return;
            }

            Session["UnityProjectInProgress"] = true;
            Response.Redirect("MyProjects.aspx");
        }
    }
}
