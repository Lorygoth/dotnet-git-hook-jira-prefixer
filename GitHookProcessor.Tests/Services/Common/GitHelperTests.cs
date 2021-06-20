using System;
using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using GitHookProcessor.Services.Common;
using Xunit;

namespace GitHookProcessor.Tests.Services.Common
{
    public class GitHelperTests
    {

        [Fact]
        public void Test_GetCurrentBranchName_WhenUnableToRead_ThrowsException()
        {
            using var fake = new AutoFake();
            // arrange
            A.CallTo(() => fake.Resolve<ICommandLine>().Execute(A<string>.Ignored)).Returns((false, 0, "anything"));
            var gitHelper = fake.Resolve<GitHelper>();

            // act
            // assert
            Assert.Throws<Exception>(gitHelper.GetCurrentBranchName);
        }

        [Fact]
        public void Test_GetCurrentBranchName_WhenEmptyCommitMessage_ThrowsException()
        {
            using var fake = new AutoFake();
            // arrange
            A.CallTo(() => fake.Resolve<ICommandLine>().Execute(A<string>.Ignored)).Returns((true, 0, null));
            var gitHelper = fake.Resolve<GitHelper>();

            // act
            // assert
            Assert.Throws<Exception>(gitHelper.GetCurrentBranchName);
        }

        [Fact]
        public void Test_GetCurrentBranchName_WhenCorrectCommitMessage_ReturnsExpectedValue()
        {
            var expectedBranchName = "branchname";
            using var fake = new AutoFake();
            // arrange
            A.CallTo(() => fake.Resolve<ICommandLine>().Execute(A<string>.Ignored)).Returns((true, 0, expectedBranchName));
            var gitHelper = fake.Resolve<GitHelper>();

            // act
            // assert
            var resultBranchName = gitHelper.GetCurrentBranchName();
            Assert.Equal(expectedBranchName, resultBranchName);
        }
    }
}