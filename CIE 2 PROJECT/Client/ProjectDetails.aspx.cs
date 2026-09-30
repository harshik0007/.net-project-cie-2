using System;

namespace CIE_2_PROJECT
{
    public partial class ClientProjectDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnViewProposals_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProjectProposal.aspx?projectId=1");
        }

        protected void btnEditProject_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateProject.aspx?projectId=1");
        }

        protected void btnDownloadFile_Click(object sender, EventArgs e)
        {
            lblActionMessage.Text =
                "Project file download will be connected when file storage is implemented.";

            lblActionMessage.CssClass =
                "action-message info-message";
        }
    }
}