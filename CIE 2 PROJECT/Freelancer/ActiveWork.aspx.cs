using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class ActiveWork : Page
    {
        private sealed class ActiveProjectItem
        {
            public int ContractID { get; set; }
            public int ProjectID { get; set; }
            public string Title { get; set; }
            public string Client { get; set; }
            public string ClientProfileUrl { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public string StartedOn { get; set; }
            public string Deadline { get; set; }
            public string AgreedPrice { get; set; }
            public string PaymentStatus { get; set; }
            public string StatusLabel { get; set; }
            public string BadgeClass { get; set; }
            public string BadgeIcon { get; set; }
            public string IconClass { get; set; }
            public string IconFa { get; set; }
            public string ProgressLabel { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadActiveWork();
        }

        private void LoadActiveWork()
        {
            lblPageMessage.Visible = false;
            pnlEmpty.Visible = false;
            try
            {
                IList<Contract> contracts = ServiceFactory.CreateContractService()
                    .GetActiveForFreelancer(TestUserContext.FreelancerID);
                IProjectService projectService = ServiceFactory.CreateProjectService();
                IUserService userService = ServiceFactory.CreateUserService();
                IList<Payment> paymentHistory = ServiceFactory.CreatePaymentService()
                    .GetFreelancerPaymentHistory(TestUserContext.FreelancerID);
                List<ActiveProjectItem> projects = new List<ActiveProjectItem>();
                decimal totalAgreedValue = 0m;
                int pendingReviews = 0;

                foreach (Contract contract in contracts)
                {
                    Project project = projectService.GetProject(contract.ProjectID);
                    if (project == null)
                        continue;

                    User client = userService.GetUser(contract.ClientID);
                    Payment payment = paymentHistory.FirstOrDefault(
                        paymentRecord => paymentRecord.ContractID == contract.ContractID);
                    string category = string.IsNullOrWhiteSpace(project.Category)
                        ? "Uncategorized" : project.Category;
                    string paymentStatus = payment == null
                        ? WorkflowStatuses.PaymentNotFunded
                        : payment.PaymentStatus;

                    ActiveProjectItem item = new ActiveProjectItem
                    {
                        ContractID = contract.ContractID,
                        ProjectID = contract.ProjectID,
                        Title = project.Title,
                        Client = client == null ? "Client #" + contract.ClientID : client.FullName,
                        ClientProfileUrl = "ViewClientProfile.aspx?id=" +
                            contract.ClientID.ToString(CultureInfo.InvariantCulture) + "&viewer=freelancer",
                        Description = string.IsNullOrWhiteSpace(contract.WorkDescription)
                            ? project.Description : contract.WorkDescription,
                        Category = category,
                        StartedOn = contract.CreatedAt.ToString("dd MMM yyyy", CultureInfo.InvariantCulture),
                        Deadline = project.Deadline.HasValue
                            ? project.Deadline.Value.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)
                            : "Not specified",
                        AgreedPrice = (payment == null
                            ? contract.AgreedAmount - SimulatedPlatformFeeCalculator.CalculateFee(contract.AgreedAmount)
                            : payment.FreelancerNetAmount).ToString("C0", CultureInfo.GetCultureInfo("en-IN")),
                        PaymentStatus = paymentStatus,
                        StatusLabel = contract.ApprovalStatus,
                        ProgressLabel = GetProgressLabel(contract.ApprovalStatus, paymentStatus)
                    };
                    SetCategoryAppearance(item, category);
                    SetStatusAppearance(item, contract.ApprovalStatus);
                    projects.Add(item);
                    totalAgreedValue += payment == null
                        ? contract.AgreedAmount - SimulatedPlatformFeeCalculator.CalculateFee(contract.AgreedAmount)
                        : payment.FreelancerNetAmount;
                    if (contract.ApprovalStatus == WorkflowStatuses.ContractPendingReview)
                        pendingReviews++;
                }

                rptActiveWork.DataSource = projects;
                rptActiveWork.DataBind();
                lblTotalActive.Text = projects.Count.ToString(CultureInfo.InvariantCulture);
                lblInProgress.Text = projects.Count.ToString(CultureInfo.InvariantCulture);
                lblPendingReview.Text = pendingReviews.ToString(CultureInfo.InvariantCulture);
                lblTotalEarnings.Text = totalAgreedValue.ToString(
                    "C0", CultureInfo.GetCultureInfo("en-IN"));
                pnlEmpty.Visible = projects.Count == 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Freelancer active work failed: " + ex);
                rptActiveWork.DataSource = null;
                rptActiveWork.DataBind();
                lblTotalActive.Text = "0";
                lblInProgress.Text = "0";
                lblPendingReview.Text = "0";
                lblTotalEarnings.Text = "₹0";
                pnlEmpty.Visible = false;
                lblPageMessage.Text = "Unable to load active work. Please try again.";
                lblPageMessage.Visible = true;
            }
        }

        private static string GetProgressLabel(string status, string paymentStatus)
        {
            if (status == WorkflowStatuses.ContractNotSubmitted)
                return paymentStatus == WorkflowStatuses.PaymentHeld
                    ? "Ready to submit work" : "Awaiting client funding";
            if (status == WorkflowStatuses.ContractPendingReview)
                return "Submitted - awaiting client review";
            if (status == WorkflowStatuses.ContractRevisionRequested)
                return "Revision requested - check client feedback";
            if (status == WorkflowStatuses.ContractApproved)
                return "Approved - awaiting payment release";
            return status;
        }

        private static void SetStatusAppearance(ActiveProjectItem item, string status)
        {
            if (status == WorkflowStatuses.ContractRevisionRequested)
            {
                item.BadgeClass = "status-pending";
                item.BadgeIcon = "fa-solid fa-rotate";
            }
            else if (status == WorkflowStatuses.ContractPendingReview)
            {
                item.BadgeClass = "status-progress";
                item.BadgeIcon = "fa-solid fa-hourglass-half";
            }
            else if (status == WorkflowStatuses.ContractApproved)
            {
                item.BadgeClass = "status-completed";
                item.BadgeIcon = "fa-solid fa-circle-check";
            }
            else
            {
                item.BadgeClass = "status-progress";
                item.BadgeIcon = "fa-solid fa-circle-check";
            }
        }

        private static void SetCategoryAppearance(ActiveProjectItem item, string category)
        {
            switch ((category ?? "").Trim().ToLowerInvariant())
            {
                case "game development":
                    item.IconClass = "game-icon";
                    item.IconFa = "fa-solid fa-gamepad";
                    break;
                case "content writing":
                case "writing":
                    item.IconClass = "writing-icon";
                    item.IconFa = "fa-regular fa-file-lines";
                    break;
                case "graphic design":
                case "ui/ux design":
                case "design":
                    item.IconClass = "design-icon";
                    item.IconFa = "fa-solid fa-palette";
                    break;
                case "mobile development":
                    item.IconClass = "mobile-icon";
                    item.IconFa = "fa-solid fa-mobile-screen";
                    break;
                case "database":
                case "data science":
                    item.IconClass = "database-icon";
                    item.IconFa = "fa-solid fa-database";
                    break;
                default:
                    item.IconClass = "code-icon";
                    item.IconFa = "fa-solid fa-code";
                    break;
            }
        }
    }
}
