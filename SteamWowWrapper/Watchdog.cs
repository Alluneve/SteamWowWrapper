using System.Diagnostics;

namespace SteamWowWrapper;

internal static class Watchdog
{
    private static string EventName(int parentPid) =>
        $@"Local\SteamWowWrapper-{parentPid}-ShutdownArmed";

    public static EventWaitHandle CreateShutdownSignal(int parentPid) =>
        new(
            initialState: false,
            EventResetMode.ManualReset,
            EventName(parentPid));

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
        using var shutdownArmed =
            EventWaitHandle.OpenExisting(EventName(parentPid));

        try
        {
            using var parent = Process.GetProcessById(parentPid);
            await parent.WaitForExitAsync();
        }
        catch (ArgumentException)
        {
        }

        if (!shutdownArmed.WaitOne(0))
            return;

        await WoW.CloseAsync();
        await BattleNet.StopAsync();
    }
}