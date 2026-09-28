using System.Text.Json;
using Lodestar.Abstractions;
using Xunit;

namespace Lodestar.Decomposition.Tests;

/// <summary>
/// The wide shape — fewer rows than columns — against the <c>TruncatedSVD</c> estimator.
/// </summary>
/// <remarks>
/// Its <c>transpose="auto"</c> factors the transpose here, from an Ω drawn for <c>Xᵀ</c>, and so
/// does <see cref="TruncatedSvd.Fit"/> (#1256); two cases ask for more components than rows, and
/// keep what the rows allow (#1231). Every output the estimator reports is frozen and asserted.
/// </remarks>
public sealed class TruncatedSvdWideTests
{
    private const double Tolerance = 1e-9;

    private static readonly JsonDocument Document = OracleLoader.Load("decomposition_svd.json");

    private static IReadOnlyList<JsonElement> Cases { get; } =
        [.. Document.RootElement.GetProperty("randomized_wide").EnumerateArray()];

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

    private static int[] Ints(JsonElement c, string name) =>
        [.. c.GetProperty(name).EnumerateArray().Select(x => x.GetInt32())];

    private static CsrMatrix Matrix(JsonElement c) => new(
        c.GetProperty("rows").GetInt32(),
        c.GetProperty("columns").GetInt32(),
        Doubles(c, "values"),
        Ints(c, "column_indices"),
        Ints(c, "row_pointers"));

    private static TruncatedSvd Fit(JsonElement c) => TruncatedSvd.Fit(
        Matrix(c),
        c.GetProperty("component_count").GetInt32(),
        new TruncatedSvdOptions
        {
            Oversampling = c.GetProperty("oversampling").GetInt32(),
            PowerIterations = c.GetProperty("power_iterations").GetInt32(),
            Normalizer = c.GetProperty("normalizer").GetString() switch
            {
                "QR" => PowerIterationNormalizer.Qr,
                "LU" => PowerIterationNormalizer.Lu,
                "none" => PowerIterationNormalizer.None,
                _ => PowerIterationNormalizer.Auto,
            },
            RandomMatrix = Doubles(c, "omega"),
        });

    private static void AssertSame(double[] expected, IReadOnlyList<double> actual)
    {
        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            // Relative past 1: a singular value near 2e8 carries its last few ulps above 1e-9 absolute.
            Assert.True(
                Math.Abs(expected[i] - actual[i]) <= Tolerance * Math.Max(1.0, Math.Abs(expected[i])),
                $"[{i}] expected {expected[i]:R}, got {actual[i]:R}");
        }
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_fixture_has_fewer_rows_than_columns(int index)
    {
        CsrMatrix matrix = Matrix(Cases[index]);

        Assert.True(matrix.RowCount < matrix.ColumnCount, $"{matrix.RowCount} × {matrix.ColumnCount}");
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_singular_values_match_the_estimator(int index)
    {
        JsonElement c = Cases[index];

        AssertSame(Doubles(c, "singular_values"), Fit(c).SingularValues);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_components_match_the_estimator(int index)
    {
        JsonElement c = Cases[index];

        AssertSame(Doubles(c, "components"), Fit(c).Components);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_explained_variance_and_its_ratio_match_the_estimator(int index)
    {
        JsonElement c = Cases[index];
        TruncatedSvd fitted = Fit(c);

        AssertSame(Doubles(c, "explained_variance"), fitted.ExplainedVariance);
        AssertSame(Doubles(c, "explained_variance_ratio"), fitted.ExplainedVarianceRatio);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_projection_matches_the_estimator(int index)
    {
        JsonElement c = Cases[index];

        AssertSame(Doubles(c, "transform"), Fit(c).Transform(Matrix(c)));
    }
}
