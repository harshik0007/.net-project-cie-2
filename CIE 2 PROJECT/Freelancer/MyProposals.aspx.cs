using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class MyProposals : System.Web.UI.Page
    {
        // ── Data Model ───────────────────────────────────────────────────────────
        private class ProposalItem
        {
            public string Title       { get; set; }
            public string Client      { get; set; }
            public string PostedOn    { get; set; }
            public string Description { get; set; }
            public string Price       { get; set; }
            public string Delivery    { get; set; }
            public string SubmittedOn { get; set; }
            public string Status      { get; set; }   // pending / accepted / rejected

            // Derived display helpers
            public string IconClass   { get; set; }
            public string IconFa      { get; set; }
            public string BadgeClass  { get; set; }
            public string BadgeIcon   { get; set; }
            public string StatusLabel { get; set; }
        }

        // ── Static proposal data (replaces a database for now) ───────────────────
        private static readonly List<ProposalItem> AllProposals = new List<ProposalItem>
        {
            new ProposalItem
            {
                Title       = "Build a .NET Core E-commerce Website",
                Client      = "John Doe",
                PostedOn    = "20 May 2026",
                Description = "We need a complete e-commerce solution using ASP.NET Core with user authentication, product management, shopping cart, and payment.",
                Price       = "₹12,000",
                Delivery    = "15 Days",
                SubmittedOn = "21 May 2026",
                Status      = "pending",
                IconClass   = "proposal-icon icon-code",
                IconFa      = "fa-solid fa-code",
                BadgeClass  = "status-badge badge-pending",
                BadgeIcon   = "fa-regular fa-clock",
                StatusLabel = "Pending"
            },
            new ProposalItem
            {
                Title       = "Unity Game Development for 2D Adventure Game",
                Client      = "Game Studio",
                PostedOn    = "18 May 2026",
                Description = "Looking for a Unity developer to create a 2D adventure game with multiple levels and characters.",
                Price       = "₹9,000",
                Delivery    = "12 Days",
                SubmittedOn = "19 May 2026",
                Status      = "accepted",
                IconClass   = "proposal-icon icon-game",
                IconFa      = "fa-solid fa-gamepad",
                BadgeClass  = "status-badge badge-accepted",
                BadgeIcon   = "fa-regular fa-circle-check",
                StatusLabel = "Accepted"
            },
            new ProposalItem
            {
                Title       = "Content Writing for College Blog",
                Client      = "College Blog",
                PostedOn    = "12 May 2026",
                Description = "Write informative and engaging blog posts related to college life, education and career tips.",
                Price       = "₹2,500",
                Delivery    = "5 Days",
                SubmittedOn = "13 May 2026",
                Status      = "rejected",
                IconClass   = "proposal-icon icon-writing",
                IconFa      = "fa-solid fa-file-lines",
                BadgeClass  = "status-badge badge-rejected",
                BadgeIcon   = "fa-regular fa-circle-xmark",
                StatusLabel = "Rejected"
            },
            new ProposalItem
            {
                Title       = "Social Media Post Design",
                Client      = "Campus Media",
                PostedOn    = "15 May 2026",
                Description = "Create attractive social media posts and stories for college events, festivals, and student club announcements.",
                Price       = "₹2,200",
                Delivery    = "3 Days",
                SubmittedOn = "16 May 2026",
                Status      = "pending",
                IconClass   = "proposal-icon icon-design",
                IconFa      = "fa-solid fa-bullhorn",
                BadgeClass  = "status-badge badge-pending",
                BadgeIcon   = "fa-regular fa-clock",
                StatusLabel = "Pending"
            },
            new ProposalItem
            {
                Title       = "Student Portal Mobile App UI Design",
                Client      = "EduTech Solutions",
                PostedOn    = "17 May 2026",
                Description = "Design a clean, modern user interface in Figma for a student management and attendance mobile application.",
                Price       = "₹15,000",
                Delivery    = "20 Days",
                SubmittedOn = "18 May 2026",
                Status      = "pending",
                IconClass   = "proposal-icon icon-mobile",
                IconFa      = "fa-solid fa-mobile-screen",
                BadgeClass  = "status-badge badge-pending",
                BadgeIcon   = "fa-regular fa-clock",
                StatusLabel = "Pending"
            },
            new ProposalItem
            {
                Title       = "Database Optimization &amp; SQL Query Tuning",
                Client      = "Apex Data Corp",
                PostedOn    = "14 May 2026",
                Description = "Analyze and optimize SQL Server database stored procedures, index maintenance, and query execution times for high traffic.",
                Price       = "₹10,500",
                Delivery    = "7 Days",
                SubmittedOn = "15 May 2026",
                Status      = "pending",
                IconClass   = "proposal-icon icon-database",
                IconFa      = "fa-solid fa-database",
                BadgeClass  = "status-badge badge-pending",
                BadgeIcon   = "fa-regular fa-clock",
                StatusLabel = "Pending"
            }
        };

        // ── Page Load ─────────────────────────────────────────────────────────────
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Default: show all proposals, highlight "All" tab
                BindProposals("all");
                SetActiveTab("all");
            }
        }

        // ── Tab button click handler ───────────────────────────────────────────────
        protected void FilterTab_Click(object sender, EventArgs e)
        {
            LinkButton btn    = (LinkButton)sender;
            string     filter = btn.CommandArgument;

            BindProposals(filter);
            SetActiveTab(filter);
        }

        // ── Bind the Repeater ─────────────────────────────────────────────────────
        private void BindProposals(string filter)
        {
            List<ProposalItem> filtered = filter == "all"
                ? AllProposals
                : AllProposals.Where(p => p.Status == filter).ToList();

            rptProposals.DataSource = filtered;
            rptProposals.DataBind();

            // Show empty panel when nothing matches
            pnlEmpty.Visible = (filtered.Count == 0);
        }

        // ── Set active CSS class on the correct tab button ────────────────────────
        private void SetActiveTab(string filter)
        {
            btnAll.CssClass      = "tab-btn" + (filter == "all"      ? " active" : "");
            btnPending.CssClass  = "tab-btn" + (filter == "pending"  ? " active" : "");
            btnAccepted.CssClass = "tab-btn" + (filter == "accepted" ? " active" : "");
            btnRejected.CssClass = "tab-btn" + (filter == "rejected" ? " active" : "");
        }
    }
}
