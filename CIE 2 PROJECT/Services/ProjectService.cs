using System;
using System.Collections.Generic;
using System.Data;
using CIE_2_PROJECT.DataAccess;
using CIE_2_PROJECT.Delegates;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;
using CIE_2_PROJECT.Validators;

namespace CIE_2_PROJECT.Services
{
    public sealed class ProjectService : WorkflowServiceBase, IProjectService
    {
        private readonly IProjectRepository projects;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly ProjectValidator validator;

        public ProjectService(
            IProjectRepository projects,
            IUnitOfWorkFactory unitOfWorkFactory,
            ProjectValidator validator,
            WorkflowAuditLogger auditLogger)
            : base(auditLogger)
        {
            if (projects == null)
                throw new ArgumentNullException("projects");
            if (unitOfWorkFactory == null)
                throw new ArgumentNullException("unitOfWorkFactory");
            if (validator == null)
                throw new ArgumentNullException("validator");

            this.projects = projects;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.validator = validator;
        }

        public Project GetProject(int projectId)
        {
            if (projectId <= 0)
                throw new ArgumentOutOfRangeException("projectId");
            return projects.GetById(projectId);
        }

        public Project GetClientProject(int clientId, int projectId)
        {
            EnsureUserId(clientId, "clientId");
            if (projectId <= 0)
                throw new ArgumentOutOfRangeException("projectId");

            Project project = projects.GetById(projectId);
            EnsureOwner(project, clientId);
            return project;
        }

        public IList<Project> GetClientProjects(int clientId)
        {
            EnsureUserId(clientId, "clientId");
            return projects.GetByClientId(clientId);
        }

        public IList<Project> FindOpenProjects(string searchText, string category)
        {
            return projects.GetOpenProjects(searchText, category);
        }

        public int CreateProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException("project");
            if (project.ProjectID != 0 || project.Status != WorkflowStatuses.ProjectOpen)
                throw new InvalidOperationException("A new project must start as Open for Proposals.");

            ThrowIfInvalid(validator.Validate(project));
            using (IUnitOfWork unit = unitOfWorkFactory.Begin(IsolationLevel.Serializable))
            {
                int activeProjectCount = projects.CountActiveByClientIdForUpdate(
                    project.ClientID, unit);
                if (activeProjectCount >= 2)
                    throw new InvalidOperationException(
                        "You can have up to 2 active projects at a time. Complete or cancel a project before posting another.");

                int projectId = projects.Create(project, unit);
                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        project.ClientID, "Project", projectId, "ProjectCreated",
                        null, WorkflowStatuses.ProjectOpen,
                        "Client created a project.", DateTime.Now),
                    unit);
                unit.Commit();
                return projectId;
            }
        }

        public void UpdateProject(
            int clientId,
            int projectId,
            string title,
            string description,
            string category,
            string experienceLevel,
            string requiredSkills,
            string preferredLocation,
            decimal minBudget,
            decimal maxBudget,
            DateTime? deadline,
            string attachmentPath)
        {
            EnsureUserId(clientId, "clientId");
            using (IUnitOfWork unit = unitOfWorkFactory.Begin(IsolationLevel.Serializable))
            {
                Project project = projects.GetByIdForUpdate(projectId, unit);
                EnsureOwner(project, clientId);

                string oldStatus = project.Status;
                project.UpdateDetails(title, description, category, experienceLevel,
                    requiredSkills, preferredLocation, minBudget, maxBudget, deadline, attachmentPath);
                ThrowIfInvalid(validator.Validate(project));
                projects.Update(project, unit);

                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        clientId, "Project", projectId, "ProjectUpdated",
                        oldStatus, project.Status, "Client updated project details.", DateTime.Now),
                    unit);
                unit.Commit();
            }
        }

        public void CancelProject(int clientId, int projectId)
        {
            EnsureUserId(clientId, "clientId");
            IList<Proposal> pendingProposals = new ProposalRepository().GetByProjectId(projectId);
            using (IUnitOfWork unit = unitOfWorkFactory.Begin(IsolationLevel.Serializable))
            {
                Project project = projects.GetByIdForUpdate(projectId, unit);
                EnsureOwner(project, clientId);
                string oldStatus = project.Status;
                project.Cancel();
                projects.Update(project, unit);

                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        clientId, "Project", projectId, "ProjectCancelled",
                        oldStatus, project.Status, "Client cancelled an open project.", DateTime.Now),
                    unit);
                unit.Commit();
                foreach (Proposal proposal in pendingProposals)
                {
                    if (proposal.Status == WorkflowStatuses.ProposalPending)
                    {
                        NotificationService.NotifySafely(
                            proposal.FreelancerID, "ProjectCancelled",
                            "The client cancelled the project " + project.Title + ".", project.ProjectID);
                    }
                }
            }
        }

        private static void EnsureOwner(Project project, int clientId)
        {
            if (project == null || project.ClientID != clientId)
                throw new InvalidOperationException("Project not found or access is denied.");
        }

        private static void EnsureUserId(int userId, string parameterName)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(parameterName);
        }

        private static void ThrowIfInvalid(IList<string> errors)
        {
            if (errors == null || errors.Count == 0)
                return;
            throw new InvalidOperationException(string.Join(" ", errors));
        }
    }
}
