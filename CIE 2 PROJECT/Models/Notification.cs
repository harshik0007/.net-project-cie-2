using System;

namespace CIE_2_PROJECT.Models
{
    public sealed class Notification
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public string Type { get; set; }
        public string MessageText { get; set; }
        public int? ProjectID { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProjectTitle { get; set; }
    }
}
