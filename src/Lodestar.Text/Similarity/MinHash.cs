using Lodestar.Text.Vectorization;

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

    /// <summary>The affine-32 coefficients cut to their width once, empty under the legacy scheme.</summary>
    private readonly uint[] _a32;

    /// <summary>The affine-32 addends, in the order of <see cref="_a32"/>.</summary>
    private readonly uint[] _b32;

    /// <summary>How long a signature this produces.</summary>
    public int Length => _permutations.Count;

    /// <summary>Builds a hasher over one set of permutations.</summary>
    /// <param name="permutations">The coefficients every signature is built from.</param>
    /// <exception cref="ArgumentNullException"><paramref name="permutations"/> is null.</exception>
    public MinHash(MinHashPermutations permutations)
    {
        Guard.NotNull(permutations);
        _permutations = permutations;
        _a32 = [];
        _b32 = [];
        if (permutations.Scheme == MinHashScheme.Affine32)
        {
            _a32 = new uint[permutations.Count];
            _b32 = new uint[permutations.Count];
            for (int i = 0; i < _a32.Length; i++)
            {
                _a32[i] = (uint)permutations.Multipliers[i];
                _b32[i] = (uint)permutations.Addends[i];
            }
        }
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

        bool affine = _permutations.Scheme == MinHashScheme.Affine32;
        using TokenDigest digest = TokenDigest.Sha1();
        Span<byte> bytes = stackalloc byte[20];
        foreach (string token in tokens)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(tokens), "A token is null.");
            }
            ulong hash = Hash32(digest, token, bytes);
            if (affine)
            {
                // Once per token, not per permutation: a weakly hashed input must not ride its
                // own structure through an affine map, which the prime modulus used to absorb.
                MinAffine(signature, MurmurHash3.Fmix((uint)hash));
            }
            else
            {
                MinLegacy(signature, hash);
            }
        }

        return signature;
    }

    // long-comment: which wrap is the specification, and why one branch divides.
    // The reference multiplies in unsigned arithmetic and relies on the overflow,
    // so it is the contract rather than an accident -- in 64 bits for the Mersenne
    // reduction below, and in 32 for the affine one, where the wrap *is* the modulo
    // and nothing is divided at all.
    private void MinLegacy(Span<uint> signature, ulong hash)
    {
        ReadOnlySpan<ulong> a = _permutations.Multipliers;
        ReadOnlySpan<ulong> b = _permutations.Addends;
        for (int i = 0; i < signature.Length; i++)
        {
            uint candidate;
            unchecked
            {
                // x % (2^61 - 1) without dividing: 2^61 is 1 modulo the prime, so the high three
                // bits fold onto the low 61, and the sum is below twice the prime.
                ulong x = (a[i] * hash) + b[i];
                ulong reduced = (x & MersennePrime) + (x >> 61);
                if (reduced >= MersennePrime)
                {
                    reduced -= MersennePrime;
                }

                candidate = (uint)(reduced & Mask32);
            }

            if (candidate < signature[i])
            {
                signature[i] = candidate;
            }
        }
    }

    private void MinAffine(uint[] signature, uint mixed)
    {
        int i = 0;
#if NET8_0_OR_GREATER
        int width = System.Numerics.Vector<uint>.Count;
        if (System.Numerics.Vector.IsHardwareAccelerated && signature.Length >= width)
        {
            // Wrapping 32-bit multiply, add and an unsigned minimum: exact integer arithmetic, so
            // the vector lanes give the scalar loop's values.
            var factor = new System.Numerics.Vector<uint>(mixed);
            for (; i <= signature.Length - width; i += width)
            {
                System.Numerics.Vector<uint> candidate =
                    (new System.Numerics.Vector<uint>(_a32, i) * factor) + new System.Numerics.Vector<uint>(_b32, i);
                System.Numerics.Vector.Min(new System.Numerics.Vector<uint>(signature, i), candidate)
                    .CopyTo(signature, i);
            }
        }
#endif
        for (; i < signature.Length; i++)
        {
            uint candidate;
            unchecked
            {
                candidate = (_a32[i] * mixed) + _b32[i];
            }

            if (candidate < signature[i])
            {
                signature[i] = candidate;
            }
        }
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
    private static ulong Hash32(TokenDigest digest, string token, Span<byte> bytes)
    {
        digest.Compute(token, bytes);
        return (ulong)bytes[0]
            | ((ulong)bytes[1] << 8)
            | ((ulong)bytes[2] << 16)
            | ((ulong)bytes[3] << 24);
    }
}
