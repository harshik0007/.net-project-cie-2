using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class FindProjects : Page
    {
        private static readonly string[] AvailableCategories =
        {
            "Web Development",
            "Mobile Development",
            "UI/UX Design",
            "Graphic Design",
            "WordPress",
            "Software Development",
            "Other"
        };

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
            public DateTime? Deadline { get; set; }
            public DateTime CreatedAt { get; set; }
            public string AttachmentPath { get; set; }
            public int ProposalCount { get; set; }
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
            ddlCategory.Items.Add(new ListItem("All Categories", ""));
            foreach (string category in AvailableCategories)
                ddlCategory.Items.Add(new ListItem(category, category));
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
            string selectedCategory = ddlCategory.SelectedValue;

            IList<CIE_2_PROJECT.Models.Project> records =
                ServiceFactory.CreateProjectService().FindOpenProjects(searchText, selectedCategory);
            IProposalService proposalService = ServiceFactory.CreateProposalService();

            foreach (CIE_2_PROJECT.Models.Project record in records)
            {
                projects.Add(new Project
                {
                    Id = record.ProjectID,
                    Title = record.Title,
                    Description = record.Description,
                    Category = record.Category,
                    ExperienceLevel = record.ExperienceLevel,
                    RequiredSkills = record.RequiredSkills ?? string.Empty,
                    MinBudget = record.MinBudget,
                    MaxBudget = record.MaxBudget,
                    Deadline = record.Deadline,
                    CreatedAt = record.CreatedAt,
                    AttachmentPath = record.AttachmentPath,
                    ProposalCount = proposalService.GetProposalCount(record.ProjectID)
                });
            }

            rptProjects.DataSource = projects;
            rptProjects.DataBind();

            pnlEmpty.Visible = projects.Count == 0;
            lblPageMessage.Visible = false;
        }

        private void ShowError(Exception ex)
        {
            System.Diagnostics.Trace.TraceError("Project discovery failed: " + ex);
            rptProjects.DataSource = new List<Project>();
            rptProjects.DataBind();

            pnlEmpty.Visible = true;
            lblPageMessage.Visible = true;

            lblPageMessage.Text = "Unable to load projects. Please try again.";
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
            if (value == null || value == DBNull.Value)
                return "Deadline not specified";
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
