namespace Lodestar.Stats.TimeSeries;

/// <summary>How the seasonal component combines with the trend.</summary>
public enum SeasonalModel
{
    /// <summary><c>series = trend + seasonal + residual</c>, the default.</summary>
    Additive,

    /// <summary><c>series = trend · seasonal · residual</c>, for a season that grows with the level.</summary>
    Multiplicative,
}
