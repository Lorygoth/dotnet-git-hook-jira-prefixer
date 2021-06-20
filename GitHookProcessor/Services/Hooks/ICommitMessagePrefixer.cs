namespace GitHookProcessor.Services.Hooks
{
    public interface ICommitMessagePrefixer
    {
        string GetPrefixedMessage(string message, string prefix);
        string GetJiraTicketName(string branchName);
    }
}