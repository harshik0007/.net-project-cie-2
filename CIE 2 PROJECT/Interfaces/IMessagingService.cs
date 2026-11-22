using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IMessagingService
    {
        IList<ConversationSummary> GetConversations(int userId);
        void EnsureConversation(int userId, int partnerId, int? projectId);
        void DeleteConversationForUser(int userId, int partnerId);
        User GetPartner(int userId, int partnerId);
        IList<MessageRecord> GetConversation(int userId, int partnerId, int? projectId);
        void MarkConversationRead(int userId, int partnerId, int? projectId);
        int GetUnreadCount(int userId);
        string GetProjectTitle(int userId, int partnerId, int projectId);
        int Send(
            int senderId,
            int receiverId,
            int? projectId,
            string messageText,
            string attachmentPath);
        string GetAttachmentPathForUser(int messageId, int userId);
    }
}
