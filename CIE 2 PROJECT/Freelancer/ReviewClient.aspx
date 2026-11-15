<%@ Page Title="Review Client" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReviewClient.aspx.cs" Inherits="CIE_2_PROJECT.ReviewClient" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/ClientReview.css?v=1" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="client-review-page">
        <a class="client-review-back" href="MyProposals.aspx?filter=completed">&larr; Back to completed projects</a>
        <header class="client-review-header">
            <span class="client-review-icon"><i class="fa-regular fa-star"></i></span>
            <div>
                <p class="client-review-eyebrow">Project feedback</p>
                <h1>Review your client</h1>
                <p>Share a fair, helpful review about your completed project.</p>
            </div>
        </header>

        <asp:Label ID="lblMessage" runat="server" CssClass="client-review-message" Visible="false" />
        <asp:Panel ID="pnlReview" runat="server" Visible="false" CssClass="client-review-card">
            <div class="client-review-project">
                <span class="client-review-field-label">Completed project</span>
                <h2><asp:Label ID="lblProject" runat="server" /></h2>
                <p><span class="client-review-field-label">Client</span><strong><asp:HyperLink ID="lnkClientProfile" runat="server" /></strong></p>
            </div>
            <div class="client-review-form">
                <asp:Label ID="lblRatingPrompt" runat="server" CssClass="client-review-label"
                    AssociatedControlID="ddlRating" Text="Overall experience" />
                <asp:DropDownList ID="ddlRating" runat="server" CssClass="client-review-rating">
                    <asp:ListItem Text="Choose a rating" Value="" />
                    <asp:ListItem Text="5 - Excellent" Value="5" />
                    <asp:ListItem Text="4 - Good" Value="4" />
                    <asp:ListItem Text="3 - Satisfactory" Value="3" />
                    <asp:ListItem Text="2 - Needs improvement" Value="2" />
                    <asp:ListItem Text="1 - Poor" Value="1" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvRating" runat="server" ControlToValidate="ddlRating"
                    InitialValue="" ValidationGroup="ClientReview" EnableClientScript="false"
                    CssClass="client-review-error" ErrorMessage="Choose a rating." />
                <asp:Label ID="lblReviewPrompt" runat="server" CssClass="client-review-label"
                    AssociatedControlID="txtReview" Text="Written feedback" />
                <asp:TextBox ID="txtReview" runat="server" TextMode="MultiLine" MaxLength="2000"
                    CssClass="client-review-text" placeholder="What went well? Share constructive feedback for future projects." />
                <asp:RequiredFieldValidator ID="rfvReviewText" runat="server" ControlToValidate="txtReview"
                    ValidationGroup="ClientReview" EnableClientScript="false"
                    CssClass="client-review-error" ErrorMessage="Write a review before submitting." />
                <div class="client-review-actions">
                    <asp:Button ID="btnSubmitReview" runat="server" Text="Submit Review"
                        CssClass="client-review-submit" ValidationGroup="ClientReview" OnClick="SubmitReview_Click" />
                    <span>Your review will appear on the client's profile.</span>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
