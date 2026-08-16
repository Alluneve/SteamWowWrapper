# SteamWowWrapper

A small Windows wrapper for launching **World of Warcraft through Steam** while still using Battle.net for authentication, launching and updates.

> **Warning**
>
> This is an unofficial personal utility provided **as-is and at your own risk**. Battle.net or WoW updates may break its behaviour. Back up important addon settings and `SavedVariables`.

## Features

* Starts Battle.net if needed.
* Waits for Battle.net before launching WoW.
* Launches Retail WoW through Battle.net.
* Keeps Steam tracking the WoW session.
* Detects normal WoW exits.
* Attempts a graceful WoW shutdown so `SavedVariables` can be written.
* Closes Battle.net when the session ends.

## Usage

Add `SteamWowWrapper.exe` as a **Non-Steam Game** and use:

```text
--bnet "C:\Program Files (x86)\Battle.net\Battle.net.exe"
```

## Build

```powershell
dotnet build
```

## Notes

Battle.net remains responsible for authentication, updates and patching.

This project does not modify World of Warcraft or bypass Battle.net.

See [AI-DECLARATION.md](AI-DECLARATION.md) for the AI usage declaration.
