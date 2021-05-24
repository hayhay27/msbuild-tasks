namespace DockerfileTasks.Logging
{
    internal static class LoggerExtensions
    {
        public static void LogLow(this ILogger logger, string message, params object[] messageArgs)
        {
            logger.Log(LogImportance.Low, message, messageArgs);
        }
        
        public static void LogNormal(this ILogger logger, string message, params object[] messageArgs)
        {
            logger.Log(LogImportance.Normal, message, messageArgs);
        }
        
        public static void LogHigh(this ILogger logger, string message, params object[] messageArgs)
        {
            logger.Log(LogImportance.High, message, messageArgs);
        }
    }
}