using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using GitHookProcessor.Services.Common;
using GitHookProcessor.Services.Hooks;
using Xunit;

namespace GitHookProcessor.Tests.Services.Hooks
{
    public class CommitPrefixerTests
    {
        [Theory]
        [InlineData("ticket-12345 commit message", "ticket-12345", "ticket-12345 commit message", null)]
        [InlineData("commit message", "ticket-12345", "ticket-12345 commit message", "Jira ticket name prefix not found")]
        [InlineData("ticket-12345 commit message", "TICKET-12345", "TICKET-12345 commit message", "Fixed jira ticket name case: ticket-12345 => TICKET-12345")]
        [InlineData("tket-123 commit message", "TICKET-12345", "TICKET-12345 commit message", "Fixed jira ticket name typo: tket-123 => TICKET-12345")]
        public void Test_GetPrefixedMessage_ReturnsExpectedValue(string message, string prefix, string expectedResult, string? expectedWarning)
        {
            using var fake = new AutoFake();
            // arrange
            var prefixer = fake.Resolve<CommitMessagePrefixer>();

            // act
            var result = prefixer.GetPrefixedMessage(message, prefix);

            // assert
            if (expectedWarning != null)
                A.CallTo(() => fake.Resolve<ILogger<CommitMessagePrefixer>>().Warn(expectedWarning)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("feature/ticket-12345", "ticket-12345")]
        [InlineData("feature/TICKET-12345-anything", "TICKET-12345")]
        [InlineData("bugfix/TICKET-12345", "TICKET-12345")]
        [InlineData("bugfix/ticket-12345-anything", "ticket-12345")]
        public void Test_GetCurrentBranchName_WhenCorrectBranchName_ReturnsExpectedValue(string branchName, string expectedResult)
        {
            using var fake = new AutoFake();
            // arrange
            var prefixer = fake.Resolve<CommitMessagePrefixer>();

            // act
            var jiraTicketName = prefixer.GetJiraTicketName(branchName);

            // assert
            Assert.Equal(expectedResult, jiraTicketName);
        }
    }
}