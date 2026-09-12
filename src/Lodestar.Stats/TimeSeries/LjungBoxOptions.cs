namespace Lodestar.Stats.TimeSeries;

/// <summary>What a Ljung-Box test may be told.</summary>
public sealed record LjungBoxOptions
{
    private int _modelDegreesOfFreedom;

    /// <summary>
    /// How many parameters the model whose residuals these are consumed. Default 0, for a raw
    /// series. Each lag's degrees of freedom is its own lag less this.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is negative.</exception>
    public int ModelDegreesOfFreedom
    {
        get => _modelDegreesOfFreedom;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value, "A model cannot consume a negative number of parameters.");
            }

            _modelDegreesOfFreedom = value;
        }
    }

    /// <summary>Whether the Box-Pierce statistic is reported beside Ljung-Box. Default false.</summary>
    /// <remarks>
    /// Box-Pierce is the same sum without the <c>n/(n - k)</c> weight, so it is the smaller of the
    /// two and the less powerful in a short series. It is here because the reference offers it and
    /// a reader comparing an old paper's numbers needs it.
    /// </remarks>
    public bool BoxPierce { get; init; }
}
