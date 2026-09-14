using BenchmarkDotNet.Attributes;
using Lodestar.Fuzzy;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245 / CA5394: a seeded Random builds a reproducible benchmark corpus; no security use.
// CA1822: BenchmarkDotNet rejects static benchmarks, as FuzzBenchmarks records.
#pragma warning disable S2245, CA5394, CA1822

/// <summary><see cref="Fuzz.PartialRatio"/> with a needle past one machine word (#720).</summary>
/// <remarks>
/// <see cref="FuzzBenchmarks"/>' sentence pair is 43 characters, under the 64 that
/// <c>ShortNeedleWindows</c> takes. <see cref="Embedded"/> is the shape the function is for, a needle
/// found inside a text twice its length; <see cref="EqualLength"/> is the scattered pair the distance
/// benchmarks use, where every window is an edge window and both orientations are scored.
/// </remarks>
[MemoryDiagnoser]
public class PartialRatioLongNeedleBenchmarks
{
    private string _needle = string.Empty;
    private string _haystack = string.Empty;
    private string _a = string.Empty;
    private string _b = string.Empty;

    /// <summary>Needle length: just past one word, two words, eight words.</summary>
    [Params(65, 128, 512)]
    public int NeedleLength { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        (_needle, string mutated) = ScatteredPair.Build(NeedleLength);
        var rng = new Random(720);
        char[] before = new char[NeedleLength / 2];
        char[] after = new char[NeedleLength - before.Length];
        for (int i = 0; i < before.Length; i++)
        {
            before[i] = Alphabets.Latin[rng.Next(Alphabets.Latin.Length)];
        }
        for (int i = 0; i < after.Length; i++)
        {
            after[i] = Alphabets.Latin[rng.Next(Alphabets.Latin.Length)];
        }
        _haystack = new string(before) + mutated + new string(after);
        (_a, _b) = ScatteredPair.Build(NeedleLength, seed: 7);
    }

    [Benchmark]
    public double Embedded() => Fuzz.PartialRatio(_needle, _haystack);

    [Benchmark]
    public double EqualLength() => Fuzz.PartialRatio(_a, _b);
}
