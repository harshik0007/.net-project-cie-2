using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.DataAccess
{
    public sealed class ReviewRepository : BaseRepository, IReviewRepository
    {
        private const string SelectColumns = @"
            ReviewID, ContractID, ReviewerID, RevieweeID, ReviewerRole,
            Rating, ReviewText, CreatedAt";

        public bool ExistsForContract(int contractId, int reviewerId, IUnitOfWork unitOfWork)
        {
            EnsureUnitOfWork(unitOfWork);
            const string sql = @"
                SELECT COUNT(1)
                FROM dbo.ContractReviews WITH (UPDLOCK, HOLDLOCK)
                WHERE ContractID = @ContractID AND ReviewerID = @ReviewerID;";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@ContractID", SqlDbType.Int, contractId);
                AddParameter(command, "@ReviewerID", SqlDbType.Int, reviewerId);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public bool ExistsForContract(int contractId, int reviewerId)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM dbo.ContractReviews
                WHERE ContractID = @ContractID AND ReviewerID = @ReviewerID;";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@ContractID", SqlDbType.Int, contractId);
                AddParameter(command, "@ReviewerID", SqlDbType.Int, reviewerId);
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public IList<Review> GetByRevieweeId(int revieweeId)
        {
            List<Review> reviews = new List<Review>();
            string sql = "SELECT " + SelectColumns +
                " FROM dbo.ContractReviews WHERE RevieweeID = @RevieweeID " +
                "ORDER BY CreatedAt DESC, ReviewID DESC;";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@RevieweeID", SqlDbType.Int, revieweeId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        reviews.Add(Map(reader));
                }
            }

            return reviews;
        }

        public IList<Review> GetByReviewerId(int reviewerId, string reviewerRole)
        {
            List<Review> reviews = new List<Review>();
            string sql = "SELECT " + SelectColumns +
                " FROM dbo.ContractReviews WHERE ReviewerID = @ReviewerID " +
                "AND ReviewerRole = @ReviewerRole " +
                "ORDER BY CreatedAt DESC, ReviewID DESC;";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@ReviewerID", SqlDbType.Int, reviewerId);
                AddParameter(command, "@ReviewerRole", SqlDbType.NVarChar, 20, reviewerRole);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        reviews.Add(Map(reader));
                }
            }

            return reviews;
        }

        public decimal GetAverageRating(int revieweeId)
        {
            const string sql = @"
                SELECT CAST(ISNULL(AVG(CAST(Rating AS decimal(10, 2))), 0)
                            AS decimal(10, 2))
                FROM dbo.ContractReviews
                WHERE RevieweeID = @RevieweeID;";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@RevieweeID", SqlDbType.Int, revieweeId);
                connection.Open();
                return Convert.ToDecimal(command.ExecuteScalar());
            }
        }

        public int GetReviewCount(int revieweeId)
        {
            const string sql = "SELECT COUNT(1) FROM dbo.ContractReviews WHERE RevieweeID = @RevieweeID;";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@RevieweeID", SqlDbType.Int, revieweeId);
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public int Create(Review review, IUnitOfWork unitOfWork)
        {
            if (review == null)
                throw new ArgumentNullException("review");
            EnsureUnitOfWork(unitOfWork);

            const string sql = @"
                INSERT INTO dbo.ContractReviews
                    (ContractID, ReviewerID, RevieweeID, ReviewerRole,
                     Rating, ReviewText, CreatedAt)
                VALUES
                    (@ContractID, @ReviewerID, @RevieweeID, @ReviewerRole,
                     @Rating, @ReviewText, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@ContractID", SqlDbType.Int, review.ContractID);
                AddParameter(command, "@ReviewerID", SqlDbType.Int, review.ReviewerID);
                AddParameter(command, "@RevieweeID", SqlDbType.Int, review.RevieweeID);
                AddParameter(command, "@ReviewerRole", SqlDbType.NVarChar, 20,
                    review.ReviewerRole);
                AddParameter(command, "@Rating", SqlDbType.TinyInt, review.Rating);
                AddParameter(command, "@ReviewText", SqlDbType.NVarChar, -1,
                    DbValue(review.ReviewText));
                AddParameter(command, "@CreatedAt", SqlDbType.DateTime2, review.CreatedAt);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private static Review Map(SqlDataReader reader)
        {
            return new Review(
                Convert.ToInt32(reader["ReviewID"]),
                Convert.ToInt32(reader["ContractID"]),
                Convert.ToInt32(reader["ReviewerID"]),
                Convert.ToInt32(reader["RevieweeID"]),
                Convert.ToString(reader["ReviewerRole"]),
                Convert.ToInt32(reader["Rating"]),
                reader["ReviewText"] == DBNull.Value ? null : Convert.ToString(reader["ReviewText"]),
                Convert.ToDateTime(reader["CreatedAt"]));
        }

        private static void EnsureUnitOfWork(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null || unitOfWork.Connection == null || unitOfWork.Transaction == null)
                throw new ArgumentNullException("unitOfWork");
        }
    }
}
