using System.Diagnostics;

namespace SteamWowWrapper;

internal static class SteamWowRunner
{
    public static async Task RunAsync(string battleNetPath)
    {
        using var self = Process.GetCurrentProcess();
        using var shutdownArmed =
            Watchdog.CreateShutdownSignal(self.Id);

        Watchdog.Start(self.Id);

        if (!await BattleNet.EnsureReadyAsync(battleNetPath))
            return;

        using var wow = await WoW.LaunchAsync(battleNetPath);

        if (wow is null)
            return;

        // From this point, Steam Stop should close WoW + Battle.net.
        shutdownArmed.Set();

        await wow.WaitForExitAsync();
        await BattleNet.StopAsync();

        // Normal WoW exit: watchdog no longer needs to clean anything up.
        shutdownArmed.Reset();
    }
}