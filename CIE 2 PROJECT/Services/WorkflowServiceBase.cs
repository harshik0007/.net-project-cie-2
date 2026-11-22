using System;
using CIE_2_PROJECT.Delegates;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Services
{
    public abstract class WorkflowServiceBase
    {
        private readonly WorkflowAuditLogger auditLogger;
        private bool publishingWorkflowChange;

        public event WorkflowAuditHandler WorkflowChanged;

        protected WorkflowServiceBase(WorkflowAuditLogger auditLogger)
        {
            if (auditLogger == null)
                throw new ArgumentNullException("auditLogger");

            this.auditLogger = auditLogger;
        }

        protected void PublishWorkflowChange(
            WorkflowAuditEntry entry,
            IUnitOfWork unitOfWork)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");
            if (publishingWorkflowChange)
                throw new InvalidOperationException(
                    "Recursive workflow audit events are not allowed.");

            WorkflowAuditHandler handlers = WorkflowChanged;
            if (handlers == null)
                throw new InvalidOperationException(
                    "No workflow audit handler is registered.");

            publishingWorkflowChange = true;
            try
            {
                // Audit participates in the business transaction. A logger
                // failure therefore rolls back the operation instead of
                // reporting a success that has no corresponding audit record.
                using (auditLogger.UseTransaction(unitOfWork))
                {
                    handlers(entry);
                }
            }
            finally
            {
                publishingWorkflowChange = false;
            }
        }
    }
}
