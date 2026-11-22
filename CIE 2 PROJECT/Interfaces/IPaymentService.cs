using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IPaymentService
    {
        int FundContract(int clientId, int contractId);
        int FundProposal(int clientId, int projectId, int proposalId);
        void ReleaseContractPayment(int clientId, int contractId);
        void RefundCancelledContract(int clientId, int contractId);
        IList<Payment> GetClientPaymentHistory(int clientId);
        IList<Payment> GetFreelancerPaymentHistory(int freelancerId);
    }
}
