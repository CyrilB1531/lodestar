using System.Text.Json;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>
/// Replays <c>statsmodels.api.WLS(...).fit()</c> over the twelve frozen cases of
/// <c>tests/oracles/stats_wls.json</c>.
/// </summary>
/// <remarks>
/// The weights are what vary: uneven, all one, all four, one of them zero, and inversely
/// proportional to the level. Five cases carry a robust covariance, where the model's own
/// constant stays out of the Wald test that OLS on the scaled rows would put it in (#768).
/// </remarks>
public sealed class WlsOracleTests
{
    private static readonly JsonDocument Corpus = OracleLoader.Load("stats_wls.json");

    private static IReadOnlyList<JsonElement> Cases => LinearOracle.Cases(Corpus);

    public static TheoryData<int> Indices() => LinearOracle.Indices(Corpus);

    private static OlsSummary Fit(JsonElement frozen) => WeightedLeastSquares.Fit(
        LinearOracle.Doubles(frozen, "design"),
        LinearOracle.Doubles(frozen, "response"),
        LinearOracle.Doubles(frozen, "weights"),
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
