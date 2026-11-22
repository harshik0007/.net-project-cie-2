using System;
using System.Web;

namespace CIE_2_PROJECT.Helpers
{
    public sealed class FlashMessageNotice
    {
        public string Message { get; private set; }
        public bool IsError { get; private set; }

        internal FlashMessageNotice(string message, bool isError)
        {
            Message = message;
            IsError = isError;
        }
    }

    public static class FlashMessageStore
    {
        private const string MessageKey = "SkillLink.Flash.Message";
        private const string ErrorKey = "SkillLink.Flash.IsError";

        public static void Success(string message)
        {
            Store(message, false);
        }

        public static void Error(string message)
        {
            Store(message, true);
        }

        public static FlashMessageNotice Consume()
        {
            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null)
                return null;

            string message = Convert.ToString(context.Session[MessageKey]);
            bool isError = context.Session[ErrorKey] is bool && (bool)context.Session[ErrorKey];
            context.Session.Remove(MessageKey);
            context.Session.Remove(ErrorKey);

            return string.IsNullOrWhiteSpace(message)
                ? null
                : new FlashMessageNotice(message, isError);
        }

        private static void Store(string message, bool isError)
        {
            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null || string.IsNullOrWhiteSpace(message))
                return;

            context.Session[MessageKey] = message.Trim();
            context.Session[ErrorKey] = isError;
        }
    }
}
