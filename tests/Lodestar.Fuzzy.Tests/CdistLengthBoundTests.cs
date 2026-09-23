using Xunit;

namespace Lodestar.Fuzzy.Tests;

// CA5394/S2245 (insecure randomness): a seeded Random draws a reproducible corpus, so a failing
// pair can be reached again from the seed alone. No security use.
#pragma warning disable CA5394, S2245

/// <summary>
/// The length bound <see cref="Process.Cdist"/> rejects pairs with: that it changes no answer it
/// is allowed to take, and that it is not taken where it would give a wrong one (#1134).
/// </summary>
public sealed class CdistLengthBoundTests
{
    private const double Tolerance = 1e-9;

    private static readonly double[] Cutoffs = [0.0, 1.0, 40.0, 60.0, 80.0, 90.0, 99.0, 100.0];

    /// <summary>The bound answers what scoring every pair answers, at every cutoff and either way round.</summary>
    /// <remarks>
    /// The second call passes a lambda over the same scorer, which is not the delegate the bound
    /// is gated on — so one side of the comparison rejects on length and the other scans, and the
    /// corpus is drawn with lengths from 1 to 40 to give the bound something to reject.
    /// </remarks>
    [Fact]
    public void The_bound_agrees_with_scoring_every_pair()
    {
        string[] queries = Corpus(48, 1134);
        string[] choices = Corpus(48, 1135);

        foreach (double cutoff in Cutoffs)
        {
            double[] bounded = Process.Cdist(queries, choices, scorer: null, cutoff).ToArray();
            double[] scored = Process.Cdist(queries, choices, (a, b) => Fuzz.Ratio(a, b), cutoff).ToArray();

            Assert.Equal(scored, bounded);
        }
    }

    /// <summary>A lambda that merely calls the default scorer is not the default scorer.</summary>
    /// <remarks>
    /// Telling them apart is impossible, so the gate errs towards scoring the pair: the wrong
    /// answer a bound taken on an unknown delegate would give is the failure that matters, and a
    /// missed rejection is only slower.
    /// </remarks>
    [Fact]
    public void A_lambda_over_Ratio_is_scored_rather_than_bounded()
    {
        string[] queries = ["cat"];
        string[] choices = ["the cat sat on the mat"];

        double wrapped = Process.Cdist(queries, choices, (a, b) => Fuzz.Ratio(a, b), 20.0)[0, 0];

        Assert.Equal(Fuzz.Ratio(queries[0], choices[0]), wrapped, Tolerance);
    }

    /// <summary>
    /// A scorer that ignores length keeps the score its lengths forbid: the bound is exact for
    /// the Indel ratio and false for every other member of <see cref="Fuzz"/>.
    /// </summary>
    [Theory]
    [InlineData("abc", "xxxabcxxx")]
    [InlineData("cat", "the cat sat on the mat")]
    public void A_length_blind_scorer_is_not_bounded(string query, string choice)
    {
        const double Cutoff = 90.0;
        string[] queries = [query];
        string[] choices = [choice];

        // 50 and 24 for these two, so both are pairs a bound taken on any delegate would zero.
        double ceiling = Ceiling(query, choice);
        Assert.True(ceiling < Cutoff, $"the pair must be one the bound rejects, not {ceiling}");
        Assert.Equal(ceiling, Process.Cdist(queries, choices)[0, 0], Tolerance);

        Assert.Equal(100.0, Process.Cdist(queries, choices, Fuzz.PartialRatio, Cutoff)[0, 0], Tolerance);
        Assert.Equal(90.0, Process.Cdist(queries, choices, Fuzz.WRatio, Cutoff)[0, 0], Tolerance);
    }

    /// <summary>A cutoff of zero rejects nothing, since no score can fall below it.</summary>
    [Fact]
    public void A_cutoff_of_zero_scores_every_pair()
    {
        string[] queries = ["a", "abcdefghijklmnop"];
        string[] choices = ["abcdefghijklmnop", "a"];

        ScoreMatrix matrix = Process.Cdist(queries, choices);

        Assert.Equal(Fuzz.Ratio(queries[0], choices[0]), matrix[0, 0], Tolerance);
        Assert.Equal(100.0, matrix[0, 1], Tolerance);
        Assert.Equal(100.0, matrix[1, 0], Tolerance);
    }

    /// <summary>Two empty strings are a total of zero, which the ceiling cannot divide by.</summary>
    [Fact]
    public void Two_empty_strings_score_100_under_any_cutoff()
    {
        string[] empty = [string.Empty];

        Assert.Equal(100.0, Process.Cdist(empty, empty, scorer: null, 100.0)[0, 0], Tolerance);
    }

    private static double Ceiling(string a, string b) =>
        100.0 * (1.0 - (Math.Abs(a.Length - b.Length) / (double)(a.Length + b.Length)));

    private static string[] Corpus(int count, int seed)
    {
        var random = new Random(seed);
        var drawn = new string[count];
        for (int i = 0; i < count; i++)
        {
            var text = new char[random.Next(1, 41)];
            for (int c = 0; c < text.Length; c++)
            {
                text[c] = (char)('a' + random.Next(6));
            }

            drawn[i] = new string(text);
        }

        return drawn;
    }
}
