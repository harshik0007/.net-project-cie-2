using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class Payments : Page
    {
        private class PendingPayment
        {
            public int Id { get; set; }
            public string Project { get; set; }
            public string ProjectDescription { get; set; }
            public string Freelancer { get; set; }
            public string Initials { get; set; }
            public string Role { get; set; }
            public string Amount { get; set; }
            public string Milestone { get; set; }
            public string MilestoneDetails { get; set; }
            public string DueDate { get; set; }
        }

        private class CompletedPayment
        {
            public int Id { get; set; }
            public string Project { get; set; }
            public string ProjectDescription { get; set; }
            public string Freelancer { get; set; }
            public string Initials { get; set; }
            public string Role { get; set; }
            public string Amount { get; set; }
            public string Milestone { get; set; }
            public string MilestoneDetails { get; set; }
            public string PaidOn { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPendingPayments();
                BindCompletedPayments();
            }
        }

        private void BindPendingPayments()
        {
            List<PendingPayment> payments = new List<PendingPayment>
            {
                new PendingPayment
                {
                    Id = 1,
                    Project = "Unity 2D Game Development",
                    ProjectDescription = "Create a 2D shooting game with multiple levels...",
                    Freelancer = "Rahul Sharma",
                    Initials = "RS",
                    Role = "Game Developer",
                    Amount = "₹25,000",
                    Milestone = "Milestone 2",
                    MilestoneDetails = "Game mechanics & levels",
                    DueDate = "30 May, 2026"
                },

                new PendingPayment
                {
                    Id = 2,
                    Project = "E-commerce Website Development",
                    ProjectDescription = "Build a fully responsive e-commerce website...",
                    Freelancer = "Priya Mehta",
                    Initials = "PM",
                    Role = "Full Stack Developer",
                    Amount = "₹20,000",
                    Milestone = "Milestone 1",
                    MilestoneDetails = "Frontend development",
                    DueDate = "25 May, 2026"
                }
            };

            rptPending.DataSource = payments;
            rptPending.DataBind();
        }

        private void BindCompletedPayments()
        {
            List<CompletedPayment> payments = new List<CompletedPayment>
            {
                new CompletedPayment
                {
                    Id = 101,
                    Project = "Brand Logo Design",
                    ProjectDescription = "Design a modern and minimal logo...",
                    Freelancer = "Neha Patel",
                    Initials = "NP",
                    Role = "Graphic Designer",
                    Amount = "₹5,000",
                    Milestone = "Full Payment",
                    MilestoneDetails = "Project completed",
                    PaidOn = "18 May, 2026"
                },

                new CompletedPayment
                {
                    Id = 102,
                    Project = "Portfolio Website",
                    ProjectDescription = "Build a responsive portfolio website...",
                    Freelancer = "Amit Verma",
                    Initials = "AV",
                    Role = "Web Developer",
                    Amount = "₹18,000",
                    Milestone = "Full Payment",
                    MilestoneDetails = "Project completed",
                    PaidOn = "10 May, 2026"
                },

                new CompletedPayment
                {
                    Id = 103,
                    Project = "Mobile App UI Design",
                    ProjectDescription = "Design clean and modern mobile app...",
                    Freelancer = "Ananya Singh",
                    Initials = "AS",
                    Role = "UI/UX Designer",
                    Amount = "₹12,500",
                    Milestone = "Full Payment",
                    MilestoneDetails = "Project completed",
                    PaidOn = "02 May, 2026"
                }
            };

            rptCompleted.DataSource = payments;
            rptCompleted.DataBind();
        }

        protected void PaymentTab_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;

            string selectedTab = button.CommandArgument;

            if (selectedTab == "Pending")
            {
                pnlPending.Visible = true;
                pnlCompleted.Visible = false;

                lnkPending.CssClass = "payment-tab active";
                lnkCompleted.CssClass = "payment-tab";
            }
            else
            {
                pnlPending.Visible = false;
                pnlCompleted.Visible = true;

                lnkPending.CssClass = "payment-tab";
                lnkCompleted.CssClass = "payment-tab active";
            }
        }

        protected void ReleasePayment_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;

            string paymentId = button.CommandArgument;

            BindPendingPayments();
        }

        protected void CancelProject_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;

            string paymentId = button.CommandArgument;

            BindPendingPayments();
        }

        protected void ViewDetails_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;

            string paymentId = button.CommandArgument;
        }
    }
}