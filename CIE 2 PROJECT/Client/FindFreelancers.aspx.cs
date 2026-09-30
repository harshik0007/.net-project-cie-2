using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class FindFreelancers : Page
    {
        private class Freelancer
        {
            public string Name { get; set; }
            public string Initials { get; set; }
            public string Title { get; set; }
            public string Rating { get; set; }
            public string Reviews { get; set; }
            public string Skills { get; set; }
            public string Category { get; set; }
            public int Experience { get; set; }
            public string Bio { get; set; }
        }

        private List<Freelancer> freelancers;

        protected void Page_Load(object sender, EventArgs e)
        {
            CreateFreelancerData();

            if (!IsPostBack)
            {
                BindFreelancers(freelancers);
            }
        }

        private void CreateFreelancerData()
        {
            freelancers = new List<Freelancer>
            {
                new Freelancer
                {
                    Name = "Rahul Sharma",
                    Initials = "RS",
                    Title = "Full Stack Developer",
                    Rating = "4.8",
                    Reviews = "26",
                    Skills = "React.js,Node.js,MongoDB",
                    Category = "Web Development",
                    Experience = 4,
                    Bio = "Experienced developer specializing in modern web applications and scalable backend systems."
                },

                new Freelancer
                {
                    Name = "Priya Mehta",
                    Initials = "PM",
                    Title = "UI/UX Designer",
                    Rating = "4.9",
                    Reviews = "18",
                    Skills = "Figma,UI/UX Design,Adobe XD",
                    Category = "UI/UX Design",
                    Experience = 3,
                    Bio = "Creative designer focused on clean interfaces, user experience and modern digital products."
                },

                new Freelancer
                {
                    Name = "Amit Verma",
                    Initials = "AV",
                    Title = "WordPress Developer",
                    Rating = "4.7",
                    Reviews = "32",
                    Skills = "WordPress,PHP,Elementor",
                    Category = "WordPress",
                    Experience = 5,
                    Bio = "WordPress developer experienced in business websites, custom themes and website optimization."
                },

                new Freelancer
                {
                    Name = "Neha Patel",
                    Initials = "NP",
                    Title = "Graphic Designer",
                    Rating = "4.9",
                    Reviews = "21",
                    Skills = "Graphic Design,Photoshop,Illustrator",
                    Category = "Graphic Design",
                    Experience = 4,
                    Bio = "Graphic designer creating brand identities, marketing materials and professional visual designs."
                },

                new Freelancer
                {
                    Name = "Karan Shah",
                    Initials = "KS",
                    Title = "Backend Developer",
                    Rating = "4.8",
                    Reviews = "17",
                    Skills = "Node.js,MongoDB,PHP",
                    Category = "Web Development",
                    Experience = 6,
                    Bio = "Backend developer focused on database design, server-side applications and reliable systems."
                },

                new Freelancer
                {
                    Name = "Ananya Singh",
                    Initials = "AS",
                    Title = "Mobile App Developer",
                    Rating = "4.7",
                    Reviews = "15",
                    Skills = "Flutter,UI/UX Design,Figma",
                    Category = "Mobile Development",
                    Experience = 3,
                    Bio = "Mobile application developer building clean and user-friendly cross-platform applications."
                }
            };
        }

        private void BindFreelancers(List<Freelancer> data)
        {
            rptFreelancers.DataSource = data;
            rptFreelancers.DataBind();

            lblResultCount.Text = data.Count + " freelancers found";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlSkill.SelectedIndex = 0;
            ddlCategory.SelectedIndex = 0;
            ddlExperience.SelectedIndex = 0;

            BindFreelancers(freelancers);
        }

        private void ApplyFilters()
        {
            string search = txtSearch.Text.Trim().ToLower();
            string skill = ddlSkill.SelectedValue;
            string category = ddlCategory.SelectedValue;
            string experience = ddlExperience.SelectedValue;

            IEnumerable<Freelancer> result = freelancers;

            if (!string.IsNullOrWhiteSpace(search))
            {
                result = result.Where(f =>
                    f.Name.ToLower().Contains(search) ||
                    f.Title.ToLower().Contains(search) ||
                    f.Skills.ToLower().Contains(search) ||
                    f.Bio.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(skill))
            {
                result = result.Where(f =>
                    f.Skills.Split(',').Any(s =>
                        s.Equals(skill, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                result = result.Where(f =>
                    f.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            if (experience == "0-2")
            {
                result = result.Where(f => f.Experience <= 2);
            }
            else if (experience == "2-5")
            {
                result = result.Where(f => f.Experience > 2 && f.Experience <= 5);
            }
            else if (experience == "5+")
            {
                result = result.Where(f => f.Experience > 5);
            }

            BindFreelancers(result.ToList());
        }

        protected string GetSkills(string skills)
        {
            string[] skillList = skills.Split(',');

            string result = "";

            foreach (string skill in skillList)
            {
                result += "<span>" + Server.HtmlEncode(skill.Trim()) + "</span>";
            }

            return result;
        }
    }
}