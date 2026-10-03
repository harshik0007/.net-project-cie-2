<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProjectDetails.aspx.cs" Inherits="CIE_2_PROJECT.ProjectDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link rel="stylesheet" href="../Content/Css/ProjectDetails.css" />

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="details-page">

        <a href="FindProjects.aspx" class="back-link">
            <i class="fa-solid fa-arrow-left"></i>
            Back to Find Projects
        </a>


        <div class="details-layout">

            <div class="details-main">

                <div class="project-details-card">

                    <h1 class="project-title">
                        Unity 2D Game Development
                    </h1>

                    <div class="project-meta">

                        <span>
                            <i class="fa-regular fa-calendar"></i>
                            Posted on 18 May 2026
                        </span>

                        <span class="meta-dot">
                            •
                        </span>

                        <span>
                            <i class="fa-regular fa-user"></i>
                            Client:
                            <a href="#">
                                John Doe
                            </a>
                        </span>

                    </div>


                    <div class="section-divider"></div>


                    <section class="detail-section">

                        <h2 class="detail-heading">

                            <i class="fa-regular fa-file-lines"></i>

                            Project Description

                        </h2>

                        <p class="detail-text">
                            Create a 2D shooting game with multiple levels, enemies, and an in-game store using Unity and C#.
                            Build a polished 2D shooting game with multiple levels, enemies, and an in-game store.
                        </p>

                    </section>


                    <section class="detail-section">

                        <h2 class="detail-heading">

                            <i class="fa-solid fa-list"></i>

                            What You Will Do

                        </h2>

                        <ul class="detail-list">

                            <li>
                                Build the game in Unity using C#
                            </li>

                            <li>
                                Create multiple playable levels and enemy types
                            </li>

                            <li>
                                Implement an in-game store
                            </li>

                            <li>
                                Design responsive controls and smooth gameplay
                            </li>

                            <li>
                                Deliver source files and project documentation
                            </li>

                        </ul>

                    </section>


                    <section class="detail-section">

                        <h2 class="detail-heading">

                            <i class="fa-solid fa-code"></i>

                            Required Skills

                        </h2>

                        <div class="skill-list">

                            <span class="skill-tag">
                                Unity
                            </span>

                            <span class="skill-tag">
                                C#
                            </span>

                            <span class="skill-tag">
                                2D Game Development
                            </span>

                            <span class="skill-tag">
                                Game Design
                            </span>

                            <span class="skill-tag">
                                Git
                            </span>

                        </div>

                    </section>


                    <section class="detail-section">

                        <h2 class="detail-heading">

                            <i class="fa-solid fa-list-check"></i>

                            Project Requirements

                        </h2>

                        <ul class="detail-list">

                            <li>
                                Intermediate Unity experience
                            </li>

                            <li>
                                Proficiency in C#
                            </li>

                            <li>
                                Experience building 2D games
                            </li>

                            <li>
                                Knowledge of game design principles
                            </li>

                            <li>
                                On-time delivery
                            </li>

                        </ul>

                    </section>

                </div>


                <div class="action-card">

                    <button type="button" class="save-button">

                        <i class="fa-regular fa-bookmark"></i>

                        Save Project

                    </button>


                    <a href="SubmitProposal.aspx" class="proposal-button">

                        <i class="fa-regular fa-paper-plane"></i>

                        Submit Proposal

                    </a>

                </div>

            </div>


            <aside class="summary-card">

                <h2 class="summary-title">
                    Project Summary
                </h2>


                <div class="summary-item">

                    <div class="summary-icon">
                        <i class="fa-solid fa-dollar-sign"></i>
                    </div>

                    <div class="summary-content">

                        <div class="summary-label">
                            Budget
                        </div>

                        <div class="summary-value budget">
                            &#8377;15,000 - &#8377;25,000
                        </div>

                    </div>

                </div>


                <div class="summary-item">

                    <div class="summary-icon">
                        <i class="fa-regular fa-clock"></i>
                    </div>

                    <div class="summary-content">

                        <div class="summary-label">
                            Project Type
                        </div>

                        <div class="summary-value">
                            Fixed Price
                        </div>

                    </div>

                </div>


                <div class="summary-item">

                    <div class="summary-icon">
                        <i class="fa-regular fa-calendar"></i>
                    </div>

                    <div class="summary-content">

                        <div class="summary-label">
                            Deadline
                        </div>

                        <div class="summary-value">
                            30 May, 2026
                        </div>

                    </div>

                </div>


                <div class="summary-item">

                    <div class="summary-icon">
                        <i class="fa-solid fa-chart-column"></i>
                    </div>

                    <div class="summary-content">

                        <div class="summary-label">
                            Experience Level
                        </div>

                        <div class="summary-value">
                            Intermediate
                        </div>

                    </div>

                </div>


                <div class="summary-item">

                    <div class="summary-icon">
                        <i class="fa-regular fa-folder"></i>
                    </div>

                    <div class="summary-content">

                        <div class="summary-label">
                            Number of Proposals
                        </div>

                        <div class="summary-value">
                            12 Proposals
                        </div>

                    </div>

                </div>

            </aside>

        </div>

    </div>

</asp:Content>
