using System;
using System.Diagnostics;
using System.Linq;

namespace GitHookProcessor.Services.Common
{
    public class CommandLine : ICommandLine
    {
        private readonly ILogger<CommandLine> logger;

        public CommandLine(ILogger<CommandLine> logger)
        {
            this.logger = logger;
        }

        private static bool IsNotWindows = (new int[] { 4, 6, 18 }).Contains((int)Environment.OSVersion.Platform);

        public (bool Success, int ExitCode, string? Output) Execute(string command)
        {
            var process = GetProcess(command);
            if (!process.Start()) return FormatFailedOutput("Failed to start process");
            process.WaitForExit();

            if (process.ExitCode != 0) return FormatFailedOutput(
                $"Process exited with error: {process.StandardOutput.ReadToEnd()}",
                process.ExitCode.ToString());


            return (true, process.ExitCode, process.StandardOutput.ReadToEnd());

            (bool Success, int ExitCode, string Output) FormatFailedOutput(string error, string output = "")
            {
                logger.Error($"Failed to execute command: {command}. {error}");
                return (false, process.ExitCode, output);
            }
        }

        private Process GetProcess(string command)
        {
            command = command.Replace("\"", "\"\"");
            var processStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            return new Process
            {
                StartInfo = IsNotWindows
                    ? SetupUnixProcess(processStartInfo, command)
                    : SetupWindowsProcess(processStartInfo, command)
            };
        }

        private ProcessStartInfo SetupUnixProcess(ProcessStartInfo processStartInfo, string command)
        {
            processStartInfo.FileName = "/bin/bash";
            processStartInfo.Arguments = $"-c \"{command}\"";
            return processStartInfo;
        }

        private ProcessStartInfo SetupWindowsProcess(ProcessStartInfo processStartInfo, string command)
        {
            processStartInfo.FileName = "CMD.exe";
            processStartInfo.Arguments = $"/c {command}";
            return processStartInfo;
        }
    }
}