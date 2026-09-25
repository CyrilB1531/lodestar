using Lodestar.Conformal;

namespace Lodestar.Text.Benchmarks.CrossLang;

/// <summary>The #1159 intervals against <c>bench/python/bench_cross_conformal.py</c>: same shapes, predictions and method.</summary>
/// <remarks>
/// Each model is a formula of the row index shifted by the mean index it was fitted on, and K-fold runs unshuffled, so
/// this side knows each fold's model without a shared file. Timed: the interval at every test point, from predictions
/// computed outside the timed region, since this package takes predictions where MAPIE calls the models.
/// </remarks>
public static class CrossConformalCrossLang
{
    private const int Folds = 10;
    private const int Bootstraps = 30;
    private const int Tests = 500;
    private const double Alpha = 0.1;

    private static readonly int[] Sizes = [1_000, 10_000];

    /// <summary>Runs every shape and writes <c>bench/results/csharp-cross-conformal.json</c>.</summary>
    public static void Run()
    {
        string outPath = Path.Combine(BenchCorpus.RepoRoot(), "bench", "results", "csharp-cross-conformal.json");
        var results = new List<Harness.OperationResult>();
        foreach (int n in Sizes)
        {
            (int[] folds, double[] shifts) = KFold(n);
            double[] y = [.. Enumerable.Range(0, n).Select(i => Base(i) + (2.0 * Math.Sin(1.3 * i)))];
            double[] outOfFold = [.. Enumerable.Range(0, n).Select(i => Base(i) + shifts[folds[i]])];
            double[] absolute = SplitConformal.AbsoluteResiduals(y, outOfFold);
            double[] gamma = SplitConformal.GammaScores(y, outOfFold);
            double[] atTest = TestPredictions(n, shifts);

            (bool[] heldOut, double[] bagShifts) = Bootstrap(n);
            double[] train = [.. Enumerable.Range(0, n * Bootstraps).Select(k =>
            {
                int row = k / Bootstraps;
                return Base(row) + bagShifts[k % Bootstraps];
            })];
            double[] bagScores = SplitConformal.AbsoluteResiduals(y, CrossConformal.OutOfSample(train, heldOut, Bootstraps));
            double[] bagTest = TestPredictions(n, bagShifts);

            string suffix = $"n{n}";
            results.Add(Harness.Measure($"cv_plus_{suffix}", () => Each(atTest, Folds, row => CrossConformal.Interval(row, folds, absolute, Alpha))));
            results.Add(Harness.Measure($"cv_minmax_{suffix}", () => Each(
                atTest, Folds, row => CrossConformal.Interval(row, folds, absolute, Alpha, CrossConformalMethod.MinMax))));
            results.Add(Harness.Measure($"cv_plus_gamma_{suffix}", () => Each(atTest, Folds, row => CrossConformal.GammaInterval(row, folds, gamma, Alpha))));
            results.Add(Harness.Measure($"bootstrap_plus_{suffix}", () => Each(bagTest, Bootstraps, row => CrossConformal.Interval(row, heldOut, bagScores, Alpha))));
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

    private delegate (double Lower, double Upper) RowInterval(ReadOnlySpan<double> row);

    private static double[] Each(double[] predictions, int models, RowInterval interval)
    {
        var bounds = new double[2 * Tests];
        for (int t = 0; t < Tests; t++)
        {
            (bounds[2 * t], bounds[(2 * t) + 1]) = interval(predictions.AsSpan(t * models, models));
        }

        return bounds;
    }

    private static double Base(double index) => 50.0 + (10.0 * Math.Sin(0.01 * index));

    /// <summary>scikit-learn's unshuffled K-fold, and each fold's model shift: the mean training index over the training size.</summary>
    private static (int[] Folds, double[] Shifts) KFold(int n)
    {
        var folds = new int[n];
        var shifts = new double[Folds];
        int start = 0;
        for (int k = 0; k < Folds; k++)
        {
            int size = (n / Folds) + (k < n % Folds ? 1 : 0);
            double held = 0.0;
            for (int i = start; i < start + size; i++)
            {
                folds[i] = k;
                held += i;
            }

            double total = (n - 1.0) * n / 2.0;
            shifts[k] = (total - held) / (n - size) / (n - size);
            start += size;
        }

        return (folds, shifts);
    }

    /// <summary>A bag per model by formula, about a third of the samples out of each, and each model's shift.</summary>
    private static (bool[] HeldOut, double[] Shifts) Bootstrap(int n)
    {
        var heldOut = new bool[n * Bootstraps];
        var shifts = new double[Bootstraps];
        for (int m = 0; m < Bootstraps; m++)
        {
            shifts[m] = 0.5 + (0.01 * m);
            for (int i = 0; i < n; i++)
            {
                heldOut[(i * Bootstraps) + m] = ((i * 7) + (m * 13)) % 11 < 4;
            }
        }

        return (heldOut, shifts);
    }

    private static double[] TestPredictions(int n, double[] shifts)
    {
        var predictions = new double[Tests * shifts.Length];
        for (int t = 0; t < Tests; t++)
        {
            for (int m = 0; m < shifts.Length; m++)
            {
                predictions[(t * shifts.Length) + m] = Base(n + t) + shifts[m];
            }
        }

        return predictions;
    }
}
