# GW2 Jade Protocol Timer

A single-instance, always-on-top floating WPF panel for manually tracking Guild Wars 2
Jade Protocol buffs. It shows two independent timers, Offensive and Defensive. The panel
stays above other windows, positions itself at the lower-right of the screen, and can be
dragged.

This app does not read game memory, send input, call APIs, or automate anything. It is
only a manual always-on-top timer.

## Controls

- Left click `Offensive` or `Defensive` to add `+45m` to that timer.
- Each timer caps at `3:00:00`.
- Right click a row to reset it back to idle.
- `READY` means that timer has expired.
- `X` minimizes the window.
- `Quit` closes the app.

## States

- Idle rows show `+45m`.
- Running rows show the countdown.
- Expired rows show `READY` with stronger border and text contrast.

## Requirements

- Windows
- .NET 10.0 (build target: `net10.0-windows`)

## Build / Run

```
dotnet build .\GW2JadeProtocolTimer.slnx
```

## Disclaimer

This is an unofficial fan-made utility for Guild Wars 2. It is not affiliated with,
endorsed by, or associated with ArenaNet, LLC or NCSOFT.

## License

Licensed under the MIT License. See [LICENSE](LICENSE).
