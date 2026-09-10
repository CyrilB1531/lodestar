using System.Text.Json;
using Lodestar.Stats;
using Lodestar.Stats.Regression;

namespace Lodestar.Text.Benchmarks.CrossLang;

/// <summary>
/// Lodestar.Stats and Lodestar.Stats.Regression against scipy and statsmodels (issue #595).
/// </summary>
/// <remarks>
/// Mirrors <c>bench/python/bench_stats.py</c> over the corpus
/// <c>bench/corpus/generate_stats.py</c> writes. Section 21 of <c>bench/README.md</c> has the
/// methodology, the corpus decisions and why the Accord comparison stays beside this one.
/// </remarks>
public static class StatsCrossLang
{
    private static readonly int[] Sizes = [1_000, 10_000, 100_000];

    /// <summary>The three hypothesis tests, against <c>scipy.stats</c>.</summary>
    public static void Run(string[] args) => Measure(args, "stats", Tests);

    /// <summary>The OLS summary and its VIFs, against <c>statsmodels</c>.</summary>
    public static void RunOls(string[] args) => Measure(args, "ols", Regression);

    private static void Measure(
        string[] args, string bench, Func<Corpus, string, List<Harness.OperationResult>> measure)
    {
        ArgumentNullException.ThrowIfNull(args);

        string[] sizes = Filter(args, "--sizes");
        string root = BenchCorpus.RepoRoot();
        string corpusDir = Path.Combine(root, "bench", "corpus", "stats");
        string outPath = Path.Combine(root, "bench", "results", $"csharp-{bench}.json");

        var results = new List<Harness.OperationResult>();
        foreach (int n in Sizes)
        {
            // Skipped before the corpus file is read: the largest is 30 MB of JSON,
            // more than a skipped measurement is worth deserializing.
            if (sizes.Length > 0 && Array.IndexOf(sizes, n.ToString(System.Globalization.CultureInfo.InvariantCulture)) < 0)
            {
                continue;
            }

            results.AddRange(measure(Load(corpusDir, n), $"n{n}"));
        }

        var payload = new Harness.Output
        {
            Metadata = new Harness.OutputMetadata
            {
                Side = "csharp",
                Library = "Lodestar",
                Runtime = Environment.Version.ToString(),
                Os = Environment.OSVersion.ToString(),
                MinTimeS = Harness.MinTimeSeconds,
                Repeats = Harness.RepeatCount,
                Filtered = sizes.Length == 0 ? null : $"sizes=[{string.Join(",", sizes)}]",
            },
            Results = results,
        };

        Harness.Write(outPath, payload);
    }

    private static List<Harness.OperationResult> Tests(Corpus corpus, string suffix) =>
    [
        Harness.Measure($"welch_t_{suffix}", () => TTest.Independent(corpus.First, corpus.Second)),
        Harness.Measure($"mann_whitney_{suffix}", () => MannWhitney.Test(corpus.First, corpus.Second)),
        Harness.Measure($"chi_square_{suffix}", () => ChiSquare.Contingency(corpus.Table)),
    ];

    private static List<Harness.OperationResult> Regression(Corpus corpus, string suffix) =>
    [
        // long-comment: the asymmetry between the two libraries is the whole reason
        // bench/compare.py folds two Python rows into one, and it is not guessable.
        // One row, not two. Fit computes the coefficients, their errors, the tails, the
        // intervals AND the VIFs in the one call, where statsmodels leaves the VIF outside
        // .fit() -- so bench_stats.py splits its side into ols_summary_* and ols_vif_*, and
        // bench/compare.py sums those two before dividing. Splitting this side to match
        // would mean fitting twice and pricing work no caller pays for.
        Harness.Measure(
            $"ols_summary_{suffix}",
            () => OrdinaryLeastSquares.Fit(corpus.Design, corpus.Response, corpus.Regressors)),
    ];

    /// <summary>Reads the comma-separated values of <paramref name="option"/>, or an empty array if it is absent.</summary>
    private static string[] Filter(string[] args, string option)
    {
        int index = Array.IndexOf(args, option);
        return index >= 0 && index + 1 < args.Length
            ? args[index + 1].Split(',', StringSplitOptions.RemoveEmptyEntries)
            : [];
    }

    private static Corpus Load(string corpusDir, int n)
    {
        string path = Path.Combine(corpusDir, $"stats_n{n}.json");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                $"The benchmark corpus is missing '{path}'. Generate it first: "
                + "python bench/corpus/generate_stats.py",
                path);
        }

        CorpusFile file = JsonSerializer.Deserialize<CorpusFile>(File.ReadAllBytes(path))
            ?? throw new InvalidOperationException($"'{path}' held no corpus.");

        return new Corpus(
            file.First, file.Second, Counts(file.Table), file.Design, file.Response, file.Regressors);
    }

    /// <summary>The contingency table as the jagged double rows <c>ChiSquare</c> takes.</summary>
    private static double[][] Counts(int[][] rows)
    {
        var table = new double[rows.Length][];
        for (int row = 0; row < rows.Length; row++)
        {
            table[row] = Array.ConvertAll(rows[row], count => (double)count);
        }

        return table;
    }

    private sealed record Corpus(
        double[] First, double[] Second, double[][] Table, double[] Design, double[] Response, int Regressors);

    private sealed record CorpusFile
    {
        [System.Text.Json.Serialization.JsonPropertyName("first")]
        public double[] First { get; init; } = [];

        [System.Text.Json.Serialization.JsonPropertyName("second")]
        public double[] Second { get; init; } = [];

        [System.Text.Json.Serialization.JsonPropertyName("table")]
        public int[][] Table { get; init; } = [];

        [System.Text.Json.Serialization.JsonPropertyName("design")]
        public double[] Design { get; init; } = [];

        [System.Text.Json.Serialization.JsonPropertyName("response")]
        public double[] Response { get; init; } = [];

        [System.Text.Json.Serialization.JsonPropertyName("regressors")]
        public int Regressors { get; init; }
    }
}
