using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using CIE_2_PROJECT.Delegates;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Services
{
    public sealed class PaymentService : WorkflowServiceBase, IPaymentService
    {
        private readonly IContractRepository contracts;
        private readonly IProjectRepository projects;
        private readonly IProposalRepository proposals;
        private readonly IPaymentRepository payments;
        private readonly ISubmissionRepository submissions;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IPaymentProcessor paymentProcessor;

        public PaymentService(
            IContractRepository contracts,
            IProjectRepository projects,
            IProposalRepository proposals,
            IPaymentRepository payments,
            ISubmissionRepository submissions,
            IUnitOfWorkFactory unitOfWorkFactory,
            IPaymentProcessor paymentProcessor,
            WorkflowAuditLogger auditLogger)
            : base(auditLogger)
        {
            if (contracts == null) throw new ArgumentNullException("contracts");
            if (projects == null) throw new ArgumentNullException("projects");
            if (proposals == null) throw new ArgumentNullException("proposals");
            if (payments == null) throw new ArgumentNullException("payments");
            if (submissions == null) throw new ArgumentNullException("submissions");
            if (unitOfWorkFactory == null) throw new ArgumentNullException("unitOfWorkFactory");
            if (paymentProcessor == null) throw new ArgumentNullException("paymentProcessor");

            this.contracts = contracts;
            this.projects = projects;
            this.proposals = proposals;
            this.payments = payments;
            this.submissions = submissions;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.paymentProcessor = paymentProcessor;
        }

        public int FundContract(int clientId, int contractId)
        {
            EnsureIds(clientId, contractId);
            using (IUnitOfWork unit = unitOfWorkFactory.Begin(IsolationLevel.Serializable))
            {
                Contract contract = contracts.GetByIdForUpdate(contractId, unit);
                EnsureClientOwns(contract, clientId);
                if (contract.ApprovalStatus == WorkflowStatuses.ContractCompleted ||
                    contract.ApprovalStatus == WorkflowStatuses.ContractCancelled)
                    throw new InvalidOperationException("This contract cannot be funded.");

                Project project = projects.GetByIdForUpdate(contract.ProjectID, unit);
                if (project == null || project.ClientID != clientId ||
                    project.Status != WorkflowStatuses.ProjectInProgress)
                    throw new InvalidOperationException("Only an active client-owned project can be funded.");

                if (payments.GetByContractId(contractId, unit) != null)
                    throw new InvalidOperationException("This contract already has a payment record.");

                DateTime now = DateTime.Now;
                Payment payment = Payment.CreateWithDemoGst(
                    0, contract.ContractID, contract.ClientID, contract.FreelancerID,
                    contract.AgreedAmount, WorkflowStatuses.PaymentNotFunded,
                    "Academic Simulation", now, null, null);

                PaymentResult result = paymentProcessor.FundPayment(payment);
                EnsurePaymentResult(result);
                int paymentId = payments.Create(payment, unit);

                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        clientId, "Payment", paymentId, "PaymentFunded",
                        WorkflowStatuses.PaymentNotFunded, payment.PaymentStatus,
                        "Client funded the contract using the academic payment simulation. GST " +
                        FormatCurrency(payment.GstAmount) + " is added to the " +
                        FormatCurrency(payment.Amount) + " contract amount. Client platform fee " +
                        FormatCurrency(payment.ClientPlatformFeeAmount) + "; client total " +
                        FormatCurrency(payment.ClientTotalAmount) + ". Freelancer fee " +
                        FormatCurrency(payment.FreelancerPlatformFeeAmount) + "; expected freelancer net " +
                        FormatCurrency(payment.FreelancerNetAmount) + ".", now),
                    unit);
                unit.Commit();
                NotificationService.NotifySafely(
                    contract.FreelancerID, "PaymentFunded",
                    "The client funded your contract. You can now submit work.", contract.ProjectID);
                return paymentId;
            }
        }

        public int FundProposal(int clientId, int projectId, int proposalId)
        {
            if (clientId <= 0 || projectId <= 0 || proposalId <= 0)
                throw new ArgumentOutOfRangeException("IDs must be positive.");
            List<Proposal> rejectedProposals = new List<Proposal>();
            using (IUnitOfWork unit = unitOfWorkFactory.Begin(IsolationLevel.Serializable))
            {
                Project project = projects.GetByIdForUpdate(projectId, unit);
                if (project == null || project.ClientID != clientId)
                    throw new InvalidOperationException("Project not found or access is denied.");
                if (project.Status != WorkflowStatuses.ProjectOpen)
                    throw new InvalidOperationException("This project is no longer accepting proposals.");

                Proposal proposal = proposals.GetByIdForUpdate(proposalId, unit);
                if (proposal == null || proposal.ProjectID != projectId ||
                    proposal.Status != WorkflowStatuses.ProposalPending)
                    throw new InvalidOperationException("This proposal is no longer available for hiring.");
                rejectedProposals = proposals.GetByProjectId(projectId)
                    .Where(item => item.ProposalID != proposalId &&
                        item.Status == WorkflowStatuses.ProposalPending)
                    .ToList();
                if (contracts.GetByProjectId(projectId, unit) != null)
                    throw new InvalidOperationException("A contract already exists for this project.");
                if (contracts.CountActiveByFreelancerIdForUpdate(proposal.FreelancerID, unit) >= 3)
                    throw new InvalidOperationException(
                        "This freelancer already has 3 active projects. They must complete or cancel an active project before accepting another.");

                DateTime now = DateTime.Now;
                proposal.Accept(now);
                proposals.Update(proposal, unit);
                proposals.RejectOtherPending(projectId, proposalId, unit);
                project.Start();
                projects.Update(project, unit);

                Contract contract = new Contract(
                    0, projectId, proposalId, clientId, proposal.FreelancerID,
                    proposal.BidAmount, project.Description,
                    WorkflowStatuses.ContractNotSubmitted, now, null, null, null);
                int contractId = contracts.Create(contract, unit);

                Payment payment = Payment.CreateWithDemoGst(
                    0, contractId, clientId, proposal.FreelancerID, proposal.BidAmount,
                    WorkflowStatuses.PaymentNotFunded, "Academic Simulation", now, null, null);
                EnsurePaymentResult(paymentProcessor.FundPayment(payment));
                int paymentId = payments.Create(payment, unit);

                PublishWorkflowChange(new WorkflowAuditEntry(
                    clientId, "Proposal", proposalId, "ProposalAccepted",
                    WorkflowStatuses.ProposalPending, WorkflowStatuses.ProposalAccepted,
                    "Proposal accepted when the client funded the contract.", now), unit);
                PublishWorkflowChange(new WorkflowAuditEntry(
                    clientId, "Contract", contractId, "ContractCreated",
                    null, WorkflowStatuses.ContractNotSubmitted,
                    "Contract created and funded after client confirmation.", now), unit);
                PublishWorkflowChange(new WorkflowAuditEntry(
                    clientId, "Payment", paymentId, "PaymentFunded",
                    WorkflowStatuses.PaymentNotFunded, payment.PaymentStatus,
                    "Client funded the contract using the academic payment simulation. GST " +
                    FormatCurrency(payment.GstAmount) + " is added to the " +
                    FormatCurrency(payment.Amount) + " contract amount. Client platform fee " +
                    FormatCurrency(payment.ClientPlatformFeeAmount) + "; client total " +
                    FormatCurrency(payment.ClientTotalAmount) + ". Freelancer fee " +
                    FormatCurrency(payment.FreelancerPlatformFeeAmount) + "; expected freelancer net " +
                    FormatCurrency(payment.FreelancerNetAmount) + ".", now), unit);

                unit.Commit();
                NotificationService.NotifySafely(
                    proposal.FreelancerID, "ProposalAccepted",
                    "Your proposal for " + project.Title + " was accepted and the contract was funded.", projectId);
                foreach (Proposal rejected in rejectedProposals)
                {
                    NotificationService.NotifySafely(
                        rejected.FreelancerID, "ProposalRejected",
                        "Another proposal was selected for " + project.Title + ".", projectId);
                }
                return contractId;
            }
        }

        public void ReleaseContractPayment(int clientId, int contractId)
        {
            EnsureIds(clientId, contractId);
            using (IUnitOfWork unit = unitOfWorkFactory.Begin(IsolationLevel.Serializable))
            {
                Contract contract = contracts.GetByIdForUpdate(contractId, unit);
                EnsureClientOwns(contract, clientId);
                if (contract.ApprovalStatus != WorkflowStatuses.ContractApproved)
                    throw new InvalidOperationException("Work must be approved before payment release.");

                WorkSubmission latest = submissions.GetLatestByContractId(contractId, unit);
                if (latest == null || latest.SubmissionStatus != WorkflowStatuses.SubmissionApproved)
                    throw new InvalidOperationException("The latest work submission has not been approved.");

                Payment payment = payments.GetByContractId(contractId, unit);
                if (payment == null || payment.PaymentStatus != WorkflowStatuses.PaymentHeld)
                    throw new InvalidOperationException("A held payment is required before release.");

                Project project = projects.GetByIdForUpdate(contract.ProjectID, unit);
                if (project == null || project.ClientID != clientId ||
                    project.Status != WorkflowStatuses.ProjectInProgress)
                    throw new InvalidOperationException("The project is not eligible for completion.");

                string oldPaymentStatus = payment.PaymentStatus;
                string oldContractStatus = contract.ApprovalStatus;
                string oldProjectStatus = project.Status;
                DateTime now = DateTime.Now;

                EnsurePaymentResult(paymentProcessor.ReleasePayment(payment));
                contract.MarkCompleted(now);
                project.Complete();
                payments.Update(payment, unit);
                contracts.Update(contract, unit);
                projects.Update(project, unit);

                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        clientId, "Payment", payment.PaymentID, "PaymentReleased",
                        oldPaymentStatus, payment.PaymentStatus,
                        "Client released the held simulated payment after approving work.", now),
                    unit);
                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        clientId, "Contract", contract.ContractID, "ContractCompleted",
                        oldContractStatus, contract.ApprovalStatus,
                        "Approved contract completed when payment was released.", now),
                    unit);
                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        clientId, "Project", project.ProjectID, "ProjectCompleted",
                        oldProjectStatus, project.Status,
                        "Project completed after approved work and payment release.", now),
                    unit);

                unit.Commit();
                NotificationService.NotifySafely(
                    contract.FreelancerID, "PaymentReleased",
                    "The client released payment for " + project.Title + ".", project.ProjectID);
            }
        }

        public void RefundCancelledContract(int clientId, int contractId)
        {
            EnsureIds(clientId, contractId);
            using (IUnitOfWork unit = unitOfWorkFactory.Begin(IsolationLevel.Serializable))
            {
                Contract contract = contracts.GetByIdForUpdate(contractId, unit);
                EnsureClientOwns(contract, clientId);
                if (contract.ApprovalStatus != WorkflowStatuses.ContractNotSubmitted)
                    throw new InvalidOperationException(
                        "A refund is eligible only before the freelancer submits any work.");
                if (submissions.GetLatestByContractId(contractId, unit) != null)
                    throw new InvalidOperationException("A contract with a work submission cannot be refunded.");

                Project project = projects.GetByIdForUpdate(contract.ProjectID, unit);
                if (project == null || project.ClientID != clientId ||
                    project.Status != WorkflowStatuses.ProjectInProgress)
                    throw new InvalidOperationException("The project is not eligible for cancellation.");

                Payment payment = payments.GetByContractId(contractId, unit);
                if (payment != null && payment.PaymentStatus != WorkflowStatuses.PaymentHeld)
                    throw new InvalidOperationException("Only a held simulated payment can be refunded.");

                DateTime now = DateTime.Now;
                string oldPaymentStatus = payment == null ? null : payment.PaymentStatus;
                string oldContractStatus = contract.ApprovalStatus;
                string oldProjectStatus = project.Status;

                if (payment != null)
                    EnsurePaymentResult(paymentProcessor.RefundPayment(payment));
                contract.CancelBeforeSubmission();
                project.CancelInProgress();
                if (payment != null)
                    payments.Update(payment, unit);
                contracts.Update(contract, unit);
                projects.Update(project, unit);

                if (payment != null)
                {
                    PublishWorkflowChange(
                        new WorkflowAuditEntry(
                            clientId, "Payment", payment.PaymentID, "PaymentRefunded",
                            oldPaymentStatus, payment.PaymentStatus,
                            "Cancellation settlement: client refund " +
                            FormatCurrency(payment.ClientRefundAmount + payment.GstRefundAmount +
                                payment.ClientPlatformFeeRefundAmount) +
                            " including GST returned " + FormatCurrency(payment.GstRefundAmount) +
                            " and client platform fee returned " +
                            FormatCurrency(payment.ClientPlatformFeeRefundAmount) +
                            ", freelancer payout " + FormatCurrency(payment.FreelancerPayoutAmount) +
                            ", platform fee " + FormatCurrency(payment.PlatformFeeAmount) + ".", now),
                        unit);
                }
                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        clientId, "Contract", contract.ContractID, "ContractCancelled",
                        oldContractStatus, contract.ApprovalStatus,
                        "Contract cancelled before any work was submitted.", now),
                    unit);
                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        clientId, "Project", project.ProjectID, "ProjectCancelled",
                        oldProjectStatus, project.Status,
                        "Project cancelled with an eligible simulated payment refund.", now),
                    unit);
                unit.Commit();
                string cancellationNotice = payment == null
                    ? "The client cancelled the contract for " + project.Title + "."
                    : "The client cancelled " + project.Title + ". Your simulated cancellation payout is " +
                        FormatCurrency(payment.FreelancerPayoutAmount) + " (10% of the pre-GST contract amount).";
                NotificationService.NotifySafely(
                    contract.FreelancerID, "ProjectCancelled", cancellationNotice, project.ProjectID);
            }
        }

        public IList<Payment> GetClientPaymentHistory(int clientId)
        {
            if (clientId <= 0)
                throw new ArgumentOutOfRangeException("clientId");
            return payments.GetByClientId(clientId);
        }

        public IList<Payment> GetFreelancerPaymentHistory(int freelancerId)
        {
            if (freelancerId <= 0)
                throw new ArgumentOutOfRangeException("freelancerId");
            return payments.GetByFreelancerId(freelancerId);
        }

        private static void EnsureClientOwns(Contract contract, int clientId)
        {
            if (contract == null || contract.ClientID != clientId)
                throw new InvalidOperationException("Contract not found or access is denied.");
        }

        private static void EnsurePaymentResult(PaymentResult result)
        {
            if (result == null || !result.Succeeded)
                throw new InvalidOperationException(
                    result == null ? "The simulated payment operation failed." : result.Message);
        }

        private static string FormatCurrency(decimal amount)
        {
            return amount.ToString("C2", CultureInfo.GetCultureInfo("en-IN"));
        }

        private static void EnsureIds(int clientId, int contractId)
        {
            if (clientId <= 0 || contractId <= 0)
                throw new ArgumentOutOfRangeException("IDs must be positive.");
        }
    }
}
