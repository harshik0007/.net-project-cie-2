using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IReviewService
    {
        IList<Review> GetReviewsForUser(int userId);
        IList<Review> GetReviewsGivenByUser(int reviewerId, string reviewerRole);
        decimal GetAverageRating(int userId);
        int GetReviewCount(int userId);
        bool HasReviewedContract(int contractId, int reviewerId);
        int SubmitReview(int contractId, int reviewerId, int revieweeId,
            string reviewerRole, int rating, string reviewText);
    }
}
