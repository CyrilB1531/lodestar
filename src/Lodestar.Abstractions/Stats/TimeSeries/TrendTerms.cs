namespace Lodestar.Stats.TimeSeries;

/// <summary>The deterministic terms a unit-root or stationarity regression carries.</summary>
public enum TrendTerms
{
    /// <summary>No constant and no trend — the reference's <c>"n"</c>. ADF only.</summary>
    None,

    /// <summary>A constant — <c>"c"</c>, the default of both tests.</summary>
    Constant,

    /// <summary>A constant and a linear trend — <c>"ct"</c>.</summary>
    ConstantAndTrend,

    /// <summary>A constant, a linear and a quadratic trend — <c>"ctt"</c>. ADF only.</summary>
    ConstantAndQuadraticTrend,
}
