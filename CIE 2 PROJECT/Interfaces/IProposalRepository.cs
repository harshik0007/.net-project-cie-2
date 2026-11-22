using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IProposalRepository
    {
        Proposal GetByIdForUpdate(int proposalId, IUnitOfWork unitOfWork);
        IList<Proposal> GetByProjectId(int projectId);
        IList<Proposal> GetByFreelancerId(int freelancerId);
        int GetCountByProjectId(int projectId);
        bool HasProposal(int projectId, int freelancerId, IUnitOfWork unitOfWork);
        int Create(Proposal proposal, IUnitOfWork unitOfWork);
        void Update(Proposal proposal, IUnitOfWork unitOfWork);
        void RejectOtherPending(int projectId, int acceptedProposalId, IUnitOfWork unitOfWork);
    }
}
