using System;
using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Validators
{
    public sealed class ProjectValidator
    {
        public IList<string> Validate(Project project)
        {
            List<string> errors = new List<string>();

            if (project == null)
            {
                errors.Add("Project details are required.");
                return errors;
            }

            if (project.Deadline.HasValue &&
                project.Deadline.Value.Date < DateTime.Today)
                errors.Add("The project deadline cannot be in the past.");

            if (project.MinBudget > project.MaxBudget)
                errors.Add("Minimum budget cannot exceed maximum budget.");

            return errors;
        }
    }
}
