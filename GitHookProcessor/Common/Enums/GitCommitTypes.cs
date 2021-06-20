using System.ComponentModel;

namespace GitHookProcessor.Common.Enums
{
    public enum GitCommitTypes
    {
        [Description("message")]
        Message,
        [Description("template")]
        Template,
        [Description("merge")]
        Merge,
        [Description("squash")]
        Squash,
        [Description("commit")]
        Commit
    }
}