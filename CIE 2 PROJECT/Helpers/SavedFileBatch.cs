using System;
using System.Collections.Generic;
using System.IO;
using CIE_2_PROJECT.Models;

namespace CIE_2_PROJECT.Helpers
{
    public sealed class SavedFileBatch
    {
        public IList<SubmissionFile> Files { get; private set; }
        public IList<string> PhysicalPaths { get; private set; }

        public SavedFileBatch()
        {
            Files = new List<SubmissionFile>();
            PhysicalPaths = new List<string>();
        }

        public void Cleanup()
        {
            foreach (string path in PhysicalPaths)
            {
                try
                {
                    if (File.Exists(path))
                        File.Delete(path);
                }
                catch (IOException)
                {
                    // Leave remaining uploaded files eligible for cleanup.
                }
                catch (UnauthorizedAccessException)
                {
                    // Preserve the original database or workflow error.
                }
            }
        }
    }
}
