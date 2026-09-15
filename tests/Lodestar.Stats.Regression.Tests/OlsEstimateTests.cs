using System.Text.Json;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>
/// <see cref="OrdinaryLeastSquares.Estimate"/> against the frozen OLS corpus and against
/// <see cref="OrdinaryLeastSquares.Fit"/>, on the numbers the estimate keeps.
/// </summary>
public sealed class OlsEstimateTests
{
    private const double Relative = 1e-9;

    private static readonly JsonDocument Corpus = OracleLoader.Load("stats_ols.json");

    private static double[] Doubles(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetDouble())];

    [Fact]
    public void Every_corpus_case_matches_statsmodels_on_the_estimate()
    {
        int replayed = 0;
        foreach (JsonElement frozen in Corpus.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = frozen.GetProperty("name").GetString()!;
            OlsEstimate estimate = OrdinaryLeastSquares.Estimate(
                Doubles(frozen, "design"),
                Doubles(frozen, "response"),
                frozen.GetProperty("featureCount").GetInt32(),
                frozen.GetProperty("withIntercept").GetBoolean());

            Close(Doubles(frozen, "coefficients"), estimate.Coefficients, $"{name} coefficients");

            // The robust cases change the standard errors, not the estimate or its residuals.
            if (frozen.GetProperty("covarianceType").GetString() == "nonrobust")
            {
                Close(Doubles(frozen, "standardErrors"), estimate.StandardErrors, $"{name} standard errors");
                Close(Doubles(frozen, "tStatistics"), estimate.TStatistics, $"{name} t statistics");
            }

            int degrees = frozen.GetProperty("residualDegreesOfFreedom").GetInt32();
            double rse = frozen.GetProperty("residualStandardError").GetDouble();
            Assert.Equal(degrees, estimate.ResidualDegreesOfFreedom);
            Close([rse * rse * degrees], [estimate.ResidualSumOfSquares], $"{name} residual sum of squares");
            Assert.Equal(frozen.GetProperty("withIntercept").GetBoolean(), estimate.HasIntercept);
            replayed++;
        }

        Assert.Equal(Corpus.RootElement.GetProperty("cases").GetArrayLength(), replayed);
    }

    // SonarLint S2245, CA5394: a seeded Random builds reproducible designs; no security use.
#pragma warning disable S2245, CA5394
    [Theory]
    [InlineData(40, 1, true)]
    [InlineData(40, 1, false)]
    [InlineData(120, 5, true)]
    [InlineData(120, 5, false)]
    [InlineData(12, 9, true)]
    public void The_estimate_agrees_with_the_whole_fit(int rows, int features, bool withIntercept)
    {
        var random = new Random((rows * 31) + features);
        var design = new double[rows * features];
        var response = new double[rows];
        for (int row = 0; row < rows; row++)
        {
            double signal = 0.0;
            for (int feature = 0; feature < features; feature++)
            {
                double value = (random.NextDouble() * 10.0) - 5.0;
                design[(row * features) + feature] = value;
                signal += value * (feature + 1);
            }

            response[row] = signal + random.NextDouble();
        }

        OlsSummary whole = OrdinaryLeastSquares.Fit(
            design, response, features, new OlsOptions { WithIntercept = withIntercept });
        OlsEstimate estimate = OrdinaryLeastSquares.Estimate(design, response, features, withIntercept);

        Close([.. whole.Coefficients], estimate.Coefficients, "coefficients");
        Close([.. whole.StandardErrors], estimate.StandardErrors, "standard errors");
        Close([.. whole.TStatistics], estimate.TStatistics, "t statistics");
        Assert.Equal(whole.ResidualDegreesOfFreedom, estimate.ResidualDegreesOfFreedom);
        double ssr = whole.ResidualStandardError * whole.ResidualStandardError * whole.ResidualDegreesOfFreedom;
        Close([ssr], [estimate.ResidualSumOfSquares], "residual sum of squares");
    }
#pragma warning restore S2245, CA5394

    [Fact]
    public void No_residual_degree_of_freedom_is_refused_as_the_whole_fit_refuses_it()
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => OrdinaryLeastSquares.Estimate([1.0, 2.0], [3.0, 4.0], featureCount: 1));

        Assert.Equal("design", refusal.ParamName);
    }

    [Fact]
    public void A_feature_count_below_one_is_refused() =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => OrdinaryLeastSquares.Estimate([1.0, 2.0, 3.0], [1.0, 2.0, 3.0], featureCount: 0));

    private static void Close(double[] expected, IReadOnlyList<double> actual, string name)
    {
        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            double scale = Math.Max(1.0, Math.Abs(expected[i]));
            Assert.True(
                Math.Abs(expected[i] - actual[i]) <= Relative * scale,
                $"{name}[{i}]: {actual[i]} against {expected[i]}.");
        }
    }
}
