
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web.UI;

namespace CIE_2_PROJECT
{
    public partial class ClientProjectDetails : Page
    {
        // Temporary test client until authentication is implemented.
        private const int TestClientId = 1;

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

        private string GetDisplayValue(
            SqlDataReader reader,
            string columnName,
            string fallback)
        {
            int index = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(index))
            {
                return Server.HtmlEncode(fallback);
            }

            string value = Convert.ToString(reader.GetValue(index));

            if (string.IsNullOrWhiteSpace(value))
            {
                value = fallback;
            }

            return Server.HtmlEncode(value);
        }

        private void LoadProject(int projectId)
        {
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings[
                    "SkillLinkDBConnection"];

            if (settings == null ||
                string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                ShowPageMessage(
                    "The database connection is not configured.");
                return;
            }

            string sql = @"
                SELECT
                    p.ProjectID,
                    p.Title,
                    p.Description,
                    p.Category,
                    p.CreatedAt,
                    p.RequiredSkills,
                    p.ExperienceLevel,
                    p.PreferredLocation,
                    p.Deadline,
                    p.MinBudget,
                    p.MaxBudget,
                    p.Status,
                    p.AttachmentPath,
                    (
                        SELECT COUNT(*)
                        FROM dbo.Proposals AS pr
                        WHERE pr.ProjectID = p.ProjectID
                    ) AS ProposalCount
                FROM dbo.Projects AS p
                WHERE p.ProjectID = @ProjectID
                  AND p.ClientID = @ClientID;
            ";

            try
            {
                using (SqlConnection con =
                    new SqlConnection(settings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add(
                        "@ProjectID", SqlDbType.Int).Value = projectId;

                    cmd.Parameters.Add(
                        "@ClientID", SqlDbType.Int).Value = TestClientId;

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            ShowPageMessage(
                                "Project not found or it does not belong " +
                                "to this test client.");
                            return;
                        }

                        lblProjectTitle.Text =
                            GetDisplayValue(reader, "Title", "Untitled Project");

                        lblCategory.Text =
                            GetDisplayValue(reader, "Category", "Uncategorized");

                        DateTime createdAt =
                            Convert.ToDateTime(reader["CreatedAt"]);

                        lblCreatedAt.Text =
                            "Posted " + createdAt.ToString(
                                "dd MMM, yyyy",
                                CultureInfo.InvariantCulture);

                        lblDescription.Text =
                            GetDisplayValue(
                                reader, "Description", "No description provided.");

                        lblSkills.Text =
                            GetDisplayValue(
                                reader, "RequiredSkills", "No specific skills listed.");

                        lblExperience.Text =
                            GetDisplayValue(
                                reader, "ExperienceLevel", "Not specified.");

                        lblExperienceSummary.Text =
                            lblExperience.Text;

                        lblLocation.Text =
                            GetDisplayValue(
                                reader, "PreferredLocation", "Not specified.");

                        DateTime deadline =
                            Convert.ToDateTime(reader["Deadline"]);

                        string deadlineText = deadline.ToString(
                            "dd MMM, yyyy",
                            CultureInfo.InvariantCulture);

                        lblDeadline.Text = Server.HtmlEncode(deadlineText);
                        lblDeadlineSummary.Text = Server.HtmlEncode(deadlineText);

                        decimal minBudget =
                            Convert.ToDecimal(reader["MinBudget"]);

                        decimal maxBudget =
                            Convert.ToDecimal(reader["MaxBudget"]);

                        string budgetText = string.Format(
                            CultureInfo.GetCultureInfo("en-IN"),
                            "₹{0:N0} - ₹{1:N0}",
                            minBudget,
                            maxBudget);

                        lblBudget.Text = Server.HtmlEncode(budgetText);
                        lblBudgetSummary.Text = Server.HtmlEncode(budgetText);

                        int proposalCount =
                            Convert.ToInt32(reader["ProposalCount"]);

                        string proposalCountText =
                            proposalCount + (
                                proposalCount == 1 ? " Proposal" : " Proposals");

                        lblProposalCountTop.Text =
                            Server.HtmlEncode(proposalCountText);

                        lblProposalCountCard.Text =
                            Server.HtmlEncode(proposalCountText);

                        lblProposalCountSummary.Text =
                            Server.HtmlEncode(proposalCount.ToString());

                        lblProposalMessage.Text = proposalCount == 0
                            ? "No proposals have been submitted for this project yet."
                            : "This project has received " +
                              proposalCount + " proposal(s).";

                        currentStatus = Convert.ToString(reader["Status"]);

                        lblStatusTop.Text =
                            Server.HtmlEncode(currentStatus);

                        lblStatusSummary.Text =
                            Server.HtmlEncode(currentStatus);

                        currentAttachmentPath =
                            reader["AttachmentPath"] == DBNull.Value
                                ? null
                                : Convert.ToString(reader["AttachmentPath"]);

                        bool hasAttachment =
                            !string.IsNullOrWhiteSpace(currentAttachmentPath);

                        pnlAttachment.Visible = hasAttachment;
                        pnlNoAttachment.Visible = !hasAttachment;

                        if (hasAttachment)
                        {
                            string extension = Path.GetExtension(
                                currentAttachmentPath);

                            lblAttachmentName.Text = Server.HtmlEncode(
                                "Project attachment " +
                                (string.IsNullOrEmpty(extension)
                                    ? ""
                                    : "(" + extension.ToUpperInvariant() + ")"));
                        }

                        bool isInProgress = string.Equals(
                            currentStatus, "In Progress",
                            StringComparison.OrdinalIgnoreCase);

                        bool isCompleted = string.Equals(
                            currentStatus, "Completed",
                            StringComparison.OrdinalIgnoreCase);

                        string actionText = isInProgress
                            ? "Review Submitted Work"
                            : "View Proposals";

                        btnViewProposals.Text = actionText;
                        btnProposals.Text = actionText;

                        btnViewProposals.Enabled = !isCompleted;
                        btnProposals.Enabled = !isCompleted;

                        lblManageMessage.Text = Server.HtmlEncode(
                            isInProgress
                                ? "Review the freelancer's submitted work."
                                : isCompleted
                                    ? "This project has been completed."
                                    : "Review proposals or update your project information.");
                    }
                }
            }
            catch (Exception)
            {
                ShowPageMessage(
                    "Unable to load this project. Please check the database " +
                    "connection and try again.");
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

            if (string.Equals(
                currentStatus, "Completed",
                StringComparison.OrdinalIgnoreCase))
            {
                lblActionMessage.Text =
                    "This project has already been completed.";

                lblActionMessage.CssClass =
                    "action-message info-message";
                return;
            }

            if (string.Equals(
                currentStatus, "In Progress",
                StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect(
                    "ReviewSubmittedWork.aspx?projectId=" + projectId, false);
            }
            else
            {
                Response.Redirect(
                    "ProjectProposal.aspx?projectId=" + projectId, false);
            }

            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnEditProject_Click(object sender, EventArgs e)
        {
            int projectId = GetProjectId();

            if (projectId <= 0)
            {
                return;
            }

            Response.Redirect(
                "CreateProject.aspx?projectId=" + projectId, false);

            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
