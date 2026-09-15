namespace Lodestar.Stats.TimeSeries;

/// <summary>How the augmented Dickey-Fuller test chooses its lag order.</summary>
public enum LagSelection
{
    /// <summary>The smallest Akaike criterion — the reference's <c>"AIC"</c>, and the default.</summary>
    Akaike,

    /// <summary>The smallest Schwarz criterion — <c>"BIC"</c>.</summary>
    Schwarz,

    /// <summary>Down from the maximum, the first lag whose own t statistic reaches 1.645 — <c>"t-stat"</c>.</summary>
    TStatistic,

    /// <summary>No search: the maximum lag is the lag — <c>autolag=None</c>.</summary>
    Fixed,
}
