using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.DataAccess
{
    public sealed class PaymentRepository : BaseRepository, IPaymentRepository
    {
        private const string SelectColumns = @"
            PaymentID, ContractID, ClientID, FreelancerID, Amount,
            PaymentStatus, PaymentMethod, CreatedAt, FundedAt, ReleasedAt,
            ClientRefundAmount, FreelancerPayoutAmount, PlatformFeeAmount,
            GstRate, GstAmount, ClientTotalAmount,
            ClientPlatformFeeRate, ClientPlatformFeeAmount,
            FreelancerNetAmount, FreelancerPlatformFeeRate, FreelancerPlatformFeeAmount,
            GstRefundAmount, ClientPlatformFeeRefundAmount";

        public Payment GetByContractId(int contractId, IUnitOfWork unitOfWork)
        {
            EnsureUnitOfWork(unitOfWork);
            using (SqlCommand command = new SqlCommand(
                "SELECT " + SelectColumns + " FROM dbo.ContractPayments WITH (UPDLOCK, HOLDLOCK) " +
                "WHERE ContractID = @ContractID;",
                unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@ContractID", SqlDbType.Int, contractId);
                using (SqlDataReader reader = command.ExecuteReader())
                    return reader.Read() ? Map(reader) : null;
            }
        }

        public IList<Payment> GetByClientId(int clientId)
        {
            return GetByParticipant("ClientID", clientId);
        }

        public IList<Payment> GetByFreelancerId(int freelancerId)
        {
            return GetByParticipant("FreelancerID", freelancerId);
        }

        public int Create(Payment payment, IUnitOfWork unitOfWork)
        {
            if (payment == null)
                throw new ArgumentNullException("payment");
            EnsureUnitOfWork(unitOfWork);

            const string sql = @"
                INSERT INTO dbo.ContractPayments
                    (ContractID, ClientID, FreelancerID, Amount, PaymentStatus,
                     PaymentMethod, CreatedAt, FundedAt, ReleasedAt,
                     ClientRefundAmount, FreelancerPayoutAmount, PlatformFeeAmount,
                     GstRate, GstAmount, ClientTotalAmount,
                     ClientPlatformFeeRate, ClientPlatformFeeAmount,
                     FreelancerNetAmount, FreelancerPlatformFeeRate, FreelancerPlatformFeeAmount,
                     GstRefundAmount, ClientPlatformFeeRefundAmount)
                VALUES
                    (@ContractID, @ClientID, @FreelancerID, @Amount, @PaymentStatus,
                     @PaymentMethod, @CreatedAt, @FundedAt, @ReleasedAt,
                     @ClientRefundAmount, @FreelancerPayoutAmount, @PlatformFeeAmount,
                     @GstRate, @GstAmount, @ClientTotalAmount,
                     @ClientPlatformFeeRate, @ClientPlatformFeeAmount,
                     @FreelancerNetAmount, @FreelancerPlatformFeeRate, @FreelancerPlatformFeeAmount,
                     @GstRefundAmount, @ClientPlatformFeeRefundAmount);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddPaymentParameters(command, payment);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(Payment payment, IUnitOfWork unitOfWork)
        {
            if (payment == null)
                throw new ArgumentNullException("payment");
            EnsureUnitOfWork(unitOfWork);

            const string sql = @"
                UPDATE dbo.ContractPayments
                SET PaymentStatus = @PaymentStatus,
                    FundedAt = @FundedAt,
                    ReleasedAt = @ReleasedAt,
                    ClientRefundAmount = @ClientRefundAmount,
                    FreelancerPayoutAmount = @FreelancerPayoutAmount,
                    PlatformFeeAmount = @PlatformFeeAmount,
                    GstRefundAmount = @GstRefundAmount,
                    ClientPlatformFeeRefundAmount = @ClientPlatformFeeRefundAmount
                WHERE PaymentID = @PaymentID AND ContractID = @ContractID;";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@PaymentStatus", SqlDbType.NVarChar, 30,
                    payment.PaymentStatus);
                AddParameter(command, "@FundedAt", SqlDbType.DateTime2, DbValue(payment.FundedAt));
                AddParameter(command, "@ReleasedAt", SqlDbType.DateTime2, DbValue(payment.ReleasedAt));
                AddAllocationParameters(command, payment);
                AddParameter(command, "@PaymentID", SqlDbType.Int, payment.PaymentID);
                AddParameter(command, "@ContractID", SqlDbType.Int, payment.ContractID);
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("The payment could not be updated.");
            }
        }

        private IList<Payment> GetByParticipant(string fieldName, int userId)
        {
            if (fieldName != "ClientID" && fieldName != "FreelancerID")
                throw new ArgumentException("Unsupported payment participant.", "fieldName");

            List<Payment> payments = new List<Payment>();
            string sql = "SELECT " + SelectColumns + " FROM dbo.ContractPayments WHERE " +
                fieldName + " = @UserID ORDER BY CreatedAt DESC, PaymentID DESC;";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@UserID", SqlDbType.Int, userId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        payments.Add(Map(reader));
                }
            }

            return payments;
        }

        private static Payment Map(SqlDataReader reader)
        {
            return new Payment(
                Convert.ToInt32(reader["PaymentID"]),
                Convert.ToInt32(reader["ContractID"]),
                Convert.ToInt32(reader["ClientID"]),
                Convert.ToInt32(reader["FreelancerID"]),
                Convert.ToDecimal(reader["Amount"]),
                Convert.ToString(reader["PaymentStatus"]),
                reader["PaymentMethod"] == DBNull.Value ? null : Convert.ToString(reader["PaymentMethod"]),
                Convert.ToDateTime(reader["CreatedAt"]),
                reader["FundedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["FundedAt"]),
                reader["ReleasedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ReleasedAt"]),
                Convert.ToDecimal(reader["ClientRefundAmount"]),
                Convert.ToDecimal(reader["FreelancerPayoutAmount"]),
                Convert.ToDecimal(reader["PlatformFeeAmount"]),
                Convert.ToDecimal(reader["GstRate"]),
                Convert.ToDecimal(reader["GstAmount"]),
                Convert.ToDecimal(reader["ClientTotalAmount"]),
                Convert.ToDecimal(reader["ClientPlatformFeeRate"]),
                Convert.ToDecimal(reader["ClientPlatformFeeAmount"]),
                Convert.ToDecimal(reader["FreelancerNetAmount"]),
                Convert.ToDecimal(reader["FreelancerPlatformFeeRate"]),
                Convert.ToDecimal(reader["FreelancerPlatformFeeAmount"]),
                Convert.ToDecimal(reader["GstRefundAmount"]),
                Convert.ToDecimal(reader["ClientPlatformFeeRefundAmount"]));
        }

        private static void AddPaymentParameters(SqlCommand command, Payment payment)
        {
            AddParameter(command, "@ContractID", SqlDbType.Int, payment.ContractID);
            AddParameter(command, "@ClientID", SqlDbType.Int, payment.ClientID);
            AddParameter(command, "@FreelancerID", SqlDbType.Int, payment.FreelancerID);
            AddParameter(command, "@Amount", SqlDbType.Decimal, payment.Amount);
            command.Parameters["@Amount"].Precision = 18;
            command.Parameters["@Amount"].Scale = 2;
            AddParameter(command, "@PaymentStatus", SqlDbType.NVarChar, 30,
                payment.PaymentStatus);
            AddParameter(command, "@PaymentMethod", SqlDbType.NVarChar, 60,
                DbValue(payment.PaymentMethod));
            AddParameter(command, "@CreatedAt", SqlDbType.DateTime2, payment.CreatedAt);
            AddParameter(command, "@FundedAt", SqlDbType.DateTime2, DbValue(payment.FundedAt));
            AddParameter(command, "@ReleasedAt", SqlDbType.DateTime2, DbValue(payment.ReleasedAt));
            AddAllocationParameters(command, payment);
            AddGstParameters(command, payment);
        }

        private static void AddAllocationParameters(SqlCommand command, Payment payment)
        {
            AddParameter(command, "@ClientRefundAmount", SqlDbType.Decimal, payment.ClientRefundAmount);
            AddParameter(command, "@FreelancerPayoutAmount", SqlDbType.Decimal, payment.FreelancerPayoutAmount);
            AddParameter(command, "@PlatformFeeAmount", SqlDbType.Decimal, payment.PlatformFeeAmount);
            AddParameter(command, "@ClientPlatformFeeRefundAmount", SqlDbType.Decimal,
                payment.ClientPlatformFeeRefundAmount);
            command.Parameters["@ClientRefundAmount"].Precision = 18;
            command.Parameters["@ClientRefundAmount"].Scale = 2;
            command.Parameters["@FreelancerPayoutAmount"].Precision = 18;
            command.Parameters["@FreelancerPayoutAmount"].Scale = 2;
            command.Parameters["@PlatformFeeAmount"].Precision = 18;
            command.Parameters["@PlatformFeeAmount"].Scale = 2;
            command.Parameters["@ClientPlatformFeeRefundAmount"].Precision = 18;
            command.Parameters["@ClientPlatformFeeRefundAmount"].Scale = 2;
            AddParameter(command, "@GstRefundAmount", SqlDbType.Decimal, payment.GstRefundAmount);
            command.Parameters["@GstRefundAmount"].Precision = 18;
            command.Parameters["@GstRefundAmount"].Scale = 2;
        }

        private static void AddGstParameters(SqlCommand command, Payment payment)
        {
            AddParameter(command, "@GstRate", SqlDbType.Decimal, payment.GstRate);
            command.Parameters["@GstRate"].Precision = 5;
            command.Parameters["@GstRate"].Scale = 2;
            AddParameter(command, "@GstAmount", SqlDbType.Decimal, payment.GstAmount);
            command.Parameters["@GstAmount"].Precision = 18;
            command.Parameters["@GstAmount"].Scale = 2;
            AddParameter(command, "@ClientTotalAmount", SqlDbType.Decimal, payment.ClientTotalAmount);
            command.Parameters["@ClientTotalAmount"].Precision = 18;
            command.Parameters["@ClientTotalAmount"].Scale = 2;
            AddParameter(command, "@ClientPlatformFeeRate", SqlDbType.Decimal,
                payment.ClientPlatformFeeRate);
            command.Parameters["@ClientPlatformFeeRate"].Precision = 5;
            command.Parameters["@ClientPlatformFeeRate"].Scale = 2;
            AddParameter(command, "@ClientPlatformFeeAmount", SqlDbType.Decimal,
                payment.ClientPlatformFeeAmount);
            command.Parameters["@ClientPlatformFeeAmount"].Precision = 18;
            command.Parameters["@ClientPlatformFeeAmount"].Scale = 2;
            AddParameter(command, "@FreelancerNetAmount", SqlDbType.Decimal, payment.FreelancerNetAmount);
            command.Parameters["@FreelancerNetAmount"].Precision = 18;
            command.Parameters["@FreelancerNetAmount"].Scale = 2;
            AddParameter(command, "@FreelancerPlatformFeeRate", SqlDbType.Decimal,
                payment.FreelancerPlatformFeeRate);
            command.Parameters["@FreelancerPlatformFeeRate"].Precision = 5;
            command.Parameters["@FreelancerPlatformFeeRate"].Scale = 2;
            AddParameter(command, "@FreelancerPlatformFeeAmount", SqlDbType.Decimal,
                payment.FreelancerPlatformFeeAmount);
            command.Parameters["@FreelancerPlatformFeeAmount"].Precision = 18;
            command.Parameters["@FreelancerPlatformFeeAmount"].Scale = 2;
        }

        private static void EnsureUnitOfWork(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null || unitOfWork.Connection == null || unitOfWork.Transaction == null)
                throw new ArgumentNullException("unitOfWork");
        }
    }
}
