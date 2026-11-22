using System;
using System.Collections.Generic;
using System.Data;
using CIE_2_PROJECT.Delegates;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Services
{
    public sealed class SubmissionService : WorkflowServiceBase, ISubmissionService
    {
        private readonly IContractRepository contracts;
        private readonly IProjectRepository projects;
        private readonly ISubmissionRepository submissions;
        private readonly IPaymentRepository payments;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IPaymentProcessor paymentProcessor;
        private readonly FileUploadHelper fileUploadHelper;

        public SubmissionService(
            IContractRepository contracts,
            IProjectRepository projects,
            ISubmissionRepository submissions,
            IPaymentRepository payments,
            IUnitOfWorkFactory unitOfWorkFactory,
            IPaymentProcessor paymentProcessor,
            FileUploadHelper fileUploadHelper,
            WorkflowAuditLogger auditLogger)
            : base(auditLogger)
        {
            if (contracts == null) throw new ArgumentNullException("contracts");
            if (projects == null) throw new ArgumentNullException("projects");
            if (submissions == null) throw new ArgumentNullException("submissions");
            if (payments == null) throw new ArgumentNullException("payments");
            if (unitOfWorkFactory == null) throw new ArgumentNullException("unitOfWorkFactory");
            if (paymentProcessor == null) throw new ArgumentNullException("paymentProcessor");
            if (fileUploadHelper == null) throw new ArgumentNullException("fileUploadHelper");

            this.contracts = contracts;
            this.projects = projects;
            this.submissions = submissions;
            this.payments = payments;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.paymentProcessor = paymentProcessor;
            this.fileUploadHelper = fileUploadHelper;
        }

        public WorkSubmission GetLatestForClient(int clientId, int contractId)
        {
            EnsureIds(clientId, contractId);
            EnsureClientOwns(contracts.GetById(contractId), clientId);
            using (IUnitOfWork unit = unitOfWorkFactory.Begin(IsolationLevel.ReadCommitted))
                return submissions.GetLatestByContractId(contractId, unit);
        }

        public WorkSubmission GetLatestForFreelancer(int freelancerId, int contractId)
        {
            EnsureIds(freelancerId, contractId);
            EnsureFreelancerOwns(contracts.GetById(contractId), freelancerId);
            using (IUnitOfWork unit = unitOfWorkFactory.Begin(IsolationLevel.ReadCommitted))
                return submissions.GetLatestByContractId(contractId, unit);
        }

        public IList<WorkSubmission> GetHistoryForClient(int clientId, int contractId)
        {
            EnsureIds(clientId, contractId);
            EnsureClientOwns(contracts.GetById(contractId), clientId);
            return submissions.GetByContractId(contractId);
        }

        public IList<SubmissionFile> GetFilesForClient(int clientId, int submissionId)
        {
            return GetFilesForParticipant(clientId, submissionId, true);
        }

        public IList<SubmissionFile> GetFilesForFreelancer(int freelancerId, int submissionId)
        {
            return GetFilesForParticipant(freelancerId, submissionId, false);
        }

        public int SubmitWork(
            int freelancerId,
            int contractId,
            string completionMessage,
            string demoUrl,
            string githubUrl,
            IList<UploadedFileInput> files)
        {
            EnsureIds(freelancerId, contractId);
            if (string.IsNullOrWhiteSpace(completionMessage))
                throw new InvalidOperationException("A completion message is required.");
            if (!UrlValidationHelper.IsValidHttpUrl(demoUrl) ||
                !UrlValidationHelper.IsValidHttpUrl(githubUrl))
                throw new InvalidOperationException("Demo and GitHub links must be valid HTTP or HTTPS URLs.");

            Contract initial = contracts.GetById(contractId);
            EnsureFreelancerOwns(initial, freelancerId);
            if (initial.ApprovalStatus != WorkflowStatuses.ContractNotSubmitted &&
                initial.ApprovalStatus != WorkflowStatuses.ContractRevisionRequested)
                throw new InvalidOperationException("This contract is not accepting a work submission.");

            SavedFileBatch batch = fileUploadHelper.SaveWorkFiles(contractId, files);
            try
            {
                using (IUnitOfWork unit = unitOfWorkFactory.Begin(IsolationLevel.Serializable))
                {
                    Contract contract = contracts.GetByIdForUpdate(contractId, unit);
                    EnsureFreelancerOwns(contract, freelancerId);
                    if (contract.ApprovalStatus != WorkflowStatuses.ContractNotSubmitted &&
                        contract.ApprovalStatus != WorkflowStatuses.ContractRevisionRequested)
                        throw new InvalidOperationException("This contract is not accepting a work submission.");

                    Payment payment = payments.GetByContractId(contractId, unit);
                    if (payment == null || payment.PaymentStatus != WorkflowStatuses.PaymentHeld)
                        throw new InvalidOperationException("The client must fund the contract before work is submitted.");

                    int number = submissions.GetNextSubmissionNumber(contractId, unit);
                    DateTime now = DateTime.Now;
                    string oldContractStatus = contract.ApprovalStatus;
                    WorkSubmission submission = new WorkSubmission(
                        0, contractId, number, completionMessage, demoUrl, githubUrl,
                        now, WorkflowStatuses.SubmissionPendingReview, null, null);
                    int submissionId = submissions.Create(submission, batch.Files, unit);

                    contract.MarkWorkSubmitted(completionMessage, now);
                    contracts.Update(contract, unit);
                    PublishWorkflowChange(
                        new WorkflowAuditEntry(
                            freelancerId, "Submission", submissionId, "WorkSubmitted",
                            oldContractStatus, WorkflowStatuses.SubmissionPendingReview,
                            "Freelancer submitted a version of the completed work.", now),
                        unit);
                    unit.Commit();
                    NotificationService.NotifySafely(
                        contract.ClientID, "WorkSubmitted",
                        "The freelancer submitted work for your project. Review the files and submission details.",
                        contract.ProjectID);
                    return submissionId;
                }
            }
            catch
            {
                batch.Cleanup();
                throw;
            }
        }

        public void RequestRevision(int clientId, int contractId, string feedback)
        {
            EnsureIds(clientId, contractId);
            if (string.IsNullOrWhiteSpace(feedback))
                throw new InvalidOperationException("Written feedback is required for a revision request.");

            using (IUnitOfWork unit = unitOfWorkFactory.Begin(IsolationLevel.Serializable))
            {
                Contract contract = contracts.GetByIdForUpdate(contractId, unit);
                EnsureClientOwns(contract, clientId);
                WorkSubmission latest = submissions.GetLatestByContractId(contractId, unit);
                if (latest == null)
                    throw new InvalidOperationException("There is no submitted work to return for revision.");
                if (latest.SubmissionStatus != WorkflowStatuses.SubmissionPendingReview)
                    throw new InvalidOperationException("Only work awaiting review can be returned for revision.");
                if (contract.ApprovalStatus != WorkflowStatuses.ContractPendingReview)
                    throw new InvalidOperationException("The contract is not awaiting a work review.");

                string oldStatus = latest.SubmissionStatus;
                DateTime now = DateTime.Now;
                latest.RequestRevision(feedback, now);
                contract.RequestRevision();
                submissions.Update(latest, unit);
                contracts.Update(contract, unit);
                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        clientId, "Submission", latest.SubmissionID, "RevisionRequested",
                        oldStatus, latest.SubmissionStatus,
                        "Client requested revisions and saved written feedback.", now),
                    unit);
                unit.Commit();
                NotificationService.NotifySafely(
                    contract.FreelancerID, "RevisionRequested",
                    "The client requested revisions. Review the feedback on your submitted work.",
                    contract.ProjectID);
            }
        }

        public void ApproveSubmission(int clientId, int contractId)
        {
            EnsureIds(clientId, contractId);
            using (IUnitOfWork unit = unitOfWorkFactory.Begin(IsolationLevel.Serializable))
            {
                Contract contract = contracts.GetByIdForUpdate(contractId, unit);
                EnsureClientOwns(contract, clientId);
                WorkSubmission latest = submissions.GetLatestByContractId(contractId, unit);
                if (latest == null || latest.SubmissionStatus != WorkflowStatuses.SubmissionPendingReview)
                    throw new InvalidOperationException("Only the latest submission awaiting review can be approved.");
                if (contract.ApprovalStatus != WorkflowStatuses.ContractPendingReview)
                    throw new InvalidOperationException("The contract is not awaiting a work review.");

                Payment payment = payments.GetByContractId(contractId, unit);
                if (payment == null || payment.PaymentStatus != WorkflowStatuses.PaymentHeld)
                    throw new InvalidOperationException("A held payment is required before approval and payment release.");

                Project project = projects.GetByIdForUpdate(contract.ProjectID, unit);
                if (project == null || project.ClientID != clientId ||
                    project.Status != WorkflowStatuses.ProjectInProgress)
                    throw new InvalidOperationException("The project is not eligible for completion.");

                DateTime now = DateTime.Now;
                string oldSubmissionStatus = latest.SubmissionStatus;
                string oldPaymentStatus = payment.PaymentStatus;
                string oldContractStatus = contract.ApprovalStatus;
                string oldProjectStatus = project.Status;
                latest.Approve(now);
                contract.ApproveSubmission();
                EnsurePaymentResult(paymentProcessor.ReleasePayment(payment));
                contract.MarkCompleted(now);
                project.Complete();
                submissions.Update(latest, unit);
                contracts.Update(contract, unit);
                payments.Update(payment, unit);
                projects.Update(project, unit);
                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        clientId, "Submission", latest.SubmissionID, "WorkApprovedAndPaymentReleased",
                        oldSubmissionStatus, latest.SubmissionStatus,
                        "Client approved the latest submission; the held simulated payment was released and the contract completed.", now),
                    unit);
                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        clientId, "Payment", payment.PaymentID, "PaymentReleased",
                        oldPaymentStatus, payment.PaymentStatus,
                        "Approval automatically released the held simulated payment.", now),
                    unit);
                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        clientId, "Contract", contract.ContractID, "ContractCompleted",
                        oldContractStatus, contract.ApprovalStatus,
                        "Contract completed after client approval and automatic payment release.", now),
                    unit);
                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        clientId, "Project", project.ProjectID, "ProjectCompleted",
                        oldProjectStatus, project.Status,
                        "Project completed after approval and payment release.", now),
                    unit);
                unit.Commit();
                NotificationService.NotifySafely(
                    contract.FreelancerID, "PaymentReleased",
                    "The client approved your work and the simulated payment was released for " + project.Title + ".",
                    contract.ProjectID);
            }
        }

        private IList<SubmissionFile> GetFilesForParticipant(
            int userId,
            int submissionId,
            bool isClient)
        {
            if (userId <= 0 || submissionId <= 0)
                throw new ArgumentOutOfRangeException("IDs must be positive.");

            WorkSubmission submission = submissions.GetById(submissionId);
            if (submission == null)
                throw new InvalidOperationException("Submission not found or access is denied.");
            Contract contract = contracts.GetById(submission.ContractID);
            if (isClient)
                EnsureClientOwns(contract, userId);
            else
                EnsureFreelancerOwns(contract, userId);

            return submissions.GetFilesBySubmissionId(submissionId);
        }

        private static void EnsureClientOwns(Contract contract, int clientId)
        {
            if (contract == null || contract.ClientID != clientId)
                throw new InvalidOperationException("Contract not found or access is denied.");
        }

        private static void EnsureFreelancerOwns(Contract contract, int freelancerId)
        {
            if (contract == null || contract.FreelancerID != freelancerId)
                throw new InvalidOperationException("Contract not found or access is denied.");
        }

        private static void EnsurePaymentResult(PaymentResult result)
        {
            if (result == null || !result.Succeeded)
                throw new InvalidOperationException(
                    result == null ? "The simulated payment operation failed." : result.Message);
        }

        private static void EnsureIds(int userId, int contractId)
        {
            if (userId <= 0 || contractId <= 0)
                throw new ArgumentOutOfRangeException("IDs must be positive.");
        }
    }
}
