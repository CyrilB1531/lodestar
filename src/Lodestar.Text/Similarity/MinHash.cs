using System.Security.Cryptography;
using System.Text;

namespace Lodestar.Text.Similarity;

/// <summary>MinHash signatures, and the Jaccard estimate two of them give.</summary>
/// <remarks>
/// Reference behavior: <c>datasketch</c> 1.6.5's <c>MinHash</c>. Broder's sketch over a
/// universal hash family, with the coefficients supplied rather than seeded
/// (<see cref="MinHashPermutations"/>). Immutable once built and safe to use from any
/// number of threads at once.
/// </remarks>
public sealed class MinHash
{
    /// <summary>The Mersenne prime the permutation reduces through, <c>2^61 - 1</c>.</summary>
    private const ulong MersennePrime = (1UL << 61) - 1UL;

    /// <summary>The 32-bit mask a permuted value is cut to, <c>2^32 - 1</c>.</summary>
    /// <remarks>
    /// Measured: the reference masks with a bitwise AND, not a modulo. The two agree only
    /// when the value is below the mask, so choosing wrong moves every signature — with
    /// <c>a = 775169054918279404</c>, <c>b = 1758426461858698312</c> and <c>h</c> the hash
    /// of <c>"hello"</c>, the AND gives 228630785 and the modulo 718713858.
    /// </remarks>
    private const ulong Mask32 = (1UL << 32) - 1UL;

    private readonly MinHashPermutations _permutations;

    /// <summary>How long a signature this produces.</summary>
    public int Length => _permutations.Count;

    /// <summary>Builds a hasher over one set of permutations.</summary>
    /// <param name="permutations">The coefficients every signature is built from.</param>
    /// <exception cref="ArgumentNullException"><paramref name="permutations"/> is null.</exception>
    public MinHash(MinHashPermutations permutations)
    {
        Guard.NotNull(permutations);
        _permutations = permutations;
    }

    /// <summary>The signature of a set of tokens.</summary>
    /// <param name="tokens">
    /// The set. Repeats change nothing — a minimum is idempotent — which is what makes this
    /// a set sketch rather than a bag one.
    /// </param>
    /// <returns><see cref="Length"/> values; an empty set gives every slot its maximum.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="tokens"/>, or a token, is null.</exception>
    public uint[] Signature(IEnumerable<string> tokens)
    {
        Guard.NotNull(tokens);

        uint[] signature = new uint[_permutations.Count];
        for (int i = 0; i < signature.Length; i++)
        {
            signature[i] = uint.MaxValue;
        }

        foreach (string token in tokens)
        {
            Guard.NotNull(token);
            ulong hash = Hash32(token);
            for (int i = 0; i < signature.Length; i++)
            {
                // Unchecked on purpose: the reference multiplies in 64-bit unsigned
                // arithmetic and relies on the wrap, so overflow is the specification.
                ulong permuted;
                unchecked
                {
                    permuted = ((_permutations.Multiplier(i) * hash) + _permutations.Addend(i)) % MersennePrime;
                }

                uint candidate = (uint)(permuted & Mask32);
                if (candidate < signature[i])
                {
                    signature[i] = candidate;
                }
            }
        }

        return signature;
    }

    /// <summary>The estimated Jaccard similarity of two signatures.</summary>
    /// <param name="left">One signature.</param>
    /// <param name="right">The other, of the same length.</param>
    /// <returns>The share of slots that agree, in <c>[0, 1]</c>.</returns>
    /// <exception cref="ArgumentException">The two are not the same non-zero length.</exception>
    public static double Jaccard(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right)
    {
        if (left.Length == 0 || left.Length != right.Length)
        {
            throw new ArgumentException(
                $"signatures of {left.Length} and {right.Length} cannot be compared.", nameof(right));
        }

        int agreed = 0;
        for (int i = 0; i < left.Length; i++)
        {
            if (left[i] == right[i])
            {
                agreed++;
            }
        }

        return (double)agreed / left.Length;
    }

    /// <summary>The reference's 32-bit hash: the first four bytes of SHA-1, little-endian.</summary>
    /// <remarks>
    /// SHA-1 is used as a hash function here and not as a signature; the reference exports
    /// this as <c>sha1_hash32</c>, so it is part of the contract rather than an internal.
    /// </remarks>
    // long-comment: CA5350 and S4790 both read this as weak cryptography, and neither
    // applies. SHA-1 is used here to spread tokens over 32 bits, never to sign, seal or
    // authenticate anything: nothing downstream trusts a signature, and a collision costs
    // an over-estimated similarity rather than a forged one. The reference exports the
    // same construction as sha1_hash32, so every frozen value in the corpus depends on it
    // -- a stronger digest would be a different algorithm and would fail the parity tests
    // that give this type its meaning, not fix a weakness. Both branches below are the one
    // call, so both rules are disabled across the pair.
#pragma warning disable CA5350, S4790
    private static ulong Hash32(string token)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(token);
#if NET6_0_OR_GREATER
        Span<byte> digest = stackalloc byte[20];
        SHA1.HashData(bytes, digest);
#else
        using SHA1 sha1 = SHA1.Create();
        byte[] digest = sha1.ComputeHash(bytes);
#endif
        return (ulong)digest[0]
            | ((ulong)digest[1] << 8)
            | ((ulong)digest[2] << 16)
            | ((ulong)digest[3] << 24);
    }
#pragma warning restore CA5350, S4790
}
