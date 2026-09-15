namespace Lodestar.Stats.TimeSeries;

/// <summary>What a seasonal decomposition may be told.</summary>
public sealed record SeasonalDecompositionOptions
{
    private int _extrapolateTrend;

    /// <summary>Additive (the default) or multiplicative.</summary>
    public SeasonalModel Model { get; init; } = SeasonalModel.Additive;

    /// <summary>Whether the moving average is centred (the default) or trails the point.</summary>
    public bool TwoSided { get; init; } = true;

    /// <summary>
    /// How many of the nearest defined trend points, less one, fit the lines that fill the trend's
    /// undefined ends. Zero, the default, leaves them <c>NaN</c>; the reference's <c>"period"</c> is
    /// <c>period − 1</c>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is negative.</exception>
    public int ExtrapolateTrend
    {
        get => _extrapolateTrend;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "An extrapolation window cannot be negative.");
            }

            _extrapolateTrend = value;
        }
    }
}
