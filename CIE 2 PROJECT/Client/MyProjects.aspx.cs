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
    public partial class MyProjects : Page
    {
        private List<ProjectRow> projects = new List<ProjectRow>();

        private sealed class ProjectRow
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Budget { get; set; }
            public int Proposals { get; set; }
            public string Deadline { get; set; }
            public string Status { get; set; }
            public string StatusLabel { get; set; }
            public string ActionText { get; set; }
            public string ActionUrl { get; set; }
            public bool CanEdit { get; set; }
            public bool ShowMessageLink { get; set; }
            public string MessageUrl { get; set; }
            public bool ShowReviewStatus { get; set; }
            public string ReviewStatus { get; set; }
            public string ReviewStatusClass { get; set; }
            public string ReviewStatusIcon { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                projects = GetProjectsFromServices();
                lblLoadError.Visible = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Client project list failed: " + ex);
                projects = new List<ProjectRow>();
                lblLoadError.Text = "Unable to load projects. Please check the database connection.";
                lblLoadError.Visible = true;
            }

            if (!IsPostBack)
                BindProjects(projects);
        }

        private List<ProjectRow> GetProjectsFromServices()
        {
            IList<Project> clientProjects =
                ServiceFactory.CreateProjectService().GetClientProjects(TestUserContext.ClientID);
            IProposalService proposalService = ServiceFactory.CreateProposalService();
            IList<Contract> contracts = ServiceFactory.CreateContractService()
                .GetForClient(TestUserContext.ClientID);
            IReviewService reviewService = ServiceFactory.CreateReviewService();
            List<ProjectRow> result = new List<ProjectRow>();

            foreach (Project project in clientProjects)
            {
                int proposalCount = proposalService.GetProjectProposals(
                    TestUserContext.ClientID, project.ProjectID, "date").Count;

                string actionText;
                string actionUrl;
                GetProjectAction(project, out actionText, out actionUrl);

                Contract projectContract = contracts.FirstOrDefault(item => item.ProjectID == project.ProjectID);

                bool showReviewStatus = project.Status == WorkflowStatuses.ProjectCompleted;
                string reviewStatus = null;
                string reviewStatusClass = null;
                string reviewStatusIcon = null;
                if (showReviewStatus)
                {
                    Contract contract = projectContract;
                    if (contract == null)
                    {
                        reviewStatus = "Rating unavailable";
                        reviewStatusClass = "review-status review-status-unavailable";
                        reviewStatusIcon = "fa-regular fa-circle-question";
                    }
                    else if (reviewService.HasReviewedContract(contract.ContractID, TestUserContext.ClientID))
                    {
                        reviewStatus = "Rating completed";
                        reviewStatusClass = "review-status review-status-complete";
                        reviewStatusIcon = "fa-solid fa-circle-check";
                    }
                    else
                    {
                        reviewStatus = "Rating pending";
                        reviewStatusClass = "review-status review-status-pending";
                        reviewStatusIcon = "fa-regular fa-clock";
                    }
                }

                result.Add(new ProjectRow
                {
                    Id = project.ProjectID,
                    Title = project.Title,
                    Description = project.Description,
                    Budget = string.Format(CultureInfo.GetCultureInfo("en-IN"),
                        "₹{0:N0} - ₹{1:N0}", project.MinBudget, project.MaxBudget),
                    Proposals = proposalCount,
                    Deadline = project.Deadline.HasValue
                        ? project.Deadline.Value.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture)
                        : "Not specified",
                    Status = project.Status,
                    StatusLabel = GetCompactStatus(project.Status),
                    ActionText = actionText,
                    ActionUrl = actionUrl,
                    CanEdit = project.Status == WorkflowStatuses.ProjectOpen,
                    ShowMessageLink = projectContract != null,
                    MessageUrl = projectContract == null ? string.Empty : ResolveUrl(
                        "~/Messages/Messages.aspx?with=" +
                        projectContract.FreelancerID.ToString(CultureInfo.InvariantCulture) + "&projectId=" +
                        project.ProjectID.ToString(CultureInfo.InvariantCulture)),
                    ShowReviewStatus = showReviewStatus,
                    ReviewStatus = reviewStatus,
                    ReviewStatusClass = reviewStatusClass,
                    ReviewStatusIcon = reviewStatusIcon
                });
            }

            return result;
        }

        private static void GetProjectAction(Project project, out string text, out string url)
        {
            string projectId = project.ProjectID.ToString(CultureInfo.InvariantCulture);
            if (string.Equals(project.Status, WorkflowStatuses.ProjectOpen,
                StringComparison.OrdinalIgnoreCase))
            {
                text = "View Proposals";
                url = "ProjectProposal.aspx?projectId=" + projectId;
                return;
            }

            if (string.Equals(project.Status, WorkflowStatuses.ProjectInProgress,
                StringComparison.OrdinalIgnoreCase))
            {
                text = "Review Work";
                url = "ReviewSubmittedWork.aspx?projectId=" + projectId;
                return;
            }

            if (string.Equals(project.Status, WorkflowStatuses.ProjectCompleted,
                StringComparison.OrdinalIgnoreCase))
            {
                text = "View Results";
                url = "ReviewSubmittedWork.aspx?projectId=" + projectId;
                return;
            }

            text = "View Project";
            url = "ProjectDetails.aspx?id=" + projectId;
        }

        private void BindProjects(IList<ProjectRow> data)
        {
            rptProjects.DataSource = data;
            rptProjects.DataBind();
            pnlEmpty.Visible = data.Count == 0;
        }

        protected void Filter_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;
            string filter = button.CommandArgument;
            SetActiveTab(button);

            if (string.Equals(filter, "All", StringComparison.OrdinalIgnoreCase))
            {
                BindProjects(projects);
                return;
            }

            List<ProjectRow> filteredProjects = projects
                .Where(p => string.Equals(p.Status, filter, StringComparison.OrdinalIgnoreCase))
                .ToList();
            BindProjects(filteredProjects);
        }

        protected void CancelProject_Click(object sender, EventArgs e)
        {
            LinkButton button = sender as LinkButton;
            int projectId;
            if (button == null || !int.TryParse(button.CommandArgument, out projectId) || projectId <= 0)
            {
                FlashMessageStore.Error("A valid project is required to cancel.");
                Response.Redirect("MyProjects.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            try
            {
                ServiceFactory.CreateProjectService().CancelProject(
                    TestUserContext.ClientID, projectId);
                FlashMessageStore.Success("Project cancelled successfully. Its status is now visible in My Projects.");
                Response.Redirect("MyProjects.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Client project cancellation failed: " + ex);
                FlashMessageStore.Error("This project could not be cancelled. Check its status and try again.");
                Response.Redirect("MyProjects.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        private void SetActiveTab(LinkButton activeButton)
        {
            lnkAll.CssClass = "project-tab";
            lnkOpen.CssClass = "project-tab";
            lnkProgress.CssClass = "project-tab";
            lnkCompleted.CssClass = "project-tab";
            lnkCancelled.CssClass = "project-tab";
            activeButton.CssClass = "project-tab active";
        }

        protected string GetStatusClass(string status)
        {
            if (status == WorkflowStatuses.ProjectOpen)
                return "status-badge status-open";
            if (status == WorkflowStatuses.ProjectInProgress)
                return "status-badge status-progress";
            if (status == WorkflowStatuses.ProjectCompleted)
                return "status-badge status-completed";
            if (status == WorkflowStatuses.ProjectCancelled)
                return "status-badge status-cancelled";
            return "status-badge";
        }

        private static string GetCompactStatus(string status)
        {
            if (string.Equals(status, WorkflowStatuses.ProjectOpen, StringComparison.OrdinalIgnoreCase))
                return "Open";
            if (string.Equals(status, WorkflowStatuses.ProjectInProgress, StringComparison.OrdinalIgnoreCase))
                return "In progress";
            if (string.Equals(status, WorkflowStatuses.ProjectCompleted, StringComparison.OrdinalIgnoreCase))
                return "Completed";
            if (string.Equals(status, WorkflowStatuses.ProjectCancelled, StringComparison.OrdinalIgnoreCase))
                return "Cancelled";
            return status ?? "Unknown";
        }
    }
}
