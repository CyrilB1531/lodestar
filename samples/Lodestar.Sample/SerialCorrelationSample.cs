using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>The autocorrelation function -- how a series echoes itself down the lags.</summary>
internal static class SerialCorrelationSample
{
    // A sawtooth trend: each pair climbs, then dips, so consecutive points track
    // each other closely and the correlation stays high for several lags.
    private static readonly double[] Series =
        [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

    public static void Run()
    {
        Console.WriteLine("Autocorrelation (Lodestar.Stats.TimeSeries)");

        AutocorrelationResult result = SerialCorrelation.Autocorrelation(Series, lagCount: 4);

        Console.WriteLine($"  r(1) / r(2)      : {Inv.F3(result.Values[1])} / {Inv.F3(result.Values[2])}");
        Console.WriteLine(
            $"  95% band at lag 1: [{Inv.F3(result.ConfidenceLower[1])}, {Inv.F3(result.ConfidenceUpper[1])}]");
        Console.WriteLine();
    }
}
