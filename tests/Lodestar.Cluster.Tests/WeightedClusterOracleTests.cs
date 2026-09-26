using System.Text.Json;
using Xunit;

namespace Lodestar.Cluster.Tests;

/// <summary>
/// Replays weighted <c>KMeans</c> and <c>DBSCAN</c>, and <c>KMeans</c> restarted over given starts, from
/// <c>tests/oracles/cluster_weighted.json</c> (#1163).
/// </summary>
/// <remarks>Centres and inertias at <c>1e-9</c>, labels and core indices exactly.</remarks>
public sealed class WeightedClusterOracleTests
{
    private const double Tolerance = 1e-9;

    private static readonly JsonDocument Corpus = OracleLoader.Load("cluster_weighted.json");

    private static IReadOnlyList<JsonElement> Section(string name) => [.. Corpus.RootElement.GetProperty(name).EnumerateArray()];

    public static TheoryData<string, int> KMeansCases()
    {
        var data = new TheoryData<string, int>();
        foreach (string section in new[] { "kmeans", "restarts" })
        {
            for (int i = 0; i < Section(section).Count; i++)
            {
                data.Add(section, i);
            }
        }

        return data;
    }

    public static TheoryData<int> DbscanCases()
    {
        var data = new TheoryData<int>();
        for (int i = 0; i < Section("dbscan").Count; i++)
        {
            data.Add(i);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(KMeansCases))]
    public void The_fit_matches_the_reference(string section, int index)
    {
        JsonElement c = Section(section)[index];
        string name = c.GetProperty("name").GetString()!;
        var options = new KMeansOptions
        {
            MaxIterations = c.GetProperty("max_iter").GetInt32(),
            Tolerance = c.GetProperty("tol").GetDouble(),
            InitialCentres = c.TryGetProperty("initial_centres", out JsonElement single) ? Doubles(single) : null,
            InitialCentreSets = c.TryGetProperty("initial_centre_sets", out JsonElement sets)
                ? [.. sets.EnumerateArray().Select(Doubles)]
                : null,
        };
        double[] samples = Doubles(c.GetProperty("samples"));
        int features = c.GetProperty("featureCount").GetInt32();
        int clusters = c.GetProperty("cluster_count").GetInt32();
        JsonElement weights = c.GetProperty("weights");
        KMeans model = weights.ValueKind == JsonValueKind.Null
            ? KMeans.Fit(samples, features, clusters, options)
            : KMeans.Fit(samples, Doubles(weights), features, clusters, options);

        double[] centres = Doubles(c.GetProperty("centres"));
        for (int i = 0; i < centres.Length; i++)
        {
            Assert.True(Math.Abs(centres[i] - model.Centres[i]) <= Tolerance, $"{name}: centre {i} {model.Centres[i]} against {centres[i]}");
        }

        Assert.Equal([.. c.GetProperty("labels").EnumerateArray().Select(v => v.GetInt32())], model.Labels);
        Assert.Equal(c.GetProperty("inertia").GetDouble(), model.Inertia, Tolerance);
        Assert.Equal(c.GetProperty("iterations").GetInt32(), model.Iterations);
    }

    [Theory]
    [MemberData(nameof(DbscanCases))]
    public void The_weighted_dbscan_matches_the_reference(int index)
    {
        JsonElement c = Section("dbscan")[index];
        double[] samples = Doubles(c.GetProperty("samples"));
        double[] weights = Doubles(c.GetProperty("weights"));
        double eps = c.GetProperty("eps").GetDouble();
        int minimum = c.GetProperty("min_samples").GetInt32();
        Dbscan model = c.GetProperty("metric").GetString() == "precomputed"
            ? Dbscan.FitPrecomputed(samples, c.GetProperty("featureCount").GetInt32(), eps, minimum, weights)
            : Dbscan.Fit(samples, c.GetProperty("featureCount").GetInt32(), eps, minimum, weights);

        Assert.Equal([.. c.GetProperty("labels").EnumerateArray().Select(v => v.GetInt32())], model.Labels);
        Assert.Equal([.. c.GetProperty("core_sample_indices").EnumerateArray().Select(v => v.GetInt32())], model.CoreSampleIndices);
    }

    private static double[] Doubles(JsonElement element) => [.. element.EnumerateArray().Select(v => v.GetDouble())];
}
