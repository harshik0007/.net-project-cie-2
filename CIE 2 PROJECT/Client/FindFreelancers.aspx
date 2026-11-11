<%@ Page Title="Find Freelancers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FindFreelancers.aspx.cs" Inherits="CIE_2_PROJECT.FindFreelancers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/FindFreelancers.css?v=2" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="find-page">

        <div class="page-header">
            <div>
                <h1>Find Freelancers</h1>
                <p>Find the right talent for your project.</p>
            </div>
        </div>


        <!-- SEARCH -->

        <div class="search-section">

            <div class="search-box">

                <i class="fa-solid fa-magnifying-glass"></i>

                <asp:TextBox
                    ID="txtSearch"
                    runat="server"
                    CssClass="search-input"
                    placeholder="Search by name, skill or keyword">
                </asp:TextBox>

            </div>

            <asp:Button
                ID="btnSearch"
                runat="server"
                Text="Search"
                CssClass="search-button"
                OnClick="btnSearch_Click" />

        </div>


        <!-- FILTERS -->

        <div class="filter-section">

            <div class="filter-item">

                <label>Skill</label>

                <asp:DropDownList
                    ID="ddlSkill"
                    runat="server"
                    CssClass="filter-select" />

            </div>


            <div class="filter-item">

                <label>Category</label>

                <asp:DropDownList
                    ID="ddlCategory"
                    runat="server"
                    CssClass="filter-select" />

            </div>


            <asp:Button
                ID="btnClear"
                runat="server"
                Text="Clear Filters"
                CssClass="clear-button"
                OnClick="btnClear_Click" />

        </div>


        <!-- RESULT HEADER -->

        <div class="result-header">

            <div>
                <h2>Freelancers</h2>

                <asp:Label
                    ID="lblResultCount"
                    runat="server"
                    CssClass="result-count">
                    6 freelancers found
                </asp:Label>
            </div>

        </div>


        <!-- FREELANCER LIST -->

        <asp:Panel ID="pnlNoFreelancers" runat="server" CssClass="freelancer-empty-state" Visible="false">
            <i class="fa-regular fa-user"></i>
            <h3>No freelancers match these filters</h3>
            <p>Try a different skill or category, or clear your filters.</p>
        </asp:Panel>

        <asp:Repeater ID="rptFreelancers" runat="server">

            <ItemTemplate>

                <div class="freelancer-row">

                    <div class="freelancer-avatar">
                        <%# Eval("Initials") %>
                    </div>


                    <div class="freelancer-main">

                        <div class="freelancer-title-row">

                            <div>

                        <h3><a href='<%#: Eval("ProfileUrl") %>'><%#: Eval("Name") %></a></h3>

                                <p class="freelancer-title">
                                    <%#: Eval("Title") %>
                                </p>
                                <p class="freelancer-location"><%#: Eval("Location") %></p>

                            </div>

                            <div class="rating">

                                <i class="fa-solid fa-star"></i>

                                <strong>
                                <%#: Eval("Rating") %>
                                </strong>

                                <span>
                                    (<%#: Eval("Reviews") %> reviews)
                                </span>

                            </div>

                        </div>


                        <div class="skills">

                            <%# Eval("SkillsMarkup") %>

                        </div>


                        <p class="freelancer-bio">
                            <%#: Eval("Bio") %>
                        </p>

                    </div>


                    <div class="freelancer-actions">

                        <a
                            href='<%#: Eval("ProfileUrl") %>'
                            class="profile-button">

                            View Profile

                        </a>

                        <a
                            href="../Messages/Messages.aspx"
                            class="contact-button">

                            <i class="fa-regular fa-message"></i>

                            Contact

                        </a>

                    </div>

                </div>

            </ItemTemplate>

        </asp:Repeater>


    </div>

</asp:Content>
