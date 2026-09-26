using Lodestar.Cluster;
// The Dbscan 3.0.0 incumbent owns a namespace of that name in this project.
using LodestarDbscan = Lodestar.Cluster.Dbscan;

namespace Lodestar.Text.Benchmarks.CrossLang;

/// <summary>The #1163 weighted k-means and DBSCAN against <c>bench/python/bench_cluster_weighted.py</c>: same data, starts and method.</summary>
/// <remarks>k-means from one given start and from four, then DBSCAN, all with whole weights from one to five.</remarks>
public static class ClusterWeightedCrossLang
{
    private const int KMeansFeatures = 8;
    private const int Clusters = 16;
    private const int Starts = 4;
    private const double Epsilon = 0.3;
    private const int Minimum = 10;

    private static readonly int[] KMeansSizes = [10_000, 100_000];
    private static readonly int[] DbscanSizes = [5_000, 20_000];

    /// <summary>Runs every shape and writes <c>bench/results/csharp-cluster-weighted.json</c>.</summary>
    public static void Run()
    {
        string outPath = Path.Combine(BenchCorpus.RepoRoot(), "bench", "results", "csharp-cluster-weighted.json");
        var results = new List<Harness.OperationResult>();
        foreach (int n in KMeansSizes)
        {
            double[] x = Blobs(n, KMeansFeatures, Clusters);
            double[] w = Weights(n);
            var one = new KMeansOptions { InitialCentres = Start(x, n, 0) };
            var several = new KMeansOptions
            {
                InitialCentreSets = [.. Enumerable.Range(0, Starts).Select(s => Start(x, n, s))],
            };
            results.Add(Harness.Measure($"kmeans_weighted_{n}", () => KMeans.Fit(x, w, KMeansFeatures, Clusters, one)));
            results.Add(Harness.Measure($"kmeans_restarts_{n}", () => KMeans.Fit(x, w, KMeansFeatures, Clusters, several)));
        }

        foreach (int n in DbscanSizes)
        {
            double[] x = Blobs(n, 2, 10);
            double[] w = Weights(n);
            results.Add(Harness.Measure($"dbscan_weighted_{n}", () => LodestarDbscan.Fit(x, 2, Epsilon, Minimum, w)));
        }

        Harness.Write(
            outPath,
            new Harness.Output
            {
                Metadata = new Harness.OutputMetadata
                {
                    Side = "csharp",
                    Library = "Lodestar",
                    Runtime = Environment.Version.ToString(),
                    Os = Environment.OSVersion.ToString(),
                    MinTimeS = Harness.MinTimeSeconds,
                    Repeats = Harness.RepeatCount,
                },
                Results = results,
            });
    }

    /// <summary>The Python side's formula: row i sits in blob i % count, jittered by a hash of its index.</summary>
    private static double[] Blobs(int n, int features, int count)
    {
        var values = new double[n * features];
        for (long i = 0; i < n; i++)
        {
            for (long j = 0; j < features; j++)
            {
                double centre = ((((i % count) * 7919) + (j * 104729)) % 97 / 97.0 * 20.0) - 10.0;
                double jitter = ((((i * 2654435761) + (j * 40503)) % 10007) / 10007.0) - 0.5;
                values[(i * features) + j] = centre + (2.0 * jitter);
            }
        }

        return values;
    }

    /// <summary>Whole weights from one to five, as deduplicated rows would carry.</summary>
    private static double[] Weights(int n)
    {
        var weights = new double[n];
        for (int i = 0; i < n; i++)
        {
            weights[i] = 1 + (i * 37 % 5);
        }

        return weights;
    }

    /// <summary>Start <paramref name="index"/>: the rows picked by a stride, distinct for every size here.</summary>
    private static double[] Start(double[] x, int n, int index)
    {
        var centres = new double[Clusters * KMeansFeatures];
        for (int c = 0; c < Clusters; c++)
        {
            int row = ((c * 131) + (index * 17)) % n;
            Array.Copy(x, row * KMeansFeatures, centres, c * KMeansFeatures, KMeansFeatures);
        }

        return centres;
    }
}
