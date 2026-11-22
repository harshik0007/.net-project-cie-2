using System;

namespace CIE_2_PROJECT.Models
{
    public sealed class Contract
    {
        public int ContractID { get; private set; }
        public int ProjectID { get; private set; }
        public int ProposalID { get; private set; }
        public int ClientID { get; private set; }
        public int FreelancerID { get; private set; }
        public decimal AgreedAmount { get; private set; }
        public string WorkDescription { get; private set; }
        public string ApprovalStatus { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? WorkSubmittedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public string CompletionMessage { get; private set; }

        public Contract(
            int contractId,
            int projectId,
            int proposalId,
            int clientId,
            int freelancerId,
            decimal agreedAmount,
            string workDescription,
            string approvalStatus,
            DateTime createdAt,
            DateTime? workSubmittedAt,
            DateTime? completedAt,
            string completionMessage)
        {
            if (contractId < 0)
                throw new ArgumentOutOfRangeException("contractId");
            if (projectId <= 0 || proposalId <= 0 || clientId <= 0 || freelancerId <= 0)
                throw new ArgumentException("Contract participant and related IDs must be positive.");
            if (agreedAmount <= 0)
                throw new ArgumentOutOfRangeException("agreedAmount");
            if (!IsContractStatus(approvalStatus))
                throw new ArgumentException("The contract status is invalid.", "approvalStatus");

            ContractID = contractId;
            ProjectID = projectId;
            ProposalID = proposalId;
            ClientID = clientId;
            FreelancerID = freelancerId;
            AgreedAmount = agreedAmount;
            WorkDescription = workDescription;
            ApprovalStatus = approvalStatus;
            CreatedAt = createdAt;
            WorkSubmittedAt = workSubmittedAt;
            CompletedAt = completedAt;
            CompletionMessage = completionMessage;
        }

        public void MarkWorkSubmitted(string completionMessage, DateTime submittedAt)
        {
            if (ApprovalStatus != WorkflowStatuses.ContractNotSubmitted &&
                ApprovalStatus != WorkflowStatuses.ContractRevisionRequested)
                throw new InvalidOperationException(
                    "Work can only be submitted for a new contract or after a revision request.");
            if (string.IsNullOrWhiteSpace(completionMessage))
                throw new ArgumentException("A completion message is required.", "completionMessage");

            CompletionMessage = completionMessage.Trim();
            WorkSubmittedAt = submittedAt;
            ApprovalStatus = WorkflowStatuses.ContractPendingReview;
        }

        public void RequestRevision()
        {
            EnsureStatus(WorkflowStatuses.ContractPendingReview);
            ApprovalStatus = WorkflowStatuses.ContractRevisionRequested;
        }

        public void ApproveSubmission()
        {
            EnsureStatus(WorkflowStatuses.ContractPendingReview);
            ApprovalStatus = WorkflowStatuses.ContractApproved;
        }

        public void MarkCompleted(DateTime completedAt)
        {
            EnsureStatus(WorkflowStatuses.ContractApproved);
            ApprovalStatus = WorkflowStatuses.ContractCompleted;
            CompletedAt = completedAt;
        }

        public void CancelBeforeSubmission()
        {
            EnsureStatus(WorkflowStatuses.ContractNotSubmitted);
            ApprovalStatus = WorkflowStatuses.ContractCancelled;
        }

        private void EnsureStatus(string expectedStatus)
        {
            if (ApprovalStatus != expectedStatus)
                throw new InvalidOperationException(
                    "This operation is not allowed while the contract is " + ApprovalStatus + ".");
        }

        private static bool IsContractStatus(string status)
        {
            return status == WorkflowStatuses.ContractNotSubmitted ||
                   status == WorkflowStatuses.ContractPendingReview ||
                   status == WorkflowStatuses.ContractRevisionRequested ||
                   status == WorkflowStatuses.ContractApproved ||
                   status == WorkflowStatuses.ContractCompleted ||
                   status == WorkflowStatuses.ContractCancelled;
        }
    }
}
