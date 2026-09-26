using Lodestar.Survival;

namespace Lodestar.Text.Benchmarks.CrossLang;

/// <summary>The #1170 survival closed forms against <c>bench/python/bench_survival_family.py</c>: same subjects, operations and method.</summary>
/// <remarks>A weighted two-group test, the multi-group and pairwise tests, the restricted mean, a fixed-point test and the concordance index.</remarks>
public static class SurvivalFamilyCrossLang
{
    private const int Groups = 5;
    private const double Horizon = 50.0;

    private static readonly int[] Sizes = [1_000, 10_000, 100_000];

    /// <summary>Runs every shape and writes <c>bench/results/csharp-survival-family.json</c>.</summary>
    public static void Run()
    {
        string outPath = Path.Combine(BenchCorpus.RepoRoot(), "bench", "results", "csharp-survival-family.json");
        var results = new List<Harness.OperationResult>();
        var wilcoxon = new LogRankOptions { Weighting = LogRankWeighting.Wilcoxon };
        var late = new LogRankOptions { Weighting = LogRankWeighting.FlemingHarrington, P = 1.0, Q = 1.0 };
        foreach (int n in Sizes)
        {
            (double[] d, bool[] e, int[] g, double[] s) = Subjects(n);
            (double[] da, bool[] ea, double[] wa) = Group(d, e, g, 0);
            (double[] db, bool[] eb, double[] wb) = Group(d, e, g, 1);
            results.Add(Harness.Measure($"logrank_wilcoxon_weighted_{n}", () => LogRank.Test(da, ea, wa, db, eb, wb, wilcoxon)));
            results.Add(Harness.Measure($"logrank_fleming_harrington_{n}", () => LogRank.Test(da, ea, db, eb, late)));
            results.Add(Harness.Measure($"multigroup_{n}", () => LogRank.MultiGroup(d, g, e)));
            results.Add(Harness.Measure($"pairwise_{n}", () => LogRank.Pairwise(d, g, e)));
            results.Add(Harness.Measure($"rmst_{n}", () => KaplanMeier.RestrictedMean(KaplanMeier.Estimate(d, e), Horizon)));
            results.Add(Harness.Measure(
                $"fixed_point_{n}", () => KaplanMeier.CompareAt(Horizon, KaplanMeier.Estimate(da, ea), KaplanMeier.Estimate(db, eb))));
            results.Add(Harness.Measure($"concordance_{n}", () => Concordance.Index(d, s, e)));
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

    /// <summary>The Python side's formula: durations, events, groups and scores from the index alone.</summary>
    private static (double[] Durations, bool[] Events, int[] Groups, double[] Scores) Subjects(int n)
    {
        var durations = new double[n];
        var events = new bool[n];
        var groups = new int[n];
        var scores = new double[n];
        for (long i = 0; i < n; i++)
        {
            durations[i] = (((i * 2654435761) % 10007) / 100.0) + 0.01;
            events[i] = (i * 37) % 10 < 7;
            groups[i] = (int)(i % Groups);
            scores[i] = (((i * 40503) % 1009) / 10.0) + durations[i];
        }

        return (durations, events, groups, scores);
    }

    /// <summary>One group's subjects, with the weights one to three the Python side gives them by index.</summary>
    private static (double[] Durations, bool[] Events, double[] Weights) Group(double[] d, bool[] e, int[] g, int label)
    {
        var durations = new List<double>();
        var events = new List<bool>();
        var weights = new List<double>();
        for (int i = 0; i < d.Length; i++)
        {
            if (g[i] == label)
            {
                durations.Add(d[i]);
                events.Add(e[i]);
                weights.Add(1.0 + (i % 3));
            }
        }

        return ([.. durations], [.. events], [.. weights]);
    }
}
