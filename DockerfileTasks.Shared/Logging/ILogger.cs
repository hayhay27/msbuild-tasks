using System;

namespace DockerfileTasks.Logging
{
    public enum LogImportance { Low, Normal, High }
    
    public interface ILogger
    {
        public void Log(LogImportance importance, string message, params object[] messageArgs);
        public void LogError(string message, params object[] messageArgs);
        public void LogError(Exception exception);
        public void LogWarning(string message, params object[] messageArgs);
        public void LogWarning(Exception exception);
    }
}