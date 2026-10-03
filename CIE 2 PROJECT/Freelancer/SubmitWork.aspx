<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SubmitWork.aspx.cs" Inherits="CIE_2_PROJECT.SubmitWork" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/SubmitWork.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="submit-work-page">

        <a href="ActiveWork.aspx" class="back-link">
            <i class="fa-solid fa-arrow-left"></i>
            Back to Active Work
        </a>

        <div class="submit-header">

            <div>
                <h1 class="page-title">Submit Work</h1>

                <p class="page-subtitle">
                    Submit your completed work for client review.
                </p>
            </div>

            <div class="step-progress">

                <div class="step-item step-active">
                    <div class="step-number">1</div>
                    <span>Submit Work</span>
                </div>

                <div class="step-line"></div>

                <div class="step-item">
                    <div class="step-number">2</div>
                    <span>Client Review</span>
                </div>

                <div class="step-line"></div>

                <div class="step-item">
                    <div class="step-number">3</div>
                    <span>Completed</span>
                </div>

            </div>

        </div>


        <div class="submit-layout">

            <div class="submit-main">

                <div class="submit-card">

                    <div class="form-section">

                        <div class="section-heading">

                            <i class="fa-regular fa-message"></i>

                            <div>
                                <h2>1. Project Completion Message</h2>

                                <p>
                                    Let the client know what you have completed and any important details.
                                </p>
                            </div>

                        </div>

                        <asp:TextBox
                            ID="txtCompletionMessage"
                            runat="server"
                            TextMode="MultiLine"
                            CssClass="completion-message"
                            MaxLength="2000"
                            placeholder="Write a message for the client...">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="rfvCompletionMessage"
                            runat="server"
                            ControlToValidate="txtCompletionMessage"
                            ErrorMessage="Please enter a completion message."
                            CssClass="validation-error"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>

                        <asp:CustomValidator
                            ID="cvCompletionMessage"
                            runat="server"
                            ControlToValidate="txtCompletionMessage"
                            OnServerValidate="cvCompletionMessage_ServerValidate"
                            EnableClientScript="false"
                            ErrorMessage="Completion message must be between 20 and 2000 characters."
                            CssClass="validation-error"
                            Display="Dynamic">
                        </asp:CustomValidator>

                        <div class="character-limit">
                            0 / 2000
                        </div>

                    </div>


                    <div class="form-section">

                        <div class="section-heading">

                            <i class="fa-solid fa-cloud-arrow-up"></i>

                            <div>
                                <h2>2. Upload Completed Work</h2>

                                <p>
                                    Upload all files related to your completed work.
                                </p>
                            </div>

                        </div>

                        <div class="upload-area">

                            <i class="fa-solid fa-cloud-arrow-up upload-icon"></i>

                            <div class="upload-title">
                                Select files to upload
                            </div>

                            <div class="upload-or">
                                or
                            </div>

                            <asp:FileUpload
                                ID="fuCompletedWork"
                                runat="server"
                                AllowMultiple="true"
                                CssClass="file-upload" />

                        </div>

                        <asp:CustomValidator
                            ID="cvCompletedWork"
                            runat="server"
                            OnServerValidate="cvCompletedWork_ServerValidate"
                            EnableClientScript="false"
                            ErrorMessage="Please upload valid files. Each file must be 10 MB or less."
                            CssClass="validation-error"
                            Display="Dynamic">
                        </asp:CustomValidator>

                        <p class="upload-note">
                            You can upload multiple files (ZIP, PDF, DOC, DOCX, Images, etc.)
                        </p>


                        <div class="uploaded-heading">
                            Uploaded Files (2)
                        </div>


                        <div class="uploaded-files">

                            <div class="uploaded-file">

                                <div class="file-left">

                                    <div class="file-icon zip-icon">
                                        <i class="fa-solid fa-file-zipper"></i>
                                    </div>

                                    <div class="file-details">
                                        <span class="file-name">
                                            Project-Final.zip
                                        </span>

                                        <span class="file-size">
                                            18.4 MB
                                        </span>
                                    </div>

                                </div>

                                <span class="file-date">
                                    22 May 2026, 11:30 AM
                                </span>

                                <i class="fa-regular fa-trash-can delete-file"></i>

                            </div>


                            <div class="uploaded-file">

                                <div class="file-left">

                                    <div class="file-icon pdf-icon">
                                        <i class="fa-regular fa-file-pdf"></i>
                                    </div>

                                    <div class="file-details">
                                        <span class="file-name">
                                            Documentation.pdf
                                        </span>

                                        <span class="file-size">
                                            2.7 MB
                                        </span>
                                    </div>

                                </div>

                                <span class="file-date">
                                    22 May 2026, 11:30 AM
                                </span>

                                <i class="fa-regular fa-trash-can delete-file"></i>

                            </div>

                        </div>

                    </div>


                    <div class="form-section links-section">

                        <div class="section-heading">

                            <i class="fa-solid fa-link"></i>

                            <div>
                                <h2>
                                    3. Project Links
                                    <span>(Optional)</span>
                                </h2>
                            </div>

                        </div>

                        <div class="links-grid">

                            <div class="input-group">

                                <label for="txtDemoLink">
                                    Live Demo / Project URL
                                </label>

                                <asp:TextBox
                                    ID="txtDemoLink"
                                    runat="server"
                                    CssClass="form-input"
                                    placeholder="https://your-demo-link.com">
                                </asp:TextBox>

                                <asp:CustomValidator
                                    ID="cvDemoLink"
                                    runat="server"
                                    ControlToValidate="txtDemoLink"
                                    OnServerValidate="cvDemoLink_ServerValidate"
                                    EnableClientScript="false"
                                    ErrorMessage="Please enter a valid URL."
                                    CssClass="validation-error"
                                    Display="Dynamic">
                                </asp:CustomValidator>

                            </div>


                            <div class="input-group">

                                <label for="txtGithubLink">
                                    GitHub / Repository URL
                                </label>

                                <asp:TextBox
                                    ID="txtGithubLink"
                                    runat="server"
                                    CssClass="form-input"
                                    placeholder="https://github.com/username/repository">
                                </asp:TextBox>

                                <asp:CustomValidator
                                    ID="cvGithubLink"
                                    runat="server"
                                    ControlToValidate="txtGithubLink"
                                    OnServerValidate="cvGithubLink_ServerValidate"
                                    EnableClientScript="false"
                                    ErrorMessage="Please enter a valid GitHub URL."
                                    CssClass="validation-error"
                                    Display="Dynamic">
                                </asp:CustomValidator>

                            </div>

                        </div>

                    </div>


                    <div class="submission-warning">

                        <i class="fa-solid fa-circle-info"></i>

                        <div>
                            <strong>
                                Once submitted, you won't be able to edit the submission.
                            </strong>

                            <span>
                                The client will review your work and may approve it or request revisions.
                            </span>
                        </div>

                    </div>


                    <div class="form-actions">

                        <a href="ActiveWork.aspx" class="cancel-button">
                            Cancel
                        </a>

                        <asp:Button
                            ID="btnSubmitWork"
                            runat="server"
                            Text="Submit for Review"
                            CssClass="submit-button"
                            OnClick="btnSubmitWork_Click" />

                    </div>

                </div>

            </div>


            <aside class="submit-sidebar">

                <div class="sidebar-card">

                    <h2>Project Summary</h2>

                    <div class="summary-project">

                        <div class="summary-project-icon">
                            <i class="fa-solid fa-gamepad"></i>
                        </div>

                        <h3>
                            Unity Game Development for<br />
                            2D Adventure Game
                        </h3>

                    </div>


                    <div class="summary-row">

                        <span>
                            <i class="fa-regular fa-user"></i>
                            Client
                        </span>

                        <strong>
                            Game Studio
                        </strong>

                    </div>


                    <div class="summary-row">

                        <span>
                            <i class="fa-solid fa-tag"></i>
                            Agreed Price
                        </span>

                        <strong class="summary-price">
                            &#8377;9,000
                        </strong>

                    </div>


                    <div class="summary-row">

                        <span>
                            <i class="fa-regular fa-calendar"></i>
                            Deadline
                        </span>

                        <strong>
                            30 May 2026
                        </strong>

                    </div>


                    <div class="summary-row">

                        <span>
                            <i class="fa-regular fa-clock"></i>
                            Started On
                        </span>

                        <strong>
                            18 May 2026
                        </strong>

                    </div>


                    <div class="summary-row">

                        <span>
                            <i class="fa-regular fa-circle"></i>
                            Project Status
                        </span>

                        <span class="status-pill">
                            In Progress
                        </span>

                    </div>

                </div>


                <div class="sidebar-card progress-card">

                    <div class="progress-header">

                        <div>
                            <h2>Progress Tracking</h2>

                            <p>
                                Update the progress of your work
                            </p>
                        </div>

                        <strong>
                            60%
                        </strong>

                    </div>

                    <div class="large-progress-track">
                        <div class="large-progress-fill"></div>
                    </div>

                    <div class="progress-labels">
                        <span>60% Completed</span>
                        <span>40% Remaining</span>
                    </div>

                    <div class="progress-info">
                        <i class="fa-solid fa-circle-info"></i>
                        Increase progress as you complete more of the work.
                    </div>

                </div>


                <div class="sidebar-card guidelines-card">

                    <h2>Submission Guidelines</h2>

                    <div class="guideline">
                        <i class="fa-regular fa-circle-check"></i>
                        <span>Make sure all requirements are completed</span>
                    </div>

                    <div class="guideline">
                        <i class="fa-regular fa-circle-check"></i>
                        <span>Test your work before submission</span>
                    </div>

                    <div class="guideline">
                        <i class="fa-regular fa-circle-check"></i>
                        <span>Provide documentation if required</span>
                    </div>

                    <div class="guideline">
                        <i class="fa-regular fa-circle-check"></i>
                        <span>Include demo link if available</span>
                    </div>

                    <div class="guideline">
                        <i class="fa-regular fa-circle-check"></i>
                        <span>Client will review within 2-3 days</span>
                    </div>

                </div>

            </aside>

        </div>

    </div>

</asp:Content>