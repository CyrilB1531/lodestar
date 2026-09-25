using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Panel;

namespace Lodestar.Text.Benchmarks.CrossLang;

/// <summary>The #1156 estimators against <c>bench/python/bench_panel.py</c>: same shapes, data formula, operations and method.</summary>
/// <remarks>
/// No corpus file: both sides build each value from its entity, period and column, and agreement is
/// <c>stats_panel.json</c>'s job. Each operation builds the whole summary, which the Python side reads out.
/// </remarks>
public static class PanelCrossLang
{
    private const int Regressors = 3;
    private const int Periods = 10;

    private static readonly int[] EntityCounts = [100, 1_000, 10_000];

    /// <summary>Runs every shape and writes <c>bench/results/csharp-panel.json</c>.</summary>
    public static void Run()
    {
        string outPath = Path.Combine(BenchCorpus.RepoRoot(), "bench", "results", "csharp-panel.json");
        var results = new List<Harness.OperationResult>();
        foreach (int entities in EntityCounts)
        {
            (double[] y, double[] x, int[] entity, int[] period) = Data(entities);
            string suffix = $"n{entities * Periods}";
            var oneWay = new PanelOptions { EntityEffects = true };
            var twoWay = new PanelOptions
            {
                EntityEffects = true,
                TimeEffects = true,
                CovarianceType = PanelCovarianceType.Clustered,
                ClusterEntity = true,
            };
            var driscollKraay = new PanelOptions { EntityEffects = true, CovarianceType = PanelCovarianceType.Kernel };
            var robust = new PanelOptions { CovarianceType = PanelCovarianceType.Robust };
            results.Add(Harness.Measure($"fe_entity_{suffix}", () => PanelRegression.FixedEffects(Design(y, x, entity, period), oneWay)));
            results.Add(Harness.Measure($"fe_twoway_clustered_{suffix}", () => PanelRegression.FixedEffects(Design(y, x, entity, period), twoWay)));
            results.Add(Harness.Measure($"fe_kernel_{suffix}", () => PanelRegression.FixedEffects(Design(y, x, entity, period), driscollKraay)));
            results.Add(Harness.Measure($"between_robust_{suffix}", () => PanelRegression.Between(Design(y, x, entity, period), robust)));
            results.Add(Harness.Measure(
                $"first_difference_robust_{suffix}",
                () => PanelRegression.FirstDifference(Design(y, x, entity, period), robust with { WithIntercept = false })));
            results.Add(Harness.Measure($"random_effects_{suffix}", () => PanelRegression.RandomEffects(Design(y, x, entity, period))));
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

    private static PanelDesign Design(double[] y, double[] x, int[] entity, int[] period) => new(y, x, Regressors, entity, period);

    /// <summary>The Python side's formula: an entity effect that moves with the regressors, a period shock, and noise.</summary>
    private static (double[] Y, double[] X, int[] Entity, int[] Period) Data(int entities)
    {
        int n = entities * Periods;
        var y = new double[n];
        var x = new double[n * Regressors];
        var entity = new int[n];
        var period = new int[n];
        for (int row = 0; row < n; row++)
        {
            int e = row / Periods;
            int t = row % Periods;
            double effect = Math.Sin(0.37 * e);
            double response = effect + (0.2 * Math.Cos(1.1 * t)) + (0.5 * Math.Sin(2.9 * row) * Math.Cos(0.13 * row));
            for (int j = 0; j < Regressors; j++)
            {
                double value = Math.Cos((0.7 * (j + 1) * row) + (0.3 * j)) + (0.5 * effect);
                x[(row * Regressors) + j] = value;
                response += value * (0.4 * (j + 1));
            }

            y[row] = response;
            entity[row] = e;
            period[row] = 2000 + t;
        }

        return (y, x, entity, period);
    }
}
