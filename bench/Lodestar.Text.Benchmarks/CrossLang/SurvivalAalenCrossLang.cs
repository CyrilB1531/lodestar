using Lodestar.Survival;

namespace Lodestar.Text.Benchmarks.CrossLang;

/// <summary>The #1173 Aalen additive fit against <c>bench/python/bench_survival_aalen.py</c>: same subjects, operations and method.</summary>
public static class SurvivalAalenCrossLang
{
    private const int Features = 4;

    private static readonly int[] Sizes = [1_000, 10_000];

    /// <summary>Runs every shape and writes <c>bench/results/csharp-survival-aalen.json</c>.</summary>
    public static void Run()
    {
        string outPath = Path.Combine(BenchCorpus.RepoRoot(), "bench", "results", "csharp-survival-aalen.json");
        var results = new List<Harness.OperationResult>();
        var penalised = new AalenOptions { CoefficientPenalizer = 0.5, SmoothingPenalizer = 1.0 };
        foreach (int n in Sizes)
        {
            (double[] x, double[] t, bool[] e, _, double[] w) = CoxExtendedCrossLang.Subjects(n);
            AalenSummary fitted = AalenAdditive.Fit(x, t, e, Features);
            double[] rows = x.AsSpan(0, 100 * Features).ToArray();
            results.Add(Harness.Measure($"fit_{n}", () => AalenAdditive.Fit(x, t, e, Features)));
            results.Add(Harness.Measure($"fit_penalised_weighted_{n}", () => AalenAdditive.Fit(x, t, e, w, Features, penalised)));
            results.Add(Harness.Measure($"predict_survival_{n}", () => fitted.PredictSurvivalFunction(rows)));
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
}
