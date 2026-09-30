using System;
using System.Web.UI;

namespace CIE_2_PROJECT
{
    public partial class ReviewSubmittedWork : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRequestRevision_Click(object sender, EventArgs e)
        {
            lblActionMessage.Text =
                "Revision request submitted to the freelancer.";
            lblActionMessage.CssClass =
                "action-message revision-message";
        }

        protected void btnApproveWork_Click(object sender, EventArgs e)
        {
            Session["UnityProjectCompleted"] = true;
            Session.Remove("UnityProjectInProgress");
            lblActionMessage.Text =
                "Work approved successfully. Unity 2D Game Development is now marked as completed.";
            lblActionMessage.CssClass =
                "action-message approval-message";
        }

        protected void btnDownloadZip_Click(object sender, EventArgs e)
        {
        }

        protected void btnDownloadPdf_Click(object sender, EventArgs e)
        {
        }
    }
}
