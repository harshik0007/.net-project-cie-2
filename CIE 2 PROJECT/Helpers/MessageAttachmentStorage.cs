using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace CIE_2_PROJECT.Helpers
{
    public static class MessageAttachmentStorage
    {
        public const long MaximumFileSize = 5L * 1024L * 1024L;

        private static readonly HashSet<string> AllowedExtensions =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx",
                ".txt", ".csv", ".zip", ".png", ".jpg", ".jpeg"
            };

        public static string Save(HttpPostedFile postedFile)
        {
            if (postedFile == null || postedFile.ContentLength <= 0)
                throw new InvalidOperationException("Choose a non-empty file to attach.");
            if (postedFile.ContentLength > MaximumFileSize)
                throw new InvalidOperationException("Message attachments must be 5 MB or smaller.");
            if (HttpContext.Current == null)
                throw new InvalidOperationException("Files can only be attached during a web request.");

            string originalName = Path.GetFileName(postedFile.FileName ?? string.Empty);
            string extension = Path.GetExtension(originalName);
            if (!AllowedExtensions.Contains(extension))
                throw new InvalidOperationException("Use a PDF, Office document, text, CSV, ZIP, JPG, or PNG file.");

            string safeName = SanitizeFileName(Path.GetFileNameWithoutExtension(originalName));
            if (safeName.Length > 150) safeName = safeName.Substring(0, 150);
            string storedName = Guid.NewGuid().ToString("N") + "_" + safeName + extension.ToLowerInvariant();
            string relativePath = "MessageAttachments/" + storedName;
            string directory = HttpContext.Current.Server.MapPath("~/App_Data/MessageAttachments/");
            string physicalPath = Path.Combine(directory, storedName);
            Directory.CreateDirectory(directory);

            long copied = 0;
            try
            {
                byte[] buffer = new byte[81920];
                using (Stream input = postedFile.InputStream)
                using (FileStream output = new FileStream(physicalPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        copied += read;
                        if (copied > MaximumFileSize)
                            throw new InvalidOperationException("Message attachments must be 5 MB or smaller.");
                        output.Write(buffer, 0, read);
                    }
                }
                if (copied == 0 || copied != postedFile.ContentLength)
                    throw new InvalidOperationException("The attachment could not be read completely.");
                return relativePath;
            }
            catch
            {
                if (File.Exists(physicalPath))
                    File.Delete(physicalPath);
                throw;
            }
        }

        public static void Delete(string relativePath)
        {
            if (!IsSafeRelativePath(relativePath) || HttpContext.Current == null)
                return;
            string root = Path.GetFullPath(HttpContext.Current.Server.MapPath("~/App_Data/MessageAttachments/"));
            string name = relativePath.Substring("MessageAttachments/".Length).Replace('/', '\\');
            string physicalPath = Path.GetFullPath(Path.Combine(root, name));
            if (physicalPath.StartsWith(root, StringComparison.OrdinalIgnoreCase) && File.Exists(physicalPath))
                File.Delete(physicalPath);
        }

        public static bool IsSafeRelativePath(string path)
        {
            return !string.IsNullOrWhiteSpace(path) &&
                path.StartsWith("MessageAttachments/", StringComparison.OrdinalIgnoreCase) &&
                path.IndexOf("..", StringComparison.Ordinal) < 0 &&
                path.IndexOf('\\') < 0;
        }

        public static string GetOriginalFileName(string path)
        {
            if (!IsSafeRelativePath(path)) return string.Empty;
            string storedName = Path.GetFileName(path);
            return storedName.Length > 33 ? storedName.Substring(33) : storedName;
        }

        private static string SanitizeFileName(string value)
        {
            char[] invalid = Path.GetInvalidFileNameChars();
            char[] chars = (value ?? string.Empty).ToCharArray();
            for (int index = 0; index < chars.Length; index++)
            {
                char current = chars[index];
                if (Array.IndexOf(invalid, current) >= 0 ||
                    !(char.IsLetterOrDigit(current) || current == '-' || current == '_' || current == ' '))
                    chars[index] = '_';
            }
            string result = new string(chars).Trim(' ', '.', '_');
            return string.IsNullOrWhiteSpace(result) ? "attachment" : result.Replace(' ', '_');
        }
    }
}
