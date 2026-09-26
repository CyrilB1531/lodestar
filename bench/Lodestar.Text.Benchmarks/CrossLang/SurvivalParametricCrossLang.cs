using Lodestar.Survival;

namespace Lodestar.Text.Benchmarks.CrossLang;

/// <summary>The #1172 parametric fits against <c>bench/python/bench_survival_parametric.py</c>: same subjects, operations and method.</summary>
/// <remarks>Four univariate fits, three AFT regressions, the Breslow-Fleming-Harrington and the left-censored Kaplan-Meier curves.</remarks>
public static class SurvivalParametricCrossLang
{
    private const int Features = 4;

    private static readonly int[] Sizes = [1_000, 10_000];

    /// <summary>Runs every shape and writes <c>bench/results/csharp-survival-parametric.json</c>.</summary>
    public static void Run()
    {
        string outPath = Path.Combine(BenchCorpus.RepoRoot(), "bench", "results", "csharp-survival-parametric.json");
        var results = new List<Harness.OperationResult>();
        var ridge = new AftOptions { Penalizer = 0.1, Robust = true };
        foreach (int n in Sizes)
        {
            (double[] x, double[] t, bool[] e, _, _) = CoxExtendedCrossLang.Subjects(n);
            double[] lower = [.. t];
            double[] upper = [.. t.Select((d, i) => Upper(d, e[i], i))];
            results.Add(Harness.Measure($"weibull_{n}", () => ParametricSurvival.Fit(ParametricModel.Weibull, t, e)));
            results.Add(Harness.Measure($"loglogistic_left_{n}", () => ParametricSurvival.FitLeftCensored(ParametricModel.LogLogistic, t, e)));
            results.Add(Harness.Measure(
                $"lognormal_interval_{n}", () => ParametricSurvival.FitIntervalCensored(ParametricModel.LogNormal, lower, upper)));
            results.Add(Harness.Measure($"generalized_gamma_{n}", () => ParametricSurvival.Fit(ParametricModel.GeneralizedGamma, t, e)));
            results.Add(Harness.Measure($"weibull_aft_{n}", () => AcceleratedFailureTime.Fit(AftModel.Weibull, x, t, e, Features)));
            results.Add(Harness.Measure(
                $"lognormal_aft_ridge_robust_{n}", () => AcceleratedFailureTime.Fit(AftModel.LogNormal, x, t, e, Features, ridge)));
            results.Add(Harness.Measure(
                $"loglogistic_aft_interval_{n}",
                () => AcceleratedFailureTime.FitIntervalCensored(AftModel.LogLogistic, x, lower, upper, Features)));
            results.Add(Harness.Measure($"breslow_fleming_harrington_{n}", () => BreslowFlemingHarrington.Estimate(t, e)));
            results.Add(Harness.Measure($"kaplan_meier_left_{n}", () => KaplanMeier.EstimateLeftCensored(t, e)));
        }

        Harness.Write(
            outPath,
            new Harness.Output
            {
                Metadata = new Harness.OutputMetadata
                {
                    Side = "csharp",
                    Library = "Lodestar",
                    Runtime = Environment.Version.ToString(),
                    Os = Environment.OSVersion.ToString(),
                    MinTimeS = Harness.MinTimeSeconds,
                    Repeats = Harness.RepeatCount,
                },
                Results = results,
            });
    }

    /// <summary>An event's own time; a censored one's half as far again, or open one time in five.</summary>
    private static double Upper(double duration, bool observed, int i)
    {
        if (observed)
        {
            return duration;
        }

        return i % 5 == 0 ? double.PositiveInfinity : duration * 1.5;
    }
}
