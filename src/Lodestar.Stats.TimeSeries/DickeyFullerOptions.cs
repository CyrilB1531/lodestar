namespace Lodestar.Stats.TimeSeries;

/// <summary>What an augmented Dickey-Fuller test may be told.</summary>
public sealed record DickeyFullerOptions
{
    private int? _maxLag;
    private TrendTerms _regression = TrendTerms.Constant;
    private LagSelection _lagSelection = LagSelection.Akaike;

    /// <summary>The deterministic terms of the regression. Default <see cref="TrendTerms.Constant"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException">A value <see cref="TrendTerms"/> does not declare.</exception>
    public TrendTerms Regression
    {
        get => _regression;
        init
        {
            // An undeclared value used to fail later, naming a parameter the caller never passed (#907).
            if (value is not (TrendTerms.None or TrendTerms.Constant or TrendTerms.ConstantAndTrend or TrendTerms.ConstantAndQuadraticTrend))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Not a trend specification.");
            }

            _regression = value;
        }
    }

    /// <summary>How the lag order is chosen. Default <see cref="LagSelection.Akaike"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException">A value <see cref="LagSelection"/> does not declare.</exception>
    public LagSelection LagSelection
    {
        get => _lagSelection;
        init
        {
            // An undeclared value used to search by Akaike's criterion without saying so (#907).
            if (value is not (LagSelection.Akaike or LagSelection.Schwarz or LagSelection.TStatistic or LagSelection.Fixed))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Not a lag selection rule.");
            }

            _lagSelection = value;
        }
    }

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
