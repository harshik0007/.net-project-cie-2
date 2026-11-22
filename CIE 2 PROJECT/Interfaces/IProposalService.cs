using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IProposalService
    {
        IList<Proposal> GetProjectProposals(int clientId, int projectId, string sortBy);
        IList<Proposal> GetFreelancerProposals(int freelancerId);
        int GetProposalCount(int projectId);
        int SubmitProposal(Proposal proposal);
    }
}
