using Lodestar.Text.Distances;
using Lodestar.Text.Tests.Oracles;
using Xunit;

namespace Lodestar.Text.Tests.Distances;

public sealed class JaroOracleTests
{
    private static readonly OracleFile<SimilarityCase> Jaro_ =
        OracleCorpus.Load<SimilarityCase>("jaro.json");

    private static readonly OracleFile<SimilarityCase> Winkler =
        OracleCorpus.Load<SimilarityCase>("jaro_winkler.json");

    [Fact]
    public void Jaro_matches_reference()
    {
        // Standard Jaro; jellyfish concurs except on combining-mark/emoji quirks
        // (decision 0007). Real-name parity with jellyfish is anchored below.
        OracleAsserts.Approx(Jaro_.Cases,
            c => c.Similarity,
            c => Jaro.Similarity(c.A, c.B, TextElement.CodePoint),
            c => $"[#{c.Id} {c.Category}] {OracleAsserts.Escape(c.A)}/{OracleAsserts.Escape(c.B)}");
    }

    [Fact]
    public void JaroWinkler_matches_reference()
    {
        OracleAsserts.Approx(Winkler.Cases,
            c => c.Similarity,
            c => JaroWinkler.Similarity(c.A, c.B, element: TextElement.CodePoint),
            c => $"[#{c.Id} {c.Category}] {OracleAsserts.Escape(c.A)}/{OracleAsserts.Escape(c.B)}");
    }

    [Theory]
    [InlineData("", "", 0.0)]
    [InlineData("a", "", 0.0)]
    [InlineData("abc", "abc", 1.0)]
    [InlineData("MARTHA", "MARHTA", 0.9444444444444445)]
    [InlineData("DWAYNE", "DUANE", 0.8222222222222223)]
    public void Jaro_known_values(string a, string b, double expected)
    {
        Assert.Equal(expected, Jaro.Similarity(a, b), 12);
    }

    [Theory]
    [InlineData("MARTHA", "MARHTA", 0.9611111111111111)]
    [InlineData("DWAYNE", "DUANE", 0.84)]
    [InlineData("DIXON", "DICKSONX", 0.8133333333333332)]
    public void JaroWinkler_known_values(string a, string b, double expected)
    {
        Assert.Equal(expected, JaroWinkler.Similarity(a, b), 12);
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    [InlineData(double.NaN)]
    public void JaroWinkler_refuses_a_prefix_weight_outside_the_unit_interval(double weight)
    {
        // rapidfuzz raises ValueError outside [0, 1] for the same parameter (#1198).
        Assert.Throws<ArgumentOutOfRangeException>(() => JaroWinkler.Similarity("MARTHA", "MARHTA", weight));
        Assert.Throws<ArgumentOutOfRangeException>(() => JaroWinkler.Distance("MARTHA", "MARHTA", weight));
    }

    [Fact]
    public void JaroWinkler_caps_a_heavy_prefix_weight_at_one_as_rapidfuzz_does()
    {
        // At 0.5 four shared letters would reach 1.0277...; rapidfuzz answers 1.0.
        Assert.Equal(1.0, JaroWinkler.Similarity("MARTHA", "MARHTA", 0.5));
        Assert.Equal(0.0, JaroWinkler.Distance("MARTHA", "MARHTA", 0.5));
    }
}
