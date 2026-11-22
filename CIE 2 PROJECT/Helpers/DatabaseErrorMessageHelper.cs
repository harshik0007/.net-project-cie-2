using System;
using System.Data.SqlClient;

namespace CIE_2_PROJECT.Helpers
{
    public static class DatabaseErrorMessageHelper
    {
        private const string AllocationMigrationMessage =
            "We couldn't load this data because the database is missing the escrow allocation fields. " +
            "Contact the project maintainer to update the SkillLinkDB schema.";

        private const string GstMigrationMessage =
            "We couldn't load this data because the database is missing payment breakdown fields. " +
            "Contact the project maintainer to update the SkillLinkDB schema.";

        private const string DatabaseUnavailableMessage =
            "We couldn't load your data from the database. Check that SkillLinkDB is available and that its migrations have been applied, then refresh the page.";

        public static string GetUserMessage(Exception exception, string fallbackMessage)
        {
            for (Exception current = exception; current != null; current = current.InnerException)
            {
                SqlException sqlException = current as SqlException;
                if (sqlException == null)
                    continue;

                foreach (SqlError error in sqlException.Errors)
                {
                    if (error.Number == 207 && IsAllocationColumnError(error.Message))
                        return AllocationMigrationMessage;
                    if (error.Number == 207 && IsGstColumnError(error.Message))
                        return GstMigrationMessage;
                }

                return DatabaseUnavailableMessage;
            }

            return fallbackMessage;
        }

        private static bool IsAllocationColumnError(string message)
        {
            return message.IndexOf("ClientRefundAmount", StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.IndexOf("FreelancerPayoutAmount", StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.IndexOf("PlatformFeeAmount", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsGstColumnError(string message)
        {
            return message.IndexOf("GstRate", StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.IndexOf("GstAmount", StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.IndexOf("ClientTotalAmount", StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.IndexOf("FreelancerNetAmount", StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.IndexOf("ClientPlatformFeeRate", StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.IndexOf("ClientPlatformFeeAmount", StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.IndexOf("ClientPlatformFeeRefundAmount", StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.IndexOf("FreelancerPlatformFeeRate", StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.IndexOf("FreelancerPlatformFeeAmount", StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.IndexOf("GstRefundAmount", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
