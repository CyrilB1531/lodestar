using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>Freireich's two arms, which is the comparison the test was published for.</summary>
internal static class LogRankSample
{
    public static void Run()
    {
        Console.WriteLine("Log-rank (Lodestar.Survival)");

        LogRankResult result = LogRank.Test(
            Trial.TreatmentDurations, Trial.TreatmentObserved,
            Trial.ControlDurations, Trial.ControlObserved);

        Console.WriteLine($"  statistic        : {Inv.F3(result.Statistic)}");
        Console.WriteLine($"  p                : {Inv.E3(result.PValue)}");

        // The Wilcoxon weighting counts the early deaths more, and treatment is weighed twice here.
        double[] twice = [.. Trial.TreatmentDurations.Select(_ => 2.0)];
        LogRankResult weighted = LogRank.Test(
            Trial.TreatmentDurations, Trial.TreatmentObserved, twice,
            Trial.ControlDurations, Trial.ControlObserved, [],
            new LogRankOptions { Weighting = LogRankWeighting.Wilcoxon });
        Console.WriteLine($"  Wilcoxon, weighted: {Inv.F3(weighted.Statistic)}");

        // Three groups at once, then two at a time.
        double[] durations = [5, 8, 12, 3, 9, 15, 4, 6, 20, 11, 7, 14];
        int[] groups = [1, 1, 1, 2, 2, 2, 3, 3, 3, 1, 2, 3];
        bool[] observed = [true, true, false, true, true, true, true, true, false, true, false, true];
        LogRankResult three = LogRank.MultiGroup(durations, groups, observed);
        LogRankResult threeWeighted = LogRank.MultiGroup(durations, groups, observed, [.. durations.Select(_ => 1.0)]);
        Console.WriteLine($"  three groups     : {Inv.F3(three.Statistic)} on {three.DegreesOfFreedom} df ({Inv.F3(threeWeighted.Statistic)} weighted by ones)");
        foreach (PairwiseLogRankResult pair in LogRank.Pairwise(durations, groups, observed))
        {
            Console.WriteLine($"  groups {pair.GroupA} and {pair.GroupB}   : {Inv.F3(pair.Result.Statistic)}");
        }
        Console.WriteLine();
    }
}
