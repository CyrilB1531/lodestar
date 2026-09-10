using System.Security.Cryptography;
using System.Text;

namespace Lodestar.Gpu.Benchmarks;

// CA5350 (weak cryptographic algorithm): SHA-1 is the hash Lodestar.Text.Similarity.MinHash
// uses, so a comparison has to supply the same one. Never a signature or a credential.
#pragma warning disable CA5350

/// <summary>The token hash the CPU path uses: the first four bytes of SHA-1, little-endian.</summary>
internal static class TokenHash
{
    /// <summary>Hashes one token the way <c>MinHash</c> does.</summary>
    public static uint Of(string token)
    {
        Span<byte> digest = stackalloc byte[20];
        SHA1.HashData(Encoding.UTF8.GetBytes(token), digest);
        return digest[0] | ((uint)digest[1] << 8) | ((uint)digest[2] << 16) | ((uint)digest[3] << 24);
    }
}
