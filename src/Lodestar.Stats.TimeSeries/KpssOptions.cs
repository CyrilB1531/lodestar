namespace Lodestar.Stats.TimeSeries;

/// <summary>What a KPSS test may be told.</summary>
public sealed record KpssOptions
{
    private TrendTerms _regression = TrendTerms.Constant;
    private int _lagCount;

    /// <summary>
    /// Stationarity around a level (<see cref="TrendTerms.Constant"/>, the default) or around a line
    /// (<see cref="TrendTerms.ConstantAndTrend"/>).
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Any other value: KPSS defines neither.</exception>
    public TrendTerms Regression
    {
        get => _regression;
        init
        {
            if (value is not (TrendTerms.Constant or TrendTerms.ConstantAndTrend))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value, "KPSS is defined around a level or a line, and nothing else.");
            }

            _regression = value;
        }
    }

    /// <summary>How the lag window is chosen. Default <see cref="KpssLagRule.Automatic"/>.</summary>
    public KpssLagRule LagRule { get; init; } = KpssLagRule.Automatic;

    /// <summary>The lag window under <see cref="KpssLagRule.Fixed"/>; ignored otherwise.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is negative.</exception>
    public int LagCount
    {
        get => _lagCount;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "A lag window cannot be negative.");
            }

            _lagCount = value;
        }
    }
}
