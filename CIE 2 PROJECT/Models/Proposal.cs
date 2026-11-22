using System;

namespace CIE_2_PROJECT.Models
{
    public sealed class Proposal
    {
        public int ProposalID { get; private set; }
        public int ProjectID { get; private set; }
        public int FreelancerID { get; private set; }
        public string CoverLetter { get; private set; }
        public decimal BidAmount { get; private set; }
        public int EstimatedDays { get; private set; }
        public string AttachmentPath { get; private set; }
        public string Status { get; private set; }
        public DateTime SubmittedAt { get; private set; }
        public DateTime? AcceptedAt { get; private set; }

        public Proposal(
            int proposalId,
            int projectId,
            int freelancerId,
            string coverLetter,
            decimal bidAmount,
            int estimatedDays,
            string attachmentPath,
            string status,
            DateTime submittedAt,
            DateTime? acceptedAt)
        {
            if (proposalId < 0)
                throw new ArgumentOutOfRangeException("proposalId");
            if (projectId <= 0 || freelancerId <= 0)
                throw new ArgumentException("Project and freelancer IDs must be positive.");
            if (string.IsNullOrWhiteSpace(coverLetter))
                throw new ArgumentException("A cover letter is required.", "coverLetter");
            if (bidAmount <= 0)
                throw new ArgumentOutOfRangeException("bidAmount");
            if (estimatedDays <= 0)
                throw new ArgumentOutOfRangeException("estimatedDays");
            if (!IsProposalStatus(status))
                throw new ArgumentException("The proposal status is invalid.", "status");

            ProposalID = proposalId;
            ProjectID = projectId;
            FreelancerID = freelancerId;
            CoverLetter = coverLetter.Trim();
            BidAmount = bidAmount;
            EstimatedDays = estimatedDays;
            AttachmentPath = attachmentPath;
            Status = status;
            SubmittedAt = submittedAt;
            AcceptedAt = acceptedAt;
        }

        public void Accept(DateTime acceptedAt)
        {
            EnsurePending();
            Status = WorkflowStatuses.ProposalAccepted;
            AcceptedAt = acceptedAt;
        }

        public void Reject()
        {
            EnsurePending();
            Status = WorkflowStatuses.ProposalRejected;
        }

        private void EnsurePending()
        {
            if (Status != WorkflowStatuses.ProposalPending)
                throw new InvalidOperationException(
                    "Only a pending proposal can be accepted or rejected.");
        }

        private static bool IsProposalStatus(string status)
        {
            return status == WorkflowStatuses.ProposalPending ||
                   status == WorkflowStatuses.ProposalAccepted ||
                   status == WorkflowStatuses.ProposalRejected;
        }
    }
}
