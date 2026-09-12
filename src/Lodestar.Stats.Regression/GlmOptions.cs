namespace Lodestar.Stats.Regression;

/// <summary>What a <see cref="GeneralizedLinearModel"/> fit may be told.</summary>
public sealed class GlmOptions
{
    /// <summary>Whether a column of ones is prepended to the design. Default true.</summary>
    public bool WithIntercept { get; init; } = true;

    /// <summary>The two-sided level the intervals are reported at. Default 0.95.</summary>
    public double ConfidenceLevel { get; init; } = 0.95;

    /// <summary>How many IRLS iterations are allowed. Default 100, which is the reference's.</summary>
    public int MaximumIterations { get; init; } = 100;

    /// <summary>The tolerance, used as both the absolute and relative term. Default 1e-8.</summary>
    public double Tolerance { get; init; } = 1e-8;

    /// <summary>Whether a fit that did not converge throws instead of returning. Default true.</summary>
    /// <remarks>
    /// A non-converged inference table is plausible and wrong — enormous standard errors and
    /// p-values that read like p-values. Turn this off to inspect one, or to freeze one in a
    /// corpus, and read <see cref="GlmSummary.Converged"/> before anything else (#616).
    /// </remarks>
    public bool ThrowOnNonConvergence { get; init; } = true;
}
