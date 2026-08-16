using System.Diagnostics;

namespace SteamWowWrapper;

public static class Logger
{
    private static readonly string LogPath =
        Path.Combine(AppContext.BaseDirectory, "SteamWowWrapper.log");

    private static readonly string OldLogPath =
        Path.Combine(AppContext.BaseDirectory, "SteamWowWrapper.old.log");

    public static void Initialize()
    {
        if (File.Exists(OldLogPath))
            File.Delete(OldLogPath);

        if (File.Exists(LogPath))
            File.Move(LogPath, OldLogPath);

#if DEBUG
        Info("Log started. Build: Debug");
#else
        Info("Log started. Build: Release");
#endif
    }

    [Conditional("DEBUG")]
    public static void Debug(string message) =>
        Write("DEBUG", message);

    public static void Info(string message) =>
        Write("INFO", message);

    public static void Error(string message) =>
        Write("ERROR", message);

    public static void Error(Exception exception) =>
        Write("ERROR", exception.ToString());

    public static void Error(string message, Exception exception) =>
        Write("ERROR", $"{message}{Environment.NewLine}{exception}");

    private static void Write(string level, string message)
    {
        File.AppendAllText(
            LogPath,
            $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} " +
            $"[{Environment.ProcessId}] " +
            $"[{level}] {message}{Environment.NewLine}");
    }
}