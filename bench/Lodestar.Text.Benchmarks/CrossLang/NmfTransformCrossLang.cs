using Lodestar.Abstractions;
using Lodestar.Decomposition;

namespace Lodestar.Text.Benchmarks.CrossLang;

/// <summary>Cross-language throughput harness for <see cref="Nmf.Transform"/>, mirroring <c>bench/python/bench_nmf_transform.py</c>.</summary>
/// <remarks>
/// Same shapes and operations in the same order, over <see cref="Harness"/>'s best-of-N. No
/// corpus file: both sides build the matrix from the row index by the same rule, and
/// <c>decomposition_nmf.json</c> is what asserts they agree. The fit is outside the timed
/// region on both sides, so what this prices is the transform.
/// </remarks>
public static class NmfTransformCrossLang
{
    private const int Columns = 300;
    private const int NonZerosPerRow = 12;
    private const int FitRows = 1500;
    private const int Rank = 15;
    private const int Iterations = 40;

    private static readonly int[] Sizes = [100, 500, 2000];

    /// <summary>Runs every shape and writes <c>bench/results/csharp-nmf-transform.json</c>.</summary>
    public static void Run()
    {
        string outPath = Path.Combine(
            BenchCorpus.RepoRoot(), "bench", "results", "csharp-nmf-transform.json");
        var results = new List<Harness.OperationResult>();

        CsrMatrix fitMatrix = Corpus(FitRows, 0);

        foreach (NmfBetaLoss loss in (NmfBetaLoss[])[NmfBetaLoss.Frobenius, NmfBetaLoss.KullbackLeibler])
        {
            // The fit is outside the timed region, and it is what fixes H and the settings the
            // transform replays; only the transform below is measured.
            Nmf fitted = Nmf.Fit(
                fitMatrix, Rank,
                new NmfOptions { BetaLoss = loss, MaxIterations = Iterations, Tolerance = 0.0 });
            string tag = loss == NmfBetaLoss.Frobenius ? "frobenius" : "kl";
            foreach (int rows in Sizes)
            {
                CsrMatrix unseen = Corpus(rows, rows);
                results.Add(Harness.Measure($"transform_{tag}_n{rows}", () => fitted.Transform(unseen)));
            }
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

    /// <summary>A sparse non-negative corpus from the row index alone — the Python side's rule.</summary>
    private static CsrMatrix Corpus(int rows, int offset)
    {
        var values = new double[rows * NonZerosPerRow];
        var columnIndices = new int[rows * NonZerosPerRow];
        var rowPointers = new int[rows + 1];

        int cursor = 0;
        for (int row = 0; row < rows; row++)
        {
            int seed = row + offset;
            for (int j = 0; j < NonZerosPerRow; j++)
            {
                columnIndices[cursor] = ((seed * 31) + (j * 97)) % Columns;
                values[cursor] = 1.0 + (((seed + j) % 17) / 4.0);
                cursor++;
            }

            // A CSR row's indices must ascend, and the stride above can wrap past Columns.
            Array.Sort(columnIndices, values, cursor - NonZerosPerRow, NonZerosPerRow);
            rowPointers[row + 1] = cursor;
        }

        return new CsrMatrix(rows, Columns, values, columnIndices, rowPointers);
    }
}
