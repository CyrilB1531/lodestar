namespace Lodestar.Stats.Internal;

/// <summary>The key test of the thread-static caches kept for the last shapes: LogBeta's, Stirling's constants, the
/// fraction's terms and log Γ's, one rule where each carried its own (#1745's Review A).</summary>
internal static class ShapeKey
{
    /// <summary>Whether <paramref name="a"/> and <paramref name="b"/> are the cached shapes bit for bit. A cache's first
    /// key, zero or NaN, matches no shape, none being either.</summary>
    internal static bool Matches(double a, double b, double cachedA, double cachedB)
    {
        // S1244: the same shapes, bit for bit, are the only ones whose cached values these are.
#pragma warning disable S1244
        return cachedA == a && cachedB == b;
#pragma warning restore S1244
    }
}
