using BenchmarkDotNet.Attributes;
using Lodestar.Text.Distances;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

/// <summary>
/// Micro-benchmarks for <see cref="Levenshtein"/>. Performance is the project's
/// selling point (§7), so it is measured from Lot 1, not bolted on later.
/// </summary>
[MemoryDiagnoser]
public class LevenshteinBenchmarks
{
    private string _a = string.Empty;
    private string _b = string.Empty;
    private string _cjkA = string.Empty;
    private string _cjkB = string.Empty;

    /// <summary>Length of the generated operands.</summary>
    /// <remarks>128 is the two-word pattern, the first length the paired kernel takes (#718).</remarks>
    [Params(8, 64, 128, 512)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        // Deterministic operands that differ in a few scattered positions —
        // representative of typo/near-duplicate matching.
        (_a, _b) = ScatteredPair.Build(Length);
        (_cjkA, _cjkB) = ScatteredPair.Build(Length, alphabet: Alphabets.Cjk);
    }

    [Benchmark(Baseline = true)]
    public int Distance_Utf16() => Levenshtein.Distance(_a, _b);

    /// <summary>The same shape over CJK, which leaves Latin-1 and so takes the blocked kernel past one word.</summary>
    [Benchmark]
    public int Distance_Utf16_Cjk() => Levenshtein.Distance(_cjkA, _cjkB);

    [Benchmark]
    public int Distance_CodePoint() => Levenshtein.Distance(_a, _b, TextElement.CodePoint);

    [Benchmark]
    public double NormalizedSimilarity_Utf16() => Levenshtein.NormalizedSimilarity(_a, _b);
}
