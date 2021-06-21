using GitHookProcessor.Common.Enums;
using GitHookProcessor.Common.Tools;
using GitHookProcessor.Services.Common;
using System.Collections.Generic;
using System.Linq;

namespace GitHookProcessor.Services.Hooks
{
    public class PrepareCommitMsgHookProcessor : IPrepareCommitMsgHookProcessor
    {
        private readonly ILogger<PrepareCommitMsgHookProcessor> logger;
        private readonly IFileService fileService;
        private readonly ICommitMessagePrefixer commitMessagePrefixer;
        private readonly IGitHelper gitHelper;

        public PrepareCommitMsgHookProcessor(
            ILogger<PrepareCommitMsgHookProcessor> logger,
            IFileService fileService,
            ICommitMessagePrefixer commitMessagePrefixer,
            IGitHelper gitHelper
        )
        {
            this.logger = logger;
            this.fileService = fileService;
            this.commitMessagePrefixer = commitMessagePrefixer;
            this.gitHelper = gitHelper;
        }

        public void Process(IReadOnlyCollection<string> args)
        {
            var commitMessageFilePath = args.FirstOrDefault();
            if (commitMessageFilePath == null)
            {
                logger.Error("Message file path not provided");
                return;
            }

            var commitType = args.Skip(1).FirstOrDefault();
            if (commitType == null)
            {
                logger.Error("Type of prepare-commit-msg hook not provided");
                return;
            }

            var commitTypesDict = EnumTools.GetDescriptionsDict<GitCommitTypes>();
            if (!commitTypesDict.TryGetValue(commitType, out var type))
            {
                logger.Error($"Unsupported type of prepare-commit-msg hook: {commitType}");
                return;
            }

            switch (type)
            {
                case GitCommitTypes.Message:
                    ProcessMessage(commitMessageFilePath);
                    return;
                default:
                    logger.Error($"Unsupported type of prepare-commit-msg: {commitType}");
                    return;
            }
        }

        private void ProcessMessage(string commitMessageFilePath)
        {
            var commitMessage = fileService.Read(commitMessageFilePath);
            var branchName = gitHelper.GetCurrentBranchName();
            if (!commitMessagePrefixer.TryGetJiraTicketName(branchName, out var jiraTicketName)) return;
            var prefixedMessage = commitMessagePrefixer.GetPrefixedMessage(commitMessage, jiraTicketName);

            fileService.Write(commitMessageFilePath, prefixedMessage);
        }
    }
}