using System.Text.Json;
using Xunit;

namespace Lodestar.Cluster.Tests;

/// <summary>
/// Replays <c>sklearn.cluster.AgglomerativeClustering</c> over the frozen cases of
/// <c>tests/oracles/cluster_agglomerative.json</c>.
/// </summary>
/// <remarks>
/// <c>labels_</c> and <c>children_</c> are compared <strong>exactly</strong>: they are integers and
/// the tree is deterministic, ties included. <c>distances_</c> is compared at the repository's 1e-9.
/// Every fixture runs under all four linkages, because the reference sends three of them to scipy
/// and one to its own spanning tree, and the two break ties differently.
/// </remarks>
public sealed class AgglomerativeClusteringOracleTests
{
    private const double Tolerance = 1e-9;

    private static readonly JsonDocument Corpus = OracleLoader.Load("cluster_agglomerative.json");

    private static IReadOnlyList<JsonElement> Cases =>
        [.. Corpus.RootElement.GetProperty("cases").EnumerateArray()];

    public static TheoryData<int> Indices()
    {
        var data = new TheoryData<int>();
        for (int i = 0; i < Cases.Count; i++)
        {
            data.Add(i);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_labels_are_the_references(int index)
    {
        JsonElement frozen = Cases[index];

        AgglomerativeClustering fitted = Fit(frozen);

        Assert.Equal(Ints(frozen, "labels"), fitted.Labels);
        Assert.Equal(frozen.GetProperty("cluster_count").GetInt32(), fitted.ClusterCount);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_merge_tree_is_the_references(int index)
    {
        JsonElement frozen = Cases[index];

        AgglomerativeClustering fitted = Fit(frozen);

        Assert.Equal(Ints(frozen, "children"), fitted.Children);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_merge_heights_are_the_references(int index)
    {
        JsonElement frozen = Cases[index];
        double[] expected = Doubles(frozen, "distances");

        AgglomerativeClustering fitted = Fit(frozen);

        Assert.Equal(expected.Length, fitted.Distances.Count);
        for (int merge = 0; merge < expected.Length; merge++)
        {
            Assert.Equal(expected[merge], fitted.Distances[merge], Tolerance);
        }
    }

    private static AgglomerativeClustering Fit(JsonElement frozen)
    {
        double[] samples = Doubles(frozen, "samples");
        int features = frozen.GetProperty("featureCount").GetInt32();
        Linkage linkage = frozen.GetProperty("linkage").GetString() switch
        {
            "complete" => Linkage.Complete,
            "average" => Linkage.Average,
            "single" => Linkage.Single,
            _ => Linkage.Ward,
        };

        return frozen.GetProperty("mode").GetString() == "distance_threshold"
            ? AgglomerativeClustering.FitToThreshold(
                samples, features, frozen.GetProperty("distance_threshold").GetDouble(), linkage)
            : AgglomerativeClustering.Fit(
                samples, features, frozen.GetProperty("n_clusters").GetInt32(), linkage);
    }

    private static int[] Ints(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetInt32())];

    private static double[] Doubles(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetDouble())];
}
