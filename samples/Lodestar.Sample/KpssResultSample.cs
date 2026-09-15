using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>A KPSS p-value, and whether it is the end of its table.</summary>
internal static class KpssResultSample
{
    private static readonly double[] Walk =
        [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8, 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

    public static void Run()
    {
        Console.WriteLine("KPSS result (Lodestar.Stats.TimeSeries)");

        KpssResult result = Stationarity.Kpss(
            Walk, new KpssOptions { LagRule = KpssLagRule.Fixed, LagCount = 2 });

        // Past the 1 % critical value the table's smallest p-value comes back, and the bound says so.
        Console.WriteLine($"  statistic / p    : {Inv.F3(result.Statistic)} / {Inv.F3(result.PValue)}");
        Console.WriteLine($"  window / bound   : {result.LagCount} / {result.PValueBound}");
        Console.WriteLine($"  1% critical      : {Inv.F3(result.CriticalValues[3])}");
        Console.WriteLine($"  truth is smaller : {result.PValueBound == PValueBound.ActualIsSmaller}");
        Console.WriteLine();
    }
}
