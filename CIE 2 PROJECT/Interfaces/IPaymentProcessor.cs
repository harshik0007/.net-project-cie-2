using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IPaymentProcessor
    {
        PaymentResult FundPayment(Payment payment);
        PaymentResult ReleasePayment(Payment payment);
        PaymentResult RefundPayment(Payment payment);
    }
}
