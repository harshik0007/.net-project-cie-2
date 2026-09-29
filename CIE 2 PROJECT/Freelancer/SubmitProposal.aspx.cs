using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class SubmitProposal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void cvCoverLetter_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            string coverLetter = args.Value.Trim();

            args.IsValid =
                coverLetter.Length >= 50 &&
                coverLetter.Length <= 1000;
        }

        protected void cvProposedPrice_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            decimal price;

            if (!decimal.TryParse(args.Value, out price))
            {
                args.IsValid = false;
                return;
            }

            args.IsValid =
                price >= 8000 &&
                price <= 15000;
        }

        protected void cvAttachment_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            if (!fuAttachment.HasFile)
            {
                args.IsValid = true;
                return;
            }

            string extension =
                Path.GetExtension(fuAttachment.FileName).ToLower();

            string[] allowedExtensions =
            {
                ".pdf",
                ".doc",
                ".docx",
                ".png",
                ".jpg",
                ".jpeg"
            };

            bool validExtension = false;

            foreach (string allowedExtension in allowedExtensions)
            {
                if (extension == allowedExtension)
                {
                    validExtension = true;
                    break;
                }
            }

            bool validSize =
                fuAttachment.PostedFile.ContentLength <=
                5 * 1024 * 1024;

            args.IsValid =
                validExtension &&
                validSize;
        }

        protected void btnSubmitProposal_Click(
            object sender,
            EventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid)
            {
                return;
            }

            string coverLetter =
                txtCoverLetter.Text.Trim();

            decimal proposedPrice =
                decimal.Parse(txtProposedPrice.Text);

            string deliveryTime =
                ddlDeliveryTime.SelectedValue;

            string attachmentName = "";

            if (fuAttachment.HasFile)
            {
                attachmentName =
                    Path.GetFileName(
                        fuAttachment.FileName
                    );
            }
        }
    }
}