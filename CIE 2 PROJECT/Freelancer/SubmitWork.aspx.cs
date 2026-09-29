using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class SubmitWork : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void cvCompletionMessage_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string message = args.Value.Trim();

            args.IsValid = message.Length >= 20 && message.Length <= 2000;
        }

        protected void cvCompletedWork_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!fuCompletedWork.HasFiles)
            {
                args.IsValid = false;
                return;
            }

            string[] allowedExtensions =
            {
                ".zip",
                ".pdf",
                ".doc",
                ".docx",
                ".png",
                ".jpg",
                ".jpeg"
            };

            bool allFilesValid = true;

            foreach (HttpPostedFile file in fuCompletedWork.PostedFiles)
            {
                string extension = Path.GetExtension(file.FileName).ToLower();

                bool validExtension = false;

                foreach (string allowedExtension in allowedExtensions)
                {
                    if (extension == allowedExtension)
                    {
                        validExtension = true;
                        break;
                    }
                }

                bool validSize = file.ContentLength <= 10 * 1024 * 1024;

                if (!validExtension || !validSize)
                {
                    allFilesValid = false;
                    break;
                }
            }

            args.IsValid = allFilesValid;
        }

        protected void cvDemoLink_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string url = args.Value.Trim();

            if (string.IsNullOrWhiteSpace(url))
            {
                args.IsValid = true;
                return;
            }

            args.IsValid = Regex.IsMatch(
                url,
                @"^https?:\/\/.+$",
                RegexOptions.IgnoreCase
            );
        }

        protected void cvGithubLink_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string url = args.Value.Trim();

            if (string.IsNullOrWhiteSpace(url))
            {
                args.IsValid = true;
                return;
            }

            args.IsValid = Regex.IsMatch(
                url,
                @"^https?:\/\/(www\.)?github\.com\/.+$",
                RegexOptions.IgnoreCase
            );
        }

        protected void btnSubmitWork_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid)
            {
                return;
            }

            string completionMessage = txtCompletionMessage.Text.Trim();
            string demoLink = txtDemoLink.Text.Trim();
            string githubLink = txtGithubLink.Text.Trim();

            // Database submission logic will be added later.
        }
    }
}