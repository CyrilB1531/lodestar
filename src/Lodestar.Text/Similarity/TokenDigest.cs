using System.Security.Cryptography;
using System.Text;

namespace Lodestar.Text.Similarity;

/// <summary>The digest of a token's UTF-8 bytes, with its buffers held for a whole sketch.</summary>
/// <remarks>
/// One per signature or fingerprint call, never shared, so the sketches stay thread-safe while
/// their tokens stop allocating an encoding each. On netstandard2.0 it also holds the one
/// hash algorithm instance, whose creation per token cost more than the hashing.
/// </remarks>
// long-comment: CA5350, CA5351 and S4790 read SHA-1 and MD5 as weak cryptography, and
// none applies. They spread tokens over 32 or 64 bits, never sign, seal or authenticate
// anything: a collision costs an over-estimated similarity rather than a forged one. The
// references (datasketch's sha1_hash32, simhash's MD5) define every frozen value in the
// corpus, so a stronger digest would be a different algorithm and would fail the parity
// tests that give MinHash and SimHash their meaning.
#pragma warning disable CA5350, CA5351, S4790
internal sealed class TokenDigest : IDisposable
{
    private byte[] _bytes = new byte[256];
#if NET6_0_OR_GREATER
    private readonly bool _md5;

    private TokenDigest(bool md5) => _md5 = md5;
#else
    private readonly HashAlgorithm _algorithm;

    private TokenDigest(bool md5) => _algorithm = md5 ? MD5.Create() : SHA1.Create();
#endif

    /// <summary>A SHA-1 digest, twenty bytes.</summary>
    public static TokenDigest Sha1() => new(md5: false);

    /// <summary>An MD5 digest, sixteen bytes.</summary>
    public static TokenDigest Md5() => new(md5: true);

    /// <summary>Writes the digest of <paramref name="token"/>'s UTF-8 encoding into <paramref name="digest"/>.</summary>
    public void Compute(string token, Span<byte> digest)
    {
        int needed = Encoding.UTF8.GetMaxByteCount(token.Length);
        if (_bytes.Length < needed)
        {
            _bytes = new byte[Math.Max(needed, _bytes.Length * 2)];
        }

        int length = Encoding.UTF8.GetBytes(token, 0, token.Length, _bytes, 0);
#if NET6_0_OR_GREATER
        if (_md5)
        {
            MD5.HashData(_bytes.AsSpan(0, length), digest);
        }
        else
        {
            SHA1.HashData(_bytes.AsSpan(0, length), digest);
        }
#else
        _algorithm.ComputeHash(_bytes, 0, length).CopyTo(digest);
#endif
    }

    public void Dispose()
    {
#if !NET6_0_OR_GREATER
        _algorithm.Dispose();
#endif
    }
}
#pragma warning restore CA5350, CA5351, S4790
