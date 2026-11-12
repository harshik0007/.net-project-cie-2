using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class ProjectProposal : Page
    {
        private int ProjectId
        {
            get
            {
                int id;
                string value = Request.QueryString["projectId"];
                if (string.IsNullOrWhiteSpace(value))
                    value = Request.QueryString["id"];

                return int.TryParse(value, out id) && id > 0 ? id : 0;
            }
        }

        private sealed class ProposalItem
        {
            public int ProposalID { get; set; }
            public string FreelancerName { get; set; }
            public string FreelancerProfileUrl { get; set; }
            public string MessageUrl { get; set; }
            public string CoverLetter { get; set; }
            public string Skills { get; set; }
            public string ProposedPrice { get; set; }
            public string EstimatedClientTotal { get; set; }
            public int DeliveryTime { get; set; }
            public string SubmittedOn { get; set; }
            public string Status { get; set; }
            public bool CanHire { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lnkBack.NavigateUrl = "ProjectDetails.aspx?id=" + ProjectId;
            if (IsPostBack)
                return;
            if (ProjectId <= 0)
            {
                pnlProjectSummary.Visible = false;
                ShowPageError("A valid project ID was not provided.");
                return;
            }

            if (LoadProject())
                LoadProposals();
        }

        private bool LoadProject()
        {
            try
            {
                Project project = ServiceFactory.CreateProjectService()
                    .GetClientProject(TestUserContext.ClientID, ProjectId);
                if (RouteForProjectStatus(project))
                    return false;

                litProjectTitle.Text = Encode(project.Title);
                litProjectDescription.Text = Encode(project.Description);
                litProjectSkills.Text = GetSkills(project.RequiredSkills);
                litProjectBudget.Text = Server.HtmlEncode(string.Format(
                    CultureInfo.GetCultureInfo("en-IN"), "₹{0:N0} - ₹{1:N0}",
                    project.MinBudget, project.MaxBudget));
                litProjectDeadline.Text = Server.HtmlEncode(project.Deadline.HasValue
                    ? project.Deadline.Value.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)
                    : "Not specified");
                litProjectStatus.Text = Encode(project.Status);
                bool proposalHistoryView = IsProposalHistoryView() &&
                    string.Equals(project.Status, WorkflowStatuses.ProjectInProgress,
                        StringComparison.OrdinalIgnoreCase);
                lblProposalHeading.Text = proposalHistoryView ? "Proposal history" : "Proposals";
                lblProposalGuidance.Text = proposalHistoryView
                    ? "This project already has a selected freelancer. Review the proposals submitted for this project here."
                    : "Review proposals and select the freelancer who best fits your project.";
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Client proposal page project load failed: " + ex);
                pnlProjectSummary.Visible = false;
                ShowPageError("Project not found or access is denied.");
                return false;
            }
        }

        private void LoadProposals()
        {
            try
            {
                Project project = ServiceFactory.CreateProjectService()
                    .GetClientProject(TestUserContext.ClientID, ProjectId);
                if (RouteForProjectStatus(project))
                    return;

                IList<Proposal> records = ServiceFactory.CreateProposalService()
                    .GetProjectProposals(TestUserContext.ClientID, ProjectId, ddlSort.SelectedValue);
                IUserService users = ServiceFactory.CreateUserService();
                List<ProposalItem> items = new List<ProposalItem>();

                foreach (Proposal proposal in records)
                {
                    User freelancer = users.GetUser(proposal.FreelancerID);
                    items.Add(new ProposalItem
                    {
                        ProposalID = proposal.ProposalID,
                        FreelancerName = freelancer == null
                            ? "Freelancer #" + proposal.FreelancerID
                            : freelancer.FullName,
                        FreelancerProfileUrl = "ViewFreelancerProfile.aspx?id=" +
                            proposal.FreelancerID.ToString(CultureInfo.InvariantCulture) + "&viewer=client",
                        MessageUrl = ResolveUrl("~/Messages/Messages.aspx?with=" +
                            proposal.FreelancerID.ToString(CultureInfo.InvariantCulture) + "&projectId=" +
                            project.ProjectID.ToString(CultureInfo.InvariantCulture)),
                        CoverLetter = proposal.CoverLetter,
                        Skills = project.RequiredSkills,
                        ProposedPrice = proposal.BidAmount.ToString(
                            "C0", CultureInfo.GetCultureInfo("en-IN")),
                        EstimatedClientTotal = (proposal.BidAmount +
                            SimulatedGstCalculator.CalculateTax(proposal.BidAmount) +
                            SimulatedPlatformFeeCalculator.CalculateFee(proposal.BidAmount)).ToString(
                                "C2", CultureInfo.GetCultureInfo("en-IN")),
                        DeliveryTime = proposal.EstimatedDays,
                        SubmittedOn = proposal.SubmittedAt.ToString(
                            "dd MMM yyyy", CultureInfo.InvariantCulture),
                        Status = proposal.Status,
                        CanHire = string.Equals(project.Status, WorkflowStatuses.ProjectOpen,
                            StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(proposal.Status, WorkflowStatuses.ProposalPending,
                                StringComparison.OrdinalIgnoreCase)
                    });
                }

                rptProposals.DataSource = items;
                rptProposals.DataBind();
                lblProposalCount.Text = items.Count.ToString(CultureInfo.InvariantCulture);
                pnlNoProposals.Visible = items.Count == 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Client proposal list load failed: " + ex);
                ShowPageError("Unable to load proposals. Please try again.");
            }
        }

        private bool RouteForProjectStatus(Project project)
        {
            if (string.Equals(project.Status, WorkflowStatuses.ProjectInProgress,
                StringComparison.OrdinalIgnoreCase) && IsProposalHistoryView())
                return false;

            if (string.Equals(project.Status, WorkflowStatuses.ProjectInProgress,
                StringComparison.OrdinalIgnoreCase) ||
                string.Equals(project.Status, WorkflowStatuses.ProjectCompleted,
                    StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("ReviewSubmittedWork.aspx?projectId=" + ProjectId, false);
                Context.ApplicationInstance.CompleteRequest();
                return true;
            }

            if (!string.Equals(project.Status, WorkflowStatuses.ProjectOpen,
                StringComparison.OrdinalIgnoreCase))
            {
                pnlProjectSummary.Visible = false;
                ShowPageError("This project is no longer accepting proposals.");
                return true;
            }

            return false;
        }

        private bool IsProposalHistoryView()
        {
            return string.Equals(Request.QueryString["view"], "proposals",
                StringComparison.OrdinalIgnoreCase);
        }

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProposals();
        }

        protected void rptProposals_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int proposalId;
            if (!string.Equals(e.CommandName, "Hire", StringComparison.OrdinalIgnoreCase))
                return;

            if (!int.TryParse(Convert.ToString(e.CommandArgument), out proposalId) || proposalId <= 0)
            {
                ShowPageError("Invalid proposal.");
                return;
            }

            Response.Redirect(
                "HireFreelancer.aspx?projectId=" + ProjectId + "&proposalId=" + proposalId,
                false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected string GetSkills(string skills)
        {
            if (string.IsNullOrWhiteSpace(skills))
                return "<span>No skills specified</span>";

            string[] skillList = skills.Split(
                new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string result = string.Empty;
            foreach (string skill in skillList)
                result += "<span>" + Server.HtmlEncode(skill.Trim()) + "</span>";
            return result;
        }

        private string Encode(object value)
        {
            return value == null ? string.Empty : Server.HtmlEncode(Convert.ToString(value));
        }

        private void ShowPageError(string message)
        {
            lblPageMessage.Text = Server.HtmlEncode(message);
            lblPageMessage.Visible = true;
        }
    }
}
