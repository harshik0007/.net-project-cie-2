using System;
using System.Web.UI;

namespace CIE_2_PROJECT
{
    public partial class HireFreelancer : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnConfirmHire_Click(object sender, EventArgs e)
        {
            Response.Redirect("SecurePayment.aspx?projectId=1");
        }
    }
}
