using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class ProjectProposal : Page
    {
        private class Proposal
        {
            public int Id { get; set; }
            public int FreelancerId { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
            public double Rating { get; set; }
            public int Reviews { get; set; }
            public string Location { get; set; }
            public string CoverLetter { get; set; }
            public string Skills { get; set; }
            public string ProposedPrice { get; set; }
            public int PriceValue { get; set; }
            public int DeliveryTime { get; set; }
        }

        private List<Proposal> proposals;

        protected void Page_Load(object sender, EventArgs e)
        {
            CreateProposalData();

            if (!IsPostBack)
            {
                BindProposals(proposals);
            }
        }

        private void CreateProposalData()
        {
            proposals = new List<Proposal>
            {
                new Proposal
                {
                    Id = 1,
                    FreelancerId = 1,
                    Name = "Rahul Sharma",
                    Title = "Full Stack Developer",
                    Rating = 4.8,
                    Reviews = 26,
                    Location = "India",
                    CoverLetter = "Hi! I have 4+ years of experience in Unity game development. I can build your 2D shooting game with clean code and smooth gameplay. I will deliver high quality work.",
                    Skills = "Unity,C#,Game Design,2D Animation",
                    ProposedPrice = "₹20,000",
                    PriceValue = 20000,
                    DeliveryTime = 15
                },

                new Proposal
                {
                    Id = 2,
                    FreelancerId = 2,
                    Name = "Priya Mehta",
                    Title = "Game Developer",
                    Rating = 4.9,
                    Reviews = 18,
                    Location = "India",
                    CoverLetter = "I can develop a fun and engaging 2D game for you using Unity. I will ensure quality, performance and on-time delivery.",
                    Skills = "Unity,C#,Game Design,UI/UX",
                    ProposedPrice = "₹18,000",
                    PriceValue = 18000,
                    DeliveryTime = 12
                },

                new Proposal
                {
                    Id = 3,
                    FreelancerId = 3,
                    Name = "Amit Verma",
                    Title = "Unity Developer",
                    Rating = 4.7,
                    Reviews = 32,
                    Location = "India",
                    CoverLetter = "I will create your 2D shooting game with multiple levels, enemies and in-game store. I have completed similar projects.",
                    Skills = "Unity,C#,2D Animation,Game Design",
                    ProposedPrice = "₹22,000",
                    PriceValue = 22000,
                    DeliveryTime = 18
                }
            };
        }

        private void BindProposals(List<Proposal> data)
        {
            rptProposals.DataSource = data;
            rptProposals.DataBind();

            lblProposalCount.Text = data.Count.ToString();
        }

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            IEnumerable<Proposal> result = proposals;

            switch (ddlSort.SelectedValue)
            {
                case "PriceLow":
                    result = proposals.OrderBy(p => p.PriceValue);
                    break;

                case "PriceHigh":
                    result = proposals.OrderByDescending(p => p.PriceValue);
                    break;

                case "Delivery":
                    result = proposals.OrderBy(p => p.DeliveryTime);
                    break;

                case "Rating":
                    result = proposals.OrderByDescending(p => p.Rating);
                    break;

                default:
                    result = proposals;
                    break;
            }

            BindProposals(result.ToList());
        }

        protected string GetSkills(string skills)
        {
            string[] skillList = skills.Split(',');

            string result = "";

            foreach (string skill in skillList)
            {
                result += "<span>" +
                          Server.HtmlEncode(skill.Trim()) +
                          "</span>";
            }

            return result;
        }

        protected void HireFreelancer_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;

            string proposalId = button.CommandArgument;

            Response.Redirect(
                "ActiveProjects.aspx?proposalId=" + proposalId);
        }
    }
}