using System;

namespace CIE_2_PROJECT.Helpers
{
    public static class UrlValidationHelper
    {
        public static bool IsValidHttpUrl(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return true;

            Uri uri;
            return Uri.TryCreate(value.Trim(), UriKind.Absolute, out uri) &&
                   (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) &&
                   string.IsNullOrEmpty(uri.UserInfo);
        }
    }
}
