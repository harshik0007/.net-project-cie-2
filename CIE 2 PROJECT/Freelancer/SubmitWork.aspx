
<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SubmitWork.aspx.cs" Inherits="CIE_2_PROJECT.SubmitWork" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/SubmitWork.css?v=4" />
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

        <asp:Label
            ID="lblSubmitMessage"
            runat="server"
            CssClass="validation-error"
            EnableViewState="false"
            Visible="false">
        </asp:Label>

        <div class="submit-layout">

            <div class="submit-main">

                <div class="submit-card">

                    <!-- COMPLETION MESSAGE -->
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
                            Maximum 2000 characters
                        </div>

                    </div>

                    <!-- FILE UPLOAD -->
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
                                Upload your completed project files
                            </div>

                            <asp:FileUpload
                                ID="fuCompletedWork"
                                runat="server"
                                AllowMultiple="true"
                                CssClass="file-upload" />

                            <asp:Button ID="btnAddFiles" runat="server"
                                Text="Add Selected Files"
                                CssClass="add-files-button"
                                CausesValidation="false"
                                OnClick="btnAddFiles_Click" />

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
                            Select up to 10 files, then click Add Selected Files. ZIP, PDF, DOC, DOCX, PNG, JPG and JPEG are allowed, up to 10 MB each.
                        </p>

                        <asp:Panel ID="pnlUploadedFiles" runat="server"
                            CssClass="uploaded-list-panel" Visible="false">
                            <div class="uploaded-heading">
                                <asp:Label ID="lblUploadedFileCount" runat="server" />
                            </div>
                            <div class="uploaded-files">
                                <asp:Repeater ID="rptUploadedFiles" runat="server"
                                    OnItemCommand="rptUploadedFiles_ItemCommand">
                                    <ItemTemplate>
                                        <div class="uploaded-file">
                                            <div class="file-left">
                                                <span class="file-icon pdf-icon">
                                                    <i class="fa-regular fa-file"></i>
                                                </span>
                                                <div class="file-details">
                                                    <strong class="file-name"><%#: Eval("OriginalFileName") %></strong>
                                                    <span class="file-size"><%#: Eval("SizeDisplay") %></span>
                                                </div>
                                            </div>
                                            <asp:LinkButton ID="btnRemoveFile" runat="server"
                                                CssClass="delete-file"
                                                Text="Remove"
                                                CommandName="RemoveFile"
                                                CommandArgument='<%# Eval("StoredFileName") %>'
                                                CausesValidation="false" />
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="pnlRevisionFeedback" runat="server"
                            Visible="false" CssClass="revision-feedback">
                            <strong>Client revision feedback</strong>
                            <asp:Label ID="lblRevisionFeedback" runat="server" />
                        </asp:Panel>

                    </div>

                    <!-- PROJECT LINKS -->
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
                                    MaxLength="500"
                                    placeholder="https://your-demo-link.com">
                                </asp:TextBox>

                                <asp:CustomValidator
                                    ID="cvDemoLink"
                                    runat="server"
                                    ControlToValidate="txtDemoLink"
                                    OnServerValidate="cvDemoLink_ServerValidate"
                                    EnableClientScript="false"
                                    ErrorMessage="Please enter a valid HTTP or HTTPS URL."
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
                                    MaxLength="500"
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

                    <!-- WARNING -->
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

                    <!-- BUTTONS -->
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

            <!-- PROJECT SIDEBAR -->
            <aside class="submit-sidebar">

                <div class="sidebar-card">

                    <h2>Project Summary</h2>

                    <div class="summary-project">

                        <div class="summary-project-icon">
                            <i class="fa-solid fa-gamepad"></i>
                        </div>

                        <h3>
                            <asp:Literal
                                ID="litProjectTitle"
                                runat="server" />
                        </h3>

                    </div>

                    <div class="summary-row">

                        <span>
                            <i class="fa-regular fa-user"></i>
                            Client
                        </span>

                        <strong>
                            <asp:HyperLink ID="lnkClientProfile" runat="server" />
                        </strong>

                    </div>

                    <asp:HyperLink ID="lnkMessageClient" runat="server" CssClass="summary-message-button">
                        <i class="fa-regular fa-comments"></i> Message client
                    </asp:HyperLink>

                    <div class="summary-row">

                        <span>
                            <i class="fa-solid fa-tag"></i>
                            Contract Amount
                        </span>

                        <strong class="summary-price">
                            <asp:Literal
                                ID="litAgreedPrice"
                                runat="server" />
                        </strong>

                    </div>

                    <div class="summary-row">
                        <span><i class="fa-solid fa-percent"></i> Freelancer platform fee (<asp:Literal ID="litFreelancerFeeRate" runat="server" />%)</span>
                        <strong><asp:Literal ID="litFreelancerFee" runat="server" /></strong>
                    </div>

                    <div class="summary-row">
                        <span><i class="fa-solid fa-wallet"></i> Your payout after fee</span>
                        <strong class="summary-price"><asp:Literal ID="litFreelancerNet" runat="server" /></strong>
                    </div>

                    <div class="summary-row">

                        <span>
                            <i class="fa-regular fa-calendar"></i>
                            Deadline
                        </span>

                        <strong>
                            <asp:Literal
                                ID="litDeadline"
                                runat="server" />
                        </strong>

                    </div>

                    <div class="summary-row">

                        <span>
                            <i class="fa-regular fa-clock"></i>
                            Started On
                        </span>

                        <strong>
                            <asp:Literal
                                ID="litStartedOn"
                                runat="server" />
                        </strong>

                    </div>

                    <div class="summary-row">

                        <span>
                            <i class="fa-regular fa-circle"></i>
                            Project Status
                        </span>

                        <span class="status-pill">
                            <asp:Literal
                                ID="litProjectStatus"
                                runat="server" />
                        </span>

                    </div>

                </div>

                <!-- PROGRESS TRACKING -->
                <div class="sidebar-card progress-card">

                    <div class="progress-header">
                        <div>
                            <h2>Submission Progress</h2>
                            <p>
                                Your work will be reviewed by the client.
                            </p>
                        </div>
                    </div>

                    <div class="progress-info">
                        <i class="fa-solid fa-circle-info"></i>
                        Submit your completed files to begin the client review process.
                    </div>

                </div>

                <!-- GUIDELINES -->
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
                        <span>Include a demo link if available</span>
                    </div>

                    <div class="guideline">
                        <i class="fa-regular fa-circle-check"></i>
                        <span>Wait for the client's review</span>
                    </div>

                </div>

            </aside>

        </div>

    </div>

</asp:Content>
