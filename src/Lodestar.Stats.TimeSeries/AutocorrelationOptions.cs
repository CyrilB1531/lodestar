namespace Lodestar.Stats.TimeSeries;

/// <summary>What an autocorrelation may be told.</summary>
/// <remarks>
/// Checked where it is set rather than where it is read: a confidence level read three functions
/// later reaches the caller as a band of the wrong width instead of an exception naming it (#617).
/// </remarks>
public sealed record AutocorrelationOptions
{
    private double _confidenceLevel = 0.95;

    /// <summary>
    /// Whether lag <c>k</c> divides by <c>n - k</c> rather than by <c>n</c>. Default false, which
    /// is the reference's, and the estimator that keeps the sequence positive semi-definite.
    /// </summary>
    public bool Adjusted { get; init; }

    /// <summary>
    /// Whether the band widens with the lag by Bartlett's formula. Default true, which is the
    /// reference's. False gives the flat band a correlogram usually draws.
    /// </summary>
    /// <remarks>
    /// <see cref="SerialCorrelation.PartialAutocorrelation"/> ignores this: its band is flat at
    /// <c>1/n</c> whatever it says, because Bartlett's formula is about an autocorrelation.
    /// </remarks>
    public bool BartlettConfidenceInterval { get; init; } = true;

    /// <summary>The two-sided level the band is reported at. Default 0.95.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value does not lie strictly inside (0, 1).</exception>
    public double ConfidenceLevel
    {
        get => _confidenceLevel;
        init
        {
            if (double.IsNaN(value) || value <= 0.0 || value >= 1.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value, "A confidence level lies strictly inside (0, 1).");
            }

            _confidenceLevel = value;
        }
    }
}
