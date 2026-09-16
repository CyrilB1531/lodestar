using System.Text.Json;
using Xunit;

namespace Lodestar.Cluster.Tests;

/// <summary>
/// Replays <c>sklearn.cluster.DBSCAN</c> over the fifteen frozen cases of
/// <c>tests/oracles/cluster_dbscan.json</c>.
/// </summary>
/// <remarks>
/// <strong>Compared exactly, with no tolerance.</strong> A label is an integer and the algorithm
/// discrete, so a case whose labels move has changed answer rather than drifted. Four cases are
/// the same nine points in four orders: a border sample joins whichever cluster is grown first,
/// and no single ordering shows that the label follows the growth order.
/// </remarks>
public sealed class DbscanOracleTests
{
    private static readonly JsonDocument Corpus = OracleLoader.Load("cluster_dbscan.json");

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

        Dbscan fitted = Fit(frozen);

        Assert.Equal(Ints(frozen, "labels"), fitted.Labels);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_core_samples_are_the_references(int index)
    {
        JsonElement frozen = Cases[index];

        Dbscan fitted = Fit(frozen);

        Assert.Equal(Ints(frozen, "core_sample_indices"), fitted.CoreSampleIndices);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_cluster_count_is_the_labels_own(int index)
    {
        JsonElement frozen = Cases[index];
        int[] labels = Ints(frozen, "labels");
        int expected = labels.Length == 0 ? 0 : labels.Distinct().Count(label => label != Dbscan.Noise);

        Dbscan fitted = Fit(frozen);

        Assert.Equal(expected, fitted.ClusterCount);
    }

    private static Dbscan Fit(JsonElement frozen)
    {
        double[] samples = [.. frozen.GetProperty("samples").EnumerateArray().Select(v => v.GetDouble())];
        int count = frozen.GetProperty("feature_count").GetInt32();
        double epsilon = frozen.GetProperty("eps").GetDouble();
        int minimum = frozen.GetProperty("min_samples").GetInt32();

        return frozen.GetProperty("metric").GetString() == "precomputed"
            ? Dbscan.FitPrecomputed(samples, count, epsilon, minimum)
            : Dbscan.Fit(samples, count, epsilon, minimum);
    }

    private static int[] Ints(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetInt32())];
}
