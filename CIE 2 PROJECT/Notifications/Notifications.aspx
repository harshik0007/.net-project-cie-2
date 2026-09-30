<%@ Page Title="Notifications" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Notifications.aspx.cs"
    Inherits="CIE_2_PROJECT.NotificationsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/Notifications.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="notifications-page">

        <div class="page-header">
            <div>
                <h1>Notifications</h1>
                <p>Stay updated with your projects and account activity.</p>
            </div>

            <asp:LinkButton
                ID="btnMarkAllRead"
                runat="server"
                CssClass="mark-read-button"
                OnClick="btnMarkAllRead_Click">
                <i class="fa-solid fa-check-double"></i>
                Mark all as read
            </asp:LinkButton>
        </div>

        <div class="notification-tabs">

            <asp:LinkButton
                ID="btnAll"
                runat="server"
                CssClass="notification-tab active-tab"
                OnClick="btnAll_Click">
                All
            </asp:LinkButton>

            <asp:LinkButton
                ID="btnUnread"
                runat="server"
                CssClass="notification-tab"
                OnClick="btnUnread_Click">
                Unread
                <span class="unread-count">3</span>
            </asp:LinkButton>

        </div>

        <div class="notification-container">

            <asp:Panel ID="pnlAllNotifications" runat="server">

                <div class="notification-section-title">
                    Today
                </div>

                <div class="notification-item unread">

                    <div class="notification-icon proposal-icon">
                        <i class="fa-solid fa-file-signature"></i>
                    </div>

                    <div class="notification-content">
                        <div class="notification-title">
                            New proposal received
                        </div>

                        <div class="notification-text">
                            Rahul Sharma submitted a proposal for
                            <strong>Unity 2D Game Development</strong>.
                        </div>

                        <div class="notification-time">
                            15 minutes ago
                        </div>
                    </div>

                    <span class="unread-dot"></span>

                </div>


                <div class="notification-item unread">

                    <div class="notification-icon message-notification">
                        <i class="fa-regular fa-envelope"></i>
                    </div>

                    <div class="notification-content">
                        <div class="notification-title">
                            New message
                        </div>

                        <div class="notification-text">
                            Priya Mehta sent you a message about
                            <strong>E-commerce Website Development</strong>.
                        </div>

                        <div class="notification-time">
                            1 hour ago
                        </div>
                    </div>

                    <span class="unread-dot"></span>

                </div>


                <div class="notification-item unread">

                    <div class="notification-icon payment-notification">
                        <i class="fa-solid fa-shield-halved"></i>
                    </div>

                    <div class="notification-content">
                        <div class="notification-title">
                            Payment secured
                        </div>

                        <div class="notification-text">
                            Your payment for <strong>E-commerce Website Development</strong>
                            has been securely held.
                        </div>

                        <div class="notification-time">
                            3 hours ago
                        </div>
                    </div>

                    <span class="unread-dot"></span>

                </div>


                <div class="notification-section-title">
                    Yesterday
                </div>


                <div class="notification-item">

                    <div class="notification-icon proposal-icon">
                        <i class="fa-solid fa-user-check"></i>
                    </div>

                    <div class="notification-content">
                        <div class="notification-title">
                            Freelancer hired
                        </div>

                        <div class="notification-text">
                            You hired <strong>Neha Patel</strong> for
                            <strong>Brand Logo Design</strong>.
                        </div>

                        <div class="notification-time">
                            Yesterday, 4:20 PM
                        </div>
                    </div>

                </div>


                <div class="notification-item">

                    <div class="notification-icon work-notification">
                        <i class="fa-solid fa-briefcase"></i>
                    </div>

                    <div class="notification-content">
                        <div class="notification-title">
                            Work submitted
                        </div>

                        <div class="notification-text">
                            A freelancer submitted work for
                            <strong>Portfolio Website</strong>.
                        </div>

                        <div class="notification-time">
                            Yesterday, 11:35 AM
                        </div>
                    </div>

                </div>


                <div class="notification-section-title">
                    Earlier
                </div>


                <div class="notification-item">

                    <div class="notification-icon project-notification">
                        <i class="fa-solid fa-folder-plus"></i>
                    </div>

                    <div class="notification-content">
                        <div class="notification-title">
                            Project posted successfully
                        </div>

                        <div class="notification-text">
                            Your project <strong>Unity 2D Game Development</strong>
                            is now open for proposals.
                        </div>

                        <div class="notification-time">
                            18 May, 2026
                        </div>
                    </div>

                </div>

            </asp:Panel>


            <asp:Panel
                ID="pnlUnreadNotifications"
                runat="server"
                Visible="false">

                <div class="notification-section-title">
                    Unread Notifications
                </div>

                <div class="notification-item unread">

                    <div class="notification-icon proposal-icon">
                        <i class="fa-solid fa-file-signature"></i>
                    </div>

                    <div class="notification-content">
                        <div class="notification-title">
                            New proposal received
                        </div>

                        <div class="notification-text">
                            Rahul Sharma submitted a proposal for
                            <strong>Unity 2D Game Development</strong>.
                        </div>

                        <div class="notification-time">
                            15 minutes ago
                        </div>
                    </div>

                    <span class="unread-dot"></span>

                </div>


                <div class="notification-item unread">

                    <div class="notification-icon message-notification">
                        <i class="fa-regular fa-envelope"></i>
                    </div>

                    <div class="notification-content">
                        <div class="notification-title">
                            New message
                        </div>

                        <div class="notification-text">
                            Priya Mehta sent you a message about
                            <strong>E-commerce Website Development</strong>.
                        </div>

                        <div class="notification-time">
                            1 hour ago
                        </div>
                    </div>

                    <span class="unread-dot"></span>

                </div>


                <div class="notification-item unread">

                    <div class="notification-icon payment-notification">
                        <i class="fa-solid fa-shield-halved"></i>
                    </div>

                    <div class="notification-content">
                        <div class="notification-title">
                            Payment secured
                        </div>

                        <div class="notification-text">
                            Your payment for <strong>E-commerce Website Development</strong>
                            has been securely held.
                        </div>

                        <div class="notification-time">
                            3 hours ago
                        </div>
                    </div>

                    <span class="unread-dot"></span>

                </div>

            </asp:Panel>

        </div>

        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="page-message">
        </asp:Label>

    </div>

</asp:Content>