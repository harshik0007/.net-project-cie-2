using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.DataAccess
{
    public sealed class SubmissionRepository : BaseRepository, ISubmissionRepository
    {
        private const string SelectColumns = @"
            SubmissionID, ContractID, SubmissionNumber, CompletionMessage,
            DemoURL, GithubURL, SubmittedAt, SubmissionStatus,
            ClientFeedback, ReviewedAt";

        public WorkSubmission GetById(int submissionId)
        {
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(
                "SELECT " + SelectColumns + " FROM dbo.ContractSubmissions " +
                "WHERE SubmissionID = @SubmissionID;", connection))
            {
                AddParameter(command, "@SubmissionID", SqlDbType.Int, submissionId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                    return reader.Read() ? Map(reader) : null;
            }
        }

        public WorkSubmission GetLatestByContractId(int contractId, IUnitOfWork unitOfWork)
        {
            EnsureUnitOfWork(unitOfWork);
            const string sql = @"
                SELECT TOP (1) SubmissionID, ContractID, SubmissionNumber,
                       CompletionMessage, DemoURL, GithubURL, SubmittedAt,
                       SubmissionStatus, ClientFeedback, ReviewedAt
                FROM dbo.ContractSubmissions WITH (UPDLOCK, HOLDLOCK)
                WHERE ContractID = @ContractID
                ORDER BY SubmissionNumber DESC;";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@ContractID", SqlDbType.Int, contractId);
                using (SqlDataReader reader = command.ExecuteReader())
                    return reader.Read() ? Map(reader) : null;
            }
        }

        public IList<WorkSubmission> GetByContractId(int contractId)
        {
            List<WorkSubmission> submissions = new List<WorkSubmission>();
            string sql = "SELECT " + SelectColumns +
                " FROM dbo.ContractSubmissions WHERE ContractID = @ContractID " +
                "ORDER BY SubmissionNumber DESC;";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@ContractID", SqlDbType.Int, contractId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        submissions.Add(Map(reader));
                }
            }

            return submissions;
        }

        public IList<SubmissionFile> GetFilesBySubmissionId(int submissionId)
        {
            List<SubmissionFile> files = new List<SubmissionFile>();
            const string sql = @"
                SELECT ContractFileID AS SubmissionFileID, SubmissionID, OriginalFileName,
                       StoredFileName, FilePath, ContentType, FileSize, UploadedAt
                FROM dbo.ContractFiles
                WHERE SubmissionID = @SubmissionID
                ORDER BY ContractFileID;";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@SubmissionID", SqlDbType.Int, submissionId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        files.Add(new SubmissionFile(
                            Convert.ToInt32(reader["SubmissionFileID"]),
                            Convert.ToInt32(reader["SubmissionID"]),
                            Convert.ToString(reader["OriginalFileName"]),
                            Convert.ToString(reader["StoredFileName"]),
                            Convert.ToString(reader["FilePath"]),
                            reader["ContentType"] == DBNull.Value ? null : Convert.ToString(reader["ContentType"]),
                            Convert.ToInt64(reader["FileSize"]),
                            Convert.ToDateTime(reader["UploadedAt"])));
                    }
                }
            }

            return files;
        }

        public int GetNextSubmissionNumber(int contractId, IUnitOfWork unitOfWork)
        {
            EnsureUnitOfWork(unitOfWork);
            const string sql = @"
                SELECT ISNULL(MAX(SubmissionNumber), 0) + 1
                FROM dbo.ContractSubmissions WITH (UPDLOCK, HOLDLOCK)
                WHERE ContractID = @ContractID;";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@ContractID", SqlDbType.Int, contractId);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public int Create(
            WorkSubmission submission,
            IList<SubmissionFile> files,
            IUnitOfWork unitOfWork)
        {
            if (submission == null)
                throw new ArgumentNullException("submission");
            EnsureUnitOfWork(unitOfWork);

            const string sql = @"
                INSERT INTO dbo.ContractSubmissions
                    (ContractID, SubmissionNumber, CompletionMessage, DemoURL,
                     GithubURL, SubmittedAt, SubmissionStatus)
                VALUES
                    (@ContractID, @SubmissionNumber, @CompletionMessage, @DemoURL,
                     @GithubURL, @SubmittedAt, @SubmissionStatus);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            int submissionId;
            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddSubmissionParameters(command, submission);
                submissionId = Convert.ToInt32(command.ExecuteScalar());
            }

            if (files != null)
            {
                foreach (SubmissionFile file in files)
                    InsertFile(submission.ContractID, submissionId, file, unitOfWork);
            }

            return submissionId;
        }

        public void Update(WorkSubmission submission, IUnitOfWork unitOfWork)
        {
            if (submission == null)
                throw new ArgumentNullException("submission");
            EnsureUnitOfWork(unitOfWork);

            const string sql = @"
                UPDATE dbo.ContractSubmissions
                SET SubmissionStatus = @SubmissionStatus,
                    ClientFeedback = @ClientFeedback,
                    ReviewedAt = @ReviewedAt
                WHERE SubmissionID = @SubmissionID AND ContractID = @ContractID;";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@SubmissionStatus", SqlDbType.NVarChar, 30,
                    submission.SubmissionStatus);
                AddParameter(command, "@ClientFeedback", SqlDbType.NVarChar, -1,
                    DbValue(submission.ClientFeedback));
                AddParameter(command, "@ReviewedAt", SqlDbType.DateTime2,
                    DbValue(submission.ReviewedAt));
                AddParameter(command, "@SubmissionID", SqlDbType.Int, submission.SubmissionID);
                AddParameter(command, "@ContractID", SqlDbType.Int, submission.ContractID);
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("The work submission could not be updated.");
            }
        }

        private static void InsertFile(
            int contractId,
            int submissionId,
            SubmissionFile file,
            IUnitOfWork unitOfWork)
        {
            if (file == null)
                throw new ArgumentException("A submission file cannot be null.", "file");

            const string sql = @"
                INSERT INTO dbo.ContractFiles
                    (ContractID, SubmissionID, OriginalFileName, StoredFileName, FilePath,
                     ContentType, FileSize, UploadedAt)
                VALUES
                    (@ContractID, @SubmissionID, @OriginalFileName, @StoredFileName, @FilePath,
                     @ContentType, @FileSize, @UploadedAt);";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@ContractID", SqlDbType.Int, contractId);
                AddParameter(command, "@SubmissionID", SqlDbType.Int, submissionId);
                AddParameter(command, "@OriginalFileName", SqlDbType.NVarChar, 255,
                    file.OriginalFileName);
                AddParameter(command, "@StoredFileName", SqlDbType.NVarChar, 255,
                    file.StoredFileName);
                AddParameter(command, "@FilePath", SqlDbType.NVarChar, 500,
                    file.FilePath);
                AddParameter(command, "@ContentType", SqlDbType.NVarChar, 100,
                    DbValue(file.ContentType));
                AddParameter(command, "@FileSize", SqlDbType.BigInt, file.FileSize);
                AddParameter(command, "@UploadedAt", SqlDbType.DateTime2, file.UploadedAt);
                command.ExecuteNonQuery();
            }
        }

        private static WorkSubmission Map(SqlDataReader reader)
        {
            return new WorkSubmission(
                Convert.ToInt32(reader["SubmissionID"]),
                Convert.ToInt32(reader["ContractID"]),
                Convert.ToInt32(reader["SubmissionNumber"]),
                Convert.ToString(reader["CompletionMessage"]),
                reader["DemoURL"] == DBNull.Value ? null : Convert.ToString(reader["DemoURL"]),
                reader["GithubURL"] == DBNull.Value ? null : Convert.ToString(reader["GithubURL"]),
                Convert.ToDateTime(reader["SubmittedAt"]),
                Convert.ToString(reader["SubmissionStatus"]),
                reader["ClientFeedback"] == DBNull.Value ? null : Convert.ToString(reader["ClientFeedback"]),
                reader["ReviewedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ReviewedAt"]));
        }

        private static void AddSubmissionParameters(SqlCommand command, WorkSubmission submission)
        {
            AddParameter(command, "@ContractID", SqlDbType.Int, submission.ContractID);
            AddParameter(command, "@SubmissionNumber", SqlDbType.Int, submission.SubmissionNumber);
            AddParameter(command, "@CompletionMessage", SqlDbType.NVarChar, -1,
                submission.CompletionMessage);
            AddParameter(command, "@DemoURL", SqlDbType.NVarChar, 2048,
                DbValue(submission.DemoURL));
            AddParameter(command, "@GithubURL", SqlDbType.NVarChar, 2048,
                DbValue(submission.GithubURL));
            AddParameter(command, "@SubmittedAt", SqlDbType.DateTime2, submission.SubmittedAt);
            AddParameter(command, "@SubmissionStatus", SqlDbType.NVarChar, 30,
                submission.SubmissionStatus);
        }

        private static void EnsureUnitOfWork(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null || unitOfWork.Connection == null || unitOfWork.Transaction == null)
                throw new ArgumentNullException("unitOfWork");
        }
    }
}
