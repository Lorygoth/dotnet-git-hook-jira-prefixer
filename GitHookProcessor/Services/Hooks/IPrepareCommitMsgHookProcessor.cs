using System.Collections.Generic;

namespace GitHookProcessor.Services.Hooks
{
    public interface IPrepareCommitMsgHookProcessor
    {
        void Process(IReadOnlyCollection<string> args);
    }
}