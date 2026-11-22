using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.DataAccess
{
    public sealed class ProjectRepository : BaseRepository, IProjectRepository
    {
        private const string SelectColumns = @"
            ProjectID, ClientID, Title, Description, Category, ExperienceLevel,
            RequiredSkills, PreferredLocation, MinBudget, MaxBudget, Deadline, CreatedAt,
            AttachmentPath, Status";

        public Project GetById(int projectId)
        {
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(
                "SELECT " + SelectColumns + " FROM dbo.Projects WHERE ProjectID = @ProjectID;",
                connection))
            {
                AddParameter(command, "@ProjectID", SqlDbType.Int, projectId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                    return reader.Read() ? Map(reader) : null;
            }
        }

        public Project GetByIdForUpdate(int projectId, IUnitOfWork unitOfWork)
        {
            EnsureUnitOfWork(unitOfWork);
            using (SqlCommand command = new SqlCommand(
                "SELECT " + SelectColumns + " FROM dbo.Projects WITH (UPDLOCK, ROWLOCK) " +
                "WHERE ProjectID = @ProjectID;",
                unitOfWork.Connection,
                unitOfWork.Transaction))
            {
                AddParameter(command, "@ProjectID", SqlDbType.Int, projectId);
                using (SqlDataReader reader = command.ExecuteReader())
                    return reader.Read() ? Map(reader) : null;
            }
        }

        public IList<Project> GetByClientId(int clientId)
        {
            List<Project> projects = new List<Project>();
            string sql = "SELECT " + SelectColumns +
                " FROM dbo.Projects WHERE ClientID = @ClientID " +
                "ORDER BY CreatedAt DESC, ProjectID DESC;";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@ClientID", SqlDbType.Int, clientId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        projects.Add(Map(reader));
                }
            }

            return projects;
        }

        public int CountActiveByClientIdForUpdate(int clientId, IUnitOfWork unitOfWork)
        {
            EnsureUnitOfWork(unitOfWork);
            const string sql = @"
                SELECT COUNT(1)
                FROM dbo.Projects WITH (UPDLOCK, HOLDLOCK)
                WHERE ClientID = @ClientID
                  AND Status IN (@OpenStatus, @InProgressStatus);";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@ClientID", SqlDbType.Int, clientId);
                AddParameter(command, "@OpenStatus", SqlDbType.NVarChar, 40,
                    WorkflowStatuses.ProjectOpen);
                AddParameter(command, "@InProgressStatus", SqlDbType.NVarChar, 40,
                    WorkflowStatuses.ProjectInProgress);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public IList<Project> GetOpenProjects(string searchText, string category)
        {
            List<Project> projects = new List<Project>();
            const string sql = @"
                SELECT ProjectID, ClientID, Title, Description, Category,
                       ExperienceLevel, RequiredSkills, PreferredLocation,
                       MinBudget, MaxBudget,
                       Deadline, CreatedAt, AttachmentPath, Status
                FROM dbo.Projects
                WHERE Status = @OpenStatus
                  AND (@SearchText = N'' OR Title LIKE N'%' + @SearchText + N'%'
                       OR Description LIKE N'%' + @SearchText + N'%'
                       OR RequiredSkills LIKE N'%' + @SearchText + N'%'
                       OR Category LIKE N'%' + @SearchText + N'%')
                  AND (@Category = N'' OR Category = @Category)
                ORDER BY CreatedAt DESC, ProjectID DESC;";

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameter(command, "@OpenStatus", SqlDbType.NVarChar, 40,
                    WorkflowStatuses.ProjectOpen);
                AddParameter(command, "@SearchText", SqlDbType.NVarChar, 200,
                    (searchText ?? string.Empty).Trim());
                AddParameter(command, "@Category", SqlDbType.NVarChar, 100,
                    (category ?? string.Empty).Trim());
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        projects.Add(Map(reader));
                }
            }

            return projects;
        }

        public int Create(Project project, IUnitOfWork unitOfWork)
        {
            if (project == null)
                throw new ArgumentNullException("project");
            EnsureUnitOfWork(unitOfWork);

            const string sql = @"
                INSERT INTO dbo.Projects
                    (ClientID, Title, Description, Category, ExperienceLevel,
                     RequiredSkills, PreferredLocation, MinBudget, MaxBudget,
                     Deadline, CreatedAt,
                     AttachmentPath, Status)
                VALUES
                    (@ClientID, @Title, @Description, @Category, @ExperienceLevel,
                     @RequiredSkills, @PreferredLocation, @MinBudget, @MaxBudget,
                     @Deadline, @CreatedAt,
                     @AttachmentPath, @Status);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddProjectParameters(command, project);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(Project project, IUnitOfWork unitOfWork)
        {
            if (project == null)
                throw new ArgumentNullException("project");
            EnsureUnitOfWork(unitOfWork);

            const string sql = @"
                UPDATE dbo.Projects
                SET Title = @Title,
                    Description = @Description,
                    Category = @Category,
                    ExperienceLevel = @ExperienceLevel,
                    RequiredSkills = @RequiredSkills,
                    PreferredLocation = @PreferredLocation,
                    MinBudget = @MinBudget,
                    MaxBudget = @MaxBudget,
                    Deadline = @Deadline,
                    AttachmentPath = @AttachmentPath,
                    Status = @Status
                WHERE ProjectID = @ProjectID AND ClientID = @ClientID;";

            using (SqlCommand command = new SqlCommand(
                sql, unitOfWork.Connection, unitOfWork.Transaction))
            {
                AddParameter(command, "@ProjectID", SqlDbType.Int, project.ProjectID);
                AddProjectParameters(command, project);
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("The project was not found for this client.");
            }
        }

        private static Project Map(SqlDataReader reader)
        {
            return new Project(
                Convert.ToInt32(reader["ProjectID"]),
                Convert.ToInt32(reader["ClientID"]),
                Convert.ToString(reader["Title"]),
                Convert.ToString(reader["Description"]),
                Convert.ToString(reader["Category"]),
                Convert.ToString(reader["ExperienceLevel"]),
                reader["RequiredSkills"] == DBNull.Value ? null : Convert.ToString(reader["RequiredSkills"]),
                reader["PreferredLocation"] == DBNull.Value ? null : Convert.ToString(reader["PreferredLocation"]),
                Convert.ToDecimal(reader["MinBudget"]),
                Convert.ToDecimal(reader["MaxBudget"]),
                reader["Deadline"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["Deadline"]),
                Convert.ToDateTime(reader["CreatedAt"]),
                reader["AttachmentPath"] == DBNull.Value ? null : Convert.ToString(reader["AttachmentPath"]),
                Convert.ToString(reader["Status"]));
        }

        private static void AddProjectParameters(SqlCommand command, Project project)
        {
            AddParameter(command, "@ClientID", SqlDbType.Int, project.ClientID);
            AddParameter(command, "@Title", SqlDbType.NVarChar, 200, project.Title);
            AddParameter(command, "@Description", SqlDbType.NVarChar, -1, project.Description);
            AddParameter(command, "@Category", SqlDbType.NVarChar, 100, project.Category);
            AddParameter(command, "@ExperienceLevel", SqlDbType.NVarChar, 40,
                DbValue(project.ExperienceLevel));
            AddParameter(command, "@RequiredSkills", SqlDbType.NVarChar, 1000,
                DbValue(project.RequiredSkills));
            AddParameter(command, "@PreferredLocation", SqlDbType.NVarChar, 100,
                DbValue(project.PreferredLocation));
            AddParameter(command, "@MinBudget", SqlDbType.Decimal, project.MinBudget);
            AddParameter(command, "@MaxBudget", SqlDbType.Decimal, project.MaxBudget);
            command.Parameters["@MinBudget"].Precision = 18;
            command.Parameters["@MinBudget"].Scale = 2;
            command.Parameters["@MaxBudget"].Precision = 18;
            command.Parameters["@MaxBudget"].Scale = 2;
            AddParameter(command, "@Deadline", SqlDbType.DateTime2, DbValue(project.Deadline));
            AddParameter(command, "@CreatedAt", SqlDbType.DateTime2, project.CreatedAt);
            AddParameter(command, "@AttachmentPath", SqlDbType.NVarChar, 500,
                DbValue(project.AttachmentPath));
            AddParameter(command, "@Status", SqlDbType.NVarChar, 40, project.Status);
        }

        private static void EnsureUnitOfWork(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null || unitOfWork.Connection == null || unitOfWork.Transaction == null)
                throw new ArgumentNullException("unitOfWork");
        }
    }
}
