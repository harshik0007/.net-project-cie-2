using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class ReviewSubmittedWork : Page
    {
        private sealed class SubmissionView
        {
            public int SubmissionNumber { get; set; }
            public string Status { get; set; }
            public string SubmittedAt { get; set; }
            public string Message { get; set; }
            public string Feedback { get; set; }
            public IList<FileView> Files { get; set; }
        }

        private sealed class FileView
        {
            public string FileKey { get; set; }
            public string Name { get; set; }
            public string Size { get; set; }
        }

        private int ContractId
        {
            get { return ViewState["ReviewContractId"] == null ? 0 : (int)ViewState["ReviewContractId"]; }
            set { ViewState["ReviewContractId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadPage();
        }

        protected void RequestRevision_Click(object sender, EventArgs e)
        {
            Page.Validate("Revision");
            if (!Page.IsValid)
                return;

            try
            {
                ServiceFactory.CreateSubmissionService().RequestRevision(
                    TestUserContext.ClientID, ContractId, txtRevisionFeedback.Text);
                FlashMessageStore.Success("Revision feedback was sent to the freelancer.");
                Response.Redirect("ReviewSubmittedWork.aspx?contractId=" +
                    ContractId.ToString(CultureInfo.InvariantCulture), false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Client revision request failed: " + ex);
                ShowMessage("The revision request could not be saved. Check that the latest submission is awaiting review.", "action-message revision-message");
            }
        }

        protected void ApproveWork_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceFactory.CreateSubmissionService().ApproveSubmission(
                    TestUserContext.ClientID, ContractId);
                FlashMessageStore.Success("Work approved. The simulated payment was released and the contract is complete.");
                Response.Redirect("ReviewSubmittedWork.aspx?contractId=" +
                    ContractId.ToString(CultureInfo.InvariantCulture), false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Client work approval failed: " + ex);
                ShowMessage("Approval and payment release could not be completed. Check that the latest submission is awaiting review and escrow is held.", "action-message revision-message");
            }
        }

        protected void SubmitReview_Click(object sender, EventArgs e)
        {
            Page.Validate("Review");
            if (!Page.IsValid)
                return;

            int rating;
            if (!int.TryParse(ddlRating.SelectedValue, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out rating))
            {
                ShowMessage("Choose a rating from 1 to 5.", "action-message revision-message");
                return;
            }

            try
            {
                Contract contract = ServiceFactory.CreateContractService().GetForClient(
                    TestUserContext.ClientID, ContractId);
                ServiceFactory.CreateReviewService().SubmitReview(
                    ContractId, TestUserContext.ClientID, contract.FreelancerID,
                    "Client", rating, txtReview.Text);
                FlashMessageStore.Success("Your review was submitted successfully.");
                Response.Redirect("ReviewSubmittedWork.aspx?contractId=" +
                    ContractId.ToString(CultureInfo.InvariantCulture), false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Client review submission failed: " + ex);
                ShowMessage("The review could not be saved. It is available once this contract is completed, once per participant.", "action-message revision-message");
            }
        }

        protected void DownloadFile_Click(object sender, EventArgs e)
        {
            LinkButton button = sender as LinkButton;
            string[] key = button == null ? new string[0] : button.CommandArgument.Split('|');
            int submissionId;
            int fileId;
            if (key.Length != 2 ||
                !int.TryParse(key[0], out submissionId) ||
                !int.TryParse(key[1], out fileId))
            {
                ShowMessage("The selected file is not available.", "action-message revision-message");
                return;
            }

            try
            {
                IList<SubmissionFile> files = ServiceFactory.CreateSubmissionService()
                    .GetFilesForClient(TestUserContext.ClientID, submissionId);
                SubmissionFile file = files.FirstOrDefault(item => item.SubmissionFileID == fileId);
                if (file == null)
                    throw new InvalidOperationException("The requested file was not found.");

                string fullPath = ResolveUploadPath(file.FilePath);
                if (!File.Exists(fullPath))
                    throw new FileNotFoundException("The stored upload is missing.");

                Response.Clear();
                Response.ContentType = string.IsNullOrWhiteSpace(file.ContentType)
                    ? "application/octet-stream" : file.ContentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" +
                    HttpUtility.UrlPathEncode(Path.GetFileName(file.OriginalFileName)) + "\"");
                Response.TransmitFile(fullPath);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Submission file download failed: " + ex);
                ShowMessage("The selected file could not be downloaded.", "action-message revision-message");
            }
        }

        private void LoadPage()
        {
            int contractId;
            if (ContractId <= 0)
            {
                if (!int.TryParse(Request.QueryString["contractId"], out contractId) || contractId <= 0)
                {
                    int projectId;
                    if (!int.TryParse(Request.QueryString["projectId"], out projectId) || projectId <= 0)
                    {
                        ShowMessage("A valid contract is required to review work.", "action-message revision-message");
                        return;
                    }

                    try
                    {
                        Contract projectContract = ServiceFactory.CreateContractService()
                            .GetForClientByProjectId(TestUserContext.ClientID, projectId);
                        contractId = projectContract.ContractID;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError("Client review contract lookup failed: " + ex);
                        ShowMessage("The contract could not be found for this client.", "action-message revision-message");
                        return;
                    }
                }
                ContractId = contractId;
            }

            try
            {
                IContractService contractService = ServiceFactory.CreateContractService();
                Contract contract = contractService.GetForClient(TestUserContext.ClientID, ContractId);
                Project project = ServiceFactory.CreateProjectService().GetProject(contract.ProjectID);
                User freelancer = ServiceFactory.CreateUserService().GetUser(contract.FreelancerID);
                if (project == null || freelancer == null)
                    throw new InvalidOperationException("The contract participants or project could not be found.");

                IList<Payment> paymentHistory = ServiceFactory.CreatePaymentService()
                    .GetClientPaymentHistory(TestUserContext.ClientID);
                Payment payment = paymentHistory.FirstOrDefault(item => item.ContractID == ContractId);
                lblPageHeading.Text = payment != null &&
                    payment.PaymentStatus == WorkflowStatuses.PaymentRefunded
                    ? "Cancellation and Payment Details"
                    : "Review Submitted Work";
                ISubmissionService submissionService = ServiceFactory.CreateSubmissionService();
                IList<WorkSubmission> history = submissionService.GetHistoryForClient(
                    TestUserContext.ClientID, ContractId);
                WorkSubmission latest = history.FirstOrDefault();

                lblProjectTitle.Text = Server.HtmlEncode(project.Title);
                lblSummaryProject.Text = Server.HtmlEncode(project.Title);
                string freelancerProfileUrl = "ViewFreelancerProfile.aspx?id=" +
                    contract.FreelancerID.ToString(CultureInfo.InvariantCulture) + "&viewer=client";
                lnkFreelancerProfile.Text = Server.HtmlEncode(freelancer.FullName);
                lnkFreelancerProfile.NavigateUrl = freelancerProfileUrl;
                lnkSummaryFreelancerProfile.Text = Server.HtmlEncode(freelancer.FullName);
                lnkSummaryFreelancerProfile.NavigateUrl = freelancerProfileUrl;
                lnkMessageFreelancer.NavigateUrl = ResolveUrl("~/Messages/Messages.aspx?with=" +
                    contract.FreelancerID.ToString(CultureInfo.InvariantCulture) + "&projectId=" +
                    contract.ProjectID.ToString(CultureInfo.InvariantCulture));
                lblFreelancerInitials.Text = Server.HtmlEncode(GetInitials(freelancer.FullName));
                lblContractStatus.Text = Server.HtmlEncode(contract.ApprovalStatus);
                CultureInfo currencyCulture = CultureInfo.GetCultureInfo("en-IN");
                lblAgreedAmount.Text = Server.HtmlEncode(contract.AgreedAmount.ToString("C2", currencyCulture));
                pnlStandardPaymentBreakdown.Visible = true;
                pnlCancellationBreakdown.Visible = false;
                if (payment == null)
                {
                    lblGstRate.Text = "0";
                    lblGstAmount.Text = "Not funded";
                    lblClientFeeRate.Text = "0";
                    lblClientFeeAmount.Text = "Not funded";
                    lblClientTotal.Text = "Not funded";
                    lblFreelancerFeeRate.Text = "0";
                    lblFreelancerFeeAmount.Text = "Not funded";
                    lblFreelancerPayout.Text = "Not funded";
                }
                else
                {
                    bool isRefunded = payment.PaymentStatus == WorkflowStatuses.PaymentRefunded;
                    pnlStandardPaymentBreakdown.Visible = !isRefunded;
                    pnlCancellationBreakdown.Visible = isRefunded;

                    if (isRefunded)
                    {
                        bool hasCancellationSplit = payment.FreelancerPayoutAmount > 0m ||
                            payment.PlatformFeeAmount > 0m;
                        decimal refundTotal = hasCancellationSplit
                            ? payment.ClientRefundAmount + payment.GstRefundAmount +
                                payment.ClientPlatformFeeRefundAmount
                            : payment.ClientTotalAmount;

                        lblClientPaidTotal.Text = Server.HtmlEncode(
                            payment.ClientTotalAmount.ToString("C2", currencyCulture));
                        lblClientRefundTotal.Text = Server.HtmlEncode(
                            refundTotal.ToString("C2", currencyCulture));
                        lblClientContractRefund.Text = Server.HtmlEncode(
                            payment.ClientRefundAmount.ToString("C2", currencyCulture));
                        lblGstReturned.Text = Server.HtmlEncode(
                            payment.GstRefundAmount.ToString("C2", currencyCulture));
                        lblClientFeeReturned.Text = Server.HtmlEncode(
                            payment.ClientPlatformFeeRefundAmount.ToString("C2", currencyCulture));
                        lblCancellationFreelancerPayout.Text = Server.HtmlEncode(
                            payment.FreelancerPayoutAmount.ToString("C2", currencyCulture));
                        lblPlatformCancellationShare.Text = Server.HtmlEncode(
                            payment.PlatformFeeAmount.ToString("C2", currencyCulture));

                        pnlCancellationComponents.Visible = hasCancellationSplit;
                        pnlCancellationAllocation.Visible = hasCancellationSplit;
                        lblLegacyRefundNote.Visible = !hasCancellationSplit;
                        lblLegacyRefundNote.Text = hasCancellationSplit
                            ? string.Empty
                            : "This earlier payment was recorded as a full refund; its settlement split was not stored.";
                        lblCancellationPolicyNote.Visible = hasCancellationSplit;
                    }
                    else
                    {
                        lblGstRate.Text = payment.GstRate.ToString("0.##", CultureInfo.InvariantCulture);
                        lblGstAmount.Text = Server.HtmlEncode(payment.GstAmount.ToString("C2", currencyCulture));
                        lblClientFeeRate.Text = payment.ClientPlatformFeeRate.ToString("0.##", CultureInfo.InvariantCulture);
                        lblClientFeeAmount.Text = Server.HtmlEncode(payment.ClientPlatformFeeAmount.ToString("C2", currencyCulture));
                        lblClientTotal.Text = Server.HtmlEncode(payment.ClientTotalAmount.ToString("C2", currencyCulture));
                        lblFreelancerFeeRate.Text = payment.FreelancerPlatformFeeRate.ToString("0.##", CultureInfo.InvariantCulture);
                        lblFreelancerFeeAmount.Text = Server.HtmlEncode(payment.FreelancerPlatformFeeAmount.ToString("C2", currencyCulture));
                        lblFreelancerPayout.Text = Server.HtmlEncode(payment.FreelancerNetAmount.ToString("C2", currencyCulture));
                    }
                }
                lblDeadline.Text = project.Deadline.HasValue
                    ? project.Deadline.Value.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) : "Not specified";
                lblCreatedAt.Text = contract.CreatedAt.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                lblPaymentStatus.Text = Server.HtmlEncode(payment == null
                    ? WorkflowStatuses.PaymentNotFunded : payment.PaymentStatus);

                pnlNoSubmission.Visible = latest == null;
                pnlReviewActions.Visible = latest != null &&
                    latest.SubmissionStatus == WorkflowStatuses.SubmissionPendingReview;
                bool contractCompleted = contract.ApprovalStatus == WorkflowStatuses.ContractCompleted;
                bool hasSubmittedReview = contractCompleted && ServiceFactory.CreateReviewService()
                    .HasReviewedContract(ContractId, TestUserContext.ClientID);
                pnlLeaveReview.Visible = contractCompleted && !hasSubmittedReview;
                pnlReviewSubmitted.Visible = contractCompleted && hasSubmittedReview;

                if (latest == null)
                {
                    lblSubmissionMessage.Text = "No submission yet.";
                    lblSubmissionDate.Text = "Not available";
                    lblSubmissionStatus.Text = "Not available";
                    lblRevisionFeedback.Text = string.Empty;
                    pnlRevisionFeedback.Visible = false;
                }
                else
                {
                    lblSubmissionMessage.Text = Server.HtmlEncode(latest.CompletionMessage);
                    lblSubmissionDate.Text = latest.SubmittedAt.ToString("dd MMM yyyy, h:mm tt", CultureInfo.InvariantCulture);
                    lblSubmissionStatus.Text = Server.HtmlEncode(latest.SubmissionStatus);
                    lblRevisionFeedback.Text = Server.HtmlEncode(latest.ClientFeedback ?? string.Empty);
                    pnlRevisionFeedback.Visible = !string.IsNullOrWhiteSpace(latest.ClientFeedback);
                }

                lnkDemo.Visible = latest != null && UrlValidationHelper.IsValidHttpUrl(latest.DemoURL);
                lnkDemo.NavigateUrl = lnkDemo.Visible ? latest.DemoURL : string.Empty;
                lblNoDemo.Visible = !lnkDemo.Visible;
                lnkGithub.Visible = latest != null && UrlValidationHelper.IsValidHttpUrl(latest.GithubURL);
                lnkGithub.NavigateUrl = lnkGithub.Visible ? latest.GithubURL : string.Empty;
                lblNoGithub.Visible = !lnkGithub.Visible;

                List<SubmissionView> submissionViews = new List<SubmissionView>();
                foreach (WorkSubmission submission in history)
                {
                    IList<FileView> files = submissionService.GetFilesForClient(
                        TestUserContext.ClientID, submission.SubmissionID)
                        .Select(file => new FileView
                        {
                            FileKey = submission.SubmissionID.ToString(CultureInfo.InvariantCulture) + "|" +
                                file.SubmissionFileID.ToString(CultureInfo.InvariantCulture),
                            Name = file.OriginalFileName,
                            Size = FormatFileSize(file.FileSize)
                        }).ToList();
                    submissionViews.Add(new SubmissionView
                    {
                        SubmissionNumber = submission.SubmissionNumber,
                        Status = submission.SubmissionStatus,
                        SubmittedAt = submission.SubmittedAt.ToString("dd MMM yyyy, h:mm tt", CultureInfo.InvariantCulture),
                        Message = submission.CompletionMessage,
                        Feedback = submission.ClientFeedback,
                        Files = files
                    });
                }
                rptSubmissions.DataSource = submissionViews;
                rptSubmissions.DataBind();
                pnlReviewPage.Visible = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Client review page failed to load: " + ex);
                pnlReviewPage.Visible = false;
                ShowMessage("Unable to load this contract and its submission history.", "action-message revision-message");
            }
        }

        protected void RptSubmissions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;
            SubmissionView submission = e.Item.DataItem as SubmissionView;
            Repeater files = e.Item.FindControl("rptSubmissionFiles") as Repeater;
            if (submission != null && files != null)
                files.DataSource = submission.Files;
            if (files != null)
                files.DataBind();
        }

        private string ResolveUploadPath(string storedPath)
        {
            if (string.IsNullOrWhiteSpace(storedPath))
                throw new InvalidOperationException("The stored upload path is empty.");

            string candidate = storedPath.StartsWith("~/", StringComparison.Ordinal)
                ? Server.MapPath(storedPath)
                : Server.MapPath("~/" + storedPath.TrimStart('/', '\\'));
            string fullPath = Path.GetFullPath(candidate);
            string uploadsRoot = Path.GetFullPath(Server.MapPath("~/Uploads/"));
            string rootPrefix = uploadsRoot.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            if (!fullPath.StartsWith(rootPrefix, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("The requested file is outside the uploads directory.");
            return fullPath;
        }

        private static string FormatFileSize(long bytes)
        {
            if (bytes >= 1024L * 1024L)
                return (bytes / (1024d * 1024d)).ToString("0.0", CultureInfo.InvariantCulture) + " MB";
            return Math.Max(1, bytes / 1024L).ToString(CultureInfo.InvariantCulture) + " KB";
        }

        private static string GetInitials(string fullName)
        {
            string[] parts = (fullName ?? string.Empty).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return "SL";
            return parts.Length == 1
                ? parts[0].Substring(0, 1).ToUpperInvariant()
                : (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpperInvariant();
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblActionMessage.Text = Server.HtmlEncode(message);
            lblActionMessage.CssClass = cssClass;
            lblActionMessage.Visible = true;
        }
    }
}
