using System.Diagnostics;

namespace SteamWowWrapper;

internal static class ProcessHelper
{
    public static async Task<Process?> WaitForProcessAsync(
        string name,
        TimeSpan timeout)
    {
        var end = DateTime.UtcNow + timeout;

        while (DateTime.UtcNow < end)
        {
            var process = Process.GetProcessesByName(name).FirstOrDefault();

            if (process is not null)
                return process;

            await Task.Delay(250);
        }

        return null;
    }

    public static async Task<bool> WaitForExitAsync(
        Process process,
        TimeSpan timeout)
    {
        using var cancellation = new CancellationTokenSource(timeout);

        try
        {
            await process.WaitForExitAsync(cancellation.Token);
            return true;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }
}