namespace Lodestar.Stats.TimeSeries;

/// <summary>A Ljung-Box test at each lag, indexed from lag 1.</summary>
/// <remarks>
/// Five parallel lists rather than a list of five-field rows, because a caller plots a column.
/// A class rather than a record, for the reason <see cref="AutocorrelationResult"/> gives.
/// </remarks>
public sealed class LjungBoxResult
{
    internal LjungBoxResult()
    {
    }

    /// <summary>The Ljung-Box statistic cumulated to each lag.</summary>
    public IReadOnlyList<double> Statistics { get; init; } = [];

    /// <summary>Its chi-square p-value, NaN where no degree of freedom is left.</summary>
    public IReadOnlyList<double> PValues { get; init; } = [];

    /// <summary>The Box-Pierce statistic, empty unless <see cref="LjungBoxOptions.BoxPierce"/>.</summary>
    public IReadOnlyList<double> BoxPierceStatistics { get; init; } = [];

    /// <summary>Box-Pierce's p-value, empty unless it was asked for.</summary>
    public IReadOnlyList<double> BoxPiercePValues { get; init; } = [];

    /// <summary>The lag less the model's parameters, which may be zero or negative.</summary>
    public IReadOnlyList<int> DegreesOfFreedom { get; init; } = [];
}
