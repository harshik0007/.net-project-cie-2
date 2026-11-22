using System;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Services
{
    public sealed class SimulatedPaymentProcessor : IPaymentProcessor
    {
        public PaymentResult FundPayment(Payment payment)
        {
            if (payment == null)
                return PaymentResult.Failure("Payment details are required.");

            try
            {
                payment.MarkFunded(DateTime.Now);
                return PaymentResult.Success("Simulated funds are held for this contract.");
            }
            catch (InvalidOperationException ex)
            {
                return PaymentResult.Failure(ex.Message);
            }
        }

        public PaymentResult ReleasePayment(Payment payment)
        {
            if (payment == null)
                return PaymentResult.Failure("Payment details are required.");

            try
            {
                payment.Release(DateTime.Now);
                return PaymentResult.Success("Simulated funds have been released.");
            }
            catch (InvalidOperationException ex)
            {
                return PaymentResult.Failure(ex.Message);
            }
        }

        public PaymentResult RefundPayment(Payment payment)
        {
            if (payment == null)
                return PaymentResult.Failure("Payment details are required.");

            try
            {
                payment.Refund();
                return PaymentResult.Success("The simulated payment has been refunded.");
            }
            catch (InvalidOperationException ex)
            {
                return PaymentResult.Failure(ex.Message);
            }
        }
    }
}
