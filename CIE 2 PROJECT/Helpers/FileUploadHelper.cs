using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using CIE_2_PROJECT.Models;
using CIE_2_PROJECT.Validators;

namespace CIE_2_PROJECT.Helpers
{
    public sealed class FileUploadHelper
    {
        private const int MaximumFilesPerSubmission = 10;
        private readonly FileValidator validator;

        public FileUploadHelper(FileValidator validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");
            this.validator = validator;
        }

        public SavedFileBatch SaveWorkFiles(
            int contractId,
            IList<UploadedFileInput> inputs)
        {
            if (contractId <= 0)
                throw new ArgumentOutOfRangeException("contractId");
            if (inputs == null || inputs.Count == 0)
                throw new ArgumentException("Attach at least one completed work file.", "inputs");
            if (inputs.Count > MaximumFilesPerSubmission)
                throw new ArgumentException("A submission can include up to 10 files.", "inputs");
            if (HttpContext.Current == null)
                throw new InvalidOperationException("Work files can only be saved during a web request.");

            SavedFileBatch batch = new SavedFileBatch();
            string relativeDirectory = "Uploads/WorkSubmissions/" + contractId;
            string physicalDirectory = HttpContext.Current.Server.MapPath(
                "~/" + relativeDirectory.Replace('/', '\\'));

            try
            {
                Directory.CreateDirectory(physicalDirectory);
                foreach (UploadedFileInput input in inputs)
                    SaveOne(input, relativeDirectory, physicalDirectory, batch);
                return batch;
            }
            catch
            {
                batch.Cleanup();
                throw;
            }
        }

        private void SaveOne(
            UploadedFileInput input,
            string relativeDirectory,
            string physicalDirectory,
            SavedFileBatch batch)
        {
            if (input == null)
                throw new ArgumentException("An upload entry cannot be empty.", "input");

            string error;
            if (!validator.IsValid(input.FileName, input.FileSize, out error))
                throw new InvalidOperationException(error);

            string originalName = Path.GetFileName(input.FileName);
            string extension = Path.GetExtension(originalName).ToLowerInvariant();
            string storedName = Guid.NewGuid().ToString("N") + extension;
            string physicalPath = Path.Combine(physicalDirectory, storedName);
            string relativePath = relativeDirectory + "/" + storedName;

            batch.PhysicalPaths.Add(physicalPath);
            long copied = 0;
            byte[] buffer = new byte[81920];
            using (FileStream output = new FileStream(
                physicalPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                int read;
                while ((read = input.Content.Read(buffer, 0, buffer.Length)) > 0)
                {
                    copied += read;
                    if (copied > FileValidator.MaximumFileSize)
                        throw new InvalidOperationException("Each file must be no larger than 10 MB.");
                    output.Write(buffer, 0, read);
                }
            }

            if (copied != input.FileSize)
            {
                File.Delete(physicalPath);
                throw new InvalidOperationException("The uploaded file could not be read completely.");
            }

            batch.Files.Add(new SubmissionFile(
                0,
                0,
                originalName,
                storedName,
                relativePath,
                input.ContentType,
                copied,
                DateTime.Now));
        }
    }
}
