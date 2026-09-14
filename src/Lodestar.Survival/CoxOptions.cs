namespace Lodestar.Survival;

/// <summary>What a <see cref="CoxProportionalHazards"/> fit may be told.</summary>
/// <remarks>
/// Each setting is checked where it is set, as <c>GlmOptions</c> does, so a budget of zero reaches
/// the caller as an exception naming the setting rather than as a table that never iterated. There
/// is no intercept: the baseline hazard absorbs it. There is no penalizer: the reference's default is
/// zero, and a penalised fit is a different estimate with its own corpus.
/// </remarks>
public sealed record CoxOptions
{
    private double _confidenceLevel = 0.95;
    private int _maximumIterations = 100;

    /// <summary>The two-sided level the intervals are reported at. Default 0.95.</summary>
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

    /// <summary>How many Newton-Raphson iterations are allowed. Default 100.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is below one.</exception>
    public int MaximumIterations
    {
        get => _maximumIterations;
        init
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value, "An iteration budget is one or more.");
            }

            _maximumIterations = value;
        }
    }
}
