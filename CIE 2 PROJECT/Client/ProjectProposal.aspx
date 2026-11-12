
<%@ Page Title="Project Proposals" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="ProjectProposal.aspx.cs"
    Inherits="CIE_2_PROJECT.ProjectProposal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/ProjectProposal.css?v=5" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="proposal-page">

        <asp:HyperLink ID="lnkBack" runat="server"
            CssClass="back-link">
            <i class="fa-solid fa-arrow-left"></i>
            Back to My Projects
        </asp:HyperLink>

        <asp:Label ID="lblPageMessage" runat="server"
            CssClass="page-message" Visible="false" />

        <!-- Project summary -->
        <asp:Panel ID="pnlProjectSummary" runat="server">

            <div class="project-summary">

                <div class="project-summary-main">

                    <h1>
                        <asp:Literal ID="litProjectTitle" runat="server" />
                    </h1>

                    <p>
                        <asp:Literal ID="litProjectDescription" runat="server" />
                    </p>

                    <div class="skills">
                        <asp:Literal ID="litProjectSkills" runat="server" />
                    </div>

                </div>

                <div class="project-summary-side">

                    <div class="summary-item">
                        <span>Budget</span>
                        <strong>
                            <asp:Literal ID="litProjectBudget" runat="server" />
                        </strong>
                    </div>

                    <div class="summary-item">
                        <span>Deadline</span>
                        <strong>
                            <asp:Literal ID="litProjectDeadline" runat="server" />
                        </strong>
                    </div>

                    <div class="summary-item">
                        <span>Project Status</span>
                        <strong>
                            <asp:Literal ID="litProjectStatus" runat="server" />
                        </strong>
                    </div>

                </div>

            </div>

        </asp:Panel>

        <!-- Proposal heading -->
        <div class="proposal-heading">

            <div>
                <h2>
                    <asp:Label ID="lblProposalHeading" runat="server" Text="Proposals" />
                    (<asp:Label ID="lblProposalCount" runat="server" Text="0" />)
                </h2>

                <asp:Label ID="lblProposalGuidance" runat="server"
                    CssClass="proposal-guidance"
                    Text="Review proposals and select the freelancer who best fits your project." />
            </div>

            <div class="sort-container">

                <span>Sort by:</span>

                <asp:DropDownList ID="ddlSort" runat="server"
                    CssClass="sort-dropdown"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">

                    <asp:ListItem Text="Newest First" Value="Newest" />
                    <asp:ListItem Text="Oldest First" Value="Oldest" />
                    <asp:ListItem Text="Price: Low to High" Value="PriceLow" />
                    <asp:ListItem Text="Price: High to Low" Value="PriceHigh" />
                    <asp:ListItem Text="Delivery Time" Value="Delivery" />

                </asp:DropDownList>

            </div>

        </div>

        <!-- No proposals -->
        <asp:Panel ID="pnlNoProposals" runat="server"
            CssClass="empty-state" Visible="false">

            <i class="fa-regular fa-folder-open"></i>

            <h2>No Proposals Yet</h2>

            <p>
                Freelancer proposals will appear here when someone applies to your project.
            </p>

        </asp:Panel>

        <!-- Proposals -->
        <div class="proposals-list">

        <asp:Repeater ID="rptProposals" runat="server" OnItemCommand="rptProposals_ItemCommand">

                <ItemTemplate>

                    <div class="proposal-card">

                        <div class="freelancer-profile">

                            <div class="freelancer-avatar">
                                <i class="fa-solid fa-user"></i>
                            </div>

                            <div class="freelancer-info">

                                <h3><a href='<%#: Eval("FreelancerProfileUrl") %>'><%#: Eval("FreelancerName") %></a></h3>

                                <span class="freelancer-title">
                                    Freelancer
                                </span>

                                <div class="location">
                                    <i class="fa-regular fa-calendar"></i>
                                    Submitted:
                                    <%#: Eval("SubmittedOn") %>
                                </div>

                            </div>

                        </div>

                        <div class="proposal-content">

                            <h4>Cover Letter</h4>

                            <p title='<%#: Eval("CoverLetter") %>'>
                                <%#: Eval("CoverLetter") %>
                            </p>

                            <div class="proposal-skills">
                                <%# GetSkills(Convert.ToString(Eval("Skills"))) %>
                            </div>

                        </div>

                        <div class="proposal-offer">

                            <div class="offer-item">
                                <span>Proposed Price</span>
                                <strong>
                                    <%#: Eval("ProposedPrice") %>
                                </strong>
                            </div>

                            <div class="offer-item">
                                <span>Estimated client total (18% demo GST + 5% fee)</span>
                                <strong>
                                    <%#: Eval("EstimatedClientTotal") %>
                                </strong>
                            </div>

                            <div class="offer-item">
                                <span>Delivery Time</span>
                                <strong>
                                    <%#: Eval("DeliveryTime") %> days
                                </strong>
                            </div>

                            <div class="offer-item">
                                <span>Proposal Status</span>
                                <strong>
                                    <%#: Eval("Status") %>
                                </strong>
                            </div>

                            <div class="proposal-actions">

                                <asp:Panel ID="pnlAcceptAction" runat="server"
                                    Visible='<%# Convert.ToBoolean(Eval("CanHire")) %>'>

                                    <asp:LinkButton ID="btnHire" runat="server"
                                        CssClass="hire-btn"
                                        CommandName="Hire"
                                        CommandArgument='<%# Eval("ProposalID") %>'
                                        CausesValidation="false">

                                        <i class="fa-solid fa-check"></i>
                                        Hire Freelancer
                                    </asp:LinkButton>

                                </asp:Panel>

                                <asp:HyperLink ID="lnkMessageFreelancer" runat="server"
                                    CssClass="message-freelancer-btn"
                                    NavigateUrl='<%# Eval("MessageUrl") %>'>
                                    <i class="fa-regular fa-comments"></i> Message freelancer
                                </asp:HyperLink>

                            </div>

                        </div>

                    </div>

                </ItemTemplate>

            </asp:Repeater>

        </div>

    </div>

</asp:Content>
