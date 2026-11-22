using System;
using CIE_2_PROJECT.Helpers;

namespace CIE_2_PROJECT.Models
{
    public sealed class Payment
    {
        public int PaymentID { get; private set; }
        public int ContractID { get; private set; }
        public int ClientID { get; private set; }
        public int FreelancerID { get; private set; }
        public decimal Amount { get; private set; }
        public string PaymentStatus { get; private set; }
        public string PaymentMethod { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? FundedAt { get; private set; }
        public DateTime? ReleasedAt { get; private set; }
        public decimal ClientRefundAmount { get; private set; }
        public decimal FreelancerPayoutAmount { get; private set; }
        public decimal PlatformFeeAmount { get; private set; }
        public decimal GstRate { get; private set; }
        public decimal GstAmount { get; private set; }
        public decimal ClientTotalAmount { get; private set; }
        public decimal ClientPlatformFeeRate { get; private set; }
        public decimal ClientPlatformFeeAmount { get; private set; }
        public decimal FreelancerNetAmount { get; private set; }
        public decimal FreelancerPlatformFeeRate { get; private set; }
        public decimal FreelancerPlatformFeeAmount { get; private set; }
        public decimal GstRefundAmount { get; private set; }
        public decimal ClientPlatformFeeRefundAmount { get; private set; }

        public Payment(
            int paymentId,
            int contractId,
            int clientId,
            int freelancerId,
            decimal amount,
            string paymentStatus,
            string paymentMethod,
            DateTime createdAt,
            DateTime? fundedAt,
            DateTime? releasedAt)
            : this(paymentId, contractId, clientId, freelancerId, amount, paymentStatus,
                paymentMethod, createdAt, fundedAt, releasedAt, 0m, 0m, 0m,
                0m, 0m, amount, 0m, 0m, amount, 0m, 0m, 0m, 0m)
        {
        }

        public Payment(
            int paymentId,
            int contractId,
            int clientId,
            int freelancerId,
            decimal amount,
            string paymentStatus,
            string paymentMethod,
            DateTime createdAt,
            DateTime? fundedAt,
            DateTime? releasedAt,
            decimal clientRefundAmount,
            decimal freelancerPayoutAmount,
            decimal platformFeeAmount)
            : this(paymentId, contractId, clientId, freelancerId, amount, paymentStatus,
                paymentMethod, createdAt, fundedAt, releasedAt, clientRefundAmount,
                freelancerPayoutAmount, platformFeeAmount, 0m, 0m, amount, 0m, 0m,
                amount, 0m, 0m, 0m, 0m)
        {
        }

        public Payment(
            int paymentId,
            int contractId,
            int clientId,
            int freelancerId,
            decimal amount,
            string paymentStatus,
            string paymentMethod,
            DateTime createdAt,
            DateTime? fundedAt,
            DateTime? releasedAt,
            decimal clientRefundAmount,
            decimal freelancerPayoutAmount,
            decimal platformFeeAmount,
            decimal gstRate,
            decimal gstAmount,
            decimal clientTotalAmount,
            decimal clientPlatformFeeRate,
            decimal clientPlatformFeeAmount,
            decimal freelancerNetAmount,
            decimal freelancerPlatformFeeRate,
            decimal freelancerPlatformFeeAmount,
            decimal gstRefundAmount,
            decimal clientPlatformFeeRefundAmount)
        {
            if (paymentId < 0)
                throw new ArgumentOutOfRangeException("paymentId");
            if (contractId <= 0 || clientId <= 0 || freelancerId <= 0)
                throw new ArgumentException("Payment participant and contract IDs must be positive.");
            if (amount <= 0)
                throw new ArgumentOutOfRangeException("amount");
            if (!IsPaymentStatus(paymentStatus))
                throw new ArgumentException("The payment status is invalid.", "paymentStatus");
            if (clientRefundAmount < 0 || freelancerPayoutAmount < 0 || platformFeeAmount < 0 ||
                clientRefundAmount + freelancerPayoutAmount + platformFeeAmount > amount)
                throw new ArgumentException("Payment allocations must be non-negative and cannot exceed the escrow amount.");
            if (gstRate < 0m || gstRate > 100m || gstAmount < 0m || gstRefundAmount < 0m ||
                gstRefundAmount > gstAmount ||
                clientPlatformFeeRate < 0m || clientPlatformFeeRate > 100m ||
                clientPlatformFeeAmount < 0m || clientPlatformFeeAmount > amount ||
                clientPlatformFeeRefundAmount < 0m ||
                clientPlatformFeeRefundAmount > clientPlatformFeeAmount ||
                (clientPlatformFeeRefundAmount > 0m &&
                    paymentStatus != WorkflowStatuses.PaymentRefunded) ||
                clientTotalAmount != amount + gstAmount + clientPlatformFeeAmount ||
                clientPlatformFeeAmount != SimulatedPlatformFeeCalculator.CalculateFee(
                    amount, clientPlatformFeeRate) ||
                freelancerPlatformFeeRate < 0m || freelancerPlatformFeeRate > 100m ||
                freelancerPlatformFeeAmount < 0m ||
                freelancerPlatformFeeAmount > amount ||
                freelancerNetAmount + freelancerPlatformFeeAmount != amount ||
                freelancerPlatformFeeAmount != SimulatedPlatformFeeCalculator.CalculateFee(
                    amount, freelancerPlatformFeeRate))
                throw new ArgumentException("The simulated payment breakdown is inconsistent with the contract amount.");

            PaymentID = paymentId;
            ContractID = contractId;
            ClientID = clientId;
            FreelancerID = freelancerId;
            Amount = amount;
            PaymentStatus = paymentStatus;
            PaymentMethod = paymentMethod;
            CreatedAt = createdAt;
            FundedAt = fundedAt;
            ReleasedAt = releasedAt;
            ClientRefundAmount = clientRefundAmount;
            FreelancerPayoutAmount = freelancerPayoutAmount;
            PlatformFeeAmount = platformFeeAmount;
            GstRate = gstRate;
            GstAmount = gstAmount;
            ClientTotalAmount = clientTotalAmount;
            ClientPlatformFeeRate = clientPlatformFeeRate;
            ClientPlatformFeeAmount = clientPlatformFeeAmount;
            FreelancerNetAmount = freelancerNetAmount;
            FreelancerPlatformFeeRate = freelancerPlatformFeeRate;
            FreelancerPlatformFeeAmount = freelancerPlatformFeeAmount;
            GstRefundAmount = gstRefundAmount;
            ClientPlatformFeeRefundAmount = clientPlatformFeeRefundAmount;
        }

        public static Payment CreateWithDemoGst(
            int paymentId,
            int contractId,
            int clientId,
            int freelancerId,
            decimal amount,
            string paymentStatus,
            string paymentMethod,
            DateTime createdAt,
            DateTime? fundedAt,
            DateTime? releasedAt)
        {
            decimal gst = SimulatedGstCalculator.CalculateTax(amount);
            decimal platformFee = SimulatedPlatformFeeCalculator.CalculateFee(amount);
            return new Payment(
                paymentId, contractId, clientId, freelancerId, amount, paymentStatus,
                paymentMethod, createdAt, fundedAt, releasedAt, 0m, 0m, 0m,
                SimulatedGstCalculator.DemoRate, gst, amount + gst + platformFee,
                SimulatedPlatformFeeCalculator.DemoRate, platformFee, amount - platformFee,
                SimulatedPlatformFeeCalculator.DemoRate, platformFee, 0m, 0m);
        }

        public void MarkFunded(DateTime fundedAt)
        {
            EnsureStatus(WorkflowStatuses.PaymentNotFunded);
            PaymentStatus = WorkflowStatuses.PaymentHeld;
            FundedAt = fundedAt;
        }

        public void Release(DateTime releasedAt)
        {
            EnsureStatus(WorkflowStatuses.PaymentHeld);
            PaymentStatus = WorkflowStatuses.PaymentReleased;
            ReleasedAt = releasedAt;
        }

        public void Refund()
        {
            EnsureStatus(WorkflowStatuses.PaymentHeld);
            ClientRefundAmount = decimal.Round(Amount * 0.75m, 2, MidpointRounding.AwayFromZero);
            FreelancerPayoutAmount = decimal.Round(Amount * 0.10m, 2, MidpointRounding.AwayFromZero);
            PlatformFeeAmount = Amount - ClientRefundAmount - FreelancerPayoutAmount;
            GstRefundAmount = GstAmount;
            ClientPlatformFeeRefundAmount = ClientPlatformFeeAmount;
            PaymentStatus = WorkflowStatuses.PaymentRefunded;
        }

        private void EnsureStatus(string expectedStatus)
        {
            if (PaymentStatus != expectedStatus)
                throw new InvalidOperationException(
                    "This payment operation is not allowed while status is " + PaymentStatus + ".");
        }

        private static bool IsPaymentStatus(string status)
        {
            return status == WorkflowStatuses.PaymentNotFunded ||
                   status == WorkflowStatuses.PaymentHeld ||
                   status == WorkflowStatuses.PaymentReleased ||
                   status == WorkflowStatuses.PaymentRefunded;
        }
    }
}
