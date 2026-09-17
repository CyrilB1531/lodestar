using System.Text.Json;
using Lodestar.Fuzzy;
using Xunit;

namespace Lodestar.Fuzzy.Tests;

public sealed class ProcessOracleTests
{
    private const double Tolerance = 1e-4;

    [Fact]
    public void Extract_matches_rapidfuzz()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "oracles", "process.json");
        using JsonDocument doc = JsonDocument.Parse(File.ReadAllText(path));
        JsonElement root = doc.RootElement;

        string[] choices = root.GetProperty("metadata").GetProperty("choices")
            .EnumerateArray().Select(e => e.GetString()!).ToArray();

        foreach (JsonElement c in root.GetProperty("cases").EnumerateArray())
        {
            string query = c.GetProperty("query").GetString()!;
            int limit = c.GetProperty("limit").GetInt32();
            double cutoff = c.GetProperty("cutoff").GetDouble();

            IReadOnlyList<ExtractResult> actual = Process.Extract(query, choices, limit: limit, scoreCutoff: cutoff);
            JsonElement expected = c.GetProperty("results");

            Assert.Equal(expected.GetArrayLength(), actual.Count);
            int r = 0;
            foreach (JsonElement e in expected.EnumerateArray())
            {
                Assert.Equal(e.GetProperty("choice").GetString(), actual[r].Choice);
                Assert.Equal(e.GetProperty("index").GetInt32(), actual[r].Index);
                Assert.True(Math.Abs(e.GetProperty("score").GetDouble() - actual[r].Score) < Tolerance,
                    $"case #{c.GetProperty("id").GetInt32()} rank {r}: score expected {e.GetProperty("score").GetDouble():R}, got {actual[r].Score:R}");
                r++;
            }

            ExtractResult? one = Process.ExtractOne(query, choices, scoreCutoff: cutoff);
            JsonElement expectedOne = c.GetProperty("extract_one");
            if (expectedOne.ValueKind == JsonValueKind.Null)
            {
                Assert.Null(one);
                continue;
            }

            Assert.NotNull(one);
            Assert.Equal(expectedOne.GetProperty("choice").GetString(), one.Value.Choice);
            Assert.Equal(expectedOne.GetProperty("index").GetInt32(), one.Value.Index);
            Assert.True(Math.Abs(expectedOne.GetProperty("score").GetDouble() - one.Value.Score) < Tolerance,
                $"case #{c.GetProperty("id").GetInt32()} extractOne: score expected {expectedOne.GetProperty("score").GetDouble():R}, got {one.Value.Score:R}");
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Extract_refuses_a_negative_limit_by_name(int limit)
    {
        var error = Assert.Throws<ArgumentOutOfRangeException>(() => Process.Extract("a", ["a", "b"], limit: limit));
        Assert.Equal("limit", error.ParamName);
    }

    [Fact]
    public void ExtractOne_returns_best()
    {
        string[] choices = ["apple", "banana", "orange"];
        ExtractResult? best = Process.ExtractOne("appel", choices);
        Assert.NotNull(best);
        Assert.Equal("apple", best.Value.Choice);
    }

    [Fact]
    public void ExtractOne_null_when_all_below_cutoff()
    {
        string[] choices = ["xxxx", "yyyy"];
        Assert.Null(Process.ExtractOne("abcd", choices, scoreCutoff: 50));
    }
}
