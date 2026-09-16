using Lodestar.Preprocessing;

namespace Lodestar.Text.Benchmarks.CrossLang;

/// <summary>
/// Cross-language throughput harness for the #762 splitters, mirroring
/// <c>bench/python/bench_splitters.py</c>: same shapes, same operations in the same order, same
/// auto-scaling best-of-N methodology via <see cref="Harness"/>.
/// </summary>
/// <remarks>
/// No corpus file: a splitter's input is a row count and a label per row, so both sides build the
/// labels from the row index by the same rule. Nothing is asserted to agree here either —
/// <c>tests/oracles/preprocessing_splitters.json</c> replays these folds index for index.
/// </remarks>
public static class SplittersCrossLang
{
    private const int FoldCount = 5;
    private const double TestFraction = 0.25;

    private static readonly int[] Sizes = [10_000, 100_000, 1_000_000];

    /// <summary>Runs every shape and writes <c>bench/results/csharp-splitters.json</c>.</summary>
    public static void Run()
    {
        string outPath = Path.Combine(BenchCorpus.RepoRoot(), "bench", "results", "csharp-splitters.json");
        var results = new List<Harness.OperationResult>();

        foreach (int n in Sizes)
        {
            int[] labels = Labels(n);
            string suffix = $"n{n}";
            results.Add(Harness.Measure($"kfold_{suffix}", () => Splitters.KFold(n, FoldCount)));
            results.Add(Harness.Measure($"stratified_{suffix}", () => Splitters.StratifiedKFold(labels, FoldCount)));
            results.Add(Harness.Measure($"traintest_{suffix}", () => Splitters.TrainTest(n, TestFraction)));
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

    /// <summary>Three classes at 60 / 30 / 10, from the row index alone — the Python side's rule.</summary>
    /// <remarks>Skewed rather than balanced: that is where a stratified splitter does work a plain one does not.</remarks>
    private static int[] Labels(int sampleCount)
    {
        var labels = new int[sampleCount];
        for (int row = 0; row < sampleCount; row++)
        {
            int remainder = row % 10;
            if (remainder < 6)
            {
                labels[row] = 0;
            }
            else if (remainder < 9)
            {
                labels[row] = 1;
            }
            else
            {
                labels[row] = 2;
            }
        }

        return labels;
    }
}
