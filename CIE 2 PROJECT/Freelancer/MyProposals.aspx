
<%@ Page Title="My Proposals" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="MyProposals.aspx.cs"
    Inherits="CIE_2_PROJECT.MyProposals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/MyProposals.css?v=6" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="proposals-page">

        <div class="page-header">
            <h1 class="page-title">My Proposals</h1>
            <p class="page-subtitle">
                Track and manage all your project proposals in one place.
            </p>
        </div>

        <div class="proposal-tabs">

            <asp:LinkButton ID="btnAll" runat="server"
                CssClass="tab-btn"
                Text="All (0)"
                CommandArgument="all"
                OnClick="FilterTab_Click" />

            <asp:LinkButton ID="btnPending" runat="server"
                CssClass="tab-btn"
                Text="Pending (0)"
                CommandArgument="pending"
                OnClick="FilterTab_Click" />

            <asp:LinkButton ID="btnAccepted" runat="server"
                CssClass="tab-btn"
                Text="Accepted (0)"
                CommandArgument="accepted"
                OnClick="FilterTab_Click" />

            <asp:LinkButton ID="btnCompleted" runat="server"
                CssClass="tab-btn"
                Text="Completed (0)"
                CommandArgument="completed"
                OnClick="FilterTab_Click" />

            <asp:LinkButton ID="btnPreviousReviews" runat="server"
                CssClass="tab-btn"
                Text="Previous Reviews (0)"
                CommandArgument="reviews"
                OnClick="FilterTab_Click" />

            <asp:LinkButton ID="btnRejected" runat="server"
                CssClass="tab-btn"
                Text="Rejected (0)"
                CommandArgument="rejected"
                OnClick="FilterTab_Click" />

        </div>

        <asp:Label ID="lblPageMessage" runat="server"
            CssClass="page-message"
            Visible="false" />

        <asp:Panel ID="pnlProposalSection" runat="server" CssClass="proposals-list">

            <asp:Repeater ID="rptProposals" runat="server">

                <ItemTemplate>

                    <div class="proposal-card">

                        <div class="proposal-main">

                            <div class='proposal-icon <%#: Eval("IconClass") %>'>
                                <i class='<%#: Eval("IconFa") %>'></i>
                            </div>

                            <div class="proposal-content">

                                <h2 class="proposal-title" title='<%#: Eval("Title") %>'>
                                    <%#: Eval("Title") %>
                                </h2>

                                <div class="proposal-meta">

                                    <span class="meta-item">
                                        <i class="fa-regular fa-user"></i>
                                        Client:
                                        <a class="client-link" href='<%#: Eval("ClientProfileUrl") %>'><%#: Eval("Client") %></a>
                                    </span>

                                    <span class="meta-item">
                                        <i class="fa-regular fa-calendar"></i>
                                        Posted on <%#: Eval("PostedOn") %>
                                    </span>

                                </div>

                                <p class="proposal-description" title='<%#: Eval("Description") %>'>
                                    <%#: Eval("Description") %>
                                </p>

                            </div>

                        </div>

                        <div class="proposal-side">

                            <div class="side-group">
                                <span class="side-label">Your Proposed Price</span>
                                <span class="side-price"><%#: Eval("Price") %></span>
                                <span class="side-label"><%#: Eval("PayoutLabel") %></span>
                                <span class="side-value"><%#: Eval("EstimatedPayout") %></span>
                            </div>

                            <div class="side-group">
                                <span class="side-label">Estimated Delivery</span>
                                <span class="side-value"><%#: Eval("Delivery") %></span>
                            </div>

                            <div class="side-group">
                                <span class="side-label">Submitted On</span>
                                <span class="side-value"><%#: Eval("SubmittedOn") %></span>
                            </div>

                            <span class='status-badge <%#: Eval("BadgeClass") %>'>
                                <i class='<%#: Eval("BadgeIcon") %>'></i>
                                <%#: Eval("StatusLabel") %>
                            </span>

                            <asp:Panel ID="pnlClientReview" runat="server" CssClass="client-review-status"
                                Visible='<%# Eval("ShowReviewStatus") %>'>
                                <span class="side-label">Your review for the client</span>
                                <span class='review-state <%#: Eval("ReviewStateClass") %>'>
                                    <%#: Eval("ReviewStatus") %>
                                </span>
                                <asp:HyperLink ID="lnkRateClient" runat="server"
                                    CssClass="rate-client-button"
                                    Text="Rate Client"
                                    NavigateUrl='<%# Eval("ReviewUrl") %>'
                                    Visible='<%# Eval("CanReviewClient") %>' />
                            </asp:Panel>

                            <asp:HyperLink ID="lnkViewProject" runat="server"
                                CssClass="view-project-button"
                                Text='<%# Eval("ActionText") %>'
                                NavigateUrl='<%# Eval("ActionUrl") %>'
                                Visible='<%# Eval("ShowActionLink") %>' />

                            <asp:HyperLink ID="lnkMessageClient" runat="server"
                                CssClass="message-client-button"
                                NavigateUrl='<%# Eval("MessageUrl") %>'>
                                <i class="fa-regular fa-comments"></i> Message client
                            </asp:HyperLink>

                            <asp:Label ID="lblMissingContract" runat="server"
                                CssClass="view-project-button"
                                Text="Accepted - contract unavailable"
                                Visible='<%# Eval("ShowContractUnavailable") %>' />

                        </div>

                    </div>

                </ItemTemplate>

            </asp:Repeater>

            <asp:Panel ID="pnlEmpty" runat="server" Visible="false">

                <div class="empty-state">
                    <div class="empty-icon">
                        <i class="fa-regular fa-folder-open"></i>
                    </div>

                    <h3><asp:Label ID="lblEmptyTitle" runat="server" Text="No Proposals Found" /></h3>

                    <p>
                        <asp:Label ID="lblEmptyDescription" runat="server"
                            Text="You don't have any proposals matching this filter. Explore projects and submit your first proposal." />
                    </p>

                    <a href="FindProjects.aspx" class="empty-action">
                        <i class="fa-solid fa-magnifying-glass"></i>
                        Find Projects
                    </a>
                </div>

            </asp:Panel>

        </asp:Panel>

        <asp:Panel ID="pnlPreviousReviews" runat="server" CssClass="previous-reviews-list" Visible="false">
            <asp:Repeater ID="rptPreviousReviews" runat="server">
                <ItemTemplate>
                    <article class="previous-review-card">
                        <div class="previous-review-heading">
                            <div>
                                <span class="previous-review-label">Project</span>
                                <h2 title='<%#: Eval("ProjectTitle") %>'><%#: Eval("ProjectTitle") %></h2>
                            </div>
                            <span class="previous-review-rating"><i class="fa-solid fa-star"></i> <%#: Eval("Rating") %> / 5</span>
                        </div>
                        <div class="previous-review-meta">
                            <span>Client: <strong><a href='<%#: Eval("ClientProfileUrl") %>'><%#: Eval("ClientName") %></a></strong></span>
                            <span>Submitted on <%#: Eval("CreatedAt") %></span>
                        </div>
                        <p class="previous-review-text"><%#: Eval("ReviewText") %></p>
                    </article>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Panel ID="pnlNoPreviousReviews" runat="server" Visible="false">
                <div class="empty-state">
                    <div class="empty-icon"><i class="fa-regular fa-star"></i></div>
                    <h3>No previous reviews yet</h3>
                    <p>Reviews you submit for clients will appear here.</p>
                </div>
            </asp:Panel>
        </asp:Panel>

    </div>

</asp:Content>
