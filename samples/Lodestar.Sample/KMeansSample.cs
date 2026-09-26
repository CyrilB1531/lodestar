using Lodestar.Cluster;

namespace Lodestar.Sample;

/// <summary>Partitioning a small matrix, and scoring it with Lodestar.Metrics.</summary>
internal static class KMeansSample
{
    public static void Run()
    {
        Console.WriteLine("k-means (Lodestar.Cluster)");

        // Row-major, two features per row: two low points, two high, one in between.
        double[] samples = [0.0, 0.0, 0.0, 1.0, 10.0, 10.0, 10.0, 11.0, 5.0, 5.0];

        // Centres given rather than drawn, so the run is reproducible anywhere (ADR 0004).
        KMeans model = KMeans.Fit(samples, featureCount: 2, clusterCount: 3,
            new KMeansOptions { InitialCentres = [0.0, 0.0, 10.0, 10.0, 5.0, 5.0] });

        Console.WriteLine($"  {model.ClusterCount} clusters x {model.FeatureCount} features, {model.Iterations} iterations");
        Console.WriteLine($"  centres          : {Inv.List(model.Centres)}");
        Console.WriteLine($"  labels / inertia : [{string.Join(", ", model.Labels)}] / {Inv.F4(model.Inertia)}");
        Console.WriteLine($"  unseen rows      : [{string.Join(", ", model.Predict([0.5, 0.5, 9.5, 10.5]))}]");

        // One weight per row: the middle point counts three times, and alone in its cluster it moves no centre.
        KMeans weighted = KMeans.Fit(samples, [1.0, 1.0, 1.0, 1.0, 3.0], featureCount: 2, clusterCount: 3,
            new KMeansOptions { InitialCentres = [0.0, 0.0, 10.0, 10.0, 5.0, 5.0] });
        Console.WriteLine($"  weighted inertia : {Inv.F4(weighted.Inertia)}");
        Console.WriteLine();
    }
}
