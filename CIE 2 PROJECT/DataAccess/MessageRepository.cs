using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.DataAccess
{
    public sealed class MessageRepository : BaseRepository, IMessageRepository
    {
        public IList<ConversationSummary> GetConversations(int userId)
        {
            const string sql = @"
                SELECT CASE WHEN thread.ClientID = @UserID THEN thread.FreelancerID ELSE thread.ClientID END AS PartnerID,
                       partner.FullName, partner.Username, partner.Role, thread.ProjectID,
                       COALESCE(project.Title, N'Direct message') AS ProjectTitle,
                       CASE WHEN NULLIF(LTRIM(RTRIM(latest.MessageText)), N'') IS NOT NULL
                            THEN latest.MessageText
                            WHEN latest.AttachmentPath IS NOT NULL THEN N'File attachment'
                            ELSE N'Start a conversation' END AS Preview,
                       COALESCE(latest.SentAt, thread.CreatedAt) AS LastSentAt,
                       (SELECT COUNT(1) FROM dbo.Messages AS unread
                        WHERE unread.ReceiverID = @UserID
                          AND unread.SenderID = CASE WHEN thread.ClientID = @UserID THEN thread.FreelancerID ELSE thread.ClientID END
                          AND ((unread.ProjectID = thread.ProjectID) OR (unread.ProjectID IS NULL AND thread.ProjectID IS NULL))
                          AND unread.IsRead = 0 AND unread.IsHiddenForReceiver = 0) AS UnreadCount
                FROM dbo.MessageThreads AS thread
                INNER JOIN dbo.Users AS partner
                    ON partner.UserID = CASE WHEN thread.ClientID = @UserID THEN thread.FreelancerID ELSE thread.ClientID END
                LEFT JOIN dbo.Projects AS project ON project.ProjectID = thread.ProjectID
                OUTER APPLY
                (
                    SELECT TOP (1) message.MessageText, message.AttachmentPath, message.SentAt
                    FROM dbo.Messages AS message
                    WHERE ((message.SenderID = @UserID AND message.ReceiverID = partner.UserID
                                AND message.IsHiddenForSender = 0)
                            OR (message.ReceiverID = @UserID AND message.SenderID = partner.UserID
                                AND message.IsHiddenForReceiver = 0))
                      AND ((message.ProjectID = thread.ProjectID)
                            OR (message.ProjectID IS NULL AND thread.ProjectID IS NULL))
                    ORDER BY message.SentAt DESC, message.MessageID DESC
                ) AS latest
                WHERE (thread.ClientID = @UserID AND thread.IsHiddenForClient = 0)
                   OR (thread.FreelancerID = @UserID AND thread.IsHiddenForFreelancer = 0)
                ORDER BY COALESCE(latest.SentAt, thread.CreatedAt) DESC, thread.ThreadID DESC;";
            List<ConversationSummary> results = new List<ConversationSummary>();
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@UserID", SqlDbType.Int, userId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new ConversationSummary
                        {
                            PartnerID = Convert.ToInt32(reader["PartnerID"]),
                            PartnerName = Convert.ToString(reader["FullName"]),
                            PartnerUsername = Convert.ToString(reader["Username"]),
                            PartnerRole = Convert.ToString(reader["Role"]),
                            ProjectID = reader["ProjectID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["ProjectID"]),
                            ProjectTitle = Convert.ToString(reader["ProjectTitle"]),
                            Preview = Convert.ToString(reader["Preview"]),
                            LastSentAt = Convert.ToDateTime(reader["LastSentAt"]),
                            UnreadCount = Convert.ToInt32(reader["UnreadCount"])
                        });
                    }
                }
            }
            return results;
        }

        public IList<MessageRecord> GetConversation(int userId, int partnerId, int? projectId)
        {
            const string sql = @"
                SELECT m.MessageID, m.SenderID, m.ReceiverID, m.ProjectID, m.MessageText,
                       m.AttachmentPath, m.IsRead, m.SentAt, sender.FullName AS SenderName
                FROM dbo.Messages AS m
                INNER JOIN dbo.Users AS sender ON sender.UserID = m.SenderID
                WHERE ((m.SenderID = @UserID AND m.ReceiverID = @PartnerID)
                    OR (m.SenderID = @PartnerID AND m.ReceiverID = @UserID))
                  AND ((m.SenderID = @UserID AND m.IsHiddenForSender = 0)
                    OR (m.ReceiverID = @UserID AND m.IsHiddenForReceiver = 0))
                  AND ((@ProjectID IS NULL AND m.ProjectID IS NULL) OR m.ProjectID = @ProjectID)
                ORDER BY m.SentAt ASC, m.MessageID ASC;";
            List<MessageRecord> results = new List<MessageRecord>();
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@UserID", SqlDbType.Int, userId);
                AddParameter(command, "@PartnerID", SqlDbType.Int, partnerId);
                AddParameter(command, "@ProjectID", SqlDbType.Int,
                    projectId.HasValue ? (object)projectId.Value : DBNull.Value);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new MessageRecord
                        {
                            MessageID = Convert.ToInt32(reader["MessageID"]),
                            SenderID = Convert.ToInt32(reader["SenderID"]),
                            ReceiverID = Convert.ToInt32(reader["ReceiverID"]),
                            ProjectID = reader["ProjectID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["ProjectID"]),
                            MessageText = Convert.ToString(reader["MessageText"]),
                            AttachmentPath = reader["AttachmentPath"] == DBNull.Value ? null : Convert.ToString(reader["AttachmentPath"]),
                            IsRead = Convert.ToBoolean(reader["IsRead"]),
                            SentAt = Convert.ToDateTime(reader["SentAt"]),
                            SenderName = Convert.ToString(reader["SenderName"])
                        });
                    }
                }
            }
            return results;
        }

        public int Create(
            int senderId,
            int receiverId,
            int clientId,
            int freelancerId,
            int? projectId,
            string messageText,
            string attachmentPath)
        {
            const string sql = @"
                INSERT INTO dbo.Messages
                    (SenderID, ReceiverID, ProjectID, MessageText, AttachmentPath, IsRead, SentAt)
                VALUES
                    (@SenderID, @ReceiverID, @ProjectID, @MessageText, @AttachmentPath, 0, GETDATE());
                SELECT CAST(SCOPE_IDENTITY() AS int);";
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    EnsureThread(connection, transaction, clientId, freelancerId, projectId);
                    using (SqlCommand command = new SqlCommand(sql, connection, transaction))
                    {
                        AddParameter(command, "@SenderID", SqlDbType.Int, senderId);
                        AddParameter(command, "@ReceiverID", SqlDbType.Int, receiverId);
                        AddParameter(command, "@ProjectID", SqlDbType.Int,
                            projectId.HasValue ? (object)projectId.Value : DBNull.Value);
                        AddParameter(command, "@MessageText", SqlDbType.NVarChar, 1000, messageText ?? string.Empty);
                        AddParameter(command, "@AttachmentPath", SqlDbType.NVarChar, 255,
                            string.IsNullOrWhiteSpace(attachmentPath) ? (object)DBNull.Value : attachmentPath);
                        int messageId = Convert.ToInt32(command.ExecuteScalar());
                        transaction.Commit();
                        return messageId;
                    }
                }
            }
        }

        public void EnsureConversation(int clientId, int freelancerId, int? projectId)
        {
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    EnsureThread(connection, transaction, clientId, freelancerId, projectId);
                    transaction.Commit();
                }
            }
        }

        private static void EnsureThread(
            SqlConnection connection,
            SqlTransaction transaction,
            int clientId,
            int freelancerId,
            int? projectId)
        {
            const string findSql = @"
                SELECT ThreadID
                FROM dbo.MessageThreads WITH (UPDLOCK, HOLDLOCK)
                WHERE ClientID = @ClientID AND FreelancerID = @FreelancerID
                  AND ((@ProjectID IS NULL AND ProjectID IS NULL) OR ProjectID = @ProjectID);";
            object threadId;
            using (SqlCommand find = new SqlCommand(findSql, connection, transaction))
            {
                AddParameter(find, "@ClientID", SqlDbType.Int, clientId);
                AddParameter(find, "@FreelancerID", SqlDbType.Int, freelancerId);
                AddParameter(find, "@ProjectID", SqlDbType.Int,
                    projectId.HasValue ? (object)projectId.Value : DBNull.Value);
                threadId = find.ExecuteScalar();
            }

            if (threadId == null || threadId == DBNull.Value)
            {
                const string insertSql = @"
                    INSERT INTO dbo.MessageThreads (ClientID, FreelancerID, ProjectID, CreatedAt)
                    VALUES (@ClientID, @FreelancerID, @ProjectID, GETDATE());";
                using (SqlCommand insert = new SqlCommand(insertSql, connection, transaction))
                {
                    AddParameter(insert, "@ClientID", SqlDbType.Int, clientId);
                    AddParameter(insert, "@FreelancerID", SqlDbType.Int, freelancerId);
                    AddParameter(insert, "@ProjectID", SqlDbType.Int,
                        projectId.HasValue ? (object)projectId.Value : DBNull.Value);
                    insert.ExecuteNonQuery();
                }
                return;
            }

            // Opening a profile's Message action restores the inbox entry for both
            // participants. Per-message hidden flags keep each user's cleared history private.
            string updateSql = @"UPDATE dbo.MessageThreads
                SET IsHiddenForClient = 0, IsHiddenForFreelancer = 0
                WHERE ThreadID = @ThreadID;";
            using (SqlCommand update = new SqlCommand(updateSql, connection, transaction))
            {
                AddParameter(update, "@ThreadID", SqlDbType.Int, Convert.ToInt32(threadId));
                update.ExecuteNonQuery();
            }
        }

        public void DeleteConversationForUser(int userId, int partnerId)
        {
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    const string hideMessagesSql = @"
                        UPDATE dbo.Messages
                        SET IsHiddenForSender = CASE WHEN SenderID = @UserID THEN 1 ELSE IsHiddenForSender END,
                            IsHiddenForReceiver = CASE WHEN ReceiverID = @UserID THEN 1 ELSE IsHiddenForReceiver END
                        WHERE (SenderID = @UserID AND ReceiverID = @PartnerID)
                           OR (SenderID = @PartnerID AND ReceiverID = @UserID);";
                    using (SqlCommand hideMessages = new SqlCommand(hideMessagesSql, connection, transaction))
                    {
                        AddParameter(hideMessages, "@UserID", SqlDbType.Int, userId);
                        AddParameter(hideMessages, "@PartnerID", SqlDbType.Int, partnerId);
                        hideMessages.ExecuteNonQuery();
                    }

                    const string hideThreadsSql = @"
                        UPDATE dbo.MessageThreads
                        SET IsHiddenForClient = CASE WHEN ClientID = @UserID THEN 1 ELSE IsHiddenForClient END,
                            IsHiddenForFreelancer = CASE WHEN FreelancerID = @UserID THEN 1 ELSE IsHiddenForFreelancer END
                        WHERE (ClientID = @UserID AND FreelancerID = @PartnerID)
                           OR (ClientID = @PartnerID AND FreelancerID = @UserID);";
                    using (SqlCommand hideThreads = new SqlCommand(hideThreadsSql, connection, transaction))
                    {
                        AddParameter(hideThreads, "@UserID", SqlDbType.Int, userId);
                        AddParameter(hideThreads, "@PartnerID", SqlDbType.Int, partnerId);
                        hideThreads.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }

        public void MarkConversationRead(int userId, int partnerId, int? projectId)
        {
            const string sql = @"
                UPDATE dbo.Messages SET IsRead = 1
                WHERE ReceiverID = @UserID AND SenderID = @PartnerID
                  AND IsHiddenForReceiver = 0
                  AND ((@ProjectID IS NULL AND ProjectID IS NULL) OR ProjectID = @ProjectID);";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@UserID", SqlDbType.Int, userId);
                AddParameter(command, "@PartnerID", SqlDbType.Int, partnerId);
                AddParameter(command, "@ProjectID", SqlDbType.Int,
                    projectId.HasValue ? (object)projectId.Value : DBNull.Value);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public int GetUnreadCount(int userId)
        {
            const string sql = @"SELECT COUNT(1) FROM dbo.Messages WHERE ReceiverID = @UserID AND IsRead = 0 AND IsHiddenForReceiver = 0;";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@UserID", SqlDbType.Int, userId);
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public string GetAttachmentPathForParticipant(int messageId, int userId)
        {
            const string sql = @"SELECT AttachmentPath FROM dbo.Messages
                WHERE MessageID = @MessageID
                  AND ((SenderID = @UserID AND IsHiddenForSender = 0)
                    OR (ReceiverID = @UserID AND IsHiddenForReceiver = 0));";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@MessageID", SqlDbType.Int, messageId);
                AddParameter(command, "@UserID", SqlDbType.Int, userId);
                connection.Open();
                object value = command.ExecuteScalar();
                return value == null || value == DBNull.Value ? null : Convert.ToString(value);
            }
        }

        public bool IsProjectConversation(int projectId, int clientId, int freelancerId)
        {
            const string sql = @"
                SELECT CASE WHEN EXISTS
                (
                    SELECT 1 FROM dbo.Projects AS p
                    WHERE p.ProjectID = @ProjectID AND p.ClientID = @ClientID
                      AND (EXISTS (SELECT 1 FROM dbo.Proposals AS pr
                                   WHERE pr.ProjectID = p.ProjectID AND pr.FreelancerID = @FreelancerID)
                           OR EXISTS (SELECT 1 FROM dbo.Contracts AS c
                                      WHERE c.ProjectID = p.ProjectID AND c.ClientID = p.ClientID
                                        AND c.FreelancerID = @FreelancerID))
                ) THEN 1 ELSE 0 END;";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@ProjectID", SqlDbType.Int, projectId);
                AddParameter(command, "@ClientID", SqlDbType.Int, clientId);
                AddParameter(command, "@FreelancerID", SqlDbType.Int, freelancerId);
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()) == 1;
            }
        }

        public string GetProjectTitle(int projectId)
        {
            const string sql = @"SELECT Title FROM dbo.Projects WHERE ProjectID = @ProjectID;";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@ProjectID", SqlDbType.Int, projectId);
                connection.Open();
                object value = command.ExecuteScalar();
                return value == null || value == DBNull.Value ? null : Convert.ToString(value);
            }
        }
    }
}
