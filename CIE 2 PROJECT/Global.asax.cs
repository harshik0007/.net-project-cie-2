using System;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;

namespace CIE_2_PROJECT
{
    public class Global : HttpApplication
    {
        void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value)) return;
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket == null || ticket.Expired) return;
                string role = ticket.UserData;
                if (role != "Client" && role != "Freelancer") return;
                GenericIdentity identity = new GenericIdentity(ticket.Name, "Forms");
                Context.User = new GenericPrincipal(identity, new[] { role });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceWarning("Invalid authentication ticket: " + ex.Message);
                FormsAuthentication.SignOut();
            }
        }

        void Application_AuthorizeRequest(object sender, EventArgs e)
        {
            if (Context.User == null || Context.User.Identity == null || !Context.User.Identity.IsAuthenticated)
                return;
            string path = Request.AppRelativeCurrentExecutionFilePath ?? string.Empty;
            string role = Context.User.IsInRole("Client") ? "Client" :
                Context.User.IsInRole("Freelancer") ? "Freelancer" : string.Empty;
            if (string.IsNullOrEmpty(role)) return;
            bool clientPage = path.StartsWith("~/Client/", StringComparison.OrdinalIgnoreCase);
            bool freelancerPage = path.StartsWith("~/Freelancer/", StringComparison.OrdinalIgnoreCase);
            if ((clientPage && role != "Client") || (freelancerPage && role != "Freelancer"))
            {
                Response.Redirect(VirtualPathUtility.ToAbsolute(role == "Client" ? "~/Client/ClientHome.aspx" : "~/Freelancer/FreelancerHome.aspx"), true);
            }
        }

        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
