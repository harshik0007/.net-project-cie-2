using System;
using System.Collections.Generic;
using System.IO;

namespace CIE_2_PROJECT.Validators
{
    public sealed class FileValidator
    {
        public const long MaximumFileSize = 10L * 1024L * 1024L;

        private static readonly Predicate<string> AllowedExtension =
            IsAllowedExtension;

        private static readonly HashSet<string> Extensions =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".zip", ".pdf", ".doc", ".docx", ".png", ".jpg", ".jpeg"
            };

        public bool IsValid(string fileName, long fileSize, out string error)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                error = "Choose a file to upload.";
                return false;
            }
            if (fileSize <= 0 || fileSize > MaximumFileSize)
            {
                error = "Each file must be no larger than 10 MB.";
                return false;
            }
            if (!AllowedExtension(Path.GetExtension(fileName)))
            {
                error = "Allowed file types are ZIP, PDF, DOC, DOCX, PNG, JPG, and JPEG.";
                return false;
            }

            error = null;
            return true;
        }

        private static bool IsAllowedExtension(string extension)
        {
            return !string.IsNullOrWhiteSpace(extension) && Extensions.Contains(extension);
        }
    }
}
