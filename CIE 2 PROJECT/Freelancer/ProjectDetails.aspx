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
                        Build a .NET Core E-commerce Website
                    </h1>

                    <div class="project-meta">

                        <span>
                            <i class="fa-regular fa-calendar"></i>
                            Posted on 20 May 2026
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
                            We need a complete e-commerce solution using ASP.NET Core.
                            The website should include user authentication, product
                            management, shopping cart, order management, and payment
                            integration. The admin panel should allow managing
                            products, orders, users, and reports.
                        </p>

                    </section>


                    <section class="detail-section">

                        <h2 class="detail-heading">

                            <i class="fa-solid fa-list"></i>

                            What You Will Do

                        </h2>

                        <ul class="detail-list">

                            <li>
                                Develop a responsive e-commerce website using ASP.NET Core
                            </li>

                            <li>
                                Implement user authentication and authorization
                            </li>

                            <li>
                                Build product catalog, cart, and checkout system
                            </li>

                            <li>
                                Integrate secure payment gateway
                            </li>

                            <li>
                                Create admin panel for managing products, orders, and users
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
                                .NET Core
                            </span>

                            <span class="skill-tag">
                                C#
                            </span>

                            <span class="skill-tag">
                                SQL Server
                            </span>

                            <span class="skill-tag">
                                JavaScript
                            </span>

                            <span class="skill-tag">
                                Bootstrap
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
                                Strong knowledge of ASP.NET Core MVC
                            </li>

                            <li>
                                Experience with SQL Server and Entity Framework
                            </li>

                            <li>
                                Implement secure payment integration
                            </li>

                            <li>
                                Clean code and responsive UI
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
                            ₹8,000 - ₹15,000
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
                            20 May 2026
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