using BenchmarkDotNet.Attributes;
using LodestarDbscan = Lodestar.Cluster.Dbscan;

namespace Lodestar.Text.Benchmarks;

// CA1822: see LevenshteinIncumbentBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// DBSCAN over planar points, where all three implementations can compete: this package against
/// NumFlat 1.3.4 and <c>Dbscan</c> 3.0.0 (#759).
/// </summary>
/// <remarks>
/// Two features because that is <c>Dbscan</c> 3.0.0's ceiling — its <c>Point</c> carries <c>X</c>
/// and <c>Y</c> and nothing else; <see cref="DbscanDimensionBenchmarks"/> is the same measurement
/// where it cannot follow. <c>GlobalSetup</c> requires the same partition from all three, cluster
/// numbering ignored, before any is timed.
/// </remarks>
[MemoryDiagnoser]
public class DbscanIncumbentBenchmarks
{
    private DbscanBlock _block = null!;

    /// <summary>Rows, features and blobs.</summary>
    [Params("5000x2x5", "20000x2x10")]
    public string Shape { get; set; } = "5000x2x5";

    [GlobalSetup]
    public void Setup()
    {
        _block = DbscanBlock.Generate(Shape);

        LodestarDbscan ours = Lodestar_Fit();
        DbscanAgreement.RequireSamePartition(ours, NumFlat_Fit(), "NumFlat 1.3.4");
        DbscanAgreement.RequireSamePartition(ours, PlanarLabels(), "Dbscan 3.0.0");
    }

    [Benchmark(Baseline = true)]
    public LodestarDbscan Lodestar_Fit() => _block.Ours();

    [Benchmark]
    public int[] NumFlat_Fit() => _block.NumFlatLabels();

    [Benchmark]
    public global::Dbscan.ClusterSet<IndexedPoint> Dbscan_CalculateClusters() =>
        global::Dbscan.Dbscan.CalculateClusters(
            _block.Points, _block.Radius, DbscanBlock.MinimumPoints);

    /// <summary>The planar library's clusters, turned back into one label per row.</summary>
    private int[] PlanarLabels()
    {
        var labels = new int[_block.Blobs.RowCount];
        Array.Fill(labels, LodestarDbscan.Noise);
        global::Dbscan.ClusterSet<IndexedPoint> clusters = Dbscan_CalculateClusters();
        for (int cluster = 0; cluster < clusters.Clusters.Count; cluster++)
        {
            foreach (IndexedPoint point in clusters.Clusters[cluster].Objects)
            {
                labels[point.Row] = cluster;
            }
        }

        return labels;
    }
}
