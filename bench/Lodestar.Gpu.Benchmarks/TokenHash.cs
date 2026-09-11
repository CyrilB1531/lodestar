using System.Security.Cryptography;
using System.Text;

namespace Lodestar.Gpu.Benchmarks;

// long-comment: why a benchmark reaches for SHA-1, and why two rules object to it.
// CA5350 and S4790 both read this as weak cryptography, and neither applies: SHA-1 is the
// hash Lodestar.Text.Similarity.MinHash uses, so a comparison against it has to supply the
// same one, and nothing here signs, seals or authenticates anything. src/'s copy disables
// both at the equivalent call; this one disabled only the first, and the build never said
// so because Lodestar.slnx did not list this project (#649).
#pragma warning disable CA5350, S4790

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
