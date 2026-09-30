<%@ Page Title="Find Freelancers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FindFreelancers.aspx.cs" Inherits="CIE_2_PROJECT.FindFreelancers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/FindFreelancers.css" />
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
                    CssClass="filter-select">

                    <asp:ListItem Text="All Skills" Value="" />
                    <asp:ListItem Text="React.js" Value="React.js" />
                    <asp:ListItem Text="Node.js" Value="Node.js" />
                    <asp:ListItem Text="MongoDB" Value="MongoDB" />
                    <asp:ListItem Text="Figma" Value="Figma" />
                    <asp:ListItem Text="UI/UX Design" Value="UI/UX Design" />
                    <asp:ListItem Text="WordPress" Value="WordPress" />
                    <asp:ListItem Text="PHP" Value="PHP" />
                    <asp:ListItem Text="Graphic Design" Value="Graphic Design" />

                </asp:DropDownList>

            </div>


            <div class="filter-item">

                <label>Category</label>

                <asp:DropDownList
                    ID="ddlCategory"
                    runat="server"
                    CssClass="filter-select">

                    <asp:ListItem Text="All Categories" Value="" />
                    <asp:ListItem Text="Web Development" Value="Web Development" />
                    <asp:ListItem Text="UI/UX Design" Value="UI/UX Design" />
                    <asp:ListItem Text="Graphic Design" Value="Graphic Design" />
                    <asp:ListItem Text="Mobile Development" Value="Mobile Development" />
                    <asp:ListItem Text="WordPress" Value="WordPress" />

                </asp:DropDownList>

            </div>


            <div class="filter-item">

                <label>Experience</label>

                <asp:DropDownList
                    ID="ddlExperience"
                    runat="server"
                    CssClass="filter-select">

                    <asp:ListItem Text="All Experience" Value="" />
                    <asp:ListItem Text="0 - 2 Years" Value="0-2" />
                    <asp:ListItem Text="2 - 5 Years" Value="2-5" />
                    <asp:ListItem Text="5+ Years" Value="5+" />

                </asp:DropDownList>

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

        <asp:Repeater
            ID="rptFreelancers"
            runat="server">

            <ItemTemplate>

                <div class="freelancer-row">

                    <div class="freelancer-avatar">
                        <%# Eval("Initials") %>
                    </div>


                    <div class="freelancer-main">

                        <div class="freelancer-title-row">

                            <div>

                                <h3>
                                    <%# Eval("Name") %>
                                </h3>

                                <p class="freelancer-title">
                                    <%# Eval("Title") %>
                                </p>

                            </div>

                            <div class="rating">

                                <i class="fa-solid fa-star"></i>

                                <strong>
                                    <%# Eval("Rating") %>
                                </strong>

                                <span>
                                    (<%# Eval("Reviews") %> reviews)
                                </span>

                            </div>

                        </div>


                        <div class="skills">

                            <%# GetSkills(Eval("Skills").ToString()) %>

                        </div>


                        <p class="freelancer-bio">
                            <%# Eval("Bio") %>
                        </p>

                    </div>


                    <div class="freelancer-actions">

                        <a
                            href="Profile.aspx"
                            class="profile-button">

                            View Profile

                        </a>

                        <a
                            href="Messages.aspx"
                            class="contact-button">

                            <i class="fa-regular fa-message"></i>

                            Contact

                        </a>

                    </div>

                </div>

            </ItemTemplate>

        </asp:Repeater>


        <!-- PAGINATION -->

        <div class="pagination">

            <asp:LinkButton
                ID="btnPrevious"
                runat="server"
                CssClass="page-button disabled">
                <i class="fa-solid fa-chevron-left"></i>
            </asp:LinkButton>

            <asp:LinkButton
                ID="btnPage1"
                runat="server"
                CssClass="page-button page-active">
                1
            </asp:LinkButton>

            <asp:LinkButton
                ID="btnPage2"
                runat="server"
                CssClass="page-button">
                2
            </asp:LinkButton>

            <asp:LinkButton
                ID="btnPage3"
                runat="server"
                CssClass="page-button">
                3
            </asp:LinkButton>

            <asp:LinkButton
                ID="btnNext"
                runat="server"
                CssClass="page-button">
                <i class="fa-solid fa-chevron-right"></i>
            </asp:LinkButton>

        </div>

    </div>

</asp:Content>