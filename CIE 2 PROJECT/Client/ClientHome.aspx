<%@ Page Title="Client Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientHome.aspx.cs" Inherits="CIE_2_PROJECT.ClientHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/DashboardHome.css?v=2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dashboard-home client-dashboard">
        <asp:Panel ID="pnlDashboardError" runat="server" CssClass="dashboard-alert" Visible="false">
            <asp:Literal ID="litDashboardError" runat="server" Mode="Encode" />
        </asp:Panel>

        <section class="dashboard-hero client-hero">
            <div class="dashboard-hero-copy">
                <span class="dashboard-eyebrow">Client workspace</span>
                <p class="dashboard-welcome">Welcome back, <strong><asp:Literal ID="litWelcomeName" runat="server" Mode="Encode" /></strong></p>
                <h1>Bring your next idea to life.</h1>
                <p>Post a project, connect with skilled freelancers, and keep every contract moving.</p>
                <div class="dashboard-actions">
                    <a href="CreateProject.aspx" class="dashboard-primary-action"><i class="fa-solid fa-plus"></i> Post a Project</a>
                    <a href="FindFreelancers.aspx" class="dashboard-secondary-action"><i class="fa-solid fa-magnifying-glass"></i> Find Freelancers</a>
                </div>
            </div>
            <div class="dashboard-hero-art client-art" aria-hidden="true">
                <div class="dashboard-art-window"><i class="fa-solid fa-clipboard-check"></i><span></span><span></span><span></span></div>
                <div class="dashboard-art-check"><i class="fa-solid fa-check"></i></div>
            </div>
        </section>

        <section class="dashboard-metrics" aria-label="Client project activity">
            <a class="dashboard-metric" href="MyProjects.aspx"><span class="metric-icon metric-blue"><i class="fa-solid fa-folder-open"></i></span><span><small>Projects posted</small><strong><asp:Literal ID="litProjectsPosted" runat="server" Mode="Encode" /></strong></span></a>
            <a class="dashboard-metric" href="MyProjects.aspx"><span class="metric-icon metric-amber"><i class="fa-regular fa-lightbulb"></i></span><span><small>Open for proposals</small><strong><asp:Literal ID="litOpenProjects" runat="server" Mode="Encode" /></strong></span></a>
            <a class="dashboard-metric" href="MyProjects.aspx"><span class="metric-icon metric-green"><i class="fa-solid fa-briefcase"></i></span><span><small>Active contracts</small><strong><asp:Literal ID="litActiveProjects" runat="server" Mode="Encode" /></strong></span></a>
            <a class="dashboard-metric" href="Payments.aspx"><span class="metric-icon metric-purple"><i class="fa-solid fa-wallet"></i></span><span><small>Payments released</small><strong><asp:Literal ID="litReleasedPayments" runat="server" Mode="Encode" /></strong></span></a>
        </section>

        <section class="dashboard-section">
            <div class="dashboard-section-heading">
                <div><span class="dashboard-eyebrow">Your workspace</span><h2>Recent projects</h2></div>
                <a href="MyProjects.aspx" class="dashboard-text-link">All projects <i class="fa-solid fa-arrow-right"></i></a>
            </div>
            <asp:Repeater ID="rptRecentProjects" runat="server">
                <HeaderTemplate><div class="dashboard-project-list"></HeaderTemplate>
                <ItemTemplate>
                    <article class="dashboard-project-row">
                        <div class="dashboard-project-symbol"><i class="fa-solid fa-briefcase"></i></div>
                        <div class="dashboard-project-row-main">
                            <h3><a href='<%#: Eval("ProjectUrl") %>'><%#: Eval("Title") %></a></h3>
                            <div class="dashboard-project-meta"><span><%#: Eval("Category") %></span><span><%#: Eval("ProposalCount") %> proposals</span></div>
                        </div>
                        <div class="dashboard-project-budget"><span>Budget</span><strong><%#: Eval("Budget") %></strong></div>
                        <span class='dashboard-project-status <%#: Eval("StatusClass") %>'><%#: Eval("Status") %></span>
                        <a href='<%#: Eval("ProjectUrl") %>' class="dashboard-row-action">View</a>
                    </article>
                </ItemTemplate>
                <FooterTemplate></div></FooterTemplate>
            </asp:Repeater>
            <asp:Panel ID="pnlNoProjects" runat="server" CssClass="dashboard-empty" Visible="false">
                <i class="fa-regular fa-folder-open"></i><strong>No projects posted yet</strong><span>Create a project to start finding the right freelancer.</span><a href="CreateProject.aspx" class="dashboard-primary-action">Post a Project</a>
            </asp:Panel>
        </section>

        <section class="dashboard-section dashboard-talent-section">
            <div class="dashboard-section-heading">
                <div><span class="dashboard-eyebrow">Meet the talent</span><h2>Freelancers to explore</h2></div>
                <a href="FindFreelancers.aspx" class="dashboard-text-link">Browse freelancers <i class="fa-solid fa-arrow-right"></i></a>
            </div>
            <asp:Repeater ID="rptFreelancers" runat="server">
                <HeaderTemplate><div class="dashboard-talent-grid"></HeaderTemplate>
                <ItemTemplate>
                    <article class="dashboard-talent-card">
                        <div class="dashboard-talent-avatar"><%#: Eval("Initials") %></div>
                        <h3><a href='<%#: Eval("ProfileUrl") %>'><%#: Eval("Name") %></a></h3>
                        <div class="dashboard-talent-rating"><i class="fa-solid fa-star"></i> <%#: Eval("Rating") %> <span>(<%#: Eval("ReviewCount") %> reviews)</span></div>
                        <p><%#: Eval("Bio") %></p>
                        <div class="dashboard-talent-skills"><%#: Eval("Skills") %></div>
                        <a href='<%#: Eval("ProfileUrl") %>' class="dashboard-secondary-action">View Profile</a>
                    </article>
                </ItemTemplate>
                <FooterTemplate></div></FooterTemplate>
            </asp:Repeater>
            <asp:Panel ID="pnlNoFreelancers" runat="server" CssClass="dashboard-empty" Visible="false">
                <i class="fa-regular fa-user"></i><strong>No freelancer profiles yet</strong><span>Freelancer profiles will appear here when they join SkillLink.</span>
            </asp:Panel>
        </section>
    </div>
</asp:Content>
