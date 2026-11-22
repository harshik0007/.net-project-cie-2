using System;
using System.Collections.Generic;
using System.Data;
using CIE_2_PROJECT.DataAccess;
using CIE_2_PROJECT.Delegates;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;
using CIE_2_PROJECT.Validators;

namespace CIE_2_PROJECT.Services
{
    public sealed class ProposalService : WorkflowServiceBase, IProposalService
    {
        private readonly IProjectRepository projects;
        private readonly IProposalRepository proposals;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly ProposalValidator validator;

        public ProposalService(
            IProjectRepository projects,
            IProposalRepository proposals,
            IUnitOfWorkFactory unitOfWorkFactory,
            ProposalValidator validator,
            WorkflowAuditLogger auditLogger)
            : base(auditLogger)
        {
            if (projects == null)
                throw new ArgumentNullException("projects");
            if (proposals == null)
                throw new ArgumentNullException("proposals");
            if (unitOfWorkFactory == null)
                throw new ArgumentNullException("unitOfWorkFactory");
            if (validator == null)
                throw new ArgumentNullException("validator");

            this.projects = projects;
            this.proposals = proposals;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.validator = validator;
        }

        public IList<Proposal> GetProjectProposals(int clientId, int projectId, string sortBy)
        {
            Project project = projects.GetById(projectId);
            if (project == null || project.ClientID != clientId)
                throw new InvalidOperationException("Project not found or access is denied.");

            List<Proposal> result = new List<Proposal>(proposals.GetByProjectId(projectId));
            ProposalComparisonHandler comparison = CreateComparison(sortBy);
            result.Sort(delegate(Proposal left, Proposal right)
            {
                return comparison(left, right);
            });
            return result;
        }

        public IList<Proposal> GetFreelancerProposals(int freelancerId)
        {
            if (freelancerId <= 0)
                throw new ArgumentOutOfRangeException("freelancerId");
            return proposals.GetByFreelancerId(freelancerId);
        }

        public int GetProposalCount(int projectId)
        {
            if (projectId <= 0)
                throw new ArgumentOutOfRangeException("projectId");
            return proposals.GetCountByProjectId(projectId);
        }

        public int SubmitProposal(Proposal proposal)
        {
            if (proposal == null)
                throw new ArgumentNullException("proposal");
            if (proposal.ProposalID != 0 || proposal.Status != WorkflowStatuses.ProposalPending)
                throw new InvalidOperationException("A new proposal must start as Pending.");

            using (IUnitOfWork unit = unitOfWorkFactory.Begin(IsolationLevel.Serializable))
            {
                Project project = projects.GetByIdForUpdate(proposal.ProjectID, unit);
                if (project == null || project.Status != WorkflowStatuses.ProjectOpen)
                    throw new InvalidOperationException("This project is not accepting proposals.");
                if (project.ClientID == proposal.FreelancerID)
                    throw new InvalidOperationException("A client cannot submit a proposal to their own project.");

                ThrowIfInvalid(validator.Validate(proposal, project));
                if (proposals.HasProposal(project.ProjectID, proposal.FreelancerID, unit))
                    throw new InvalidOperationException(
                        "You already have an active proposal for this project.");

                int proposalId = proposals.Create(proposal, unit);
                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        proposal.FreelancerID, "Proposal", proposalId, "ProposalSubmitted",
                        null, WorkflowStatuses.ProposalPending,
                        "Freelancer submitted a proposal.", DateTime.Now),
                    unit);
                unit.Commit();
                NotificationService.NotifySafely(
                    project.ClientID, "NewProposal",
                    "A freelancer submitted a proposal for " + project.Title + ".",
                    project.ProjectID);
                return proposalId;
            }
        }

        private static ProposalComparisonHandler CreateComparison(string sortBy)
        {
            if (string.Equals(sortBy, "PriceLow", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(sortBy, "price", StringComparison.OrdinalIgnoreCase))
            {
                return delegate(Proposal left, Proposal right)
                {
                    int amount = left.BidAmount.CompareTo(right.BidAmount);
                    return amount != 0 ? amount : right.SubmittedAt.CompareTo(left.SubmittedAt);
                };
            }
            if (string.Equals(sortBy, "PriceHigh", StringComparison.OrdinalIgnoreCase))
            {
                return delegate(Proposal left, Proposal right)
                {
                    int amount = right.BidAmount.CompareTo(left.BidAmount);
                    return amount != 0 ? amount : right.SubmittedAt.CompareTo(left.SubmittedAt);
                };
            }
            if (string.Equals(sortBy, "delivery", StringComparison.OrdinalIgnoreCase))
            {
                return SortByDelivery();
            }
            if (string.Equals(sortBy, "Oldest", StringComparison.OrdinalIgnoreCase))
            {
                return delegate(Proposal left, Proposal right)
                {
                    return left.SubmittedAt.CompareTo(right.SubmittedAt);
                };
            }

            return delegate(Proposal left, Proposal right)
            {
                return right.SubmittedAt.CompareTo(left.SubmittedAt);
            };
        }

        private static ProposalComparisonHandler SortByDelivery()
        {
            return delegate(Proposal left, Proposal right)
            {
                int days = left.EstimatedDays.CompareTo(right.EstimatedDays);
                return days != 0 ? days : left.BidAmount.CompareTo(right.BidAmount);
            };
        }

        private static void ThrowIfInvalid(IList<string> errors)
        {
            if (errors == null || errors.Count == 0)
                return;
            throw new InvalidOperationException(string.Join(" ", errors));
        }
    }
}
