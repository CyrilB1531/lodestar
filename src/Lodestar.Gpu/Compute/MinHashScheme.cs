namespace Lodestar.Gpu.Compute;

/// <summary>Which permutation family a signature is built from.</summary>
/// <remarks>
/// Declared here rather than shared with <c>Lodestar.Text</c>'s enum of the same name, because
/// this package carries no edge to any other — the whole point of the satellite tier, and the
/// reason <c>MersennePrime</c> and <c>Mask32</c> are spelled twice as well. The two are the same
/// two families, and the corpus that freezes one freezes the other.
/// </remarks>
public enum MinHashScheme
{
    /// <summary><c>(a·h + b) mod (2^61 − 1)</c>, masked to 32 bits.</summary>
    Legacy = 0,

    /// <summary><c>a·fmix32(h) + b</c> in 32-bit arithmetic, with an odd <c>a</c>.</summary>
    /// <remarks>
    /// The MurmurHash3 finalizer is applied as the shared tile is filled, so it costs once per
    /// token per group rather than once per token per thread — and, more to the point, it leaves
    /// <see cref="DeviceTokenHashes"/> scheme-neutral. Mixing before upload would make a resident
    /// batch belong to one family, which is exactly what residency exists not to do.
    /// </remarks>
    Affine32 = 1,
}
