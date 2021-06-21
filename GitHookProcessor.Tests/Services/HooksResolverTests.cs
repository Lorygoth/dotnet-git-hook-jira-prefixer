using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using GitHookProcessor.Common.Extensions;
using GitHookProcessor.Services;
using GitHookProcessor.Services.Common;
using GitHookProcessor.Services.Hooks;
using System.Collections.Generic;
using Xunit;

namespace GitHookProcessor.Tests.Services
{
    public class HooksResolverTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void Test_ProcessArgs_LogsExpectedError(IReadOnlyCollection<string> args, string expectedErrorLog)
        {
            using var fake = new AutoFake();
            // arrange
            var resolver = fake.Resolve<HooksResolver>();

            // act
            resolver.ProcessArgs(args);

            // assert
            A.CallTo(() => fake.Resolve<ILogger<HooksResolver>>().Error(expectedErrorLog)).MustHaveHappenedOnceExactly();
        }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { new string[] {}, "Insufficient amount of hook event args" },
            new object[] { new string[] { "unsupported-hook" }, "Unsupported hook type: unsupported-hook" },
        };

        [Fact]
        public void Test_ProcessArgs_ExecutesWithoutError()
        {
            using var fake = new AutoFake();
            // arrange
            var hook = fake.Resolve<HooksResolver>();

            var gitCommitType = GitHookTypes.PrepareCommitMsg.GetDescription();
            var args = new string[] { gitCommitType };

            // act
            hook.ProcessArgs(args);

            // assert
            A.CallTo(() => fake.Resolve<IPrepareCommitMsgHookProcessor>().Process(A<IReadOnlyCollection<string>>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
    }
}