using System;
using System.Data;
using System.Data.SqlClient;
using CIE_2_PROJECT.Interfaces;

namespace CIE_2_PROJECT.DataAccess
{
    public sealed class SqlUnitOfWork : IUnitOfWork
    {
        private bool completed;

        public SqlConnection Connection { get; private set; }
        public SqlTransaction Transaction { get; private set; }

        public SqlUnitOfWork(string connectionString, IsolationLevel isolationLevel)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("A database connection string is required.", "connectionString");

            Connection = new SqlConnection(connectionString);
            Connection.Open();

            try
            {
                Transaction = Connection.BeginTransaction(isolationLevel);
            }
            catch
            {
                Connection.Dispose();
                Connection = null;
                throw;
            }
        }

        public void Commit()
        {
            EnsureActive();
            Transaction.Commit();
            completed = true;
        }

        public void Rollback()
        {
            if (completed || Transaction == null)
                return;

            Transaction.Rollback();
            completed = true;
        }

        public void Dispose()
        {
            if (!completed && Transaction != null)
            {
                try
                {
                    Transaction.Rollback();
                }
                catch (InvalidOperationException)
                {
                    // The server may already have rolled the transaction back.
                }
                catch (SqlException)
                {
                    // Preserve the original failure while still disposing resources.
                }
            }

            if (Transaction != null)
                Transaction.Dispose();
            if (Connection != null)
                Connection.Dispose();

            Transaction = null;
            Connection = null;
            completed = true;
        }

        private void EnsureActive()
        {
            if (completed || Transaction == null)
                throw new InvalidOperationException("The database transaction is no longer active.");
        }
    }
}
