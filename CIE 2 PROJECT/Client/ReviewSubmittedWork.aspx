<%@ Page Title="Review Submitted Work" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReviewSubmittedWork.aspx.cs" Inherits="CIE_2_PROJECT.ReviewSubmittedWork" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/ReviewSubmittedWork.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="review-page">

        <div class="page-top">

            <div>
                <a href="MyProjects.aspx" class="back-link">
                    <i class="fa-solid fa-arrow-left"></i>
                    Back to Active Projects
                </a>

                <h1>Review Submitted Work</h1>

                <p>
                    Review the work submitted by the freelancer and take action.
                </p>
            </div>

            <div class="progress-steps">

                <div class="progress-step completed">
                    <div class="step-circle">1</div>
                    <span>Work Submitted</span>
                </div>

                <div class="step-line active"></div>

                <div class="progress-step current">
                    <div class="step-circle">2</div>
                    <span>Client Review</span>
                </div>

                <div class="step-line"></div>

                <div class="progress-step">
                    <div class="step-circle">3</div>
                    <span>Completed</span>
                </div>

            </div>

        </div>


        <div class="review-layout">

            <div class="review-main">

                <div class="submission-header">

                    <div class="freelancer-box">

                        <div class="freelancer-avatar">
                            XYZ
                        </div>

                        <div>
                            <span class="small-label">Freelancer</span>

                            <div class="freelancer-name-row">
                                <strong>xyz abc</strong>
                                <span class="rating">
                                    <i class="fa-solid fa-star"></i>
                                    4.8
                                </span>
                            </div>
                        </div>

                    </div>

                    <div class="submission-date">

                        <i class="fa-regular fa-calendar"></i>

                        <div>
                            <span class="small-label">Submitted On</span>
                            <strong>22 May 2026, 11:30 AM</strong>
                        </div>

                    </div>

                    <div class="submission-status">

                        <span class="small-label">Submission Status</span>

                        <span class="pending-badge">
                            Pending Review
                        </span>

                    </div>

                </div>


                <div class="content-card">

                    <div class="message-section">

                        <div class="section-icon">
                            <i class="fa-regular fa-comment-dots"></i>
                        </div>

                        <div class="message-box">

                            <h3>Message from Freelancer</h3>

                            <p>
                                I have completed all the required features mentioned
                                in the project. All functionalities have been tested.
                            </p>

                            <p>
                                Please review the work and let me know if any changes
                                are required.
                            </p>

                            <p>
                                Thank you!
                            </p>

                        </div>

                    </div>


                    <div class="content-divider"></div>


                    <div class="files-section">

                        <div class="section-heading">

                            <i class="fa-solid fa-paperclip"></i>

                            <div>
                                <h3>Submitted Files <span>(2)</span></h3>
                            </div>

                        </div>


                        <div class="file-list">

                            <div class="file-item">

                                <div class="file-icon zip">
                                    <i class="fa-regular fa-file-zipper"></i>
                                </div>

                                <div class="file-info">
                                    <strong>Project-Final.zip</strong>
                                    <span>18.4 MB</span>
                                </div>

                                <span class="file-date">
                                    22 May 2026, 11:30 AM
                                </span>

                                <asp:LinkButton
                                    ID="btnDownloadZip"
                                    runat="server"
                                    CssClass="download-button"
                                    OnClick="btnDownloadZip_Click">
                                    <i class="fa-solid fa-download"></i>
                                </asp:LinkButton>

                            </div>


                            <div class="file-item">

                                <div class="file-icon pdf">
                                    <i class="fa-regular fa-file-pdf"></i>
                                </div>

                                <div class="file-info">
                                    <strong>Documentation.pdf</strong>
                                    <span>2.7 MB</span>
                                </div>

                                <span class="file-date">
                                    22 May 2026, 11:30 AM
                                </span>

                                <asp:LinkButton
                                    ID="btnDownloadPdf"
                                    runat="server"
                                    CssClass="download-button"
                                    OnClick="btnDownloadPdf_Click">
                                    <i class="fa-solid fa-download"></i>
                                </asp:LinkButton>

                            </div>

                        </div>

                    </div>


                    <div class="content-divider"></div>


                    <div class="links-section">

                        <div class="section-heading">

                            <i class="fa-solid fa-link"></i>

                            <div>
                                <h3>Project Links</h3>
                            </div>

                        </div>


                        <div class="links-grid">

                            <div class="link-field">

                                <label>Live Demo / Project URL</label>

                                <div class="url-box">
                                    <asp:TextBox
                                        ID="txtLiveUrl"
                                        runat="server"
                                        Text="https://your-demo-link.com"
                                        CssClass="url-input">
                                    </asp:TextBox>

                                    <a href="https://your-demo-link.com" target="_blank">
                                        <i class="fa-solid fa-arrow-up-right-from-square"></i>
                                    </a>
                                </div>

                            </div>


                            <div class="link-field">

                                <label>GitHub / Repository URL</label>

                                <div class="url-box">
                                    <asp:TextBox
                                        ID="txtGithubUrl"
                                        runat="server"
                                        Text="https://github.com/username/repository"
                                        CssClass="url-input">
                                    </asp:TextBox>

                                    <a href="https://github.com" target="_blank">
                                        <i class="fa-solid fa-arrow-up-right-from-square"></i>
                                    </a>
                                </div>

                            </div>

                        </div>

                    </div>


                    <div class="content-divider"></div>


                    <div class="review-section">

                        <div class="section-heading">

                            <i class="fa-regular fa-comment"></i>

                            <div>
                                <h3>
                                    Add Review
                                    <span>(Optional)</span>
                                </h3>

                                <p>Write your feedback for the freelancer</p>
                            </div>

                        </div>


                        <asp:TextBox
                            ID="txtReview"
                            runat="server"
                            TextMode="MultiLine"
                            CssClass="review-textbox"
                            placeholder="Write your feedback, suggestions or any changes required...">
                        </asp:TextBox>

                        <div class="character-count">
                            0 / 2000
                        </div>

                    </div>


                    <div class="attachment-section">

                        <div class="section-heading">

                            <i class="fa-solid fa-paperclip"></i>

                            <div>
                                <h3>
                                    Attachment
                                    <span>(Optional)</span>
                                </h3>

                                <p>Upload reference files or screenshots if needed</p>
                            </div>

                        </div>


                        <div class="upload-row">

                            <asp:FileUpload
                                ID="fuAttachment"
                                runat="server"
                                CssClass="file-upload" />

                            <span class="upload-info">
                                PDF, DOC, DOCX, ZIP, PNG, JPG (Max 10MB)
                            </span>

                        </div>

                    </div>


                    <div class="action-area">

                        <asp:Button
                            ID="btnRequestRevision"
                            runat="server"
                            Text="Request Revision"
                            CssClass="revision-button"
                            OnClick="btnRequestRevision_Click" />

                        <asp:Button
                            ID="btnApproveWork"
                            runat="server"
                            Text="Approve Work"
                            CssClass="approve-button"
                            OnClick="btnApproveWork_Click" />

                    </div>


                    <asp:Label
                        ID="lblActionMessage"
                        runat="server"
                        CssClass="action-message">
                    </asp:Label>

                </div>


                <div class="information-box">

                    <i class="fa-solid fa-circle-info"></i>

                    <span>
                        Once you approve the work, the project will be marked as
                        completed and payment will be released.
                    </span>

                </div>

            </div>


            <aside class="review-sidebar">

                <div class="summary-card">

                    <h2>Project Summary</h2>

                    <div class="summary-project">

                        <div class="project-icon">
                            <i class="fa-solid fa-gamepad"></i>
                        </div>

                        <strong>
                            Unity Game Development for
                            2D Adventure Game
                        </strong>

                    </div>


                    <div class="summary-row">
                        <span>
                            <i class="fa-regular fa-user"></i>
                            Freelancer
                        </span>

                        <strong>xyz abc</strong>
                    </div>


                    <div class="summary-row">
                        <span>
                            <i class="fa-solid fa-sack-dollar"></i>
                            Agreed Price
                        </span>

                        <strong class="price">₹15,000</strong>
                    </div>


                    <div class="summary-row">
                        <span>
                            <i class="fa-regular fa-calendar"></i>
                            Deadline
                        </span>

                        <strong>30 May 2026</strong>
                    </div>


                    <div class="summary-row">
                        <span>
                            <i class="fa-regular fa-clock"></i>
                            Started On
                        </span>

                        <strong>18 May 2026</strong>
                    </div>


                    <div class="summary-row">
                        <span>
                            <i class="fa-solid fa-spinner"></i>
                            Current Status
                        </span>

                        <strong class="status-text">
                            In Progress
                        </strong>
                    </div>


                    <div class="progress-title">
                        <span>
                            <i class="fa-solid fa-list-check"></i>
                            Progress
                        </span>

                        <strong>100%</strong>
                    </div>

                    <div class="progress-bar">
                        <div class="progress-fill"></div>
                    </div>

                    <p class="progress-note">
                        All project tasks completed. Ready for review.
                    </p>

                </div>


                <div class="tasks-card">

                    <h2>Project Tasks</h2>

                    <div class="task-row">
                        <span>
                            <i class="fa-regular fa-circle-check"></i>
                            Game Setup & Player Movement
                        </span>
                        <strong>20%</strong>
                    </div>

                    <div class="task-row">
                        <span>
                            <i class="fa-regular fa-circle-check"></i>
                            Enemy & Combat System
                        </span>
                        <strong>25%</strong>
                    </div>

                    <div class="task-row">
                        <span>
                            <i class="fa-regular fa-circle-check"></i>
                            Levels & Progression
                        </span>
                        <strong>20%</strong>
                    </div>

                    <div class="task-row">
                        <span>
                            <i class="fa-regular fa-circle-check"></i>
                            UI & Sound Effects
                        </span>
                        <strong>15%</strong>
                    </div>

                    <div class="task-row">
                        <span>
                            <i class="fa-regular fa-circle-check"></i>
                            Testing & Final Delivery
                        </span>
                        <strong>20%</strong>
                    </div>

                    <div class="task-total">
                        <span>Total Progress</span>
                        <strong>100%</strong>
                    </div>

                </div>


                <div class="escrow-card">

                    <div class="escrow-icon">
                        <i class="fa-solid fa-shield-halved"></i>
                    </div>

                    <div>
                        <h3>Payment Held in Escrow</h3>

                        <p>
                            Your payment remains securely held until
                            you approve the submitted work.
                        </p>
                    </div>

                </div>

            </aside>

        </div>

    </div>

</asp:Content>