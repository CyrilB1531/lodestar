namespace Lodestar.Stats;

/// <summary>Whether a p-value comes from the exact null distribution or its normal approximation.</summary>
/// <remarks>
/// scipy calls this <c>method</c> and defaults it to <c>'auto'</c>, which picks
/// exact for small untied samples and asymptotic otherwise. The choice changes
/// the number returned, not merely how long it takes, so it cannot be hidden.
/// </remarks>
public enum ExactMethod
{
    /// <summary>Exact when the sample is small and free of ties, asymptotic otherwise. scipy's <c>'auto'</c>.</summary>
    Auto,

    /// <summary>
    /// Enumerate the null distribution, whatever the sample. Measured: scipy
    /// computes an exact p-value on tied data too rather than refusing, so this
    /// does the same and the remarks say the number is only approximate there.
    /// </summary>
    Exact,

    /// <summary>Use the normal (or Kolmogorov) approximation, whatever the sample size.</summary>
    Asymptotic,
}
