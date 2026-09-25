using System.Text.Json;
using Lodestar.Stats.Regression.Instrumental;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>Replays <c>linearmodels</c>' <c>IV2SLS</c>, <c>IVLIML</c> and <c>IVGMM</c> over <c>tests/oracles/stats_iv.json</c> (#1155).</summary>
/// <remarks>
/// Four simulated problems under every covariance, kernel, bandwidth rule, debiasing, Fuller correction and GMM weight
/// the reference offers. Values at <c>1e-9</c> relative; p-values also within an absolute <c>1e-15</c>, since the
/// reference computes <c>2 − 2·cdf</c> and returns zero where this returns the tail.
/// </remarks>
public sealed class IvOracleTests
{
    private const double Relative = 1e-9;
    private const double PFloor = 1e-15;

    private static readonly JsonDocument Corpus = OracleLoader.Load("stats_iv.json");

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
        IvSummary summary = Fit(frozen);

        CloseAll(expected, "coefficients", summary.Coefficients, name);
        CloseAll(expected, "standardErrors", summary.StandardErrors, name);
        CloseAll(expected, "tStatistics", summary.TStatistics, name);
        Probabilities(expected, "pValues", summary.PValues, name);
        CloseAll(expected, "confidenceLower", summary.ConfidenceLower, name);
        CloseAll(expected, "confidenceUpper", summary.ConfidenceUpper, name);
        Close(expected.GetProperty("rSquared").GetDouble(), summary.RSquared, $"{name}: R²");
        Close(expected.GetProperty("adjustedRSquared").GetDouble(), summary.AdjustedRSquared, $"{name}: adjusted R²");
        Assert.Equal(expected.GetProperty("residualDf").GetInt32(), summary.ResidualDegreesOfFreedom);
        Assert.Equal(OptionalInt(expected, "bandwidth"), summary.Bandwidth);
        if (expected.GetProperty("kappa").ValueKind != JsonValueKind.Null)
        {
            Close(expected.GetProperty("kappa").GetDouble(), summary.Kappa!.Value, $"{name}: κ");
        }
        else
        {
            Assert.Null(summary.Kappa);
        }
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_model_and_overidentification_tests_match_the_reference(int index)
    {
        JsonElement frozen = Cases[index];
        string name = frozen.GetProperty("name").GetString()!;
        JsonElement expected = frozen.GetProperty("expected");
        IvSummary summary = Fit(frozen);

        IvTest model = summary.ModelTest!;
        Close(expected.GetProperty("modelStatistic").GetDouble(), model.Statistic, $"{name}: model statistic");
        Probability(expected.GetProperty("modelPValue").GetDouble(), model.PValue, $"{name}: model p-value");
        Assert.Equal(expected.GetProperty("modelDf").GetInt32(), model.DegreesOfFreedom);
        Assert.Equal(OptionalInt(expected, "modelDenominatorDf"), model.DenominatorDegreesOfFreedom);

        JsonElement over = expected.GetProperty("overidentification");
        if (over.ValueKind == JsonValueKind.Null)
        {
            Assert.Null(summary.Overidentification);
            return;
        }

        Close(over.GetProperty("statistic").GetDouble(), summary.Overidentification!.Statistic, $"{name}: overidentification");
        Probability(over.GetProperty("pValue").GetDouble(), summary.Overidentification.PValue, $"{name}: overidentification p");
        Assert.Equal(over.GetProperty("df").GetInt32(), summary.Overidentification.DegreesOfFreedom);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_first_stage_matches_the_reference(int index)
    {
        JsonElement frozen = Cases[index];
        string name = frozen.GetProperty("name").GetString()!;
        JsonElement[] rows = [.. frozen.GetProperty("expected").GetProperty("firstStage").EnumerateArray()];
        IvSummary summary = Fit(frozen);

        Assert.Equal(rows.Length, summary.FirstStage.Count);
        for (int j = 0; j < rows.Length; j++)
        {
            IvFirstStage actual = summary.FirstStage[j];
            Close(rows[j].GetProperty("rSquared").GetDouble(), actual.RSquared, $"{name}: first stage {j} R²");
            Close(rows[j].GetProperty("partialRSquared").GetDouble(), actual.PartialRSquared, $"{name}: first stage {j} partial R²");
            Close(rows[j].GetProperty("sheaRSquared").GetDouble(), actual.SheaRSquared, $"{name}: first stage {j} Shea R²");
            Close(rows[j].GetProperty("statistic").GetDouble(), actual.InstrumentTest.Statistic, $"{name}: first stage {j} statistic");
            Probability(rows[j].GetProperty("pValue").GetDouble(), actual.InstrumentTest.PValue, $"{name}: first stage {j} p");
        }
    }

    private static IvSummary Fit(JsonElement frozen)
    {
        JsonElement data = Corpus.RootElement.GetProperty("problems").GetProperty(frozen.GetProperty("data").GetString()!);
        double[] response = Doubles(data, "response");
        double[] exogenous = Doubles(data, "exogenous");
        double[] endogenous = Doubles(data, "endogenous");
        double[] instruments = Doubles(data, "instruments");
        var design = new IvDesign(
            response,
            exogenous,
            data.GetProperty("exogenousCount").GetInt32(),
            endogenous,
            data.GetProperty("endogenousCount").GetInt32(),
            instruments,
            data.GetProperty("instrumentCount").GetInt32());
        IvOptions options = Options(frozen.GetProperty("options"));
        int[] clusters = [.. data.GetProperty("clusters").EnumerateArray().Select(v => v.GetInt32())];
        bool clustered = options.CovarianceType == IvCovarianceType.Clustered
            || (frozen.GetProperty("method").GetString() == "gmm" && options.GmmWeightType == IvCovarianceType.Clustered);

        return frozen.GetProperty("method").GetString() switch
        {
            "2sls" => clustered ? InstrumentalVariables.TwoStageLeastSquares(design, clusters, options) : InstrumentalVariables.TwoStageLeastSquares(design, options),
            "liml" => clustered ? InstrumentalVariables.Liml(design, clusters, options) : InstrumentalVariables.Liml(design, options),
            _ => clustered ? InstrumentalVariables.Gmm(design, clusters, options) : InstrumentalVariables.Gmm(design, options),
        };
    }

    private static IvOptions Options(JsonElement frozen)
    {
        var options = new IvOptions();
        foreach (JsonProperty property in frozen.EnumerateObject())
        {
            JsonElement value = property.Value;
            options = property.Name switch
            {
                "covarianceType" => options with { CovarianceType = Covariance(value.GetString()!) },
                "debiased" => options with { Debiased = value.GetBoolean() },
                "kernel" => options with { Kernel = Kernel(value.GetString()!) },
                "bandwidth" => options with { Bandwidth = value.ValueKind == JsonValueKind.Null ? null : value.GetInt32() },
                "fuller" => options with { Fuller = value.GetDouble() },
                "withIntercept" => options with { WithIntercept = value.GetBoolean() },
                "gmmWeightType" => options with { GmmWeightType = Covariance(value.GetString()!) },
                "gmmWeightKernel" => options with { GmmWeightKernel = Kernel(value.GetString()!) },
                "gmmWeightBandwidth" => options with { GmmWeightBandwidth = value.GetInt32() },
                _ => throw new InvalidOperationException($"Unknown option {property.Name}."),
            };
        }

        return options;
    }

    private static IvCovarianceType Covariance(string name) => name switch
    {
        "unadjusted" => IvCovarianceType.Unadjusted,
        "robust" => IvCovarianceType.Robust,
        "kernel" => IvCovarianceType.Kernel,
        _ => IvCovarianceType.Clustered,
    };

    private static IvKernel Kernel(string name) => name switch
    {
        "bartlett" => IvKernel.Bartlett,
        "parzen" => IvKernel.Parzen,
        _ => IvKernel.QuadraticSpectral,
    };

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

    private static void Probability(double expected, double actual, string what) =>
        Assert.True(
            Math.Abs(actual - expected) <= (Relative * Math.Abs(expected)) + PFloor,
            $"{what}: {actual:R} against {expected:R}");

    private static int? OptionalInt(JsonElement element, string key) =>
        element.GetProperty(key).ValueKind == JsonValueKind.Null ? null : element.GetProperty(key).GetInt32();

    private static double[] Doubles(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetDouble())];
}
