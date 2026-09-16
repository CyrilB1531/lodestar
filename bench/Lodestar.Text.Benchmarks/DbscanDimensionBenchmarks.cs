using BenchmarkDotNet.Attributes;
using LodestarDbscan = Lodestar.Cluster.Dbscan;

namespace Lodestar.Text.Benchmarks;

// CA1822: see LevenshteinIncumbentBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// The same DBSCAN above two features, where only NumFlat 1.3.4 can follow (#759).
/// </summary>
/// <remarks>
/// <c>Dbscan</c> 3.0.0 has no entry point here at all — the absence decision 0131 is built on —
/// so this class is two libraries rather than three, and the gap it prices is NumFlat's
/// <c>net8.0</c> floor against this package's <c>netstandard2.0</c>, not a missing algorithm.
/// The fixture and the agreement check are <see cref="DbscanBlock"/>'s, shared with
/// <see cref="DbscanIncumbentBenchmarks"/> rather than restated.
/// </remarks>
[MemoryDiagnoser]
public class DbscanDimensionBenchmarks
{
    private DbscanBlock _block = null!;

    /// <summary>Rows, features and blobs.</summary>
    [Params("10000x8x8", "5000x16x8")]
    public string Shape { get; set; } = "10000x8x8";

    [GlobalSetup]
    public void Setup()
    {
        _block = DbscanBlock.Generate(Shape);

        DbscanAgreement.RequireSamePartition(Lodestar_Fit(), NumFlat_Fit(), "NumFlat 1.3.4");
    }

    [Benchmark(Baseline = true)]
    public LodestarDbscan Lodestar_Fit() => _block.Ours();

    [Benchmark]
    public int[] NumFlat_Fit() => _block.NumFlatLabels();
}
