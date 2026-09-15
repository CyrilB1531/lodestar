using System.Text.Json;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>
/// Replays <c>statsmodels.api.GLS(...).fit()</c> over the nine frozen cases of
/// <c>tests/oracles/stats_gls.json</c>.
/// </summary>
/// <remarks>
/// The error covariance is what varies: AR(1) at three correlations and two equicorrelated blocks
/// with unequal variances, which no diagonal reaches. Four cases carry a robust covariance on the
/// whitened rows (#771).
/// </remarks>
public sealed class GlsOracleTests
{
    private static readonly JsonDocument Corpus = OracleLoader.Load("stats_gls.json");

    private static IReadOnlyList<JsonElement> Cases => LinearOracle.Cases(Corpus);

    public static TheoryData<int> Indices() => LinearOracle.Indices(Corpus);

    private static OlsSummary Fit(JsonElement frozen) => GeneralizedLeastSquares.Fit(
        LinearOracle.Doubles(frozen, "design"),
        LinearOracle.Doubles(frozen, "response"),
        LinearOracle.Doubles(frozen, "covariance"),
        frozen.GetProperty("featureCount").GetInt32(),
        LinearOracle.Options(frozen));

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
    public void The_weighted_whole_model_statistics_match_the_reference(int index)
    {
        JsonElement frozen = Cases[index];

        LinearOracle.AssertWholeModel(frozen, Fit(frozen));
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_variance_inflation_factors_are_those_of_the_design_as_given(int index)
    {
        JsonElement frozen = Cases[index];

        LinearOracle.AssertList(frozen, "varianceInflationFactors", Fit(frozen).VarianceInflationFactors);
    }
}
