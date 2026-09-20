namespace GW2JadeProtocolTimer.Models;

public enum JadeProtocolTimerPhase
{
    Idle,
    Running,
    Ready,
}

public sealed class JadeProtocolTimerState
{
    public static readonly TimeSpan AddAmount = TimeSpan.FromMinutes(45);
    public static readonly TimeSpan MaxDuration = TimeSpan.FromHours(3);

    private TimeSpan _remaining;
    private DateTime _lastUpdatedUtc;

    public JadeProtocolTimerState(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public JadeProtocolTimerPhase Phase { get; private set; } = JadeProtocolTimerPhase.Idle;

    public bool IsIdle => Phase == JadeProtocolTimerPhase.Idle;

    public bool IsRunning => Phase == JadeProtocolTimerPhase.Running;

    public bool IsReady => Phase == JadeProtocolTimerPhase.Ready;

    public TimeSpan Remaining => _remaining;

    public void AddTime(DateTime nowUtc)
    {
        Tick(nowUtc);

        TimeSpan remaining = IsRunning ? _remaining : TimeSpan.Zero;
        _remaining = Min(remaining + AddAmount, MaxDuration);
        _lastUpdatedUtc = nowUtc;
        Phase = JadeProtocolTimerPhase.Running;
    }

    public void ResetToIdle()
    {
        _remaining = TimeSpan.Zero;
        Phase = JadeProtocolTimerPhase.Idle;
    }

    public bool Tick(DateTime nowUtc)
    {
        if (!IsRunning)
            return false;

        TimeSpan elapsed = nowUtc - _lastUpdatedUtc;
        if (elapsed <= TimeSpan.Zero)
            return false;

        _lastUpdatedUtc = nowUtc;
        _remaining -= elapsed;

        if (_remaining > TimeSpan.Zero)
            return false;

        _remaining = TimeSpan.Zero;
        Phase = JadeProtocolTimerPhase.Ready;
        return true;
    }

    public string DisplayText()
    {
        if (IsReady)
            return "READY";
        if (IsIdle)
            return "+45m";

        return FormatRemaining(_remaining);
    }

    private static TimeSpan Min(TimeSpan first, TimeSpan second) =>
        first <= second ? first : second;

    private static string FormatRemaining(TimeSpan remaining)
    {
        int totalSeconds = (int)Math.Ceiling(remaining.TotalSeconds);
        int hours = totalSeconds / 3600;
        int minutes = totalSeconds % 3600 / 60;
        int seconds = totalSeconds % 60;

        return hours > 0
            ? $"{hours}:{minutes:00}:{seconds:00}"
            : $"{minutes:00}:{seconds:00}";
    }
}
