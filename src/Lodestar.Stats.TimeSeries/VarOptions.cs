namespace Lodestar.Stats.TimeSeries;

/// <summary>What a vector autoregression should estimate.</summary>
/// <remarks>
/// <c>statsmodels</c>' <c>VAR(y).fit(p, trend="c")</c> by default: a constant in every equation. Setting
/// <see cref="WithIntercept"/> to <see langword="false"/> is its <c>trend="n"</c>. The reference's <c>"ct"</c> and
/// <c>"ctt"</c> trends are not fitted here.
/// </remarks>
public sealed record VarOptions
{
    /// <summary>Whether each equation carries a constant; <see langword="true"/> by default.</summary>
    public bool WithIntercept { get; init; } = true;
}
