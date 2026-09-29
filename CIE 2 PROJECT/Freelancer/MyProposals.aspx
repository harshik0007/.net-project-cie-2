<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyProposals.aspx.cs" Inherits="CIE_2_PROJECT.MyProposals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/MyProposals.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="proposals-page">

        <div class="page-header">
            <h1 class="page-title">My Proposals</h1>
            <p class="page-subtitle">Track and manage all your project proposals in one place.</p>
        </div>

        <%-- Filter Tab Buttons - PostBack to C# code-behind --%>
        <div class="proposal-tabs">
            <asp:LinkButton ID="btnAll"      runat="server" CssClass="tab-btn" OnClick="FilterTab_Click" CommandArgument="all">All (6)</asp:LinkButton>
            <asp:LinkButton ID="btnPending"  runat="server" CssClass="tab-btn" OnClick="FilterTab_Click" CommandArgument="pending">Pending (4)</asp:LinkButton>
            <asp:LinkButton ID="btnAccepted" runat="server" CssClass="tab-btn" OnClick="FilterTab_Click" CommandArgument="accepted">Accepted (1)</asp:LinkButton>
            <asp:LinkButton ID="btnRejected" runat="server" CssClass="tab-btn" OnClick="FilterTab_Click" CommandArgument="rejected">Rejected (1)</asp:LinkButton>
        </div>

        <%-- Proposals rendered server-side by Repeater --%>
        <div class="proposals-list">

            <asp:Repeater ID="rptProposals" runat="server">
                <ItemTemplate>
                    <div class="proposal-card">

                        <div class="proposal-main">
                            <div class="proposal-icon <%# Eval("IconClass") %>">
                                <i class="<%# Eval("IconFa") %>"></i>
                            </div>
                            <div class="proposal-content">
                                <h2 class="proposal-title"><%# Eval("Title") %></h2>
                                <div class="proposal-meta">
                                    <span class="meta-item">
                                        <i class="fa-regular fa-user"></i>
                                        Client: <a href="#" class="client-link"><%# Eval("Client") %></a>
                                    </span>
                                    <span class="meta-item">
                                        <i class="fa-regular fa-calendar"></i>
                                        Posted on <%# Eval("PostedOn") %>
                                    </span>
                                </div>
                                <p class="proposal-description"><%# Eval("Description") %></p>
                            </div>
                        </div>

                        <div class="proposal-side">
                            <div class="side-group">
                                <span class="side-label">Your Proposed Price</span>
                                <span class="side-price"><%# Eval("Price") %></span>
                            </div>
                            <div class="side-group">
                                <span class="side-label">Estimated Delivery</span>
                                <span class="side-value"><%# Eval("Delivery") %></span>
                            </div>
                            <div class="side-group">
                                <span class="side-label">Submitted On</span>
                                <span class="side-value"><%# Eval("SubmittedOn") %></span>
                            </div>
                            <span class="status-badge <%# Eval("BadgeClass") %>">
                                <i class="<%# Eval("BadgeIcon") %>"></i>
                                <%# Eval("StatusLabel") %>
                            </span>
                            <a href="ProjectDetails.aspx" class="view-project-button">View Project</a>
                        </div>

                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <%-- Empty state shown by C# when no results --%>
            <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
                <div class="empty-state">
                    <i class="fa-regular fa-folder-open"></i>
                    <h3>No Proposals Found</h3>
                    <p>You don't have any proposals matching this filter.</p>
                </div>
            </asp:Panel>

        </div>

    </div>
</asp:Content>
