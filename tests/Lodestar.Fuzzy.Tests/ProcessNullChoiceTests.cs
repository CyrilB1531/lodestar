using Lodestar.Fuzzy;
using Xunit;

namespace Lodestar.Fuzzy.Tests;

/// <summary>
/// A null choice or query, as rapidfuzz 3.14.6 treats <c>None</c>: <c>extract</c> and <c>extractOne</c> skip the choice
/// with its index kept and answer the query with nothing, <c>cdist</c> scores either 0. Each threw
/// <c>ArgumentNullException</c>, from the scorer or the query guard (#1233).
/// </summary>
public sealed class ProcessNullChoiceTests
{
    private const double Tolerance = 1e-9;

    private static readonly string?[] Choices = ["apple", null, "apples", ""];

    /// <summary>Refuses a null argument, so a test fails if a null choice ever reaches the scorer.</summary>
    private static double RefusingNull(Func<string, string, double> scorer, string a, string b) =>
        a is null || b is null ? throw new InvalidOperationException("A null reached the scorer.") : scorer(a, b);

    private static double RatioRefusingNull(string a, string b) => RefusingNull(Fuzz.Ratio, a, b);

    private static void AssertClose(double[] expected, double[] actual)
    {
        Assert.Equal(expected.Length, actual.Length);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], actual[i], Tolerance);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData(2)]
    public void Extract_skips_a_null_choice_keeping_its_index(int? limit)
    {
        // process.extract("apple", ["apple", None, "apples", ""], limit=None) == [(apple, 100, 0), (apples, 90.90…, 2), ("", 0, 3)].
        IReadOnlyList<ExtractResult> hits = Process.Extract("apple", Choices, limit: limit);

        Assert.Equal(limit ?? 3, hits.Count);
        Assert.Equal(("apple", 0), (hits[0].Choice, hits[0].Index));
        Assert.Equal(100.0, hits[0].Score, Tolerance);
        Assert.Equal(("apples", 2), (hits[1].Choice, hits[1].Index));
        Assert.Equal(90.9090909090909, hits[1].Score, Tolerance);
        if (limit is null)
        {
            Assert.Equal(("", 0.0, 3), (hits[2].Choice, hits[2].Score, hits[2].Index));
        }
    }

    [Fact]
    public void Extract_never_hands_a_null_choice_to_the_scorer()
    {
        Assert.Equal([0, 2, 3], Process.Extract("apple", Choices, RatioRefusingNull, limit: null).Select(h => h.Index).Order());
        Assert.Equal([0, 2], Process.Extract("apple", Choices, RatioRefusingNull, limit: 2).Select(h => h.Index));
        Assert.Empty(Process.Extract("apple", [null, null], RatioRefusingNull));
    }

    [Fact]
    public void A_null_query_matches_nothing()
    {
        // process.extract(None, …) == [] even under limit=-1, which it never reads; process.extractOne(None, …) is None.
        Assert.Empty(Process.Extract(null, Choices, RatioRefusingNull));
        Assert.Empty(Process.Extract(null, Choices, RatioRefusingNull, limit: -1));
        Assert.Null(Process.ExtractOne(null, Choices, RatioRefusingNull));

        // Before the choices too: process.extract(None, None) == [] and process.extractOne(None, None) is None.
        Assert.Empty(Process.Extract(null, null!));
        Assert.Null(Process.ExtractOne(null, null!));
    }

    [Fact]
    public void ExtractOne_skips_a_null_choice_keeping_its_index()
    {
        // process.extractOne("apple", [None, "aple", None], scorer=fuzz.ratio) == ("aple", 88.88…, 1); over [None] it is None.
        ExtractResult? found = Process.ExtractOne("apple", [null, "aple", null], RatioRefusingNull);

        Assert.NotNull(found);
        Assert.Equal(("aple", 1), (found.Value.Choice, found.Value.Index));
        Assert.Equal(88.88888888888889, found.Value.Score, Tolerance);
        Assert.Null(Process.ExtractOne("apple", [null], RatioRefusingNull));
    }

    [Theory]
    [InlineData(0.0, 90.9090909090909)]
    [InlineData(95.0, 0.0)]
    public void Cdist_scores_a_null_choice_zero(double scoreCutoff, double apples)
    {
        // process.cdist(["apple", "x"], ["apple", None, "apples", ""], dtype="float64"): the None column is 0 in every row.
        double[] expected = [100.0, 0.0, apples, 0.0, 0.0, 0.0, 0.0, 0.0];

        AssertClose(expected, Process.Cdist(["apple", "x"], Choices, scoreCutoff: scoreCutoff).ToArray());
        AssertClose(expected, Process.Cdist(["apple", "x"], Choices, RatioRefusingNull, scoreCutoff).ToArray());
    }

    [Fact]
    public void Cdist_scores_a_null_query_zero()
    {
        // process.cdist([None, "apple"], ["apple", None], scorer=fuzz.WRatio) == [[0, 0], [100, 0]]; ratio reads garbage there.
        ScoreMatrix scores = Process.Cdist([null, "apple"], ["apple", null], (a, b) => RefusingNull(Fuzz.WRatio, a, b));

        AssertClose([0.0, 0.0, 100.0, 0.0], scores.ToArray());
    }

    [Fact]
    public void Cdist_scores_a_null_query_zero_on_the_length_bounded_default()
    {
        // The default scorer under a cutoff rejects pairs on their lengths first; a null query must not reach that bound.
        ScoreMatrix scores = Process.Cdist([null, "apple"], ["apple", "a"], scoreCutoff: 50.0);

        AssertClose([0.0, 0.0, 100.0, 0.0], scores.ToArray());
    }
}
