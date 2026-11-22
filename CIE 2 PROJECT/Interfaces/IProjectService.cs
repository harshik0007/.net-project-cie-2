using System;
using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IProjectService
    {
        Project GetProject(int projectId);
        Project GetClientProject(int clientId, int projectId);
        IList<Project> GetClientProjects(int clientId);
        IList<Project> FindOpenProjects(string searchText, string category);
        int CreateProject(Project project);
        void UpdateProject(int clientId, int projectId, string title,
            string description, string category, string experienceLevel,
            string requiredSkills, string preferredLocation,
            decimal minBudget, decimal maxBudget,
            DateTime? deadline, string attachmentPath);
        void CancelProject(int clientId, int projectId);
    }
}
