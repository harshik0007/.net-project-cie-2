using System;
using System.Collections.Generic;
using System.Linq;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Helpers
{
    public static class FreelancerPerformanceHelper
    {
        public static int GetSlowWorkerBadgeCount(
            IList<Contract> contracts,
            IProjectService projectService,
            IList<Payment> payments)
        {
            if (contracts == null || projectService == null || payments == null)
                return 0;

            Dictionary<int, Project> projects = new Dictionary<int, Project>();
            Func<Contract, Project> getProject = contract =>
            {
                Project project;
                if (!projects.TryGetValue(contract.ProjectID, out project))
                {
                    project = projectService.GetProject(contract.ProjectID);
                    projects[contract.ProjectID] = project;
                }
                return project;
            };

            int count = 0;
            IEnumerable<Contract> completed = contracts
                .Where(contract => contract.ApprovalStatus == WorkflowStatuses.ContractCompleted)
                .OrderBy(contract => contract.CompletedAt ?? contract.CreatedAt)
                .ThenBy(contract => contract.ContractID);

            foreach (Contract contract in completed)
            {
                Project project = getProject(contract);
                if (project == null || !project.Deadline.HasValue || !contract.WorkSubmittedAt.HasValue)
                    continue;

                if (contract.WorkSubmittedAt.Value.Date > project.Deadline.Value.Date)
                    count++;
                else if (count > 0)
                    count--;
            }

            foreach (Contract contract in contracts)
            {
                if (contract.ApprovalStatus == WorkflowStatuses.ContractCompleted ||
                    contract.ApprovalStatus == WorkflowStatuses.ContractCancelled)
                    continue;

                Payment payment = payments.FirstOrDefault(item => item.ContractID == contract.ContractID);
                if (payment == null || payment.PaymentStatus != WorkflowStatuses.PaymentHeld)
                    continue;

                Project project = getProject(contract);
                if (project == null || !project.Deadline.HasValue ||
                    project.Deadline.Value.Date >= DateTime.Today)
                    continue;

                if (!contract.WorkSubmittedAt.HasValue ||
                    contract.WorkSubmittedAt.Value.Date > project.Deadline.Value.Date)
                    count++;
            }

            return count;
        }
    }
}
