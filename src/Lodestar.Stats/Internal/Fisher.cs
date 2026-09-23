namespace Lodestar.Stats.Internal;

/// <summary>The Fisher z transform, and its inverse where the framework has none.</summary>
/// <remarks>
/// <c>Math.Atanh</c> arrived with netstandard2.1, so the older target reaches the same value
/// through the identity it is defined by. The two agree to the last bits of a double over
/// <c>(-1, 1)</c> except within about <c>1e-8</c> of either endpoint, where both are already
/// past the range a correlation's interval can report — <c>Math.Tanh</c> maps anything above
/// 19 back onto exactly 1.
/// </remarks>
internal static class Fisher
{
    internal static double Atanh(double r)
    {
#if NETSTANDARD2_0
        // atanh(r) = ln((1 + r) / (1 - r)) / 2, the definition Math.Atanh evaluates.
        return 0.5 * Math.Log((1.0 + r) / (1.0 - r));
#else
        return Math.Atanh(r);
#endif
    }
}
