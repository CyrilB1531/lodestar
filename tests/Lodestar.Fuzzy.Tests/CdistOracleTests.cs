using System.Text.Json;
using Lodestar.Text;
using Xunit;

namespace Lodestar.Fuzzy.Tests;

/// <summary>Replays <c>tests/oracles/process_cdist.json</c>, frozen at <c>float64</c>.</summary>
/// <remarks>
/// Compared at decision 0005's <c>1e-9</c>, which the corpus's own dtype is what makes possible:
/// <c>process.cdist</c> answers in <c>float32</c> unless told otherwise, and seven decimal digits
/// would make the tolerance meaningless (#1123).
/// </remarks>
public sealed class CdistOracleTests
{
    private const double Tolerance = 1e-9;

    [Fact]
    public void Cdist_matches_rapidfuzz()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "oracles", "process_cdist.json");
        using JsonDocument doc = JsonDocument.Parse(File.ReadAllText(path));
        int replayed = 0;

        foreach (JsonElement c in doc.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            JsonElement args = c.GetProperty("args");
            string[] queries = Strings(c.GetProperty("queries"));
            string[] choices = Strings(c.GetProperty("choices"));

            ScoreMatrix actual = Process.Cdist(
                queries,
                choices,
                Scorer(args.GetProperty("scorer").GetString()!, args.GetProperty("element").GetString()!),
                args.GetProperty("scoreCutoff").GetDouble());

            Assert.Equal(c.GetProperty("rows").GetInt32(), actual.Rows);
            Assert.Equal(c.GetProperty("columns").GetInt32(), actual.Columns);

            double[] expected = [.. c.GetProperty("scores").EnumerateArray().Select(v => v.GetDouble())];
            double[] flat = actual.ToArray();
            Assert.Equal(expected.Length, flat.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(
                    Math.Abs(expected[i] - flat[i]) <= Tolerance,
                    $"{name}[{i / Math.Max(actual.Columns, 1)},{i % Math.Max(actual.Columns, 1)}]: " +
                    $"{expected[i]} differs from {flat[i]}");
            }

            // The indexer and the row window read the same storage the flat copy does.
            for (int row = 0; row < actual.Rows; row++)
            {
                ReadOnlySpan<double> window = actual.Row(row);
                for (int column = 0; column < actual.Columns; column++)
                {
                    Assert.Equal(flat[(row * actual.Columns) + column], actual[row, column]);
                    Assert.Equal(flat[(row * actual.Columns) + column], window[column]);
                }
            }

            replayed++;
        }

        Assert.True(replayed >= 19, $"only {replayed} cases replayed");
    }

    /// <summary>The default is <c>Fuzz.Ratio</c>, which is the reference's and not <c>Extract</c>'s.</summary>
    [Fact]
    public void The_default_scorer_is_Ratio_rather_than_WRatio()
    {
        string[] queries = ["fuzzy wuzzy was a bear"];
        string[] choices = ["wuzzy fuzzy was a bear"];

        ScoreMatrix byDefault = Process.Cdist(queries, choices);

        Assert.Equal(Fuzz.Ratio(queries[0], choices[0]), byDefault[0, 0], Tolerance);
        Assert.NotEqual(Fuzz.WRatio(queries[0], choices[0]), byDefault[0, 0], Tolerance);
    }

    private static string[] Strings(JsonElement array) =>
        [.. array.EnumerateArray().Select(v => v.GetString()!)];

    private static Func<string, string, double> Scorer(string name, string element)
    {
        TextElement unit = element == "codePoint" ? TextElement.CodePoint : TextElement.Utf16Unit;

        return name switch
        {
            "partial_ratio" => (a, b) => Fuzz.PartialRatio(a, b, unit),
            "token_sort_ratio" => (a, b) => Fuzz.TokenSortRatio(a, b, unit),
            "token_set_ratio" => (a, b) => Fuzz.TokenSetRatio(a, b, unit),
            "partial_token_sort_ratio" => (a, b) => Fuzz.PartialTokenSortRatio(a, b, unit),
            "partial_token_set_ratio" => (a, b) => Fuzz.PartialTokenSetRatio(a, b, unit),
            "WRatio" => (a, b) => Fuzz.WRatio(a, b, unit),
            _ => (a, b) => Fuzz.Ratio(a, b, unit),
        };
    }
}
