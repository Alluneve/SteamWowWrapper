using System.Diagnostics;

namespace SteamWowWrapper;

internal static class WoW
{
    public static async Task<Process?> LaunchAsync(string battleNetPath)
    {
        var existing = Find();

        if (existing is not null)
            return existing;

        while (BattleNet.IsRunning())
        {
            BattleNet.LaunchWoW(battleNetPath);

            var wow = await ProcessHelper.WaitForProcessAsync(
                "Wow",
                TimeSpan.FromSeconds(10));

            if (wow is not null)
                return wow;
        }

        return null;
    }

    private static Process? Find() =>
        Process.GetProcessesByName("Wow").FirstOrDefault();

    public static async Task CloseAsync()
    {
        using var wow = Find();

        if (wow is null || wow.HasExited)
            return;

        Console.WriteLine("Requesting graceful WoW shutdown...");

        wow.CloseMainWindow();

        if (await ProcessHelper.WaitForExitAsync(
                wow,
                TimeSpan.FromSeconds(30)))
        {
            Console.WriteLine("WoW exited gracefully.");
            return;
        }

        Console.WriteLine("WoW did not exit after 30 seconds. Forcing shutdown.");

        wow.Kill(entireProcessTree: true);
        await wow.WaitForExitAsync();
    }
}