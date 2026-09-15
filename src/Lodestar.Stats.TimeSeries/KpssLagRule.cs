namespace Lodestar.Stats.TimeSeries;

/// <summary>How KPSS chooses the lag window of its long-run variance.</summary>
public enum KpssLagRule
{
    /// <summary>Hobijn, Franses and Ooms (1998), from the residuals — <c>"auto"</c>, the default.</summary>
    Automatic,

    /// <summary><c>ceil(12·(n/100)^¼)</c> — <c>"legacy"</c>, the rule before statsmodels 0.12.</summary>
    Legacy,

    /// <summary><see cref="KpssOptions.LagCount"/>, as given.</summary>
    Fixed,
}
