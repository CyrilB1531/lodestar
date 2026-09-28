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

    /// <summary>Hashes every member as the generated equality compares it, each <c>NaN</c> alike (#1285).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + ValueEquality.HashOfItem(Weighting);
            hash = (hash * 31) + ValueEquality.HashOf(P);
            hash = (hash * 31) + ValueEquality.HashOf(Q);
            return (hash * 31) + ValueEquality.HashOf(Truncation);
        }
    }
}
