namespace Lodestar.Text.Similarity;

/// <summary>The permutation coefficients a MinHash signature is built from.</summary>
/// <remarks>
/// <strong>The permutations are an input, not a seed</strong> — the same call
/// [decision 0072](../../../docs/decisions/0072-omega-is-an-input-not-a-seed.md) made for
/// randomized SVD's Ω, and for the same reason: a randomized algorithm whose randomness is
/// supplied is an ordinary parity target, where one that derives it from a seed would have
/// to reproduce another library's generator stream to agree with it. <c>datasketch</c>
/// exposes its own pair as <c>MinHash.permutations</c>, and the corpus freezes them.
/// </remarks>
public sealed class MinHashPermutations
{
    private readonly ulong[] _a;
    private readonly ulong[] _b;

    /// <summary>How many permutations, and so how long a signature is.</summary>
    public int Count => _a.Length;

    /// <summary>Which arithmetic these coefficients are to be read through.</summary>
    public MinHashScheme Scheme { get; }

    /// <summary>Takes one multiplier and one addend per permutation, read as
    /// <see cref="MinHashScheme.Legacy"/>.</summary>
    /// <param name="multipliers">The <c>a</c> coefficients.</param>
    /// <param name="addends">The <c>b</c> coefficients, one per multiplier.</param>
    /// <exception cref="ArgumentException">The two are not the same non-zero length.</exception>
    public MinHashPermutations(ReadOnlySpan<ulong> multipliers, ReadOnlySpan<ulong> addends)
        : this(multipliers, addends, MinHashScheme.Legacy)
    {
    }

    /// <summary>Takes one multiplier and one addend per permutation, and the scheme that reads them.</summary>
    /// <param name="multipliers">The <c>a</c> coefficients.</param>
    /// <param name="addends">The <c>b</c> coefficients, one per multiplier.</param>
    /// <param name="scheme">Which arithmetic the coefficients belong to.</param>
    /// <exception cref="ArgumentException">
    /// The two are not the same non-zero length, <paramref name="scheme"/> is not a declared
    /// member, or a coefficient does not fit the scheme it is given.
    /// </exception>
    public MinHashPermutations(
        ReadOnlySpan<ulong> multipliers, ReadOnlySpan<ulong> addends, MinHashScheme scheme)
    {
        if (multipliers.Length == 0)
        {
            throw new ArgumentException("A signature needs at least one permutation.", nameof(multipliers));
        }

        if (multipliers.Length != addends.Length)
        {
            throw new ArgumentException(
                $"{multipliers.Length} multipliers and {addends.Length} addends.", nameof(addends));
        }

        if (scheme is not (MinHashScheme.Legacy or MinHashScheme.Affine32))
        {
            throw new ArgumentException($"{scheme} is not a permutation scheme.", nameof(scheme));
        }

        if (scheme == MinHashScheme.Affine32)
        {
            MinHashCoefficients.RefuseWhatAffine32CannotRead(
                multipliers, addends, nameof(multipliers));
        }

        Scheme = scheme;
        _a = multipliers.ToArray();
        _b = addends.ToArray();
    }

    /// <summary>The multiplier of one permutation.</summary>
    /// <param name="index">Which permutation.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the set.</exception>
    public ulong Multiplier(int index) => _a[index];

    /// <summary>The addend of one permutation.</summary>
    /// <param name="index">Which permutation.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the set.</exception>
    public ulong Addend(int index) => _b[index];
}
