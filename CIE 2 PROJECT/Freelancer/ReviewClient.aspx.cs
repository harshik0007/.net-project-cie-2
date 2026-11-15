using System;
using System.Globalization;
using System.Web.UI;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class ReviewClient : Page
    {
        private int ContractId
        {
            get { return ViewState["ReviewContractId"] == null ? 0 : (int)ViewState["ReviewContractId"]; }
            set { ViewState["ReviewContractId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadContract();
        }

        protected void SubmitReview_Click(object sender, EventArgs e)
        {
            Page.Validate("ClientReview");
            if (!Page.IsValid)
                return;

            int rating;
            if (!int.TryParse(ddlRating.SelectedValue, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out rating))
            {
                ShowMessage("Choose a rating from 1 to 5.");
                return;
            }

            try
            {
                Contract contract = ServiceFactory.CreateContractService().GetForFreelancer(
                    TestUserContext.FreelancerID, ContractId);
                ServiceFactory.CreateReviewService().SubmitReview(
                    ContractId, TestUserContext.FreelancerID, contract.ClientID,
                    "Freelancer", rating, txtReview.Text);
                FlashMessageStore.Success("Your review was submitted successfully.");
                Response.Redirect("MyProposals.aspx?filter=reviews", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Freelancer review submission failed: " + ex);
                ShowMessage("The review could not be saved. Reviews are available once per participant after contract completion.");
            }
        }

        private void LoadContract()
        {
            int contractId;
            if (!int.TryParse(Request.QueryString["contractId"], out contractId) || contractId <= 0)
            {
                ShowMessage("A valid completed contract is required.");
                return;
            }
            ContractId = contractId;

            try
            {
                IContractService contractService = ServiceFactory.CreateContractService();
                Contract contract = contractService.GetForFreelancer(
                    TestUserContext.FreelancerID, ContractId);
                if (contract.ApprovalStatus != WorkflowStatuses.ContractCompleted)
                    throw new InvalidOperationException("Only completed contracts can be reviewed.");
                if (ServiceFactory.CreateReviewService().HasReviewedContract(
                    ContractId, TestUserContext.FreelancerID))
                {
                    ShowMessage("You have already reviewed this contract.");
                    return;
                }

                Project project = ServiceFactory.CreateProjectService().GetProject(contract.ProjectID);
                User client = ServiceFactory.CreateUserService().GetUser(contract.ClientID);
                if (project == null || client == null)
                    throw new InvalidOperationException("The contract details could not be found.");
                lblProject.Text = Server.HtmlEncode(project.Title);
                lnkClientProfile.Text = Server.HtmlEncode(client.FullName);
                lnkClientProfile.NavigateUrl = "ViewClientProfile.aspx?id=" +
                    contract.ClientID.ToString(CultureInfo.InvariantCulture) + "&viewer=freelancer";
                pnlReview.Visible = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Freelancer client review page failed to load: " + ex);
                ShowMessage("This completed contract could not be loaded for review.");
            }
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = Server.HtmlEncode(message);
            lblMessage.Visible = true;
        }
    }
}
