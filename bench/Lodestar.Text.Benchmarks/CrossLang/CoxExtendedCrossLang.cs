using Lodestar.Survival;

namespace Lodestar.Text.Benchmarks.CrossLang;

/// <summary>The #1171 Cox extensions against <c>bench/python/bench_cox_extended.py</c>: same subjects, operations and method.</summary>
/// <remarks>Stratified and weighted, ridge with the robust variance, lasso, the proportional hazards test, predictions, time-varying.</remarks>
public static class CoxExtendedCrossLang
{
    private const int Features = 4;

    private static readonly int[] Sizes = [1_000, 10_000];

    /// <summary>Runs every shape and writes <c>bench/results/csharp-cox-extended.json</c>.</summary>
    public static void Run()
    {
        string outPath = Path.Combine(BenchCorpus.RepoRoot(), "bench", "results", "csharp-cox-extended.json");
        var results = new List<Harness.OperationResult>();
        var ridge = new CoxOptions { Penalizer = 0.1, Robust = true };
        var lasso = new CoxOptions { Penalizer = 0.05, L1Ratio = 1.0 };
        foreach (int n in Sizes)
        {
            (double[] x, double[] t, bool[] e, int[] s, double[] w) = Subjects(n);
            CoxSummary fitted = CoxProportionalHazards.Fit(x, t, e, Features);
            double[] rows = x[..(100 * Features)];
            double[] times = Quantiles(t, 10);
            (double[] vx, double[] starts, double[] stops, bool[] ve) = Intervals(x, t, e);
            results.Add(Harness.Measure($"stratified_weighted_{n}", () => CoxProportionalHazards.Fit(x, t, e, w, s, [], Features)));
            results.Add(Harness.Measure($"ridge_robust_{n}", () => CoxProportionalHazards.Fit(x, t, e, Features, ridge)));
            results.Add(Harness.Measure($"lasso_{n}", () => CoxProportionalHazards.Fit(x, t, e, Features, lasso)));
            results.Add(Harness.Measure($"ph_test_{n}", () => CoxProportionalHazards.TestProportionalHazards(x, t, e, fitted)));
            results.Add(Harness.Measure($"predict_survival_{n}", () => fitted.PredictSurvivalFunction(rows, [], times)));
            results.Add(Harness.Measure($"time_varying_{n}", () => CoxTimeVarying.Fit(vx, starts, stops, ve, Features)));
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

    /// <summary>The Python side's formula, from the index alone; <see cref="SurvivalParametricCrossLang"/> reads it too.</summary>
    internal static (double[] X, double[] T, bool[] E, int[] S, double[] W) Subjects(int n)
    {
        var x = new double[n * Features];
        var t = new double[n];
        var e = new bool[n];
        var s = new int[n];
        var w = new double[n];
        for (long i = 0; i < n; i++)
        {
            for (long j = 0; j < Features; j++)
            {
                x[(i * Features) + j] = (((i * (2654435761 + (j * 104729))) % 10007) / 10007.0 * 2.0) - 1.0;
            }

            double u = (((i * 7919) % 10007) + 1) / 10008.0;
            t[i] = Math.Round(-Math.Log(u) / Math.Exp((0.5 * x[i * Features]) - (0.3 * x[(i * Features) + 1])) * 10.0, 2, MidpointRounding.ToEven) + 0.01;
            e[i] = (i * 37) % 10 < 7;
            s[i] = (int)(i % 3);
            w[i] = 1 + (i % 3);
        }

        return (x, t, e, s, w);
    }

    /// <summary>Two intervals per subject, split at half its duration; the second carries x1 moved by a quarter.</summary>
    private static (double[] X, double[] Starts, double[] Stops, bool[] E) Intervals(double[] x, double[] t, bool[] e)
    {
        int n = t.Length;
        var vx = new double[2 * n * Features];
        var starts = new double[2 * n];
        var stops = new double[2 * n];
        var ve = new bool[2 * n];
        for (int i = 0; i < n; i++)
        {
            Array.Copy(x, i * Features, vx, i * Features, Features);
            Array.Copy(x, i * Features, vx, (n + i) * Features, Features);
            vx[((n + i) * Features) + 1] += 0.25;
            stops[i] = t[i] / 2.0;
            starts[n + i] = t[i] / 2.0;
            stops[n + i] = t[i];
            ve[n + i] = e[i];
        }

        return (vx, starts, stops, ve);
    }

    /// <summary>numpy's default linear quantiles at <paramref name="count"/> evenly spaced levels from 5 % to 95 %.</summary>
    private static double[] Quantiles(double[] values, int count)
    {
        double[] sorted = [.. values.OrderBy(v => v)];
        var result = new double[count];
        for (int k = 0; k < count; k++)
        {
            double level = 0.05 + (0.9 * k / (count - 1));
            double position = level * (sorted.Length - 1);
            int low = (int)Math.Floor(position);
            int high = Math.Min(low + 1, sorted.Length - 1);
            result[k] = sorted[low] + ((position - low) * (sorted[high] - sorted[low]));
        }

        return result;
    }
}
