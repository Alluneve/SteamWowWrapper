using System.Diagnostics;

namespace SteamWowWrapper;

internal static class Watchdog
{
    public static void Start(int parentPid)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = Environment.ProcessPath!,
            Arguments = $"--watchdog {parentPid}",
            CreateNoWindow = true
        });
    }

    public static async Task RunAsync(int parentPid)
    {
        try
        {
            using var parent = Process.GetProcessById(parentPid);
            await parent.WaitForExitAsync();
        }
        catch (ArgumentException)
        {
        }

        await WoW.CloseAsync();
        await BattleNet.StopAsync();
    }
}