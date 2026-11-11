<%@ Page Title="Project Details" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="ProjectDetails.aspx.cs"
    Inherits="CIE_2_PROJECT.ProjectDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/ProjectDetails.css?v=3" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="details-page">

        <a href="FindProjects.aspx" class="back-link">
            <i class="fa-solid fa-arrow-left"></i>
            Back to Find Projects
        </a>

        <asp:Panel ID="pnlMessage" runat="server" Visible="false"
            CssClass="project-details-card">
            <h2>Project Unavailable</h2>
            <p class="detail-text">
                <asp:Literal ID="litMessage" runat="server" />
            </p>
        </asp:Panel>

        <asp:Panel ID="pnlProject" runat="server" Visible="false">

            <div class="details-layout">

                <div class="details-main">

                    <div class="project-details-card">

                        <h1 class="project-title">
                            <asp:Literal ID="litTitle" runat="server" />
                        </h1>

                        <div class="project-meta">
                            <span>
                                <i class="fa-regular fa-calendar"></i>
                                <asp:Literal ID="litPosted" runat="server" />
                            </span>

                            <span class="meta-dot">&#8226;</span>

                            <span>
                                <i class="fa-solid fa-folder"></i>
                                <asp:Literal ID="litCategory" runat="server" />
                            </span>

                            <span>
                                <i class="fa-regular fa-user"></i>
                                Posted by
                                <asp:HyperLink ID="lnkClientProfile" runat="server" />
                            </span>
                        </div>

                        <div class="section-divider"></div>

                        <section class="detail-section">
                            <h2 class="detail-heading">
                                <i class="fa-regular fa-file-lines"></i>
                                Project Description
                            </h2>

                            <p class="detail-text">
                                <asp:Literal ID="litDescription" runat="server" />
                            </p>
                        </section>

                        <section class="detail-section">
                            <h2 class="detail-heading">
                                <i class="fa-solid fa-code"></i>
                                Required Skills
                            </h2>

                            <div class="skill-list">
                                <asp:Literal ID="litSkills" runat="server" />
                            </div>
                        </section>

                        <section class="detail-section">
                            <h2 class="detail-heading">
                                <i class="fa-solid fa-chart-column"></i>
                                Experience Level
                            </h2>

                            <p class="detail-text">
                                <asp:Literal ID="litExperience" runat="server" />
                            </p>
                        </section>

                        <section class="detail-section">
                            <h2 class="detail-heading">
                                <i class="fa-solid fa-paperclip"></i>
                                Project Attachment
                            </h2>

                            <asp:HyperLink ID="lnkAttachment" runat="server"
                                CssClass="attachment-link"
                                Text="View project attachment"
                                Target="_blank"
                                Visible="false" />

                            <asp:Label ID="lblNoAttachment" runat="server"
                                CssClass="detail-text"
                                Text="No attachment available."
                                Visible="true" />
                        </section>

                    </div>

                    <div class="action-card">
                        <asp:HyperLink ID="lnkSubmitProposal" runat="server"
                            CssClass="proposal-button">
                            <i class="fa-regular fa-paper-plane"></i>
                            Submit Proposal
                        </asp:HyperLink>

                    </div>

                </div>

                <aside class="summary-card">

                    <h2 class="summary-title">Project Summary</h2>

                    <div class="summary-item">
                        <div class="summary-icon">
                            <i class="fa-solid fa-indian-rupee-sign"></i>
                        </div>
                        <div class="summary-content">
                            <div class="summary-label">Budget</div>
                            <div class="summary-value budget">
                                <asp:Literal ID="litBudget" runat="server" />
                            </div>
                        </div>
                    </div>

                    <div class="summary-item">
                        <div class="summary-icon">
                            <i class="fa-regular fa-calendar"></i>
                        </div>
                        <div class="summary-content">
                            <div class="summary-label">Deadline</div>
                            <div class="summary-value">
                                <asp:Literal ID="litDeadline" runat="server" />
                            </div>
                        </div>
                    </div>

                    <div class="summary-item">
                        <div class="summary-icon">
                            <i class="fa-solid fa-chart-column"></i>
                        </div>
                        <div class="summary-content">
                            <div class="summary-label">Experience Level</div>
                            <div class="summary-value">
                                <asp:Literal ID="litExperienceSummary" runat="server" />
                            </div>
                        </div>
                    </div>

                    <div class="summary-item">
                        <div class="summary-icon">
                            <i class="fa-regular fa-folder"></i>
                        </div>
                        <div class="summary-content">
                            <div class="summary-label">Number of Proposals</div>
                            <div class="summary-value">
                                <asp:Literal ID="litProposalCount" runat="server" />
                            </div>
                        </div>
                    </div>

                </aside>

            </div>

        </asp:Panel>

    </div>

</asp:Content>
