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
    public partial class FindFreelancers : Page
    {
        private static readonly string[] Categories =
        {
            "Web Development", "Mobile Development", "UI/UX Design",
            "Graphic Design", "WordPress", "Software Development", "Other"
        };

        private sealed class FreelancerCard
        {
            public int UserID { get; set; }
            public string Name { get; set; }
            public string Initials { get; set; }
            public string Title { get; set; }
            public string Rating { get; set; }
            public string Reviews { get; set; }
            public string Skills { get; set; }
            public string SkillsMarkup { get; set; }
            public string Categories { get; set; }
            public string Bio { get; set; }
            public string Location { get; set; }
            public string ProfileUrl { get; set; }
        }

        private IList<FreelancerCard> freelancers;

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                LoadFreelancers();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Freelancer search data could not be loaded: " + ex);
                freelancers = new List<FreelancerCard>();
            }

            BindFilters();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindFreelancers(freelancers);
            }
        }

        private void LoadFreelancers()
        {
            IUserService users = ServiceFactory.CreateUserService();
            IReviewService reviews = ServiceFactory.CreateReviewService();
            freelancers = users.GetUsersByRole("Freelancer")
                .Select(user =>
                {
                    string skills = user.Skills ?? string.Empty;
                    List<string> skillList = SplitSkills(skills);
                    List<string> categories = GetCategories(skillList);
                    string name = user.FullName ?? string.Empty;
                    string categoryTitle = categories.FirstOrDefault() ?? "Freelancer";
                    decimal rating = reviews.GetAverageRating(user.UserID);
                    int reviewCount = reviews.GetReviewCount(user.UserID);
                    return new FreelancerCard
                    {
                        UserID = user.UserID,
                        Name = name,
                        Initials = GetInitials(name),
                        Title = skillList.Count > 0 ? string.Join(" · ", skillList.Take(3)) : categoryTitle,
                        Rating = rating.ToString("0.0", CultureInfo.InvariantCulture),
                        Reviews = reviewCount.ToString(CultureInfo.InvariantCulture),
                        Skills = skills,
                        SkillsMarkup = string.Join(string.Empty, skillList.Select(skill =>
                            "<span>" + Server.HtmlEncode(skill) + "</span>")),
                        Categories = string.Join("|", categories),
                        Bio = string.IsNullOrWhiteSpace(user.Bio)
                            ? "This freelancer has not added an introduction yet."
                            : user.Bio,
                        Location = string.IsNullOrWhiteSpace(user.Location) ? string.Empty : user.Location,
                        ProfileUrl = "ViewFreelancerProfile.aspx?id=" + user.UserID.ToString(CultureInfo.InvariantCulture)
                    };
                })
                .OrderByDescending(card => decimal.Parse(card.Rating, CultureInfo.InvariantCulture))
                .ThenBy(card => card.Name)
                .ToList();
        }

        private void BindFilters()
        {
            ddlSkill.Items.Clear();
            ddlSkill.Items.Add(new ListItem("All Skills", string.Empty));
            foreach (string skill in freelancers.SelectMany(card => SplitSkills(card.Skills))
                .Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(skill => skill))
            {
                ddlSkill.Items.Add(new ListItem(skill, skill));
            }

            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("All Categories", string.Empty));
            foreach (string category in Categories)
                ddlCategory.Items.Add(new ListItem(category, category));
        }

        private void BindFreelancers(IEnumerable<FreelancerCard> data)
        {
            List<FreelancerCard> rows = data.ToList();
            rptFreelancers.DataSource = rows;
            rptFreelancers.DataBind();
            lblResultCount.Text = rows.Count == 1 ? "1 freelancer found" :
                rows.Count.ToString(CultureInfo.InvariantCulture) + " freelancers found";
            pnlNoFreelancers.Visible = rows.Count == 0;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlSkill.SelectedIndex = 0;
            ddlCategory.SelectedIndex = 0;
            BindFreelancers(freelancers);
        }

        private void ApplyFilters()
        {
            string search = txtSearch.Text.Trim();
            string skill = ddlSkill.SelectedValue;
            string category = ddlCategory.SelectedValue;
            IEnumerable<FreelancerCard> result = freelancers;

            if (!string.IsNullOrWhiteSpace(search))
            {
                result = result.Where(card => Contains(card.Name, search) ||
                    Contains(card.Title, search) || Contains(card.Skills, search) ||
                    Contains(card.Bio, search) || Contains(card.Location, search) ||
                    Contains(card.Categories, search));
            }
            if (!string.IsNullOrWhiteSpace(skill))
            {
                result = result.Where(card => SplitSkills(card.Skills).Any(item =>
                    string.Equals(item, skill, StringComparison.OrdinalIgnoreCase)));
            }
            if (!string.IsNullOrWhiteSpace(category))
            {
                result = result.Where(card => card.Categories.Split('|').Any(item =>
                    string.Equals(item, category, StringComparison.OrdinalIgnoreCase)));
            }

            BindFreelancers(result);
        }

        protected static List<string> SplitSkills(string skills)
        {
            return (skills ?? string.Empty)
                .Split(new[] { ',', ';', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(skill => skill.Trim())
                .Where(skill => skill.Length > 0)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static List<string> GetCategories(IList<string> skills)
        {
            List<string> categories = new List<string>();
            foreach (string skill in skills)
            {
                string value = skill.ToLowerInvariant();
                if (HasAny(value, "flutter", "android", "ios", "react native", "mobile", "kotlin", "swift"))
                    AddCategory(categories, "Mobile Development");
                if (HasAny(value, "figma", "ui/ux", "ux", "ui design", "adobe xd", "user experience"))
                    AddCategory(categories, "UI/UX Design");
                if (HasAny(value, "graphic", "photoshop", "illustrator", "branding", "logo design"))
                    AddCategory(categories, "Graphic Design");
                if (HasAny(value, "wordpress", "woocommerce"))
                    AddCategory(categories, "WordPress");
                if (HasAny(value, "web", "react", "javascript", "node", "html", "css", "php", "mongodb", "asp.net"))
                    AddCategory(categories, "Web Development");
                if (HasAny(value, "software", "c#", ".net", "java", "python", "sql", "c++", "database"))
                    AddCategory(categories, "Software Development");
            }
            if (categories.Count == 0)
                categories.Add("Other");
            return categories;
        }

        private static bool HasAny(string value, params string[] terms)
        {
            return terms.Any(term => value.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static void AddCategory(ICollection<string> categories, string category)
        {
            if (!categories.Contains(category))
                categories.Add(category);
        }

        private static bool Contains(string value, string search)
        {
            return (value ?? string.Empty).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string GetInitials(string fullName)
        {
            string[] parts = (fullName ?? string.Empty).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return "?";
            if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpperInvariant();
            return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpperInvariant();
        }
    }
}
