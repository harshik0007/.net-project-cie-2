
<%@ Page Title="Submit Proposal" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="SubmitProposal.aspx.cs"
    Inherits="CIE_2_PROJECT.SubmitProposal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Content/Css/SubmitProposal.css") %>" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="proposal-page">

        <asp:Panel ID="pnlUnavailable" runat="server"
            CssClass="proposal-card" Visible="false">

            <h2>Project Unavailable</h2>

            <p class="proposal-description">
                <asp:Literal ID="litUnavailableMessage" runat="server" />
            </p>

            <a href="FindProjects.aspx" class="back-link">
                <i class="fa-solid fa-arrow-left"></i>
                Back to Find Projects
            </a>

        </asp:Panel>

        <asp:Panel ID="pnlProposalForm" runat="server" Visible="false">

            <asp:HyperLink ID="backToDetails" runat="server"
                CssClass="back-link">
                <i class="fa-solid fa-arrow-left"></i>
                Back to Project Details
            </asp:HyperLink>

            <!-- Project summary -->
            <div class="project-summary-card">

                <div class="project-summary-content">

                    <h1>
                        <asp:Literal ID="litProjectTitle" runat="server" />
                    </h1>

                    <div class="project-meta">

                        <span>
                            <i class="fa-regular fa-calendar"></i>
                            <asp:Literal ID="litProjectPosted" runat="server" />
                        </span>

                        <span class="meta-dot">&#8226;</span>

                        <span>
                            <i class="fa-solid fa-folder"></i>
                            <asp:Literal ID="litProjectCategory" runat="server" />
                        </span>

                    </div>

                    <p>
                        <asp:Literal ID="litProjectDescription" runat="server" />
                    </p>

                </div>

                <div class="summary-side">

                    <asp:HyperLink ID="lnkViewDetails" runat="server"
                        CssClass="view-details-button">
                        <i class="fa-regular fa-eye"></i>
                        View Project Details
                    </asp:HyperLink>

                    <div class="budget-box">
                        <span class="budget-label">Project Budget</span>
                        <span class="budget-value">
                            <asp:Literal ID="litProjectBudget" runat="server" />
                        </span>
                    </div>

                </div>

            </div>

            <!-- Proposal form -->
            <div class="proposal-card">

                <h2 class="proposal-title">Your Proposal</h2>

                <p class="proposal-description">
                    Explain your approach, experience, price, and estimated
                    time to complete this project.
                </p>

                <asp:ValidationSummary ID="vsProposal" runat="server"
                    CssClass="validation-error"
                    HeaderText="Please correct the following:"
                    DisplayMode="BulletList"
                    ValidationGroup="Proposal"
                    EnableClientScript="false" />

                <!-- Cover letter -->
                <div class="form-group">

                    <label class="form-label"
                        for="<%= txtCoverLetter.ClientID %>">
                        Cover Letter / Proposal
                        <span class="required">*</span>
                    </label>

                    <asp:TextBox ID="txtCoverLetter" runat="server"
                        CssClass="cover-letter"
                        TextMode="MultiLine"
                        MaxLength="1000"
                        placeholder="Introduce yourself and explain why you are the best fit for this project." />

                    <asp:RequiredFieldValidator ID="rfvCoverLetter"
                        runat="server"
                        ControlToValidate="txtCoverLetter"
                        ErrorMessage="Cover letter is required."
                        CssClass="validation-error"
                        Display="Dynamic"
                        ValidationGroup="Proposal"
                        EnableClientScript="false" />

                    <asp:CustomValidator ID="cvCoverLetter"
                        runat="server"
                        ControlToValidate="txtCoverLetter"
                        ErrorMessage="Cover letter must contain 50 to 1000 characters."
                        CssClass="validation-error"
                        Display="Dynamic"
                        ValidationGroup="Proposal"
                        EnableClientScript="false"
                        OnServerValidate="cvCoverLetter_ServerValidate" />

                    <div class="character-count">
                        Write between 50 and 1000 characters.
                    </div>

                </div>

                <!-- Price and delivery time -->
                <div class="two-column">

                    <div class="form-group">

                        <label class="form-label"
                            for="<%= txtProposedPrice.ClientID %>">
                            Your Proposed Price (&#8377;)
                            <span class="required">*</span>
                        </label>

                        <asp:TextBox ID="txtProposedPrice" runat="server"
                            CssClass="input-box"
                            TextMode="Number"
                            placeholder="Enter your proposed price" />

                        <asp:RequiredFieldValidator ID="rfvProposedPrice"
                            runat="server"
                            ControlToValidate="txtProposedPrice"
                            ErrorMessage="Proposed price is required."
                            CssClass="validation-error"
                            Display="Dynamic"
                            ValidationGroup="Proposal"
                            EnableClientScript="false" />

                        <asp:CustomValidator ID="cvProposedPrice"
                            runat="server"
                            ControlToValidate="txtProposedPrice"
                            ErrorMessage="Enter a price within the project budget."
                            CssClass="validation-error"
                            Display="Dynamic"
                            ValidationGroup="Proposal"
                            EnableClientScript="false"
                            OnServerValidate="cvProposedPrice_ServerValidate" />

                        <div class="helper-text">
                            Your price must be within the project's budget.
                        </div>

                    </div>

                    <div class="form-group">

                        <label class="form-label"
                            for="<%= ddlDeliveryTime.ClientID %>">
                            Estimated Delivery Time
                            <span class="required">*</span>
                        </label>

                        <asp:DropDownList ID="ddlDeliveryTime" runat="server"
                            CssClass="input-box">

                            <asp:ListItem Text="Select delivery time" Value="" />
                            <asp:ListItem Text="1 - 3 days" Value="1-3" />
                            <asp:ListItem Text="4 - 7 days" Value="4-7" />
                            <asp:ListItem Text="1 - 2 weeks" Value="1-2-weeks" />
                            <asp:ListItem Text="2 - 4 weeks" Value="2-4-weeks" />
                            <asp:ListItem Text="More than 4 weeks" Value="4-plus-weeks" />

                        </asp:DropDownList>

                        <asp:RequiredFieldValidator ID="rfvDeliveryTime"
                            runat="server"
                            ControlToValidate="ddlDeliveryTime"
                            InitialValue=""
                            ErrorMessage="Please select an estimated delivery time."
                            CssClass="validation-error"
                            Display="Dynamic"
                            ValidationGroup="Proposal"
                            EnableClientScript="false" />

                        <div class="helper-text">
                            Choose the time required to complete the project.
                        </div>

                    </div>

                </div>

                <!-- Attachment upload -->
                <div class="form-group attachment-group">

                    <label class="form-label">
                        Attachments
                        <span class="optional">(Optional)</span>
                    </label>

                    <div class="attachment-description">
                        Upload your portfolio, document, or relevant work sample.
                    </div>

                    <div class="simple-upload-box">

                        <div class="simple-upload-heading">
                            <i class="fa-solid fa-cloud-arrow-up"></i>

                            <div>
                                <div class="upload-title">
                                    Select your attachment
                                </div>

                                <div class="upload-info">
                                    Choose a file from your computer.
                                </div>
                            </div>
                        </div>

                        <!-- Visible native file input displays selected filename -->
                        <asp:FileUpload ID="fuAttachment" runat="server"
                            CssClass="native-file-upload"
                            accept=".pdf,.doc,.docx,.png,.jpg,.jpeg"
                            ToolTip="Select an attachment" />

                        <div class="upload-rules">
                            Allowed: PDF, DOC, DOCX, PNG, JPG, JPEG
                            <span>Maximum size: 5 MB</span>
                        </div>

                    </div>

                    <asp:CustomValidator ID="cvAttachment"
                        runat="server"
                        ErrorMessage="Invalid attachment. Allowed types: PDF, DOC, DOCX, PNG, JPG or JPEG. Maximum size is 5 MB."
                        CssClass="validation-error"
                        Display="Dynamic"
                        ValidationGroup="Proposal"
                        EnableClientScript="false"
                        OnServerValidate="cvAttachment_ServerValidate" />

                </div>

                <!-- Information -->
                <div class="note-box">

                    <i class="fa-solid fa-circle-info"></i>

                    <span>
                        <strong>Note:</strong>
                        Make your proposal clear and professional.
                        A good proposal helps clients understand your offer.
                    </span>

                </div>

                <asp:Label ID="lblSubmitMessage" runat="server"
                    CssClass="validation-error"
                    Visible="false" />

                <div class="proposal-divider"></div>

                <div class="proposal-actions">

                    <asp:HyperLink ID="lnkCancel" runat="server"
                        CssClass="cancel-button">
                        Cancel
                    </asp:HyperLink>

                    <asp:Button ID="btnSubmitProposal" runat="server"
                        Text="Submit Proposal"
                        CssClass="submit-button"
                        ValidationGroup="Proposal"
                        OnClick="btnSubmitProposal_Click" />

                </div>

            </div>

        </asp:Panel>

    </div>

</asp:Content>
