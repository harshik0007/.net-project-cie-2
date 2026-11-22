using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface ISubmissionService
    {
        WorkSubmission GetLatestForClient(int clientId, int contractId);
        WorkSubmission GetLatestForFreelancer(int freelancerId, int contractId);
        IList<WorkSubmission> GetHistoryForClient(int clientId, int contractId);
        IList<SubmissionFile> GetFilesForClient(int clientId, int submissionId);
        IList<SubmissionFile> GetFilesForFreelancer(int freelancerId, int submissionId);
        int SubmitWork(int freelancerId, int contractId, string completionMessage,
            string demoUrl, string githubUrl, IList<UploadedFileInput> files);
        void RequestRevision(int clientId, int contractId, string feedback);
        void ApproveSubmission(int clientId, int contractId);
    }
}
