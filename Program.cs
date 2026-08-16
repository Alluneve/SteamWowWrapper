using SteamWowWrapper;

if (args is ["--watchdog", var pid])
{
    await Watchdog.RunAsync(int.Parse(pid));
    return;
}

if (args is not ["--bnet", var battleNetPath])
    throw new ArgumentException("Expected --bnet <Battle.net.exe path>");

await SteamWowRunner.RunAsync(battleNetPath);