using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IPaymentRepository
    {
        Payment GetByContractId(int contractId, IUnitOfWork unitOfWork);
        IList<Payment> GetByClientId(int clientId);
        IList<Payment> GetByFreelancerId(int freelancerId);
        int Create(Payment payment, IUnitOfWork unitOfWork);
        void Update(Payment payment, IUnitOfWork unitOfWork);
    }
}
