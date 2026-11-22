using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Validators
{
    public sealed class ReviewValidator
    {
        public IList<string> Validate(Review review, Contract contract)
        {
            List<string> errors = new List<string>();
            if (review == null)
            {
                errors.Add("Review details are required.");
                return errors;
            }
            if (contract == null)
            {
                errors.Add("The completed contract could not be found.");
                return errors;
            }
            if (contract.ApprovalStatus != WorkflowStatuses.ContractCompleted)
                errors.Add("Reviews are available after a contract is completed.");
            if (review.ContractID != contract.ContractID)
                errors.Add("The review does not match the completed contract.");
            if (review.ReviewerID == review.RevieweeID)
                errors.Add("A participant cannot review themselves.");
            if (review.ReviewerID != contract.ClientID &&
                review.ReviewerID != contract.FreelancerID)
                errors.Add("Only participants in the contract can submit a review.");
            if (review.RevieweeID != contract.ClientID &&
                review.RevieweeID != contract.FreelancerID)
                errors.Add("A review can only be given to the other contract participant.");
            if (string.IsNullOrWhiteSpace(review.ReviewText))
                errors.Add("A written review is required.");
            else if (review.ReviewText.Length > 2000)
                errors.Add("A review cannot exceed 2,000 characters.");

            return errors;
        }
    }
}
