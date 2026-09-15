namespace Lodestar.Stats.TimeSeries;

/// <summary>A series split into its trend, its seasonal pattern and what neither explains.</summary>
/// <remarks>
/// Three lists the length of the series. The trend and the residual are <c>NaN</c> where the moving
/// average has no full window, unless <see cref="SeasonalDecompositionOptions.ExtrapolateTrend"/> filled
/// them. A class rather than a record, for <see cref="AutocorrelationResult"/>'s reason.
/// </remarks>
public sealed class SeasonalComponents
{
    internal SeasonalComponents()
    {
    }

    /// <summary>The centred (or trailing) moving average.</summary>
    public IReadOnlyList<double> Trend { get; init; } = [];

    /// <summary>The average detrended value at each phase, centred, tiled over the series.</summary>
    public IReadOnlyList<double> Seasonal { get; init; } = [];

    /// <summary>The series less (or divided by) the trend and the seasonal component.</summary>
    public IReadOnlyList<double> Residual { get; init; } = [];
}
