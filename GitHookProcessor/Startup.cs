using Autofac;
using GitHookProcessor.Services;
using GitHookProcessor.Services.Common;
using GitHookProcessor.Services.Hooks;

namespace GitHookProcessor
{
    public class Startup
    {
        private IContainer container;
        public Startup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));
            builder.RegisterType<CommandLine>().As<ICommandLine>();
            builder.RegisterType<FileService>().As<IFileService>();
            builder.RegisterType<GitHelper>().As<IGitHelper>();
            builder.RegisterType<CommitMessagePrefixer>().As<ICommitMessagePrefixer>();
            builder.RegisterType<PrepareCommitMsgHookProcessor>().As<IPrepareCommitMsgHookProcessor>();
            builder.RegisterType<HooksResolver>().As<IHooksResolver>();

            this.container = builder.Build();
        }

        public T Resolve<T>() where T : notnull
        {
            return container.Resolve<T>();
        }
    }
}
