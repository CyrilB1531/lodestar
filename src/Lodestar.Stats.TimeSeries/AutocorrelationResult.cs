namespace Lodestar.Stats.TimeSeries;

/// <summary>An autocorrelation sequence and the band around it, indexed by lag.</summary>
/// <remarks>
/// A class rather than a record: a record's equality compares these three lists by reference, so
/// two results holding the same numbers would compare unequal
/// ([#668](https://github.com/CyrilB1531/lodestar/issues/668)). Nobody compares two correlograms,
/// so this promises no equality at all rather than a broken one.
/// </remarks>
public sealed class AutocorrelationResult
{
    internal AutocorrelationResult()
    {
    }

    /// <summary>The correlation at each lag, from 0 — where it is always 1 — upwards.</summary>
    public IReadOnlyList<double> Values { get; init; } = [];

    /// <summary>The lower end of the band, centred on <see cref="Values"/> rather than on zero.</summary>
    public IReadOnlyList<double> ConfidenceLower { get; init; } = [];

    /// <summary>The upper end of the band.</summary>
    public IReadOnlyList<double> ConfidenceUpper { get; init; } = [];
}
