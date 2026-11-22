<%@ Page Title="Notifications" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs" Inherits="CIE_2_PROJECT.NotificationsPage" %>

<asp:Content ID="NotificationsHead" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/Notifications.css?v=4" />
</asp:Content>

<asp:Content ID="NotificationsContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="notifications-page">
        <header class="page-header">
            <div>
                <h1>Notifications</h1>
                <p>Project updates, messages, payments, and reviews.</p>
            </div>
            <asp:LinkButton ID="btnMarkAllRead" runat="server" CssClass="mark-read-button" OnClick="btnMarkAllRead_Click">
                <i class="fa-solid fa-check-double"></i> Mark all as read
            </asp:LinkButton>
        </header>

        <nav class="notification-tabs" aria-label="Notification filters">
            <asp:HyperLink ID="btnAll" runat="server" CssClass="notification-tab" NavigateUrl="~/Notifications/Notifications.aspx" Text="All" />
            <span class="notification-filter-group">
                <asp:HyperLink ID="btnUnread" runat="server" CssClass="notification-tab" NavigateUrl="~/Notifications/Notifications.aspx?filter=unread" Text="Unread" />
                <span class="unread-count"><asp:Literal ID="litUnreadCount" runat="server" Mode="Encode" /></span>
            </span>
        </nav>

        <asp:Label ID="lblMessage" runat="server" CssClass="page-message" Visible="false" />
        <section class="notification-container">
            <asp:Repeater ID="rptNotifications" runat="server" OnItemCommand="rptNotifications_ItemCommand">
                <ItemTemplate>
                    <article class='notification-item <%#: Eval("ReadClass") %>'>
                        <span class='notification-icon <%#: Eval("IconClass") %>'><i class='<%#: Eval("IconName") %>'></i></span>
                        <div class="notification-content">
                            <div class="notification-title"><%#: Eval("Title") %></div>
                            <div class="notification-text"><%#: Eval("MessageText") %></div>
                            <div class="notification-time"><%#: Eval("CreatedText") %></div>
                        </div>
                        <asp:HyperLink ID="btnOpenNotification" runat="server" CssClass="notification-action"
                            NavigateUrl='<%#: Eval("TargetUrl") %>' Text="View" />
                        <asp:LinkButton ID="btnMarkNotificationRead" runat="server" CssClass="notification-mark-read"
                            CommandName="MarkRead" CommandArgument='<%# Eval("NotificationID") %>' Text="Mark read"
                            Visible='<%# !(bool)Eval("IsRead") %>' />
                        <asp:LinkButton ID="btnDeleteNotification" runat="server" CssClass="notification-delete"
                            CommandName="Delete" CommandArgument='<%# Eval("NotificationID") %>'
                            CausesValidation="false" ToolTip="Delete notification" aria-label="Delete notification">
                            <i class="fa-regular fa-trash-can"></i>
                        </asp:LinkButton>
                        <asp:Panel ID="pnlUnreadDot" runat="server" CssClass="unread-dot" Visible='<%# !(bool)Eval("IsRead") %>' />
                    </article>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Panel ID="pnlEmpty" runat="server" CssClass="notification-empty" Visible="false">
                <span class="notification-empty-icon"><i class="fa-regular fa-bell-slash"></i></span>
                <h2><asp:Literal ID="litEmptyTitle" runat="server" Mode="Encode" /></h2>
                <p><asp:Literal ID="litEmptyText" runat="server" Mode="Encode" /></p>
            </asp:Panel>
        </section>
    </main>
</asp:Content>
