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
    public partial class MyProposals : Page
    {
        private sealed class ProposalItem
        {
            public int ProjectID { get; set; }
            public string Title { get; set; }
            public string Client { get; set; }
            public string ClientProfileUrl { get; set; }
            public string MessageUrl { get; set; }
            public string PostedOn { get; set; }
            public string Description { get; set; }
            public string Price { get; set; }
            public string EstimatedPayout { get; set; }
            public string PayoutLabel { get; set; }
            public string Delivery { get; set; }
            public string SubmittedOn { get; set; }
            public string StatusLabel { get; set; }
            public string IconClass { get; set; }
            public string IconFa { get; set; }
            public string BadgeClass { get; set; }
            public string BadgeIcon { get; set; }
            public string ActionText { get; set; }
            public string ActionUrl { get; set; }
            public bool ShowActionLink { get; set; }
            public bool ShowContractUnavailable { get; set; }
            public bool ShowReviewStatus { get; set; }
            public bool CanReviewClient { get; set; }
            public string ReviewStatus { get; set; }
            public string ReviewStateClass { get; set; }
            public string ReviewUrl { get; set; }
        }

        private sealed class PreviousReviewItem
        {
            public string ProjectTitle { get; set; }
            public string ClientName { get; set; }
            public string ClientProfileUrl { get; set; }
            public string Rating { get; set; }
            public string CreatedAt { get; set; }
            public string ReviewText { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string requestedFilter = (Request.QueryString["filter"] ?? "").Trim().ToLowerInvariant();
                LoadPage(IsSupportedFilter(requestedFilter) ? requestedFilter : "all");
            }
        }

        protected void FilterTab_Click(object sender, EventArgs e)
        {
            LinkButton button = sender as LinkButton;
            if (button == null)
                return;

            string filter = button.CommandArgument;
            if (!IsSupportedFilter(filter))
                filter = "all";

            LoadPage(filter);
        }

        private void LoadPage(string filter)
        {
            lblPageMessage.Visible = false;
            try
            {
                IList<Proposal> records = ServiceFactory.CreateProposalService()
                    .GetFreelancerProposals(TestUserContext.FreelancerID);
                IList<Contract> contracts = ServiceFactory.CreateContractService()
                    .GetForFreelancerHistory(TestUserContext.FreelancerID);
                HashSet<int> proposalIds = new HashSet<int>(records.Select(proposal => proposal.ProposalID));
                HashSet<int> completedProposalIds = new HashSet<int>(contracts
                    .Where(contract => string.Equals(contract.ApprovalStatus,
                        WorkflowStatuses.ContractCompleted, StringComparison.OrdinalIgnoreCase) &&
                        proposalIds.Contains(contract.ProposalID))
                    .Select(contract => contract.ProposalID));
                btnAll.Text = "All (" + records.Count + ")";
                btnPending.Text = "Pending (" + CountStatus(records, WorkflowStatuses.ProposalPending) + ")";
                btnAccepted.Text = "Accepted (" + records.Count(proposal =>
                    string.Equals(proposal.Status, WorkflowStatuses.ProposalAccepted,
                        StringComparison.OrdinalIgnoreCase) && !completedProposalIds.Contains(proposal.ProposalID)) + ")";
                btnRejected.Text = "Rejected (" + CountStatus(records, WorkflowStatuses.ProposalRejected) + ")";

                IProjectService projectService = ServiceFactory.CreateProjectService();
                IUserService userService = ServiceFactory.CreateUserService();
                IReviewService reviewService = ServiceFactory.CreateReviewService();
                IList<Payment> paymentHistory = ServiceFactory.CreatePaymentService()
                    .GetFreelancerPaymentHistory(TestUserContext.FreelancerID);
                IList<Review> previousReviews = reviewService.GetReviewsGivenByUser(
                    TestUserContext.FreelancerID, "Freelancer");
                btnPreviousReviews.Text = "Previous Reviews (" + previousReviews.Count + ")";

                if (filter == "reviews")
                {
                    pnlProposalSection.Visible = false;
                    pnlPreviousReviews.Visible = true;
                    pnlEmpty.Visible = false;
                    List<PreviousReviewItem> reviewItems = new List<PreviousReviewItem>();
                    foreach (Review review in previousReviews)
                    {
                        Contract contract = contracts.FirstOrDefault(
                            record => record.ContractID == review.ContractID);
                        if (contract == null || contract.FreelancerID != TestUserContext.FreelancerID ||
                            review.ReviewerRole != "Freelancer")
                            continue;

                        Project project = projectService.GetProject(contract.ProjectID);
                        if (project == null)
                            continue;
                        User client = userService.GetUser(review.RevieweeID);
                        reviewItems.Add(new PreviousReviewItem
                        {
                            ProjectTitle = project.Title,
                            ClientName = client == null ? "Client #" + review.RevieweeID : client.FullName,
                            ClientProfileUrl = "ViewClientProfile.aspx?id=" +
                                review.RevieweeID.ToString(CultureInfo.InvariantCulture) + "&viewer=freelancer",
                            Rating = review.Rating.ToString(CultureInfo.InvariantCulture),
                            CreatedAt = review.CreatedAt.ToString("dd MMM yyyy", CultureInfo.InvariantCulture),
                            ReviewText = string.IsNullOrWhiteSpace(review.ReviewText)
                                ? "No written feedback was included."
                                : review.ReviewText
                        });
                    }

                    rptPreviousReviews.DataSource = reviewItems;
                    rptPreviousReviews.DataBind();
                    pnlNoPreviousReviews.Visible = reviewItems.Count == 0;
                    rptProposals.DataSource = null;
                    rptProposals.DataBind();
                    SetActiveTab(filter);
                    return;
                }

                pnlProposalSection.Visible = true;
                pnlPreviousReviews.Visible = false;
                pnlNoPreviousReviews.Visible = false;
                List<ProposalItem> items = new List<ProposalItem>();
                foreach (Proposal proposal in records)
                {
                    Contract contract = contracts.FirstOrDefault(
                        record => record.ProposalID == proposal.ProposalID);
                    bool isCompleted = contract != null && string.Equals(
                        contract.ApprovalStatus, WorkflowStatuses.ContractCompleted,
                        StringComparison.OrdinalIgnoreCase);

                    if (filter == "completed" && !isCompleted)
                        continue;
                    if (filter == "accepted" &&
                        (!string.Equals(proposal.Status, WorkflowStatuses.ProposalAccepted,
                            StringComparison.OrdinalIgnoreCase) || isCompleted))
                        continue;
                    if (filter != "all" && filter != "completed" && filter != "accepted" &&
                        !string.Equals(proposal.Status, filter, StringComparison.OrdinalIgnoreCase))
                        continue;

                    Project project = projectService.GetProject(proposal.ProjectID);
                    if (project == null)
                        continue;
                    User client = userService.GetUser(project.ClientID);
                    Payment payment = contract == null ? null : paymentHistory.FirstOrDefault(
                        record => record.ContractID == contract.ContractID);
                    decimal expectedPayout = payment == null
                        ? proposal.BidAmount - SimulatedPlatformFeeCalculator.CalculateFee(proposal.BidAmount)
                        : payment.FreelancerNetAmount;
                    ProposalItem item = new ProposalItem
                    {
                        ProjectID = proposal.ProjectID,
                        Title = project.Title,
                        Client = client == null ? "Client #" + project.ClientID : client.FullName,
                        ClientProfileUrl = "ViewClientProfile.aspx?id=" +
                            project.ClientID.ToString(CultureInfo.InvariantCulture) + "&viewer=freelancer",
                        MessageUrl = ResolveUrl("~/Messages/Messages.aspx?with=" +
                            project.ClientID.ToString(CultureInfo.InvariantCulture) + "&projectId=" +
                            project.ProjectID.ToString(CultureInfo.InvariantCulture)),
                        PostedOn = project.CreatedAt.ToString("dd MMM yyyy", CultureInfo.InvariantCulture),
                        Description = project.Description,
                        Price = proposal.BidAmount.ToString("C0", CultureInfo.GetCultureInfo("en-IN")),
                        EstimatedPayout = expectedPayout.ToString("C2", CultureInfo.GetCultureInfo("en-IN")),
                        PayoutLabel = payment == null || payment.FreelancerPlatformFeeRate > 0m
                            ? "Estimated payout after " + SimulatedPlatformFeeCalculator.DemoRate.ToString("0.##", CultureInfo.InvariantCulture) + "% fee"
                            : "Payout on release",
                        Delivery = proposal.EstimatedDays + " Days",
                        SubmittedOn = proposal.SubmittedAt.ToString("dd MMM yyyy", CultureInfo.InvariantCulture),
                        ActionText = "View Project",
                        ActionUrl = "ProjectDetails.aspx?id=" + proposal.ProjectID,
                        ShowActionLink = true
                    };

                    if (isCompleted)
                    {
                        bool hasReviewedClient = reviewService.HasReviewedContract(
                            contract.ContractID, TestUserContext.FreelancerID);
                        item.StatusLabel = "Completed";
                        item.BadgeClass = "badge-completed";
                        item.BadgeIcon = "fa-regular fa-circle-check";
                        item.ShowReviewStatus = true;
                        item.CanReviewClient = !hasReviewedClient;
                        item.ReviewStatus = hasReviewedClient ? "Review submitted" : "Review pending";
                        item.ReviewStateClass = hasReviewedClient ? "review-complete" : "review-pending";
                        item.ReviewUrl = "ReviewClient.aspx?contractId=" + contract.ContractID;
                        item.ActionText = "View Project";
                        item.ActionUrl = "ProjectDetails.aspx?id=" + proposal.ProjectID;
                    }
                    else if (string.Equals(proposal.Status, WorkflowStatuses.ProposalAccepted,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        if (contract == null)
                        {
                            item.ShowActionLink = false;
                            item.ShowContractUnavailable = true;
                        }
                        else
                        {
                            item.ActionText = "Continue Work";
                            item.ActionUrl = "SubmitWork.aspx?contractId=" + contract.ContractID;
                        }
                    }

                    if (!isCompleted)
                        SetStatusAppearance(item, proposal.Status);
                    SetCategoryAppearance(item, project.Category);
                    items.Add(item);
                }

                btnCompleted.Text = "Completed (" + completedProposalIds.Count + ")";

                rptProposals.DataSource = items;
                rptProposals.DataBind();
                pnlEmpty.Visible = items.Count == 0;
                rptPreviousReviews.DataSource = null;
                rptPreviousReviews.DataBind();
                SetEmptyState(filter);
                SetActiveTab(filter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Freelancer proposals failed to load: " + ex);
                rptProposals.DataSource = null;
                rptProposals.DataBind();
                rptPreviousReviews.DataSource = null;
                rptPreviousReviews.DataBind();
                pnlProposalSection.Visible = filter != "reviews";
                pnlPreviousReviews.Visible = filter == "reviews";
                pnlNoPreviousReviews.Visible = false;
                pnlEmpty.Visible = false;
                lblPageMessage.Text = "Unable to load your proposals. Please try again.";
                lblPageMessage.Visible = true;
                SetActiveTab(filter);
            }
        }

        private static int CountStatus(IList<Proposal> proposals, string status)
        {
            return proposals.Count(proposal => string.Equals(
                proposal.Status, status, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsSupportedFilter(string filter)
        {
            return filter == "all" || filter == "pending" || filter == "accepted" ||
                filter == "rejected" || filter == "completed" || filter == "reviews";
        }

        private void SetEmptyState(string filter)
        {
            switch (filter)
            {
                case "completed":
                    lblEmptyTitle.Text = "No completed projects yet";
                    lblEmptyDescription.Text = "Completed contracts will appear here after your client approves the work.";
                    break;
                case "pending":
                    lblEmptyTitle.Text = "No pending proposals";
                    lblEmptyDescription.Text = "Proposals awaiting a client decision will appear here.";
                    break;
                case "accepted":
                    lblEmptyTitle.Text = "No active accepted projects";
                    lblEmptyDescription.Text = "Accepted projects that are still in progress will appear here.";
                    break;
                case "rejected":
                    lblEmptyTitle.Text = "No rejected proposals";
                    lblEmptyDescription.Text = "Proposals declined by clients will appear here.";
                    break;
                default:
                    lblEmptyTitle.Text = "No proposals yet";
                    lblEmptyDescription.Text = "Explore available projects and submit your first proposal.";
                    break;
            }
        }

        private void SetStatusAppearance(ProposalItem item, string status)
        {
            switch ((status ?? "").Trim().ToLowerInvariant())
            {
                case "accepted":
                    item.StatusLabel = "Accepted";
                    item.BadgeClass = "badge-accepted";
                    item.BadgeIcon = "fa-regular fa-circle-check";
                    break;
                case "rejected":
                    item.StatusLabel = "Rejected";
                    item.BadgeClass = "badge-rejected";
                    item.BadgeIcon = "fa-regular fa-circle-xmark";
                    break;
                case "pending":
                    item.StatusLabel = "Pending";
                    item.BadgeClass = "badge-pending";
                    item.BadgeIcon = "fa-regular fa-clock";
                    break;
                default:
                    item.StatusLabel = string.IsNullOrWhiteSpace(status) ? "Unknown" : status;
                    item.BadgeClass = "badge-pending";
                    item.BadgeIcon = "fa-regular fa-clock";
                    break;
            }
        }

        private void SetCategoryAppearance(ProposalItem item, string category)
        {
            switch ((category ?? "").Trim().ToLowerInvariant())
            {
                case "game development":
                    item.IconClass = "icon-game";
                    item.IconFa = "fa-solid fa-gamepad";
                    break;
                case "content writing":
                case "writing":
                    item.IconClass = "icon-writing";
                    item.IconFa = "fa-solid fa-file-lines";
                    break;
                case "graphic design":
                case "ui/ux design":
                case "design":
                    item.IconClass = "icon-design";
                    item.IconFa = "fa-solid fa-palette";
                    break;
                case "mobile development":
                    item.IconClass = "icon-mobile";
                    item.IconFa = "fa-solid fa-mobile-screen";
                    break;
                case "database":
                case "data science":
                    item.IconClass = "icon-database";
                    item.IconFa = "fa-solid fa-database";
                    break;
                default:
                    item.IconClass = "icon-code";
                    item.IconFa = "fa-solid fa-code";
                    break;
            }
        }

        private void SetActiveTab(string filter)
        {
            btnAll.CssClass = "tab-btn" + (filter == "all" ? " active" : "");
            btnPending.CssClass = "tab-btn" + (filter == "pending" ? " active" : "");
            btnAccepted.CssClass = "tab-btn" + (filter == "accepted" ? " active" : "");
            btnCompleted.CssClass = "tab-btn" + (filter == "completed" ? " active" : "");
            btnPreviousReviews.CssClass = "tab-btn" + (filter == "reviews" ? " active" : "");
            btnRejected.CssClass = "tab-btn" + (filter == "rejected" ? " active" : "");
        }
    }
}
