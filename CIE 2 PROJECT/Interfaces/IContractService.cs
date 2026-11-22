using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IContractService
    {
        Contract GetForClient(int clientId, int contractId);
        Contract GetForClientByProjectId(int clientId, int projectId);
        Contract GetForFreelancer(int freelancerId, int contractId);
        IList<Contract> GetForClient(int clientId);
        IList<Contract> GetForFreelancerHistory(int freelancerId);
        IList<Contract> GetActiveForFreelancer(int freelancerId);
    }
}
