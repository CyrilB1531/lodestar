using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>
/// pdf, cdf, sf, quantile and inverse sf of the normal, t, chi-squared and F laws against
/// <c>scipy.stats</c>, over the body and both far tails (#1158).
/// </summary>
/// <remarks>
/// Compared relatively, as the older distributions corpus is: the grid reaches probabilities of
/// 1e-300, where an absolute 1e-9 would accept an implementation returning zero.
/// </remarks>
public sealed class DistributionFunctionsOracleTests
{
    private const double Relative = 1e-9;

    private static readonly JsonDocument Corpus = StatsCorpus.Load("stats_distribution_functions.json");

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
    public void Each_function_matches_scipy(int index)
    {
        JsonElement frozen = Cases[index];
        JsonElement args = frozen.GetProperty("args");
        double expected = frozen.GetProperty("value").GetDouble();
        double x = args.GetProperty("x").GetDouble();
        string call = $"{frozen.GetProperty("law").GetString()}.{frozen.GetProperty("function").GetString()}";

        double actual = call switch
        {
            "norm.pdf" => Distributions.NormalPdf(x),
            "norm.cdf" => Distributions.NormalCdf(x),
            "norm.sf" => Distributions.NormalSf(x),
            "norm.ppf" => Distributions.NormalQuantile(x),
            "norm.isf" => Distributions.NormalIsf(x),
            "t.pdf" => Distributions.StudentPdf(x, Arg(args, "df")),
            "t.cdf" => Distributions.StudentCdf(x, Arg(args, "df")),
            "t.sf" => Distributions.StudentSf(x, Arg(args, "df")),
            "t.ppf" => Distributions.StudentQuantile(x, Arg(args, "df")),
            "t.isf" => Distributions.StudentIsf(x, Arg(args, "df")),
            "chi2.pdf" => Distributions.ChiSquaredPdf(x, Arg(args, "df")),
            "chi2.cdf" => Distributions.ChiSquaredCdf(x, Arg(args, "df")),
            "chi2.sf" => Distributions.ChiSquaredSf(x, Arg(args, "df")),
            "chi2.ppf" => Distributions.ChiSquaredQuantile(x, Arg(args, "df")),
            "chi2.isf" => Distributions.ChiSquaredIsf(x, Arg(args, "df")),
            "f.pdf" => Distributions.FisherPdf(x, Arg(args, "dfn"), Arg(args, "dfd")),
            "f.cdf" => Distributions.FisherCdf(x, Arg(args, "dfn"), Arg(args, "dfd")),
            "f.sf" => Distributions.FisherSf(x, Arg(args, "dfn"), Arg(args, "dfd")),
            "f.ppf" => Distributions.FisherQuantile(x, Arg(args, "dfn"), Arg(args, "dfd")),
            "f.isf" => Distributions.FisherIsf(x, Arg(args, "dfn"), Arg(args, "dfd")),
            _ => throw new InvalidOperationException($"No function for {call}."),
        };

        AssertRelative(expected, actual, call, args);
    }

    private static double Arg(JsonElement args, string name) => args.GetProperty(name).GetDouble();

    private static void AssertRelative(double expected, double actual, string call, JsonElement args)
    {
        // S1244: a value scipy answers as exactly zero is compared as zero; every other is relative.
#pragma warning disable S1244
        double error = expected == 0.0 ? Math.Abs(actual) : Math.Abs(actual - expected) / Math.Abs(expected);
#pragma warning restore S1244
        Assert.True(error <= Relative,
            $"{call}({args.GetRawText().Replace("\n", "", StringComparison.Ordinal).Replace(" ", "", StringComparison.Ordinal)}): expected {expected:R}, got {actual:R}, relative error {error:E2}");
    }
}
