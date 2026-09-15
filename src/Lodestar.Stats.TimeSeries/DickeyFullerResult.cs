namespace Lodestar.Stats.TimeSeries;

/// <summary>An augmented Dickey-Fuller test: the statistic, its MacKinnon p-value and what the regression used.</summary>
/// <remarks>
/// A class rather than a record, for <see cref="AutocorrelationResult"/>'s reason: a record's equality
/// would compare <see cref="CriticalValues"/> by reference.
/// </remarks>
public sealed class DickeyFullerResult
{
    internal DickeyFullerResult()
    {
    }

    /// <summary>The lagged level's t statistic in the chosen regression.</summary>
    public double Statistic { get; init; }

    /// <summary>MacKinnon's (1994) approximate p-value, against the null of a unit root.</summary>
    public double PValue { get; init; }

    /// <summary>The lag order the regression was fitted at.</summary>
    public int UsedLag { get; init; }

    /// <summary>How many rows that regression fitted: the series length less the lag, less one.</summary>
    public int ObservationCount { get; init; }

    /// <summary>MacKinnon's (2010) critical values at 1 %, 5 % and 10 %, for <see cref="ObservationCount"/>.</summary>
    public IReadOnlyList<double> CriticalValues { get; init; } = [];

    /// <summary>The winning criterion of the lag search; <c>NaN</c> under <see cref="LagSelection.Fixed"/>.</summary>
    public double InformationCriterion { get; init; }
}
