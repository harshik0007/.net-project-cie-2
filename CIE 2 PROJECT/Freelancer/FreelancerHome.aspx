<%@ Page Title="Freelancer Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FreelancerHome.aspx.cs" Inherits="CIE_2_PROJECT.FreelancerHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/DashboardHome.css?v=2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dashboard-home freelancer-dashboard">
        <asp:Panel ID="pnlDashboardError" runat="server" CssClass="dashboard-alert" Visible="false">
            <asp:Literal ID="litDashboardError" runat="server" Mode="Encode" />
        </asp:Panel>

        <section class="dashboard-hero freelancer-hero">
            <div class="dashboard-hero-copy">
                <span class="dashboard-eyebrow">Freelancer workspace</span>
                <p class="dashboard-welcome">Welcome back, <strong><asp:Literal ID="litWelcomeName" runat="server" Mode="Encode" /></strong></p>
                <h1>Find your next great project.</h1>
                <p>Explore live opportunities, keep work moving, and track your marketplace progress from one place.</p>
                <div class="dashboard-actions">
                    <a href="FindProjects.aspx" class="dashboard-primary-action"><i class="fa-solid fa-magnifying-glass"></i> Find Projects</a>
                    <a href="MyProposals.aspx" class="dashboard-secondary-action">My Proposals</a>
                </div>
            </div>
            <div class="dashboard-hero-art" aria-hidden="true">
                <div class="dashboard-art-window"><i class="fa-solid fa-code"></i><span></span><span></span><span></span></div>
                <div class="dashboard-art-check"><i class="fa-solid fa-check"></i></div>
            </div>
        </section>

        <section class="dashboard-metrics" aria-label="Freelancer activity">
            <a class="dashboard-metric" href="ActiveWork.aspx"><span class="metric-icon metric-blue"><i class="fa-solid fa-briefcase"></i></span><span><small>Active contracts</small><strong><asp:Literal ID="litActiveContracts" runat="server" Mode="Encode" /></strong></span></a>
            <a class="dashboard-metric" href="MyProposals.aspx?filter=pending"><span class="metric-icon metric-amber"><i class="fa-regular fa-clock"></i></span><span><small>Pending proposals</small><strong><asp:Literal ID="litPendingProposals" runat="server" Mode="Encode" /></strong></span></a>
            <a class="dashboard-metric" href="MyProposals.aspx?filter=completed"><span class="metric-icon metric-green"><i class="fa-regular fa-circle-check"></i></span><span><small>Completed projects</small><strong><asp:Literal ID="litCompletedProjects" runat="server" Mode="Encode" /></strong></span></a>
            <a class="dashboard-metric" href="WorkHistory.aspx"><span class="metric-icon metric-purple"><i class="fa-solid fa-wallet"></i></span><span><small>Payments released</small><strong><asp:Literal ID="litReleasedPayments" runat="server" Mode="Encode" /></strong></span></a>
        </section>

        <section class="dashboard-section">
            <div class="dashboard-section-heading">
                <div><span class="dashboard-eyebrow">Picked for you</span><h2>Recommended projects</h2></div>
                <a href="FindProjects.aspx" class="dashboard-text-link">Browse all <i class="fa-solid fa-arrow-right"></i></a>
            </div>
            <asp:Repeater ID="rptRecommendedProjects" runat="server">
                <HeaderTemplate><div class="dashboard-project-grid"></HeaderTemplate>
                <ItemTemplate>
                    <article class="dashboard-project-card">
                        <div class="dashboard-project-top"><span class="dashboard-category"><%#: Eval("Category") %></span><span class="dashboard-deadline"><i class="fa-regular fa-calendar"></i> <%#: Eval("Deadline") %></span></div>
                        <h3><a href='<%#: Eval("ProjectUrl") %>'><%#: Eval("Title") %></a></h3>
                        <p class="dashboard-project-description"><%#: Eval("Description") %></p>
                        <div class="dashboard-project-footer"><strong><%#: Eval("Budget") %></strong><a href='<%#: Eval("ProjectUrl") %>' aria-label="View project"><i class="fa-solid fa-arrow-up-right-from-square"></i></a></div>
                    </article>
                </ItemTemplate>
                <FooterTemplate></div></FooterTemplate>
            </asp:Repeater>
            <asp:Panel ID="pnlNoRecommendedProjects" runat="server" CssClass="dashboard-empty" Visible="false">
                <i class="fa-regular fa-folder-open"></i><strong>No open projects to show yet</strong><span>New opportunities will appear here as clients post them.</span>
            </asp:Panel>
        </section>

        <section class="dashboard-profile-progress">
            <div class="dashboard-progress-icon"><i class="fa-regular fa-user"></i></div>
            <div class="dashboard-progress-copy"><div class="dashboard-progress-title"><strong>Profile completeness</strong><span><asp:Literal ID="litProfileCompletion" runat="server" Mode="Encode" />%</span></div><div class="dashboard-progress-track"><asp:Panel ID="pnlProfileCompletionBar" runat="server" CssClass="dashboard-progress-fill" /></div><p>A complete profile helps clients understand your skills and experience.</p></div>
            <a href="Profile.aspx" class="dashboard-secondary-action">Review profile</a>
        </section>
    </div>
</asp:Content>
