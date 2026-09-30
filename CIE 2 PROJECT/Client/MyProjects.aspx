<%@ Page Title="My Projects" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyProjects.aspx.cs" Inherits="CIE_2_PROJECT.MyProjects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/MyProjects.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="my-projects-page">

        <div class="page-header">

            <div>
                <h1>My Projects</h1>
                <p>Manage all your projects and review proposals.</p>
            </div>

            <a href="CreateProject.aspx" class="post-project-btn">
                <i class="fa-solid fa-plus"></i>
                Post a Project
            </a>

        </div>

        <div class="projects-container">

            <div class="project-tabs">

                <asp:LinkButton
                    ID="lnkAll"
                    runat="server"
                    CssClass="project-tab active"
                    CommandArgument="All"
                    OnClick="Filter_Click">
                    All Projects
                </asp:LinkButton>

                <asp:LinkButton
                    ID="lnkOpen"
                    runat="server"
                    CssClass="project-tab"
                    CommandArgument="Open for Proposals"
                    OnClick="Filter_Click">
                    Open for Proposals
                </asp:LinkButton>

                <asp:LinkButton
                    ID="lnkProgress"
                    runat="server"
                    CssClass="project-tab"
                    CommandArgument="In Progress"
                    OnClick="Filter_Click">
                    In Progress
                </asp:LinkButton>

                <asp:LinkButton
                    ID="lnkCompleted"
                    runat="server"
                    CssClass="project-tab"
                    CommandArgument="Completed"
                    OnClick="Filter_Click">
                    Completed
                </asp:LinkButton>

                <asp:LinkButton
                    ID="lnkCancelled"
                    runat="server"
                    CssClass="project-tab"
                    CommandArgument="Cancelled"
                    OnClick="Filter_Click">
                    Cancelled
                </asp:LinkButton>

            </div>

            <div class="table-header">

                <div class="column-project">PROJECT</div>
                <div class="column-budget">BUDGET</div>
                <div class="column-proposals">PROPOSALS</div>
                <div class="column-deadline">DEADLINE</div>
                <div class="column-status">STATUS</div>
                <div class="column-action">ACTION</div>

            </div>

            <asp:Repeater ID="rptProjects" runat="server">

                <ItemTemplate>

                    <div class="project-row">

                        <div class="project-info">

                            <h2>
                                <%# Eval("Title") %>
                            </h2>

                            <p>
                                <%# Eval("Description") %>
                            </p>

                        </div>

                        <div class="budget">
                            <%# Eval("Budget") %>
                        </div>

                        <div class="proposals">

                            <strong>
                                <%# Eval("Proposals") %>
                            </strong>

                            <span>proposals</span>

                        </div>

                        <div class="deadline">
                            <%# Eval("Deadline") %>
                        </div>

                        <div class="status">

                            <span class='<%# GetStatusClass(Eval("Status").ToString()) %>'>

                                <span class="status-dot"></span>

                                <%# Eval("Status") %>

                            </span>

                        </div>

                        <div class="action">

                            <a
                                href='<%# Eval("Status").ToString() == "In Progress" ? "ReviewSubmittedWork.aspx?projectId=1" : "ProjectDetails.aspx?id=1" %>'
                                class="view-btn">
                                <%# Eval("Status").ToString() == "In Progress" ? "See Progress" : "View Project" %>
                            </a>

                        </div>

                    </div>

                </ItemTemplate>

            </asp:Repeater>

            <asp:Panel
                ID="pnlEmpty"
                runat="server"
                CssClass="empty-projects"
                Visible="false">

                <i class="fa-regular fa-folder-open"></i>

                <h2>No projects found</h2>

                <p>There are no projects in this category.</p>

            </asp:Panel>

        </div>

    </div>

</asp:Content>
