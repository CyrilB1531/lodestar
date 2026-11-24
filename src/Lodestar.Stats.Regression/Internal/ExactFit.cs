namespace Lodestar.Stats.Regression.Internal;

/// <summary>Whether a fit is exact, every residual it computed exactly zero, and the zero test an HC divisor, a GMM s² and a panel total share.</summary>
/// <remarks>
/// Bitwise, with no tolerance: a residual of one rounding is the ordinary formulas' to read, as statsmodels and
/// linearmodels read it. The rounding bound #1779 tested found a new edge case at each review, so exactness is what the
/// arithmetic produced (#1784; <c>docs/superpowers/specs/2026-11-19_1784_regression-review-findings.md</c>, The rule).
/// </remarks>
internal static class ExactFit
{
    /// <summary>Whether every residual is exactly zero, so <c>s² = 0</c>, and a covariance read from them zero where it divides by no zero.</summary>
    /// <remarks>
    /// Each residual, not their sum of squares: residuals below 1e-162 square to zero, and a noisy fit whose response
    /// underflows too, as at 1e-170, read <c>+∞</c> where its arithmetic reads 0/0 (Review A pass 7 on #1779). Stops at the
    /// first that is not zero.
    /// </remarks>
    public static bool Vanishes(double[] residuals) => Array.TrueForAll(residuals, IsZero);

    /// <summary>Whether a value is exactly zero, the one test the bitwise rule reads: a residual, a divisor, an <c>s²</c>.</summary>
    // S1244: exactly zero by design, the bitwise rule's one comparison: no tolerance decides a residual, a divisor or an s².
#pragma warning disable S1244
    public static bool IsZero(double value) => value == 0.0;
#pragma warning restore S1244
}
