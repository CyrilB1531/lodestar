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
        Console.WriteLine();
    }
}
