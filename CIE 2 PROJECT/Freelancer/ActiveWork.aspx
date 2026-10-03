<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ActiveWork.aspx.cs" Inherits="CIE_2_PROJECT.ActiveWork" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/ActiveWork.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="active-work-page">

        <div class="page-header">
            <h1 class="page-title">Active Work</h1>
            <p class="page-subtitle">
                Projects you are working on. Track progress and manage your active jobs.
            </p>
        </div>

        <div class="work-summary">

            <div class="summary-item">
                <div class="summary-icon summary-blue">
                    <i class="fa-solid fa-briefcase"></i>
                </div>

                <div class="summary-content">
                    <span class="summary-label">Total Active Projects</span>
                    <span class="summary-value">4</span>
                </div>
            </div>

            <div class="summary-item">
                <div class="summary-icon summary-green">
                    <i class="fa-solid fa-list-check"></i>
                </div>

                <div class="summary-content">
                    <span class="summary-label">In Progress</span>
                    <span class="summary-value">3</span>
                </div>
            </div>

            <div class="summary-item">
                <div class="summary-icon summary-purple">
                    <i class="fa-regular fa-clock"></i>
                </div>

                <div class="summary-content">
                    <span class="summary-label">Pending Review</span>
                    <span class="summary-value">1</span>
                </div>
            </div>

            <div class="summary-item summary-last">
                <div class="summary-icon summary-red">
                    <i class="fa-solid fa-money-bill-wave"></i>
                </div>

                <div class="summary-content">
                    <span class="summary-label">Total Earnings</span>
                    <span class="summary-value">&#8377;24,500</span>
                </div>
            </div>

        </div>


        <div class="active-projects">

            <div class="active-project-card">

                <div class="project-main">

                    <div class="project-icon game-icon">
                        <i class="fa-solid fa-gamepad"></i>
                    </div>

                    <div class="project-content">

                        <h2 class="project-title">
                            Unity Game Development for 2D Adventure Game
                        </h2>

                        <div class="project-meta">

                            <span>
                                <i class="fa-regular fa-user"></i>
                                Client:
                                <a href="#" class="client-link">Game Studio</a>
                            </span>

                            <span class="meta-dot">•</span>

                            <span>
                                <i class="fa-regular fa-calendar"></i>
                                Started on 18 May 2026
                            </span>

                        </div>

                        <p class="project-description">
                            Develop a 2D adventure game in Unity with multiple levels,
                            player movement, enemies, and power-ups.
                        </p>

                    </div>

                </div>


                <div class="project-side">

                    <div class="side-top">

                        <div class="side-info">
                            <span class="side-label">Agreed Price</span>
                            <span class="side-price">&#8377;9,000</span>
                        </div>

                        <span class="status-badge status-progress">
                            <i class="fa-solid fa-rotate"></i>
                            In Progress
                        </span>

                    </div>

                    <div class="deadline-section">
                        <span class="side-label">Deadline</span>
                        <strong>30 May 2026</strong>
                    </div>

                    <div class="progress-row">
                        <div class="progress-track">
                            <div class="progress-fill progress-60"></div>
                        </div>

                        <span class="progress-value">60%</span>
                    </div>

                    <a href="SubmitWork.aspx" class="open-project-button">
                        Open Project
                    </a>

                </div>

            </div>


            <div class="active-project-card">

                <div class="project-main">

                    <div class="project-icon code-icon">
                        <i class="fa-solid fa-code"></i>
                    </div>

                    <div class="project-content">

                        <h2 class="project-title">
                            Build a .NET Core E-commerce Website
                        </h2>

                        <div class="project-meta">

                            <span>
                                <i class="fa-regular fa-user"></i>
                                Client:
                                <a href="#" class="client-link">John Doe</a>
                            </span>

                            <span class="meta-dot">•</span>

                            <span>
                                <i class="fa-regular fa-calendar"></i>
                                Started on 22 May 2026
                            </span>

                        </div>

                        <p class="project-description">
                            Build a complete e-commerce website using ASP.NET Core
                            with user authentication, product management, and payment
                            gateway.
                        </p>

                    </div>

                </div>


                <div class="project-side">

                    <div class="side-top">

                        <div class="side-info">
                            <span class="side-label">Agreed Price</span>
                            <span class="side-price">&#8377;12,000</span>
                        </div>

                        <span class="status-badge status-progress blue-status">
                            <i class="fa-solid fa-rotate"></i>
                            In Progress
                        </span>

                    </div>

                    <div class="deadline-section">
                        <span class="side-label">Deadline</span>
                        <strong>10 June 2026</strong>
                    </div>

                    <div class="progress-row">
                        <div class="progress-track">
                            <div class="progress-fill progress-40"></div>
                        </div>

                        <span class="progress-value">40%</span>
                    </div>

                    <a href="SubmitWork.aspx" class="open-project-button">
                        Open Project
                    </a>

                </div>

            </div>


            <div class="active-project-card">

                <div class="project-main">

                    <div class="project-icon writing-icon">
                        <i class="fa-regular fa-file-lines"></i>
                    </div>

                    <div class="project-content">

                        <h2 class="project-title">
                            Content Writing for College Blog
                        </h2>

                        <div class="project-meta">

                            <span>
                                <i class="fa-regular fa-user"></i>
                                Client:
                                <a href="#" class="client-link">College Blog</a>
                            </span>

                            <span class="meta-dot">•</span>

                            <span>
                                <i class="fa-regular fa-calendar"></i>
                                Started on 12 May 2026
                            </span>

                        </div>

                        <p class="project-description">
                            Write informative and engaging blog posts related to
                            college life, education and career tips.
                        </p>

                    </div>

                </div>


                <div class="project-side">

                    <div class="side-top">

                        <div class="side-info">
                            <span class="side-label">Agreed Price</span>
                            <span class="side-price">&#8377;2,500</span>
                        </div>

                        <span class="status-badge status-review">
                            <i class="fa-regular fa-clock"></i>
                            Pending Review
                        </span>

                    </div>

                    <div class="deadline-section">
                        <span class="side-label">Deadline</span>
                        <strong>20 May 2026</strong>
                    </div>

                    <div class="progress-row">
                        <div class="progress-track">
                            <div class="progress-fill progress-100"></div>
                        </div>

                        <span class="progress-value">100%</span>
                    </div>

                    <a href="SubmitWork.aspx" class="open-project-button">
                        Open Project
                    </a>

                </div>

            </div>

        </div>


        <div class="work-note">
            <i class="fa-regular fa-circle-info"></i>
            <span>Keep your clients updated and deliver on time to build a strong reputation!</span>
        </div>

    </div>

</asp:Content>
