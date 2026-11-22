using System.Diagnostics;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Delegates
{
    public static class WorkflowDiagnosticHandler
    {
        public static void Record(WorkflowAuditEntry entry)
        {
            if (entry == null)
                return;

            try
            {
                Trace.WriteLine(
                    "SkillLink workflow: " + entry.Action + " " +
                    entry.EntityType + " #" + entry.EntityID +
                    " by user #" + entry.ActorUserID + " at " +
                    entry.CreatedAt.ToString("o"));
            }
            catch
            {
                // A development diagnostic must not affect a committed workflow.
            }
        }
    }
}
