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
    public partial class ClientHome : Page
    {
        private sealed class ProjectCard
        {
            public string Title { get; set; }
            public string Category { get; set; }
            public string Budget { get; set; }
            public string Status { get; set; }
            public string StatusClass { get; set; }
            public string ProposalCount { get; set; }
            public string ProjectUrl { get; set; }
        }

        private sealed class FreelancerCard
        {
            public string Name { get; set; }
            public string Initials { get; set; }
            public string Rating { get; set; }
            public string ReviewCount { get; set; }
            public string Bio { get; set; }
            public string Skills { get; set; }
            public string ProfileUrl { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindDashboard();
        }

        private void BindDashboard()
        {
            try
            {
                int clientId = TestUserContext.ClientID;
                IUserService userService = ServiceFactory.CreateUserService();
                User client = userService.GetUser(clientId);
                if (client == null)
                    throw new InvalidOperationException("The client profile could not be found.");

                IList<Project> projects = ServiceFactory.CreateProjectService()
                    .GetClientProjects(clientId);
                IList<Contract> contracts = ServiceFactory.CreateContractService()
                    .GetForClient(clientId);
                IList<Payment> payments = ServiceFactory.CreatePaymentService()
                    .GetClientPaymentHistory(clientId);

                litWelcomeName.Text = client.FullName;
                litProjectsPosted.Text = projects.Count.ToString(CultureInfo.InvariantCulture);
                litOpenProjects.Text = projects.Count(project =>
                    project.Status == WorkflowStatuses.ProjectOpen)
                    .ToString(CultureInfo.InvariantCulture);
                litActiveProjects.Text = contracts.Count(contract =>
                    contract.ApprovalStatus != WorkflowStatuses.ContractCompleted &&
                    contract.ApprovalStatus != WorkflowStatuses.ContractCancelled)
                    .ToString(CultureInfo.InvariantCulture);
                decimal released = payments.Where(payment =>
                    payment.PaymentStatus == WorkflowStatuses.PaymentReleased)
                    .Sum(payment => payment.ClientTotalAmount);
                litReleasedPayments.Text = released.ToString("C0", CultureInfo.GetCultureInfo("en-IN"));

                BindProjects(projects);
                BindFreelancers(userService.GetUsersByRole("Freelancer"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Client dashboard failed to load: " + ex);
                litDashboardError.Text = "We could not load your dashboard right now. Please refresh and try again.";
                pnlDashboardError.Visible = true;
            }
        }

        private void BindProjects(IList<Project> projects)
        {
            IProposalService proposalService = ServiceFactory.CreateProposalService();
            List<ProjectCard> rows = projects
                .OrderByDescending(project => project.CreatedAt)
                .Take(5)
                .Select(project => new ProjectCard
                {
                    Title = project.Title,
                    Category = project.Category,
                    Budget = string.Format(CultureInfo.GetCultureInfo("en-IN"),
                        "₹{0:N0} - ₹{1:N0}", project.MinBudget, project.MaxBudget),
                    Status = project.Status,
                    StatusClass = GetProjectStatusClass(project.Status),
                    ProposalCount = proposalService.GetProposalCount(project.ProjectID)
                        .ToString(CultureInfo.InvariantCulture),
                    ProjectUrl = "ProjectDetails.aspx?id=" + project.ProjectID
                })
                .ToList();

            rptRecentProjects.DataSource = rows;
            rptRecentProjects.DataBind();
            pnlNoProjects.Visible = rows.Count == 0;
        }

        private void BindFreelancers(IList<User> users)
        {
            IReviewService reviewService = ServiceFactory.CreateReviewService();
            List<FreelancerCard> rows = users.Select(user => new FreelancerCard
                {
                    Name = user.FullName,
                    Initials = GetInitials(user.FullName),
                    Rating = reviewService.GetAverageRating(user.UserID)
                        .ToString("0.0", CultureInfo.InvariantCulture),
                    ReviewCount = reviewService.GetReviewCount(user.UserID)
                        .ToString(CultureInfo.InvariantCulture),
                    Bio = string.IsNullOrWhiteSpace(user.Bio)
                        ? "This freelancer has not added a profile introduction yet."
                        : user.Bio,
                    Skills = GetSkillsSummary(user.Skills),
                    ProfileUrl = "ViewFreelancerProfile.aspx?id=" + user.UserID.ToString(CultureInfo.InvariantCulture)
                })
                .OrderByDescending(freelancer => Convert.ToDecimal(freelancer.Rating,
                    CultureInfo.InvariantCulture))
                .ThenByDescending(freelancer => Convert.ToInt32(freelancer.ReviewCount,
                    CultureInfo.InvariantCulture))
                .Take(4)
                .ToList();

            rptFreelancers.DataSource = rows;
            rptFreelancers.DataBind();
            pnlNoFreelancers.Visible = rows.Count == 0;
        }

        private static string GetProjectStatusClass(string status)
        {
            if (status == WorkflowStatuses.ProjectCompleted)
                return "dashboard-status-complete";
            if (status == WorkflowStatuses.ProjectInProgress)
                return "dashboard-status-active";
            if (status == WorkflowStatuses.ProjectCancelled)
                return "dashboard-status-cancelled";
            return "dashboard-status-open";
        }

        private static string GetSkillsSummary(string skills)
        {
            string[] items = (skills ?? string.Empty)
                .Split(new[] { ',', ';', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(skill => skill.Trim())
                .Where(skill => skill.Length > 0)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Take(4)
                .ToArray();
            return items.Length == 0 ? "Skills not added" : string.Join(" · ", items);
        }

        private static string GetInitials(string name)
        {
            string[] parts = (name ?? string.Empty).Split(
                new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return "?";
            if (parts.Length == 1)
                return parts[0].Substring(0, 1).ToUpperInvariant();
            return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1))
                .ToUpperInvariant();
        }
    }
}
