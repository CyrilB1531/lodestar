using BenchmarkDotNet.Attributes;
using Lodestar.Text.Distances;

namespace Lodestar.Text.Benchmarks;

/// <summary>
/// <see cref="Osa"/> over the same operands <see cref="LevenshteinBenchmarks"/> measures, so the
/// two restricted edit distances can be read side by side.
/// </summary>
/// <remarks>
/// 64 is the last length the single-word kernel takes; 128 falls back to the dynamic program
/// after the affix trim.
/// </remarks>
[MemoryDiagnoser]
public class OsaBenchmarks
{
    private string _a = string.Empty;
    private string _b = string.Empty;

    /// <summary>Length of the generated operands.</summary>
    [Params(8, 32, 64, 128)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup() => (_a, _b) = ScatteredPair.Build(Length);

    [Benchmark(Baseline = true)]
    public int Distance_Utf16() => Osa.Distance(_a, _b);

    [Benchmark]
    public int Distance_CodePoint() => Osa.Distance(_a, _b, TextElement.CodePoint);
}
