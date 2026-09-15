using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>The two stationarity tests, whose nulls are opposite, run side by side.</summary>
internal static class StationaritySample
{
    // A series drifting upwards: ADF cannot reject a unit root, and KPSS rejects stationarity.
    private static readonly double[] Walk =
        [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8, 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

    public static void Run()
    {
        Console.WriteLine("Stationarity (Lodestar.Stats.TimeSeries)");

        DickeyFullerResult adf = Stationarity.AugmentedDickeyFuller(Walk);
        KpssResult kpss = Stationarity.Kpss(Walk);

        Console.WriteLine($"  ADF p            : {Inv.F3(adf.PValue)}");
        Console.WriteLine($"  KPSS p           : {Inv.F3(kpss.PValue)}");
        Console.WriteLine();
    }
}
