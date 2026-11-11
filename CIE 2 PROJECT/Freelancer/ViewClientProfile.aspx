<%@ Page Title="Client Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewClientProfile.aspx.cs" Inherits="CIE_2_PROJECT.ViewClientProfile" %>

<asp:Content ID="ProfileHead" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/PublicProfile.css?v=4" />
</asp:Content>

<asp:Content ID="ProfileContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="public-profile-page">
        <a class="public-profile-back" href="MyProposals.aspx"><i class="fa-solid fa-arrow-left"></i> Back to proposals</a>
        <asp:Panel ID="pnlProfileError" runat="server" CssClass="public-profile-error" Visible="false">
            <asp:Literal ID="litProfileError" runat="server" Mode="Encode" />
        </asp:Panel>

        <asp:Panel ID="pnlProfile" runat="server" Visible="false">
            <section class="public-profile-hero">
                <div class="public-profile-identity">
                    <div class="public-profile-avatar">
                        <asp:Image ID="imgProfile" runat="server" AlternateText="Client profile photo" Visible="false" />
                        <asp:Panel ID="pnlInitials" runat="server"><asp:Literal ID="litInitials" runat="server" Mode="Encode" /></asp:Panel>
                    </div>
                    <div class="public-profile-heading">
                        <span class="public-profile-eyebrow">Client profile</span>
                        <h1><asp:Literal ID="litFullName" runat="server" Mode="Encode" /></h1>
                        <div class="public-profile-username">@<asp:Literal ID="litUsername" runat="server" Mode="Encode" /></div>
                        <div class="public-profile-meta">
                            <span><i class="fa-solid fa-location-dot"></i><asp:Literal ID="litLocation" runat="server" Mode="Encode" /></span>
                            <span><i class="fa-regular fa-calendar"></i>Member since <asp:Literal ID="litMemberSince" runat="server" Mode="Encode" /></span>
                        </div>
                    </div>
                </div>
                <div class="public-profile-rating-summary">
                    <span>Freelancer rating</span>
                    <strong><asp:Literal ID="litAverageRating" runat="server" Mode="Encode" /> <small>/ 5</small></strong>
                    <span class="public-profile-stars"><asp:Literal ID="litStars" runat="server" /></span>
                    <span><asp:Literal ID="litReviewCount" runat="server" Mode="Encode" /> reviews</span>
                </div>
                <asp:HyperLink ID="lnkSendMessage" runat="server" CssClass="public-profile-message-button" Text="Message" />
            </section>

            <div class="public-profile-grid">
                <div class="public-profile-main">
                    <section class="public-profile-card">
                        <h2>About</h2>
                        <p class="public-profile-bio"><asp:Literal ID="litBio" runat="server" Mode="Encode" /></p>
                        <div class="public-profile-details">
                            <div><span>Organization</span><strong><asp:Literal ID="litOrganization" runat="server" Mode="Encode" /></strong></div>
                            <div><span>Completed projects</span><strong><asp:Literal ID="litCompletedProjects" runat="server" Mode="Encode" /></strong></div>
                        </div>
                    </section>

                    <section class="public-profile-card public-profile-contact-card">
                        <h2>Contact information</h2>
                        <div class="public-profile-contact-row">
                            <i class="fa-regular fa-envelope"></i>
                            <div><span>Email</span><asp:HyperLink ID="lnkContactEmail" runat="server" CssClass="public-profile-link" /></div>
                        </div>
                        <div class="public-profile-contact-row" id="portfolioContactRow" runat="server" visible="false">
                            <i class="fa-solid fa-arrow-up-right-from-square"></i>
                            <div><span>Website or portfolio</span><asp:HyperLink ID="lnkPortfolio" runat="server" CssClass="public-profile-link" Text="Visit website" Target="_blank" /></div>
                        </div>
                        <p class="public-profile-contact-note">Phone and payment details are kept private.</p>
                        <asp:HyperLink ID="lnkContactMessage" runat="server" CssClass="public-profile-contact-message">
                            <i class="fa-regular fa-comments"></i> Message on SkillLink
                        </asp:HyperLink>
                    </section>

                    <section class="public-profile-card">
                        <div class="public-profile-section-heading"><h2>Recent projects</h2></div>
                        <asp:Repeater ID="rptProjects" runat="server">
                            <ItemTemplate>
                                <article class="public-profile-project-item">
                                    <div><strong><%#: Eval("Title") %></strong><span><%#: Eval("Category") %></span></div>
                                    <span class='public-profile-status <%#: Eval("StatusClass") %>'><%#: Eval("Status") %></span>
                                </article>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Panel ID="pnlNoProjects" runat="server" CssClass="public-profile-empty" Visible="false">This client has not posted projects yet.</asp:Panel>
                    </section>

                    <section class="public-profile-card">
                        <div class="public-profile-section-heading"><h2>Freelancer reviews</h2><asp:HyperLink ID="lnkAllReviews" runat="server" CssClass="public-profile-link" Text="View all" /></div>
                        <asp:Repeater ID="rptReviews" runat="server">
                            <ItemTemplate>
                                <article class="public-profile-review-item">
                                    <div class="public-profile-review-heading"><strong><a href='<%#: Eval("ReviewerProfileUrl") %>'><%#: Eval("ReviewerName") %></a></strong><span><%#: Eval("CreatedDate") %></span></div>
                                    <div class="public-profile-review-rating"><i class="fa-solid fa-star"></i> <%#: Eval("Rating") %> / 5</div>
                                    <p><%#: Eval("ReviewText") %></p>
                                </article>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Panel ID="pnlNoReviews" runat="server" CssClass="public-profile-empty" Visible="false">Freelancer reviews will appear after completed projects.</asp:Panel>
                    </section>
                </div>

                <aside class="public-profile-side">
                    <section class="public-profile-card public-profile-stat-card">
                        <h2>Client overview</h2>
                        <div><span>Projects posted</span><strong><asp:Literal ID="litProjectCount" runat="server" Mode="Encode" /></strong></div>
                        <div><span>Projects in progress</span><strong><asp:Literal ID="litActiveProjects" runat="server" Mode="Encode" /></strong></div>
                        <div><span>Completed projects</span><strong><asp:Literal ID="litCompletedProjectsSide" runat="server" Mode="Encode" /></strong></div>
                    </section>
                    <div class="public-profile-privacy-note"><i class="fa-solid fa-shield-halved"></i><span>Only the contact options shared on this profile are visible. Phone, account, and payment details stay private.</span></div>
                </aside>
            </div>
        </asp:Panel>
    </main>
</asp:Content>
