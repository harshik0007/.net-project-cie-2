using System.Collections.Generic;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Interfaces
{
    public interface ISubmissionRepository
    {
        WorkSubmission GetById(int submissionId);
        WorkSubmission GetLatestByContractId(int contractId, IUnitOfWork unitOfWork);
        IList<WorkSubmission> GetByContractId(int contractId);
        IList<SubmissionFile> GetFilesBySubmissionId(int submissionId);
        int GetNextSubmissionNumber(int contractId, IUnitOfWork unitOfWork);
        int Create(WorkSubmission submission, IList<SubmissionFile> files, IUnitOfWork unitOfWork);
        void Update(WorkSubmission submission, IUnitOfWork unitOfWork);
    }
}
