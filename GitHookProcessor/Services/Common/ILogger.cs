using System;

namespace GitHookProcessor.Services.Common
{
    public interface ILogger<T>
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Error(string message, Exception ex);
    }
}