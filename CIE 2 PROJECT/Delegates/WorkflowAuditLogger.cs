using System;
using System.Data;
using System.Data.SqlClient;
using CIE_2_PROJECT.DataAccess;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Delegates
{
    public sealed class WorkflowAuditLogger : BaseRepository
    {
        private SqlConnection activeConnection;
        private SqlTransaction activeTransaction;

        public IDisposable UseTransaction(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");
            if (activeTransaction != null)
                throw new InvalidOperationException("An audit transaction is already active.");

            activeConnection = unitOfWork.Connection;
            activeTransaction = unitOfWork.Transaction;
            return new TransactionBinding(this);
        }

        public void Handle(WorkflowAuditEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");
            if (activeConnection == null || activeTransaction == null)
                throw new InvalidOperationException(
                    "Workflow audit records must be written inside the workflow transaction.");

            const string sql = @"
                INSERT INTO dbo.WorkflowAuditLogs
                    (ActorUserID, EntityType, EntityID, [Action], OldStatus,
                     NewStatus, [Description], CreatedAt)
                VALUES
                    (@ActorUserID, @EntityType, @EntityID, @Action, @OldStatus,
                     @NewStatus, @Description, @CreatedAt);";

            using (SqlCommand command = new SqlCommand(
                sql, activeConnection, activeTransaction))
            {
                AddParameter(command, "@ActorUserID", SqlDbType.Int, entry.ActorUserID);
                AddParameter(command, "@EntityType", SqlDbType.NVarChar, 40, entry.EntityType);
                AddParameter(command, "@EntityID", SqlDbType.Int, entry.EntityID);
                AddParameter(command, "@Action", SqlDbType.NVarChar, 60, entry.Action);
                AddParameter(command, "@OldStatus", SqlDbType.NVarChar, 40, DbValue(entry.OldStatus));
                AddParameter(command, "@NewStatus", SqlDbType.NVarChar, 40, DbValue(entry.NewStatus));
                AddParameter(command, "@Description", SqlDbType.NVarChar, -1, DbValue(entry.Description));
                AddParameter(command, "@CreatedAt", SqlDbType.DateTime2, entry.CreatedAt);
                command.ExecuteNonQuery();
            }
        }

        private void ClearTransaction()
        {
            activeTransaction = null;
            activeConnection = null;
        }

        private sealed class TransactionBinding : IDisposable
        {
            private WorkflowAuditLogger logger;

            public TransactionBinding(WorkflowAuditLogger logger)
            {
                this.logger = logger;
            }

            public void Dispose()
            {
                if (logger == null)
                    return;

                logger.ClearTransaction();
                logger = null;
            }
        }
    }
}
