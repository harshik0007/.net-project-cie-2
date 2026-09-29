<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SubmitProposal.aspx.cs" Inherits="CIE_2_PROJECT.SubmitProposal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/SubmitProposal.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="proposal-page">

        <a href="ProjectDetails.aspx" class="back-link">
            <i class="fa-solid fa-arrow-left"></i>
            Back to Project Details
        </a>

        <div class="project-summary-card">

            <div class="project-summary-content">

                <h1>Build a .NET Core E-commerce Website</h1>

                <div class="project-meta">

                    <span>
                        <i class="fa-regular fa-calendar"></i>
                        Posted on 20 May 2026
                    </span>

                    <span class="meta-dot">•</span>

                    <span>
                        <i class="fa-regular fa-user"></i>
                        Client:
                        <a href="#">John Doe</a>
                    </span>

                </div>

                <p>
                    We need a complete e-commerce solution using ASP.NET Core
                    with user authentication, product management, shopping cart,
                    order management, and payment integration.
                </p>

            </div>

            <div class="summary-side">

                <a href="ProjectDetails.aspx" class="view-details-button">
                    <i class="fa-regular fa-eye"></i>
                    View Project Details
                </a>

                <div class="budget-box">
                    <span class="budget-label">Budget</span>
                    <span class="budget-value">₹8,000 - ₹15,000</span>
                </div>

            </div>

        </div>

        <div class="proposal-card">

            <h2 class="proposal-title">
                Your Proposal
            </h2>

            <p class="proposal-description">
                Provide details about your approach, experience and why you are
                the best fit for this project.
            </p>

            <div class="form-group">

                <label class="form-label">
                    Cover Letter / Proposal
                    <span class="required">*</span>
                </label>

                <asp:TextBox
                    ID="txtCoverLetter"
                    runat="server"
                    CssClass="cover-letter"
                    TextMode="MultiLine"
                    MaxLength="1000"
                    placeholder="Introduce yourself and explain why you are the best fit for this project.">
                </asp:TextBox>

                <asp:RequiredFieldValidator
                    ID="rfvCoverLetter"
                    runat="server"
                    ControlToValidate="txtCoverLetter"
                    ErrorMessage="Cover letter is required."
                    CssClass="validation-error"
                    Display="Dynamic"
                    EnableClientScript="false">
                </asp:RequiredFieldValidator>

                <asp:CustomValidator
                    ID="cvCoverLetter"
                    runat="server"
                    ControlToValidate="txtCoverLetter"
                    ErrorMessage="Cover letter must contain at least 50 characters."
                    CssClass="validation-error"
                    Display="Dynamic"
                    EnableClientScript="false"
                    OnServerValidate="cvCoverLetter_ServerValidate">
                </asp:CustomValidator>

                <div class="character-count">
                    0/1000
                </div>

            </div>

            <div class="two-column">

                <div class="form-group">

                    <label class="form-label">
                        Your Proposed Price (₹)
                        <span class="required">*</span>
                    </label>

                    <asp:TextBox
                        ID="txtProposedPrice"
                        runat="server"
                        CssClass="input-box"
                        TextMode="Number"
                        placeholder="Enter your proposed price">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvProposedPrice"
                        runat="server"
                        ControlToValidate="txtProposedPrice"
                        ErrorMessage="Proposed price is required."
                        CssClass="validation-error"
                        Display="Dynamic"
                        EnableClientScript="false">
                    </asp:RequiredFieldValidator>

                    <asp:CustomValidator
                        ID="cvProposedPrice"
                        runat="server"
                        ControlToValidate="txtProposedPrice"
                        ErrorMessage="Enter a valid price between ₹8,000 and ₹15,000."
                        CssClass="validation-error"
                        Display="Dynamic"
                        EnableClientScript="false"
                        OnServerValidate="cvProposedPrice_ServerValidate">
                    </asp:CustomValidator>

                    <div class="helper-text">
                        Enter amount between ₹8,000 - ₹15,000
                    </div>

                </div>

                <div class="form-group">

                    <label class="form-label">
                        Estimated Delivery Time
                        <span class="required">*</span>
                    </label>

                    <asp:DropDownList
                        ID="ddlDeliveryTime"
                        runat="server"
                        CssClass="input-box">

                        <asp:ListItem
                            Text="Select delivery time"
                            Value="">
                        </asp:ListItem>

                        <asp:ListItem
                            Text="1 - 3 days"
                            Value="1-3">
                        </asp:ListItem>

                        <asp:ListItem
                            Text="4 - 7 days"
                            Value="4-7">
                        </asp:ListItem>

                        <asp:ListItem
                            Text="1 - 2 weeks"
                            Value="1-2-weeks">
                        </asp:ListItem>

                        <asp:ListItem
                            Text="2 - 4 weeks"
                            Value="2-4-weeks">
                        </asp:ListItem>

                        <asp:ListItem
                            Text="More than 4 weeks"
                            Value="4-plus-weeks">
                        </asp:ListItem>

                    </asp:DropDownList>

                    <asp:RequiredFieldValidator
                        ID="rfvDeliveryTime"
                        runat="server"
                        ControlToValidate="ddlDeliveryTime"
                        InitialValue=""
                        ErrorMessage="Please select an estimated delivery time."
                        CssClass="validation-error"
                        Display="Dynamic"
                        EnableClientScript="false">
                    </asp:RequiredFieldValidator>

                    <div class="helper-text">
                        Choose the time required to complete this project
                    </div>

                </div>

            </div>

            <div class="form-group attachment-group">

                <label class="form-label">
                    Attachments
                    <span class="optional">(Optional)</span>
                </label>

                <div class="attachment-description">
                    You can attach any relevant documents like portfolio,
                    samples or work examples.
                </div>

                <div class="upload-box">

                    <asp:FileUpload
                        ID="fuAttachment"
                        runat="server"
                        CssClass="file-upload" />

                    <div class="upload-content">

                        <div class="upload-icon">
                            <i class="fa-solid fa-cloud-arrow-up"></i>
                        </div>

                        <div class="upload-title">
                            Drag and drop files here or click to browse
                        </div>

                        <div class="upload-info">
                            Max file size: 5MB (PDF, DOC, DOCX, PNG, JPG)
                        </div>

                    </div>

                </div>

                <asp:CustomValidator
                    ID="cvAttachment"
                    runat="server"
                    ErrorMessage="Invalid attachment. Maximum size is 5MB and allowed types are PDF, DOC, DOCX, PNG and JPG."
                    CssClass="validation-error"
                    Display="Dynamic"
                    EnableClientScript="false"
                    OnServerValidate="cvAttachment_ServerValidate">
                </asp:CustomValidator>

            </div>

            <div class="note-box">

                <i class="fa-solid fa-circle-info"></i>

                <span>
                    <strong>Note:</strong>
                    Make sure your proposal is clear and professional.
                    A good proposal increases your chances of getting hired.
                </span>

            </div>

            <div class="proposal-divider"></div>

            <div class="proposal-actions">

                <a href="ProjectDetails.aspx" class="cancel-button">
                    Cancel
                </a>

                <asp:Button
                    ID="btnSubmitProposal"
                    runat="server"
                    Text="Submit Proposal"
                    CssClass="submit-button"
                    OnClick="btnSubmitProposal_Click" />

            </div>

        </div>

    </div>

</asp:Content>