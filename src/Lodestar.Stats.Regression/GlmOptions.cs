namespace Lodestar.Stats.Regression;

/// <summary>What a <see cref="GeneralizedLinearModel"/> fit may be told.</summary>
/// <remarks>
/// Every setting is checked where it is set rather than where it is read, as
/// <see cref="OlsOptions"/> already does: a budget of zero or a tolerance of zero reaches the
/// caller as a table of <c>0/0</c> instead of an exception naming the setting (#616).
/// </remarks>
public sealed class GlmOptions
{
    private double _confidenceLevel = 0.95;
    private int _maximumIterations = 100;
    private double _tolerance = 1e-8;

    /// <summary>Whether a column of ones is prepended to the design. Default true.</summary>
    public bool WithIntercept { get; init; } = true;

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

    /// <summary>How many IRLS iterations are allowed. Default 100, which is the reference's.</summary>
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

    /// <summary>
    /// The absolute bound on the change in deviance between iterations, which is the reference's
    /// <c>atol</c> with its <c>rtol</c> left at zero. Default 1e-8.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is not above zero.</exception>
    public double Tolerance
    {
        get => _tolerance;
        init
        {
            if (double.IsNaN(value) || value <= 0.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value, "A convergence tolerance is above zero.");
            }

            _tolerance = value;
        }
    }

    /// <summary>Whether a fit that did not converge throws instead of returning. Default true.</summary>
    /// <remarks>
    /// A non-converged inference table is plausible and wrong — enormous standard errors and
    /// p-values that read like p-values. Turn this off to inspect one, or to freeze one in a
    /// corpus, and read <see cref="GlmSummary.Converged"/> before anything else (#616).
    /// </remarks>
    public bool ThrowOnNonConvergence { get; init; } = true;
}
