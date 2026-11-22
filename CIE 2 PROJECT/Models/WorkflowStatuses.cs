namespace CIE_2_PROJECT.Models
{
    public static class WorkflowStatuses
    {
        public const string ProjectOpen = "Open for Proposals";
        public const string ProjectInProgress = "In Progress";
        public const string ProjectCompleted = "Completed";
        public const string ProjectCancelled = "Cancelled";

        public const string ProposalPending = "Pending";
        public const string ProposalAccepted = "Accepted";
        public const string ProposalRejected = "Rejected";

        public const string ContractNotSubmitted = "Not Submitted";
        public const string ContractPendingReview = "Pending Review";
        public const string ContractRevisionRequested = "Revision Requested";
        public const string ContractApproved = "Approved";
        public const string ContractCompleted = "Completed";
        public const string ContractCancelled = "Cancelled";

        public const string PaymentNotFunded = "Not Funded";
        public const string EscrowNotFunded = "Not Funded";
        public const string PaymentHeld = "Held";
        public const string PaymentReleased = "Released";
        public const string PaymentRefunded = "Refunded";

        public const string SubmissionPendingReview = "Pending Review";
        public const string SubmissionRevisionRequested = "Revision Requested";
        public const string SubmissionApproved = "Approved";
    }
}
