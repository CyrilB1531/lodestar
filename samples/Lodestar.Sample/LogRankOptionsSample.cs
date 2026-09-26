using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>A late-weighted test, stopped at a truncation.</summary>
internal static class LogRankOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("LogRankOptions (Lodestar.Survival)");

        // Fleming-Harrington with q = 1 weighs the late times; nothing after week 20 counts as an event.
        var late = new LogRankOptions { Weighting = LogRankWeighting.FlemingHarrington, P = 0.0, Q = 1.0, Truncation = 20.0 };
        LogRankResult result = LogRank.Test(
            Trial.TreatmentDurations, Trial.TreatmentObserved, Trial.ControlDurations, Trial.ControlObserved, late);

        Console.WriteLine($"  {late.Weighting}, p {Inv.F1(late.P)}, q {Inv.F1(late.Q)}, until {Inv.F1(late.Truncation ?? 0.0)}");
        Console.WriteLine($"  statistic        : {Inv.F3(result.Statistic)}");
        Console.WriteLine();
    }
}
