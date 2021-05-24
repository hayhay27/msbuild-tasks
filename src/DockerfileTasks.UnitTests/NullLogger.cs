using System;
using DockerfileTasks.Logging;

namespace DockerfileTasks.UnitTests
{
    public class NullLogger : ILogger
    {
        public static readonly ILogger Instance = new NullLogger();
        
        private NullLogger() {}
        
        public void Log(LogImportance importance, string message, params object[] messageArgs)
        {
        }

        public void LogError(string message, params object[] messageArgs)
        {
        }

        public void LogError(Exception exception)
        {
        }

        public void LogWarning(string message, params object[] messageArgs)
        {
        }

        public void LogWarning(Exception exception)
        {
        }
    }
}