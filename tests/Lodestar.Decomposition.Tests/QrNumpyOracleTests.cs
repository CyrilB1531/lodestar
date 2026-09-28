using System.Text.Json;
using Xunit;

namespace Lodestar.Decomposition.Tests;

/// <summary>
/// The published QR against <c>numpy.linalg.qr</c>'s factors entry by entry, infinities and NaNs included (#1305).
/// </summary>
/// <remarks>
/// The reflectors are LAPACK's, so the signs agree and no per-column flip is allowed for. A non-finite entry is
/// compared exactly — which entries LAPACK's walk leaves NaN, infinite or finite is what the corpus pins — and a
/// finite one relative to the factor's largest, since a block scaled by 1e-300 or 1e200 is among the cases.
/// </remarks>
public sealed class QrNumpyOracleTests
{
    private const double Tolerance = 1e-9;

    private static readonly JsonDocument Document = OracleLoader.Load("decomposition_qr_numpy.json");

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

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_factors_are_numpys(int index)
    {
        JsonElement c = Cases[index];
        int rows = c.GetProperty("rows").GetInt32();
        int columns = c.GetProperty("columns").GetInt32();

        QrDecomposition qr = QrDecomposition.Householder(Numbers(c, "matrix"), rows, columns);

        AssertMatches(Numbers(c, "q"), qr.Q);
        AssertMatches(Numbers(c, "r"), qr.R);
    }

    [Fact]
    public void An_infinity_below_the_first_pivot_leaves_an_infinite_pivot_not_nan()
    {
        // The case #1305 reported: R[1,1] was ∞ − ∞ reflected onto itself.
        QrDecomposition qr = QrDecomposition.Householder(
            [1.0, double.PositiveInfinity, 1.0, 1.0, 2.0, 3.0], rowCount: 3, columnCount: 2);

        Assert.Equal(double.PositiveInfinity, qr.R[3]);
    }

    private static void AssertMatches(double[] expected, IReadOnlyList<double> actual)
    {
        Assert.Equal(expected.Length, actual.Count);
        double scale = expected.Where(double.IsFinite).Select(Math.Abs).DefaultIfEmpty(0.0).Max();
        for (int i = 0; i < expected.Length; i++)
        {
            if (double.IsFinite(expected[i]))
            {
                Assert.Equal(expected[i], actual[i], Tolerance * scale);
            }
            else
            {
                Assert.Equal(expected[i], actual[i]);
            }
        }
    }

    private static double[] Numbers(JsonElement c, string name) =>
    [
        .. c.GetProperty(name).EnumerateArray().Select(x => x.ValueKind == JsonValueKind.String
            ? x.GetString() switch
            {
                "NaN" => double.NaN,
                "Infinity" => double.PositiveInfinity,
                "-Infinity" => double.NegativeInfinity,
                string other => throw new InvalidDataException($"Unexpected oracle value '{other}'."),
                null => throw new InvalidDataException("A null oracle value."),
            }
            : x.GetDouble()),
    ];
}
