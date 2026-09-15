using Lodestar.Stats.Regression.Internal;

namespace Lodestar.Stats.Regression;

/// <summary>What a multinomial logit fit should estimate, at what confidence, and how long Newton may run.</summary>
/// <remarks>
/// The defaults are <c>statsmodels</c>' <c>MNLogit.fit()</c>: an intercept, 35 Newton iterations, and a step of at most
/// <c>1e-8</c> in every parameter to stop. The names follow <see cref="GlmOptions"/>.
/// </remarks>
public sealed record MultinomialLogitOptions
{
    private double _confidenceLevel = 0.95;
    private int _maximumIterations = 35;
    private double _tolerance = 1e-8;

    /// <summary>Whether to fit an intercept in every equation; <see langword="true"/> by default.</summary>
    public bool WithIntercept { get; init; } = true;

    /// <summary>The confidence level of the reported intervals; 0.95 by default.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value does not lie strictly inside (0, 1).</exception>
    public double ConfidenceLevel
    {
        get => _confidenceLevel;
        init => _confidenceLevel = OptionGuards.ConfidenceLevel(value);
    }

    /// <summary>The Newton iteration budget; 35 by default, the reference's.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is below one.</exception>
    public int MaximumIterations
    {
        get => _maximumIterations;
        init => _maximumIterations = OptionGuards.MaximumIterations(value);
    }

    /// <summary>The largest step in any parameter that still counts as converged; <c>1e-8</c> by default.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is not above zero.</exception>
    public double Tolerance
    {
        get => _tolerance;
        init => _tolerance = OptionGuards.Tolerance(value);
    }

    /// <summary>Whether a fit that spends its whole budget throws; <see langword="true"/> by default.</summary>
    public bool ThrowOnNonConvergence { get; init; } = true;
}
