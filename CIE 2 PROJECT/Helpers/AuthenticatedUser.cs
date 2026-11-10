using System;
using System.Globalization;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace CIE_2_PROJECT.Helpers
{
    public static class AuthenticatedUser
    {
        public static int UserID
        {
            get
            {
                int userId;
                if (HttpContext.Current == null || HttpContext.Current.User == null ||
                    HttpContext.Current.User.Identity == null || !HttpContext.Current.User.Identity.IsAuthenticated ||
                    !int.TryParse(HttpContext.Current.User.Identity.Name, NumberStyles.None,
                        CultureInfo.InvariantCulture, out userId) || userId <= 0)
                    throw new HttpException(401, "Please log in to continue.");
                return userId;
            }
        }

        public static string Role
        {
            get
            {
                IPrincipal principal = HttpContext.Current == null ? null : HttpContext.Current.User;
                if (principal == null || principal.IsInRole("Client") == principal.IsInRole("Freelancer"))
                    return null;
                return principal.IsInRole("Client") ? "Client" : "Freelancer";
            }
        }

        public static void SignIn(int userId, string role)
        {
            if (userId <= 0 || (role != "Client" && role != "Freelancer"))
                throw new ArgumentException("A valid user account is required.");
            DateTime now = DateTime.Now;
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                userId.ToString(CultureInfo.InvariantCulture), now, now.AddHours(8), false, role,
                FormsAuthentication.FormsCookiePath);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                FormsAuthentication.Encrypt(ticket))
            {
                HttpOnly = true,
                Secure = HttpContext.Current.Request.IsSecureConnection,
                Path = FormsAuthentication.FormsCookiePath
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void SignOut()
        {
            FormsAuthentication.SignOut();
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
            }
        }
    }
}
