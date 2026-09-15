using System.Text.Json;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>
/// Replays <c>statsmodels.api.OLS(...).fit()</c> over the eighteen frozen cases of
/// <c>tests/oracles/stats_ols.json</c>.
/// </summary>
/// <remarks>
/// Each case is chosen for something an inference table can get wrong rather than for
/// something a solve can: a fitted intercept and none, a 99% level, a near-collinear pair
/// whose VIF reaches 6e4, and one residual degree of freedom. Six carry an HC covariance and replay
/// the distribution switch as much as the arithmetic (#686); six more carry HAC or cluster (#775).
/// </remarks>
public sealed class OlsOracleTests
{
    private static readonly JsonDocument Corpus = OracleLoader.Load("stats_ols.json");

    private static IReadOnlyList<JsonElement> Cases => LinearOracle.Cases(Corpus);

    public static TheoryData<int> Indices() => LinearOracle.Indices(Corpus);

    private static OlsSummary Fit(JsonElement frozen) => LinearOracle.Groups(frozen) is { } groups
        ? OrdinaryLeastSquares.Fit(
            LinearOracle.Doubles(frozen, "design"),
            LinearOracle.Doubles(frozen, "response"),
            groups,
            frozen.GetProperty("featureCount").GetInt32(),
            LinearOracle.Options(frozen))
        : OrdinaryLeastSquares.Fit(
            LinearOracle.Doubles(frozen, "design"),
            LinearOracle.Doubles(frozen, "response"),
            frozen.GetProperty("featureCount").GetInt32(),
            LinearOracle.Options(frozen));

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_summary_echoes_the_estimator_it_was_asked_for(int index)
    {
        // Which distribution the p-values came from follows from this and nothing else
        // on the summary, so a reader who has only the summary needs it to be right.
        JsonElement frozen = Cases[index];

        Assert.Equal(LinearOracle.Covariance(frozen), Fit(frozen).CovarianceType);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_estimates_and_their_errors_match_the_reference(int index)
    {
        JsonElement frozen = Cases[index];

        LinearOracle.AssertEstimates(frozen, Fit(frozen));
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_p_values_and_intervals_match_the_reference(int index)
    {
        JsonElement frozen = Cases[index];

        LinearOracle.AssertTests(frozen, Fit(frozen));
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_whole_model_statistics_match_the_reference(int index)
    {
        JsonElement frozen = Cases[index];

        LinearOracle.AssertWholeModel(frozen, Fit(frozen));
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_variance_inflation_factors_match_the_reference(int index)
    {
        JsonElement frozen = Cases[index];

        LinearOracle.AssertList(frozen, "varianceInflationFactors", Fit(frozen).VarianceInflationFactors);
    }
}
