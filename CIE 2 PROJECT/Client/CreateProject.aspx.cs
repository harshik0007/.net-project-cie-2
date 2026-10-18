
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class CreateProject : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Enable file uploads in the ASP.NET master-page form.
            if (Page.Form != null)
            {
                Page.Form.Enctype = "multipart/form-data";
            }
        }

        protected void cvProjectTitle_ServerValidate(
            object source, ServerValidateEventArgs args)
        {
            string value = args.Value.Trim();
            args.IsValid = value.Length >= 5 && value.Length <= 100;
        }

        protected void cvDescription_ServerValidate(
            object source, ServerValidateEventArgs args)
        {
            string value = args.Value.Trim();
            args.IsValid = value.Length >= 20 && value.Length <= 2000;
        }

        protected void cvCategory_ServerValidate(
            object source, ServerValidateEventArgs args)
        {
            args.IsValid =
                !string.IsNullOrWhiteSpace(ddlCategory.SelectedValue);
        }

        protected void cvExperience_ServerValidate(
            object source, ServerValidateEventArgs args)
        {
            args.IsValid =
                !string.IsNullOrWhiteSpace(ddlExperience.SelectedValue);
        }

        protected void cvMinBudget_ServerValidate(
            object source, ServerValidateEventArgs args)
        {
            decimal value;

            args.IsValid =
                decimal.TryParse(
                    args.Value.Trim(),
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out value)
                && value > 0
                && value <= 99999999.99m;
        }

        protected void cvMaxBudget_ServerValidate(
            object source, ServerValidateEventArgs args)
        {
            decimal minBudget;
            decimal maxBudget;

            bool minValid = decimal.TryParse(
                txtMinBudget.Text.Trim(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out minBudget);

            bool maxValid = decimal.TryParse(
                txtMaxBudget.Text.Trim(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out maxBudget);

            args.IsValid =
                minValid &&
                maxValid &&
                minBudget > 0 &&
                maxBudget >= minBudget &&
                maxBudget <= 99999999.99m;
        }

        protected void cvDeadline_ServerValidate(
            object source, ServerValidateEventArgs args)
        {
            DateTime deadline;

            bool validDate = DateTime.TryParseExact(
                args.Value.Trim(),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out deadline);

            args.IsValid =
                validDate && deadline.Date > DateTime.Today;
        }

        protected void btnPostProject_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid)
            {
                return;
            }

            decimal minBudget;
            decimal maxBudget;
            DateTime deadline;

            bool minValid = decimal.TryParse(
                txtMinBudget.Text.Trim(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out minBudget);

            bool maxValid = decimal.TryParse(
                txtMaxBudget.Text.Trim(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out maxBudget);

            bool dateValid = DateTime.TryParseExact(
                txtDeadline.Text.Trim(),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out deadline);

            if (!minValid ||
                !maxValid ||
                !dateValid ||
                minBudget <= 0 ||
                minBudget > 99999999.99m ||
                maxBudget < minBudget ||
                maxBudget > 99999999.99m ||
                deadline.Date <= DateTime.Today)
            {
                lblMessage.Text =
                    "Please check your budget and deadline values.";
                lblMessage.CssClass = "form-message";
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
                lblMessage.Text =
                    "Required skills must be at most 500 characters " +
                    "and location must be at most 100 characters.";
                lblMessage.CssClass = "form-message";
                return;
            }

            // Temporary test client created in SkillLinkDB.
            // Replace this with the authenticated client's UserID later.
            const int testClientId = 1;

            string attachmentPath = null;
            string savedPhysicalPath = null;

            string[] allowedExtensions =
            {
                ".pdf", ".doc", ".docx",
                ".zip", ".png", ".jpg", ".jpeg"
            };

            // Validate the file before saving it.
            if (fuProjectFiles.HasFile)
            {
                string extension = Path.GetExtension(
                    fuProjectFiles.FileName).ToLowerInvariant();

                if (Array.IndexOf(allowedExtensions, extension) < 0)
                {
                    lblMessage.Text =
                        "File type not allowed. Choose PDF, DOC, DOCX, " +
                        "ZIP, PNG or JPG.";
                    lblMessage.CssClass = "form-message";
                    return;
                }

                const int maxFileSize = 3 * 1024 * 1024;

                int fileSize = fuProjectFiles.PostedFile.ContentLength;

                if (fileSize <= 0 || fileSize > maxFileSize)
                {
                    lblMessage.Text =
                        "The attachment must be between 1 byte and 3 MB.";
                    lblMessage.CssClass = "form-message";
                    return;
                }
            }

            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings[
                    "SkillLinkDBConnection"];

            if (settings == null ||
                string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                lblMessage.Text =
                    "Database connection setting was not found in Web.config.";
                lblMessage.CssClass = "form-message";
                return;
            }

            string sql = @"
                INSERT INTO dbo.Projects
                (
                    ClientID,
                    Title,
                    Description,
                    Category,
                    ExperienceLevel,
                    RequiredSkills,
                    MinBudget,
                    MaxBudget,
                    Deadline,
                    PreferredLocation,
                    AttachmentPath,
                    Status,
                    CreatedAt
                )
                VALUES
                (
                    @ClientID,
                    @Title,
                    @Description,
                    @Category,
                    @ExperienceLevel,
                    @RequiredSkills,
                    @MinBudget,
                    @MaxBudget,
                    @Deadline,
                    @PreferredLocation,
                    @AttachmentPath,
                    @Status,
                    @CreatedAt
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            try
            {
                // Save the attachment to the project folder.
                // The original filename is never used as the disk filename.
                if (fuProjectFiles.HasFile)
                {
                    string extension = Path.GetExtension(
                        fuProjectFiles.FileName).ToLowerInvariant();

                    string uploadFolder = Server.MapPath(
                        "~/Uploads/ProjectAttachments/");

                    Directory.CreateDirectory(uploadFolder);

                    string newFileName =
                        Guid.NewGuid().ToString("N") + extension;

                    savedPhysicalPath = Path.Combine(
                        uploadFolder, newFileName);

                    attachmentPath =
                        "~/Uploads/ProjectAttachments/" + newFileName;

                    fuProjectFiles.SaveAs(savedPhysicalPath);
                }

                using (SqlConnection con =
                    new SqlConnection(settings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add(
                        "@ClientID", SqlDbType.Int).Value = testClientId;

                    cmd.Parameters.Add(
                        "@Title", SqlDbType.NVarChar, 100).Value = title;

                    cmd.Parameters.Add(
                        "@Description", SqlDbType.NVarChar, 2000).Value =
                        description;

                    cmd.Parameters.Add(
                        "@Category", SqlDbType.NVarChar, 100).Value =
                        category;

                    cmd.Parameters.Add(
                        "@ExperienceLevel", SqlDbType.NVarChar, 30).Value =
                        experience;

                    cmd.Parameters.Add(
                        "@RequiredSkills", SqlDbType.NVarChar, 500).Value =
                        string.IsNullOrWhiteSpace(skills)
                            ? (object)DBNull.Value
                            : skills;

                    SqlParameter minParam =
                        cmd.Parameters.Add(
                            "@MinBudget", SqlDbType.Decimal);

                    minParam.Precision = 10;
                    minParam.Scale = 2;
                    minParam.Value = minBudget;

                    SqlParameter maxParam =
                        cmd.Parameters.Add(
                            "@MaxBudget", SqlDbType.Decimal);

                    maxParam.Precision = 10;
                    maxParam.Scale = 2;
                    maxParam.Value = maxBudget;

                    cmd.Parameters.Add(
                        "@Deadline", SqlDbType.Date).Value = deadline.Date;

                    cmd.Parameters.Add(
                        "@PreferredLocation",
                        SqlDbType.NVarChar, 100).Value =
                        string.IsNullOrWhiteSpace(location)
                            ? (object)DBNull.Value
                            : location;

                    cmd.Parameters.Add(
                        "@AttachmentPath",
                        SqlDbType.NVarChar, 255).Value =
                        attachmentPath == null
                            ? (object)DBNull.Value
                            : attachmentPath;

                    cmd.Parameters.Add(
                        "@Status", SqlDbType.NVarChar, 30).Value =
                        "Open for Proposals";

                    cmd.Parameters.Add(
                        "@CreatedAt", SqlDbType.DateTime).Value =
                        DateTime.Now;

                    con.Open();

                    int newProjectId =
                        Convert.ToInt32(cmd.ExecuteScalar());

                    lblMessage.Text =
                        "Project posted successfully! Project ID: " +
                        newProjectId;

                    lblMessage.CssClass =
                        "form-message success-message";
                }
            }
            catch (Exception ex)
            {
                // Remove the file if the database insert fails.
                if (!string.IsNullOrEmpty(savedPhysicalPath) &&
                    File.Exists(savedPhysicalPath))
                {
                    try
                    {
                        File.Delete(savedPhysicalPath);
                    }
                    catch
                    {
                        // Do not hide the original error.
                    }
                }

                lblMessage.Text =
                    "Unable to post project: " +
                    Server.HtmlEncode(ex.Message);

                lblMessage.CssClass = "form-message";
            }
        }
    }
}
