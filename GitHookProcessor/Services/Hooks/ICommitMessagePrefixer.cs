namespace GitHookProcessor.Services.Hooks
{
    public interface ICommitMessagePrefixer
    {
        string GetPrefixedMessage(string message, string prefix);
        bool TryGetJiraTicketName(string branchName, out string jiraTicketName);
    }
}