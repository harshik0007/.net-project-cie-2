using System;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class SubmitProposal : Page
    {
        private const int MaxFileSize = 5 * 1024 * 1024;
        private static readonly string[] AllowedExtensions =
        {
            ".pdf", ".doc", ".docx", ".png", ".jpg", ".jpeg"
        };

        private int ProjectId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id) && id > 0 ? id : 0;
            }
        }

        private decimal MinBudget
        {
            get { return ViewState["MinBudget"] == null ? 0m : (decimal)ViewState["MinBudget"]; }
            set { ViewState["MinBudget"] = value; }
        }

        private decimal MaxBudget
        {
            get { return ViewState["MaxBudget"] == null ? 0m : (decimal)ViewState["MaxBudget"]; }
            set { ViewState["MaxBudget"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Form != null)
                Page.Form.Enctype = "multipart/form-data";

            string detailsUrl = "ProjectDetails.aspx?id=" + ProjectId;
            backToDetails.NavigateUrl = detailsUrl;
            lnkViewDetails.NavigateUrl = detailsUrl;
            lnkCancel.NavigateUrl = "FindProjects.aspx";

            if (!IsPostBack)
                LoadProject();
        }

        private void LoadProject()
        {
            if (ProjectId <= 0)
            {
                ShowUnavailable("Invalid project ID.");
                return;
            }

            try
            {
                Project project = ServiceFactory.CreateProjectService().GetProject(ProjectId);
                if (project == null)
                {
                    ShowUnavailable("This project could not be found.");
                    return;
                }
                if (project.Status != WorkflowStatuses.ProjectOpen)
                {
                    ShowUnavailable("This project is no longer accepting proposals.");
                    return;
                }

                MinBudget = project.MinBudget;
                MaxBudget = project.MaxBudget;
                litProjectTitle.Text = Server.HtmlEncode(project.Title);
                litProjectDescription.Text = Server.HtmlEncode(project.Description);
                litProjectCategory.Text = Server.HtmlEncode(project.Category);
                litProjectPosted.Text = Server.HtmlEncode("Posted " + project.CreatedAt.ToString(
                    "dd MMM, yyyy", CultureInfo.InvariantCulture));
                litProjectBudget.Text = Server.HtmlEncode(string.Format(
                    CultureInfo.GetCultureInfo("en-IN"), "₹{0:N0} - ₹{1:N0}",
                    MinBudget, MaxBudget));

                pnlUnavailable.Visible = false;
                pnlProposalForm.Visible = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Proposal project load failed: " + ex);
                ShowUnavailable("Unable to load project details. Please check the database connection.");
            }
        }

        private void ShowUnavailable(string message)
        {
            pnlUnavailable.Visible = true;
            pnlProposalForm.Visible = false;
            litUnavailableMessage.Text = Server.HtmlEncode(message);
        }

        private void ShowError(string message)
        {
            lblSubmitMessage.Visible = true;
            lblSubmitMessage.CssClass = "validation-error message-error";
            lblSubmitMessage.Text = Server.HtmlEncode(message);
        }

        protected void cvCoverLetter_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string value = txtCoverLetter.Text.Trim();
            args.IsValid = value.Length >= 50 && value.Length <= 1000;
        }

        protected void cvProposedPrice_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal price;
            bool validPrice = decimal.TryParse(txtProposedPrice.Text.Trim(),
                NumberStyles.Number, CultureInfo.InvariantCulture, out price);
            args.IsValid = validPrice && price > 0 && price >= MinBudget && price <= MaxBudget;
        }

        protected void cvAttachment_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!fuAttachment.HasFile)
            {
                args.IsValid = true;
                return;
            }

            string extension = Path.GetExtension(fuAttachment.FileName).ToLowerInvariant();
            int fileSize = fuAttachment.PostedFile.ContentLength;
            args.IsValid = Array.IndexOf(AllowedExtensions, extension) >= 0 &&
                fileSize > 0 && fileSize <= MaxFileSize;
        }

        private int GetEstimatedDays(string selection)
        {
            switch (selection)
            {
                case "1-3": return 2;
                case "4-7": return 6;
                case "1-2-weeks": return 10;
                case "2-4-weeks": return 21;
                case "4-plus-weeks": return 35;
                default: return 0;
            }
        }

        private string SaveProposalAttachment(out string physicalFilePath)
        {
            physicalFilePath = null;
            if (!fuAttachment.HasFile)
                return null;

            string extension = Path.GetExtension(fuAttachment.FileName).ToLowerInvariant();
            if (Array.IndexOf(AllowedExtensions, extension) < 0)
                throw new InvalidOperationException("This file type is not allowed.");

            int fileSize = fuAttachment.PostedFile.ContentLength;
            if (fileSize <= 0 || fileSize > MaxFileSize)
                throw new InvalidOperationException("The attachment must be no larger than 5 MB.");

            string virtualFolder = "~/Uploads/ProposalAttachments/";
            string physicalFolder = Server.MapPath(virtualFolder);
            Directory.CreateDirectory(physicalFolder);
            string savedFileName = Guid.NewGuid().ToString("N") + extension;
            physicalFilePath = Path.Combine(physicalFolder, savedFileName);
            fuAttachment.SaveAs(physicalFilePath);
            return virtualFolder + savedFileName;
        }

        protected void btnSubmitProposal_Click(object sender, EventArgs e)
        {
            Page.Validate("Proposal");
            if (!Page.IsValid)
                return;
            if (ProjectId <= 0)
            {
                ShowError("Invalid project ID.");
                return;
            }

            decimal bidAmount;
            if (!decimal.TryParse(txtProposedPrice.Text.Trim(), NumberStyles.Number,
                CultureInfo.InvariantCulture, out bidAmount) || bidAmount <= 0)
            {
                ShowError("Please enter a valid proposed price.");
                return;
            }

            int estimatedDays = GetEstimatedDays(ddlDeliveryTime.SelectedValue);
            if (estimatedDays == 0)
            {
                ShowError("Please select a delivery time.");
                return;
            }

            string attachmentPath = null;
            string physicalFilePath = null;
            try
            {
                attachmentPath = SaveProposalAttachment(out physicalFilePath);
                Proposal proposal = new Proposal(
                    0,
                    ProjectId,
                    TestUserContext.FreelancerID,
                    txtCoverLetter.Text.Trim(),
                    bidAmount,
                    estimatedDays,
                    attachmentPath,
                    WorkflowStatuses.ProposalPending,
                    DateTime.Now,
                    null);

                ServiceFactory.CreateProposalService().SubmitProposal(proposal);
                FlashMessageStore.Success("Your proposal was submitted successfully and is now visible to the client.");
                Response.Redirect("MyProposals.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(physicalFilePath) && File.Exists(physicalFilePath))
                {
                    try { File.Delete(physicalFilePath); }
                    catch (IOException cleanupError) { System.Diagnostics.Trace.TraceWarning(cleanupError.ToString()); }
                }

                System.Diagnostics.Trace.TraceError("Proposal submission failed: " + ex);
                ShowError("Proposal could not be saved. Please review your details and try again.");
            }
        }
    }
}
