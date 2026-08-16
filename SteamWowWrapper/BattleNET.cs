using System.Diagnostics;

namespace SteamWowWrapper;

internal static class BattleNet
{
    public static async Task<bool> EnsureReadyAsync(string path)
    {
        var process = Find();

        if (process is null)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                Arguments = "--autostarted",
                UseShellExecute = true
            });

            process = await ProcessHelper.WaitForProcessAsync(
                "Battle.net",
                TimeSpan.FromSeconds(15));
        }

        if (process is null)
            return false;

        try
        {
            if (!process.WaitForInputIdle(TimeSpan.FromSeconds(30)))
                return false;
        }
        catch (InvalidOperationException)
        {
            return false;
        }

        await Task.Delay(1500);

        process.Refresh();

        return !process.HasExited;
    }

    public static void LaunchWoW(string path) =>
        Process.Start(path, "--exec=\"launch WoW\"");

    public static bool IsRunning() =>
        Find() is not null;

    private static Process? Find() =>
        Process.GetProcessesByName("Battle.net").FirstOrDefault();

    public static async Task StopAsync()
    {
        foreach (var process in Process.GetProcessesByName("Battle.net"))
        {
            if (process.HasExited)
                continue;

            process.CloseMainWindow();

            try
            {
                using var timeout =
                    new CancellationTokenSource(TimeSpan.FromSeconds(30));

                await process.WaitForExitAsync(timeout.Token);
            }
            catch (OperationCanceledException)
            {
                process.Kill(entireProcessTree: true);
            }
        }
    }
}