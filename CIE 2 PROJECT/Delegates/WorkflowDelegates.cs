using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Delegates
{
    public delegate void WorkflowAuditHandler(WorkflowAuditEntry entry);
    public delegate int ProposalComparisonHandler(Proposal left, Proposal right);
}
