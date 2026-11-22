using System;
using System.IO;

namespace CIE_2_PROJECT.Models
{
    public sealed class UploadedFileInput
    {
        public string FileName { get; private set; }
        public string ContentType { get; private set; }
        public long FileSize { get; private set; }
        public Stream Content { get; private set; }

        public UploadedFileInput(string fileName, string contentType, long fileSize, Stream content)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("A file name is required.", "fileName");
            if (fileSize <= 0)
                throw new ArgumentOutOfRangeException("fileSize");
            if (content == null)
                throw new ArgumentNullException("content");

            FileName = fileName;
            ContentType = contentType;
            FileSize = fileSize;
            Content = content;
        }
    }
}
