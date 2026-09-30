<%@ Page Title="Project Details" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="ProjectDetails.aspx.cs"
    Inherits="CIE_2_PROJECT.ClientProjectDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/ClientProjectDetails.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="project-details-page">

        <div class="page-top">
            <div>
                <a href="MyProjects.aspx" class="back-link">
                    <i class="fa-solid fa-arrow-left"></i>
                    Back to My Projects
                </a>

                <h1>Unity 2D Game Development</h1>

                <div class="project-meta">
                    <span>
                        <i class="fa-solid fa-layer-group"></i>
                        Game Development
                    </span>

                    <span>
                        <i class="fa-regular fa-calendar"></i>
                        Posted 12 May, 2026
                    </span>

                    <span>
                        <i class="fa-solid fa-users"></i>
                        12 Proposals
                    </span>
                </div>
            </div>

            <div class="status-badge">
                Open for Proposals
            </div>
        </div>


        <div class="details-layout">

            <div class="main-column">

                <section class="detail-card">

                    <div class="card-title">
                        <h2>Project Description</h2>
                    </div>

                    <p class="description">
                        We are looking for an experienced Unity developer to create
                        a 2D game with engaging gameplay mechanics, multiple levels,
                        player controls, scoring system and polished UI.
                    </p>

                    <p class="description">
                        The project should be well structured and optimized for
                        performance. The selected freelancer should be able to
                        communicate regularly and provide progress updates during
                        development.
                    </p>

                </section>


                <section class="detail-card">

                    <div class="card-title">
                        <h2>Required Skills</h2>
                    </div>

                    <div class="skills-list">
                        <span class="skill-tag">Unity</span>
                        <span class="skill-tag">C#</span>
                        <span class="skill-tag">2D Game Development</span>
                        <span class="skill-tag">Game Design</span>
                        <span class="skill-tag">Git</span>
                    </div>

                </section>


                <section class="detail-card">

                    <div class="card-title">
                        <h2>Project Requirements</h2>
                    </div>

                    <div class="requirements">

                        <div class="requirement-row">
                            <div class="requirement-label">
                                <i class="fa-solid fa-briefcase"></i>
                                Experience Required
                            </div>

                            <div class="requirement-value">
                                Intermediate
                            </div>
                        </div>

                        <div class="requirement-row">
                            <div class="requirement-label">
                                <i class="fa-solid fa-location-dot"></i>
                                Preferred Location
                            </div>

                            <div class="requirement-value">
                                Remote
                            </div>
                        </div>

                        <div class="requirement-row">
                            <div class="requirement-label">
                                <i class="fa-regular fa-clock"></i>
                                Project Deadline
                            </div>

                            <div class="requirement-value">
                                30 May, 2026
                            </div>
                        </div>

                        <div class="requirement-row">
                            <div class="requirement-label">
                                <i class="fa-solid fa-file-lines"></i>
                                Project Type
                            </div>

                            <div class="requirement-value">
                                Fixed Price
                            </div>
                        </div>

                    </div>

                </section>


                <section class="detail-card">

                    <div class="card-title">
                        <h2>Project Files</h2>
                    </div>

                    <div class="file-item">

                        <div class="file-icon">
                            <i class="fa-regular fa-file-pdf"></i>
                        </div>

                        <div class="file-info">
                            <div class="file-name">
                                Game_Project_Requirements.pdf
                            </div>

                            <div class="file-size">
                                2.4 MB
                            </div>
                        </div>

                        <asp:LinkButton
                            ID="btnDownloadFile"
                            runat="server"
                            CssClass="download-button"
                            OnClick="btnDownloadFile_Click">
                            <i class="fa-solid fa-download"></i>
                            Download
                        </asp:LinkButton>

                    </div>

                </section>


                <section class="detail-card">

                    <div class="card-title proposal-title">

                        <div>
                            <h2>Proposals</h2>
                            <p>Freelancers who have submitted proposals for this project.</p>
                        </div>

                        <span class="proposal-count">
                            12 Proposals
                        </span>

                    </div>

                    <div class="proposal-preview">

                        <div class="proposal-person">

                            <div class="person-avatar">
                                RS
                            </div>

                            <div>
                                <div class="person-name">
                                    Rahul Sharma
                                </div>

                                <div class="person-role">
                                    Unity Developer
                                </div>
                            </div>

                        </div>

                        <div class="proposal-info">
                            <span>
                                <i class="fa-solid fa-star"></i>
                                4.8
                            </span>

                            <span>
                                ₹20,000
                            </span>
                        </div>

                    </div>

                    <asp:Button
                        ID="btnViewProposals"
                        runat="server"
                        Text="View All Proposals"
                        CssClass="secondary-button"
                        OnClick="btnViewProposals_Click" />

                </section>

            </div>


            <aside class="side-column">

                <section class="summary-card">

                    <div class="summary-title">
                        Project Summary
                    </div>

                    <div class="summary-row">
                        <span>Budget</span>
                        <strong>₹15,000 - ₹25,000</strong>
                    </div>

                    <div class="summary-row">
                        <span>Proposals</span>
                        <strong>12</strong>
                    </div>

                    <div class="summary-row">
                        <span>Deadline</span>
                        <strong>30 May, 2026</strong>
                    </div>

                    <div class="summary-row">
                        <span>Experience</span>
                        <strong>Intermediate</strong>
                    </div>

                    <div class="summary-row">
                        <span>Status</span>
                        <strong class="summary-status">
                            Open for Proposals
                        </strong>
                    </div>

                </section>


                <section class="action-card">

                    <h3>Manage Project</h3>

                    <p>
                        Review proposals or update your project information.
                    </p>

                    <asp:Button
                        ID="btnProposals"
                        runat="server"
                        Text="View Proposals"
                        CssClass="primary-button"
                        OnClick="btnViewProposals_Click" />

                    <asp:Button
                        ID="btnEditProject"
                        runat="server"
                        Text="Edit Project"
                        CssClass="outline-button"
                        OnClick="btnEditProject_Click" />

                </section>


                <section class="info-card">

                    <div class="info-icon">
                        <i class="fa-solid fa-shield-halved"></i>
                    </div>

                    <div>
                        <h3>SkillLink Protection</h3>

                        <p>
                            Your project payment is held securely until
                            the submitted work is reviewed and approved.
                        </p>
                    </div>

                </section>

            </aside>

        </div>


        <asp:Label
            ID="lblActionMessage"
            runat="server"
            CssClass="action-message">
        </asp:Label>

    </div>

</asp:Content>