using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IContractRepository
    {
        Contract GetById(int contractId);
        Contract GetByProjectId(int projectId);
        Contract GetByIdForUpdate(int contractId, IUnitOfWork unitOfWork);
        Contract GetByProjectId(int projectId, IUnitOfWork unitOfWork);
        IList<Contract> GetByClientId(int clientId);
        IList<Contract> GetByFreelancerId(int freelancerId);
        IList<Contract> GetActiveByFreelancerId(int freelancerId);
        int CountActiveByFreelancerIdForUpdate(int freelancerId, IUnitOfWork unitOfWork);
        int Create(Contract contract, IUnitOfWork unitOfWork);
        void Update(Contract contract, IUnitOfWork unitOfWork);
    }
}
