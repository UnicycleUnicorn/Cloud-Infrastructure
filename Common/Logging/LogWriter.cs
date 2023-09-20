using Common.SingleThreadedExecutors;
using System.Runtime.CompilerServices;

namespace Common.Logging;
public static class LogWriter
{
    public static LogLevel OutputLevel { get; set; } = LogLevel.DEBUG;
    public static bool ShouldConsoleLog { get; set; } = true;

    private static readonly string exePath = Directory.GetCurrentDirectory() ?? throw new Exception();
    private static readonly string logName = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + ".log";
    private static readonly string logPath = Path.Combine(exePath, "Logs", logName);

    private static readonly SingleThreadedActionExecutor<LogLevel, string, string, string, int> actionExecutor = new(Log);

    // Called via actionExecutor
    private static void Log(LogLevel logLevel, string message, string callerFilePath, string callerMemberName, int callerLineNumber)
    {
        if (logLevel >= OutputLevel)
        {
            string callerInfoString = $"{callerMemberName} in {callerFilePath.Replace(exePath + "\\", "")} at line {callerLineNumber}";
            File.AppendAllText(logPath, $"{DateTime.Now:dd/MM/yyyy HH:mm:ss} [{logLevel}] {{Called from {callerInfoString}}} << {message} >>{Environment.NewLine}");

            if (ShouldConsoleLog)
            {
                switch (logLevel)
                {
                    case LogLevel.DEBUG:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;

                    case LogLevel.INFO:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;

                    case LogLevel.WARN:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;

                    case LogLevel.ERROR:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;

                    case LogLevel.FATAL:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                }

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss} {logLevel} from {callerInfoString}] {message}");
                Console.ResetColor();
            }
        }
    }

    public static void LogInfo(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => actionExecutor.Add(LogLevel.INFO, message, callerFilePath, callerMemberName, callerLineNumber);
    public static void LogDebug(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => actionExecutor.Add(LogLevel.DEBUG, message, callerFilePath, callerMemberName, callerLineNumber);
    public static void LogWarn(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => actionExecutor.Add(LogLevel.WARN, message, callerFilePath, callerMemberName, callerLineNumber);
    public static void LogError(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => actionExecutor.Add(LogLevel.ERROR, message, callerFilePath, callerMemberName, callerLineNumber);
    public static void LogFatal(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => actionExecutor.Add(LogLevel.FATAL, message, callerFilePath, callerMemberName, callerLineNumber);
}
