using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IMessageRepository
    {
        IList<ConversationSummary> GetConversations(int userId);
        IList<MessageRecord> GetConversation(int userId, int partnerId, int? projectId);
        int Create(
            int senderId,
            int receiverId,
            int clientId,
            int freelancerId,
            int? projectId,
            string messageText,
            string attachmentPath);
        void EnsureConversation(int clientId, int freelancerId, int? projectId);
        void DeleteConversationForUser(int userId, int partnerId);
        void MarkConversationRead(int userId, int partnerId, int? projectId);
        int GetUnreadCount(int userId);
        string GetAttachmentPathForParticipant(int messageId, int userId);
        bool IsProjectConversation(int projectId, int clientId, int freelancerId);
        string GetProjectTitle(int projectId);
    }
}
