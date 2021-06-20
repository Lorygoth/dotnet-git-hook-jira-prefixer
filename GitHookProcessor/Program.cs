using GitHookProcessor.Services;

namespace GitHookProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            Startup.Init()
                .Resolve<IHooksResolver>()
                .ProcessArgs(args);
        }
    }
}
