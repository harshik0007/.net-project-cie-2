using System;

namespace CIE_2_PROJECT.Models
{
    public sealed class WorkflowAuditEntry
    {
        public int ActorUserID { get; private set; }
        public string EntityType { get; private set; }
        public int EntityID { get; private set; }
        public string Action { get; private set; }
        public string OldStatus { get; private set; }
        public string NewStatus { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public WorkflowAuditEntry(
            int actorUserId,
            string entityType,
            int entityId,
            string action,
            string oldStatus,
            string newStatus,
            string description,
            DateTime createdAt)
        {
            if (actorUserId <= 0)
                throw new ArgumentOutOfRangeException("actorUserId");
            if (string.IsNullOrWhiteSpace(entityType))
                throw new ArgumentException("An entity type is required.", "entityType");
            if (entityId <= 0)
                throw new ArgumentOutOfRangeException("entityId");
            if (string.IsNullOrWhiteSpace(action))
                throw new ArgumentException("An action is required.", "action");

            ActorUserID = actorUserId;
            EntityType = entityType.Trim();
            EntityID = entityId;
            Action = action.Trim();
            OldStatus = oldStatus;
            NewStatus = newStatus;
            Description = description;
            CreatedAt = createdAt;
        }
    }
}
