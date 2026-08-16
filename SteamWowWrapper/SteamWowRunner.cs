using System.Diagnostics;

namespace SteamWowWrapper;

internal static class SteamWowRunner
{
    public static async Task RunAsync(string battleNetPath)
    {
        using var self = Process.GetCurrentProcess();

        Watchdog.Start(self.Id);

        if (!await BattleNet.EnsureReadyAsync(battleNetPath))
            return;

        using var wow = await WoW.LaunchAsync(battleNetPath);
        if (wow is null)
            return;
        await wow.WaitForExitAsync();

        await BattleNet.StopAsync();
    }
}