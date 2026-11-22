namespace CIE_2_PROJECT.Models
{
    public sealed class PaymentResult
    {
        public bool Succeeded { get; private set; }
        public string Message { get; private set; }

        private PaymentResult(bool succeeded, string message)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public static PaymentResult Success(string message)
        {
            return new PaymentResult(true, message);
        }

        public static PaymentResult Failure(string message)
        {
            return new PaymentResult(false, message);
        }
    }
}
