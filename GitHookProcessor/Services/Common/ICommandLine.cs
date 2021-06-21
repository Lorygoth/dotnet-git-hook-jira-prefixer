namespace GitHookProcessor.Services.Common
{
    public interface ICommandLine
    {
        (bool Success, int ExitCode, string? Output) Execute(string command);
    }
}