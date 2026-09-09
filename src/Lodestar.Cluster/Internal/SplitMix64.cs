namespace Lodestar.Cluster.Internal;

/// <summary>Uniform draws from an <see cref="int"/> seed, reproducible everywhere.</summary>
/// <remarks>
/// <see cref="Random"/> is not the answer: its algorithm changed in .NET 6, so the same seed
/// gives different numbers on .NET Framework and on net10.0 — and this package ships to both.
/// A seed reproduces a run of <em>this</em> library and nothing else, which is why the oracle
/// corpus passes the initial centres explicitly rather than seeding.
/// </remarks>
internal sealed class SplitMix64
{
    private ulong _state;

    internal SplitMix64(ulong seed) => _state = unchecked(seed + 0x9E3779B97F4A7C15UL);

    /// <summary>A uniform on <c>(0, 1]</c> — the 53 significant bits of a double.</summary>
    internal double NextDouble() => ((NextState() >> 11) + 1) * (1.0 / 9007199254740992.0);

    /// <summary>SplitMix64, whose whole state is one addition and three mixes.</summary>
    private ulong NextState()
    {
        unchecked
        {
            _state += 0x9E3779B97F4A7C15UL;
            ulong z = _state;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            return z ^ (z >> 31);
        }
    }
}
