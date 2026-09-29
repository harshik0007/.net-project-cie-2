<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FindProjects.aspx.cs" Inherits="CIE_2_PROJECT.FindProjects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link rel="stylesheet" href="../Content/Css/FindProjects.css" />

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="projects-page">

        <div class="search-box">

            <i class="fa-solid fa-magnifying-glass search-icon"></i>

            <input
                type="text"
                class="search-input"
                placeholder="Search projects by title, skill or keyword..." />

            <select class="category-select">

                <option>All Categories</option>
                <option>Web Development</option>
                <option>UI/UX Design</option>
                <option>Game Development</option>
                <option>Mobile Development</option>
                <option>Data Analysis</option>
                <option>Content Writing</option>
                <option>Digital Marketing</option>

            </select>

            <button type="button" class="search-button">
                Search
            </button>

        </div>


        <div class="projects-list">


            <div class="project-card">

                <div class="project-main">

                    <div class="project-icon web-icon">
                        <i class="fa-solid fa-code"></i>
                    </div>

                    <div class="project-content">

                        <h2 class="project-title">
                            Build a .NET Core E-commerce Website
                        </h2>

                        <p class="project-description">
                            Need a complete e-commerce solution with ASP.NET Core,
                            SQL Server and payment integration.
                        </p>

                        <div class="project-tags">

                            <span class="project-tag blue">
                                .NET Core
                            </span>

                            <span class="project-tag blue">
                                C#
                            </span>

                            <span class="project-tag blue">
                                SQL Server
                            </span>

                        </div>

                        <div class="posted-info">

                            <i class="fa-regular fa-clock"></i>

                            <span>
                                Posted 1 day ago
                            </span>

                        </div>

                    </div>

                </div>


                <div class="project-action">

                    <div class="project-price">
                        &#8377;8,000 - &#8377;15,000
                    </div>

                    <div class="price-type">
                        Fixed Price
                    </div>

                    <button type="button" class="bookmark-button">
                        <i class="fa-regular fa-bookmark"></i>
                    </button>

                    <div class="deadline">

                        <i class="fa-regular fa-calendar"></i>

                        <span>
                            Deadline: 20 May 2026
                        </span>

                    </div>

                    <a href="ProjectDetails.aspx" class="view-project">
                        View Project
                    </a>

                </div>

            </div>


            <div class="project-card">

                <div class="project-main">

                    <div class="project-icon game-icon">
                        <i class="fa-solid fa-gamepad"></i>
                    </div>

                    <div class="project-content">

                        <h2 class="project-title">
                            Unity Game Development for 2D Adventure Game
                        </h2>

                        <p class="project-description">
                            Looking for a Unity developer to create a 2D adventure
                            game with multiple levels and characters.
                        </p>

                        <div class="project-tags">

                            <span class="project-tag green">
                                Unity
                            </span>

                            <span class="project-tag green">
                                C#
                            </span>

                            <span class="project-tag green">
                                2D Animation
                            </span>

                        </div>

                        <div class="posted-info">

                            <i class="fa-regular fa-clock"></i>

                            <span>
                                Posted 2 days ago
                            </span>

                        </div>

                    </div>

                </div>


                <div class="project-action">

                    <div class="project-price">
                        &#8377;5,000 - &#8377;10,000
                    </div>

                    <div class="price-type">
                        Fixed Price
                    </div>

                    <button type="button" class="bookmark-button">
                        <i class="fa-regular fa-bookmark"></i>
                    </button>

                    <div class="deadline">

                        <i class="fa-regular fa-calendar"></i>

                        <span>
                            Deadline: 18 May 2026
                        </span>

                    </div>

                    <a href="ProjectDetails.aspx" class="view-project">
                        View Project
                    </a>

                </div>

            </div>


            <div class="project-card">

                <div class="project-main">

                    <div class="project-icon writing-icon">
                        <i class="fa-solid fa-file-lines"></i>
                    </div>

                    <div class="project-content">

                        <h2 class="project-title">
                            Content Writing for College Blog
                        </h2>

                        <p class="project-description">
                            Write informative and engaging blog posts related to
                            college life, education and career tips.
                        </p>

                        <div class="project-tags">

                            <span class="project-tag yellow">
                                Content Writing
                            </span>

                            <span class="project-tag yellow">
                                Blog Writing
                            </span>

                        </div>

                        <div class="posted-info">

                            <i class="fa-regular fa-clock"></i>

                            <span>
                                Posted 3 days ago
                            </span>

                        </div>

                    </div>

                </div>


                <div class="project-action">

                    <div class="project-price">
                        &#8377;2,000 - &#8377;4,000
                    </div>

                    <div class="price-type">
                        Fixed Price
                    </div>

                    <button type="button" class="bookmark-button">
                        <i class="fa-regular fa-bookmark"></i>
                    </button>

                    <div class="deadline">

                        <i class="fa-regular fa-calendar"></i>

                        <span>
                            Deadline: 12 May 2026
                        </span>

                    </div>

                    <a href="ProjectDetails.aspx" class="view-project">
                        View Project
                    </a>

                </div>

            </div>


            <div class="project-card">

                <div class="project-main">

                    <div class="project-icon design-icon">
                        <i class="fa-solid fa-bullhorn"></i>
                    </div>

                    <div class="project-content">

                        <h2 class="project-title">
                            Social Media Post Design
                        </h2>

                        <p class="project-description">
                            Create attractive social media posts and stories for
                            college events and announcements.
                        </p>

                        <div class="project-tags">

                            <span class="project-tag purple">
                                Canva
                            </span>

                            <span class="project-tag purple">
                                Graphic Design
                            </span>

                        </div>

                        <div class="posted-info">

                            <i class="fa-regular fa-clock"></i>

                            <span>
                                Posted 4 days ago
                            </span>

                        </div>

                    </div>

                </div>


                <div class="project-action">

                    <div class="project-price">
                        &#8377;1,500 - &#8377;3,000
                    </div>

                    <div class="price-type">
                        Fixed Price
                    </div>

                    <button type="button" class="bookmark-button">
                        <i class="fa-regular fa-bookmark"></i>
                    </button>

                    <div class="deadline">

                        <i class="fa-regular fa-calendar"></i>

                        <span>
                            Deadline: 15 May 2026
                        </span>

                    </div>

                    <a href="ProjectDetails.aspx" class="view-project">
                        View Project
                    </a>

                </div>

            </div>


        </div>

    </div>

</asp:Content>