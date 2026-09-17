using System.Security.Cryptography;
using System.Text;
using Lodestar.Text.Similarity;
using Xunit;

namespace Lodestar.Text.Tests.Similarity;

/// <summary>
/// The signature loops against the formula they compute, at lengths that leave a vector tail and
/// with coefficients that push the Mersenne reduction to its edges.
/// </summary>
public sealed class MinHashKernelTests
{
    private const ulong Prime = (1UL << 61) - 1UL;

    private static readonly string[] Tokens =
        ["", "a", "hello", "h\u00e9llo", "\ud83d\ude00", new string('x', 300), "term0001", "\ud800"];

    [Theory]
    [InlineData(1)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(33)]
    [InlineData(130)]
    public void Affine32_signature_is_the_formula(int length)
    {
        // CA5394: seeded coefficients, the same on every run; nothing here is secret.
#pragma warning disable CA5394
        var random = new Random(length);
        ulong[] a = [.. Enumerable.Range(0, length).Select(_ => (ulong)random.NextInt64(0, uint.MaxValue) | 1UL)];
        ulong[] b = [.. Enumerable.Range(0, length).Select(_ => (ulong)random.NextInt64(0, (long)uint.MaxValue + 1))];
#pragma warning restore CA5394
        uint[] signature = new MinHash(new MinHashPermutations(a, b, MinHashScheme.Affine32)).Signature(Tokens);

        for (int i = 0; i < length; i++)
        {
            uint expected = uint.MaxValue;
            foreach (string token in Tokens)
            {
                uint candidate = unchecked(((uint)a[i] * Fmix((uint)Sha1Hash32(token))) + (uint)b[i]);
                expected = Math.Min(expected, candidate);
            }

            Assert.Equal(expected, signature[i]);
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(64)]
    public void Legacy_signature_is_the_formula(int length)
    {
#pragma warning disable CA5394
        var random = new Random(length + 1_000);
        ulong[] a = [.. Enumerable.Range(0, length).Select(_ => (ulong)random.NextInt64(1, long.MaxValue) * 2UL)];
        ulong[] b = [.. Enumerable.Range(0, length).Select(_ => (ulong)random.NextInt64(0, long.MaxValue) * 2UL)];
#pragma warning restore CA5394
        // The largest operands, and the prime itself, which reduces to zero rather than to the prime.
        a[0] = ulong.MaxValue;
        b[0] = ulong.MaxValue;
        if (length > 1)
        {
            a[1] = Prime;
            b[1] = Prime;
        }

        uint[] signature = new MinHash(new MinHashPermutations(a, b)).Signature(Tokens);

        for (int i = 0; i < length; i++)
        {
            uint expected = uint.MaxValue;
            foreach (string token in Tokens)
            {
                ulong hash = Sha1Hash32(token);
                uint candidate = (uint)(unchecked((a[i] * hash) + b[i]) % Prime & uint.MaxValue);
                expected = Math.Min(expected, candidate);
            }

            Assert.Equal(expected, signature[i]);
        }
    }

    // CA5350, S4790: SHA-1 is the reference's token hash, restated here to check against.
#pragma warning disable CA5350, S4790
    private static ulong Sha1Hash32(string token)
    {
        byte[] digest = SHA1.HashData(Encoding.UTF8.GetBytes(token));
        return digest[0] | ((ulong)digest[1] << 8) | ((ulong)digest[2] << 16) | ((ulong)digest[3] << 24);
    }
#pragma warning restore CA5350, S4790

    private static uint Fmix(uint h)
    {
        unchecked
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;
            return h;
        }
    }
}
