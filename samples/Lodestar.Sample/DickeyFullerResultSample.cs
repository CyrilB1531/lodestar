using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>What an augmented Dickey-Fuller test reports beside its p-value.</summary>
internal static class DickeyFullerResultSample
{
    private static readonly double[] Walk =
        [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8, 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

    public static void Run()
    {
        Console.WriteLine("Dickey-Fuller result (Lodestar.Stats.TimeSeries)");

        DickeyFullerResult result = Stationarity.AugmentedDickeyFuller(Walk);

        Console.WriteLine($"  statistic / p    : {Inv.F3(result.Statistic)} / {Inv.F3(result.PValue)}");
        Console.WriteLine($"  lag / rows       : {result.UsedLag} / {result.ObservationCount}");
        Console.WriteLine($"  5% critical     : {Inv.F3(result.CriticalValues[1])}");
        Console.WriteLine($"  Akaike           : {Inv.F3(result.InformationCriterion)}");
        Console.WriteLine();
    }
}
