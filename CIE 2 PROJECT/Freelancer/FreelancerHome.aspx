<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FreelancerHome.aspx.cs" Inherits="CIE_2_PROJECT.FreelancerHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link rel="stylesheet" href="../Content/Css/FreelancerHome.css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="home-page">

        <section class="hero">

            <div class="hero-content">

                <div class="welcome">
                    Welcome back, xyz abc!
                </div>

                <h1 class="hero-title">
                    Find your next
                    <br />
                    <span>great project</span>
                </h1>

                <p class="hero-description">
                    Explore thousands of projects posted by clients
                    and build your career on your terms.
                </p>

                <a href="FindProjects.aspx" class="primary-button">
                    <i class="fa-solid fa-magnifying-glass"></i>
                    Find Projects
                </a>

            </div>

            <img
                src="../Content/Images/freelancer-home-banner.png"
                class="hero-image"
                alt="Freelancer working on a project" />

        </section>


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

            </select>

            <a href="FindProjects.aspx" class="search-button" style="display: inline-flex; align-items: center; justify-content: center; text-decoration: none;">
                Search
            </a>

        </div>


        <section class="section">

            <div class="section-header">

                <h2 class="section-title">
                    Recommended Projects
                </h2>

                <a href="FindProjects.aspx" class="view-all">
                    View all
                </a>

            </div>


            <div class="dashboard-grid">

                <div class="project-grid">

                    <div class="project-card">

                        <h3 class="project-title">
                            <a href="ProjectDetails.aspx" style="color: inherit; text-decoration: none;">Unity 2D Game Development</a>
                        </h3>

                        <p class="project-description">
                            Need a complete e-commerce solution with ASP.NET
                            Core, SQL Server and payment...
                        </p>

                        <div class="tags">

                            <span class="tag">.NET Core</span>
                            <span class="tag">C#</span>
                            <span class="tag">SQL Server</span>

                        </div>

                        <div class="project-bottom">

                            <span class="project-price">
                                &#8377;15,000 - &#8377;25,000
                            </span>

                            <span class="project-days">
                                5 days left
                            </span>

                        </div>

                    </div>


                    <div class="project-card">

                        <h3 class="project-title">
                            <a href="ProjectDetails.aspx" style="color: inherit; text-decoration: none;">Unity 2D Game Development</a>
                        </h3>

                        <p class="project-description">
                            Looking for a Unity developer to create a 2D
                            adventure game with multiple levels and...
                        </p>

                        <div class="tags">

                            <span class="tag">Unity</span>
                            <span class="tag">C#</span>
                            <span class="tag">2D Animation</span>

                        </div>

                        <div class="project-bottom">

                            <span class="project-price">
                                &#8377;15,000 - &#8377;25,000
                            </span>

                            <span class="project-days">
                                7 days left
                            </span>

                        </div>

                    </div>

                </div>


                <div class="profile-card">

                    <div class="profile-progress-header">

                        <span class="profile-section-title">
                            Profile Completion
                        </span>

                        <span class="profile-percentage">
                            75%
                        </span>

                    </div>

                    <div class="progress">
                        <div class="progress-bar"></div>
                    </div>

                    <p class="profile-description">
                        Complete your profile to get more project recommendations.
                    </p>

                    <a href="../Authentication/FreelancerSkills.aspx" class="complete-button" style="display: flex; align-items: center; justify-content: center; text-decoration: none;">
                        Complete Profile
                    </a>


                    <div class="profile-divider"></div>


                    <div class="skills-header">

                        <span class="profile-section-title">
                            My Skills
                        </span>

                        <a href="../Authentication/FreelancerSkills.aspx" class="edit-link">
                            Edit
                        </a>

                    </div>

                    <div class="skill-tags">

                        <span class="skill-tag">C#</span>
                        <span class="skill-tag">ASP.NET Core</span>
                        <span class="skill-tag">SQL</span>
                        <span class="skill-tag">Unity</span>

                    </div>

                    <a href="../Authentication/FreelancerSkills.aspx" class="add-skill">
                        + Add more
                    </a>

                </div>

            </div>

        </section>


        <section class="section">

            <div class="section-header">

                <h2 class="section-title">
                    Popular Categories
                </h2>

                <a href="FindProjects.aspx" class="view-all">
                    View all
                </a>

            </div>


            <div class="dashboard-grid">

                <div class="categories">

                    <div class="category-card">

                        <div class="category-icon">
                            <i class="fa-solid fa-code"></i>
                        </div>

                        <span class="category-name">
                            Web Dev
                        </span>

                    </div>


                    <div class="category-card">

                        <div class="category-icon">
                            <i class="fa-solid fa-palette"></i>
                        </div>

                        <span class="category-name">
                            UI/UX Design
                        </span>

                    </div>


                    <div class="category-card">

                        <div class="category-icon">
                            <i class="fa-solid fa-gamepad"></i>
                        </div>

                        <span class="category-name">
                            Game Dev
                        </span>

                    </div>


                    <div class="category-card">

                        <div class="category-icon">
                            <i class="fa-solid fa-ellipsis"></i>
                        </div>

                        <span class="category-name">
                            More
                        </span>

                    </div>

                </div>


                <div class="stats-card">

                    <a href="MyProposals.aspx" class="stat" style="text-decoration: none; color: inherit;">

                        <div class="stat-icon">
                            <i class="fa-solid fa-paper-plane"></i>
                        </div>

                        <div class="stat-number">
                            12
                        </div>

                        <div class="stat-label">
                            Proposals Sent
                        </div>

                    </a>


                    <div class="stat">

                        <div class="stat-icon">
                            <i class="fa-solid fa-briefcase"></i>
                        </div>

                        <div class="stat-number">
                            5
                        </div>

                        <div class="stat-label">
                            Projects In<br />
                            Progress
                        </div>

                    </div>


                    <div class="stat">

                        <div class="stat-icon">
                            <i class="fa-regular fa-circle-check"></i>
                        </div>

                        <div class="stat-number">
                            3
                        </div>

                        <div class="stat-label">
                            Projects<br />
                            Completed
                        </div>

                    </div>

                </div>

            </div>

        </section>


        <section class="benefits">

            <div class="benefit">

                <div class="benefit-icon">
                    <i class="fa-solid fa-shield-halved"></i>
                </div>

                <div>

                    <div class="benefit-title">
                        Verified Clients
                    </div>

                    <div class="benefit-text">
                        Work with trusted and verified clients.
                    </div>

                </div>

            </div>


            <div class="benefit">

                <div class="benefit-icon">
                    <i class="fa-solid fa-money-bill-wave"></i>
                </div>

                <div>

                    <div class="benefit-title">
                        Secure Payments
                    </div>

                    <div class="benefit-text">
                        Get paid safely for your hard work.
                    </div>

                </div>

            </div>


            <div class="benefit">

                <div class="benefit-icon">
                    <i class="fa-solid fa-headset"></i>
                </div>

                <div>

                    <div class="benefit-title">
                        24/7 Support
                    </div>

                    <div class="benefit-text">
                        We're here to help you anytime.
                    </div>

                </div>

            </div>

        </section>

    </div>

</asp:Content>
