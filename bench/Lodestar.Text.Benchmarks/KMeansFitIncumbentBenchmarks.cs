using BenchmarkDotNet.Attributes;
using Meta.Numerics.Statistics;
using LodestarKMeans = Lodestar.Cluster.KMeans;
using LodestarKMeansOptions = Lodestar.Cluster.KMeansOptions;
using NumFlatKMeans = NumFlat.Clustering.KMeans;
using NumFlatKMeansOptions = NumFlat.Clustering.KMeansOptions;

namespace Lodestar.Text.Benchmarks;

// CA1822: see LevenshteinIncumbentBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// What a caller pays for a whole fit, k-means++ included: <see cref="LodestarKMeans.Fit"/> against
/// NumFlat's <c>KMeans</c> constructor and Meta.Numerics' <c>MeansClustering</c> (#681).
/// </summary>
/// <remarks>
/// Not like-for-like in the loop: each side seeds its own k-means++ and so runs its own number of
/// iterations. The blobs are separated widely enough that all three find the same partition, which
/// <c>GlobalSetup</c> checks — centres matched up to relabelling — before any is timed. NumFlat runs
/// one attempt, as this package and scikit-learn's default do; its own default is three.
/// </remarks>
[MemoryDiagnoser]
public class KMeansFitIncumbentBenchmarks
{
    private ClusterBlobs _blobs = null!;
    private LodestarKMeansOptions _options = null!;
    private NumFlatKMeansOptions _numFlatOptions = null!;

    /// <summary>Rows, features and clusters.</summary>
    [Params("10000x2x8", "10000x16x16", "50000x8x32")]
    public string Shape { get; set; } = "10000x2x8";

    [GlobalSetup]
    public void Setup()
    {
        _blobs = ClusterBlobs.Generate(Shape, spread: 1000.0, seed: 681);
        _options = new LodestarKMeansOptions { Seed = 681 };
        _numFlatOptions = new NumFlatKMeansOptions { TryCount = 1 };

        LodestarKMeans ours = Lodestar_Fit();
        KMeansAgreement.RequireSameCentres(ours, NumFlat_Fit(), identity: false, _blobs);
        KMeansAgreement.RequireSameCentres(ours, MetaNumerics_Fit(), _blobs);
    }

    [Benchmark(Baseline = true)]
    public LodestarKMeans Lodestar_Fit() =>
        LodestarKMeans.Fit(_blobs.Matrix, _blobs.FeatureCount, _blobs.ClusterCount, _options);

    // SonarLint S2245, CA5394: a seeded Random picks NumFlat's starting centres; no security use.
#pragma warning disable S2245, CA5394
    [Benchmark]
    public NumFlatKMeans NumFlat_Fit() =>
        new(_blobs.Rows, _blobs.ClusterCount, _numFlatOptions, new Random(681));
#pragma warning restore S2245, CA5394

    [Benchmark]
    public MeansClusteringResult MetaNumerics_Fit() =>
        Multivariate.MeansClustering(_blobs.Columns, _blobs.ClusterCount);
}
