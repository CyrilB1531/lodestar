namespace Lodestar.Survival;

/// <summary>Which member of the log-rank family <c>LogRank</c> runs, and up to when.</summary>
public sealed record LogRankOptions
{
    /// <summary>How each event time is weighed; <see cref="LogRankWeighting.LogRank"/> by default.</summary>
    public LogRankWeighting Weighting { get; init; } = LogRankWeighting.LogRank;

    /// <summary>The exponent of <c>S</c> under <see cref="LogRankWeighting.FlemingHarrington"/>; lifelines' <c>p</c>, non-negative.</summary>
    public double P { get; init; }

    /// <summary>The exponent of <c>1 − S</c> under <see cref="LogRankWeighting.FlemingHarrington"/>; lifelines' <c>q</c>, non-negative.</summary>
    public double Q { get; init; }

    /// <summary>A time past which every event counts as a censoring, or <see langword="null"/> for none; lifelines' <c>t_0</c>.</summary>
    public double? Truncation { get; init; }
}
