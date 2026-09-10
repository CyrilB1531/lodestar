using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>
/// The published tails and quantiles against <c>scipy.stats</c>, over the range a caller reaching
/// for them directly would use.
/// </summary>
/// <remarks>
/// The corpus decision 0081 asked for before this layer could be published: it reaches
/// 3.1e-24, where an <em>absolute</em> 1e-9 would accept an implementation returning zero.
/// Probabilities are therefore compared relatively, as every p-value here is.
/// </remarks>
public sealed class DistributionsOracleTests
{
    private const double Relative = 1e-9;

    private static readonly JsonDocument Corpus = StatsCorpus.Load("stats_distributions.json");

    private static IReadOnlyList<JsonElement> Cases =>
        [.. Corpus.RootElement.GetProperty("cases").EnumerateArray()];

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
    public void Each_published_tail_matches_scipy(int index)
    {
        JsonElement frozen = Cases[index];
        JsonElement args = frozen.GetProperty("args");
        double expected = frozen.GetProperty("value").GetDouble();
        double x = args.GetProperty("x").GetDouble();

        switch (frozen.GetProperty("call").GetString())
        {
            case "t.sf":
                AssertRelative(expected, Distributions.StudentSf(x, Arg(args, "df")));
                break;
            case "t.ppf":
                Assert.Equal(expected, Distributions.StudentQuantile(x, Arg(args, "df")), Relative);
                break;
            case "chi2.sf":
                AssertRelative(expected, Distributions.ChiSquaredSf(x, Arg(args, "df")));
                break;
            case "norm.ppf":
                Assert.Equal(expected, Distributions.NormalQuantile(x), Relative);
                break;
            default:
                AssertRelative(
                    expected, Distributions.FisherSf(x, Arg(args, "dfn"), Arg(args, "dfd")));
                break;
        }
    }

    private static double Arg(JsonElement args, string name) => args.GetProperty(name).GetDouble();

    /// <summary>
    /// Relative, because the corpus reaches 1e-24: an absolute tolerance there asserts that
    /// a number came back, not that it was the right one.
    /// </summary>
    private static void AssertRelative(double expected, double actual)
    {
        double gap = Math.Abs(actual - expected) / Math.Abs(expected);

        Assert.True(
            gap <= Relative,
            $"expected {expected:R}, got {actual:R} — relative gap {gap:R}");
    }
}
