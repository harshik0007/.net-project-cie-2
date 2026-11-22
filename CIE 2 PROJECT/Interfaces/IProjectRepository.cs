using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IProjectRepository
    {
        Project GetById(int projectId);
        Project GetByIdForUpdate(int projectId, IUnitOfWork unitOfWork);
        IList<Project> GetByClientId(int clientId);
        int CountActiveByClientIdForUpdate(int clientId, IUnitOfWork unitOfWork);
        IList<Project> GetOpenProjects(string searchText, string category);
        int Create(Project project, IUnitOfWork unitOfWork);
        void Update(Project project, IUnitOfWork unitOfWork);
    }
}
