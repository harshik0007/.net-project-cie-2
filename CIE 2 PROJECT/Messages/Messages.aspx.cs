using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using CIE_2_PROJECT.Factories;
using CIE_2_PROJECT.Helpers;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT
{
    public partial class MessagesPage : Page
    {
        private sealed class ConversationItem
        {
            public int PartnerID { get; set; }
            public string PartnerName { get; set; }
            public string ProjectTitle { get; set; }
            public string Preview { get; set; }
            public string LastSentText { get; set; }
            public string Initials { get; set; }
            public string SelectionClass { get; set; }
            public string Url { get; set; }
            public int UnreadCount { get; set; }
        }

        private sealed class MessageItem
        {
            public bool IsSent { get; set; }
            public string RowClass { get; set; }
            public string DisplayText { get; set; }
            public string SentText { get; set; }
            public string SenderInitials { get; set; }
            public bool HasAttachment { get; set; }
            public string AttachmentName { get; set; }
            public string AttachmentUrl { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindPage();
        }

        protected void cvMessage_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string message = (txtMessage.Text ?? string.Empty).Trim();
            if (message.Length > 1000)
            {
                cvMessage.ErrorMessage = "Messages can contain up to 1,000 characters.";
                args.IsValid = false;
                return;
            }
            if (message.Length == 0 && !fuAttachment.HasFile)
            {
                cvMessage.ErrorMessage = "Write a message or attach a file.";
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
                return;

            int senderId = AuthenticatedUser.UserID;
            int receiverId;
            if (!int.TryParse(hidRecipientId.Value, NumberStyles.None,
                CultureInfo.InvariantCulture, out receiverId) || receiverId <= 0)
            {
                ShowMessage("Choose a conversation partner before sending.", false);
                return;
            }

            int parsedProjectId;
            int? projectId = int.TryParse(hidProjectId.Value, NumberStyles.None,
                CultureInfo.InvariantCulture, out parsedProjectId) && parsedProjectId > 0
                ? (int?)parsedProjectId : null;

            string attachmentPath = null;
            try
            {
                if (fuAttachment.HasFile)
                    attachmentPath = MessageAttachmentStorage.Save(fuAttachment.PostedFile);

                IMessagingService service = ServiceFactory.CreateMessagingService();
                service.Send(senderId, receiverId, projectId, txtMessage.Text, attachmentPath);

                string url = "Messages.aspx?with=" + receiverId.ToString(CultureInfo.InvariantCulture);
                if (projectId.HasValue)
                    url += "&projectId=" + projectId.Value.ToString(CultureInfo.InvariantCulture);
                FlashMessageStore.Success("Your message was sent.");
                Response.Redirect(url, false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(attachmentPath))
                    MessageAttachmentStorage.Delete(attachmentPath);
                System.Diagnostics.Trace.TraceError("Sending a message failed: " + ex);
                string message = ex is InvalidOperationException
                    ? ex.Message
                    : "Your message could not be sent. Please try again.";
                ShowMessage(message, false);
            }
        }

        protected void rptConversations_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!string.Equals(e.CommandName, "ClearConversation", StringComparison.Ordinal))
                return;

            int partnerId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument, CultureInfo.InvariantCulture),
                NumberStyles.None, CultureInfo.InvariantCulture, out partnerId) || partnerId <= 0)
            {
                ShowMessage("Choose a valid conversation to clear.", false);
                return;
            }

            try
            {
                ServiceFactory.CreateMessagingService().DeleteConversationForUser(AuthenticatedUser.UserID, partnerId);
                FlashMessageStore.Success("The conversation was cleared from your inbox.");
                Response.Redirect(ResolveUrl("~/Messages/Messages.aspx"), false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Clearing a message conversation failed: " + ex);
                ShowMessage("This conversation could not be cleared. Please try again.", false);
                BindPage();
            }
        }

        private void BindPage()
        {
            int userId = AuthenticatedUser.UserID;
            IMessagingService service = ServiceFactory.CreateMessagingService();
            int partnerId;
            int.TryParse(Request.QueryString["with"], NumberStyles.None,
                CultureInfo.InvariantCulture, out partnerId);
            int requestedProjectId;
            bool hasRequestedProject = int.TryParse(Request.QueryString["projectId"], NumberStyles.None,
                CultureInfo.InvariantCulture, out requestedProjectId) && requestedProjectId > 0;
            int? projectId = hasRequestedProject ? (int?)requestedProjectId : null;

            string selectionError = null;
            if (partnerId > 0)
            {
                try
                {
                    service.EnsureConversation(userId, partnerId, projectId);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("Starting a message conversation failed: " + ex);
                    selectionError = ex is InvalidOperationException
                        ? ex.Message : "This conversation could not be started. Please try again.";
                    partnerId = 0;
                    projectId = null;
                }
            }

            IList<ConversationSummary> conversations = service.GetConversations(userId);

            if (partnerId <= 0 && conversations.Count > 0 && string.IsNullOrWhiteSpace(selectionError))
            {
                partnerId = conversations[0].PartnerID;
                projectId = conversations[0].ProjectID;
            }

            BindConversations(conversations, partnerId, projectId);
            litConversationCount.Text = conversations.Count.ToString(CultureInfo.InvariantCulture);
            pnlNoConversations.Visible = conversations.Count == 0;
            pnlNoSelection.Visible = partnerId <= 0;
            pnlChat.Visible = partnerId > 0;

            if (!string.IsNullOrWhiteSpace(selectionError))
                ShowMessage(selectionError, false);

            if (partnerId <= 0)
                return;

            try
            {
                User partner = service.GetPartner(userId, partnerId);
                if (partner == null)
                {
                    ShowMessage("This conversation partner could not be found.", false);
                    pnlChat.Visible = false;
                    pnlNoSelection.Visible = true;
                    return;
                }

                IList<MessageRecord> messages = service.GetConversation(userId, partnerId, projectId);
                service.MarkConversationRead(userId, partnerId, projectId);

                List<MessageItem> messageItems = new List<MessageItem>();
                foreach (MessageRecord message in messages)
                {
                    bool isSent = message.SenderID == userId;
                    bool hasAttachment = MessageAttachmentStorage.IsSafeRelativePath(message.AttachmentPath);
                    messageItems.Add(new MessageItem
                    {
                        IsSent = isSent,
                        RowClass = isSent ? "sent" : "received",
                        DisplayText = message.MessageText ?? string.Empty,
                        SentText = message.SentAt.ToString("dd MMM, h:mm tt", CultureInfo.InvariantCulture),
                        SenderInitials = GetInitials(message.SenderName),
                        HasAttachment = hasAttachment,
                        AttachmentName = hasAttachment
                            ? MessageAttachmentStorage.GetOriginalFileName(message.AttachmentPath)
                            : string.Empty,
                        AttachmentUrl = hasAttachment
                            ? "DownloadAttachment.ashx?id=" + message.MessageID.ToString(CultureInfo.InvariantCulture)
                            : string.Empty
                    });
                }

                rptConversationMessages.DataSource = messageItems;
                rptConversationMessages.DataBind();
                pnlThreadEmpty.Visible = messageItems.Count == 0;
                litPartnerInitials.Text = GetInitials(partner.FullName);
                litPartnerRole.Text = partner.Role;
                lnkPartnerProfile.Text = Server.HtmlEncode(partner.FullName);
                lnkPartnerProfile.NavigateUrl = partner.Role == "Freelancer"
                    ? ResolveUrl("~/Client/ViewFreelancerProfile.aspx?id=" + partnerId.ToString(CultureInfo.InvariantCulture))
                    : ResolveUrl("~/Freelancer/ViewClientProfile.aspx?id=" + partnerId.ToString(CultureInfo.InvariantCulture));
                litProjectTitle.Text = projectId.HasValue
                    ? (service.GetProjectTitle(userId, partnerId, projectId.Value) ?? "Project conversation")
                    : "Direct message";
                hidRecipientId.Value = partnerId.ToString(CultureInfo.InvariantCulture);
                hidProjectId.Value = projectId.HasValue
                    ? projectId.Value.ToString(CultureInfo.InvariantCulture)
                    : string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Loading a message conversation failed: " + ex);
                pnlChat.Visible = false;
                pnlNoSelection.Visible = true;
                ShowMessage(ex is InvalidOperationException
                    ? ex.Message
                    : "This conversation could not be loaded. Please try again.", false);
            }
        }

        private void BindConversations(
            IList<ConversationSummary> conversations,
            int selectedPartnerId,
            int? selectedProjectId)
        {
            List<ConversationItem> items = new List<ConversationItem>();
            foreach (ConversationSummary conversation in conversations)
            {
                bool selected = conversation.PartnerID == selectedPartnerId &&
                    conversation.ProjectID == selectedProjectId;
                string url = "Messages.aspx?with=" + conversation.PartnerID.ToString(CultureInfo.InvariantCulture);
                if (conversation.ProjectID.HasValue)
                    url += "&projectId=" + conversation.ProjectID.Value.ToString(CultureInfo.InvariantCulture);
                items.Add(new ConversationItem
                {
                    PartnerID = conversation.PartnerID,
                    PartnerName = conversation.PartnerName,
                    ProjectTitle = conversation.ProjectTitle,
                    Preview = conversation.Preview,
                    LastSentText = conversation.LastSentAt.ToString("dd MMM", CultureInfo.InvariantCulture),
                    Initials = GetInitials(conversation.PartnerName),
                    SelectionClass = selected ? "active-conversation" : string.Empty,
                    Url = url,
                    UnreadCount = conversation.UnreadCount
                });
            }
            rptConversations.DataSource = items;
            rptConversations.DataBind();
        }

        private void ShowMessage(string message, bool success)
        {
            lblMessage.Text = Server.HtmlEncode(message);
            lblMessage.CssClass = success ? "action-message success-message" : "action-message error-message";
            lblMessage.Visible = true;
        }

        private static string GetInitials(string fullName)
        {
            string[] names = (fullName ?? string.Empty).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (names.Length == 0) return "?";
            return names.Length == 1
                ? names[0].Substring(0, 1).ToUpperInvariant()
                : (names[0].Substring(0, 1) + names[names.Length - 1].Substring(0, 1)).ToUpperInvariant();
        }
    }
}
