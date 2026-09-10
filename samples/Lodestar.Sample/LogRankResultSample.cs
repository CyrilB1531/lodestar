using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>The record a log-rank test comes back in.</summary>
internal static class LogRankResultSample
{
    public static void Run()
    {
        Console.WriteLine("LogRankResult (Lodestar.Survival)");

        // Identical arms cannot separate, so the statistic is exactly zero.
        LogRankResult same = LogRank.Test(
            [2.0, 4.0, 6.0], [true, true, true],
            [2.0, 4.0, 6.0], [true, true, true]);

        Console.WriteLine($"  statistic        : {Inv.F1(same.Statistic)}");
        Console.WriteLine($"  p                : {Inv.F1(same.PValue)}");
        Console.WriteLine($"  degrees of freedom: {same.DegreesOfFreedom}");
        Console.WriteLine();
    }
}
