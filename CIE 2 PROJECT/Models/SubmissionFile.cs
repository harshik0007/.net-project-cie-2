using System;

namespace CIE_2_PROJECT.Models
{
    public sealed class SubmissionFile
    {
        public int SubmissionFileID { get; private set; }
        public int SubmissionID { get; private set; }
        public string OriginalFileName { get; private set; }
        public string StoredFileName { get; private set; }
        public string FilePath { get; private set; }
        public string ContentType { get; private set; }
        public long FileSize { get; private set; }
        public DateTime UploadedAt { get; private set; }

        public SubmissionFile(
            int submissionFileId,
            int submissionId,
            string originalFileName,
            string storedFileName,
            string filePath,
            string contentType,
            long fileSize,
            DateTime uploadedAt)
        {
            if (submissionFileId < 0)
                throw new ArgumentOutOfRangeException("submissionFileId");
            if (submissionId < 0)
                throw new ArgumentOutOfRangeException("submissionId");
            if (string.IsNullOrWhiteSpace(originalFileName) ||
                string.IsNullOrWhiteSpace(storedFileName) ||
                string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Original name, stored name, and path are required.");
            if (fileSize <= 0)
                throw new ArgumentOutOfRangeException("fileSize");

            SubmissionFileID = submissionFileId;
            SubmissionID = submissionId;
            OriginalFileName = originalFileName.Trim();
            StoredFileName = storedFileName.Trim();
            FilePath = filePath.Trim();
            ContentType = contentType;
            FileSize = fileSize;
            UploadedAt = uploadedAt;
        }
    }
}
