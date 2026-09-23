using Lodestar.Fuzzy;

namespace Lodestar.Text.Benchmarks.CrossLang;

/// <summary>
/// Cross-language throughput harness for <see cref="Process.Cdist"/>, mirroring
/// <c>bench/python/bench_cdist.py</c>: same shapes, same operations in the same order, same
/// auto-scaling best-of-N methodology via <see cref="Harness"/>.
/// </summary>
/// <remarks>
/// No corpus file: both sides build the phrases from the row index by the same rule, so a
/// committed corpus here would be a file holding <c>word[i mod 12]</c> three times and an index.
/// Nothing is asserted to agree — <c>tests/oracles/process_cdist.json</c> replays the scores.
/// </remarks>
public static class CdistCrossLang
{
    private static readonly string[] Words =
    [
        "new", "york", "boston", "atlanta", "brooklyn", "angeles", "lakers", "mets", "braves",
        "red", "sox", "knicks",
    ];

    private static readonly int[] Sizes = [50, 200, 500];

    /// <summary>Runs every shape and writes <c>bench/results/csharp-cdist.json</c>.</summary>
    public static void Run()
    {
        string outPath = Path.Combine(BenchCorpus.RepoRoot(), "bench", "results", "csharp-cdist.json");
        var results = new List<Harness.OperationResult>();

        foreach (int n in Sizes)
        {
            string[] queries = Phrases(n, 0);
            string[] choices = Phrases(n, 7);
            string suffix = $"n{n}";

            results.Add(Harness.Measure($"cdist_ratio_{suffix}", () => Process.Cdist(queries, choices)));
            results.Add(Harness.Measure(
                $"cdist_wratio_{suffix}", () => Process.Cdist(queries, choices, Fuzz.WRatio)));

            // A cutoff prunes on both sides, so the pair above is only half the comparison, and
            // three-word phrases give either side little to prune (#1134).
            string[] wideQueries = Varied(n, 0);
            string[] wideChoices = Varied(n, 7);

            results.Add(Harness.Measure(
                $"cdist_ratio_varied_{suffix}", () => Process.Cdist(wideQueries, wideChoices)));
            results.Add(Harness.Measure(
                $"cdist_ratio_varied_cutoff90_{suffix}",
                () => Process.Cdist(wideQueries, wideChoices, scorer: null, 90.0)));
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

    /// <summary>Three words and an index, from the row index alone — the Python side's rule.</summary>
    /// <remarks>
    /// The trailing index keeps every phrase distinct, so no pair scores 100 by accident and the
    /// scorer does the same work on both sides.
    /// </remarks>
    private static string[] Phrases(int count, int offset)
    {
        var phrases = new string[count];
        for (int i = 0; i < count; i++)
        {
            int k = i + offset;
            phrases[i] = $"{Words[k % Words.Length]} {Words[(k * 5) % Words.Length]} " +
                $"{Words[(k * 7) % Words.Length]} {i}";
        }

        return phrases;
    }

    /// <summary>One to six words and an index, from the row index alone — the Python side's rule.</summary>
    /// <remarks>
    /// The spread is the point: a length bound rejects a pair only where the lengths differ, and
    /// the three-word rule above holds every phrase to within a word of every other.
    /// </remarks>
    private static string[] Varied(int count, int offset)
    {
        var phrases = new string[count];
        for (int i = 0; i < count; i++)
        {
            int k = i + offset;
            int words = 1 + ((k * 3) % 6);
            var phrase = new System.Text.StringBuilder();
            for (int w = 0; w < words; w++)
            {
                phrase.Append(Words[(k * (w + 1)) % Words.Length]).Append(' ');
            }

            phrases[i] = phrase.Append(i.ToString(System.Globalization.CultureInfo.InvariantCulture)).ToString();
        }

        return phrases;
    }
}
