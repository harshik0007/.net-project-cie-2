using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class HireFreelancer : Page
    {
        private int ProjectId
        {
            get
            {
                int id;
                string value = Request.QueryString["projectId"];
                if (string.IsNullOrWhiteSpace(value))
                    value = Request.QueryString["id"];
                return int.TryParse(value, out id) && id > 0 ? id : 0;
            }
        }

        private int ProposalId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["proposalId"], out id) && id > 0
                    ? id : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int projectId = ProjectId;
            lnkBack.NavigateUrl = projectId > 0
                ? "ProjectProposal.aspx?projectId=" + projectId
                : "MyProjects.aspx";
            lnkCancel.NavigateUrl = lnkBack.NavigateUrl;

            if (!IsPostBack)
                LoadProposal();
        }

        protected void btnConfirmHire_Click(object sender, EventArgs e)
        {
            int projectId = ProjectId;
            int proposalId = ProposalId;
            if (projectId <= 0 || proposalId <= 0)
            {
                ShowMessage("A valid project and proposal are required.");
                pnlConfirmation.Visible = false;
                btnConfirmHire.Enabled = false;
                return;
            }

            try
            {
                FlashMessageStore.Success("Review the funding summary and simulate payment to confirm this hire.");
                Response.Redirect("SecurePayment.aspx?projectId=" + projectId + "&proposalId=" + proposalId, false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Proposal confirmation failed: " + ex);
                ShowMessage("Unable to hire this freelancer. The project or proposal may have changed.");
                LoadProposal();
            }
        }

        private void LoadProposal()
        {
            int projectId = ProjectId;
            int proposalId = ProposalId;
            if (projectId <= 0 || proposalId <= 0)
            {
                ShowMessage("A valid project and proposal are required to continue.");
                pnlConfirmation.Visible = false;
                btnConfirmHire.Enabled = false;
                return;
            }

            try
            {
                Project project = ServiceFactory.CreateProjectService().GetClientProject(
                    TestUserContext.ClientID, projectId);
                IList<Proposal> proposals = ServiceFactory.CreateProposalService()
                    .GetProjectProposals(TestUserContext.ClientID, projectId, "date");
                Proposal selectedProposal = null;
                foreach (Proposal proposal in proposals)
                {
                    if (proposal.ProposalID == proposalId)
                    {
                        selectedProposal = proposal;
                        break;
                    }
                }

                if (selectedProposal == null)
                    throw new InvalidOperationException("The proposal does not belong to this project.");

                User freelancer = ServiceFactory.CreateUserService()
                    .GetUser(selectedProposal.FreelancerID);
                lblProjectTitle.Text = Encode(project.Title);
                lblProjectDescription.Text = Encode(project.Description);
                lblProjectSkills.Text = FormatSkills(project.RequiredSkills);
                lblProjectBudget.Text = Server.HtmlEncode(string.Format(
                    CultureInfo.GetCultureInfo("en-IN"), "₹{0:N0} - ₹{1:N0}",
                    project.MinBudget, project.MaxBudget));
                lblProjectDeadline.Text = Server.HtmlEncode(project.Deadline.HasValue
                    ? project.Deadline.Value.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)
                    : "Not specified");
                lnkFreelancerProfile.Text = Encode(freelancer == null
                    ? "Freelancer #" + selectedProposal.FreelancerID
                    : freelancer.FullName);
                lnkFreelancerProfile.NavigateUrl = "ViewFreelancerProfile.aspx?id=" +
                    selectedProposal.FreelancerID.ToString(CultureInfo.InvariantCulture) + "&viewer=client";
                lblProposedPrice.Text = Server.HtmlEncode(selectedProposal.BidAmount.ToString(
                    "C0", CultureInfo.GetCultureInfo("en-IN")));
                lblDeliveryTime.Text = Server.HtmlEncode(selectedProposal.EstimatedDays + " days");
                lblProposalStatus.Text = Encode(selectedProposal.Status);
                lblCoverLetter.Text = Encode(selectedProposal.CoverLetter);
                BindProposalAttachment(selectedProposal.AttachmentPath);

                pnlConfirmation.Visible = true;
                bool canHire = project.Status == WorkflowStatuses.ProjectOpen &&
                    selectedProposal.Status == WorkflowStatuses.ProposalPending;
                btnConfirmHire.Enabled = canHire;
                if (canHire && ServiceFactory.CreateContractService()
                    .GetActiveForFreelancer(selectedProposal.FreelancerID).Count >= 3)
                {
                    btnConfirmHire.Enabled = false;
                    ShowMessage("This freelancer already has 3 active projects. Choose another proposal or wait until one of their projects is complete.");
                }
                else if (!canHire)
                    ShowMessage("This proposal is no longer available for hiring.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Hire confirmation page failed to load: " + ex);
                pnlConfirmation.Visible = false;
                btnConfirmHire.Enabled = false;
                ShowMessage("This project or proposal could not be loaded. Return to the project proposals and try again.");
            }
        }

        private string Encode(object value)
        {
            return value == null ? string.Empty : Server.HtmlEncode(Convert.ToString(value));
        }

        private void BindProposalAttachment(string attachmentPath)
        {
            lnkProposalAttachment.Visible = false;
            if (string.IsNullOrWhiteSpace(attachmentPath))
            {
                lblProposalAttachmentDetails.Text = "No attachment was included with this proposal.";
                return;
            }

            const string virtualFolder = "~/Uploads/ProposalAttachments/";
            string normalizedPath = attachmentPath.Trim().Replace('\\', '/');
            if (!normalizedPath.StartsWith(virtualFolder, StringComparison.OrdinalIgnoreCase))
            {
                lblProposalAttachmentDetails.Text = "This attachment is unavailable.";
                return;
            }

            string storedFileName = normalizedPath.Substring(virtualFolder.Length);
            if (string.IsNullOrWhiteSpace(storedFileName) ||
                !string.Equals(storedFileName, Path.GetFileName(storedFileName),
                    StringComparison.Ordinal) || storedFileName.IndexOf("..", StringComparison.Ordinal) >= 0)
            {
                lblProposalAttachmentDetails.Text = "This attachment is unavailable.";
                return;
            }

            try
            {
                string physicalFolder = Path.GetFullPath(Server.MapPath(virtualFolder));
                string physicalPath = Path.GetFullPath(Path.Combine(physicalFolder, storedFileName));
                string folderPrefix = physicalFolder.TrimEnd(Path.DirectorySeparatorChar,
                    Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
                if (!physicalPath.StartsWith(folderPrefix, StringComparison.OrdinalIgnoreCase) ||
                    !File.Exists(physicalPath))
                {
                    lblProposalAttachmentDetails.Text = "This attachment is no longer available.";
                    return;
                }

                string extension = Path.GetExtension(storedFileName).TrimStart('.').ToUpperInvariant();
                lblProposalAttachmentDetails.Text = string.IsNullOrWhiteSpace(extension)
                    ? "File attached to this proposal."
                    : extension + " file attached to this proposal.";
                lnkProposalAttachment.NavigateUrl = ResolveUrl(virtualFolder + storedFileName);
                lnkProposalAttachment.Visible = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Proposal attachment could not be displayed: " + ex);
                lblProposalAttachmentDetails.Text = "This attachment is unavailable.";
            }
        }

        private string FormatSkills(string skills)
        {
            if (string.IsNullOrWhiteSpace(skills))
                return "<span>No skills specified</span>";

            string[] values = skills.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string result = string.Empty;
            foreach (string skill in values)
                result += "<span>" + Server.HtmlEncode(skill.Trim()) + "</span>";
            return result;
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = Server.HtmlEncode(message);
            lblMessage.Visible = true;
        }
    }
}
