using System.Security.Cryptography;
using System.Text;

namespace Lodestar.Text.Similarity;

/// <summary>Charikar's SimHash: one 64-bit fingerprint per document, near-duplicates near each other.</summary>
/// <remarks>
/// Reference behavior: <c>simhash</c> 2.1.2. Where <see cref="MinHash"/> estimates Jaccard over
/// a set, this estimates cosine over a weighted bag — so a token repeated twice counts twice,
/// which is the difference that decides which of the two a caller wants. Thread-safe.
/// </remarks>
public static class SimHash
{
    /// <summary>How many bits a fingerprint carries.</summary>
    public const int Bits = 64;

    /// <summary>The fingerprint of a bag of tokens, each weighted once.</summary>
    /// <param name="tokens">The tokens; repeats add weight rather than being ignored.</param>
    /// <returns>A 64-bit fingerprint; an empty bag gives zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="tokens"/>, or a token, is null.</exception>
    public static ulong Fingerprint(IEnumerable<string> tokens)
    {
        Guard.NotNull(tokens);
        return Fold(Weigh(tokens));
    }

    /// <summary>The fingerprint of tokens carrying their own weights.</summary>
    /// <param name="weighted">Token and weight pairs; a weight may be any non-negative count.</param>
    /// <returns>A 64-bit fingerprint; an empty sequence gives zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="weighted"/>, or a token, is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">A weight is negative.</exception>
    public static ulong Fingerprint(IEnumerable<KeyValuePair<string, int>> weighted)
    {
        Guard.NotNull(weighted);
        long[] columns = new long[Bits];
        foreach (KeyValuePair<string, int> pair in weighted)
        {
            Guard.NotNull(pair.Key);
            if (pair.Value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(weighted), pair.Value, "A token's weight is not negative.");
            }

            Accumulate(columns, pair.Key, pair.Value);
        }

        return Fold(columns);
    }

    /// <summary>How many bits two fingerprints differ in.</summary>
    /// <param name="left">One fingerprint.</param>
    /// <param name="right">The other.</param>
    /// <returns>A count in <c>[0, 64]</c>; zero means the two documents fingerprint alike.</returns>
    public static int HammingDistance(ulong left, ulong right)
    {
        ulong difference = left ^ right;
#if NET6_0_OR_GREATER
        return System.Numerics.BitOperations.PopCount(difference);
#else
        int count = 0;
        while (difference != 0)
        {
            difference &= difference - 1;
            count++;
        }

        return count;
#endif
    }

    /// <summary>One column per bit, each the signed weight of the tokens setting it.</summary>
    private static long[] Weigh(IEnumerable<string> tokens)
    {
        long[] columns = new long[Bits];
        foreach (string token in tokens)
        {
            Guard.NotNull(token);
            Accumulate(columns, token, 1);
        }

        return columns;
    }

    /// <summary>Adds or subtracts one token's weight in every column, by that token's hash.</summary>
    private static void Accumulate(long[] columns, string token, int weight)
    {
        ulong hash = Hash64(token);
        for (int bit = 0; bit < Bits; bit++)
        {
            bool set = ((hash >> bit) & 1UL) != 0UL;
            columns[bit] += set ? weight : -weight;
        }
    }

    /// <summary>Sets each bit whose column came out positive.</summary>
    private static ulong Fold(long[] columns)
    {
        ulong fingerprint = 0UL;
        for (int bit = 0; bit < Bits; bit++)
        {
            if (columns[bit] > 0)
            {
                fingerprint |= 1UL << bit;
            }
        }

        return fingerprint;
    }

    // long-comment: CA5351 and S4790 both read this as broken cryptography, and neither
    // applies. MD5 is used here to spread tokens over 64 bits, never to sign, seal or
    // authenticate anything: a collision costs two documents an equal fingerprint, which
    // this type already treats as a candidate to verify rather than an answer. The
    // reference hashes with MD5, so every frozen fingerprint in the corpus depends on it
    // -- a stronger digest would be a different algorithm and would fail the parity tests
    // that give this type its meaning, not fix a weakness. Both branches below are the one
    // call, so both rules are disabled across the pair.
#pragma warning disable CA5351, S4790
    /// <summary>The reference's hash: MD5 read as a big integer, of which the low 64 bits are used.</summary>
    /// <remarks>
    /// The digest is big-endian as a number, so its low 64 bits are its <em>last</em> eight
    /// bytes, most significant first. Reading the first eight instead fingerprints every
    /// document differently while looking equally plausible.
    /// </remarks>
    private static ulong Hash64(string token)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(token);
#if NET6_0_OR_GREATER
        Span<byte> digest = stackalloc byte[16];
        MD5.HashData(bytes, digest);
#else
        using MD5 md5 = MD5.Create();
        byte[] digest = md5.ComputeHash(bytes);
#endif
        ulong low = 0UL;
        for (int i = 8; i < 16; i++)
        {
            low = (low << 8) | digest[i];
        }

        return low;
    }
#pragma warning restore CA5351, S4790
}
