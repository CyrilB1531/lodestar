using BenchmarkDotNet.Attributes;
using Lodestar.Text.Distances;

namespace Lodestar.Text.Benchmarks;

/// <summary><see cref="RatcliffObershelp"/> where one operand holds the other, and over a near-duplicate pair.</summary>
/// <remarks>
/// Containment is where the longest-match scan can stop at the first full-length run instead
/// of reading the rest of the table.
/// </remarks>
[MemoryDiagnoser]
public class RatcliffObershelpBenchmarks
{
    private string _haystack = string.Empty;
    private string _needle = string.Empty;
    private string _a = string.Empty;
    private string _b = string.Empty;

    /// <summary>Length of the longer operand.</summary>
    [Params(64, 512)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        (_a, _b) = ScatteredPair.Build(Length);
        _haystack = _a;
        // The front quarter: a match found early, which is where stopping saves the most.
        _needle = _a.Substring(Length / 8, Length / 4);
    }

    [Benchmark(Baseline = true)]
    public double Similarity_Containment() => RatcliffObershelp.Similarity(_haystack, _needle);

    [Benchmark]
    public double Similarity_NearDuplicate() => RatcliffObershelp.Similarity(_a, _b);
}
