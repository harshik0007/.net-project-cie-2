using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.DataAccess
{
    public sealed class NotificationRepository : BaseRepository
    {
        public void Create(int userId, string type, string message, int? projectId)
        {
            const string sql = @"
                INSERT INTO dbo.Notifications
                    (UserID, NotificationType, Message, ProjectID, IsRead, CreatedAt)
                VALUES
                    (@UserID, @NotificationType, @Message, @ProjectID, 0, GETDATE());";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@UserID", SqlDbType.Int, userId);
                AddParameter(command, "@NotificationType", SqlDbType.NVarChar, 50, type);
                AddParameter(command, "@Message", SqlDbType.NVarChar, 500, message);
                AddParameter(command, "@ProjectID", SqlDbType.Int,
                    projectId.HasValue ? (object)projectId.Value : DBNull.Value);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IList<Notification> GetForUser(int userId, bool unreadOnly)
        {
            const string sql = @"
                SELECT n.NotificationID, n.UserID, n.NotificationType, n.Message,
                       n.ProjectID, n.IsRead, n.CreatedAt, p.Title AS ProjectTitle
                FROM dbo.Notifications AS n
                LEFT JOIN dbo.Projects AS p ON p.ProjectID = n.ProjectID
                WHERE n.UserID = @UserID AND (@UnreadOnly = 0 OR n.IsRead = 0)
                ORDER BY n.CreatedAt DESC, n.NotificationID DESC;";
            List<Notification> results = new List<Notification>();
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@UserID", SqlDbType.Int, userId);
                AddParameter(command, "@UnreadOnly", SqlDbType.Bit, unreadOnly);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        results.Add(Map(reader));
                }
            }
            return results;
        }

        public Notification GetForUser(int userId, int notificationId)
        {
            const string sql = @"
                SELECT n.NotificationID, n.UserID, n.NotificationType, n.Message,
                       n.ProjectID, n.IsRead, n.CreatedAt, p.Title AS ProjectTitle
                FROM dbo.Notifications AS n
                LEFT JOIN dbo.Projects AS p ON p.ProjectID = n.ProjectID
                WHERE n.UserID = @UserID AND n.NotificationID = @NotificationID;";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@UserID", SqlDbType.Int, userId);
                AddParameter(command, "@NotificationID", SqlDbType.Int, notificationId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                    return reader.Read() ? Map(reader) : null;
            }
        }

        public int GetUnreadCount(int userId)
        {
            const string sql = @"SELECT COUNT(1) FROM dbo.Notifications WHERE UserID = @UserID AND IsRead = 0;";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@UserID", SqlDbType.Int, userId);
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void MarkRead(int userId, int notificationId)
        {
            const string sql = @"UPDATE dbo.Notifications SET IsRead = 1 WHERE UserID = @UserID AND NotificationID = @NotificationID;";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@UserID", SqlDbType.Int, userId);
                AddParameter(command, "@NotificationID", SqlDbType.Int, notificationId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteForUser(int userId, int notificationId)
        {
            const string sql = @"DELETE FROM dbo.Notifications
                WHERE UserID = @UserID AND NotificationID = @NotificationID;";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@UserID", SqlDbType.Int, userId);
                AddParameter(command, "@NotificationID", SqlDbType.Int, notificationId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void MarkAllRead(int userId)
        {
            const string sql = @"UPDATE dbo.Notifications SET IsRead = 1 WHERE UserID = @UserID AND IsRead = 0;";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@UserID", SqlDbType.Int, userId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private static Notification Map(SqlDataReader reader)
        {
            return new Notification
            {
                NotificationID = Convert.ToInt32(reader["NotificationID"]),
                UserID = Convert.ToInt32(reader["UserID"]),
                Type = Convert.ToString(reader["NotificationType"]),
                MessageText = Convert.ToString(reader["Message"]),
                ProjectID = reader["ProjectID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["ProjectID"]),
                IsRead = Convert.ToBoolean(reader["IsRead"]),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                ProjectTitle = reader["ProjectTitle"] == DBNull.Value ? null : Convert.ToString(reader["ProjectTitle"])
            };
        }
    }
}
