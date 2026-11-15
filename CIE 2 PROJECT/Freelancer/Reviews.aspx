<%@ Page Title="Freelancer Reviews" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reviews.aspx.cs" Inherits="CIE_2_PROJECT.FreelancerReviews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/WorkHistory.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="work-history-page">
        <div class="work-history-header">
            <div><h1>Freelancer Reviews</h1><p>Feedback left by clients after completed contracts.</p></div>
            <asp:HyperLink ID="lnkBackProfile" runat="server">Back to Profile</asp:HyperLink>
        </div>
        <div class="work-history-card review-summary">
            <strong>Average rating: <asp:Label ID="lblAverageRating" runat="server" Text="0.0" /> / 5</strong>
            <span>Review count: <asp:Label ID="lblReviewCount" runat="server" Text="0" /></span>
        </div>
        <asp:Label ID="lblMessage" runat="server" CssClass="work-history-message" Visible="false" />
        <asp:Panel ID="pnlEmpty" runat="server" CssClass="work-history-empty" Visible="false">
            <p>No reviews have been submitted yet.</p>
        </asp:Panel>
        <asp:Repeater ID="rptReviews" runat="server">
            <ItemTemplate>
                <article class="work-history-card review-entry">
                    <div><h2><a href='<%#: Eval("ReviewerProfileUrl") %>'><%#: Eval("Reviewer") %></a> &#183; <%#: Eval("Rating") %>/5</h2>
                        <p><%#: Eval("CreatedAt") %></p>
                        <p><%#: Eval("ReviewText") %></p></div>
                </article>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
