using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Instrumental;

namespace Lodestar.Text.Benchmarks.CrossLang;

/// <summary>The #1155 estimators against <c>bench/python/bench_iv.py</c>: same shapes, data formula, operations and method.</summary>
/// <remarks>
/// No corpus file: both sides build each value from its indices, and agreement is <c>stats_iv.json</c>'s job.
/// Each operation builds the whole summary, which the Python side reads out since <c>linearmodels</c> defers it.
/// </remarks>
public static class IvCrossLang
{
    private const int ExogenousCount = 3;
    private const int EndogenousCount = 2;
    private const int InstrumentCount = 4;
    private const int RowsPerCluster = 20;

    private static readonly int[] Sizes = [1_000, 10_000, 100_000];

    /// <summary>Runs every shape and writes <c>bench/results/csharp-iv.json</c>.</summary>
    public static void Run()
    {
        string outPath = Path.Combine(BenchCorpus.RepoRoot(), "bench", "results", "csharp-iv.json");
        var results = new List<Harness.OperationResult>();
        foreach (int n in Sizes)
        {
            (double[] y, double[] exogenous, double[] endogenous, double[] instruments, int[] clusters) = Data(n);
            string suffix = $"n{n}";
            var clustered = new IvOptions { CovarianceType = IvCovarianceType.Clustered };
            var kernel = new IvOptions { CovarianceType = IvCovarianceType.Kernel };
            results.Add(Harness.Measure($"2sls_robust_{suffix}", () => InstrumentalVariables.TwoStageLeastSquares(
                Design(y, exogenous, endogenous, instruments))));
            results.Add(Harness.Measure($"liml_robust_{suffix}", () => InstrumentalVariables.Liml(
                Design(y, exogenous, endogenous, instruments))));
            results.Add(Harness.Measure($"gmm_robust_{suffix}", () => InstrumentalVariables.Gmm(
                Design(y, exogenous, endogenous, instruments))));
            results.Add(Harness.Measure($"2sls_kernel_{suffix}", () => InstrumentalVariables.TwoStageLeastSquares(
                Design(y, exogenous, endogenous, instruments), kernel)));
            results.Add(Harness.Measure($"2sls_clustered_{suffix}", () => InstrumentalVariables.TwoStageLeastSquares(
                Design(y, exogenous, endogenous, instruments), clusters, clustered)));
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

    private static IvDesign Design(double[] y, double[] exogenous, double[] endogenous, double[] instruments) =>
        new(y, exogenous, ExogenousCount, endogenous, EndogenousCount, instruments, InstrumentCount);

    /// <summary>The Python side's formula: smooth, deterministic, and an error shared by the endogenous block and the response.</summary>
    /// <remarks>Each column has its own frequency: columns at one frequency and different phases span two dimensions, not three.</remarks>
    private static (double[] Y, double[] Exogenous, double[] Endogenous, double[] Instruments, int[] Clusters) Data(int n)
    {
        var y = new double[n];
        var exogenous = new double[n * ExogenousCount];
        var endogenous = new double[n * EndogenousCount];
        var instruments = new double[n * InstrumentCount];
        var clusters = new int[n];
        for (int i = 0; i < n; i++)
        {
            double u = Math.Sin(1.7 * i) * Math.Cos(0.3 * i);
            for (int j = 0; j < InstrumentCount; j++)
            {
                instruments[(i * InstrumentCount) + j] = Math.Sin((0.7 * (j + 1) * i) + (1.3 * j)) + (0.5 * Math.Cos(0.11 * i * (j + 1)));
            }

            double response = 0.5 + u;
            for (int j = 0; j < ExogenousCount; j++)
            {
                double w = Math.Cos((0.9 * (j + 1) * i) + (0.4 * j));
                exogenous[(i * ExogenousCount) + j] = w;
                response += w * 0.2 * (j + 1);
            }

            for (int k = 0; k < EndogenousCount; k++)
            {
                double x = (0.6 * u) + (0.3 * Math.Sin((2.3 * i) + k));
                for (int j = 0; j < InstrumentCount; j++)
                {
                    x += instruments[(i * InstrumentCount) + j] * (0.5 + (0.1 * (j + k)));
                }

                endogenous[(i * EndogenousCount) + k] = x;
                response += k == 0 ? 1.5 * x : -0.7 * x;
            }

            y[i] = response;
            clusters[i] = i / RowsPerCluster;
        }

        return (y, exogenous, endogenous, instruments, clusters);
    }
}
