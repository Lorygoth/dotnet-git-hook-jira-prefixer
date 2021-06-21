using GitHookProcessor.Services.Common;
using System;
using System.Text.RegularExpressions;

namespace GitHookProcessor.Services.Hooks
{
    public class CommitMessagePrefixer : ICommitMessagePrefixer
    {
        private readonly ILogger<CommitMessagePrefixer> logger;

        private const string JiraTicketNameRegex = "\\w+-\\d+";

        public CommitMessagePrefixer(ILogger<CommitMessagePrefixer> logger)
        {
            this.logger = logger;
        }

        public string GetPrefixedMessage(string message, string prefix)
        {
            if (
                TryFixInvalidCase(prefix, ref message) ||
                TryFixTypos(prefix, ref message) ||
                TrySkipPrefixing(prefix, message))
            {
                return message;
            }

            return FormatCommit(prefix, message);
        }

        private bool TryFixInvalidCase(string prefix, ref string message)
        {
            var loweredMessage = message.ToLower();
            if (!loweredMessage.StartsWith(prefix.ToLower())) return false;

            var originalPrefix = message.Substring(0, prefix.Length);
            if (originalPrefix == prefix) return false;

            message = FormatCommit(prefix, message.Substring(prefix.Length).Trim());
            logger.Warn($"Fixed jira ticket name case: {originalPrefix} => {prefix}");
            return true;
        }

        private bool TryFixTypos(string prefix, ref string message)
        {
            var groupMatch = Regex.Match(message, $"^{JiraTicketNameRegex}").Groups[0];
            if (!groupMatch.Success) return false;

            var originalPrefix = groupMatch.Value;
            if (originalPrefix == prefix) return false;

            message = FormatCommit(prefix, message.Substring(originalPrefix.Length).Trim());
            logger.Warn($"Fixed jira ticket name typo: {originalPrefix} => {prefix}");
            return true;
        }

        private bool TrySkipPrefixing(string prefix, string message)
        {
            var groupMatch = Regex.Match(message, $"^{JiraTicketNameRegex}").Groups[0];
            if (!groupMatch.Success)
            {
                logger.Warn($"Jira ticket name prefix not found");
                return false;
            }

            var originalPrefix = groupMatch.Value;
            if (originalPrefix == prefix) return true;

            return false;
        }

        private string FormatCommit(string prefix, string message)
        {
            return $"{prefix} {message}";
        }

        public bool TryGetJiraTicketName(string branchName, out string jiraTicketName)
        {
            const string regexGroup = "ticketname";

            var pattern = $"(?!feature|bugfix)\\/(?<{regexGroup}>{JiraTicketNameRegex}).*";
            var ticketNameMatch = Regex.Match(branchName, pattern);
            var match = ticketNameMatch.Groups[regexGroup];

            if (!match.Success || string.IsNullOrEmpty(match.Value))
            {
                logger.Warn($"Branch name doesn't match pattern: {pattern}");
                jiraTicketName = string.Empty;
                return false;
            }

            jiraTicketName = match.Value;
            return true;
        }
    }
}