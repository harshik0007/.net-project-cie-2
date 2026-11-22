using System;
using System.Collections.Generic;
using System.Data;
using CIE_2_PROJECT.Delegates;
using CIE_2_PROJECT.Interfaces;
using CIE_2_PROJECT.Models;
using CIE_2_PROJECT.Validators;

namespace CIE_2_PROJECT.Services
{
    public sealed class ReviewService : WorkflowServiceBase, IReviewService
    {
        private readonly IContractRepository contracts;
        private readonly IReviewRepository reviews;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly ReviewValidator validator;

        public ReviewService(
            IContractRepository contracts,
            IReviewRepository reviews,
            IUnitOfWorkFactory unitOfWorkFactory,
            ReviewValidator validator,
            WorkflowAuditLogger auditLogger)
            : base(auditLogger)
        {
            if (contracts == null) throw new ArgumentNullException("contracts");
            if (reviews == null) throw new ArgumentNullException("reviews");
            if (unitOfWorkFactory == null) throw new ArgumentNullException("unitOfWorkFactory");
            if (validator == null) throw new ArgumentNullException("validator");

            this.contracts = contracts;
            this.reviews = reviews;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.validator = validator;
        }

        public IList<Review> GetReviewsForUser(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException("userId");
            return reviews.GetByRevieweeId(userId);
        }

        public IList<Review> GetReviewsGivenByUser(int reviewerId, string reviewerRole)
        {
            if (reviewerId <= 0)
                throw new ArgumentOutOfRangeException("reviewerId");
            if (reviewerRole != "Client" && reviewerRole != "Freelancer")
                throw new ArgumentException("The reviewer role is invalid.", "reviewerRole");
            return reviews.GetByReviewerId(reviewerId, reviewerRole);
        }

        public decimal GetAverageRating(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException("userId");
            return reviews.GetAverageRating(userId);
        }

        public int GetReviewCount(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException("userId");
            return reviews.GetReviewCount(userId);
        }

        public bool HasReviewedContract(int contractId, int reviewerId)
        {
            if (contractId <= 0 || reviewerId <= 0)
                throw new ArgumentOutOfRangeException("IDs must be positive.");
            Contract contract = contracts.GetById(contractId);
            if (contract == null ||
                (contract.ClientID != reviewerId && contract.FreelancerID != reviewerId))
                throw new InvalidOperationException("Only contract participants can view review state.");
            return reviews.ExistsForContract(contractId, reviewerId);
        }

        public int SubmitReview(
            int contractId,
            int reviewerId,
            int revieweeId,
            string reviewerRole,
            int rating,
            string reviewText)
        {
            if (contractId <= 0 || reviewerId <= 0 || revieweeId <= 0)
                throw new ArgumentOutOfRangeException("IDs must be positive.");

            using (IUnitOfWork unit = unitOfWorkFactory.Begin(IsolationLevel.Serializable))
            {
                Contract contract = contracts.GetByIdForUpdate(contractId, unit);
                if (contract == null)
                    throw new InvalidOperationException("Contract not found or access is denied.");

                string requiredRole;
                int expectedReviewee;
                if (reviewerId == contract.ClientID)
                {
                    requiredRole = "Client";
                    expectedReviewee = contract.FreelancerID;
                }
                else if (reviewerId == contract.FreelancerID)
                {
                    requiredRole = "Freelancer";
                    expectedReviewee = contract.ClientID;
                }
                else
                {
                    throw new InvalidOperationException("Only participants in this contract can leave a review.");
                }

                if (!string.Equals(reviewerRole, requiredRole, StringComparison.Ordinal))
                    throw new InvalidOperationException("The reviewer role does not match this participant.");
                if (revieweeId != expectedReviewee)
                    throw new InvalidOperationException("A participant can only review the other person in the contract.");
                if (reviews.ExistsForContract(contractId, reviewerId, unit))
                    throw new InvalidOperationException("You have already reviewed this contract.");

                Review review = new Review(
                    0, contractId, reviewerId, revieweeId, requiredRole,
                    rating, reviewText, DateTime.Now);
                ThrowIfInvalid(validator.Validate(review, contract));
                int reviewId = reviews.Create(review, unit);

                PublishWorkflowChange(
                    new WorkflowAuditEntry(
                        reviewerId, "Review", reviewId, "ReviewSubmitted",
                        null, null, "A contract participant submitted a review.", review.CreatedAt),
                    unit);
                unit.Commit();
                NotificationService.NotifySafely(
                    revieweeId, "ReviewReceived",
                    "You received a new " + rating.ToString(System.Globalization.CultureInfo.InvariantCulture) + "-star review.",
                    contract.ProjectID);
                return reviewId;
            }
        }

        private static void ThrowIfInvalid(IList<string> errors)
        {
            if (errors == null || errors.Count == 0)
                return;
            throw new InvalidOperationException(string.Join(" ", errors));
        }
    }
}
