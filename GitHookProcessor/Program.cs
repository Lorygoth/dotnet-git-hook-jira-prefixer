using GitHookProcessor.Services;

namespace GitHookProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            new Startup()
                .Resolve<IHooksResolver>()
                .ProcessArgs(args);
        }
    }
}
