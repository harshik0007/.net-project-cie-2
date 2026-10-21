<%@ Page Title="Find Projects" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="FindProjects.aspx.cs"
    Inherits="CIE_2_PROJECT.FindProjects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/FindProjects.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="projects-page">

        <div class="search-box">

            <i class="fa-solid fa-magnifying-glass search-icon"></i>

            <asp:TextBox
                ID="txtSearch"
                runat="server"
                CssClass="search-input"
                MaxLength="150"
                placeholder="Search projects by title, skill or keyword..." />

            <asp:DropDownList
                ID="ddlCategory"
                runat="server"
                CssClass="category-select"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">

                <asp:ListItem Text="All Categories" Value="" />

            </asp:DropDownList>

            <asp:Button
                ID="btnSearch"
                runat="server"
                Text="Search"
                CssClass="search-button"
                OnClick="btnSearch_Click" />

        </div>

        <asp:Label
            ID="lblPageMessage"
            runat="server"
            CssClass="page-message"
            Visible="false">
        </asp:Label>

        <div class="projects-list">

            <asp:Repeater ID="rptProjects" runat="server">

                <ItemTemplate>

                    <div class="project-card">

                        <div class="project-main">

                            <div class='<%# GetIconContainerClass(Eval("Category")) %>'>
                                <i class='<%# GetCategoryIcon(Eval("Category")) %>'></i>
                            </div>

                            <div class="project-content">

                                <h2 class="project-title">
                                    <%#: Eval("Title") %>
                                </h2>

                                <p class="project-description">
                                    <%#: Eval("Description") %>
                                </p>

                                <div class="project-tags">

                                    <span class='project-tag <%# GetTagClass(Eval("Category")) %>'>
                                        <%#: Eval("Category") %>
                                    </span>

                                    <span class="project-tag blue">
                                        <%#: Eval("ExperienceLevel") %>
                                    </span>

                                    <asp:PlaceHolder
                                        runat="server"
                                        Visible='<%# HasText(Eval("RequiredSkills")) %>'>

                                        <span class="project-tag green">
                                            Skills: <%#: Eval("RequiredSkills") %>
                                        </span>

                                    </asp:PlaceHolder>

                                </div>

                                <div class="posted-info">
                                    <i class="fa-regular fa-clock"></i>
                                    <span>
                                        <%#: GetPostedText(Eval("CreatedAt")) %>
                                    </span>
                                </div>

                                <asp:HyperLink
                                    ID="lnkAttachment"
                                    runat="server"
                                    CssClass="attachment-link"
                                    Text="View project attachment"
                                    Target="_blank"
                                    NavigateUrl='<%# ResolveAttachmentUrl(Eval("AttachmentPath")) %>'
                                    Visible='<%# HasAttachment(Eval("AttachmentPath")) %>' />

                            </div>

                        </div>

                        <div class="project-action">

                            <div class="project-price">
                                <%#: FormatBudget(Eval("MinBudget"), Eval("MaxBudget")) %>
                            </div>

                            <div class="price-type">
                                Budget Range
                            </div>

                            <div class="deadline">
                                <i class="fa-regular fa-calendar"></i>
                                <span>
                                    <%#: FormatDeadline(Eval("Deadline")) %>
                                </span>
                            </div>

                            <div class="posted-info">
                                <i class="fa-solid fa-users"></i>
                                <span>
                                    <%#: GetProposalText(Eval("ProposalCount")) %>
                                </span>
                            </div>

                            <a
                                href='<%# "ProjectDetails.aspx?id=" + Eval("Id") %>'
                                class="view-project">
                                View Project
                            </a>

                        </div>

                    </div>

                </ItemTemplate>

            </asp:Repeater>

        </div>

        <asp:Panel
            ID="pnlEmpty"
            runat="server"
            CssClass="no-projects"
            Visible="false">

            <i class="fa-regular fa-folder-open"></i>

            <h2>No projects found</h2>

            <p>
                No open projects match your search or category.
                Try a different keyword or select All Categories.
            </p>

        </asp:Panel>

    </div>

</asp:Content>