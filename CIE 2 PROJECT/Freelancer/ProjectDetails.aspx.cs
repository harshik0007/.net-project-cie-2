using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class ProjectDetails : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            int projectId;
            if (!int.TryParse(Request.QueryString["id"], out projectId) || projectId <= 0)
            {
                ShowMessage("Invalid project ID.");
                return;
            }

            LoadProject(projectId);
        }

        private void LoadProject(int projectId)
        {
            try
            {
                Project project = ServiceFactory.CreateProjectService().GetProject(projectId);
                if (project == null || project.Status != WorkflowStatuses.ProjectOpen)
                {
                    ShowMessage("This project is unavailable or no longer open for proposals.");
                    return;
                }

                litTitle.Text = Encode(project.Title);
                litDescription.Text = Encode(project.Description);
                litCategory.Text = Encode(project.Category);
                CIE_2_PROJECT.Models.User client = ServiceFactory.CreateUserService().GetUser(project.ClientID);
                lnkClientProfile.Text = Encode(client == null ? "Client" : client.FullName);
                lnkClientProfile.NavigateUrl = "ViewClientProfile.aspx?id=" +
                    project.ClientID.ToString(CultureInfo.InvariantCulture) + "&viewer=freelancer";
                string experience = Encode(project.ExperienceLevel);
                litExperience.Text = experience;
                litExperienceSummary.Text = experience;
                litBudget.Text = Server.HtmlEncode(string.Format(
                    CultureInfo.GetCultureInfo("en-IN"), "₹{0:N0} - ₹{1:N0}",
                    project.MinBudget, project.MaxBudget));
                litDeadline.Text = Server.HtmlEncode(project.Deadline.HasValue
                    ? project.Deadline.Value.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture)
                    : "Not specified");
                litPosted.Text = "Posted on " + project.CreatedAt.ToString(
                    "dd MMM yyyy", CultureInfo.InvariantCulture);
                int proposalCount = ServiceFactory.CreateProposalService()
                    .GetProposalCount(project.ProjectID);
                litProposalCount.Text = proposalCount +
                    (proposalCount == 1 ? " Proposal" : " Proposals");
                litSkills.Text = FormatSkills(project.RequiredSkills);

                string attachment = project.AttachmentPath;
                bool hasAttachment = !string.IsNullOrWhiteSpace(attachment) &&
                    attachment.StartsWith("~/Uploads/ProjectAttachments/",
                        StringComparison.OrdinalIgnoreCase);
                lnkAttachment.Visible = hasAttachment;
                lblNoAttachment.Visible = !hasAttachment;
                if (hasAttachment)
                    lnkAttachment.NavigateUrl = ResolveUrl(attachment);

                lnkSubmitProposal.NavigateUrl = "SubmitProposal.aspx?id=" + projectId;
                pnlProject.Visible = true;
                pnlMessage.Visible = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Freelancer project details failed: " + ex);
                ShowMessage("Unable to load project details. Please try again later.");
            }
        }

        private string Encode(object value)
        {
            return value == null ? string.Empty : HttpUtility.HtmlEncode(Convert.ToString(value));
        }

        private string FormatSkills(string skills)
        {
            if (string.IsNullOrWhiteSpace(skills))
                return "<span class=\"skill-tag\">Not specified</span>";

            StringBuilder result = new StringBuilder();
            foreach (string item in skills.Split(','))
            {
                string skill = item.Trim();
                if (skill.Length > 0)
                {
                    result.Append("<span class=\"skill-tag\">");
                    result.Append(HttpUtility.HtmlEncode(skill));
                    result.Append("</span>");
                }
            }
            return result.Length == 0
                ? "<span class=\"skill-tag\">Not specified</span>"
                : result.ToString();
        }

        private void ShowMessage(string message)
        {
            pnlProject.Visible = false;
            pnlMessage.Visible = true;
            litMessage.Text = HttpUtility.HtmlEncode(message);
        }
    }
}
