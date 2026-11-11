
using System;
using System.Globalization;
using System.IO;
using System.Web.UI;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class ClientProjectDetails : Page
    {
        private string currentStatus = "";
        private string currentAttachmentPath = null;

        private int GetProjectId()
        {
            int projectId;

            if (int.TryParse(Request.QueryString["id"], out projectId) &&
                projectId > 0)
            {
                return projectId;
            }

            return 0;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int projectId = GetProjectId();

            if (projectId == 0)
            {
                ShowPageMessage("A valid project ID was not provided.");
                return;
            }

            LoadProject(projectId);
        }

        private void ShowPageMessage(string message)
        {
            pnlProjectDetails.Visible = false;
            lblPageMessage.Text = Server.HtmlEncode(message);
            lblPageMessage.Visible = true;
        }

        private string GetDisplayValue(object value, string fallback)
        {
            string displayValue = value == null ? null : Convert.ToString(value);
            return Server.HtmlEncode(string.IsNullOrWhiteSpace(displayValue) ? fallback : displayValue);
        }

        private void LoadProject(int projectId)
        {
            try
            {
                Project project = ServiceFactory.CreateProjectService()
                    .GetClientProject(TestUserContext.ClientID, projectId);
                int proposalCount = ServiceFactory.CreateProposalService()
                    .GetProjectProposals(TestUserContext.ClientID, projectId, "date").Count;

                lblProjectTitle.Text = GetDisplayValue(project.Title, "Untitled Project");
                lblCategory.Text = GetDisplayValue(project.Category, "Uncategorized");
                lblCreatedAt.Text = "Posted " + project.CreatedAt.ToString(
                    "dd MMM, yyyy", CultureInfo.InvariantCulture);
                lblDescription.Text = GetDisplayValue(project.Description, "No description provided.");
                lblSkills.Text = GetDisplayValue(project.RequiredSkills, "No specific skills listed.");
                lblExperience.Text = GetDisplayValue(project.ExperienceLevel, "Not specified.");
                lblExperienceSummary.Text = lblExperience.Text;
                lblLocation.Text = GetDisplayValue(project.PreferredLocation, "Not specified.");

                string deadlineText = project.Deadline.HasValue
                    ? project.Deadline.Value.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture)
                    : "Not specified";
                lblDeadline.Text = Server.HtmlEncode(deadlineText);
                lblDeadlineSummary.Text = Server.HtmlEncode(deadlineText);

                string budgetText = string.Format(
                    CultureInfo.GetCultureInfo("en-IN"), "₹{0:N0} - ₹{1:N0}",
                    project.MinBudget, project.MaxBudget);
                lblBudget.Text = Server.HtmlEncode(budgetText);
                lblBudgetSummary.Text = Server.HtmlEncode(budgetText);

                string proposalCountText = proposalCount +
                    (proposalCount == 1 ? " Proposal" : " Proposals");
                lblProposalCountTop.Text = Server.HtmlEncode(proposalCountText);
                lblProposalCountCard.Text = Server.HtmlEncode(proposalCountText);
                lblProposalCountSummary.Text = Server.HtmlEncode(proposalCount.ToString());
                lblProposalMessage.Text = proposalCount == 0
                    ? "No proposals have been submitted for this project yet."
                    : "This project has received " + proposalCount + " proposal(s).";

                currentStatus = project.Status;
                lblStatusTop.Text = Server.HtmlEncode(currentStatus);
                lblStatusSummary.Text = Server.HtmlEncode(currentStatus);
                currentAttachmentPath = project.AttachmentPath;

                bool hasAttachment = !string.IsNullOrWhiteSpace(currentAttachmentPath);
                pnlAttachment.Visible = hasAttachment;
                pnlNoAttachment.Visible = !hasAttachment;
                if (hasAttachment)
                {
                    string extension = Path.GetExtension(currentAttachmentPath);
                    lblAttachmentName.Text = Server.HtmlEncode(
                        "Project attachment " + (string.IsNullOrEmpty(extension)
                            ? "" : "(" + extension.ToUpperInvariant() + ")"));
                }

                bool isOpen = string.Equals(currentStatus, WorkflowStatuses.ProjectOpen,
                    StringComparison.OrdinalIgnoreCase);
                bool isInProgress = string.Equals(currentStatus, WorkflowStatuses.ProjectInProgress,
                    StringComparison.OrdinalIgnoreCase);
                bool isCompleted = string.Equals(currentStatus, WorkflowStatuses.ProjectCompleted,
                    StringComparison.OrdinalIgnoreCase);
                bool isCancelled = string.Equals(currentStatus, WorkflowStatuses.ProjectCancelled,
                    StringComparison.OrdinalIgnoreCase);
                string actionText = isInProgress ? "Review Submitted Work"
                    : isCompleted ? "View Completed Work"
                    : isCancelled ? "Project Cancelled"
                    : isOpen ? "View Proposals" : "Unavailable";
                btnViewProposals.Text = actionText;
                btnProposals.Text = actionText;
                btnViewProposals.Enabled = isOpen || isInProgress || isCompleted;
                btnProposals.Enabled = isOpen || isInProgress || isCompleted;
                btnEditProject.Visible = isOpen;
                lblManageMessage.Text = Server.HtmlEncode(
                    isInProgress ? "Review the active contract, submitted work, or revision feedback."
                    : isCompleted ? "View the completed work and manage your review."
                    : isCancelled ? "This project was cancelled. Project actions are closed."
                    : isOpen ? "Review proposals or update your project information."
                    : "No actions are available for this project status.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Client project details failed: " + ex);
                ShowPageMessage("Unable to load this project. Please check the database connection and try again.");
            }
        }

        protected void btnDownloadFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(currentAttachmentPath) ||
                !currentAttachmentPath.StartsWith(
                    "~/Uploads/ProjectAttachments/",
                    StringComparison.OrdinalIgnoreCase))
            {
                lblActionMessage.Text =
                    "No valid attachment is available for this project.";

                lblActionMessage.CssClass =
                    "action-message info-message";
                return;
            }

            try
            {
                string uploadFolder = Path.GetFullPath(
                    Server.MapPath("~/Uploads/ProjectAttachments/"));

                string fullPath = Path.GetFullPath(
                    Server.MapPath(currentAttachmentPath));

                string folderPrefix = uploadFolder.TrimEnd(
                    Path.DirectorySeparatorChar,
                    Path.AltDirectorySeparatorChar)
                    + Path.DirectorySeparatorChar;

                if (!fullPath.StartsWith(
                    folderPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    lblActionMessage.Text =
                        "The attachment path is invalid.";

                    lblActionMessage.CssClass =
                        "action-message info-message";
                    return;
                }

                if (!File.Exists(fullPath))
                {
                    lblActionMessage.Text =
                        "The attachment file could not be found on the server.";

                    lblActionMessage.CssClass =
                        "action-message info-message";
                    return;
                }

                string extension =
                    Path.GetExtension(fullPath).ToLowerInvariant();

                string contentType = "application/octet-stream";

                switch (extension)
                {
                    case ".pdf":
                        contentType = "application/pdf";
                        break;

                    case ".doc":
                        contentType = "application/msword";
                        break;

                    case ".docx":
                        contentType =
                            "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;

                    case ".zip":
                        contentType = "application/zip";
                        break;

                    case ".png":
                        contentType = "image/png";
                        break;

                    case ".jpg":
                    case ".jpeg":
                        contentType = "image/jpeg";
                        break;
                }

                string fileName = Path.GetFileName(fullPath)
                    .Replace("\"", "");

                Response.Clear();
                Response.ContentType = contentType;
                Response.AddHeader(
                    "Content-Disposition",
                    "attachment; filename=\"" + fileName + "\"");

                Response.TransmitFile(fullPath);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                lblActionMessage.Text =
                    "Unable to download the attachment.";

                lblActionMessage.CssClass =
                    "action-message info-message";
            }
        }

        protected void btnViewProposals_Click(object sender, EventArgs e)
        {
            int projectId = GetProjectId();

            if (projectId == 0)
            {
                return;
            }

            if (string.Equals(currentStatus, WorkflowStatuses.ProjectCancelled,
                StringComparison.OrdinalIgnoreCase))
            {
                lblActionMessage.Text =
                    "This project was cancelled. No workflow actions are available.";

                lblActionMessage.CssClass =
                    "action-message info-message";
                return;
            }

            if (string.Equals(currentStatus, WorkflowStatuses.ProjectInProgress,
                StringComparison.OrdinalIgnoreCase) ||
                string.Equals(currentStatus, WorkflowStatuses.ProjectCompleted,
                    StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect(
                    "ReviewSubmittedWork.aspx?projectId=" + projectId, false);
            }
            else if (string.Equals(currentStatus, WorkflowStatuses.ProjectOpen,
                StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect(
                    "ProjectProposal.aspx?projectId=" + projectId, false);
            }
            else
            {
                lblActionMessage.Text = "No actions are available for this project status.";
                lblActionMessage.CssClass = "action-message info-message";
                return;
            }

            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnEditProject_Click(object sender, EventArgs e)
        {
            int projectId = GetProjectId();

            if (projectId <= 0 || !string.Equals(currentStatus,
                WorkflowStatuses.ProjectOpen, StringComparison.OrdinalIgnoreCase))
            {
                lblActionMessage.Text = "Only open projects can be edited.";
                lblActionMessage.CssClass = "action-message info-message";
                return;
            }

            Response.Redirect(
                "CreateProject.aspx?projectId=" + projectId, false);

            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
