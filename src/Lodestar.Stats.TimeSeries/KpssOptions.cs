namespace Lodestar.Stats.TimeSeries;

/// <summary>What a KPSS test may be told.</summary>
public sealed record KpssOptions
{
    private TrendTerms _regression = TrendTerms.Constant;
    private KpssLagRule _lagRule = KpssLagRule.Automatic;
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
    /// <exception cref="ArgumentOutOfRangeException">A value <see cref="KpssLagRule"/> does not declare.</exception>
    public KpssLagRule LagRule
    {
        get => _lagRule;
        init
        {
            // Checked here, as every other time-series option enum is, rather than when Kpss reads it (#984).
            if (value is not (KpssLagRule.Automatic or KpssLagRule.Legacy or KpssLagRule.Fixed))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Not a KPSS lag rule.");
            }

            _lagRule = value;
        }
    }

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
