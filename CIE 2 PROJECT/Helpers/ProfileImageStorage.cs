using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace CIE_2_PROJECT.Helpers
{
    public static class ProfileImageStorage
    {
        private const int MaximumFileSizeBytes = 3 * 1024 * 1024;
        private const int MaximumImageDimension = 4000;

        public static string Save(HttpPostedFile upload, string applicationRoot)
        {
            if (upload == null || upload.ContentLength <= 0)
                return null;
            if (upload.ContentLength > MaximumFileSizeBytes)
                throw new ArgumentException("Choose a profile photo smaller than 3 MB.");

            string extension = Path.GetExtension(upload.FileName);
            bool isPng = string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase);
            bool isJpeg = string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(extension, ".jpeg", StringComparison.OrdinalIgnoreCase);
            if (!isPng && !isJpeg)
                throw new ArgumentException("Choose a JPG or PNG image for your profile photo.");

            string relativeDirectory = "~/Uploads/ProfileImages/";
            string physicalDirectory = Path.Combine(applicationRoot, "Uploads", "ProfileImages");
            Directory.CreateDirectory(physicalDirectory);

            string filename = Guid.NewGuid().ToString("N") + (isPng ? ".png" : ".jpg");
            string physicalPath = Path.Combine(physicalDirectory, filename);
            try
            {
                using (Image image = Image.FromStream(upload.InputStream, true, true))
                {
                    if (image.Width <= 0 || image.Height <= 0 ||
                        image.Width > MaximumImageDimension || image.Height > MaximumImageDimension)
                        throw new ArgumentException("Profile photos must be no larger than 4000 by 4000 pixels.");

                    image.Save(physicalPath, isPng ? ImageFormat.Png : ImageFormat.Jpeg);
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("The selected file could not be read as a JPG or PNG image.", ex);
            }

            return relativeDirectory + filename;
        }

        public static void Delete(string profileImagePath, string applicationRoot)
        {
            string relativePath = (profileImagePath ?? string.Empty).Replace('\\', '/');
            const string allowedPrefix = "~/Uploads/ProfileImages/";
            if (!relativePath.StartsWith(allowedPrefix, StringComparison.OrdinalIgnoreCase) ||
                relativePath.IndexOf("..", StringComparison.Ordinal) >= 0 ||
                relativePath.IndexOf("://", StringComparison.Ordinal) >= 0)
                return;

            string filename = relativePath.Substring(allowedPrefix.Length);
            if (filename.IndexOf('/') >= 0 || filename.IndexOf('\\') >= 0)
                return;

            string physicalPath = Path.Combine(applicationRoot, "Uploads", "ProfileImages", filename);
            try
            {
                if (File.Exists(physicalPath))
                    File.Delete(physicalPath);
            }
            catch (IOException)
            {
                // Cleanup is best-effort; a failed profile save should not throw a second error.
            }
            catch (UnauthorizedAccessException)
            {
                // Cleanup is best-effort; a failed profile save should not throw a second error.
            }
        }
    }
}
