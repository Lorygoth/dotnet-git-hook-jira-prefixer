using System;
using GitHookProcessor.Services.Hooks;

namespace GitHookProcessor.Services.Common
{
    public class GitHelper : IGitHelper
    {
        private readonly ILogger<CommitMessagePrefixer> logger;
        private readonly ICommandLine commandLine;
        public GitHelper(
            ILogger<CommitMessagePrefixer> logger,
            ICommandLine commandLine)
        {
            this.logger = logger;
            this.commandLine = commandLine;
        }


        public string GetCurrentBranchName()
        {
            const string getBranchNameCommand = "git symbolic-ref --short HEAD";
            var (success, _, branchName) = commandLine.Execute(getBranchNameCommand);
            if (!success || branchName == null) throw new Exception("Failed to get branch name");

            return branchName;
        }
    }
}