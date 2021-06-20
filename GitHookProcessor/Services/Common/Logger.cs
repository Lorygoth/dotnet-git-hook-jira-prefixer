using System;

namespace GitHookProcessor.Services.Common
{
    public class Logger<T> : ILogger<T>
    {
        public void Info(string message)
        {
            using var _ = new ConsoleColor(System.ConsoleColor.Green);
            Console.WriteLine(FormatMessage("Info", message));
        }
        public void Warn(string message)
        {
            using var _ = new ConsoleColor(System.ConsoleColor.Yellow);
            Console.WriteLine(FormatMessage("Warn", message));
        }

        public void Error(string message)
        {
            using var _ = new ConsoleColor(System.ConsoleColor.Red);
            Console.WriteLine(FormatMessage("ERROR", message));
        }

        public void Error(string message, Exception ex)
        {
            using var _ = new ConsoleColor(System.ConsoleColor.Red);
            Error(string.Join(Environment.NewLine, message, $"Exception: {ex}", $"StackTrace: {ex.StackTrace}"));
        }

        private string FormatMessage(string level, string message)
        {
            return $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {typeof(T).Name} - {level} - {message}";
        }

        private class ConsoleColor : IDisposable
        {
            public System.ConsoleColor previousColor;
            public ConsoleColor(System.ConsoleColor color)
            {
                this.previousColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
            }

            public void Dispose()
            {
                Console.ForegroundColor = previousColor;
            }
        }
    }
}