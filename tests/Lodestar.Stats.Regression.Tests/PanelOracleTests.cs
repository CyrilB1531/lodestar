using System.Text.Json;
using Lodestar.Stats.Regression.Panel;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>
/// Replays <c>linearmodels</c>' <c>PanelOLS</c>, <c>BetweenOLS</c>, <c>FirstDifferenceOLS</c> and <c>RandomEffects</c> over
/// <c>tests/oracles/stats_panel.json</c> (#1156).
/// </summary>
/// <remarks>
/// A balanced, an unbalanced and a long narrow panel under every effect, covariance, cluster and kernel each estimator
/// accepts. Values at <c>1e-9</c> relative; p-values also within an absolute <c>1e-15</c>, since the reference computes
/// <c>2 − 2·cdf</c> and returns zero where this returns the tail.
/// </remarks>
public sealed class PanelOracleTests
{
    private const double Relative = 1e-9;
    private const double PFloor = 1e-15;

    private static readonly JsonDocument Corpus = OracleLoader.Load("stats_panel.json");

    private static IReadOnlyList<JsonElement> Cases => [.. Corpus.RootElement.GetProperty("cases").EnumerateArray()];

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
    public void The_table_matches_the_reference(int index)
    {
        JsonElement frozen = Cases[index];
        string name = frozen.GetProperty("name").GetString()!;
        JsonElement expected = frozen.GetProperty("expected");
        PanelSummary summary = Fit(frozen);

        CloseAll(expected, "coefficients", summary.Coefficients, name);
        CloseAll(expected, "standardErrors", summary.StandardErrors, name);
        CloseAll(expected, "tStatistics", summary.TStatistics, name);
        Probabilities(expected, "pValues", summary.PValues, name);
        CloseAll(expected, "confidenceLower", summary.ConfidenceLower, name);
        CloseAll(expected, "confidenceUpper", summary.ConfidenceUpper, name);
        Assert.Equal(expected.GetProperty("residualDf").GetInt32(), summary.ResidualDegreesOfFreedom);
        Assert.Equal(expected.GetProperty("observationCount").GetInt32(), summary.ObservationCount);
        Assert.Equal(expected.GetProperty("entityCount").GetInt32(), summary.EntityCount);
        Assert.Equal(expected.GetProperty("periodCount").GetInt32(), summary.PeriodCount);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_fit_and_variance_components_match_the_reference(int index)
    {
        JsonElement frozen = Cases[index];
        string name = frozen.GetProperty("name").GetString()!;
        JsonElement expected = frozen.GetProperty("expected");
        PanelSummary summary = Fit(frozen);

        Nearly(expected.GetProperty("rSquared").GetDouble(), summary.RSquared, $"{name}: R²");
        Nearly(expected.GetProperty("rSquaredWithin").GetDouble(), summary.RSquaredWithin, $"{name}: within R²");
        Nearly(expected.GetProperty("rSquaredBetween").GetDouble(), summary.RSquaredBetween, $"{name}: between R²");
        Nearly(expected.GetProperty("rSquaredOverall").GetDouble(), summary.RSquaredOverall, $"{name}: overall R²");
        Optional(expected, "residualVariance", summary.ResidualVariance, name);
        Optional(expected, "effectsVariance", summary.EffectsVariance, name);
        Optional(expected, "rho", summary.Rho, name);
        if (expected.GetProperty("theta").ValueKind == JsonValueKind.Null)
        {
            Assert.Null(summary.Theta);
        }
        else
        {
            CloseAll(expected, "theta", summary.Theta!, name);
        }
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_model_tests_match_the_reference(int index)
    {
        JsonElement frozen = Cases[index];
        string name = frozen.GetProperty("name").GetString()!;
        JsonElement expected = frozen.GetProperty("expected");
        PanelSummary summary = Fit(frozen);

        Test(expected.GetProperty("modelTest"), summary.ModelTest, $"{name}: model F");
        Test(expected.GetProperty("robustModelTest"), summary.RobustModelTest, $"{name}: robust model test");
        Test(expected.GetProperty("poolabilityTest"), summary.PoolabilityTest, $"{name}: poolability");
    }

    private static PanelSummary Fit(JsonElement frozen)
    {
        JsonElement data = Corpus.RootElement.GetProperty("problems").GetProperty(frozen.GetProperty("data").GetString()!);
        var design = new PanelDesign(
            Doubles(data, "response"),
            Doubles(data, "exogenous"),
            data.GetProperty("exogenousCount").GetInt32(),
            Ints(data, "entities"),
            Ints(data, "periods"));
        JsonElement settings = frozen.GetProperty("options");
        PanelOptions options = Options(settings);
        bool custom = settings.TryGetProperty("customClusters", out JsonElement flag) && flag.GetBoolean();
        int[] clusters = Ints(data, "clusters");

        return settings.GetProperty("estimator").GetString() switch
        {
            "fixedEffects" => custom ? PanelRegression.FixedEffects(design, clusters, options) : PanelRegression.FixedEffects(design, options),
            "between" => custom ? PanelRegression.Between(design, clusters, options) : PanelRegression.Between(design, options),
            "firstDifference" => custom ? PanelRegression.FirstDifference(design, clusters, options) : PanelRegression.FirstDifference(design, options),
            _ => custom ? PanelRegression.RandomEffects(design, clusters, options) : PanelRegression.RandomEffects(design, options),
        };
    }

    private static PanelOptions Options(JsonElement frozen)
    {
        var options = new PanelOptions();
        foreach (JsonProperty property in frozen.EnumerateObject())
        {
            JsonElement value = property.Value;
            options = property.Name switch
            {
                "estimator" or "customClusters" => options,
                "covarianceType" => options with { CovarianceType = Covariance(value.GetString()!) },
                "debiased" => options with { Debiased = value.GetBoolean() },
                "kernel" => options with { Kernel = Kernel(value.GetString()!) },
                "bandwidth" => options with { Bandwidth = value.GetInt32() },
                "withIntercept" => options with { WithIntercept = value.GetBoolean() },
                "entityEffects" => options with { EntityEffects = value.GetBoolean() },
                "timeEffects" => options with { TimeEffects = value.GetBoolean() },
                "clusterEntity" => options with { ClusterEntity = value.GetBoolean() },
                "clusterTime" => options with { ClusterTime = value.GetBoolean() },
                _ => throw new InvalidOperationException($"Unknown option {property.Name}."),
            };
        }

        return options;
    }

    private static PanelCovarianceType Covariance(string name) => name switch
    {
        "unadjusted" => PanelCovarianceType.Unadjusted,
        "robust" => PanelCovarianceType.Robust,
        "kernel" => PanelCovarianceType.Kernel,
        _ => PanelCovarianceType.Clustered,
    };

    private static KernelType Kernel(string name) => name switch
    {
        "bartlett" => KernelType.Bartlett,
        "parzen" => KernelType.Parzen,
        _ => KernelType.QuadraticSpectral,
    };

    private static void Test(JsonElement expected, WaldTest? actual, string what)
    {
        if (expected.ValueKind == JsonValueKind.Null)
        {
            Assert.Null(actual);
            return;
        }

        Assert.NotNull(actual);
        Close(expected.GetProperty("statistic").GetDouble(), actual.Statistic, what);
        Probability(expected.GetProperty("pValue").GetDouble(), actual.PValue, $"{what} p");
        Assert.Equal(expected.GetProperty("df").GetInt32(), actual.DegreesOfFreedom);
        JsonElement denominator = expected.GetProperty("denominatorDf");
        Assert.Equal(denominator.ValueKind == JsonValueKind.Null ? null : denominator.GetInt32(), actual.DenominatorDegreesOfFreedom);
    }

    private static void Optional(JsonElement expected, string key, double? actual, string name)
    {
        JsonElement value = expected.GetProperty(key);
        if (value.ValueKind == JsonValueKind.Null)
        {
            Assert.Null(actual);
            return;
        }

        Assert.NotNull(actual);
        Nearly(value.GetDouble(), actual.Value, $"{name}: {key}");
    }

    private static void CloseAll(JsonElement expected, string key, IReadOnlyList<double> actual, string name)
    {
        double[] values = Doubles(expected, key);
        Assert.Equal(values.Length, actual.Count);
        for (int i = 0; i < values.Length; i++)
        {
            Close(values[i], actual[i], $"{name}: {key}[{i}]");
        }
    }

    private static void Probabilities(JsonElement expected, string key, IReadOnlyList<double> actual, string name)
    {
        double[] values = Doubles(expected, key);
        for (int i = 0; i < values.Length; i++)
        {
            Probability(values[i], actual[i], $"{name}: {key}[{i}]");
        }
    }

    private static void Close(double expected, double actual, string what) =>
        Assert.True(
            Math.Abs(actual - expected) <= Relative * Math.Abs(expected),
            $"{what}: {actual:R} against {expected:R}");

    /// <summary>Relative, with the same absolute floor as a p-value: an R² or a variance can be an exact zero there.</summary>
    private static void Nearly(double expected, double actual, string what) => Probability(expected, actual, what);

    private static void Probability(double expected, double actual, string what) =>
        Assert.True(
            Math.Abs(actual - expected) <= (Relative * Math.Abs(expected)) + PFloor,
            $"{what}: {actual:R} against {expected:R}");

    private static double[] Doubles(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetDouble())];

    private static int[] Ints(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetInt32())];
}
