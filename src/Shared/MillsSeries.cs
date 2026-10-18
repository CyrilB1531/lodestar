namespace Lodestar.Internal;

/// <summary>The asymptotic series of Mills' ratio, <c>Φ̄(z)/φ(z) = (1/z) Σ (−1)^k (2k−1)!! / z^(2k)</c>, without its 1/z.</summary>
/// <remarks>One copy for the normal tail's log and erfcx in Lodestar.Stats and the generalized gamma quantile's deep tail in
/// Lodestar.Survival (#1729's Review A).</remarks>
internal static class MillsSeries
{
    /// <summary>The sum to <paramref name="terms"/> terms past the first, at <paramref name="inverseSquare"/> = 1/z².</summary>
    internal static double Sum(double inverseSquare, int terms)
    {
        double term = 1.0;
        double sum = 1.0;
        for (int k = 1; k <= terms; k++)
        {
            term *= -((2.0 * k) - 1.0) * inverseSquare;
            sum += term;
        }
        return sum;
    }
}
