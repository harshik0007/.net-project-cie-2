using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.DataAccess
{
    public sealed class ContractRepository : BaseRepository, IContractRepository
    {
        private const string SelectColumns = @"
            ContractID, ProjectID, ProposalID, ClientID, FreelancerID,
            AgreedAmount, WorkDescription, ApprovalStatus, CreatedAt,
            WorkSubmittedAt, CompletedAt, CompletionMessage";

        public Contract GetById(int contractId)
        {
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(
                "SELECT " + SelectColumns + " FROM dbo.Contracts WHERE ContractID = @ContractID;",
                connection))
            {
                AddParameter(command, "@ContractID", SqlDbType.Int, contractId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                    return reader.Read() ? Map(reader) : null;
            }
        }

        public Contract GetByProjectId(int projectId)
        {
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(
                "SELECT " + SelectColumns + " FROM dbo.Contracts WHERE ProjectID = @ProjectID;",
                connection))
            {
                AddParameter(command, "@ProjectID", SqlDbType.Int, projectId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                    return reader.Read() ? Map(reader) : null;
            }
        }

        public IList<Contract> GetByClientId(int clientId)
        {
            List<Contract> contracts = new List<Contract>();
            string sql = "SELECT " + SelectColumns +
                " FROM dbo.Contracts WHERE ClientID = @ClientID " +
                "ORDER BY CreatedAt DESC, ContractID DESC;";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@ClientID", SqlDbType.Int, clientId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        contracts.Add(Map(reader));
                }
            }
            return contracts;
        }

        public IList<Contract> GetByFreelancerId(int freelancerId)
        {
            List<Contract> contracts = new List<Contract>();
            string sql = "SELECT " + SelectColumns +
                " FROM dbo.Contracts WHERE FreelancerID = @FreelancerID " +
                "ORDER BY CreatedAt DESC, ContractID DESC;";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@FreelancerID", SqlDbType.Int, freelancerId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        contracts.Add(Map(reader));
                }
            }
            return contracts;
        }

        public Contract GetByIdForUpdate(int contractId, IUnitOfWork unitOfWork)
        {
            EnsureUnitOfWork(unitOfWork);
            using (SqlCommand command = new SqlCommand(
                "SELECT " + SelectColumns + " FROM dbo.Contracts WITH (UPDLOCK, ROWLOCK) " +
                "WHERE ContractID = @ContractID;",
                unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@ContractID", SqlDbType.Int, contractId);
                using (SqlDataReader reader = command.ExecuteReader())
                    return reader.Read() ? Map(reader) : null;
            }
        }

        public Contract GetByProjectId(int projectId, IUnitOfWork unitOfWork)
        {
            EnsureUnitOfWork(unitOfWork);
            using (SqlCommand command = new SqlCommand(
                "SELECT " + SelectColumns + " FROM dbo.Contracts WITH (UPDLOCK, HOLDLOCK) " +
                "WHERE ProjectID = @ProjectID;",
                unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@ProjectID", SqlDbType.Int, projectId);
                using (SqlDataReader reader = command.ExecuteReader())
                    return reader.Read() ? Map(reader) : null;
            }
        }

        public IList<Contract> GetActiveByFreelancerId(int freelancerId)
        {
            List<Contract> contracts = new List<Contract>();
            const string sql = @"
                SELECT ContractID, ProjectID, ProposalID, ClientID, FreelancerID,
                       AgreedAmount, WorkDescription, ApprovalStatus, CreatedAt,
                       WorkSubmittedAt, CompletedAt, CompletionMessage
                FROM dbo.Contracts
                WHERE FreelancerID = @FreelancerID
                  AND ApprovalStatus NOT IN (@Completed, @Cancelled)
                ORDER BY CreatedAt DESC, ContractID DESC;";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@FreelancerID", SqlDbType.Int, freelancerId);
                AddParameter(command, "@Completed", SqlDbType.NVarChar, 40,
                    WorkflowStatuses.ContractCompleted);
                AddParameter(command, "@Cancelled", SqlDbType.NVarChar, 40,
                    WorkflowStatuses.ContractCancelled);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        contracts.Add(Map(reader));
                }
            }

            return contracts;
        }

        public int CountActiveByFreelancerIdForUpdate(int freelancerId, IUnitOfWork unitOfWork)
        {
            EnsureUnitOfWork(unitOfWork);
            const string sql = @"
                SELECT COUNT(1)
                FROM dbo.Contracts WITH (UPDLOCK, HOLDLOCK)
                WHERE FreelancerID = @FreelancerID
                  AND ApprovalStatus NOT IN (@Completed, @Cancelled);";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@FreelancerID", SqlDbType.Int, freelancerId);
                AddParameter(command, "@Completed", SqlDbType.NVarChar, 40,
                    WorkflowStatuses.ContractCompleted);
                AddParameter(command, "@Cancelled", SqlDbType.NVarChar, 40,
                    WorkflowStatuses.ContractCancelled);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public int Create(Contract contract, IUnitOfWork unitOfWork)
        {
            if (contract == null)
                throw new ArgumentNullException("contract");
            EnsureUnitOfWork(unitOfWork);

            const string sql = @"
                INSERT INTO dbo.Contracts
                    (ProjectID, ProposalID, ClientID, FreelancerID, AgreedAmount,
                     PaymentStatus, EscrowStatus, WorkDescription, ApprovalStatus, CreatedAt)
                VALUES
                    (@ProjectID, @ProposalID, @ClientID, @FreelancerID, @AgreedAmount,
                     @PaymentStatus, @EscrowStatus, @WorkDescription, @ApprovalStatus, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddContractParameters(command, contract);
                AddParameter(command, "@PaymentStatus", SqlDbType.NVarChar, 30,
                    WorkflowStatuses.PaymentNotFunded);
                AddParameter(command, "@EscrowStatus", SqlDbType.NVarChar, 30,
                    WorkflowStatuses.EscrowNotFunded);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(Contract contract, IUnitOfWork unitOfWork)
        {
            if (contract == null)
                throw new ArgumentNullException("contract");
            EnsureUnitOfWork(unitOfWork);

            const string sql = @"
                UPDATE dbo.Contracts
                SET WorkDescription = @WorkDescription,
                    ApprovalStatus = @ApprovalStatus,
                    WorkSubmittedAt = @WorkSubmittedAt,
                    CompletedAt = @CompletedAt,
                    CompletionMessage = @CompletionMessage
                WHERE ContractID = @ContractID;";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@WorkDescription", SqlDbType.NVarChar, -1,
                    DbValue(contract.WorkDescription));
                AddParameter(command, "@ApprovalStatus", SqlDbType.NVarChar, 40,
                    contract.ApprovalStatus);
                AddParameter(command, "@WorkSubmittedAt", SqlDbType.DateTime2,
                    DbValue(contract.WorkSubmittedAt));
                AddParameter(command, "@CompletedAt", SqlDbType.DateTime2,
                    DbValue(contract.CompletedAt));
                AddParameter(command, "@CompletionMessage", SqlDbType.NVarChar, -1,
                    DbValue(contract.CompletionMessage));
                AddParameter(command, "@ContractID", SqlDbType.Int, contract.ContractID);
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("The contract could not be updated.");
            }
        }

        private static Contract Map(SqlDataReader reader)
        {
            return new Contract(
                Convert.ToInt32(reader["ContractID"]),
                Convert.ToInt32(reader["ProjectID"]),
                Convert.ToInt32(reader["ProposalID"]),
                Convert.ToInt32(reader["ClientID"]),
                Convert.ToInt32(reader["FreelancerID"]),
                Convert.ToDecimal(reader["AgreedAmount"]),
                reader["WorkDescription"] == DBNull.Value ? null : Convert.ToString(reader["WorkDescription"]),
                Convert.ToString(reader["ApprovalStatus"]),
                Convert.ToDateTime(reader["CreatedAt"]),
                reader["WorkSubmittedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["WorkSubmittedAt"]),
                reader["CompletedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CompletedAt"]),
                reader["CompletionMessage"] == DBNull.Value ? null : Convert.ToString(reader["CompletionMessage"]));
        }

        private static void AddContractParameters(SqlCommand command, Contract contract)
        {
            AddParameter(command, "@ProjectID", SqlDbType.Int, contract.ProjectID);
            AddParameter(command, "@ProposalID", SqlDbType.Int, contract.ProposalID);
            AddParameter(command, "@ClientID", SqlDbType.Int, contract.ClientID);
            AddParameter(command, "@FreelancerID", SqlDbType.Int, contract.FreelancerID);
            AddParameter(command, "@AgreedAmount", SqlDbType.Decimal, contract.AgreedAmount);
            command.Parameters["@AgreedAmount"].Precision = 18;
            command.Parameters["@AgreedAmount"].Scale = 2;
            AddParameter(command, "@WorkDescription", SqlDbType.NVarChar, -1,
                DbValue(contract.WorkDescription));
            AddParameter(command, "@ApprovalStatus", SqlDbType.NVarChar, 40,
                contract.ApprovalStatus);
            AddParameter(command, "@CreatedAt", SqlDbType.DateTime2, contract.CreatedAt);
        }

        private static void EnsureUnitOfWork(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null || unitOfWork.Connection == null || unitOfWork.Transaction == null)
                throw new ArgumentNullException("unitOfWork");
        }
    }
}
