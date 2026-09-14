using BenchmarkDotNet.Attributes;
using Lodestar.Text.Distances;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

/// <summary><see cref="Indel"/> in code-point mode, on operands that leave the BMP.</summary>
/// <remarks>
/// <see cref="IndelBenchmarks"/> is ASCII, so its <c>Distance_CodePoint</c> row takes the route for
/// text with no surrogate. This one takes the route that renames astral code points before the
/// bit-parallel kernel, which nothing measured while the mode sat on the dynamic program (#675).
/// </remarks>
[MemoryDiagnoser]
public class IndelCodePointBenchmarks
{
    // U+1F300..U+1FAFF, as LevenshteinCodePointBenchmarks draws from: every one a surrogate pair.
    private const int SupplementaryBase = 0x1F300;
    private const int SupplementaryCount = 0x1FAFF - 0x1F300 + 1;
    private const int Distinct = 32;

    private string _a = string.Empty;
    private string _b = string.Empty;

    /// <summary>Length of the generated operands, in code points.</summary>
    [Params(20, 128, 512)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var rng = new Random(675);
        int[] a = new int[Length];
        int[] b = new int[Length];
        for (int i = 0; i < Length; i++)
        {
            a[i] = SupplementaryBase + (rng.Next(Distinct) * SupplementaryCount / Distinct);
            b[i] = SupplementaryBase + (rng.Next(Distinct) * SupplementaryCount / Distinct);
        }

        _a = Compose(a);
        _b = Compose(b);
    }

    private static string Compose(int[] codePoints)
    {
        var text = new System.Text.StringBuilder(codePoints.Length * 2);
        foreach (int codePoint in codePoints)
        {
            text.Append(char.ConvertFromUtf32(codePoint));
        }
        return text.ToString();
    }

    [Benchmark(Baseline = true)]
    public int Distance_CodePoint() => Indel.Distance(_a, _b, TextElement.CodePoint);

    // Context, not a comparison: the same operands as UTF-16 units, twice as long, a different answer.
    [Benchmark]
    public int Distance_Utf16() => Indel.Distance(_a, _b);
}
