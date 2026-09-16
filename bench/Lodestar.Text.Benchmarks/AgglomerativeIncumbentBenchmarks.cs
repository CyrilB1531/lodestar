using Aglomera;
using Aglomera.Linkage;
using BenchmarkDotNet.Attributes;
using Lodestar.Cluster;

namespace Lodestar.Text.Benchmarks;

// CA1822: see LevenshteinIncumbentBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// A whole merge tree, this package against <c>Aglomera</c> 1.1.1, under each of the four linkages (#760).
/// </summary>
/// <remarks>
/// Gaussian blobs, so no two merge heights tie: <c>Aglomera</c> reverses the reference's tie order and
/// can build a different tree where they do, and timing two different trees measures nothing.
/// <c>GlobalSetup</c> checks both return every merge height in the same order — Ward's after undoing
/// <c>Aglomera</c>'s <c>d²/2</c> scale — and the same partition at the cut, before either is timed.
/// </remarks>
[MemoryDiagnoser]
public class AgglomerativeIncumbentBenchmarks
{
    private const int Features = 4;
    private const int Blobs = 5;

    private ClusterBlobs _blobs = null!;
    private HashSet<AgglomeraPoint> _points = null!;
    private Linkage _linkage;

    /// <summary>How many samples, drawn from five blobs in four dimensions.</summary>
    [Params(500, 1500)]
    public int Rows { get; set; } = 500;

    /// <summary>The linkage, by the reference's own name.</summary>
    [Params("ward", "complete", "average", "single")]
    public string Method { get; set; } = "ward";

    [GlobalSetup]
    public void Setup()
    {
        _blobs = ClusterBlobs.Generate($"{Rows}x{Features}x{Blobs}", spread: 50.0, seed: 760);
        _points = [.. Enumerable.Range(0, Rows).Select(row =>
            new AgglomeraPoint(row, _blobs.Matrix.AsSpan(row * Features, Features).ToArray()))];
        _linkage = Method switch
        {
            "complete" => Linkage.Complete,
            "average" => Linkage.Average,
            "single" => Linkage.Single,
            _ => Linkage.Ward,
        };

        AgglomerativeAgreement.RequireSameTree(Lodestar_Fit(), Aglomera(), _linkage, Blobs);
    }

    [Benchmark(Baseline = true)]
    public AgglomerativeClustering Lodestar_Fit() =>
        AgglomerativeClustering.Fit(_blobs.Matrix, Features, Blobs, _linkage);

    [Benchmark]
    public int Aglomera_GetClustering() => Aglomera().Count;

    private ClusteringResult<AgglomeraPoint> Aglomera()
    {
        var metric = new AgglomeraEuclidean();
        ILinkageCriterion<AgglomeraPoint> criterion = _linkage switch
        {
            Linkage.Complete => new CompleteLinkage<AgglomeraPoint>(metric),
            Linkage.Average => new AverageLinkage<AgglomeraPoint>(metric),
            Linkage.Single => new SingleLinkage<AgglomeraPoint>(metric),
            _ => new WardsMinimumVarianceLinkage<AgglomeraPoint>(metric, Centroid),
        };
        return new AgglomerativeClusteringAlgorithm<AgglomeraPoint>(criterion).GetClustering(_points);
    }

    /// <summary>The centroid <c>Aglomera</c>'s Ward linkage asks the caller for.</summary>
    private static AgglomeraPoint Centroid(IEnumerable<AgglomeraPoint> members)
    {
        var total = new double[Features];
        int count = 0;
        foreach (AgglomeraPoint member in members)
        {
            for (int feature = 0; feature < Features; feature++)
            {
                total[feature] += member.Values[feature];
            }

            count++;
        }

        for (int feature = 0; feature < Features; feature++)
        {
            total[feature] /= count;
        }

        return new AgglomeraPoint(-1, total);
    }
}
