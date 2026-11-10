using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class CreateProject : Page
    {
        private int EditingProjectId
        {
            get { return ViewState["EditingProjectId"] == null ? 0 : (int)ViewState["EditingProjectId"]; }
            set { ViewState["EditingProjectId"] = value; }
        }

        private string OriginalAttachmentPath
        {
            get { return ViewState["OriginalAttachmentPath"] as string; }
            set { ViewState["OriginalAttachmentPath"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Form != null)
                Page.Form.Enctype = "multipart/form-data";
            if (!IsPostBack)
            {
                LoadProjectForEditing();
                if (EditingProjectId == 0)
                    CheckActiveProjectLimit();
            }
        }

        private void CheckActiveProjectLimit()
        {
            try
            {
                int activeProjectCount = 0;
                foreach (Project project in ServiceFactory.CreateProjectService()
                    .GetClientProjects(TestUserContext.ClientID))
                {
                    if (project.Status == WorkflowStatuses.ProjectOpen ||
                        project.Status == WorkflowStatuses.ProjectInProgress)
                        activeProjectCount++;
                }

                if (activeProjectCount >= 2)
                {
                    ShowMessage("You already have 2 active projects. Complete or cancel a project before posting another.", false);
                    btnPostProject.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Client project limit check failed: " + ex);
            }
        }

        private void LoadProjectForEditing()
        {
            int projectId;
            if (!int.TryParse(Request.QueryString["id"], out projectId) || projectId <= 0)
                return;

            try
            {
                Project project = ServiceFactory.CreateProjectService().GetClientProject(
                    TestUserContext.ClientID, projectId);
                if (project.Status != WorkflowStatuses.ProjectOpen)
                {
                    ShowMessage("Only open projects can be edited.", false);
                    btnPostProject.Enabled = false;
                    return;
                }

                EditingProjectId = project.ProjectID;
                OriginalAttachmentPath = project.AttachmentPath;
                txtProjectTitle.Text = project.Title;
                txtDescription.Text = project.Description;
                SelectIfPresent(ddlCategory, project.Category);
                SelectIfPresent(ddlExperience, project.ExperienceLevel);
                txtSkills.Text = project.RequiredSkills;
                txtLocation.Text = project.PreferredLocation;
                txtMinBudget.Text = project.MinBudget.ToString("0.##", CultureInfo.InvariantCulture);
                txtMaxBudget.Text = project.MaxBudget.ToString("0.##", CultureInfo.InvariantCulture);
                txtDeadline.Text = project.Deadline.HasValue
                    ? project.Deadline.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                    : string.Empty;
                lblPageHeading.Text = "Edit Project";
                btnPostProject.Text = "Save Changes";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Client project edit load failed: " + ex);
                ShowMessage("This project could not be loaded for editing.", false);
                btnPostProject.Enabled = false;
            }
        }

        protected void cvProjectTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string value = args.Value.Trim();
            args.IsValid = value.Length >= 5 && value.Length <= 100;
        }

        protected void cvDescription_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string value = args.Value.Trim();
            args.IsValid = value.Length >= 20 && value.Length <= 2000;
        }

        protected void cvCategory_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrWhiteSpace(ddlCategory.SelectedValue);
        }

        protected void cvExperience_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrWhiteSpace(ddlExperience.SelectedValue);
        }

        protected void cvMinBudget_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal value;
            args.IsValid = decimal.TryParse(args.Value.Trim(), NumberStyles.Number,
                CultureInfo.InvariantCulture, out value) && value > 0 && value <= 99999999.99m;
        }

        protected void cvMaxBudget_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal minBudget;
            decimal maxBudget;
            bool minValid = decimal.TryParse(txtMinBudget.Text.Trim(), NumberStyles.Number,
                CultureInfo.InvariantCulture, out minBudget);
            bool maxValid = decimal.TryParse(txtMaxBudget.Text.Trim(), NumberStyles.Number,
                CultureInfo.InvariantCulture, out maxBudget);

            args.IsValid = minValid && maxValid && minBudget > 0 &&
                maxBudget >= minBudget && maxBudget <= 99999999.99m;
        }

        protected void cvDeadline_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime deadline;
            bool validDate = DateTime.TryParseExact(args.Value.Trim(), "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out deadline);
            args.IsValid = validDate && deadline.Date > DateTime.Today;
        }

        protected void btnPostProject_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
                return;

            decimal minBudget = 0m;
            decimal maxBudget = 0m;
            DateTime deadline = DateTime.MinValue;
            bool valuesValid =
                decimal.TryParse(txtMinBudget.Text.Trim(), NumberStyles.Number,
                    CultureInfo.InvariantCulture, out minBudget) &&
                decimal.TryParse(txtMaxBudget.Text.Trim(), NumberStyles.Number,
                    CultureInfo.InvariantCulture, out maxBudget) &&
                DateTime.TryParseExact(txtDeadline.Text.Trim(), "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out deadline);

            if (!valuesValid || minBudget <= 0 || minBudget > 99999999.99m ||
                maxBudget < minBudget || maxBudget > 99999999.99m ||
                deadline.Date <= DateTime.Today)
            {
                ShowMessage("Please check your budget and deadline values.", false);
                return;
            }

            string title = txtProjectTitle.Text.Trim();
            string description = txtDescription.Text.Trim();
            string category = ddlCategory.SelectedValue;
            string experience = ddlExperience.SelectedValue;
            string skills = txtSkills.Text.Trim();
            string location = txtLocation.Text.Trim();

            if (skills.Length > 500 || location.Length > 100)
            {
                ShowMessage("Required skills must be at most 500 characters and location at most 100 characters.", false);
                return;
            }

            string attachmentPath = OriginalAttachmentPath;
            string savedPhysicalPath = null;
            string[] allowedExtensions = { ".pdf", ".doc", ".docx", ".zip", ".png", ".jpg", ".jpeg" };

            if (fuProjectFiles.HasFile)
            {
                string extension = Path.GetExtension(fuProjectFiles.FileName).ToLowerInvariant();
                if (Array.IndexOf(allowedExtensions, extension) < 0)
                {
                    ShowMessage("File type not allowed. Choose PDF, DOC, DOCX, ZIP, PNG or JPG.", false);
                    return;
                }

                int fileSize = fuProjectFiles.PostedFile.ContentLength;
                if (fileSize <= 0 || fileSize > 3 * 1024 * 1024)
                {
                    ShowMessage("The project attachment must be no larger than 3 MB.", false);
                    return;
                }
            }

            try
            {
                if (fuProjectFiles.HasFile)
                {
                    string extension = Path.GetExtension(fuProjectFiles.FileName).ToLowerInvariant();
                    string uploadFolder = Server.MapPath("~/Uploads/ProjectAttachments/");
                    Directory.CreateDirectory(uploadFolder);

                    string newFileName = Guid.NewGuid().ToString("N") + extension;
                    savedPhysicalPath = Path.Combine(uploadFolder, newFileName);
                    attachmentPath = "~/Uploads/ProjectAttachments/" + newFileName;
                    fuProjectFiles.SaveAs(savedPhysicalPath);
                }

                if (EditingProjectId > 0)
                {
                    ServiceFactory.CreateProjectService().UpdateProject(
                        TestUserContext.ClientID, EditingProjectId,
                        title, description, category, experience,
                        string.IsNullOrWhiteSpace(skills) ? null : skills,
                        string.IsNullOrWhiteSpace(location) ? null : location,
                        minBudget, maxBudget, deadline.Date, attachmentPath);
                    OriginalAttachmentPath = attachmentPath;
                    FlashMessageStore.Success("Project changes saved successfully.");
                    Response.Redirect("MyProjects.aspx?tab=All", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
                else
                {
                    Project project = new Project(
                        0,
                        TestUserContext.ClientID,
                        title,
                        description,
                        category,
                        experience,
                        string.IsNullOrWhiteSpace(skills) ? null : skills,
                        string.IsNullOrWhiteSpace(location) ? null : location,
                        minBudget,
                        maxBudget,
                        deadline.Date,
                        DateTime.Now,
                        attachmentPath,
                        WorkflowStatuses.ProjectOpen);

                    int projectId = ServiceFactory.CreateProjectService().CreateProject(project);
                    FlashMessageStore.Success("Project posted successfully. Project #" + projectId + " is now in My Projects.");
                    Response.Redirect("MyProjects.aspx?tab=All", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(savedPhysicalPath) && File.Exists(savedPhysicalPath))
                {
                    try { File.Delete(savedPhysicalPath); }
                    catch (IOException cleanupError) { System.Diagnostics.Trace.TraceWarning(cleanupError.ToString()); }
                }

                System.Diagnostics.Trace.TraceError("Project creation failed: " + ex);
                ShowMessage(ex is InvalidOperationException
                    ? ex.Message
                    : "Unable to post the project. Please review the details and try again.", false);
            }
        }

        private void ShowMessage(string message, bool succeeded)
        {
            lblMessage.Text = Server.HtmlEncode(message);
            lblMessage.CssClass = succeeded
                ? "form-message success-message"
                : "form-message error-message";
        }

        private static void SelectIfPresent(DropDownList list, string value)
        {
            if (list.Items.FindByValue(value ?? string.Empty) != null)
                list.SelectedValue = value ?? string.Empty;
        }
    }
}
