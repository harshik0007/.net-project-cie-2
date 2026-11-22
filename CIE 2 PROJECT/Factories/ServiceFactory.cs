using CIE_2_PROJECT.DataAccess;
using CIE_2_PROJECT.Delegates;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Services;
using CIE_2_PROJECT.Validators;

namespace CIE_2_PROJECT.Factories
{
    public static class ServiceFactory
    {
        public static IProjectService CreateProjectService()
        {
            WorkflowAuditLogger logger = new WorkflowAuditLogger();
            ProjectService service = new ProjectService(
                new ProjectRepository(),
                new SqlUnitOfWorkFactory(),
                new ProjectValidator(),
                logger);
            return RegisterWorkflowHandlers(service, logger);
        }

        public static IProposalService CreateProposalService()
        {
            WorkflowAuditLogger logger = new WorkflowAuditLogger();
            ProposalService service = new ProposalService(
                new ProjectRepository(),
                new ProposalRepository(),
                new SqlUnitOfWorkFactory(),
                new ProposalValidator(),
                logger);
            return RegisterWorkflowHandlers(service, logger);
        }

        public static IContractService CreateContractService()
        {
            WorkflowAuditLogger logger = new WorkflowAuditLogger();
            ContractService service = new ContractService(
                new ContractRepository(), logger);
            return RegisterWorkflowHandlers(service, logger);
        }

        public static IPaymentService CreatePaymentService()
        {
            WorkflowAuditLogger logger = new WorkflowAuditLogger();
            PaymentService service = new PaymentService(
                new ContractRepository(),
                new ProjectRepository(),
                new ProposalRepository(),
                new PaymentRepository(),
                new SubmissionRepository(),
                new SqlUnitOfWorkFactory(),
                new SimulatedPaymentProcessor(),
                logger);
            return RegisterWorkflowHandlers(service, logger);
        }

        public static ISubmissionService CreateSubmissionService()
        {
            WorkflowAuditLogger logger = new WorkflowAuditLogger();
            FileValidator fileValidator = new FileValidator();
            SubmissionService service = new SubmissionService(
                new ContractRepository(),
                new ProjectRepository(),
                new SubmissionRepository(),
                new PaymentRepository(),
                new SqlUnitOfWorkFactory(),
                new SimulatedPaymentProcessor(),
                new FileUploadHelper(fileValidator),
                logger);
            return RegisterWorkflowHandlers(service, logger);
        }

        public static IReviewService CreateReviewService()
        {
            WorkflowAuditLogger logger = new WorkflowAuditLogger();
            ReviewService service = new ReviewService(
                new ContractRepository(),
                new ReviewRepository(),
                new SqlUnitOfWorkFactory(),
                new ReviewValidator(),
                logger);
            return RegisterWorkflowHandlers(service, logger);
        }

        public static IUserService CreateUserService()
        {
            return new UserService(new UserRepository());
        }

        public static IMessagingService CreateMessagingService()
        {
            return new MessagingService(new MessageRepository(), new UserRepository());
        }

        private static T RegisterWorkflowHandlers<T>(T service, WorkflowAuditLogger logger)
            where T : WorkflowServiceBase
        {
            service.WorkflowChanged += logger.Handle;
#if DEBUG
            service.WorkflowChanged += WorkflowDiagnosticHandler.Record;
#endif
            return service;
        }
    }
}
