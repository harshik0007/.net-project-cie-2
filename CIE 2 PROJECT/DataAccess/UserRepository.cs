using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.DataAccess
{
    public sealed class UserRepository : BaseRepository, IUserRepository
    {
        public bool UsernameOrEmailExists(string username, string email)
        {
            const string sql = @"SELECT CASE WHEN EXISTS (
                SELECT 1 FROM dbo.Users WHERE Username = @Username OR Email = @Email
            ) THEN 1 ELSE 0 END;";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@Username", SqlDbType.NVarChar, 50, username);
                AddParameter(command, "@Email", SqlDbType.NVarChar, 100, email);
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()) == 1;
            }
        }

        public LoginAccount GetLoginAccount(string emailOrUsername)
        {
            const string sql = @"SELECT TOP (1) UserID, Username, Email, Role, PasswordHash
                FROM dbo.Users WHERE Username = @Login OR Email = @Login;";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@Login", SqlDbType.NVarChar, 100, emailOrUsername.Trim());
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    return new LoginAccount
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        Username = Convert.ToString(reader["Username"]),
                        Email = Convert.ToString(reader["Email"]),
                        Role = Convert.ToString(reader["Role"]),
                        PasswordHash = Convert.ToString(reader["PasswordHash"])
                    };
                }
            }
        }

        public int CreateAccount(string fullName, string username, string email, string passwordHash,
            string role, string phoneNumber, string profileImage, string skills)
        {
            const string sql = @"INSERT INTO dbo.Users
                (FullName, Username, Email, PasswordHash, Role, PhoneNumber, ProfileImage, Skills, CreatedAt)
                VALUES (@FullName, @Username, @Email, @PasswordHash, @Role, @PhoneNumber, @ProfileImage, @Skills, GETDATE());
                SELECT CAST(SCOPE_IDENTITY() AS int);";
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@FullName", SqlDbType.NVarChar, 100, fullName);
                AddParameter(command, "@Username", SqlDbType.NVarChar, 50, username);
                AddParameter(command, "@Email", SqlDbType.NVarChar, 100, email);
                AddParameter(command, "@PasswordHash", SqlDbType.NVarChar, 255, passwordHash);
                AddParameter(command, "@Role", SqlDbType.NVarChar, 20, role);
                AddParameter(command, "@PhoneNumber", SqlDbType.NVarChar, 30, DbValue(phoneNumber));
                AddParameter(command, "@ProfileImage", SqlDbType.NVarChar, 255, DbValue(profileImage));
                AddParameter(command, "@Skills", SqlDbType.NVarChar, 500, DbValue(skills));
                connection.Open();
                try
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 207)
                        throw new InvalidOperationException("The SkillLinkDB schema is missing profile fields. Contact the project maintainer.", ex);
                    throw;
                }
            }
        }

        public User GetById(int userId)
        {
            const string sql = @"
                SELECT *
                FROM dbo.Users
                WHERE UserID = @UserID;";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@UserID", SqlDbType.Int, userId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return Map(reader);
                }
            }
        }

        public IList<User> GetByRole(string role)
        {
            List<User> users = new List<User>();
            const string sql = @"
                SELECT *
                FROM dbo.Users
                WHERE Role = @Role
                ORDER BY FullName, UserID;";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@Role", SqlDbType.NVarChar, 20, role);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        users.Add(Map(reader));
                }
            }

            return users;
        }

        public void UpdateProfile(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            const string sql = @"
                UPDATE dbo.Users
                SET FullName = @FullName,
                    Bio = @Bio,
                    Location = @Location,
                    Skills = @Skills,
                    Education = @Education,
                    ContactEmail = @ContactEmail,
                    PhoneNumber = @PhoneNumber,
                    OrganizationName = @OrganizationName,
                    PortfolioUrl = @PortfolioUrl,
                    PreferredPaymentMethod = @PreferredPaymentMethod,
                    PaymentAccountLabel = @PaymentAccountLabel,
                    ProfileImage = @ProfileImage
                WHERE UserID = @UserID;";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@UserID", SqlDbType.Int, user.UserID);
                AddParameter(command, "@FullName", SqlDbType.NVarChar, 100, user.FullName);
                AddParameter(command, "@Bio", SqlDbType.NVarChar, 500, DbValue(user.Bio));
                AddParameter(command, "@Location", SqlDbType.NVarChar, 100, DbValue(user.Location));
                AddParameter(command, "@Skills", SqlDbType.NVarChar, 500, DbValue(user.Skills));
                AddParameter(command, "@Education", SqlDbType.NVarChar, 250, DbValue(user.Education));
                AddParameter(command, "@ContactEmail", SqlDbType.NVarChar, 100, DbValue(user.ContactEmail));
                AddParameter(command, "@PhoneNumber", SqlDbType.NVarChar, 30, DbValue(user.PhoneNumber));
                AddParameter(command, "@OrganizationName", SqlDbType.NVarChar, 150, DbValue(user.OrganizationName));
                AddParameter(command, "@PortfolioUrl", SqlDbType.NVarChar, 500, DbValue(user.PortfolioUrl));
                AddParameter(command, "@PreferredPaymentMethod", SqlDbType.NVarChar, 40, DbValue(user.PreferredPaymentMethod));
                AddParameter(command, "@PaymentAccountLabel", SqlDbType.NVarChar, 120, DbValue(user.PaymentAccountLabel));
                AddParameter(command, "@ProfileImage", SqlDbType.NVarChar, 255, DbValue(user.ProfileImage));
                connection.Open();
                try
                {
                    if (command.ExecuteNonQuery() != 1)
                        throw new InvalidOperationException("The profile could not be saved.");
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 207)
                        throw new InvalidOperationException(
                            "The SkillLinkDB schema is missing profile fields. Contact the project maintainer before saving these details.", ex);
                    throw;
                }
            }
        }

        private static User Map(SqlDataReader reader)
        {
            return new User(
                Convert.ToInt32(reader["UserID"]),
                Convert.ToString(reader["FullName"]),
                Convert.ToString(reader["Username"]),
                Convert.ToString(reader["Email"]),
                Convert.ToString(reader["Role"]),
                reader["Bio"] == DBNull.Value ? null : Convert.ToString(reader["Bio"]),
                reader["Location"] == DBNull.Value ? null : Convert.ToString(reader["Location"]),
                reader["ProfileImage"] == DBNull.Value ? null : Convert.ToString(reader["ProfileImage"]),
                reader["Skills"] == DBNull.Value ? null : Convert.ToString(reader["Skills"]),
                GetOptionalString(reader, "Education"),
                GetOptionalString(reader, "ContactEmail"),
                GetOptionalString(reader, "PhoneNumber"),
                GetOptionalString(reader, "OrganizationName"),
                GetOptionalString(reader, "PortfolioUrl"),
                GetOptionalString(reader, "PreferredPaymentMethod"),
                GetOptionalString(reader, "PaymentAccountLabel"),
                Convert.ToDateTime(reader["CreatedAt"]));
        }

        private static string GetOptionalString(SqlDataReader reader, string columnName)
        {
            for (int index = 0; index < reader.FieldCount; index++)
            {
                if (!string.Equals(reader.GetName(index), columnName, StringComparison.OrdinalIgnoreCase))
                    continue;
                return reader.IsDBNull(index) ? null : Convert.ToString(reader.GetValue(index));
            }

            return null;
        }
    }
}
