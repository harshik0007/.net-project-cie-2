using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IReviewRepository
    {
        bool ExistsForContract(int contractId, int reviewerId, IUnitOfWork unitOfWork);
        bool ExistsForContract(int contractId, int reviewerId);
        IList<Review> GetByRevieweeId(int revieweeId);
        IList<Review> GetByReviewerId(int reviewerId, string reviewerRole);
        decimal GetAverageRating(int revieweeId);
        int GetReviewCount(int revieweeId);
        int Create(Review review, IUnitOfWork unitOfWork);
    }
}
