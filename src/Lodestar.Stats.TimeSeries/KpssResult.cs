namespace Lodestar.Stats.TimeSeries;

/// <summary>A KPSS test: the statistic, its tabulated p-value and whether that p-value was clamped.</summary>
/// <remarks>A class rather than a record, for <see cref="AutocorrelationResult"/>'s reason.</remarks>
public sealed class KpssResult
{
    internal KpssResult()
    {
    }

    /// <summary>The residual partial-sum statistic over the long-run variance.</summary>
    public double Statistic { get; init; }

    /// <summary>The p-value against the null of stationarity, interpolated in Kwiatkowski et al.'s table.</summary>
    public double PValue { get; init; }

    /// <summary>The lag window the long-run variance used.</summary>
    public int LagCount { get; init; }

    /// <summary>The critical values at 10 %, 5 %, 2.5 % and 1 %.</summary>
    public IReadOnlyList<double> CriticalValues { get; init; } = [];

    /// <summary>Whether <see cref="PValue"/> is the table's end rather than an interpolation, and which way the truth lies.</summary>
    public PValueBound PValueBound { get; init; }
}
