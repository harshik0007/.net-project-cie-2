using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;
using CIE_2_PROJECT.Validators;

namespace CIE_2_PROJECT
{
    public partial class SubmitWork : Page
    {
        [Serializable]
        private sealed class StagedWorkFile
        {
            public string OriginalFileName { get; set; }
            public string StoredFileName { get; set; }
            public string ContentType { get; set; }
            public long FileSize { get; set; }
            public string SizeDisplay { get; set; }
        }

        private const int MaximumFilesPerSubmission = 10;

        private int ContractId
        {
            get
            {
                int id;
                string value = Request.QueryString["contractId"];
                if (string.IsNullOrWhiteSpace(value))
                    value = Request.QueryString["id"];

                return int.TryParse(value, out id) && id > 0 ? id : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Form != null)
                Page.Form.Enctype = "multipart/form-data";

            if (!IsPostBack)
            {
                if (ContractId <= 0)
                {
                    ShowMessage("Invalid contract ID.");
                    btnSubmitWork.Enabled = false;
                    btnAddFiles.Enabled = false;
                    return;
                }
                LoadContract();
            }

            BindStagedFiles();
        }

        private void LoadContract()
        {
            try
            {
                Contract contract = ServiceFactory.CreateContractService()
                    .GetForFreelancer(TestUserContext.FreelancerID, ContractId);
                Project project = ServiceFactory.CreateProjectService().GetProject(contract.ProjectID);
                if (project == null)
                    throw new InvalidOperationException("The contract project was not found.");

                if (contract.ApprovalStatus != WorkflowStatuses.ContractNotSubmitted &&
                    contract.ApprovalStatus != WorkflowStatuses.ContractRevisionRequested)
                {
                    ShowMessage("This contract is not accepting another work submission.");
                    btnSubmitWork.Enabled = false;
                    btnAddFiles.Enabled = false;
                }

                User client = ServiceFactory.CreateUserService().GetUser(contract.ClientID);
                litProjectTitle.Text = Server.HtmlEncode(project.Title);
                lnkClientProfile.Text = Server.HtmlEncode(client == null
                    ? "Client #" + contract.ClientID : client.FullName);
                lnkClientProfile.NavigateUrl = "ViewClientProfile.aspx?id=" +
                    contract.ClientID.ToString(CultureInfo.InvariantCulture) + "&viewer=freelancer";
                lnkMessageClient.NavigateUrl = ResolveUrl("~/Messages/Messages.aspx?with=" +
                    contract.ClientID.ToString(CultureInfo.InvariantCulture) + "&projectId=" +
                    contract.ProjectID.ToString(CultureInfo.InvariantCulture));
                litAgreedPrice.Text = "&#8377;" + contract.AgreedAmount.ToString("N2");
                litDeadline.Text = project.Deadline.HasValue
                    ? Server.HtmlEncode(project.Deadline.Value.ToString("dd MMM yyyy", CultureInfo.InvariantCulture))
                    : "Not specified";
                litStartedOn.Text = contract.CreatedAt.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                litProjectStatus.Text = Server.HtmlEncode(project.Status);

                ISubmissionService submissionService = ServiceFactory.CreateSubmissionService();
                WorkSubmission latest = submissionService.GetLatestForFreelancer(
                    TestUserContext.FreelancerID, ContractId);
                if (latest != null && latest.SubmissionStatus == WorkflowStatuses.SubmissionRevisionRequested)
                {
                    pnlRevisionFeedback.Visible = true;
                    lblRevisionFeedback.Text = Server.HtmlEncode(latest.ClientFeedback);
                }

                Payment payment = null;
                foreach (Payment item in ServiceFactory.CreatePaymentService()
                    .GetFreelancerPaymentHistory(TestUserContext.FreelancerID))
                {
                    if (item.ContractID == ContractId)
                    {
                        payment = item;
                        break;
                    }
                }
                if (payment == null || payment.PaymentStatus != WorkflowStatuses.PaymentHeld)
                {
                    litFreelancerFeeRate.Text = SimulatedPlatformFeeCalculator.DemoRate
                        .ToString("0.##", CultureInfo.InvariantCulture);
                    litFreelancerFee.Text = "Not funded";
                    litFreelancerNet.Text = "Not funded";
                    ShowMessage("The client must fund this contract before work can be submitted.");
                    btnSubmitWork.Enabled = false;
                    btnAddFiles.Enabled = false;
                }
                else
                {
                    CultureInfo currencyCulture = CultureInfo.GetCultureInfo("en-IN");
                    litAgreedPrice.Text = "&#8377;" + payment.Amount.ToString("N2", currencyCulture);
                    litFreelancerFeeRate.Text = payment.FreelancerPlatformFeeRate
                        .ToString("0.##", CultureInfo.InvariantCulture);
                    litFreelancerFee.Text = "&#8377;" + payment.FreelancerPlatformFeeAmount.ToString("N2", currencyCulture);
                    litFreelancerNet.Text = "&#8377;" + payment.FreelancerNetAmount.ToString("N2", currencyCulture);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Freelancer contract load failed: " + ex);
                ShowMessage("Contract not found or access is denied.");
                btnSubmitWork.Enabled = false;
                btnAddFiles.Enabled = false;
            }
        }

        protected void btnAddFiles_Click(object sender, EventArgs e)
        {
            List<string> filesCreated = new List<string>();
            try
            {
                EnsureContractCanAcceptWork();

                List<HttpPostedFile> selectedFiles = new List<HttpPostedFile>();
                foreach (HttpPostedFile file in fuCompletedWork.PostedFiles)
                {
                    if (file != null && file.ContentLength > 0)
                        selectedFiles.Add(file);
                }

                if (selectedFiles.Count == 0)
                    throw new InvalidOperationException("Choose one or more files before clicking Add Selected Files.");

                List<StagedWorkFile> stagedFiles = new List<StagedWorkFile>(GetStagedFiles());
                if (stagedFiles.Count + selectedFiles.Count > MaximumFilesPerSubmission)
                    throw new InvalidOperationException("A submission can include up to 10 files.");

                FileValidator validator = new FileValidator();
                foreach (HttpPostedFile file in selectedFiles)
                {
                    string error;
                    if (!validator.IsValid(file.FileName, file.ContentLength, out error))
                        throw new InvalidOperationException(error);
                }

                string directory = GetStagingDirectory();
                Directory.CreateDirectory(directory);
                foreach (HttpPostedFile file in selectedFiles)
                    stagedFiles.Add(SaveStagedFile(file, directory, filesCreated));

                Session[StagedFilesSessionKey] = stagedFiles;
                lblSubmitMessage.Visible = false;
                BindStagedFiles();
            }
            catch (Exception ex)
            {
                foreach (string path in filesCreated)
                {
                    try
                    {
                        if (File.Exists(path))
                            File.Delete(path);
                    }
                    catch (IOException) { }
                    catch (UnauthorizedAccessException) { }
                }

                System.Diagnostics.Trace.TraceError("Staging work files failed: " + ex);
                ShowMessage(ex is InvalidOperationException
                    ? ex.Message
                    : "The selected files could not be added. Please try again.");
                BindStagedFiles();
            }
        }

        protected void rptUploadedFiles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!string.Equals(e.CommandName, "RemoveFile", StringComparison.OrdinalIgnoreCase))
                return;

            List<StagedWorkFile> stagedFiles = GetStagedFiles();
            string storedName = Convert.ToString(e.CommandArgument);
            StagedWorkFile selected = stagedFiles.FirstOrDefault(file =>
                string.Equals(file.StoredFileName, storedName, StringComparison.Ordinal));
            if (selected == null)
                return;

            try
            {
                string path = Path.Combine(GetStagingDirectory(), selected.StoredFileName);
                if (File.Exists(path))
                    File.Delete(path);
                stagedFiles.Remove(selected);
                Session[StagedFilesSessionKey] = stagedFiles;
                lblSubmitMessage.Visible = false;
            }
            catch (IOException ex)
            {
                System.Diagnostics.Trace.TraceError("Removing staged work file failed: " + ex);
                ShowMessage("That file could not be removed. Please try again.");
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Trace.TraceError("Removing staged work file failed: " + ex);
                ShowMessage("That file could not be removed. Please try again.");
            }

            BindStagedFiles();
        }

        protected void cvCompletionMessage_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string message = args.Value.Trim();
            args.IsValid = message.Length >= 20 && message.Length <= 2000;
        }

        protected void cvCompletedWork_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (fuCompletedWork.HasFiles)
            {
                cvCompletedWork.ErrorMessage = "Click Add Selected Files so the files appear in the list before submitting.";
                args.IsValid = false;
                return;
            }

            List<StagedWorkFile> stagedFiles = GetStagedFiles();
            if (stagedFiles.Count == 0)
            {
                cvCompletedWork.ErrorMessage = "Please add at least one completed work file.";
                args.IsValid = false;
                return;
            }

            foreach (StagedWorkFile stagedFile in stagedFiles)
            {
                if (!File.Exists(Path.Combine(GetStagingDirectory(), stagedFile.StoredFileName)))
                {
                    cvCompletedWork.ErrorMessage = "A selected file is no longer available. Add it again.";
                    args.IsValid = false;
                    return;
                }
            }

            args.IsValid = true;
        }

        protected void cvDemoLink_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = UrlValidationHelper.IsValidHttpUrl(args.Value.Trim());
        }

        protected void cvGithubLink_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string url = args.Value.Trim();
            if (string.IsNullOrWhiteSpace(url))
            {
                args.IsValid = true;
                return;
            }

            Uri uri;
            bool valid = Uri.TryCreate(url, UriKind.Absolute, out uri) &&
                UrlValidationHelper.IsValidHttpUrl(url);
            args.IsValid = valid &&
                (uri.Host.Equals("github.com", StringComparison.OrdinalIgnoreCase) ||
                 uri.Host.EndsWith(".github.com", StringComparison.OrdinalIgnoreCase));
        }

        protected void btnSubmitWork_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
                return;
            if (ContractId <= 0)
            {
                ShowMessage("Invalid contract.");
                return;
            }

            try
            {
                EnsureContractCanAcceptWork();
                List<StagedWorkFile> stagedFiles = GetStagedFiles();
                List<FileStream> openStreams = new List<FileStream>();
                List<UploadedFileInput> files = new List<UploadedFileInput>();
                try
                {
                    foreach (StagedWorkFile stagedFile in stagedFiles)
                    {
                        FileStream stream = new FileStream(
                            Path.Combine(GetStagingDirectory(), stagedFile.StoredFileName),
                            FileMode.Open, FileAccess.Read, FileShare.Read);
                        openStreams.Add(stream);
                        files.Add(new UploadedFileInput(
                            stagedFile.OriginalFileName,
                            stagedFile.ContentType,
                            stagedFile.FileSize,
                            stream));
                    }

                    ServiceFactory.CreateSubmissionService().SubmitWork(
                        TestUserContext.FreelancerID,
                        ContractId,
                        txtCompletionMessage.Text.Trim(),
                        txtDemoLink.Text.Trim(),
                        txtGithubLink.Text.Trim(),
                        files);
                }
                finally
                {
                    foreach (FileStream stream in openStreams)
                        stream.Dispose();
                }

                ClearStagedFiles(stagedFiles);
                FlashMessageStore.Success("Your work was submitted successfully. The client can now review the files and request changes or approve it.");
                Response.Redirect("ActiveWork.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Work submission failed: " + ex);
                ShowMessage("Submission failed. Please review the form and try again.");
            }
        }

        private void EnsureContractCanAcceptWork()
        {
            if (ContractId <= 0)
                throw new InvalidOperationException("A valid contract is required.");

            Contract contract = ServiceFactory.CreateContractService()
                .GetForFreelancer(TestUserContext.FreelancerID, ContractId);
            if (contract.ApprovalStatus != WorkflowStatuses.ContractNotSubmitted &&
                contract.ApprovalStatus != WorkflowStatuses.ContractRevisionRequested)
                throw new InvalidOperationException("This contract is not accepting another work submission.");

            Payment payment = ServiceFactory.CreatePaymentService()
                .GetFreelancerPaymentHistory(TestUserContext.FreelancerID)
                .FirstOrDefault(item => item.ContractID == ContractId);
            if (payment == null || payment.PaymentStatus != WorkflowStatuses.PaymentHeld)
                throw new InvalidOperationException("The client must fund this contract before work can be submitted.");
        }

        private StagedWorkFile SaveStagedFile(
            HttpPostedFile upload,
            string directory,
            IList<string> createdPaths)
        {
            string originalName = Path.GetFileName(upload.FileName);
            string extension = Path.GetExtension(originalName).ToLowerInvariant();
            string storedName = Guid.NewGuid().ToString("N") + extension;
            string path = Path.Combine(directory, storedName);
            createdPaths.Add(path);

            long copied = 0;
            byte[] buffer = new byte[81920];
            using (FileStream output = new FileStream(
                path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                int read;
                while ((read = upload.InputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    copied += read;
                    if (copied > FileValidator.MaximumFileSize)
                        throw new InvalidOperationException("Each file must be no larger than 10 MB.");
                    output.Write(buffer, 0, read);
                }
            }

            if (copied != upload.ContentLength)
                throw new InvalidOperationException("A selected file could not be read completely.");

            return new StagedWorkFile
            {
                OriginalFileName = originalName,
                StoredFileName = storedName,
                ContentType = upload.ContentType,
                FileSize = copied,
                SizeDisplay = FormatFileSize(copied)
            };
        }

        private List<StagedWorkFile> GetStagedFiles()
        {
            List<StagedWorkFile> files = Session[StagedFilesSessionKey] as List<StagedWorkFile>;
            return files ?? new List<StagedWorkFile>();
        }

        private string StagedFilesSessionKey
        {
            get { return "SubmitWork.Files." + ContractId.ToString(CultureInfo.InvariantCulture); }
        }

        private string GetStagingDirectory()
        {
            string tokenKey = "SubmitWork.StageToken." + ContractId.ToString(CultureInfo.InvariantCulture);
            string token = Session[tokenKey] as string;
            if (string.IsNullOrWhiteSpace(token))
            {
                token = Guid.NewGuid().ToString("N");
                Session[tokenKey] = token;
            }

            return Server.MapPath("~/App_Data/WorkSubmissionStaging/" +
                ContractId.ToString(CultureInfo.InvariantCulture) + "/" + token);
        }

        private void BindStagedFiles()
        {
            List<StagedWorkFile> files = GetStagedFiles();
            rptUploadedFiles.DataSource = files;
            rptUploadedFiles.DataBind();
            pnlUploadedFiles.Visible = files.Count > 0;
            lblUploadedFileCount.Text = files.Count == 1
                ? "1 file ready to submit"
                : files.Count.ToString(CultureInfo.InvariantCulture) + " files ready to submit";
        }

        private void ClearStagedFiles(IList<StagedWorkFile> files)
        {
            foreach (StagedWorkFile file in files)
            {
                try
                {
                    string path = Path.Combine(GetStagingDirectory(), file.StoredFileName);
                    if (File.Exists(path))
                        File.Delete(path);
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Trace.TraceWarning("Staged work file cleanup failed: " + ex);
                }
                catch (UnauthorizedAccessException ex)
                {
                    System.Diagnostics.Trace.TraceWarning("Staged work file cleanup failed: " + ex);
                }
            }

            Session.Remove(StagedFilesSessionKey);
            Session.Remove("SubmitWork.StageToken." + ContractId.ToString(CultureInfo.InvariantCulture));
        }

        private static string FormatFileSize(long bytes)
        {
            if (bytes >= 1024L * 1024L)
                return (bytes / (1024d * 1024d)).ToString("0.##", CultureInfo.InvariantCulture) + " MB";
            if (bytes >= 1024L)
                return (bytes / 1024d).ToString("0.##", CultureInfo.InvariantCulture) + " KB";
            return bytes.ToString(CultureInfo.InvariantCulture) + " B";
        }

        private void ShowMessage(string message)
        {
            lblSubmitMessage.Text = Server.HtmlEncode(message);
            lblSubmitMessage.Visible = true;
        }
    }
}
