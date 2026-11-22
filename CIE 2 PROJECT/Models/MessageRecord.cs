using System;

namespace CIE_2_PROJECT.Models
{
    public sealed class MessageRecord
    {
        public int MessageID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public int? ProjectID { get; set; }
        public string MessageText { get; set; }
        public string AttachmentPath { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
        public string SenderName { get; set; }
    }

    public sealed class ConversationSummary
    {
        public int PartnerID { get; set; }
        public string PartnerName { get; set; }
        public string PartnerUsername { get; set; }
        public string PartnerRole { get; set; }
        public int? ProjectID { get; set; }
        public string ProjectTitle { get; set; }
        public string Preview { get; set; }
        public DateTime LastSentAt { get; set; }
        public int UnreadCount { get; set; }
    }
}
