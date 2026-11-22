<%@ Page Title="Messages" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Messages.aspx.cs" Inherits="CIE_2_PROJECT.MessagesPage" %>

<asp:Content ID="MessagesHead" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/Messages.css?v=4" />
</asp:Content>

<asp:Content ID="MessagesContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="messages-page">
        <header class="messages-header">
            <div>
                <h1>Messages</h1>
                <p>Discuss projects and share files with clients and freelancers.</p>
            </div>
        </header>

        <asp:Label ID="lblMessage" runat="server" CssClass="action-message" Visible="false" />

        <div class="messages-layout">
            <aside class="conversation-panel">
                <div class="conversation-header">
                    <h2>Conversations</h2>
                    <span class="conversation-count"><asp:Literal ID="litConversationCount" runat="server" Mode="Encode" /></span>
                </div>
                <asp:Repeater ID="rptConversations" runat="server" OnItemCommand="rptConversations_ItemCommand">
                    <ItemTemplate>
                        <div class="conversation-entry">
                            <a class='conversation-item <%#: Eval("SelectionClass") %>' href='<%#: Eval("Url") %>'>
                                <span class="conversation-avatar"><%#: Eval("Initials") %></span>
                                <span class="conversation-info">
                                    <span class="conversation-top"><span class="conversation-name"><%#: Eval("PartnerName") %></span><span class="conversation-time"><%#: Eval("LastSentText") %></span></span>
                                    <span class="conversation-project"><%#: Eval("ProjectTitle") %></span>
                                    <span class="conversation-preview"><%#: Eval("Preview") %></span>
                                </span>
                                <asp:Panel ID="pnlUnreadDot" runat="server" CssClass="conversation-unread-count" Visible='<%# (int)Eval("UnreadCount") > 0 %>'>
                                    <%#: Eval("UnreadCount") %>
                                </asp:Panel>
                            </a>
                            <asp:LinkButton ID="btnClearConversation" runat="server" CssClass="conversation-delete-button"
                                CommandName="ClearConversation" CommandArgument='<%# Eval("PartnerID") %>'
                                CausesValidation="false" ToolTip="Delete this conversation from your inbox. The other participant keeps their history." aria-label="Delete conversation">
                                <i class="fa-regular fa-trash-can"></i><span>Delete</span>
                            </asp:LinkButton>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Panel ID="pnlNoConversations" runat="server" CssClass="conversation-empty" Visible="false">
                    <i class="fa-regular fa-comments"></i>
                    <strong>No conversations yet</strong>
                    <span>Open a client or freelancer profile and choose Message to start.</span>
                </asp:Panel>
            </aside>

            <section class="chat-panel">
                <asp:Panel ID="pnlNoSelection" runat="server" CssClass="chat-empty-state">
                    <i class="fa-regular fa-paper-plane"></i>
                    <h2>Choose a conversation</h2>
                    <p>Select a conversation or start one from a profile.</p>
                </asp:Panel>

                <asp:Panel ID="pnlChat" runat="server" Visible="false">
                    <header class="chat-header">
                        <div class="chat-user">
                            <span class="chat-avatar"><asp:Literal ID="litPartnerInitials" runat="server" Mode="Encode" /></span>
                            <div>
                                <asp:HyperLink ID="lnkPartnerProfile" runat="server" CssClass="chat-name" />
                                <div class="chat-status"><asp:Literal ID="litPartnerRole" runat="server" Mode="Encode" /></div>
                            </div>
                        </div>
                        <div class="chat-project"><span>Project</span><strong><asp:Literal ID="litProjectTitle" runat="server" Mode="Encode" /></strong></div>
                    </header>

                    <div class="chat-messages">
                        <asp:Repeater ID="rptConversationMessages" runat="server">
                            <ItemTemplate>
                                <article class='message-row <%#: Eval("RowClass") %>'>
                                    <asp:Panel ID="pnlMessageAvatar" runat="server" CssClass="message-avatar" Visible='<%# !(bool)Eval("IsSent") %>'>
                                        <%#: Eval("SenderInitials") %>
                                    </asp:Panel>
                                    <div class="message-content">
                                        <asp:Panel ID="pnlMessageText" runat="server" CssClass="message-bubble" Visible='<%# !string.IsNullOrWhiteSpace(Convert.ToString(Eval("DisplayText"))) %>'>
                                            <span class="message-body"><%#: Eval("DisplayText") %></span>
                                        </asp:Panel>
                                        <asp:PlaceHolder ID="phMessageAttachment" runat="server" Visible='<%# (bool)Eval("HasAttachment") %>'>
                                            <a class="message-attachment" href='<%#: Eval("AttachmentUrl") %>'>
                                                <i class="fa-regular fa-file-lines"></i><span><%#: Eval("AttachmentName") %></span><i class="fa-solid fa-download"></i>
                                            </a>
                                        </asp:PlaceHolder>
                                        <div class="message-time"><%#: Eval("SentText") %></div>
                                    </div>
                                </article>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Panel ID="pnlThreadEmpty" runat="server" CssClass="thread-empty" Visible="false">
                            <i class="fa-regular fa-comment-dots"></i>
                            <span>Start the conversation with a message or file.</span>
                        </asp:Panel>
                    </div>

                    <asp:Panel ID="pnlComposer" runat="server" CssClass="chat-input-area">
                        <asp:HiddenField ID="hidRecipientId" runat="server" />
                        <asp:HiddenField ID="hidProjectId" runat="server" />
                        <div class="attachment-preview">
                            <asp:FileUpload ID="fuAttachment" runat="server" CssClass="file-upload"
                                accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.txt,.csv,.zip,.png,.jpg,.jpeg" />
                            <span>One file up to 5 MB. PDF, Office documents, text, CSV, ZIP, JPG, or PNG.</span>
                        </div>
                        <div class="message-input-row">
                            <asp:TextBox ID="txtMessage" runat="server" CssClass="message-input" TextMode="MultiLine" Rows="2"
                                MaxLength="1000" placeholder="Write a message..."></asp:TextBox>
                            <asp:Button ID="btnSendMessage" runat="server" Text="Send" CssClass="send-button" OnClick="btnSendMessage_Click" />
                        </div>
                        <asp:CustomValidator ID="cvMessage" runat="server" OnServerValidate="cvMessage_ServerValidate"
                            EnableClientScript="false" ValidateEmptyText="true"
                            ErrorMessage="Write a message or attach a file. Messages can contain up to 1,000 characters."
                            CssClass="validation-message" Display="Dynamic" />
                    </asp:Panel>
                </asp:Panel>
            </section>
        </div>
    </main>
</asp:Content>
