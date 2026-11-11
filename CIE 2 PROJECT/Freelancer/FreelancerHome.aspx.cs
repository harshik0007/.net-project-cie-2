using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class FreelancerHome : Page
    {
        private sealed class ProjectCard
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public string Deadline { get; set; }
            public string Budget { get; set; }
            public string ProjectUrl { get; set; }
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
                int freelancerId = TestUserContext.FreelancerID;
                User user = ServiceFactory.CreateUserService().GetUser(freelancerId);
                if (user == null)
                    throw new InvalidOperationException("The freelancer profile could not be found.");

                IList<Contract> contracts = ServiceFactory.CreateContractService()
                    .GetForFreelancerHistory(freelancerId);
                IList<Proposal> proposals = ServiceFactory.CreateProposalService()
                    .GetFreelancerProposals(freelancerId);
                IList<Payment> payments = ServiceFactory.CreatePaymentService()
                    .GetFreelancerPaymentHistory(freelancerId);

                litWelcomeName.Text = user.FullName;
                litActiveContracts.Text = contracts.Count(contract =>
                    contract.ApprovalStatus != WorkflowStatuses.ContractCompleted &&
                    contract.ApprovalStatus != WorkflowStatuses.ContractCancelled)
                    .ToString(CultureInfo.InvariantCulture);
                litPendingProposals.Text = proposals.Count(proposal =>
                    proposal.Status == WorkflowStatuses.ProposalPending)
                    .ToString(CultureInfo.InvariantCulture);
                litCompletedProjects.Text = contracts.Count(contract =>
                    contract.ApprovalStatus == WorkflowStatuses.ContractCompleted)
                    .ToString(CultureInfo.InvariantCulture);
                decimal released = payments.Where(payment =>
                    payment.PaymentStatus == WorkflowStatuses.PaymentReleased)
                    .Sum(payment => payment.FreelancerNetAmount);
                litReleasedPayments.Text = released.ToString("C0", CultureInfo.GetCultureInfo("en-IN"));

                int completedProfileFields = 1;
                if (!string.IsNullOrWhiteSpace(user.Location)) completedProfileFields++;
                if (!string.IsNullOrWhiteSpace(user.Bio)) completedProfileFields++;
                if (!string.IsNullOrWhiteSpace(user.Skills)) completedProfileFields++;
                int completion = completedProfileFields * 25;
                litProfileCompletion.Text = completion.ToString(CultureInfo.InvariantCulture);
                pnlProfileCompletionBar.Style["width"] = completion.ToString(CultureInfo.InvariantCulture) + "%";

                BindRecommendedProjects(user);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Freelancer dashboard failed to load: " + ex);
                litDashboardError.Text = "We could not load your dashboard right now. Please refresh and try again.";
                pnlDashboardError.Visible = true;
            }
        }

        private void BindRecommendedProjects(User user)
        {
            IList<Project> openProjects = ServiceFactory.CreateProjectService()
                .FindOpenProjects(string.Empty, string.Empty);
            List<string> skills = (user.Skills ?? string.Empty)
                .Split(new[] { ',', ';', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(skill => skill.Trim())
                .Where(skill => skill.Length > 0)
                .ToList();

            IEnumerable<Project> recommendations = openProjects;
            if (skills.Count > 0)
            {
                List<Project> matching = openProjects.Where(project =>
                    skills.Any(skill =>
                        ContainsIgnoreCase(project.RequiredSkills, skill) ||
                        ContainsIgnoreCase(project.Category, skill)))
                    .ToList();
                if (matching.Count > 0)
                    recommendations = matching;
            }

            List<ProjectCard> cards = recommendations
                .OrderByDescending(project => project.CreatedAt)
                .Take(4)
                .Select(project => new ProjectCard
                {
                    Title = project.Title,
                    Description = project.Description,
                    Category = project.Category,
                    Deadline = project.Deadline.HasValue
                        ? project.Deadline.Value.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)
                        : "No deadline",
                    Budget = string.Format(CultureInfo.GetCultureInfo("en-IN"),
                        "₹{0:N0} - ₹{1:N0}", project.MinBudget, project.MaxBudget),
                    ProjectUrl = "ProjectDetails.aspx?id=" + project.ProjectID
                })
                .ToList();

            rptRecommendedProjects.DataSource = cards;
            rptRecommendedProjects.DataBind();
            pnlNoRecommendedProjects.Visible = cards.Count == 0;
        }

        private static bool ContainsIgnoreCase(string source, string value)
        {
            return !string.IsNullOrWhiteSpace(source) &&
                source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
