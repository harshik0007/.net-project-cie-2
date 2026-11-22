using System;

namespace CIE_2_PROJECT.Models
{
    public sealed class WorkSubmission
    {
        public int SubmissionID { get; private set; }
        public int ContractID { get; private set; }
        public int SubmissionNumber { get; private set; }
        public string CompletionMessage { get; private set; }
        public string DemoURL { get; private set; }
        public string GithubURL { get; private set; }
        public DateTime SubmittedAt { get; private set; }
        public string SubmissionStatus { get; private set; }
        public string ClientFeedback { get; private set; }
        public DateTime? ReviewedAt { get; private set; }

        public WorkSubmission(
            int submissionId,
            int contractId,
            int submissionNumber,
            string completionMessage,
            string demoUrl,
            string githubUrl,
            DateTime submittedAt,
            string submissionStatus,
            string clientFeedback,
            DateTime? reviewedAt)
        {
            if (submissionId < 0)
                throw new ArgumentOutOfRangeException("submissionId");
            if (contractId <= 0)
                throw new ArgumentOutOfRangeException("contractId");
            if (submissionNumber <= 0)
                throw new ArgumentOutOfRangeException("submissionNumber");
            if (string.IsNullOrWhiteSpace(completionMessage))
                throw new ArgumentException("A completion message is required.", "completionMessage");
            if (!IsSubmissionStatus(submissionStatus))
                throw new ArgumentException("The submission status is invalid.", "submissionStatus");

            SubmissionID = submissionId;
            ContractID = contractId;
            SubmissionNumber = submissionNumber;
            CompletionMessage = completionMessage.Trim();
            DemoURL = demoUrl;
            GithubURL = githubUrl;
            SubmittedAt = submittedAt;
            SubmissionStatus = submissionStatus;
            ClientFeedback = clientFeedback;
            ReviewedAt = reviewedAt;
        }

        public void RequestRevision(string feedback, DateTime reviewedAt)
        {
            EnsurePendingReview();
            if (string.IsNullOrWhiteSpace(feedback))
                throw new ArgumentException("Written feedback is required for a revision request.", "feedback");

            SubmissionStatus = WorkflowStatuses.SubmissionRevisionRequested;
            ClientFeedback = feedback.Trim();
            ReviewedAt = reviewedAt;
        }

        public void Approve(DateTime reviewedAt)
        {
            EnsurePendingReview();
            SubmissionStatus = WorkflowStatuses.SubmissionApproved;
            ReviewedAt = reviewedAt;
        }

        private void EnsurePendingReview()
        {
            if (SubmissionStatus != WorkflowStatuses.SubmissionPendingReview)
                throw new InvalidOperationException(
                    "Only a submission awaiting review can be approved or returned for revision.");
        }

        private static bool IsSubmissionStatus(string status)
        {
            return status == WorkflowStatuses.SubmissionPendingReview ||
                   status == WorkflowStatuses.SubmissionRevisionRequested ||
                   status == WorkflowStatuses.SubmissionApproved;
        }
    }
}
