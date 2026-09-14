using BenchmarkDotNet.Attributes;
using NumFlat;
using LodestarKMeans = Lodestar.Cluster.KMeans;
using LodestarKMeansOptions = Lodestar.Cluster.KMeansOptions;
using NumFlatKMeans = NumFlat.Clustering.KMeans;

namespace Lodestar.Text.Benchmarks;

// CA1822: see LevenshteinIncumbentBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// Lloyd's iterations alone: <see cref="LodestarKMeans.Fit"/> from given centres against NumFlat's
/// <c>KMeans.Update</c> run the same number of times from the same centres (#681).
/// </summary>
/// <remarks>
/// The one like-for-like pair a k-means comparison has: neither side's initialisation is in the
/// measured call, and both return the same centres, checked in <c>GlobalSetup</c> before either is
/// timed. The blobs overlap, so the loop runs for more than two iterations.
/// </remarks>
[MemoryDiagnoser]
public class KMeansLloydIncumbentBenchmarks
{
    private ClusterBlobs _blobs = null!;
    private LodestarKMeansOptions _options = null!;
    private NumFlatKMeans _start = null!;
    private int _iterations;

    /// <summary>Rows, features and clusters.</summary>
    [Params("10000x2x8", "10000x16x16", "50000x8x32")]
    public string Shape { get; set; } = "10000x2x8";

    [GlobalSetup]
    public void Setup()
    {
        _blobs = ClusterBlobs.Generate(Shape, spread: 4.0, seed: 681);
        double[] first = _blobs.FirstRows;
        _options = new LodestarKMeansOptions { InitialCentres = first, Tolerance = 0.0 };
        _start = new NumFlatKMeans(Enumerable.Range(0, _blobs.ClusterCount)
            .Select(c => new Vec<double>(first.AsSpan(c * _blobs.FeatureCount, _blobs.FeatureCount).ToArray())));

        LodestarKMeans ours = Lodestar_Lloyd();
        _iterations = ours.Iterations;
        KMeansAgreement.RequireSameCentres(ours, NumFlat_Lloyd(), identity: true, _blobs);
    }

    [Benchmark(Baseline = true)]
    public LodestarKMeans Lodestar_Lloyd() =>
        LodestarKMeans.Fit(_blobs.Matrix, _blobs.FeatureCount, _blobs.ClusterCount, _options);

    [Benchmark]
    public NumFlatKMeans NumFlat_Lloyd()
    {
        NumFlatKMeans model = _start;
        for (int step = 0; step < _iterations; step++)
        {
            model = model.Update(_blobs.Rows).Item1;
        }

        return model;
    }
}
