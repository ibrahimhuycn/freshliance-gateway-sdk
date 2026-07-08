namespace Freshliance.Dashboard.Services;

public sealed class PollingOptions
{
    public int IntervalSeconds { get; set; } = 30;
    public bool Enabled { get; set; } = true;
}
