using System;

namespace CIE_2_PROJECT.Models
{
    public sealed class Review
    {
        public int ReviewID { get; private set; }
        public int ContractID { get; private set; }
        public int ReviewerID { get; private set; }
        public int RevieweeID { get; private set; }
        public string ReviewerRole { get; private set; }
        public int Rating { get; private set; }
        public string ReviewText { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Review(
            int reviewId,
            int contractId,
            int reviewerId,
            int revieweeId,
            string reviewerRole,
            int rating,
            string reviewText,
            DateTime createdAt)
        {
            if (reviewId < 0)
                throw new ArgumentOutOfRangeException("reviewId");
            if (contractId <= 0 || reviewerId <= 0 || revieweeId <= 0)
                throw new ArgumentException("Review and contract participant IDs must be positive.");
            if (reviewerId == revieweeId)
                throw new ArgumentException("A participant cannot review themselves.");
            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException("rating", "Rating must be between 1 and 5.");
            if (reviewerRole != "Client" && reviewerRole != "Freelancer")
                throw new ArgumentException("The reviewer role is invalid.", "reviewerRole");

            ReviewID = reviewId;
            ContractID = contractId;
            ReviewerID = reviewerId;
            RevieweeID = revieweeId;
            ReviewerRole = reviewerRole;
            Rating = rating;
            ReviewText = reviewText == null ? string.Empty : reviewText.Trim();
            CreatedAt = createdAt;
        }
    }
}
