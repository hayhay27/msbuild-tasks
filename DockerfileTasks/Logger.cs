using System;
using DockerfileTasks.Logging;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace DockerfileTasks
{
    internal sealed class Logger : Logging.ILogger
    {
        private readonly TaskLoggingHelper _helper;

        public Logger(TaskLoggingHelper helper)
        {
            _helper = helper;
        }


        public void Log(LogImportance importance, string message, params object[] messageArgs)
        {
            switch (importance)
            {
                case LogImportance.Low:
                    _helper.LogMessage(MessageImportance.Low, message, messageArgs);
                    break;
                case LogImportance.Normal:
                    _helper.LogMessage(MessageImportance.Normal, message, messageArgs);
                    break;
                case LogImportance.High:
                    _helper.LogMessage(MessageImportance.High, message, messageArgs);
                    break;
            }
        }

        public void LogError(string message, params object[] messageArgs)
        {
            _helper.LogError(message, messageArgs);
        }
        
        public void LogError(Exception exception)
        {
            _helper.LogErrorFromException(exception);
        }
        
        public void LogWarning(string message, params object[] messageArgs)
        {
            _helper.LogWarning(message, messageArgs);
        }

        public void LogWarning(Exception exception)
        {
            _helper.LogWarningFromException(exception);
        }
    }
}