using System.Text.Json;
using Xunit;

namespace Lodestar.Decomposition.Tests;

/// <summary>
/// The explained variance against scikit-learn's <c>PCA(svd_solver="full")</c>, and what it refuses.
/// </summary>
/// <remarks>
/// Only variances are compared: a component's sign is <c>svd_flip</c>'s convention, and nothing
/// this type reports depends on one. The corpus freezes the <c>n &lt; p</c> edge, where the
/// component count is capped at the sample count and centring leaves the last component empty.
/// </remarks>
public sealed class PrincipalComponentVarianceTests
{
    private const double Tolerance = 1e-9;

    private static readonly JsonDocument Document = OracleLoader.Load("decomposition_pca.json");

    private static IReadOnlyList<JsonElement> Cases { get; } =
        [.. Document.RootElement.GetProperty("cases").EnumerateArray()];

    public static TheoryData<int> Indices()
    {
        var data = new TheoryData<int>();
        for (int i = 0; i < Cases.Count; i++)
        {
            data.Add(i);
        }
        return data;
    }

    private static double[] Doubles(JsonElement c, string name) =>
        [.. c.GetProperty(name).EnumerateArray().Select(x => x.GetDouble())];

    private static PrincipalComponentVariance Compute(JsonElement c) =>
        PrincipalComponentVariance.Compute(
            Doubles(c, "matrix"), c.GetProperty("rows").GetInt32(), c.GetProperty("columns").GetInt32());

    private static void AssertSequence(double[] expected, IReadOnlyList<double> actual)
    {
        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], actual[i], Tolerance);
        }
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_component_count_matches_scikit_learn(int index)
    {
        JsonElement c = Cases[index];

        PrincipalComponentVariance result = Compute(c);

        Assert.Equal(c.GetProperty("component_count").GetInt32(), result.ComponentCount);
        Assert.Equal(c.GetProperty("rows").GetInt32(), result.SampleCount);
        Assert.Equal(c.GetProperty("columns").GetInt32(), result.FeatureCount);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_explained_variance_matches_scikit_learn(int index)
    {
        JsonElement c = Cases[index];

        PrincipalComponentVariance result = Compute(c);

        AssertSequence(Doubles(c, "explained_variance"), result.ExplainedVariance);
        Assert.Equal(c.GetProperty("total_variance").GetDouble(), result.TotalVariance, Tolerance);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_ratio_and_its_cumulative_curve_match_scikit_learn(int index)
    {
        JsonElement c = Cases[index];

        PrincipalComponentVariance result = Compute(c);

        AssertSequence(Doubles(c, "explained_variance_ratio"), result.ExplainedVarianceRatio);
        AssertSequence(
            Doubles(c, "cumulative_explained_variance_ratio"), result.CumulativeExplainedVarianceRatio);
    }

    [Fact]
    public void The_input_is_not_centred_in_place()
    {
        double[] matrix = [1.0, 2.0, 3.0, 5.0, 4.0, 9.0];
        double[] copy = [.. matrix];

        PrincipalComponentVariance.Compute(matrix, rowCount: 3, columnCount: 2);

        Assert.Equal(copy, matrix);
    }

    [Fact]
    public void A_shape_below_two_samples_or_one_feature_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => PrincipalComponentVariance.Compute([1.0, 2.0], rowCount: 1, columnCount: 2));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => PrincipalComponentVariance.Compute([], rowCount: 2, columnCount: 0));
    }

    [Fact]
    public void A_matrix_whose_length_disagrees_with_its_shape_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => PrincipalComponentVariance.Compute([1.0, 2.0, 3.0], rowCount: 2, columnCount: 2));

        Assert.Equal("matrix", error.ParamName);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void A_value_that_is_not_finite_is_refused(double value)
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => PrincipalComponentVariance.Compute([1.0, 2.0, value, 4.0], rowCount: 2, columnCount: 2));

        Assert.Equal("matrix", error.ParamName);
    }

    /// <summary>scikit-learn divides by the zero total and answers NaN; a scree of NaN is refused here.</summary>
    [Fact]
    public void A_matrix_with_no_variance_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => PrincipalComponentVariance.Compute([3.0, 7.0, 3.0, 7.0, 3.0, 7.0], rowCount: 3, columnCount: 2));

        Assert.Equal("matrix", error.ParamName);
    }
}
