using System.Text.Json;
using Xunit;

namespace Lodestar.Cluster.Tests;

/// <summary>
/// Replays <c>sklearn.cluster.KMeans(algorithm="lloyd")</c> over the eight frozen cases of
/// <c>tests/oracles/cluster_kmeans.json</c>.
/// </summary>
/// <remarks>
/// Every case passes the starting centres, so what is under test is Lloyd and not a draw —
/// decision 0072's move, applied here. Each case is chosen for a branch: strict convergence,
/// an empty cluster, a run cut short by <c>max_iter</c>, a zero tolerance, and a stop on the
/// scaled shift.
/// </remarks>
public sealed class KMeansOracleTests
{
    /// <summary>The tolerance the whole repository uses for oracle replay.</summary>
    private const double Tolerance = 1e-9;

    private static readonly JsonDocument Corpus = OracleLoader.Load("cluster_kmeans.json");

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

    private static double[] Doubles(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetDouble())];

    private static KMeans Fit(JsonElement frozen) => KMeans.Fit(
        Doubles(frozen, "samples"),
        frozen.GetProperty("feature_count").GetInt32(),
        frozen.GetProperty("cluster_count").GetInt32(),
        new KMeansOptions
        {
            InitialCentres = Doubles(frozen, "initial_centres"),
            MaxIterations = frozen.GetProperty("max_iter").GetInt32(),
            Tolerance = frozen.GetProperty("tol").GetDouble(),
        });

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_centres_match_the_reference(int index)
    {
        JsonElement frozen = Cases[index];
        double[] expected = Doubles(frozen, "centres");

        KMeans model = Fit(frozen);

        Assert.Equal(expected.Length, model.Centres.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], model.Centres[i], Tolerance);
        }
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_labels_and_the_inertia_match_the_reference(int index)
    {
        JsonElement frozen = Cases[index];
        int[] expected = [.. frozen.GetProperty("labels").EnumerateArray().Select(v => v.GetInt32())];

        KMeans model = Fit(frozen);

        Assert.Equal(expected, model.Labels);
        Assert.Equal(frozen.GetProperty("inertia").GetDouble(), model.Inertia, Tolerance);
    }

    /// <summary>
    /// The iteration count is part of the contract, not an implementation detail: it is what
    /// says whether the run stopped on the labels, on the shift or on <c>max_iter</c>.
    /// </summary>
    [Theory]
    [MemberData(nameof(Indices))]
    public void The_iteration_count_matches_the_reference(int index)
    {
        JsonElement frozen = Cases[index];

        Assert.Equal(frozen.GetProperty("iterations").GetInt32(), Fit(frozen).Iterations);
    }

    /// <summary>
    /// Assigning the fitted samples again must reproduce the fitted labels. That is the
    /// property the reference protects with a final E-step when it stops on the shift rather
    /// than on the labels, and the one that breaks if that step is skipped.
    /// </summary>
    [Theory]
    [MemberData(nameof(Indices))]
    public void Predicting_the_fitted_samples_reproduces_the_fitted_labels(int index)
    {
        JsonElement frozen = Cases[index];
        KMeans model = Fit(frozen);

        Assert.Equal(model.Labels, model.Predict(Doubles(frozen, "samples")));
    }
}
