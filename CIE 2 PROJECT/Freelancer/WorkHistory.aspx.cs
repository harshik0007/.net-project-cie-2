using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class WorkHistory : Page
    {
        private sealed class PaymentView
        {
            public string ProjectTitle { get; set; }
            public string ActivityDate { get; set; }
            public string Amount { get; set; }
            public string Status { get; set; }
            public string StatusClass { get; set; }
            public string StatusIcon { get; set; }
            public string ActivityLabel { get; set; }
            public string Details { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadHistory();
        }

        private void LoadHistory()
        {
            try
            {
                IContractService contractService = ServiceFactory.CreateContractService();
                IProjectService projectService = ServiceFactory.CreateProjectService();
                IPaymentService paymentService = ServiceFactory.CreatePaymentService();
                List<PaymentView> paymentRows = new List<PaymentView>();
                decimal totalReceived = 0m;
                foreach (Payment payment in paymentService.GetFreelancerPaymentHistory(
                    TestUserContext.FreelancerID))
                {
                    Contract contract = contractService.GetForFreelancer(
                        TestUserContext.FreelancerID, payment.ContractID);
                    if (contract == null)
                        continue;
                    Project project = projectService.GetProject(contract.ProjectID);
                    if (project == null)
                        continue;
                    DateTime activityAt = payment.ReleasedAt ?? payment.FundedAt ?? payment.CreatedAt;
                    string statusClass;
                    string statusIcon;
                    string activityLabel;
                    GetPaymentPresentation(payment.PaymentStatus, out statusClass,
                        out statusIcon, out activityLabel);
                    decimal receivedAmount = payment.PaymentStatus == WorkflowStatuses.PaymentRefunded
                        ? payment.FreelancerPayoutAmount
                        : payment.PaymentStatus == WorkflowStatuses.PaymentReleased ? payment.FreelancerNetAmount : 0m;
                    string status = payment.PaymentStatus == WorkflowStatuses.PaymentRefunded &&
                        payment.FreelancerPayoutAmount > 0m ? "Cancellation payout" : payment.PaymentStatus;
                    if (payment.PaymentStatus == WorkflowStatuses.PaymentRefunded)
                    {
                        if (payment.FreelancerPayoutAmount > 0m)
                        {
                            statusClass = "payment-status-released";
                            statusIcon = "fa-solid fa-circle-check";
                            activityLabel = "Cancellation payout (10%)";
                        }
                        else
                            activityLabel = "Escrow refunded to client";
                    }
                    string details = payment.PaymentStatus == WorkflowStatuses.PaymentRefunded &&
                        payment.FreelancerPayoutAmount > 0m
                        ? "10% allocation from the pre-GST contract amount " + payment.Amount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) +
                            "; client refund including returned GST " +
                            (payment.ClientRefundAmount + payment.GstRefundAmount + payment.ClientPlatformFeeRefundAmount).ToString("C2", CultureInfo.GetCultureInfo("en-IN")) +
                            "; GST returned " + payment.GstRefundAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) +
                            "; client platform fee returned " + payment.ClientPlatformFeeRefundAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) +
                            "; platform fee " + payment.PlatformFeeAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) + "."
                        : payment.PaymentStatus == WorkflowStatuses.PaymentReleased
                            ? "Contract amount " + payment.Amount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) +
                                "; freelancer platform fee " + payment.FreelancerPlatformFeeRate.ToString("0.##", CultureInfo.InvariantCulture) + "% (" +
                                payment.FreelancerPlatformFeeAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) +
                                "); net payout " + payment.FreelancerNetAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) +
                                " released. Client GST at " + payment.GstRate.ToString("0.##", CultureInfo.InvariantCulture) + "% (" +
                                payment.GstAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) + ")" +
                                " and client platform fee " + payment.ClientPlatformFeeRate.ToString("0.##", CultureInfo.InvariantCulture) + "% (" +
                                payment.ClientPlatformFeeAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")) +
                                ") were charged separately."
                            : payment.PaymentStatus == WorkflowStatuses.PaymentRefunded
                                ? "No freelancer payout was recorded for this full refund."
                                : payment.PaymentStatus == WorkflowStatuses.PaymentHeld
                                    ? "Funds are held in escrow; this is not yet a received payment."
                                    : "This contract is awaiting funding.";
                    decimal displayedAmount = payment.PaymentStatus == WorkflowStatuses.PaymentRefunded
                        ? payment.FreelancerPayoutAmount : payment.FreelancerNetAmount;
                    paymentRows.Add(new PaymentView
                    {
                        ProjectTitle = project.Title,
                        ActivityDate = activityAt.ToString("dd MMM yyyy", CultureInfo.InvariantCulture),
                        Amount = displayedAmount.ToString("C2", CultureInfo.GetCultureInfo("en-IN")),
                        Status = status,
                        StatusClass = statusClass,
                        StatusIcon = statusIcon,
                        ActivityLabel = activityLabel,
                        Details = details
                    });
                    totalReceived += receivedAmount;
                }
                rptPayments.DataSource = paymentRows;
                rptPayments.DataBind();
                pnlNoPayments.Visible = paymentRows.Count == 0;
                lblTransactionCount.Text = paymentRows.Count.ToString(CultureInfo.InvariantCulture);
                lblTotalReleased.Text = totalReceived.ToString("C2", CultureInfo.GetCultureInfo("en-IN"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Freelancer work history failed to load: " + ex);
                rptPayments.DataSource = null;
                rptPayments.DataBind();
                pnlNoPayments.Visible = false;
                lblMessage.Text = Server.HtmlEncode(DatabaseErrorMessageHelper.GetUserMessage(
                    ex, "Unable to load your payment history."));
                lblMessage.Visible = true;
            }
        }

        private static void GetPaymentPresentation(string status, out string cssClass,
            out string icon, out string activityLabel)
        {
            switch (status)
            {
                case WorkflowStatuses.PaymentReleased:
                    cssClass = "payment-status-released";
                    icon = "fa-solid fa-circle-check";
                    activityLabel = "Payment released";
                    break;
                case WorkflowStatuses.PaymentRefunded:
                    cssClass = "payment-status-refunded";
                    icon = "fa-solid fa-arrow-rotate-left";
                    activityLabel = "Escrow refund";
                    break;
                case WorkflowStatuses.PaymentHeld:
                    cssClass = "payment-status-held";
                    icon = "fa-regular fa-clock";
                    activityLabel = "Funds held";
                    break;
                default:
                    cssClass = "payment-status-pending";
                    icon = "fa-regular fa-clock";
                    activityLabel = "Payment pending";
                    break;
            }
        }
    }
}
