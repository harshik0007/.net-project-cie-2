using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class SecurePayment : Page
    {
        private int ContractId
        {
            get { return ViewState["FundingContractId"] == null ? 0 : (int)ViewState["FundingContractId"]; }
            set { ViewState["FundingContractId"] = value; }
        }

        private int ProposalId
        {
            get { return ViewState["FundingProposalId"] == null ? 0 : (int)ViewState["FundingProposalId"]; }
            set { ViewState["FundingProposalId"] = value; }
        }

        private int ProjectId
        {
            get { return ViewState["FundingProjectId"] == null ? 0 : (int)ViewState["FundingProjectId"]; }
            set { ViewState["FundingProjectId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadContract();
        }

        protected void FundContract_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProposalId > 0)
                    ServiceFactory.CreatePaymentService().FundProposal(
                        TestUserContext.ClientID, ProjectId, ProposalId);
                else
                    ServiceFactory.CreatePaymentService().FundContract(
                        TestUserContext.ClientID, ContractId);
                FlashMessageStore.Success("Payment was simulated successfully. The freelancer is hired and the project is now in progress.");
                Response.Redirect("Payments.aspx?tab=Pending", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Simulated contract funding failed: " + ex);
                string message = ex is InvalidOperationException ? ex.Message :
                    DatabaseErrorMessageHelper.GetUserMessage(
                        ex, "The contract could not be funded. Check the database connection and try again.");
                LoadContract();
                ShowMessage(message);
            }
        }

        private void LoadContract()
        {
            int projectId = 0;
            int proposalId = 0;
            if (int.TryParse(Request.QueryString["projectId"], out projectId) && projectId > 0 &&
                int.TryParse(Request.QueryString["proposalId"], out proposalId) && proposalId > 0)
            {
                LoadProposalForFunding(projectId, proposalId);
                return;
            }

            if (ContractId <= 0 &&
                (!int.TryParse(Request.QueryString["projectId"], out projectId) || projectId <= 0))
            {
                ShowMessage("A valid project is required to fund a contract.");
                btnFundContract.Enabled = false;
                return;
            }

            try
            {
                IContractService contractService = ServiceFactory.CreateContractService();
                Contract contract;
                if (ContractId > 0)
                    contract = contractService.GetForClient(TestUserContext.ClientID, ContractId);
                else
                {
                    contract = contractService.GetForClientByProjectId(
                        TestUserContext.ClientID, projectId);
                    ContractId = contract.ContractID;
                }

                Project project = ServiceFactory.CreateProjectService().GetProject(contract.ProjectID);
                User freelancer = ServiceFactory.CreateUserService().GetUser(contract.FreelancerID);
                if (project == null || freelancer == null)
                    throw new InvalidOperationException("The contract details could not be found.");

                BindPaymentBreakdown(contract.AgreedAmount);
                lblProjectTitle.Text = Server.HtmlEncode(project.Title);
                lblProjectDescription.Text = Server.HtmlEncode(project.Description);
                string freelancerProfileUrl = "ViewFreelancerProfile.aspx?id=" +
                    contract.FreelancerID.ToString(CultureInfo.InvariantCulture) + "&viewer=client";
                lnkFreelancerProfile.Text = Server.HtmlEncode(freelancer.FullName);
                lnkFreelancerProfile.NavigateUrl = freelancerProfileUrl;
                lblSummaryProject.Text = Server.HtmlEncode(project.Title);
                lnkSummaryFreelancerProfile.Text = Server.HtmlEncode(freelancer.FullName);
                lnkSummaryFreelancerProfile.NavigateUrl = freelancerProfileUrl;

                bool alreadyFunded = ServiceFactory.CreatePaymentService()
                    .GetClientPaymentHistory(TestUserContext.ClientID)
                    .Any(payment => payment.ContractID == contract.ContractID);
                btnFundContract.Enabled = !alreadyFunded &&
                    contract.ApprovalStatus != WorkflowStatuses.ContractCompleted &&
                    contract.ApprovalStatus != WorkflowStatuses.ContractCancelled;
                if (alreadyFunded)
                    ShowMessage("A simulated payment record already exists for this contract.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Simulated payment page failed to load: " + ex);
                btnFundContract.Enabled = false;
                ShowMessage(DatabaseErrorMessageHelper.GetUserMessage(
                    ex, "This contract could not be loaded for simulated funding."));
            }
        }

        private void LoadProposalForFunding(int projectId, int proposalId)
        {
            try
            {
                ProjectId = projectId;
                ProposalId = proposalId;
                ContractId = 0;
                Project project = ServiceFactory.CreateProjectService().GetClientProject(
                    TestUserContext.ClientID, projectId);
                Proposal proposal = ServiceFactory.CreateProposalService()
                    .GetProjectProposals(TestUserContext.ClientID, projectId, "date")
                    .FirstOrDefault(item => item.ProposalID == proposalId);
                User freelancer = proposal == null ? null :
                    ServiceFactory.CreateUserService().GetUser(proposal.FreelancerID);
                if (project == null || proposal == null || freelancer == null ||
                    project.Status != WorkflowStatuses.ProjectOpen ||
                    proposal.Status != WorkflowStatuses.ProposalPending)
                    throw new InvalidOperationException("This project or proposal is no longer available. No freelancer has been selected.");

                if (ServiceFactory.CreateContractService()
                    .GetActiveForFreelancer(proposal.FreelancerID).Count >= 3)
                {
                    throw new InvalidOperationException(
                        "This freelancer already has 3 active projects. They must complete one before accepting another.");
                }

                BindPaymentBreakdown(proposal.BidAmount);
                lblProjectTitle.Text = Server.HtmlEncode(project.Title);
                lblProjectDescription.Text = Server.HtmlEncode(project.Description);
                string freelancerProfileUrl = "ViewFreelancerProfile.aspx?id=" +
                    proposal.FreelancerID.ToString(CultureInfo.InvariantCulture) + "&viewer=client";
                lnkFreelancerProfile.Text = Server.HtmlEncode(freelancer.FullName);
                lnkFreelancerProfile.NavigateUrl = freelancerProfileUrl;
                lblSummaryProject.Text = Server.HtmlEncode(project.Title);
                lnkSummaryFreelancerProfile.Text = Server.HtmlEncode(freelancer.FullName);
                lnkSummaryFreelancerProfile.NavigateUrl = freelancerProfileUrl;
                btnFundContract.Enabled = true;
                btnFundContract.Text = "Simulate Payment and Confirm Hire";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Proposal funding page failed to load: " + ex);
                btnFundContract.Enabled = false;
                ShowMessage(ex is InvalidOperationException ? ex.Message :
                    DatabaseErrorMessageHelper.GetUserMessage(
                        ex, "This proposal could not be loaded. Check the database connection and try again."));
            }
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = Server.HtmlEncode(message);
            lblMessage.Visible = true;
        }

        private void BindPaymentBreakdown(decimal contractAmount)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo("en-IN");
            decimal gstAmount = SimulatedGstCalculator.CalculateTax(contractAmount);
            decimal clientPlatformFee = SimulatedPlatformFeeCalculator.CalculateFee(contractAmount);
            decimal freelancerPlatformFee = SimulatedPlatformFeeCalculator.CalculateFee(contractAmount);
            decimal clientTotal = contractAmount + gstAmount + clientPlatformFee;
            lblAmount.Text = Server.HtmlEncode(contractAmount.ToString("C2", culture));
            lblGstRate.Text = SimulatedGstCalculator.DemoRate.ToString("0.##", CultureInfo.InvariantCulture);
            lblGstAmount.Text = Server.HtmlEncode(gstAmount.ToString("C2", culture));
            lblClientFeeRate.Text = SimulatedPlatformFeeCalculator.DemoRate.ToString("0.##", CultureInfo.InvariantCulture);
            lblClientFeeAmount.Text = Server.HtmlEncode(clientPlatformFee.ToString("C2", culture));
            lblFreelancerFeeRate.Text = SimulatedPlatformFeeCalculator.DemoRate.ToString("0.##", CultureInfo.InvariantCulture);
            lblFreelancerFeeAmount.Text = Server.HtmlEncode(freelancerPlatformFee.ToString("C2", culture));
            lblClientTotal.Text = Server.HtmlEncode(clientTotal.ToString("C2", culture));
            lblFreelancerNet.Text = Server.HtmlEncode((contractAmount - freelancerPlatformFee).ToString("C2", culture));
            lblSummaryAmount.Text = Server.HtmlEncode(contractAmount.ToString("C2", culture));
            lblSummaryGstRate.Text = SimulatedGstCalculator.DemoRate.ToString("0.##", CultureInfo.InvariantCulture);
            lblSummaryGst.Text = Server.HtmlEncode(gstAmount.ToString("C2", culture));
            lblSummaryClientFeeRate.Text = SimulatedPlatformFeeCalculator.DemoRate.ToString("0.##", CultureInfo.InvariantCulture);
            lblSummaryClientFee.Text = Server.HtmlEncode(clientPlatformFee.ToString("C2", culture));
            lblSummaryFreelancerNet.Text = Server.HtmlEncode((contractAmount - freelancerPlatformFee).ToString("C2", culture));
            lblSummaryTotal.Text = Server.HtmlEncode(clientTotal.ToString("C2", culture));
        }
    }
}
