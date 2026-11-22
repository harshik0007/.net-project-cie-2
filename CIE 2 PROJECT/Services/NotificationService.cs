using System;
using System.Collections.Generic;
using CIE_2_PROJECT.DataAccess;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Services
{
    public static class NotificationService
    {
        public static void NotifySafely(int userId, string type, string message, int? projectId)
        {
            if (userId <= 0 || string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(message))
                return;

            try
            {
                string safeType = type.Trim();
                string safeMessage = message.Trim();
                if (safeType.Length > 50) safeType = safeType.Substring(0, 50);
                if (safeMessage.Length > 500) safeMessage = safeMessage.Substring(0, 500);
                new NotificationRepository().Create(userId, safeType, safeMessage, projectId);
            }
            catch (Exception ex)
            {
                // A notification is secondary to the workflow action; keep the committed action successful.
                System.Diagnostics.Trace.TraceError("Notification could not be saved: " + ex);
            }
        }

        public static IList<Notification> GetForUser(int userId, bool unreadOnly)
        {
            EnsureUserId(userId);
            return new NotificationRepository().GetForUser(userId, unreadOnly);
        }

        public static Notification GetForUser(int userId, int notificationId)
        {
            EnsureUserId(userId);
            if (notificationId <= 0)
                throw new ArgumentOutOfRangeException("notificationId");
            return new NotificationRepository().GetForUser(userId, notificationId);
        }

        public static int GetUnreadCount(int userId)
        {
            EnsureUserId(userId);
            return new NotificationRepository().GetUnreadCount(userId);
        }

        public static void MarkRead(int userId, int notificationId)
        {
            EnsureUserId(userId);
            if (notificationId <= 0)
                throw new ArgumentOutOfRangeException("notificationId");
            new NotificationRepository().MarkRead(userId, notificationId);
        }

        public static void DeleteForUser(int userId, int notificationId)
        {
            EnsureUserId(userId);
            if (notificationId <= 0)
                throw new ArgumentOutOfRangeException("notificationId");
            new NotificationRepository().DeleteForUser(userId, notificationId);
        }

        public static void MarkAllRead(int userId)
        {
            EnsureUserId(userId);
            new NotificationRepository().MarkAllRead(userId);
        }

        private static void EnsureUserId(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException("userId");
        }
    }
}
