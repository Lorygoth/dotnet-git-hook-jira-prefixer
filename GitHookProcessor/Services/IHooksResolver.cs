using System.Collections.Generic;

namespace GitHookProcessor.Services
{
    public interface IHooksResolver
    {
        void ProcessArgs(IReadOnlyCollection<string> args);
    }
}