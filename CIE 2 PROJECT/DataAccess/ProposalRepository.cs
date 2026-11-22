using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.DataAccess
{
    public sealed class ProposalRepository : BaseRepository, IProposalRepository
    {
        private const string SelectColumns = @"
            ProposalID, ProjectID, FreelancerID, CoverLetter, BidAmount,
            EstimatedDays, AttachmentPath, Status, SubmittedAt, AcceptedAt";

        public Proposal GetByIdForUpdate(int proposalId, IUnitOfWork unitOfWork)
        {
            EnsureUnitOfWork(unitOfWork);
            using (SqlCommand command = new SqlCommand(
                "SELECT " + SelectColumns + " FROM dbo.Proposals WITH (UPDLOCK, ROWLOCK) " +
                "WHERE ProposalID = @ProposalID;",
                unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@ProposalID", SqlDbType.Int, proposalId);
                using (SqlDataReader reader = command.ExecuteReader())
                    return reader.Read() ? Map(reader) : null;
            }
        }

        public IList<Proposal> GetByProjectId(int projectId)
        {
            return GetByIdList("ProjectID", projectId);
        }

        public IList<Proposal> GetByFreelancerId(int freelancerId)
        {
            return GetByIdList("FreelancerID", freelancerId);
        }

        public int GetCountByProjectId(int projectId)
        {
            const string sql = "SELECT COUNT(1) FROM dbo.Proposals WHERE ProjectID = @ProjectID;";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@ProjectID", SqlDbType.Int, projectId);
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public bool HasProposal(int projectId, int freelancerId, IUnitOfWork unitOfWork)
        {
            EnsureUnitOfWork(unitOfWork);
            const string sql = @"
                SELECT COUNT(1)
                FROM dbo.Proposals WITH (UPDLOCK, HOLDLOCK)
                WHERE ProjectID = @ProjectID AND FreelancerID = @FreelancerID
                  AND Status IN (@Pending, @Accepted);";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@ProjectID", SqlDbType.Int, projectId);
                AddParameter(command, "@FreelancerID", SqlDbType.Int, freelancerId);
                AddParameter(command, "@Pending", SqlDbType.NVarChar, 30,
                    WorkflowStatuses.ProposalPending);
                AddParameter(command, "@Accepted", SqlDbType.NVarChar, 30,
                    WorkflowStatuses.ProposalAccepted);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public int Create(Proposal proposal, IUnitOfWork unitOfWork)
        {
            if (proposal == null)
                throw new ArgumentNullException("proposal");
            EnsureUnitOfWork(unitOfWork);

            const string sql = @"
                INSERT INTO dbo.Proposals
                    (ProjectID, FreelancerID, CoverLetter, BidAmount,
                     EstimatedDays, AttachmentPath, Status, SubmittedAt)
                VALUES
                    (@ProjectID, @FreelancerID, @CoverLetter, @BidAmount,
                     @EstimatedDays, @AttachmentPath, @Status, @SubmittedAt);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddProposalParameters(command, proposal);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(Proposal proposal, IUnitOfWork unitOfWork)
        {
            if (proposal == null)
                throw new ArgumentNullException("proposal");
            EnsureUnitOfWork(unitOfWork);

            const string sql = @"
                UPDATE dbo.Proposals
                SET Status = @Status, AcceptedAt = @AcceptedAt
                WHERE ProposalID = @ProposalID AND ProjectID = @ProjectID;";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@Status", SqlDbType.NVarChar, 30, proposal.Status);
                AddParameter(command, "@AcceptedAt", SqlDbType.DateTime2,
                    DbValue(proposal.AcceptedAt));
                AddParameter(command, "@ProposalID", SqlDbType.Int, proposal.ProposalID);
                AddParameter(command, "@ProjectID", SqlDbType.Int, proposal.ProjectID);
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("The proposal could not be updated.");
            }
        }

        public void RejectOtherPending(
            int projectId,
            int acceptedProposalId,
            IUnitOfWork unitOfWork)
        {
            EnsureUnitOfWork(unitOfWork);
            const string sql = @"
                UPDATE dbo.Proposals
                SET Status = @Rejected
                WHERE ProjectID = @ProjectID AND ProposalID <> @AcceptedProposalID
                  AND Status = @Pending;";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@Rejected", SqlDbType.NVarChar, 30,
                    WorkflowStatuses.ProposalRejected);
                AddParameter(command, "@ProjectID", SqlDbType.Int, projectId);
                AddParameter(command, "@AcceptedProposalID", SqlDbType.Int, acceptedProposalId);
                AddParameter(command, "@Pending", SqlDbType.NVarChar, 30,
                    WorkflowStatuses.ProposalPending);
                command.ExecuteNonQuery();
            }
        }

        private IList<Proposal> GetByIdList(string fieldName, int id)
        {
            if (fieldName != "ProjectID" && fieldName != "FreelancerID")
                throw new ArgumentException("Unsupported proposal filter.", "fieldName");

            List<Proposal> proposals = new List<Proposal>();
            string sql = "SELECT " + SelectColumns + " FROM dbo.Proposals WHERE " +
                fieldName + " = @ID ORDER BY SubmittedAt DESC, ProposalID DESC;";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@ID", SqlDbType.Int, id);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        proposals.Add(Map(reader));
                }
            }

            return proposals;
        }

        private static Proposal Map(SqlDataReader reader)
        {
            return new Proposal(
                Convert.ToInt32(reader["ProposalID"]),
                Convert.ToInt32(reader["ProjectID"]),
                Convert.ToInt32(reader["FreelancerID"]),
                Convert.ToString(reader["CoverLetter"]),
                Convert.ToDecimal(reader["BidAmount"]),
                Convert.ToInt32(reader["EstimatedDays"]),
                reader["AttachmentPath"] == DBNull.Value ? null : Convert.ToString(reader["AttachmentPath"]),
                Convert.ToString(reader["Status"]),
                Convert.ToDateTime(reader["SubmittedAt"]),
                reader["AcceptedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["AcceptedAt"]));
        }

        private static void AddProposalParameters(SqlCommand command, Proposal proposal)
        {
            AddParameter(command, "@ProjectID", SqlDbType.Int, proposal.ProjectID);
            AddParameter(command, "@FreelancerID", SqlDbType.Int, proposal.FreelancerID);
            AddParameter(command, "@CoverLetter", SqlDbType.NVarChar, -1, proposal.CoverLetter);
            AddParameter(command, "@BidAmount", SqlDbType.Decimal, proposal.BidAmount);
            command.Parameters["@BidAmount"].Precision = 18;
            command.Parameters["@BidAmount"].Scale = 2;
            AddParameter(command, "@EstimatedDays", SqlDbType.Int, proposal.EstimatedDays);
            AddParameter(command, "@AttachmentPath", SqlDbType.NVarChar, 500,
                DbValue(proposal.AttachmentPath));
            AddParameter(command, "@Status", SqlDbType.NVarChar, 30, proposal.Status);
            AddParameter(command, "@SubmittedAt", SqlDbType.DateTime2, proposal.SubmittedAt);
        }

        private static void EnsureUnitOfWork(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null || unitOfWork.Connection == null || unitOfWork.Transaction == null)
                throw new ArgumentNullException("unitOfWork");
        }
    }
}
