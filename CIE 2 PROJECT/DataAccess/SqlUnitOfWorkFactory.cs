using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CIE_2_PROJECT.Interfaces;

namespace CIE_2_PROJECT.DataAccess
{
    public sealed class SqlUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly string connectionString;

        public SqlUnitOfWorkFactory()
            : this("SkillLinkDBConnection")
        {
        }

        public SqlUnitOfWorkFactory(string connectionStringName)
        {
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings[connectionStringName];

            if (settings == null)
                throw new System.Configuration.ConfigurationErrorsException(
                    "The connection string '" + connectionStringName + "' is not configured.");

            connectionString = settings.ConnectionString;
        }

        public IUnitOfWork Begin(IsolationLevel isolationLevel)
        {
            return new SqlUnitOfWork(connectionString, isolationLevel);
        }
    }
}
