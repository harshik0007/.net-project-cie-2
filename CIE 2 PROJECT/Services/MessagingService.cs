using System;
using System.Collections.Generic;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Services
{
    public sealed class MessagingService : IMessagingService
    {
        private readonly IMessageRepository messages;
        private readonly IUserRepository users;

        public MessagingService(IMessageRepository messages, IUserRepository users)
        {
            if (messages == null) throw new ArgumentNullException("messages");
            if (users == null) throw new ArgumentNullException("users");
            this.messages = messages;
            this.users = users;
        }

        public IList<ConversationSummary> GetConversations(int userId)
        {
            EnsureUserId(userId);
            return messages.GetConversations(userId);
        }

        public void EnsureConversation(int userId, int partnerId, int? projectId)
        {
            User current = users.GetById(userId);
            User partner = EnsurePartner(userId, partnerId);
            if (projectId.HasValue)
                EnsureProjectConversation(userId, partner, projectId.Value);

            int clientId = current.Role == "Client" ? current.UserID : partner.UserID;
            int freelancerId = current.Role == "Freelancer" ? current.UserID : partner.UserID;
            messages.EnsureConversation(clientId, freelancerId, projectId);
        }

        public void DeleteConversationForUser(int userId, int partnerId)
        {
            EnsurePartner(userId, partnerId);
            messages.DeleteConversationForUser(userId, partnerId);
        }

        public User GetPartner(int userId, int partnerId)
        {
            EnsureUserId(userId);
            EnsurePartner(userId, partnerId);
            return users.GetById(partnerId);
        }

        public IList<MessageRecord> GetConversation(int userId, int partnerId, int? projectId)
        {
            User partner = EnsurePartner(userId, partnerId);
            if (projectId.HasValue)
                EnsureProjectConversation(userId, partner, projectId.Value);
            return messages.GetConversation(userId, partnerId, projectId);
        }

        public void MarkConversationRead(int userId, int partnerId, int? projectId)
        {
            User partner = EnsurePartner(userId, partnerId);
            if (projectId.HasValue)
                EnsureProjectConversation(userId, partner, projectId.Value);
            messages.MarkConversationRead(userId, partnerId, projectId);
        }

        public int GetUnreadCount(int userId)
        {
            EnsureUserId(userId);
            return messages.GetUnreadCount(userId);
        }

        public string GetProjectTitle(int userId, int partnerId, int projectId)
        {
            User partner = EnsurePartner(userId, partnerId);
            EnsureProjectConversation(userId, partner, projectId);
            return messages.GetProjectTitle(projectId);
        }

        public int Send(
            int senderId,
            int receiverId,
            int? projectId,
            string messageText,
            string attachmentPath)
        {
            User sender = users.GetById(senderId);
            User receiver = EnsurePartner(senderId, receiverId);
            string text = (messageText ?? string.Empty).Trim();
            if (text.Length > 1000)
                throw new InvalidOperationException("Messages can contain up to 1,000 characters.");
            if (text.Length == 0 && string.IsNullOrWhiteSpace(attachmentPath))
                throw new InvalidOperationException("Write a message or attach a file before sending.");
            if (projectId.HasValue)
                EnsureProjectConversation(senderId, receiver, projectId.Value);

            int clientId = sender.Role == "Client" ? sender.UserID : receiver.UserID;
            int freelancerId = sender.Role == "Freelancer" ? sender.UserID : receiver.UserID;
            int messageId = messages.Create(
                senderId, receiverId, clientId, freelancerId, projectId, text, attachmentPath);
            string note = string.IsNullOrWhiteSpace(sender.FullName)
                ? "You have a new message."
                : "New message from " + sender.FullName + ".";
            NotificationService.NotifySafely(receiverId, "NewMessage", note, projectId);
            return messageId;
        }

        public string GetAttachmentPathForUser(int messageId, int userId)
        {
            EnsureUserId(userId);
            if (messageId <= 0)
                throw new ArgumentOutOfRangeException("messageId");
            string path = messages.GetAttachmentPathForParticipant(messageId, userId);
            if (string.IsNullOrWhiteSpace(path))
                throw new InvalidOperationException("The attachment was not found or access is denied.");
            return path;
        }

        private User EnsurePartner(int userId, int partnerId)
        {
            EnsureUserId(userId);
            if (partnerId <= 0 || partnerId == userId)
                throw new InvalidOperationException("Choose a valid conversation partner.");

            User current = users.GetById(userId);
            User partner = users.GetById(partnerId);
            if (current == null || partner == null)
                throw new InvalidOperationException("The conversation participant could not be found.");

            bool oppositeRoles =
                (current.Role == "Client" && partner.Role == "Freelancer") ||
                (current.Role == "Freelancer" && partner.Role == "Client");
            if (!oppositeRoles)
                throw new InvalidOperationException("Messages are available between clients and freelancers.");
            return partner;
        }

        private void EnsureProjectConversation(int userId, User partner, int projectId)
        {
            if (projectId <= 0)
                throw new InvalidOperationException("Choose a valid project conversation.");
            User current = users.GetById(userId);
            int clientId = current.Role == "Client" ? current.UserID : partner.UserID;
            int freelancerId = current.Role == "Freelancer" ? current.UserID : partner.UserID;
            if (!messages.IsProjectConversation(projectId, clientId, freelancerId))
                throw new InvalidOperationException("This project conversation is not available to these participants.");
        }

        private static void EnsureUserId(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException("userId");
        }
    }
}
