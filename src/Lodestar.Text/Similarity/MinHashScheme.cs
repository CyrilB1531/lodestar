namespace Lodestar.Text.Similarity;

/// <summary>Which permutation family a <see cref="MinHash"/> signature is built from.</summary>
/// <remarks>
/// <c>datasketch</c> had one family through 1.6.5 and named three in 2.0.0, making
/// <c>affine32</c> the default — so the same reference call that returned <see cref="Legacy"/>
/// values now returns <see cref="Affine32"/> ones. The scheme travels with the coefficients, on
/// <see cref="MinHashPermutations"/>, because they are chosen together.
/// </remarks>
public enum MinHashScheme
{
    /// <summary><c>(a·h + b) mod (2^61 − 1)</c>, masked to 32 bits.</summary>
    /// <remarks>
    /// The reference's only family through 1.6.5, and its <c>legacy</c> from 2.0.0, which
    /// computes it identically. The default here, so a signature built before 2.0.0 existed
    /// keeps comparing to one built after.
    /// </remarks>
    Legacy = 0,

    /// <summary><c>a·fmix32(h) + b</c> in 32-bit arithmetic, with an odd <c>a</c>.</summary>
    /// <remarks>
    /// The reference's default from 2.0.0. An odd multiplier makes the map a bijection — a
    /// collision would need <c>2^32</c> to divide <c>a·(h₁ − h₂)</c>, and an odd <c>a</c>
    /// contributes no factor of two — so no prime modulus is needed and none is computed. The
    /// MurmurHash3 finalizer runs once per token first, which is what lets a weakly hashed
    /// input be safe to feed an affine map.
    /// </remarks>
    Affine32 = 1,
}
