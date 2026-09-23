using Lodestar.Preprocessing;

namespace Lodestar.Text.Benchmarks.CrossLang;

/// <summary>
/// Cross-language throughput harness for the #1122 transformers, mirroring
/// <c>bench/python/bench_transformers.py</c>: same shapes, same operations in the same order,
/// same auto-scaling best-of-N methodology via <see cref="Harness"/>.
/// </summary>
/// <remarks>
/// No corpus file: the matrix is built from the row index by a rule both sides spell out, so a
/// committed corpus here would be a file holding <c>exp(i mod 97 / 24)</c>. Nothing is asserted to
/// agree — <c>tests/oracles/preprocessing_*.json</c> replay these seven against the reference.
/// </remarks>
public static class TransformersCrossLang
{
    private const int Features = 4;

    /// <summary>Quadratic in the rows, so the imputer reads a fixed slice at every size.</summary>
    private const int ImputerRows = 2_000;

    private static readonly int[] Sizes = [10_000, 100_000];

    /// <summary>Runs every shape and writes <c>bench/results/csharp-transformers.json</c>.</summary>
    public static void Run()
    {
        string outPath = Path.Combine(BenchCorpus.RepoRoot(), "bench", "results", "csharp-transformers.json");
        var results = new List<Harness.OperationResult>();

        foreach (int n in Sizes)
        {
            double[] matrix = Matrix(n);
            double[] gaps = WithGaps(ImputerRows);
            KBinsDiscretizer bins = KBinsDiscretizer.Fit(matrix, Features);
            QuantileTransformer quantiles = QuantileTransformer.Fit(matrix, Features);
            PowerTransformer power = PowerTransformer.Fit(matrix, Features);
            KnnImputer imputer = KnnImputer.Fit(gaps, Features);
            string[] labels = Labels(n);
            string suffix = $"n{n}";

            results.Add(Harness.Measure($"normalize_{suffix}", () => Normalizer.Transform(matrix, Features)));
            results.Add(Harness.Measure($"polynomial_{suffix}", () => PolynomialFeatures.Transform(matrix, Features)));
            results.Add(Harness.Measure($"kbins_fit_{suffix}", () => KBinsDiscretizer.Fit(matrix, Features)));
            results.Add(Harness.Measure($"kbins_{suffix}", () => bins.Transform(matrix)));
            results.Add(Harness.Measure($"quantile_fit_{suffix}", () => QuantileTransformer.Fit(matrix, Features)));
            results.Add(Harness.Measure($"quantile_{suffix}", () => quantiles.Transform(matrix)));
            results.Add(Harness.Measure($"power_fit_{suffix}", () => PowerTransformer.Fit(matrix, Features)));
            results.Add(Harness.Measure($"power_{suffix}", () => power.Transform(matrix)));
            results.Add(Harness.Measure($"knn_impute_{suffix}", () => imputer.Transform(gaps)));
            results.Add(Harness.Measure($"label_{suffix}", () => Encoders.Label<string>(labels).Transform(labels)));
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

    /// <summary>A strictly positive, right-skewed matrix from the row index — the Python side's rule.</summary>
    /// <remarks>
    /// Skewed rather than uniform because that is the column a power or a quantile map is called
    /// on; strictly positive because Box-Cox refuses anything else, so one matrix serves both.
    /// </remarks>
    private static double[] Matrix(int sampleCount)
    {
        var matrix = new double[sampleCount * Features];
        for (int i = 0; i < matrix.Length; i++)
        {
            matrix[i] = Math.Exp((i % 97) / 24.0);
        }

        return matrix;
    }

    /// <summary>Fifty classes from the row index — the Python side's rule.</summary>
    /// <remarks>Fifty rather than two: a label encoder's cost is the distinct count, not the row count.</remarks>
    private static string[] Labels(int sampleCount)
    {
        var labels = new string[sampleCount];
        for (int row = 0; row < sampleCount; row++)
        {
            labels[row] = "c" + (row % 50).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        return labels;
    }

    /// <summary>The same rule with one value in twenty missing, which is what an imputer is called on.</summary>
    private static double[] WithGaps(int sampleCount)
    {
        double[] matrix = Matrix(sampleCount);
        for (int i = 0; i < matrix.Length; i++)
        {
            if (i % 20 == 0)
            {
                matrix[i] = double.NaN;
            }
        }

        return matrix;
    }
}
