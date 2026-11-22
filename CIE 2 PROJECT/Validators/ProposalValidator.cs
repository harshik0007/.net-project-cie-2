using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Validators
{
    public sealed class ProposalValidator
    {
        public IList<string> Validate(Proposal proposal, Project project)
        {
            List<string> errors = new List<string>();
            if (proposal == null)
            {
                errors.Add("Proposal details are required.");
                return errors;
            }
            if (project == null)
            {
                errors.Add("The selected project could not be found.");
                return errors;
            }
            if (project.Status != WorkflowStatuses.ProjectOpen)
                errors.Add("This project is not accepting proposals.");
            if (proposal.ProjectID != project.ProjectID)
                errors.Add("The proposal does not match the selected project.");
            if (proposal.BidAmount < project.MinBudget ||
                proposal.BidAmount > project.MaxBudget)
                errors.Add("The proposed bid must be within the project's budget range.");

            return errors;
        }
    }
}
