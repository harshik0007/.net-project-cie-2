<%@ Page Title="Messages" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Messages.aspx.cs"
    Inherits="CIE_2_PROJECT.MessagesPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/Messages.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="messages-page">

        <div class="messages-header">
            <div>
                <h1>Messages</h1>
                <p>Communicate with clients and freelancers about your projects.</p>
            </div>
        </div>

        <div class="messages-layout">

            <!-- CONVERSATIONS -->

            <aside class="conversation-panel">

                <div class="conversation-header">
                    <h2>Conversations</h2>

                    <span class="conversation-count">
                        3
                    </span>
                </div>

                <div class="conversation-search">
                    <i class="fa-solid fa-magnifying-glass"></i>

                    <asp:TextBox
                        ID="txtSearchConversation"
                        runat="server"
                        CssClass="search-input"
                        placeholder="Search conversations">
                    </asp:TextBox>
                </div>

                <asp:LinkButton
                    ID="btnRahul"
                    runat="server"
                    CssClass="conversation-item active-conversation"
                    OnClick="btnRahul_Click">

                    <div class="conversation-avatar">
                        RS
                    </div>

                    <div class="conversation-info">

                        <div class="conversation-top">
                            <span class="conversation-name">
                                Rahul Sharma
                            </span>

                            <span class="conversation-time">
                                10:42 AM
                            </span>
                        </div>

                        <div class="conversation-project">
                            Unity 2D Game Development
                        </div>

                        <div class="conversation-preview">
                            I have attached the latest version...
                        </div>

                    </div>

                    <span class="unread-dot"></span>

                </asp:LinkButton>


                <asp:LinkButton
                    ID="btnPriya"
                    runat="server"
                    CssClass="conversation-item"
                    OnClick="btnPriya_Click">

                    <div class="conversation-avatar purple-avatar">
                        PM
                    </div>

                    <div class="conversation-info">

                        <div class="conversation-top">
                            <span class="conversation-name">
                                Priya Mehta
                            </span>

                            <span class="conversation-time">
                                Yesterday
                            </span>
                        </div>

                        <div class="conversation-project">
                            E-commerce Website Development
                        </div>

                        <div class="conversation-preview">
                            Can we discuss the project requirements?
                        </div>

                    </div>

                </asp:LinkButton>


                <asp:LinkButton
                    ID="btnNeha"
                    runat="server"
                    CssClass="conversation-item"
                    OnClick="btnNeha_Click">

                    <div class="conversation-avatar green-avatar">
                        NP
                    </div>

                    <div class="conversation-info">

                        <div class="conversation-top">
                            <span class="conversation-name">
                                Neha Patel
                            </span>

                            <span class="conversation-time">
                                18 May
                            </span>
                        </div>

                        <div class="conversation-project">
                            Brand Logo Design
                        </div>

                        <div class="conversation-preview">
                            Thank you for approving the design.
                        </div>

                    </div>

                </asp:LinkButton>

            </aside>


            <!-- CHAT -->

            <section class="chat-panel">

                <div class="chat-header">

                    <div class="chat-user">

                        <div class="chat-avatar">
                            RS
                        </div>

                        <div>
                            <div class="chat-name">
                                Rahul Sharma
                            </div>

                            <div class="chat-status">
                                <span class="online-dot"></span>
                                Online
                            </div>
                        </div>

                    </div>

                    <div class="chat-project">

                        <span>Project</span>

                        <strong>
                            Unity 2D Game Development
                        </strong>

                    </div>

                </div>


                <div class="chat-messages">

                    <div class="date-divider">
                        <span>Today</span>
                    </div>


                    <div class="message-row received">

                        <div class="message-avatar">
                            RS
                        </div>

                        <div class="message-content">

                            <div class="message-bubble">
                                Hello! I have started working on the Unity
                                project. I wanted to share the initial progress
                                with you.
                            </div>

                            <div class="message-time">
                                10:15 AM
                            </div>

                        </div>

                    </div>


                    <div class="message-row sent">

                        <div class="message-content">

                            <div class="message-bubble">
                                Great! Please make sure the player movement
                                and basic game mechanics are implemented first.
                            </div>

                            <div class="message-time">
                                10:24 AM
                            </div>

                        </div>

                    </div>


                    <div class="message-row received">

                        <div class="message-avatar">
                            RS
                        </div>

                        <div class="message-content">

                            <div class="message-bubble">
                                Sure. I will complete those parts and send
                                you an update today.
                            </div>

                            <div class="message-time">
                                10:31 AM
                            </div>

                        </div>

                    </div>


                    <div class="message-row received">

                        <div class="message-avatar">
                            RS
                        </div>

                        <div class="message-content">

                            <div class="message-bubble attachment-message">

                                <div class="attachment-file">
                                    <div class="attachment-icon">
                                        <i class="fa-regular fa-file"></i>
                                    </div>

                                    <div>
                                        <div class="attachment-name">
                                            Unity_Project_v1.zip
                                        </div>

                                        <div class="attachment-size">
                                            8.4 MB
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="message-time">
                                10:42 AM
                            </div>

                        </div>

                    </div>

                </div>


                <div class="chat-input-area">

                    <div class="attachment-preview">
                        <asp:FileUpload
                            ID="fuAttachment"
                            runat="server"
                            CssClass="file-upload" />
                    </div>

                    <div class="message-input-row">

                        <asp:TextBox
                            ID="txtMessage"
                            runat="server"
                            CssClass="message-input"
                            TextMode="MultiLine"
                            Rows="2"
                            placeholder="Type your message...">
                        </asp:TextBox>

                        <asp:Button
                            ID="btnSendMessage"
                            runat="server"
                            Text="Send"
                            CssClass="send-button"
                            OnClick="btnSendMessage_Click" />

                    </div>

                    <asp:RequiredFieldValidator
                        ID="rfvMessage"
                        runat="server"
                        ControlToValidate="txtMessage"
                        ErrorMessage="Please enter a message."
                        CssClass="validation-message"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                    <asp:CustomValidator
                        ID="cvMessage"
                        runat="server"
                        ControlToValidate="txtMessage"
                        OnServerValidate="cvMessage_ServerValidate"
                        EnableClientScript="false"
                        ErrorMessage="Message must be between 1 and 1000 characters."
                        CssClass="validation-message"
                        Display="Dynamic">
                    </asp:CustomValidator>

                    <asp:Label
                        ID="lblMessage"
                        runat="server"
                        CssClass="action-message">
                    </asp:Label>

                </div>

            </section>

        </div>

    </div>

</asp:Content>