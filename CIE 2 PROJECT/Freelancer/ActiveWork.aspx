
<%@ Page Title="Active Work" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="ActiveWork.aspx.cs"
    Inherits="CIE_2_PROJECT.ActiveWork" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/ActiveWork.css?v=3" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="active-work-page">

        <!-- Page Header -->
        <div class="page-header">
            <h1 class="page-title">Active Work</h1>
            <p class="page-subtitle">
                Projects you are working on. Track progress and manage your active jobs.
            </p>
            <p class="active-work-shortcuts">
                <a href="WorkHistory.aspx">Payment history</a>
            </p>
        </div>

        <asp:Label ID="lblPageMessage" runat="server"
            CssClass="page-message" Visible="false" />

        <!-- Summary -->
        <div class="work-summary">

            <div class="summary-item">
                <div class="summary-icon summary-blue">
                    <i class="fa-solid fa-briefcase"></i>
                </div>

                <div class="summary-content">
                    <span class="summary-label">Total Active Projects</span>

                    <asp:Label ID="lblTotalActive" runat="server"
                        CssClass="summary-value" Text="0" />
                </div>
            </div>

            <div class="summary-item">
                <div class="summary-icon summary-green">
                    <i class="fa-solid fa-list-check"></i>
                </div>

                <div class="summary-content">
                    <span class="summary-label">In Progress</span>

                    <asp:Label ID="lblInProgress" runat="server"
                        CssClass="summary-value" Text="0" />
                </div>
            </div>

            <div class="summary-item">
                <div class="summary-icon summary-purple">
                    <i class="fa-regular fa-clock"></i>
                </div>

                <div class="summary-content">
                    <span class="summary-label">Pending Review</span>

                    <asp:Label ID="lblPendingReview" runat="server"
                        CssClass="summary-value" Text="&#8212;" />
                </div>
            </div>

            <div class="summary-item summary-last">
                <div class="summary-icon summary-red">
                    <i class="fa-solid fa-indian-rupee-sign"></i>
                </div>

                <div class="summary-content">
                    <span class="summary-label">Total Expected Payout</span>

                    <asp:Label ID="lblTotalEarnings" runat="server"
                        CssClass="summary-value" Text="&#8377;0" />
                </div>
            </div>

        </div>

        <!-- Empty State -->
        <asp:Panel ID="pnlEmpty" runat="server"
            CssClass="empty-state" Visible="false">

            <div class="empty-icon">
                <i class="fa-regular fa-folder-open"></i>
            </div>

            <h2>No Active Projects Yet</h2>

            <p>
                Projects with accepted proposals will appear here.
                Find a project and submit a proposal to get started.
            </p>

            <a href="FindProjects.aspx" class="empty-action">
                <i class="fa-solid fa-magnifying-glass"></i>
                Find Projects
            </a>

        </asp:Panel>

        <!-- Active Projects -->
        <asp:Repeater ID="rptActiveWork" runat="server">

            <HeaderTemplate>
                <div class="active-projects">
            </HeaderTemplate>

            <ItemTemplate>

                <div class="active-project-card">

                    <!-- Project Information -->
                    <div class="project-main">

                        <div class='project-icon <%#: Eval("IconClass") %>'>
                            <i class='<%#: Eval("IconFa") %>'></i>
                        </div>

                        <div class="project-content">

                            <h2 class="project-title">
                                <%#: Eval("Title") %>
                            </h2>

                            <div class="project-meta">

                                <span>
                                    <i class="fa-regular fa-user"></i>
                                    Client:

                                    <a class="client-link" href='<%#: Eval("ClientProfileUrl") %>'><%#: Eval("Client") %></a>
                                </span>

                                <span class="meta-dot">&#8226;</span>

                                <span>
                                    <i class="fa-regular fa-calendar"></i>
                                    Started on <%#: Eval("StartedOn") %>
                                </span>

                            </div>

                            <p class="project-description">
                                <%#: Eval("Description") %>
                            </p>

                            <div class="project-skills">

                                <span class="skill-label">Category</span>

                                <span class="category-tag">
                                    <%#: Eval("Category") %>
                                </span>

                            </div>

                        </div>

                    </div>

                    <!-- Project Details -->
                    <div class="project-side">

                        <div class="side-top">

                            <div class="side-info">
                                <span class="side-label">Expected Payout After Fee</span>

                                <span class="side-price">
                                    <%#: Eval("AgreedPrice") %>
                                </span>
                            </div>

                            <span class='status-badge <%#: Eval("BadgeClass") %>'>
                                <i class='<%#: Eval("BadgeIcon") %>'></i>
                                <%#: Eval("StatusLabel") %>
                            </span>

                        </div>

                        <div class="deadline-section">
                            <span class="side-label">Project Deadline</span>

                            <strong>
                                <%#: Eval("Deadline") %>
                            </strong>
                        </div>

                        <div class="deadline-section">
                            <span class="side-label">Payment Status</span>
                            <strong><%#: Eval("PaymentStatus") %></strong>
                        </div>

                        <div class="progress-heading">
                            <span class="side-label">Work Progress</span>

                            <span class="progress-value">
                                <%#: Eval("ProgressLabel") %>
                            </span>
                        </div>

                        <div class="progress-track">
                            <div class="progress-fill progress-untracked"></div>
                        </div>

                        <asp:HyperLink ID="lnkOpenProject" runat="server"
                            CssClass="open-project-button"
                            Text="Continue Work"
                            NavigateUrl='<%# "SubmitWork.aspx?id=" + Eval("ContractID") %>' />

                    </div>

                </div>

            </ItemTemplate>

            <FooterTemplate>
                </div>
            </FooterTemplate>

        </asp:Repeater>

        <!-- Information -->
        <div class="work-note">
            <i class="fa-regular fa-circle-info"></i>

            <span>
                Keep your clients updated and deliver your work on time
                to build a strong reputation.
            </span>
        </div>

    </div>

</asp:Content>
