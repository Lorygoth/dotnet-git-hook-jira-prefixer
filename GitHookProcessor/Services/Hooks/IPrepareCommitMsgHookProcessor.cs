using System.Collections.Generic;
using GitHookProcessor.Services.Common;

namespace GitHookProcessor.Services.Hooks
{
    public interface IPrepareCommitMsgHookProcessor
    {
        void Process(IReadOnlyCollection<string> args);
    }
}