<%@ Page Title="Client Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientHome.aspx.cs" Inherits="CIE_2_PROJECT.ClientHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/ClientHome.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="client-home">

        <!-- HERO SECTION -->

        <section class="hero-section">

            <div class="hero-content">

                <p class="welcome-text">
                    Welcome back, Client Co.
                </p>

                <h1>
                    Find the right talent.<br />
                    Get your work done.
                </h1>

                <p class="hero-description">
                    Post your project and hire skilled freelancers<br />
                    to get it done on time.
                </p>

                <div class="hero-actions">

                    <a href="CreateProject.aspx" class="primary-button">
                        <i class="fa-solid fa-plus"></i>
                        <span>Post a Project</span>
                    </a>

                    <a href="FindFreelancers.aspx" class="secondary-button">
                        <i class="fa-solid fa-magnifying-glass"></i>
                        <span>Search Freelancers</span>
                    </a>

                </div>

            </div>


            <div class="hero-illustration">

                <div class="document-card">

                    <div class="document-line document-line-small"></div>
                    <div class="document-line"></div>
                    <div class="document-line"></div>
                    <div class="document-line"></div>

                </div>

                <div class="user-card">

                    <div class="user-circle">
                        <i class="fa-solid fa-user"></i>
                    </div>

                    <div class="verified-icon">
                        <i class="fa-solid fa-check"></i>
                    </div>

                </div>

            </div>

        </section>


        <!-- STATISTICS -->

        <section class="stats-grid">

            <div class="stat-card">

                <div class="stat-icon stat-blue">
                    <i class="fa-solid fa-folder-open"></i>
                </div>

                <div class="stat-content">

                    <div class="stat-value">
                        3
                    </div>

                    <div class="stat-label">
                        Active Projects
                    </div>

                    <a href="MyProjects.aspx" class="stat-link">
                        View All
                    </a>

                </div>

            </div>


            <div class="stat-card">

                <div class="stat-icon stat-yellow">
                    <i class="fa-regular fa-clock"></i>
                </div>

                <div class="stat-content">

                    <div class="stat-value">
                        2
                    </div>

                    <div class="stat-label">
                        Open for Proposals
                    </div>

                    <a href="MyProjects.aspx" class="stat-link">
                        View All
                    </a>

                </div>

            </div>


            <div class="stat-card">

                <div class="stat-icon stat-green">
                    <i class="fa-regular fa-circle-check"></i>
                </div>

                <div class="stat-content">

                    <div class="stat-value">
                        1
                    </div>

                    <div class="stat-label">
                        Completed Projects
                    </div>

                    <a href="MyProjects.aspx" class="stat-link">
                        View All
                    </a>

                </div>

            </div>


            <div class="stat-card">

                <div class="stat-icon stat-purple">
                    <i class="fa-solid fa-wallet"></i>
                </div>

                <div class="stat-content">

                    <div class="stat-value">
                        &#8377;7,500
                    </div>

                    <div class="stat-label">
                        Total Spent
                    </div>

                    <a href="Payments.aspx" class="stat-link">
                        View Payments
                    </a>

                </div>

            </div>

        </section>


        <!-- MY PROJECTS -->

        <section class="projects-section">

            <div class="section-header">

                <h2>
                    My Projects
                </h2>

                <a href="MyProjects.aspx" class="view-all-link">
                    View All Projects
                    <i class="fa-solid fa-arrow-right"></i>
                </a>

            </div>


            <div class="project-tabs">

                <a href="#" class="project-tab active">
                    Active (3)
                </a>

                <a href="#" class="project-tab">
                    Open for Proposals (2)
                </a>

                <a href="#" class="project-tab">
                    Completed (1)
                </a>

            </div>


            <!-- PROJECT 1 -->

            <div class="project-row">

                <div class="project-left">

                    <div class="project-icon project-icon-green">
                        <i class="fa-solid fa-gamepad"></i>
                    </div>

                    <div class="project-info">

                        <h3>
                            Unity 2D Game Development
                        </h3>

                        <div class="project-meta">
                            <span>12 Proposals</span>
                            <span class="meta-dot">•</span>
                            <span>Updated 2h ago</span>
                        </div>

                    </div>

                </div>


                <div class="project-budget">

                    <strong>
                        &#8377;15,000 - &#8377;25,000
                    </strong>

                    <span>
                        Budget
                    </span>

                </div>


                <div class="project-progress">

                    <div class="progress-top">
                        <strong>60%</strong>
                    </div>

                    <div class="progress-track">
                        <div class="progress-fill progress-60"></div>
                    </div>

                    <span>
                        Progress
                    </span>

                </div>


                <div class="project-status status-progress">
                    In Progress
                </div>


                <div class="project-menu">
                    <i class="fa-solid fa-ellipsis-vertical"></i>
                </div>

            </div>


            <!-- PROJECT 2 -->

            <div class="project-row">

                <div class="project-left">

                    <div class="project-icon project-icon-purple">
                        <i class="fa-solid fa-code"></i>
                    </div>

                    <div class="project-info">

                        <h3>
                            E-commerce Website Development
                        </h3>

                        <div class="project-meta">
                            <span>8 Proposals</span>
                            <span class="meta-dot">•</span>
                            <span>Updated 3d ago</span>
                        </div>

                    </div>

                </div>


                <div class="project-budget">

                    <strong>
                        &#8377;20,000 - &#8377;40,000
                    </strong>

                    <span>
                        Budget
                    </span>

                </div>


                <div class="project-progress">

                    <div class="progress-top">
                        <strong>40%</strong>
                    </div>

                    <div class="progress-track">
                        <div class="progress-fill progress-40"></div>
                    </div>

                    <span>
                        Progress
                    </span>

                </div>


                <div class="project-status status-progress">
                    In Progress
                </div>


                <div class="project-menu">
                    <i class="fa-solid fa-ellipsis-vertical"></i>
                </div>

            </div>


            <!-- PROJECT 3 -->

            <div class="project-row">

                <div class="project-left">

                    <div class="project-icon project-icon-yellow">
                        <i class="fa-solid fa-pen-nib"></i>
                    </div>

                    <div class="project-info">

                        <h3>
                            Brand Logo Design
                        </h3>

                        <div class="project-meta">
                            <span>5 Proposals</span>
                            <span class="meta-dot">•</span>
                            <span>Updated 1w ago</span>
                        </div>

                    </div>

                </div>


                <div class="project-budget">

                    <strong>
                        &#8377;2,000 - &#8377;5,000
                    </strong>

                    <span>
                        Budget
                    </span>

                </div>


                <div class="project-progress">

                    <div class="progress-top">
                        <strong>100%</strong>
                    </div>

                    <div class="progress-track">
                        <div class="progress-fill progress-100"></div>
                    </div>

                    <span>
                        Progress
                    </span>

                </div>


                <div class="project-status status-completed">
                    Completed
                </div>


                <div class="project-menu">
                    <i class="fa-solid fa-ellipsis-vertical"></i>
                </div>

            </div>

        </section>


        <!-- TOP FREELANCERS -->

        <section class="freelancers-section">

            <div class="section-header">

                <h2>
                    Top Freelancers for You
                </h2>

                <a href="FindFreelancers.aspx" class="view-all-link">
                    View All Freelancers
                    <i class="fa-solid fa-arrow-right"></i>
                </a>

            </div>


            <div class="freelancer-grid">

                <!-- RAHUL -->

                <div class="freelancer-card">

                    <h3>
                        Rahul Sharma
                    </h3>

                    <p class="freelancer-role">
                        Full Stack Developer
                    </p>

                    <div class="rating">
                        <i class="fa-solid fa-star"></i>
                        <span>4.8 (26)</span>
                    </div>

                    <div class="skill-tags">

                        <span>React.js</span>
                        <span>Node.js</span>
                        <span>MongoDB</span>

                    </div>

                    <div class="hourly-rate">
                        &#8377;800 <span>/hr</span>
                    </div>

                    <a href="FindFreelancers.aspx" class="profile-button">
                        View Profile
                    </a>

                </div>


                <!-- PRIYA -->

                <div class="freelancer-card">

                    <h3>
                        Priya Mehta
                    </h3>

                    <p class="freelancer-role">
                        UI/UX Designer
                    </p>

                    <div class="rating">
                        <i class="fa-solid fa-star"></i>
                        <span>4.9 (18)</span>
                    </div>

                    <div class="skill-tags">

                        <span>Figma</span>
                        <span>UI Design</span>
                        <span>Adobe XD</span>

                    </div>

                    <div class="hourly-rate">
                        &#8377;700 <span>/hr</span>
                    </div>

                    <a href="FindFreelancers.aspx" class="profile-button">
                        View Profile
                    </a>

                </div>


                <!-- AMIT -->

                <div class="freelancer-card">

                    <h3>
                        Amit Verma
                    </h3>

                    <p class="freelancer-role">
                        WordPress Developer
                    </p>

                    <div class="rating">
                        <i class="fa-solid fa-star"></i>
                        <span>4.7 (32)</span>
                    </div>

                    <div class="skill-tags">

                        <span>WordPress</span>
                        <span>PHP</span>
                        <span>Elementor</span>

                    </div>

                    <div class="hourly-rate">
                        &#8377;600 <span>/hr</span>
                    </div>

                    <a href="FindFreelancers.aspx" class="profile-button">
                        View Profile
                    </a>

                </div>


                <!-- NEHA -->

                <div class="freelancer-card">

                    <h3>
                        Neha Patel
                    </h3>

                    <p class="freelancer-role">
                        Graphic Designer
                    </p>

                    <div class="rating">
                        <i class="fa-solid fa-star"></i>
                        <span>4.9 (21)</span>
                    </div>

                    <div class="skill-tags">

                        <span>Photoshop</span>
                        <span>Illustrator</span>
                        <span>Logo Design</span>

                    </div>

                    <div class="hourly-rate">
                        &#8377;500 <span>/hr</span>
                    </div>

                    <a href="FindFreelancers.aspx" class="profile-button">
                        View Profile
                    </a>

                </div>

            </div>

        </section>


        <!-- QUICK ACTIONS -->

        <section class="quick-actions-section">

            <div class="section-header quick-header">

                <h2>
                    Quick Actions
                </h2>

            </div>


            <div class="quick-actions-grid">

                <a href="CreateProject.aspx" class="quick-action">

                    <div class="quick-icon quick-blue">
                        <i class="fa-solid fa-plus"></i>
                    </div>

                    <div class="quick-text">

                        <strong>
                            Post a Project
                        </strong>

                        <span>
                            Get proposals from freelancers
                        </span>

                    </div>

                    <i class="fa-solid fa-chevron-right quick-arrow"></i>

                </a>


                <a href="FindFreelancers.aspx" class="quick-action">

                    <div class="quick-icon quick-green">
                        <i class="fa-solid fa-magnifying-glass"></i>
                    </div>

                    <div class="quick-text">

                        <strong>
                            Search Freelancers
                        </strong>

                        <span>
                            Find the right talent
                        </span>

                    </div>

                    <i class="fa-solid fa-chevron-right quick-arrow"></i>

                </a>


                    <a href="../Messages/Messages.aspx" class="quick-action">

                    <div class="quick-icon quick-yellow">
                        <i class="fa-regular fa-message"></i>
                    </div>

                    <div class="quick-text">

                        <strong>
                            Messages
                        </strong>

                        <span>
                            Chat with freelancers
                        </span>

                    </div>

                    <i class="fa-solid fa-chevron-right quick-arrow"></i>

                </a>


                <a href="Payments.aspx" class="quick-action">

                    <div class="quick-icon quick-purple">
                        <i class="fa-solid fa-wallet"></i>
                    </div>

                    <div class="quick-text">

                        <strong>
                            Payments
                        </strong>

                        <span>
                            View transactions
                        </span>

                    </div>

                    <i class="fa-solid fa-chevron-right quick-arrow"></i>

                </a>

            </div>

        </section>

    </div>

</asp:Content>
