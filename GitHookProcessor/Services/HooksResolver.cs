using GitHookProcessor.Common.Tools;
using GitHookProcessor.Services.Common;
using GitHookProcessor.Services.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GitHookProcessor.Services
{
    public class HooksResolver : IHooksResolver
    {
        private readonly ILogger<HooksResolver> logger;
        private readonly IPrepareCommitMsgHookProcessor prepareCommitMsgHook;

        public HooksResolver(
            ILogger<HooksResolver> logger,
            IPrepareCommitMsgHookProcessor prepareCommitMsgHook
        )
        {
            this.logger = logger;
            this.prepareCommitMsgHook = prepareCommitMsgHook;
        }

        public void ProcessArgs(IReadOnlyCollection<string> args)
        {
            if (!TryResolveHookType(args, out var type, out var otherArgs)) return;
            ProcessHook(type, otherArgs);
        }

        private bool TryResolveHookType(IReadOnlyCollection<string> args, out GitHookTypes type, out IReadOnlyCollection<string> otherArgs)
        {
            otherArgs = Array.Empty<string>();
            type = default(GitHookTypes);
            if (args.Count < 1)
            {
                logger.Error("Insufficient amount of hook event args");
                return false;
            }

            var typesDict = EnumTools.GetDescriptionsDict<GitHookTypes>();
            var hookName = args.First();
            if (!typesDict.TryGetValue(hookName, out type))
            {
                logger.Error($"Unsupported hook type: {hookName}");
                return false;
            }

            otherArgs = args.Skip(1).ToArray();
            return true;
        }

        private void ProcessHook(GitHookTypes commitType, IReadOnlyCollection<string> otherArgs)
        {
            switch (commitType)
            {
                case GitHookTypes.PrepareCommitMsg:
                    prepareCommitMsgHook.Process(otherArgs);
                    break;
                default:
                    logger.Error($"Unsupported hook type: {commitType}");
                    break;
            };
        }
    }
}