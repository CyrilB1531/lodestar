using System.Text.Json;
using Lodestar.Abstractions;
using Xunit;

namespace Lodestar.Decomposition.Tests;

/// <summary>Replays the <c>transform</c> section of <c>tests/oracles/decomposition_nmf.json</c>.</summary>
/// <remarks>
/// The corpus exists because <c>NMF.transform</c> turned out to be deterministic: with H held
/// fixed and the multiplicative solver, the reference fills W with <c>sqrt(X.mean() / k)</c>
/// rather than drawing it, so no <c>random_state</c> reaches it and decision 0005's `1e-9`
/// applies (#1124).
/// </remarks>
public sealed class NmfTransformTests
{
    private const double Tolerance = 1e-9;

    private static readonly JsonDocument Document = OracleLoader.Load("decomposition_nmf.json");

    private static readonly IReadOnlyList<JsonElement> Cases =
        [.. Document.RootElement.GetProperty("transform").EnumerateArray()];

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
    public void Transform_matches_scikit_learn(int index)
    {
        JsonElement c = Cases[index];
        Nmf fit = Fit(c);

        double[] actual = fit.Transform(Unseen(c));
        double[] expected = Doubles(c, "transformed_w");

        Assert.Equal(expected.Length, actual.Length);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.True(
                Math.Abs(expected[i] - actual[i]) <= Tolerance,
                $"case {index} ({c.GetProperty("shape").GetString()}) W[{i}]: " +
                $"{expected[i]} differs from {actual[i]}");
        }
    }

    /// <summary>The fitted H is what the transform holds fixed, so it must come back untouched.</summary>
    [Theory]
    [MemberData(nameof(Indices))]
    public void Transform_leaves_the_components_alone(int index)
    {
        JsonElement c = Cases[index];
        Nmf fit = Fit(c);
        double[] before = [.. fit.Components];

        fit.Transform(Unseen(c));

        Assert.Equal(before, fit.Components);
    }

    [Fact]
    public void A_matrix_of_the_wrong_width_is_refused()
    {
        Nmf fit = Fit(Cases[0]);
        var narrow = new CsrMatrix(2, fit.FeatureCount - 1, [1.0], [0], [0, 1, 1]);

        Assert.Throws<ArgumentException>(() => fit.Transform(narrow));
    }

    /// <summary>The reference refuses a row count of zero too, naming the same minimum.</summary>
    [Fact]
    public void A_matrix_with_no_row_is_refused()
    {
        Nmf fit = Fit(Cases[0]);
        var empty = new CsrMatrix(0, fit.FeatureCount, [], [], [0]);

        Assert.Throws<ArgumentException>(() => fit.Transform(empty));
    }

    [Fact]
    public void A_null_matrix_is_refused()
    {
        Nmf fit = Fit(Cases[0]);

        Assert.Throws<ArgumentNullException>(() => fit.Transform(null!));
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void A_value_no_factorization_admits_is_refused(double value)
    {
        Nmf fit = Fit(Cases[0]);
        var matrix = new CsrMatrix(1, fit.FeatureCount, [value], [0], [0, 1]);

        Assert.Throws<ArgumentException>(() => fit.Transform(matrix));
    }

    private static double[] Doubles(JsonElement c, string name) =>
        [.. c.GetProperty(name).EnumerateArray().Select(x => x.GetDouble())];

    private static int[] Ints(JsonElement c, string name) =>
        [.. c.GetProperty(name).EnumerateArray().Select(x => x.GetInt32())];

    private static Nmf Fit(JsonElement c) => Nmf.Fit(
        new CsrMatrix(
            c.GetProperty("rows").GetInt32(),
            c.GetProperty("columns").GetInt32(),
            Doubles(c, "values"),
            Ints(c, "column_indices"),
            Ints(c, "row_pointers")),
        Doubles(c, "initial_w"),
        Doubles(c, "initial_h"),
        new NmfOptions
        {
            BetaLoss = c.GetProperty("beta_loss").GetString() == "kullback-leibler"
                ? NmfBetaLoss.KullbackLeibler
                : NmfBetaLoss.Frobenius,
            MaxIterations = c.GetProperty("max_iterations").GetInt32(),
            Tolerance = c.GetProperty("tolerance").GetDouble(),
        });

    private static CsrMatrix Unseen(JsonElement c) => new(
        c.GetProperty("new_rows").GetInt32(),
        c.GetProperty("columns").GetInt32(),
        Doubles(c, "new_values"),
        Ints(c, "new_column_indices"),
        Ints(c, "new_row_pointers"));
}
