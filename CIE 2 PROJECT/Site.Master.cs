using System;

namespace CIE_2_PROJECT
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string path = Request.AppRelativeCurrentExecutionFilePath;

            bool isClientPage = path.StartsWith(
                "~/Client/",
                StringComparison.OrdinalIgnoreCase
            );

            freelancerNav.Visible = !isClientPage;
            clientNav.Visible = isClientPage;

            freelancerProfile.Visible = !isClientPage;
            clientProfile.Visible = isClientPage;

            if (isClientPage)
            {
                brandLink.HRef = ResolveUrl("~/Client/ClientHome.aspx");
            }
            else
            {
                brandLink.HRef = ResolveUrl("~/Freelancer/FreelancerHome.aspx");
            }
        }
    }
}