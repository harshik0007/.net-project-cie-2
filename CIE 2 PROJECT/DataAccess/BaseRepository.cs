using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CIE_2_PROJECT.DataAccess
{
    public abstract class BaseRepository
    {
        private readonly string connectionString;

        protected BaseRepository()
            : this("SkillLinkDBConnection")
        {
        }

        protected BaseRepository(string connectionStringName)
        {
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings[connectionStringName];

            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
                throw new InvalidOperationException(
                    "The connection string '" + connectionStringName + "' is not configured.");

            connectionString = settings.ConnectionString;
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }

        protected static void AddParameter(
            SqlCommand command,
            string name,
            SqlDbType type,
            object value)
        {
            SqlParameter parameter = command.Parameters.Add(name, type);
            parameter.Value = value ?? DBNull.Value;
        }

        protected static void AddParameter(
            SqlCommand command,
            string name,
            SqlDbType type,
            int size,
            object value)
        {
            SqlParameter parameter = command.Parameters.Add(name, type, size);
            parameter.Value = value ?? DBNull.Value;
        }

        protected static object DbValue(object value)
        {
            return value == null ? DBNull.Value : value;
        }
    }
}
