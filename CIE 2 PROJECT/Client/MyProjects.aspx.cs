
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class MyProjects : Page
    {
        // Temporary client ID until authentication is implemented.
        private const int TestClientId = 1;

        private List<Project> projects = new List<Project>();

        private class Project
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Budget { get; set; }
            public int Proposals { get; set; }
            public string Deadline { get; set; }
            public string Status { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                projects = GetProjectsFromDatabase();

                lblLoadError.Visible = false;
            }
            catch (Exception)
            {
                projects = new List<Project>();

                lblLoadError.Text =
                    "Unable to load projects. Please check the database connection.";

                lblLoadError.Visible = true;
            }

            if (!IsPostBack)
            {
                BindProjects(projects);
            }
        }

        private List<Project> GetProjectsFromDatabase()
        {
            List<Project> result = new List<Project>();

            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings[
                    "SkillLinkDBConnection"];

            if (settings == null ||
                string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new InvalidOperationException(
                    "SkillLinkDBConnection is missing from Web.config.");
            }

            string sql = @"
                SELECT
                    p.ProjectID AS Id,
                    p.Title,
                    p.Description,
                    p.MinBudget,
                    p.MaxBudget,
                    (
                        SELECT COUNT(*)
                        FROM dbo.Proposals AS pr
                        WHERE pr.ProjectID = p.ProjectID
                    ) AS ProposalCount,
                    p.Deadline,
                    p.Status
                FROM dbo.Projects AS p
                WHERE p.ClientID = @ClientID
                ORDER BY p.CreatedAt DESC, p.ProjectID DESC;
            ";

            using (SqlConnection con =
                new SqlConnection(settings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@ClientID", SqlDbType.Int).Value =
                    TestClientId;

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        decimal minBudget =
                            reader.GetDecimal(
                                reader.GetOrdinal("MinBudget"));

                        decimal maxBudget =
                            reader.GetDecimal(
                                reader.GetOrdinal("MaxBudget"));

                        DateTime deadline =
                            reader.GetDateTime(
                                reader.GetOrdinal("Deadline"));

                        Project project = new Project
                        {
                            Id = reader.GetInt32(
                                reader.GetOrdinal("Id")),

                            Title = reader.GetString(
                                reader.GetOrdinal("Title")),

                            Description = reader.GetString(
                                reader.GetOrdinal("Description")),

                            Budget = string.Format(
                                CultureInfo.InvariantCulture,
                                "₹{0:N0} - ₹{1:N0}",
                                minBudget,
                                maxBudget),

                            Proposals = reader.GetInt32(
                                reader.GetOrdinal("ProposalCount")),

                            Deadline = deadline.ToString(
                                "dd MMM, yyyy",
                                CultureInfo.InvariantCulture),

                            Status = reader.GetString(
                                reader.GetOrdinal("Status"))
                        };

                        result.Add(project);
                    }
                }
            }

            return result;
        }

        private void BindProjects(List<Project> data)
        {
            rptProjects.DataSource = data;
            rptProjects.DataBind();

            pnlEmpty.Visible = data.Count == 0;
        }

        protected void Filter_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;
            string filter = button.CommandArgument;

            SetActiveTab(button);

            if (filter == "All")
            {
                BindProjects(projects);
                return;
            }

            List<Project> filteredProjects = projects
                .Where(p => string.Equals(
                    p.Status,
                    filter,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();

            BindProjects(filteredProjects);
        }

        private void SetActiveTab(LinkButton activeButton)
        {
            lnkAll.CssClass = "project-tab";
            lnkOpen.CssClass = "project-tab";
            lnkProgress.CssClass = "project-tab";
            lnkCompleted.CssClass = "project-tab";
            lnkCancelled.CssClass = "project-tab";

            activeButton.CssClass = "project-tab active";
        }

        protected string GetStatusClass(string status)
        {
            if (status == "Open for Proposals")
            {
                return "status-badge status-open";
            }

            if (status == "In Progress")
            {
                return "status-badge status-progress";
            }

            if (status == "Completed")
            {
                return "status-badge status-completed";
            }

            if (status == "Cancelled")
            {
                return "status-badge status-cancelled";
            }

            return "status-badge";
        }
    }
}
