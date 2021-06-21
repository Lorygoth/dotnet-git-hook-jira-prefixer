using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using GitHookProcessor.Common.Enums;
using GitHookProcessor.Common.Extensions;
using GitHookProcessor.Services.Common;
using GitHookProcessor.Services.Hooks;
using System.Collections.Generic;
using Xunit;

namespace GitHookProcessor.Tests.Services.Hooks
{
    public class PrepareCommitMsgHookTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void Test_Process_LogsExpectedError(IReadOnlyCollection<string> args, string expectedErrorLog)
        {
            using var fake = new AutoFake();
            // arrange
            var hook = fake.Resolve<PrepareCommitMsgHookProcessor>();

            // act
            hook.Process(args);

            // assert
            A.CallTo(() => fake.Resolve<ILogger<PrepareCommitMsgHookProcessor>>().Error(expectedErrorLog)).MustHaveHappenedOnceExactly();
        }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { new string[] {}, "Message file path not provided" },
            new object[] { new string[] {"filepath"}, "Type of prepare-commit-msg hook not provided" },
            new object[] { new string[] {"filepath", "qwerty"}, "Unsupported type of prepare-commit-msg hook: qwerty" },
        };

        [Fact]
        public void Test_Process_WhenTryGetJiraTicketNameIsTrue_ExecutesCompletelyWithoutError()
        {
            using var fake = new AutoFake();
            // arrange
            var hook = fake.Resolve<PrepareCommitMsgHookProcessor>();

            var filepath = "filepath";
            var gitCommitType = GitCommitTypes.Message.GetDescription();
            var args = new string[] { filepath, gitCommitType };

            var commitMessage = "commit message";
            A.CallTo(() => fake.Resolve<IFileService>().Read(filepath)).Returns(commitMessage);
            var branchName = "branchName";
            A.CallTo(() => fake.Resolve<IGitHelper>().GetCurrentBranchName()).Returns(branchName);
            var jiraTicketName = "jira ticket name";
            string ticketName;
            A.CallTo(() => fake.Resolve<ICommitMessagePrefixer>().TryGetJiraTicketName(branchName, out ticketName))
                .Returns(true)
                .AssignsOutAndRefParameters(jiraTicketName);
            var prefixedMessage = "prefixed message";
            A.CallTo(() => fake.Resolve<ICommitMessagePrefixer>().GetPrefixedMessage(commitMessage, jiraTicketName)).Returns(prefixedMessage);

            // act
            hook.Process(args);

            // assert
            A.CallTo(() => fake.Resolve<IFileService>().Read(filepath)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fake.Resolve<IGitHelper>().GetCurrentBranchName()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fake.Resolve<ICommitMessagePrefixer>().TryGetJiraTicketName(branchName, out ticketName)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fake.Resolve<ICommitMessagePrefixer>().GetPrefixedMessage(commitMessage, jiraTicketName)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fake.Resolve<IFileService>().Write(filepath, prefixedMessage)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Test_Process_WhenTryGetJiraTicketNameIsFalse_StopsExecutingWithoutError()
        {
            using var fake = new AutoFake();
            // arrange
            var hook = fake.Resolve<PrepareCommitMsgHookProcessor>();

            var filepath = "filepath";
            var gitCommitType = GitCommitTypes.Message.GetDescription();
            var args = new string[] { filepath, gitCommitType };

            var commitMessage = "commit message";
            A.CallTo(() => fake.Resolve<IFileService>().Read(filepath)).Returns(commitMessage);
            var branchName = "branchName";
            A.CallTo(() => fake.Resolve<IGitHelper>().GetCurrentBranchName()).Returns(branchName);
            var jiraTicketName = "jira ticket name";
            string ticketName;
            A.CallTo(() => fake.Resolve<ICommitMessagePrefixer>().TryGetJiraTicketName(branchName, out ticketName))
                .Returns(false)
                .AssignsOutAndRefParameters(jiraTicketName);
            var prefixedMessage = "prefixed message";
            A.CallTo(() => fake.Resolve<ICommitMessagePrefixer>().GetPrefixedMessage(commitMessage, jiraTicketName)).Returns(prefixedMessage);

            // act
            hook.Process(args);

            // assert
            A.CallTo(() => fake.Resolve<IFileService>().Read(filepath)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fake.Resolve<IGitHelper>().GetCurrentBranchName()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fake.Resolve<ICommitMessagePrefixer>().TryGetJiraTicketName(branchName, out ticketName)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fake.Resolve<ICommitMessagePrefixer>().GetPrefixedMessage(commitMessage, jiraTicketName)).MustNotHaveHappened();
            A.CallTo(() => fake.Resolve<IFileService>().Write(filepath, prefixedMessage)).MustNotHaveHappened();
        }
    }
}