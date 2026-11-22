using System;
using System.IO;
using System.Web;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;

namespace CIE_2_PROJECT.Messages
{
    public sealed class DownloadAttachmentHandler : IHttpHandler
    {
        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            if (context == null)
                return;

            try
            {
                int messageId;
                if (!int.TryParse(context.Request.QueryString["id"], out messageId) || messageId <= 0)
                {
                    NotFound(context);
                    return;
                }

                int userId = AuthenticatedUser.UserID;
                IMessagingService service = ServiceFactory.CreateMessagingService();
                string relativePath = service.GetAttachmentPathForUser(messageId, userId);
                if (!MessageAttachmentStorage.IsSafeRelativePath(relativePath))
                {
                    NotFound(context);
                    return;
                }

                string fileName = relativePath.Substring("MessageAttachments/".Length).Replace('/', '\\');
                string root = Path.GetFullPath(context.Server.MapPath("~/App_Data/MessageAttachments/"));
                string physicalPath = Path.GetFullPath(Path.Combine(root, fileName));
                if (!physicalPath.StartsWith(root, StringComparison.OrdinalIgnoreCase) || !File.Exists(physicalPath))
                {
                    NotFound(context);
                    return;
                }

                string downloadName = MessageAttachmentStorage.GetOriginalFileName(relativePath);
                context.Response.Clear();
                context.Response.ContentType = "application/octet-stream";
                context.Response.AddHeader("X-Content-Type-Options", "nosniff");
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + downloadName + "\"");
                context.Response.AddHeader("Content-Length", new FileInfo(physicalPath).Length.ToString());
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.TransmitFile(physicalPath);
                context.ApplicationInstance.CompleteRequest();
            }
            catch (HttpException ex)
            {
                if (ex.GetHttpCode() == 401)
                {
                    context.Response.StatusCode = 401;
                    context.Response.TrySkipIisCustomErrors = true;
                    context.ApplicationInstance.CompleteRequest();
                    return;
                }
                System.Diagnostics.Trace.TraceWarning("Message attachment download was denied: " + ex.Message);
                NotFound(context);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Message attachment download failed: " + ex);
                NotFound(context);
            }
        }

        private static void NotFound(HttpContext context)
        {
            context.Response.Clear();
            context.Response.StatusCode = 404;
            context.Response.TrySkipIisCustomErrors = true;
            context.Response.ContentType = "text/plain";
            context.Response.Write("Attachment not found.");
            context.ApplicationInstance.CompleteRequest();
        }
    }
}
