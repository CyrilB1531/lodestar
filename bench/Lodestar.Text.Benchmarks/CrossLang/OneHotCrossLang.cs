using Lodestar.Preprocessing;

namespace Lodestar.Text.Benchmarks.CrossLang;

/// <summary>The #1161 sparse one-hot encoding against <c>bench/python/bench_onehot.py</c>: same categories, operations and method.</summary>
/// <remarks>Fit, then the sparse transform of the fitted rows, with and without the infrequent tail grouped.</remarks>
public static class OneHotCrossLang
{
    private const int Features = 3;

    private static readonly int[] Sizes = [10_000, 100_000];

    /// <summary>Runs every shape and writes <c>bench/results/csharp-onehot.json</c>.</summary>
    public static void Run()
    {
        string outPath = Path.Combine(BenchCorpus.RepoRoot(), "bench", "results", "csharp-onehot.json");
        var results = new List<Harness.OperationResult>();
        var grouped = new OneHotEncoderOptions { MinFrequency = 5, MaxCategories = 200 };
        foreach (int n in Sizes)
        {
            int[] values = Categories(n);
            results.Add(Harness.Measure($"sparse_{n}", () => Encoders.OneHot<int>(values, Features).TransformSparse(values)));
            results.Add(Harness.Measure(
                $"sparse_infrequent_{n}", () => Encoders.OneHot<int>(values, Features, grouped).TransformSparse(values)));
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

    /// <summary>The Python side's formula: a hashed index folded twice, so small codes are far more frequent.</summary>
    private static int[] Categories(int n)
    {
        var values = new int[n * Features];
        for (long i = 0; i < n; i++)
        {
            for (long j = 0; j < Features; j++)
            {
                long spread = ((i * 7919) + (j * 104729)) % 9973;
                values[(i * Features) + j] = (int)(spread % (1 + (((i * 31) + j) % 997)));
            }
        }

        return values;
    }
}
