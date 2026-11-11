
<%@ Page Title="Project Details" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="ProjectDetails.aspx.cs"
    Inherits="CIE_2_PROJECT.ClientProjectDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/ClientProjectDetails.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Label
        ID="lblPageMessage"
        runat="server"
        CssClass="action-message"
        Visible="false">
    </asp:Label>

    <asp:Panel ID="pnlProjectDetails" runat="server">

        <div class="project-details-page">

            <div class="page-top">

                <div>
                    <a href="MyProjects.aspx" class="back-link">
                        <i class="fa-solid fa-arrow-left"></i>
                        Back to My Projects
                    </a>

                    <h1>
                        <asp:Label ID="lblProjectTitle" runat="server" />
                    </h1>

                    <div class="project-meta">

                        <span>
                            <i class="fa-solid fa-layer-group"></i>
                            <asp:Label ID="lblCategory" runat="server" />
                        </span>

                        <span>
                            <i class="fa-regular fa-calendar"></i>
                            <asp:Label ID="lblCreatedAt" runat="server" />
                        </span>

                        <span>
                            <i class="fa-solid fa-users"></i>
                            <asp:Label ID="lblProposalCountTop" runat="server" />
                        </span>

                    </div>
                </div>

                <span class="status-badge">
                    <asp:Label ID="lblStatusTop" runat="server" />
                </span>

            </div>

            <div class="details-layout">

                <div class="main-column">

                    <section class="detail-card">

                        <div class="card-title">
                            <h2>Project Description</h2>
                        </div>

                        <p class="description">
                            <asp:Label ID="lblDescription" runat="server" />
                        </p>

                    </section>

                    <section class="detail-card">

                        <div class="card-title">
                            <h2>Required Skills</h2>
                        </div>

                        <div class="skills-list">
                            <asp:Label ID="lblSkills" runat="server" />
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
                                    <asp:Label ID="lblExperience" runat="server" />
                                </div>
                            </div>

                            <div class="requirement-row">
                                <div class="requirement-label">
                                    <i class="fa-solid fa-location-dot"></i>
                                    Preferred Location
                                </div>
                                <div class="requirement-value">
                                    <asp:Label ID="lblLocation" runat="server" />
                                </div>
                            </div>

                            <div class="requirement-row">
                                <div class="requirement-label">
                                    <i class="fa-regular fa-clock"></i>
                                    Project Deadline
                                </div>
                                <div class="requirement-value">
                                    <asp:Label ID="lblDeadline" runat="server" />
                                </div>
                            </div>

                            <div class="requirement-row">
                                <div class="requirement-label">
                                    <i class="fa-solid fa-indian-rupee-sign"></i>
                                    Project Budget
                                </div>
                                <div class="requirement-value">
                                    <asp:Label ID="lblBudget" runat="server" />
                                </div>
                            </div>

                        </div>

                    </section>

                    <section class="detail-card">

                        <div class="card-title">
                            <h2>Project Files</h2>
                        </div>

                        <asp:Panel
                            ID="pnlAttachment"
                            runat="server"
                            CssClass="file-item"
                            Visible="false">

                            <div class="file-icon">
                                <i class="fa-solid fa-file-lines"></i>
                            </div>

                            <div class="file-info">
                                <div class="file-name">
                                    <asp:Label
                                        ID="lblAttachmentName"
                                        runat="server" />
                                </div>

                                <div class="file-size">
                                    Project attachment
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

                        </asp:Panel>

                        <asp:Panel
                            ID="pnlNoAttachment"
                            runat="server"
                            Visible="false">

                            <p class="description">
                                No attachment was uploaded for this project.
                            </p>

                        </asp:Panel>

                    </section>

                    <section class="detail-card">

                        <div class="card-title proposal-title">
                            <div>
                                <h2>Proposals</h2>
                                <p>
                                    Freelancers who have submitted proposals
                                    for this project.
                                </p>
                            </div>

                            <span class="proposal-count">
                                <asp:Label
                                    ID="lblProposalCountCard"
                                    runat="server" />
                            </span>
                        </div>

                        <p class="description">
                            <asp:Label
                                ID="lblProposalMessage"
                                runat="server" />
                        </p>

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
                            <strong>
                                <asp:Label ID="lblBudgetSummary" runat="server" />
                            </strong>
                        </div>

                        <div class="summary-row">
                            <span>Proposals</span>
                            <strong>
                                <asp:Label ID="lblProposalCountSummary" runat="server" />
                            </strong>
                        </div>

                        <div class="summary-row">
                            <span>Deadline</span>
                            <strong>
                                <asp:Label ID="lblDeadlineSummary" runat="server" />
                            </strong>
                        </div>

                        <div class="summary-row">
                            <span>Experience</span>
                            <strong>
                                <asp:Label ID="lblExperienceSummary" runat="server" />
                            </strong>
                        </div>

                        <div class="summary-row">
                            <span>Status</span>
                            <strong class="summary-status">
                                <asp:Label ID="lblStatusSummary" runat="server" />
                            </strong>
                        </div>

                    </section>

                    <section class="action-card">

                        <h3>Manage Project</h3>

                        <p>
                            <asp:Label ID="lblManageMessage" runat="server" />
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

    </asp:Panel>

</asp:Content>
