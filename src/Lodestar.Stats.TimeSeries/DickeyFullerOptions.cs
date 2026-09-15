namespace Lodestar.Stats.TimeSeries;

/// <summary>What an augmented Dickey-Fuller test may be told.</summary>
public sealed record DickeyFullerOptions
{
    private int? _maxLag;

    /// <summary>The deterministic terms of the regression. Default <see cref="TrendTerms.Constant"/>.</summary>
    public TrendTerms Regression { get; init; } = TrendTerms.Constant;

    /// <summary>How the lag order is chosen. Default <see cref="LagSelection.Akaike"/>.</summary>
    public LagSelection LagSelection { get; init; } = LagSelection.Akaike;

    /// <summary>
    /// The largest lag the search considers, or the lag itself under <see cref="LagSelection.Fixed"/>.
    /// Null takes Schwert's <c>ceil(12·(n/100)^¼)</c>, capped at <c>n/2 − terms − 1</c>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is negative.</exception>
    public int? MaxLag
    {
        get => _maxLag;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "A lag order cannot be negative.");
            }

            _maxLag = value;
        }
    }
}
