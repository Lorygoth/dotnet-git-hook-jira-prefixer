using System;

namespace GitHookProcessor.Services.Common
{
    public interface IGitHelper
    {
        string GetCurrentBranchName();
    }
}