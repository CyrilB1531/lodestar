namespace Lodestar.Stats.Internal;

/// <summary>The Fisher z transform, and its form where the framework has none.</summary>
/// <remarks>
/// <c>Math.Atanh</c>, the C runtime's as numpy's arctanh is on the same platform, arrived with netstandard2.1. The older target's
/// <c>ln((1 + r) / (1 − r)) / 2</c> read <c>Atanh(1e-9)</c> 8e-8 off and <c>Atanh(1e-17)</c> as 0 (#1711); it takes
/// <c>(log1p(r) − log1p(−r)) / 2</c>, within two ulps of the function, so the two targets agree to that.
/// </remarks>
internal static class Fisher
{
    internal static double Atanh(double r)
    {
#if NETSTANDARD2_0
        return 0.5 * (NearZero.Log1P(r) - NearZero.Log1P(-r));
#else
        return Math.Atanh(r);
#endif
    }
}
