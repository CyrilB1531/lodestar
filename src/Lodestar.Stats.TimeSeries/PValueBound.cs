namespace Lodestar.Stats.TimeSeries;

/// <summary>Whether a tabulated p-value was clamped at the end of its table, and which way the truth lies.</summary>
public enum PValueBound
{
    /// <summary>The statistic fell inside the table, and the p-value is interpolated.</summary>
    None,

    /// <summary>The statistic is at or past the table's smallest p-value, which came back; the true one is smaller.</summary>
    ActualIsSmaller,

    /// <summary>The statistic is at or before the table's largest p-value, which came back; the true one is greater.</summary>
    ActualIsGreater,
}
