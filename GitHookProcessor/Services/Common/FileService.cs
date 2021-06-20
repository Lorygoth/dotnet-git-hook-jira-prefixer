using System;
using System.IO;

namespace GitHookProcessor.Services.Common
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> logger;
        public FileService(ILogger<FileService> logger)
        {
            this.logger = logger;
        }

        public string Read(string path)
        {
            try
            {
                return System.IO.File.ReadAllText(path);
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Failed to get commit message by path: {Path.GetFullPath(path)}", ex);
            }
        }

        public bool Write(string path, string text)
        {
            try
            {
                System.IO.File.WriteAllText(path, text);
                return true;
            }
            catch (System.Exception ex)
            {
                logger.Error($"Error while write file text. Path: {Path.GetFullPath(path)}. Text: {text}", ex);
                return false;
            }
        }
    }
}