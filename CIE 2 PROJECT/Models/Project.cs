using System;

namespace CIE_2_PROJECT.Models
{
    public sealed class Project
    {
        public int ProjectID { get; private set; }
        public int ClientID { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public string ExperienceLevel { get; private set; }
        public string RequiredSkills { get; private set; }
        public string PreferredLocation { get; private set; }
        public decimal MinBudget { get; private set; }
        public decimal MaxBudget { get; private set; }
        public DateTime? Deadline { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string AttachmentPath { get; private set; }
        public string Status { get; private set; }

        public Project(
            int projectId,
            int clientId,
            string title,
            string description,
            string category,
            string experienceLevel,
            string requiredSkills,
            string preferredLocation,
            decimal minBudget,
            decimal maxBudget,
            DateTime? deadline,
            DateTime createdAt,
            string attachmentPath,
            string status)
        {
            if (projectId < 0)
                throw new ArgumentOutOfRangeException("projectId");
            if (clientId <= 0)
                throw new ArgumentOutOfRangeException("clientId");
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("A project title is required.", "title");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("A project description is required.", "description");
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("A project category is required.", "category");
            if (minBudget <= 0 || maxBudget < minBudget)
                throw new ArgumentException("The project budget range is invalid.");
            if (!IsProjectStatus(status))
                throw new ArgumentException("The project status is invalid.", "status");

            ProjectID = projectId;
            ClientID = clientId;
            Title = title.Trim();
            Description = description.Trim();
            Category = category.Trim();
            ExperienceLevel = experienceLevel;
            RequiredSkills = requiredSkills;
            PreferredLocation = preferredLocation;
            MinBudget = minBudget;
            MaxBudget = maxBudget;
            Deadline = deadline;
            CreatedAt = createdAt;
            AttachmentPath = attachmentPath;
            Status = status;
        }

        public void UpdateDetails(
            string title,
            string description,
            string category,
            string experienceLevel,
            string requiredSkills,
            string preferredLocation,
            decimal minBudget,
            decimal maxBudget,
            DateTime? deadline,
            string attachmentPath)
        {
            EnsureStatus(WorkflowStatuses.ProjectOpen);
            if (string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(description) ||
                string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Title, description, and category are required.");
            if (minBudget <= 0 || maxBudget < minBudget)
                throw new ArgumentException("The project budget range is invalid.");

            Title = title.Trim();
            Description = description.Trim();
            Category = category.Trim();
            ExperienceLevel = experienceLevel;
            RequiredSkills = requiredSkills;
            PreferredLocation = preferredLocation;
            MinBudget = minBudget;
            MaxBudget = maxBudget;
            Deadline = deadline;
            AttachmentPath = attachmentPath;
        }

        public void Start()
        {
            EnsureStatus(WorkflowStatuses.ProjectOpen);
            Status = WorkflowStatuses.ProjectInProgress;
        }

        public void Complete()
        {
            EnsureStatus(WorkflowStatuses.ProjectInProgress);
            Status = WorkflowStatuses.ProjectCompleted;
        }

        public void Cancel()
        {
            EnsureStatus(WorkflowStatuses.ProjectOpen);
            Status = WorkflowStatuses.ProjectCancelled;
        }

        public void CancelInProgress()
        {
            EnsureStatus(WorkflowStatuses.ProjectInProgress);
            Status = WorkflowStatuses.ProjectCancelled;
        }

        private void EnsureStatus(string expectedStatus)
        {
            if (!string.Equals(Status, expectedStatus, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "This operation is not allowed while the project is " + Status + ".");
        }

        private static bool IsProjectStatus(string status)
        {
            return status == WorkflowStatuses.ProjectOpen ||
                   status == WorkflowStatuses.ProjectInProgress ||
                   status == WorkflowStatuses.ProjectCompleted ||
                   status == WorkflowStatuses.ProjectCancelled;
        }
    }
}
