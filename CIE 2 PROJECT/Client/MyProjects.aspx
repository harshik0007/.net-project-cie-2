
<%@ Page Title="My Projects" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyProjects.aspx.cs" Inherits="CIE_2_PROJECT.MyProjects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/MyProjects.css?v=7" />
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

            <!-- Displays a database loading error when necessary -->
            <asp:Label
                ID="lblLoadError"
                runat="server"
                CssClass="form-message"
                Visible="false"
                EnableViewState="false">
            </asp:Label>

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

                            <h2 title='<%#: Eval("Title") %>'>
                                <%#: Eval("Title") %>
                            </h2>

                            <p title='<%#: Eval("Description") %>'>
                                <%#: Eval("Description") %>
                            </p>

                            <asp:HyperLink ID="lnkMessageFreelancer" runat="server"
                                CssClass="project-message-link"
                                NavigateUrl='<%# Eval("MessageUrl") %>'
                                Visible='<%# Convert.ToBoolean(Eval("ShowMessageLink")) %>'>
                                <i class="fa-regular fa-comments"></i> Message freelancer
                            </asp:HyperLink>

                        </div>

                        <div class="budget">
                            <%#: Eval("Budget") %>
                        </div>

                        <div class="proposals">

                            <strong>
                                <%#: Eval("Proposals") %>
                            </strong>

                            <span>proposals</span>

                        </div>

                        <div class="deadline">
                            <%#: Eval("Deadline") %>
                        </div>

                        <div class="status">

                            <span class='<%# GetStatusClass(Eval("Status").ToString()) %>' title='<%#: Eval("Status") %>'>

                                <span class="status-dot"></span>

                                <%#: Eval("StatusLabel") %>

                            </span>

                        </div>

                        <div class="action">
                            <a
                                href='<%# Eval("ActionUrl") %>'
                                class="view-btn">
                                <%#: Eval("ActionText") %>
                            </a>
                            <asp:Panel ID="pnlReviewStatus" runat="server"
                                CssClass="review-status-wrap"
                                Visible='<%# Convert.ToBoolean(Eval("ShowReviewStatus")) %>'>
                                <span class='<%# Eval("ReviewStatusClass") %>'>
                                    <i class='<%# Eval("ReviewStatusIcon") %>'></i>
                                    <%#: Eval("ReviewStatus") %>
                                </span>
                            </asp:Panel>
                            <asp:HyperLink ID="lnkEditProject" runat="server" CssClass="project-action-link project-edit-link"
                                NavigateUrl='<%# "CreateProject.aspx?id=" + Eval("Id") %>'
                                Text="Edit" Visible='<%# Convert.ToBoolean(Eval("CanEdit")) %>' />
                            <asp:LinkButton ID="btnCancelProject" runat="server"
                                Text="Cancel project" CssClass="project-action-link project-cancel-link" CommandArgument='<%# Eval("Id") %>'
                                Visible='<%# Convert.ToBoolean(Eval("CanEdit")) %>'
                                OnClick="CancelProject_Click" CausesValidation="false" />
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
