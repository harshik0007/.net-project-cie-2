using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class Payments : Page
    {
        private sealed class PendingPayment
        {
            public int ContractID { get; set; }
            public string Project { get; set; }
            public string ProjectDescription { get; set; }
            public string Freelancer { get; set; }
            public string FreelancerProfileUrl { get; set; }
            public string Initials { get; set; }
            public string Role { get; set; }
            public string Amount { get; set; }
            public string Milestone { get; set; }
            public string MilestoneDetails { get; set; }
            public string DueDate { get; set; }
            public string PaymentStatus { get; set; }
            public string ActionText { get; set; }
            public bool CanRelease { get; set; }
            public bool CanReviewProposals { get; set; }
            public string ProposalReviewUrl { get; set; }
            public bool CanCancel { get; set; }
        }

        private sealed class CompletedPayment
        {
            public int ContractID { get; set; }
            public string Project { get; set; }
            public string ProjectDescription { get; set; }
            public string Freelancer { get; set; }
            public string FreelancerProfileUrl { get; set; }
            public string Initials { get; set; }
            public string Role { get; set; }
            public string Amount { get; set; }
            public string Milestone { get; set; }
            public string MilestoneDetails { get; set; }
            public string PaidOn { get; set; }
            public string PaymentStatus { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadPayments(Request.QueryString["tab"] ?? "Pending");
        }

        private void LoadPayments(string selectedTab)
        {
            selectedTab = NormalizeTab(selectedTab);
            SetTab(selectedTab);
            try
            {
                IContractService contractService = ServiceFactory.CreateContractService();
                IProjectService projectService = ServiceFactory.CreateProjectService();
                IUserService userService = ServiceFactory.CreateUserService();
                IPaymentService paymentService = ServiceFactory.CreatePaymentService();
                ISubmissionService submissionService = ServiceFactory.CreateSubmissionService();

                IList<Contract> contracts = contractService.GetForClient(TestUserContext.ClientID);
                IList<Payment> paymentHistory = paymentService.GetClientPaymentHistory(TestUserContext.ClientID);
                List<PendingPayment> pending = new List<PendingPayment>();
                List<CompletedPayment> completed = new List<CompletedPayment>();
                decimal escrowHeld = 0m;
                decimal pendingAmount = 0m;
                decimal totalPaid = 0m;
                decimal completedTotal = 0m;
                int escrowContracts = 0;

                foreach (Contract contract in contracts)
                {
                    Payment payment = paymentHistory.FirstOrDefault(
                        record => record.ContractID == contract.ContractID);
                    if (payment == null && contract.ApprovalStatus == WorkflowStatuses.ContractCancelled)
                        continue;
                    Project project = projectService.GetProject(contract.ProjectID);
                    if (project == null)
                        continue;
                    User freelancer = userService.GetUser(contract.FreelancerID);
                    string freelancerName = freelancer == null
                        ? "Freelancer #" + contract.FreelancerID : freelancer.FullName;
                    string paymentStatus = payment == null
                        ? WorkflowStatuses.PaymentNotFunded : payment.PaymentStatus;
                    WorkSubmission latest = null;
                    if (contract.ApprovalStatus != WorkflowStatuses.ContractNotSubmitted &&
                        contract.ApprovalStatus != WorkflowStatuses.ContractCancelled)
                    {
                        latest = submissionService.GetLatestForClient(
                            TestUserContext.ClientID, contract.ContractID);
                    }

                    string description = ShortDescription(project.Description);
                    decimal contractAmount = payment == null ? contract.AgreedAmount : payment.Amount;
                    decimal gstRate = payment == null ? SimulatedGstCalculator.DemoRate : payment.GstRate;
                    decimal gstAmount = payment == null
                        ? SimulatedGstCalculator.CalculateTax(contractAmount, gstRate)
                        : payment.GstAmount;
                    decimal clientPlatformFee = payment == null
                        ? SimulatedPlatformFeeCalculator.CalculateFee(contractAmount)
                        : payment.ClientPlatformFeeAmount;
                    decimal clientFeeRate = payment == null
                        ? SimulatedPlatformFeeCalculator.DemoRate : payment.ClientPlatformFeeRate;
                    decimal clientTotal = payment == null
                        ? contractAmount + gstAmount + clientPlatformFee : payment.ClientTotalAmount;
                    string amount = clientTotal.ToString("C2", CultureInfo.GetCultureInfo("en-IN"));
                    string paymentBreakdown = gstRate > 0m
                        ? "Contract " + contractAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) +
                            " + demo GST " + gstRate.ToString("0.##", CultureInfo.InvariantCulture) + "% " +
                            gstAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) +
                            "; client platform fee " + clientFeeRate.ToString("0.##", CultureInfo.InvariantCulture) + "% " +
                            clientPlatformFee.ToString("C2", CultureInfo.GetCultureInfo("en-IN"))
                        : "Contract amount " + contractAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) +
                            "; no GST or client platform fee was recorded for this earlier transaction";
                    string initials = GetInitials(freelancerName);

                    if (paymentStatus == WorkflowStatuses.PaymentReleased ||
                        paymentStatus == WorkflowStatuses.PaymentRefunded)
                    {
                        if (paymentStatus == WorkflowStatuses.PaymentReleased)
                            totalPaid += payment.FreelancerNetAmount;
                        else if (paymentStatus == WorkflowStatuses.PaymentRefunded)
                            totalPaid += payment.FreelancerPayoutAmount;
                        DateTime paidAt = paymentStatus == WorkflowStatuses.PaymentReleased && payment.ReleasedAt.HasValue
                            ? payment.ReleasedAt.Value
                            : payment.FundedAt ?? payment.CreatedAt;
                        bool hasCancellationSplit = payment.FreelancerPayoutAmount > 0m ||
                            payment.PlatformFeeAmount > 0m;
                        decimal completedValue = paymentStatus == WorkflowStatuses.PaymentRefunded
                            ? (hasCancellationSplit
                                ? payment.ClientRefundAmount + payment.GstRefundAmount + payment.ClientPlatformFeeRefundAmount
                                : payment.ClientTotalAmount)
                            : payment.ClientTotalAmount;
                        completedTotal += completedValue;
                        string completedAmount = completedValue.ToString(
                            "C2", CultureInfo.GetCultureInfo("en-IN"));
                        string details = paymentStatus == WorkflowStatuses.PaymentReleased
                            ? payment.GstRate > 0m
                                ? "Client paid: " + payment.ClientTotalAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) + "\n" +
                                    "Contract amount: " + payment.Amount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) + "\n" +
                                    "Demo GST (" + payment.GstRate.ToString("0.##", CultureInfo.InvariantCulture) + "%): " +
                                    payment.GstAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) + "\n" +
                                    "Client platform fee (" + payment.ClientPlatformFeeRate.ToString("0.##", CultureInfo.InvariantCulture) + "%): " +
                                    payment.ClientPlatformFeeAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) + "\n" +
                                    "Freelancer platform fee (" + payment.FreelancerPlatformFeeRate.ToString("0.##", CultureInfo.InvariantCulture) + "%): " +
                                    payment.FreelancerPlatformFeeAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) + "\n" +
                                    "Freelancer payout after fee: " + payment.FreelancerNetAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN"))
                                : "Approved work payment released; no GST was recorded for this earlier transaction"
                            : hasCancellationSplit
                                ? "Client paid total: " + payment.ClientTotalAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) + "\n" +
                                    "Client refund total: " + (payment.ClientRefundAmount + payment.GstRefundAmount +
                                        payment.ClientPlatformFeeRefundAmount).ToString("C2", CultureInfo.GetCultureInfo("en-IN")) + "\n" +
                                    "  Contract refund (75%): " + payment.ClientRefundAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) + "\n" +
                                    "  GST returned: " + payment.GstRefundAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) + "\n" +
                                    "  Client fee returned: " + payment.ClientPlatformFeeRefundAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) + "\n" +
                                    "Freelancer cancellation payout (10%): " + payment.FreelancerPayoutAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) + "\n" +
                                    "Platform cancellation share (15%): " + payment.PlatformFeeAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN"))
                                : "Full simulated refund (legacy cancellation)";
                        completed.Add(new CompletedPayment
                        {
                            ContractID = contract.ContractID,
                            Project = Encode(project.Title),
                            ProjectDescription = Encode(description),
                            Freelancer = Encode(freelancerName),
                            FreelancerProfileUrl = "ViewFreelancerProfile.aspx?id=" +
                                contract.FreelancerID.ToString(CultureInfo.InvariantCulture) + "&viewer=client",
                            Initials = Encode(initials),
                            Role = "Freelancer",
                            Amount = Encode(completedAmount),
                            Milestone = paymentStatus == WorkflowStatuses.PaymentReleased
                                ? "Payment breakdown" : "Cancellation settlement",
                            MilestoneDetails = Encode(details),
                            PaidOn = paidAt.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture),
                            PaymentStatus = paymentStatus
                        });
                        continue;
                    }

                    bool canRelease = paymentStatus == WorkflowStatuses.PaymentHeld &&
                        contract.ApprovalStatus == WorkflowStatuses.ContractApproved &&
                        latest != null && latest.SubmissionStatus == WorkflowStatuses.SubmissionApproved;
                    bool canReviewProposals = payment == null &&
                        contract.ApprovalStatus != WorkflowStatuses.ContractCompleted &&
                        contract.ApprovalStatus != WorkflowStatuses.ContractCancelled &&
                        (project.Status == WorkflowStatuses.ProjectOpen ||
                         project.Status == WorkflowStatuses.ProjectInProgress);
                    bool canCancel = contract.ApprovalStatus == WorkflowStatuses.ContractNotSubmitted &&
                        project.Status == WorkflowStatuses.ProjectInProgress && latest == null &&
                        (payment == null || payment.PaymentStatus == WorkflowStatuses.PaymentHeld);

                    if (paymentStatus == WorkflowStatuses.PaymentHeld)
                    {
                        escrowHeld += payment.ClientTotalAmount;
                        escrowContracts++;
                    }
                    else if (payment == null)
                    {
                        pendingAmount += clientTotal;
                    }

                    pending.Add(new PendingPayment
                    {
                        ContractID = contract.ContractID,
                        Project = Encode(project.Title),
                        ProjectDescription = Encode(description),
                        Freelancer = Encode(freelancerName),
                        FreelancerProfileUrl = "ViewFreelancerProfile.aspx?id=" +
                            contract.FreelancerID.ToString(CultureInfo.InvariantCulture) + "&viewer=client",
                        Initials = Encode(initials),
                        Role = "Freelancer",
                        Amount = Encode(amount),
                        Milestone = "Total due (GST + client fee)",
                        MilestoneDetails = Encode(paymentBreakdown),
                        DueDate = project.Deadline.HasValue
                            ? project.Deadline.Value.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture)
                            : "Not specified",
                        PaymentStatus = paymentStatus,
                        ActionText = canReviewProposals ? "Review proposals" :
                            canRelease ? "Release Payment" : "Awaiting Approval",
                        CanRelease = canRelease,
                        CanReviewProposals = canReviewProposals,
                        ProposalReviewUrl = "ProjectProposal.aspx?projectId=" +
                            contract.ProjectID.ToString(CultureInfo.InvariantCulture) + "&view=proposals",
                        CanCancel = canCancel
                    });
                }

                rptPending.DataSource = pending;
                rptPending.DataBind();
                rptCompleted.DataSource = completed;
                rptCompleted.DataBind();
                pnlPendingEmpty.Visible = pending.Count == 0;
                pnlCompletedEmpty.Visible = completed.Count == 0;
                lblPendingCount.Text = pending.Count.ToString(CultureInfo.InvariantCulture);
                lblCompletedCount.Text = completed.Count.ToString(CultureInfo.InvariantCulture);
                lblEscrowHeld.Text = escrowHeld.ToString("C0", CultureInfo.GetCultureInfo("en-IN"));
                lblEscrowProjects.Text = escrowContracts.ToString(CultureInfo.InvariantCulture);
                lblTotalPaid.Text = totalPaid.ToString("C0", CultureInfo.GetCultureInfo("en-IN"));
                lblPendingAmount.Text = pendingAmount.ToString("C0", CultureInfo.GetCultureInfo("en-IN"));
                lblPendingSummary.Text = pending.Count.ToString(CultureInfo.InvariantCulture) + " pending contracts";
                lblCompletedAmount.Text = completedTotal.ToString("C0", CultureInfo.GetCultureInfo("en-IN"));
                lblCompletedSummary.Text = completed.Count.ToString(CultureInfo.InvariantCulture) + " completed transactions";
                SetTab(selectedTab);
                lblPageMessage.Visible = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Client payment history failed to load: " + ex);
                rptPending.DataSource = null;
                rptPending.DataBind();
                rptCompleted.DataSource = null;
                rptCompleted.DataBind();
                pnlPendingEmpty.Visible = false;
                pnlCompletedEmpty.Visible = false;
                lblPageMessage.Text = Server.HtmlEncode(DatabaseErrorMessageHelper.GetUserMessage(
                    ex, "Unable to load payment history. Please try again."));
                lblPageMessage.Visible = true;
                SetTab(selectedTab);
            }
        }

        protected void ReleasePayment_Click(object sender, EventArgs e)
        {
            int contractId = GetCommandId(sender);
            if (contractId <= 0)
            {
                FlashMessageStore.Error("A valid contract is required to release payment.");
                Response.Redirect("Payments.aspx?tab=Pending", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            try
            {
                IList<Payment> history = ServiceFactory.CreatePaymentService()
                    .GetClientPaymentHistory(TestUserContext.ClientID);
                Payment payment = history.FirstOrDefault(item => item.ContractID == contractId);
                if (payment == null)
                {
                    Contract contract = ServiceFactory.CreateContractService()
                        .GetForClient(TestUserContext.ClientID, contractId);
                    Response.Redirect("ProjectProposal.aspx?projectId=" +
                        contract.ProjectID.ToString(CultureInfo.InvariantCulture) + "&view=proposals", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                ServiceFactory.CreatePaymentService().ReleaseContractPayment(
                    TestUserContext.ClientID, contractId);

                FlashMessageStore.Success("Payment released successfully. The project is now complete.");
                Response.Redirect("Payments.aspx?tab=Completed", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Client payment action failed: " + ex);
                FlashMessageStore.Error(DatabaseErrorMessageHelper.GetUserMessage(
                    ex, "This payment action could not be completed. Please try again."));
                Response.Redirect("Payments.aspx?tab=Pending", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void CancelProject_Click(object sender, EventArgs e)
        {
            int contractId = GetCommandId(sender);
            if (contractId <= 0)
            {
                FlashMessageStore.Error("A valid contract is required to cancel this project.");
                Response.Redirect("Payments.aspx?tab=Pending", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            try
            {
                IPaymentService paymentService = ServiceFactory.CreatePaymentService();
                Payment heldPayment = paymentService
                    .GetClientPaymentHistory(TestUserContext.ClientID)
                    .FirstOrDefault(item => item.ContractID == contractId);
                paymentService.RefundCancelledContract(TestUserContext.ClientID, contractId);
                FlashMessageStore.Success(heldPayment == null
                    ? "Project cancelled. No escrow payment was held, so no refund or cancellation payout was due."
                    : "Project cancelled and the refund, freelancer payout, and platform share were recorded.");
                Response.Redirect("Payments.aspx?tab=Completed", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Contract cancellation failed: " + ex);
                FlashMessageStore.Error(DatabaseErrorMessageHelper.GetUserMessage(
                    ex, "This contract can only be cancelled before work is submitted."));
                Response.Redirect("Payments.aspx?tab=Pending", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void ViewDetails_Click(object sender, EventArgs e)
        {
            int contractId = GetCommandId(sender);
            if (contractId <= 0)
            {
                ShowMessage("Invalid contract.");
                return;
            }

            Response.Redirect("ReviewSubmittedWork.aspx?contractId=" + contractId, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected string GetPaymentStatusClass(object value)
        {
            string status = Convert.ToString(value);
            if (status == WorkflowStatuses.PaymentReleased)
                return "status-completed";
            if (status == WorkflowStatuses.PaymentHeld)
                return "status-progress";
            if (status == WorkflowStatuses.PaymentRefunded)
                return "status-cancelled";
            return "status-pending";
        }

        private void SetTab(string selectedTab)
        {
            bool showPending = NormalizeTab(selectedTab) == "Pending";
            pnlPending.Visible = showPending;
            pnlCompleted.Visible = !showPending;
            lnkPending.Attributes["class"] = "payment-tab" + (showPending ? " active" : "");
            lnkCompleted.Attributes["class"] = "payment-tab" + (showPending ? "" : " active");
        }

        private static string NormalizeTab(string selectedTab)
        {
            return string.Equals(selectedTab, "Completed", StringComparison.OrdinalIgnoreCase)
                ? "Completed"
                : "Pending";
        }

        private int GetCommandId(object sender)
        {
            LinkButton button = sender as LinkButton;
            int id;
            return button != null && int.TryParse(button.CommandArgument, out id) && id > 0
                ? id : 0;
        }

        private void ShowMessage(string message)
        {
            lblPageMessage.Text = Server.HtmlEncode(message);
            lblPageMessage.Visible = true;
        }

        private string ShortDescription(string value)
        {
            string description = value ?? string.Empty;
            return description.Length <= 150 ? description : description.Substring(0, 147) + "...";
        }

        private string GetInitials(string fullName)
        {
            string[] parts = (fullName ?? string.Empty).Split(
                new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return "SL";
            return parts.Length == 1
                ? parts[0].Substring(0, 1).ToUpperInvariant()
                : (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpperInvariant();
        }

        private string Encode(string value)
        {
            return Server.HtmlEncode(value ?? string.Empty);
        }
    }
}
