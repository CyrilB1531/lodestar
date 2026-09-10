using System.Text.Json;
using Lodestar.Abstractions;
using Lodestar.Text.Search;
using Xunit;

namespace Lodestar.Text.Tests.Search;

/// <summary>BM25 against <c>rank_bm25</c> 0.2.2's <c>BM25Okapi</c>, over its own corpora.</summary>
/// <remarks>
/// Scores are compared absolutely at 1e-12: they are sums of a handful of ratios, not a
/// far tail, so nothing here needs the relative comparison a p-value does.
/// </remarks>
public sealed class Bm25OracleTests
{
    private static readonly JsonDocument Corpus = LoadRaw("search_bm25.json");

    private static IReadOnlyList<JsonElement> Cases =>
        [.. Corpus.RootElement.GetProperty("cases").EnumerateArray()];

    private static JsonDocument LoadRaw(string name)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "oracles", name);
        return JsonDocument.Parse(File.ReadAllText(path));
    }

    public static TheoryData<int> Indices()
    {
        var data = new TheoryData<int>();
        for (int i = 0; i < Cases.Count; i++)
        {
            data.Add(i);
        }

        return data;
    }

    [Fact]
    public void Metadata_names_rank_bm25_and_its_variant()
    {
        JsonElement metadata = Corpus.RootElement.GetProperty("metadata");

        Assert.Equal("rank_bm25", metadata.GetProperty("library").GetString());
        Assert.Contains("Robertson", metadata.GetProperty("variant").GetString(), StringComparison.Ordinal);
        Assert.NotEmpty(Cases);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void Scores_match_rank_bm25(int index)
    {
        JsonElement frozen = Cases[index];
        string name = frozen.GetProperty("name").GetString()!;

        Bm25Index bm25 = Build(frozen);
        double[] scores = bm25.Score(Columns(frozen));
        double[] expected = [.. frozen.GetProperty("scores").EnumerateArray().Select(e => e.GetDouble())];

        Assert.Equal(expected.Length, scores.Length);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], scores[i], 1e-12);
        }

        Assert.Equal(
            frozen.GetProperty("averageDocumentLength").GetDouble(),
            bm25.AverageDocumentLength,
            1e-12);
        Assert.True(scores.Length > 0, name);
    }

    /// <summary>The defaults this package documents are the ones the reference used.</summary>
    [Theory]
    [MemberData(nameof(Indices))]
    public void The_frozen_parameters_are_this_packages_defaults(int index)
    {
        JsonElement frozen = Cases[index];
        var defaults = new Bm25Options();

        Assert.Equal(frozen.GetProperty("k1").GetDouble(), defaults.K1);
        Assert.Equal(frozen.GetProperty("b").GetDouble(), defaults.B);
        Assert.Equal(frozen.GetProperty("epsilon").GetDouble(), defaults.Epsilon);
    }

    /// <summary>Top-k is the same scores, ordered — not a second scoring path.</summary>
    [Theory]
    [MemberData(nameof(Indices))]
    public void Top_returns_the_same_scores_in_descending_order(int index)
    {
        JsonElement frozen = Cases[index];
        Bm25Index bm25 = Build(frozen);
        double[] scores = bm25.Score(Columns(frozen));

        IReadOnlyList<SearchHit> top = bm25.Top(Columns(frozen), scores.Length);

        Assert.Equal(scores.Length, top.Count);
        for (int i = 0; i < top.Count; i++)
        {
            Assert.Equal(scores[top[i].Document], top[i].Score, 1e-12);
            if (i > 0)
            {
                Assert.True(top[i - 1].Score >= top[i].Score);
            }
        }
    }

    private static Bm25Index Build(JsonElement frozen)
    {
        int[][] counts =
        [
            .. frozen.GetProperty("counts").EnumerateArray()
                .Select(row => row.EnumerateArray().Select(v => v.GetInt32()).ToArray())
        ];

        int columns = frozen.GetProperty("vocabulary").GetArrayLength();
        List<double> values = [];
        List<int> indices = [];
        int[] pointers = new int[counts.Length + 1];
        for (int row = 0; row < counts.Length; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                if (counts[row][column] != 0)
                {
                    values.Add(counts[row][column]);
                    indices.Add(column);
                }
            }

            pointers[row + 1] = values.Count;
        }

        return new Bm25Index(
            new CsrMatrix(counts.Length, columns, [.. values], [.. indices], pointers));
    }

    private static int[] Columns(JsonElement frozen) =>
        [.. frozen.GetProperty("queryColumns").EnumerateArray().Select(e => e.GetInt32())];
}
