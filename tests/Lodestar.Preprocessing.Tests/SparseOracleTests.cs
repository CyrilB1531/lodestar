using System.Text.Json;
using Lodestar.Abstractions;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// Replays the three scalers scikit-learn fits on a sparse matrix, against its dense answer for the
/// same data (#765).
/// </summary>
/// <remarks>
/// A sparse fit is not a different statistic: it is the same one read without visiting the zeros.
/// Every case asserts both, so an implementation that skipped the absent zeros in a mean — the easy
/// mistake — matches neither.
/// </remarks>
public sealed class SparseOracleTests
{
    /// <summary>The tolerance the whole repository uses for oracle replay.</summary>
    private const double Tolerance = 1e-9;

    private static readonly JsonDocument Corpus = OracleLoader.Load("preprocessing_sparse.json");

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
    public void Every_case_matches_scikit_learn(int index)
    {
        JsonElement frozen = Cases[index];
        string name = frozen.GetProperty("name").GetString()!;
        CsrMatrix matrix = Matrix(frozen);
        double[] samples = Doubles(frozen, "samples");
        int featureCount = frozen.GetProperty("columnCount").GetInt32();

        IReadOnlyList<double> sparse;
        IReadOnlyList<double> dense;
        switch (frozen.GetProperty("scaler").GetString())
        {
            case "maxabs":
                sparse = MaxAbsScaler.Fit(matrix).Scale;
                dense = MaxAbsScaler.Fit(samples, featureCount).Scale;
                break;
            case "robust":
                sparse = RobustScaler.Fit(matrix).Scale!;
                dense = RobustScaler.Fit(
                    samples, featureCount, new RobustScalerOptions { WithCentring = false }).Scale!;
                break;
            default:
                sparse = StandardScaler.Fit(matrix).Scale!;
                dense = StandardScaler.Fit(
                    samples, featureCount, new StandardScalerOptions { WithMean = false }).Scale!;
                break;
        }

        AssertSame(Doubles(frozen, "sparseScale"), sparse, $"{name}: sparse");

        // The same statistic, read the other way: the dense overload on the same data.
        AssertSame(Doubles(frozen, "denseScale"), dense, $"{name}: dense");
    }

    [Fact]
    public void Every_frozen_case_is_replayed()
    {
        Assert.Equal(Corpus.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(), Cases.Count);
    }

    private static CsrMatrix Matrix(JsonElement frozen) =>
        new(
            frozen.GetProperty("rowCount").GetInt32(),
            frozen.GetProperty("columnCount").GetInt32(),
            Doubles(frozen, "sparseValues"),
            [.. frozen.GetProperty("columnIndices").EnumerateArray().Select(v => v.GetInt32())],
            [.. frozen.GetProperty("rowPointers").EnumerateArray().Select(v => v.GetInt32())]);

    private static double[] Doubles(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetDouble())];

    private static void AssertSame(double[] expected, IReadOnlyList<double> actual, string what)
    {
        Assert.True(expected.Length == actual.Count, $"{what}: {expected.Length} values expected, {actual.Count} reported");
        for (int i = 0; i < expected.Length; i++)
        {
            double allowed = Math.Max(Tolerance, Math.Abs(expected[i]) * Tolerance);
            Assert.True(
                Math.Abs(expected[i] - actual[i]) <= allowed,
                $"{what}[{i}]: expected {expected[i]}, got {actual[i]}");
        }
    }
}
