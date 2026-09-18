using Lodestar.Cluster;

namespace Lodestar.Sample;

/// <summary>A merge tree built once, cut at a count and at a height, under two linkages.</summary>
internal static class AgglomerativeClusteringSample
{
    public static void Run()
    {
        Console.WriteLine("Agglomerative clustering (Lodestar.Cluster)");

        // Probe for #1055, never merged: a member no package exports, so only the `sample` job fails.
        Console.WriteLine(AgglomerativeClustering.ProbeForIssue1055());

        // One feature per row: two pairs a unit apart, and one point far from both.
        double[] samples = [0.0, 1.0, 5.0, 6.0, 20.0];

        AgglomerativeClustering ward = AgglomerativeClustering.Fit(samples, featureCount: 1, clusterCount: 3);
        Console.WriteLine($"  ward, 3 clusters    : [{string.Join(", ", ward.Labels)}] ({ward.Linkage})");
        Console.WriteLine($"  merges              : {ward.Children.Count / 2}, heights {Inv.List(ward.Distances)}");

        AgglomerativeClustering single = AgglomerativeClustering.FitToThreshold(
            samples, featureCount: 1, distanceThreshold: 4.5, Linkage.Single);
        Console.WriteLine($"  single, below 4.5   : {single.ClusterCount} clusters [{string.Join(", ", single.Labels)}]");
        Console.WriteLine($"  first pair          : [{single.Children[0]}, {single.Children[1]}] in {single.FeatureCount} feature");
        Console.WriteLine();
    }
}
