using Lodestar.Stats.Regression.Internal;

namespace Lodestar.Stats.Regression;

/// <summary>What a <see cref="GeneralizedLinearModel"/> fit may be told.</summary>
/// <remarks>
/// Every setting is checked where it is set rather than where it is read, as
/// <see cref="OlsOptions"/> already does: a budget of zero or a tolerance of zero reaches the
/// caller as a table of <c>0/0</c> instead of an exception naming the setting (#616).
/// </remarks>
public sealed record GlmOptions
{
    private double _confidenceLevel = 0.95;
    private int _maximumIterations = 100;
    private double _tolerance = 1e-8;
    private double? _negativeBinomialAlpha;
    private GlmLink _link = GlmLink.Default;

    /// <summary>Whether a column of ones is prepended to the design. Default true.</summary>
    public bool WithIntercept { get; init; } = true;

    /// <summary>The two-sided level the intervals are reported at. Default 0.95.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value does not lie strictly inside (0, 1).</exception>
    public double ConfidenceLevel
    {
        get => _confidenceLevel;
        init => _confidenceLevel = OptionGuards.ConfidenceLevel(value);
    }

    /// <summary>How many IRLS iterations are allowed. Default 100, which is the reference's.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is below one.</exception>
    public int MaximumIterations
    {
        get => _maximumIterations;
        init => _maximumIterations = OptionGuards.MaximumIterations(value);
    }

    /// <summary>
    /// The absolute bound on the change in deviance between iterations, which is the reference's
    /// <c>atol</c> with its <c>rtol</c> left at zero. Default 1e-8.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is not above zero.</exception>
    public double Tolerance
    {
        get => _tolerance;
        init => _tolerance = OptionGuards.Tolerance(value);
    }

    /// <summary>
    /// The dispersion <c>α</c> of <see cref="GlmFamily.NegativeBinomial"/>, whose variance is <c>μ + αμ²</c>.
    /// Default <see langword="null"/>, which fits the reference's own default of 1.
    /// </summary>
    /// <remarks>
    /// Given, not estimated, as <c>sm.families.NegativeBinomial(alpha=...)</c> takes it. The reference warns
    /// when it is left unset; a library has no warning channel a caller reads, so the default is stated here
    /// instead. Setting it for another family is refused by <c>GeneralizedLinearModel.Fit</c>: a
    /// setting that silently does nothing is a mistake in the call (#769).
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">The value is not finite and above zero.</exception>
    public double? NegativeBinomialAlpha
    {
        get => _negativeBinomialAlpha;
        init
        {
            if (value is { } alpha && (double.IsNaN(alpha) || double.IsInfinity(alpha) || alpha <= 0.0))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value, "A negative binomial alpha is finite and above zero.");
            }

            _negativeBinomialAlpha = value;
        }
    }

    /// <summary>The link the mean is fitted through. Default <see cref="GlmLink.Default"/>, each family's statsmodels default.</summary>
    /// <remarks>
    /// <see cref="GlmFamily.Gamma"/> takes <see cref="GlmLink.Inverse"/> or <see cref="GlmLink.Log"/>; the two
    /// count families take <see cref="GlmLink.Log"/>, their default; binomial takes only its default logit.
    /// <c>GeneralizedLinearModel.Fit</c> refuses any other pairing (#770).
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">The value is not a declared <see cref="GlmLink"/>.</exception>
    public GlmLink Link
    {
        get => _link;
        init
        {
            if (value is not (GlmLink.Default or GlmLink.Log or GlmLink.Inverse))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value, $"{value} is not a declared {nameof(GlmLink)}.");
            }

            _link = value;
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
