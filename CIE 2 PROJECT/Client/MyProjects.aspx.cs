using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class MyProjects : Page
    {
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

        private List<Project> projects;

        protected void Page_Load(object sender, EventArgs e)
        {
            CreateProjectData();

            if (!IsPostBack)
            {
                BindProjects(projects);
            }
        }

        private void CreateProjectData()
        {
            projects = new List<Project>
            {
                new Project
                {
                    Id = 1,
                    Title = "Unity 2D Game Development",
                    Description = "Create a 2D shooting game with multiple levels, enemies, and in-game store.",
                    Budget = "&#8377;15,000 - &#8377;25,000",
                    Proposals = 12,
                    Deadline = "30 May, 2026",
                    Status = "Open for Proposals"
                }
            };

            projects.Add(new Project
            {
                Id = 2,
                Title = "Unity 2D Game Development",
                Description = "Create a 2D shooting game with multiple levels, enemies, and in-game store.",
                Budget = "&#8377;15,000 - &#8377;25,000",
                Proposals = 12,
                Deadline = "30 May, 2026",
                Status = "In Progress"
            });
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
                .Where(p => p.Status.Equals(
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
