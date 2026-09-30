<%@ Page Title="Project Proposals" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProjectProposal.aspx.cs" Inherits="CIE_2_PROJECT.ProjectProposal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/ProjectProposal.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="proposal-page">

        <a href="MyProjects.aspx" class="back-link">
            <i class="fa-solid fa-arrow-left"></i>
            Back to My Projects
        </a>

        <div class="project-summary">

            <div class="project-summary-main">

                <h1>Unity 2D Game Development</h1>

                <p>
                    Create a 2D shooting game with multiple levels, enemies,
                    in-game store and leaderboard system.
                </p>

                <div class="skills">

                    <span>Unity</span>
                    <span>C#</span>
                    <span>Game Design</span>

                </div>

            </div>

            <div class="project-summary-side">

                <div class="summary-item">

                    <span>Budget</span>

                    <strong>
                        ₹15,000 - ₹25,000
                    </strong>

                </div>

                <div class="summary-item">

                    <span>Deadline</span>

                    <strong>
                        30 May, 2026
                    </strong>

                </div>

            </div>

        </div>

        <div class="proposal-heading">

            <div>

                <h2>
                    Proposals (<asp:Label ID="lblProposalCount" runat="server"></asp:Label>)
                </h2>

                <p>
                    Review proposals and hire the best freelancer for your project.
                </p>

            </div>

            <div class="sort-container">

                <span>Sort by:</span>

                <asp:DropDownList
                    ID="ddlSort"
                    runat="server"
                    CssClass="sort-dropdown"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">

                    <asp:ListItem Text="Recommended" Value="Recommended"></asp:ListItem>
                    <asp:ListItem Text="Price: Low to High" Value="PriceLow"></asp:ListItem>
                    <asp:ListItem Text="Price: High to Low" Value="PriceHigh"></asp:ListItem>
                    <asp:ListItem Text="Delivery Time" Value="Delivery"></asp:ListItem>
                    <asp:ListItem Text="Rating" Value="Rating"></asp:ListItem>

                </asp:DropDownList>

            </div>

        </div>

        <div class="proposals-list">

            <asp:Repeater ID="rptProposals" runat="server">

                <ItemTemplate>

                    <div class="proposal-card">

                        <div class="freelancer-profile">

                            <div class="freelancer-avatar">

                                <i class="fa-solid fa-user"></i>

                                <span class="online-dot"></span>

                            </div>

                            <div class="freelancer-info">

                                <h3>
                                    <%# Eval("Name") %>
                                </h3>

                                <span class="freelancer-title">
                                    <%# Eval("Title") %>
                                </span>

                                <div class="rating">

                                    <i class="fa-solid fa-star"></i>

                                    <strong>
                                        <%# Eval("Rating") %>
                                    </strong>

                                    <span>
                                        (<%# Eval("Reviews") %> reviews)
                                    </span>

                                </div>

                                <div class="location">

                                    <i class="fa-solid fa-location-dot"></i>

                                    <%# Eval("Location") %>

                                </div>

                            </div>

                        </div>

                        <div class="proposal-content">

                            <p>
                                <%# Eval("CoverLetter") %>
                            </p>

                            <div class="proposal-skills">

                                <%# GetSkills(Eval("Skills").ToString()) %>

                            </div>

                        </div>

                        <div class="proposal-offer">

                            <div class="offer-item">

                                <span>Proposed Price</span>

                                <strong>
                                    <%# Eval("ProposedPrice") %>
                                </strong>

                            </div>

                            <div class="offer-item">

                                <span>Delivery Time</span>

                                <strong>
                                    <%# Eval("DeliveryTime") %> days
                                </strong>

                            </div>

                            <div class="proposal-actions">

                                <a
                                    href='<%# "Profile.aspx?id=" + Eval("FreelancerId") %>'
                                    class="view-profile-btn">
                                    View Profile
                                </a>

                                <asp:LinkButton
                                    ID="btnHire"
                                    runat="server"
                                    CssClass="hire-btn"
                                    CommandArgument='<%# Eval("Id") %>'
                                    OnClick="HireFreelancer_Click">
                                    Hire Freelancer
                                </asp:LinkButton>

                            </div>

                        </div>

                    </div>

                </ItemTemplate>

            </asp:Repeater>

        </div>

    </div>

</asp:Content>