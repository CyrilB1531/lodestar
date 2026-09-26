using System.Text.Json;
using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>lifelines' parametric univariate fitters and AFT regressions at their maximum, against lifelines 0.30.3 polished by Newton.</summary>
/// <remarks>
/// Values at 1e-9, absolutely below one and relatively above; the generalized gamma at 2e-9, lifelines differentiating
/// its incomplete gamma by finite differences (#1160, #1172). p-values relatively.
/// </remarks>
public sealed class ParametricOracleTests
{
    private static readonly JsonDocument Corpus = OracleLoader.Load("survival_parametric.json");

    public static TheoryData<string, int> Cases()
    {
        var data = new TheoryData<string, int>();
        foreach (JsonProperty section in Corpus.RootElement.EnumerateObject().Where(p => p.Name != "metadata"))
        {
            for (int i = 0; i < section.Value.GetArrayLength(); i++)
            {
                data.Add(section.Name, i);
            }
        }

        return data;
    }

    /// <summary>The generator drops a case lifelines cannot fit; every model under every censoring must survive that.</summary>
    [Fact]
    public void Every_model_is_frozen_under_every_censoring()
    {
        var pairs = Corpus.RootElement.GetProperty("univariate").EnumerateArray()
            .Select(c => (c.GetProperty("model").GetString(), c.GetProperty("censoring").GetString()))
            .ToHashSet();

        Assert.Equal(Enum.GetValues<ParametricModel>().Length * 3, pairs.Count);
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public void The_parametric_fit_matches_lifelines(string section, int index)
    {
        JsonElement c = Corpus.RootElement.GetProperty(section)[index];
        string name = c.GetProperty("name").GetString()!;
        if (section == "aft")
        {
            Aft(c, name);
        }
        else if (section is "breslow_fleming_harrington" or "kaplan_meier_left")
        {
            Curve(c, name, section);
        }
        else if (section == "fixed_point")
        {
            FixedPoint(c, name);
        }
        else
        {
            Assert.Equal("univariate", section);
            Univariate(c, name);
        }
    }

    private static void Aft(JsonElement c, string name)
    {
        var model = Enum.Parse<AftModel>(c.GetProperty("model").GetString()!);
        JsonElement o = c.GetProperty("options");
        var options = new AftOptions
        {
            Penalizer = o.GetProperty("penalizer").GetDouble(),
            FitIntercept = o.GetProperty("fitIntercept").GetBoolean(),
            Ancillary = o.GetProperty("ancillary").GetBoolean(),
            Robust = o.GetProperty("robust").GetBoolean(),
        };
        double[] weights = o.GetProperty("weighted").GetBoolean() ? Doubles(c, "weights") : [];
        double[] entries = o.GetProperty("entries").GetBoolean() ? Doubles(c, "entries") : [];
        double[] design = Rows(c, "design");
        AftSummary fit = c.GetProperty("censoring").GetString() switch
        {
            "left" => AcceleratedFailureTime.FitLeftCensored(model, design, Doubles(c, "durations"), Flags(c, "eventObserved"), weights, entries, 2, options),
            "interval" => AcceleratedFailureTime.FitIntervalCensored(model, design, Doubles(c, "lower"), Doubles(c, "upper"), weights, entries, 2, options),
            _ => AcceleratedFailureTime.Fit(model, design, Doubles(c, "durations"), Flags(c, "eventObserved"), weights, entries, 2, options),
        };

        string[] names = [.. fit.ParameterNames.Select((p, j) => $"{p}:{(fit.CovariateIndices[j] < 0 ? "Intercept" : $"x{fit.CovariateIndices[j]}")}")];
        Assert.Equal(Strings(c, "names"), names);
        const double tolerance = 1e-9;
        Near(c, "coefficients", fit.Coefficients, name, tolerance);
        Near(c, "standardErrors", fit.StandardErrors, name, tolerance);
        Near(c, "zStatistics", fit.ZStatistics, name, tolerance);
        Relative(c.GetProperty("pValues"), fit.PValues, $"{name}: p");
        Near(c, "confidenceLower", fit.ConfidenceLower, name, tolerance);
        Near(c, "confidenceUpper", fit.ConfidenceUpper, name, tolerance);
        Near(c, "expCoefficients", fit.ExpCoefficients, name, tolerance);
        Near(c.GetProperty("logLikelihood").GetDouble(), fit.LogLikelihood, $"{name}: log-likelihood", tolerance);
        Near(c.GetProperty("nullLogLikelihood").GetDouble(), fit.NullLogLikelihood, $"{name}: null", tolerance);
        Near(c.GetProperty("likelihoodRatio").GetDouble(), fit.LikelihoodRatioStatistic, $"{name}: LR", tolerance);
        Relative(c.GetProperty("likelihoodRatioP"), [fit.LikelihoodRatioPValue], $"{name}: LR p");
        Assert.Equal(c.GetProperty("likelihoodRatioDf").GetInt32(), fit.LikelihoodRatioDegreesOfFreedom);
        Near(c.GetProperty("aic").GetDouble(), fit.Aic, $"{name}: AIC", tolerance);
        JsonElement concordance = c.GetProperty("concordance");
        if (concordance.ValueKind == JsonValueKind.Null)
        {
            Assert.True(double.IsNaN(fit.ConcordanceIndex), $"{name}: concordance {fit.ConcordanceIndex}");
        }
        else
        {
            Near(concordance.GetDouble(), fit.ConcordanceIndex, $"{name}: concordance", tolerance);
        }

        double[] predict = Rows(c, "predictDesign");
        double[] times = Doubles(c, "times");
        Near(c, "median", fit.PredictMedian(predict), name, tolerance);
        Near(c, "quartile", fit.PredictPercentile(predict, 0.25), name, tolerance);
        Near(c, "expectation", fit.PredictExpectation(predict), name, tolerance);
        Near(c, "survival", fit.PredictSurvivalFunction(predict, times), name, tolerance);
        Near(c, "cumulativeHazard", fit.PredictCumulativeHazard(predict, times), name, tolerance);
    }

    /// <summary>lifelines labels the larger survival bound its lower one in both estimators; the curve here orders them.</summary>
    private static void Curve(JsonElement c, string name, string section)
    {
        double[] durations = Doubles(c, "durations");
        bool[] events = Flags(c, "eventObserved");
        (SurvivalStep[] steps, double[] survival, double[] lower, double[] upper) = section == "kaplan_meier_left"
            ? Parts(KaplanMeier.EstimateLeftCensored(durations, events))
            : Parts(BreslowFlemingHarrington.Estimate(durations, events, OptionalDoubles(c, "entries")));
        const double tolerance = 1e-12;
        Near(c, "times", [.. steps.Select(s => s.Time)], name, 0.0);
        Assert.Equal(Ints(c, "atRisk"), steps.Select(s => s.AtRisk));
        Assert.Equal(Ints(c, "events"), steps.Select(s => s.Events));
        Assert.Equal(Ints(c, "censored"), steps.Select(s => s.Censored));
        Near(c, "survival", survival, name, tolerance);
        Near(c, "lifelinesUpper", lower, name, tolerance);
        Near(c, "lifelinesLower", upper, name, tolerance);
    }

    private static (SurvivalStep[], double[], double[], double[]) Parts(KaplanMeierCurve curve) =>
        (curve.Steps, curve.Survival, curve.Lower, curve.Upper);

    private static (SurvivalStep[], double[], double[], double[]) Parts(SurvivalCurve curve) =>
        (curve.Steps, curve.Survival, curve.Lower, curve.Upper);

    private static int[] Ints(JsonElement c, string name) => [.. c.GetProperty(name).EnumerateArray().Select(e => e.GetInt32())];

    private static void FixedPoint(JsonElement c, string name)
    {
        var options = new ParametricOptions { Breakpoints = OptionalDoubles(c, "breakpoints") };
        ParametricFit a = ParametricSurvival.Fit(
            Enum.Parse<ParametricModel>(c.GetProperty("modelA").GetString()!), Doubles(c, "durationsA"), Flags(c, "eventsA"), options);
        ParametricFit b = ParametricSurvival.Fit(
            Enum.Parse<ParametricModel>(c.GetProperty("modelB").GetString()!), Doubles(c, "durationsB"), Flags(c, "eventsB"), options);
        Lodestar.Stats.TestResult result = ParametricSurvival.CompareAt(c.GetProperty("time").GetDouble(), a, b);

        Near(c.GetProperty("statistic").GetDouble(), result.Statistic, $"{name}: statistic", 1e-9);
        Relative(c.GetProperty("pValue"), [result.PValue], $"{name}: p");
    }

    /// <summary>p-values relatively, the tail being as accurate far out as near one.</summary>
    private static void Relative(JsonElement expected, IReadOnlyList<double> actual, string name)
    {
        double[] values = expected.ValueKind == JsonValueKind.Array ? [.. expected.EnumerateArray().Select(e => e.GetDouble())] : [expected.GetDouble()];
        Assert.Equal(values.Length, actual.Count);
        for (int i = 0; i < values.Length; i++)
        {
            Assert.True(Math.Abs(values[i] - actual[i]) <= 1e-8 * Math.Abs(values[i]) + 1e-300, $"{name}[{i}]: {actual[i]} against {values[i]}");
        }
    }

    private static double[] Rows(JsonElement c, string name) =>
        [.. c.GetProperty(name).EnumerateArray().SelectMany(row => row.EnumerateArray().Select(e => e.GetDouble()))];

    private static void Univariate(JsonElement c, string name)
    {
        var model = Enum.Parse<ParametricModel>(c.GetProperty("model").GetString()!);
        // A value-only case is frozen where autograd's derivative is NaN, at an optimum values alone place to about 1e-7.
        bool valueOnly = c.TryGetProperty("valueOnly", out JsonElement flag) && flag.GetBoolean();
        double tolerance = model == ParametricModel.GeneralizedGamma ? 2e-9 : 1e-9;
        double located = valueOnly ? 1e-6 : tolerance;
        var options = new ParametricOptions { Breakpoints = OptionalDoubles(c, "breakpoints") };
        double[] weights = OptionalDoubles(c, "weights");
        double[] entries = OptionalDoubles(c, "entries");
        ParametricFit fit = c.GetProperty("censoring").GetString() switch
        {
            "left" => ParametricSurvival.FitLeftCensored(model, Doubles(c, "durations"), Flags(c, "eventObserved"), weights, entries, options),
            "interval" => ParametricSurvival.FitIntervalCensored(model, Doubles(c, "lower"), Doubles(c, "upper"), weights, entries, options),
            _ => ParametricSurvival.Fit(model, Doubles(c, "durations"), Flags(c, "eventObserved"), weights, entries, options),
        };

        Assert.Equal(Strings(c, "names"), fit.ParameterNames);
        Near(c, "parameters", fit.Parameters, name, located);
        // The generalized gamma's inference is frozen from an independent Hessian good to about 1e-8, lifelines' own
        // being 2e-5 off; lifelines' standard errors stay beside it, to 1e-3.
        double inference = model == ParametricModel.GeneralizedGamma ? 1e-6 : tolerance;
        inference = valueOnly ? 1e-5 : inference;
        Near(c, "standardErrors", fit.StandardErrors, name, inference);
        Near(c, "zStatistics", fit.ZStatistics, name, inference);
        if (model == ParametricModel.GeneralizedGamma && !valueOnly)
        {
            Near(c, "lifelinesStandardErrors", fit.StandardErrors, name, 1e-3);
        }
        Near(c.GetProperty("logLikelihood").GetDouble(), fit.LogLikelihood, $"{name}: log-likelihood", tolerance);
        Near(c.GetProperty("aic").GetDouble(), fit.Aic, $"{name}: AIC", tolerance);
        double[] times = Doubles(c, "times");
        Near(c, "survival", fit.Survival(times), name, located);
        Near(c, "cumulativeHazard", fit.CumulativeHazard(times), name, located);
        Near(c, "hazard", fit.Hazard(times), name, located);
        (double[] lower, double[] upper) = fit.SurvivalBounds(times);
        Near(c.GetProperty("survivalBounds")[0], lower, $"{name}: survival lower", inference);
        Near(c.GetProperty("survivalBounds")[1], upper, $"{name}: survival upper", inference);
        (lower, upper) = fit.CumulativeHazardBounds(times);
        Near(c.GetProperty("cumulativeHazardBounds")[0], lower, $"{name}: cumulative lower", inference);
        Near(c.GetProperty("cumulativeHazardBounds")[1], upper, $"{name}: cumulative upper", inference);
        Percentile(c, "median", fit, 0.5, name, valueOnly ? 1e-6 : 1e-8);
        Percentile(c, "quartile", fit, 0.25, name, valueOnly ? 1e-6 : 1e-8);
    }

    /// <summary>lifelines finds a percentile by a root search, to its own tolerance; the closed form here is compared at 1e-8.</summary>
    private static void Percentile(JsonElement c, string key, ParametricFit fit, double probability, string name, double tolerance)
    {
        JsonElement expected = c.GetProperty(key);
        if (expected.ValueKind == JsonValueKind.Null)
        {
            return;
        }

        Near(expected.GetDouble(), fit.Percentile(probability), $"{name}: {key}", tolerance);
    }

    private static void Near(JsonElement c, string key, IReadOnlyList<double> actual, string name, double tolerance) =>
        Near(c.GetProperty(key), actual, $"{name}: {key}", tolerance);

    private static void Near(JsonElement expected, IReadOnlyList<double> actual, string name, double tolerance)
    {
        double[] values = [.. expected.EnumerateArray().Select(e => e.GetDouble())];
        Assert.Equal(values.Length, actual.Count);
        for (int i = 0; i < values.Length; i++)
        {
            Near(values[i], actual[i], $"{name}[{i}]", tolerance);
        }
    }

    private static void Near(double expected, double actual, string name, double tolerance) =>
        Assert.True(Math.Abs(expected - actual) <= tolerance * Math.Max(1.0, Math.Abs(expected)), $"{name}: {actual} against {expected}");

    private static double[] OptionalDoubles(JsonElement c, string name) =>
        c.GetProperty(name).ValueKind == JsonValueKind.Null ? [] : Doubles(c, name);

    private static double[] Doubles(JsonElement c, string name) =>
        [.. c.GetProperty(name).EnumerateArray().Select(e => e.ValueKind == JsonValueKind.Null ? double.PositiveInfinity : e.GetDouble())];

    private static string[] Strings(JsonElement c, string name) =>
        [.. c.GetProperty(name).EnumerateArray().Select(e => e.GetString()!)];

    private static bool[] Flags(JsonElement c, string name) =>
        [.. c.GetProperty(name).EnumerateArray().Select(e => e.GetInt32() == 1)];
}
