using SteamWowWrapper;

if (args is ["--watchdog", var pid])
{
    await Watchdog.RunAsync(int.Parse(pid));
    return;
}

string? battleNetPath = args switch
{
    ["--bnet", var path] => path,

    [var arg] when arg.StartsWith(
            "--bnet=",
            StringComparison.OrdinalIgnoreCase) =>
        arg["--bnet=".Length..],

    _ => null
};

if (battleNetPath is null)
{
    throw new ArgumentException(
        "Expected --bnet <Battle.net.exe path>");
}

await SteamWowRunner.RunAsync(battleNetPath);