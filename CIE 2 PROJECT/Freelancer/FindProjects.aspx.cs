using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class FindProjects : Page
    {
        private class Project
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public string ExperienceLevel { get; set; }
            public string RequiredSkills { get; set; }
            public decimal MinBudget { get; set; }
            public decimal MaxBudget { get; set; }
            public DateTime Deadline { get; set; }
            public DateTime CreatedAt { get; set; }
            public string AttachmentPath { get; set; }
            public int ProposalCount { get; set; }
        }

        private string ConnectionString
        {
            get
            {
                ConnectionStringSettings settings =
                    ConfigurationManager.ConnectionStrings[
                        "SkillLinkDBConnection"];

                if (settings == null ||
                    string.IsNullOrWhiteSpace(settings.ConnectionString))
                {
                    throw new InvalidOperationException(
                        "SkillLinkDBConnection is missing from Web.config.");
                }

                return settings.ConnectionString;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    LoadCategories();
                    BindProjects();
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            }
        }

        private void LoadCategories()
        {
            ddlCategory.Items.Clear();

            ddlCategory.Items.Add(
                new ListItem("All Categories", ""));

            string sql = @"
                SELECT DISTINCT Category
                FROM dbo.Projects
                WHERE Status = @Status
                ORDER BY Category;
            ";

            using (SqlConnection con =
                new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add(
                    "@Status", SqlDbType.NVarChar, 30).Value =
                    "Open for Proposals";

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string category = reader.GetString(0);

                        ddlCategory.Items.Add(
                            new ListItem(category, category));
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindProjects();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        protected void ddlCategory_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            try
            {
                BindProjects();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void BindProjects()
        {
            List<Project> projects = new List<Project>();

            string searchText = txtSearch.Text.Trim();
            string searchPattern = string.IsNullOrWhiteSpace(searchText)
                ? ""
                : "%" + searchText + "%";

            string selectedCategory = ddlCategory.SelectedValue;

            string sql = @"
                SELECT
                    p.ProjectID AS Id,
                    p.Title,
                    p.Description,
                    p.Category,
                    p.ExperienceLevel,
                    p.RequiredSkills,
                    p.MinBudget,
                    p.MaxBudget,
                    p.Deadline,
                    p.CreatedAt,
                    p.AttachmentPath,
                    (
                        SELECT COUNT(*)
                        FROM dbo.Proposals AS pr
                        WHERE pr.ProjectID = p.ProjectID
                    ) AS ProposalCount
                FROM dbo.Projects AS p
                WHERE p.Status = @Status
                    AND (
                        @Category = N''
                        OR p.Category = @Category
                    )
                    AND (
                        @Search = N''
                        OR p.Title LIKE @Search
                        OR p.Description LIKE @Search
                        OR ISNULL(p.RequiredSkills, N'') LIKE @Search
                        OR p.Category LIKE @Search
                    )
                ORDER BY p.CreatedAt DESC, p.ProjectID DESC;
            ";

            using (SqlConnection con =
                new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add(
                    "@Status", SqlDbType.NVarChar, 30).Value =
                    "Open for Proposals";

                cmd.Parameters.Add(
                    "@Category", SqlDbType.NVarChar, 100).Value =
                    selectedCategory;

                cmd.Parameters.Add(
                    "@Search", SqlDbType.NVarChar, 500).Value =
                    searchPattern;

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Project project = new Project();

                        project.Id =
                            Convert.ToInt32(reader["Id"]);

                        project.Title =
                            Convert.ToString(reader["Title"]);

                        project.Description =
                            Convert.ToString(reader["Description"]);

                        project.Category =
                            Convert.ToString(reader["Category"]);

                        project.ExperienceLevel =
                            Convert.ToString(reader["ExperienceLevel"]);

                        project.RequiredSkills =
                            reader["RequiredSkills"] == DBNull.Value
                                ? ""
                                : Convert.ToString(reader["RequiredSkills"]);

                        project.MinBudget =
                            Convert.ToDecimal(reader["MinBudget"]);

                        project.MaxBudget =
                            Convert.ToDecimal(reader["MaxBudget"]);

                        project.Deadline =
                            Convert.ToDateTime(reader["Deadline"]);

                        project.CreatedAt =
                            Convert.ToDateTime(reader["CreatedAt"]);

                        project.AttachmentPath =
                            reader["AttachmentPath"] == DBNull.Value
                                ? null
                                : Convert.ToString(reader["AttachmentPath"]);

                        project.ProposalCount =
                            Convert.ToInt32(reader["ProposalCount"]);

                        projects.Add(project);
                    }
                }
            }

            rptProjects.DataSource = projects;
            rptProjects.DataBind();

            pnlEmpty.Visible = projects.Count == 0;
            lblPageMessage.Visible = false;
        }

        private void ShowError(Exception ex)
        {
            rptProjects.DataSource = new List<Project>();
            rptProjects.DataBind();

            pnlEmpty.Visible = true;
            lblPageMessage.Visible = true;

            lblPageMessage.Text =
                "Unable to load projects: " +
                Server.HtmlEncode(ex.Message);
        }

        protected string FormatBudget(object minValue, object maxValue)
        {
            decimal minBudget = Convert.ToDecimal(minValue);
            decimal maxBudget = Convert.ToDecimal(maxValue);

            CultureInfo culture = CultureInfo.GetCultureInfo("en-IN");

            return string.Format(
                culture,
                "₹{0:N0} - ₹{1:N0}",
                minBudget,
                maxBudget);
        }

        protected string FormatDeadline(object value)
        {
            DateTime deadline = Convert.ToDateTime(value);

            return "Deadline: " + deadline.ToString(
                "dd MMM, yyyy",
                CultureInfo.InvariantCulture);
        }

        protected string GetPostedText(object value)
        {
            DateTime created = Convert.ToDateTime(value);
            int days = (DateTime.Today - created.Date).Days;

            if (days <= 0)
            {
                return "Posted today";
            }

            if (days == 1)
            {
                return "Posted 1 day ago";
            }

            return "Posted " + days + " days ago";
        }

        protected string GetProposalText(object value)
        {
            int count = Convert.ToInt32(value);

            return count + (count == 1 ? " proposal" : " proposals");
        }

        protected bool HasText(object value)
        {
            return value != null &&
                value != DBNull.Value &&
                !string.IsNullOrWhiteSpace(Convert.ToString(value));
        }

        protected bool HasAttachment(object value)
        {
            if (!HasText(value))
            {
                return false;
            }

            string path = Convert.ToString(value);

            return path.StartsWith(
                "~/Uploads/ProjectAttachments/",
                StringComparison.OrdinalIgnoreCase);
        }

        protected string ResolveAttachmentUrl(object value)
        {
            if (!HasAttachment(value))
            {
                return "#";
            }

            string path = Convert.ToString(value);

            return ResolveUrl(path);
        }

        protected string GetIconContainerClass(object value)
        {
            string category = Convert.ToString(value).ToLowerInvariant();

            if (category.Contains("game"))
            {
                return "project-icon game-icon";
            }

            if (category.Contains("writing"))
            {
                return "project-icon writing-icon";
            }

            if (category.Contains("design") ||
                category.Contains("marketing"))
            {
                return "project-icon design-icon";
            }

            return "project-icon web-icon";
        }

        protected string GetCategoryIcon(object value)
        {
            string category = Convert.ToString(value).ToLowerInvariant();

            if (category.Contains("game"))
            {
                return "fa-solid fa-gamepad";
            }

            if (category.Contains("writing"))
            {
                return "fa-solid fa-file-lines";
            }

            if (category.Contains("design"))
            {
                return "fa-solid fa-palette";
            }

            if (category.Contains("marketing"))
            {
                return "fa-solid fa-bullhorn";
            }

            if (category.Contains("mobile"))
            {
                return "fa-solid fa-mobile-screen";
            }

            return "fa-solid fa-code";
        }

        protected string GetTagClass(object value)
        {
            string category = Convert.ToString(value).ToLowerInvariant();

            if (category.Contains("writing"))
            {
                return "yellow";
            }

            if (category.Contains("design") ||
                category.Contains("marketing"))
            {
                return "purple";
            }

            if (category.Contains("game"))
            {
                return "green";
            }

            return "blue";
        }
    }
}