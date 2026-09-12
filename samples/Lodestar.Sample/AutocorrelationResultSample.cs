using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>A correlogram: the value at each lag, bracketed by its own band.</summary>
internal static class AutocorrelationResultSample
{
    private static readonly double[] Series =
        [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

    public static void Run()
    {
        Console.WriteLine("The autocorrelation result (Lodestar.Stats.TimeSeries)");

        AutocorrelationResult result = SerialCorrelation.Autocorrelation(Series, lagCount: 4);

        for (int lag = 0; lag < result.Values.Count; lag++)
        {
            Console.WriteLine(
                $"  lag {lag}: {Inv.F3(result.Values[lag])}  "
                + $"[{Inv.F3(result.ConfidenceLower[lag])}, {Inv.F3(result.ConfidenceUpper[lag])}]");
        }

        // The same result type, produced by the partial function instead: the shorter
        // lags' influence on the longer ones is removed before this is reported.
        AutocorrelationResult partial = SerialCorrelation.PartialAutocorrelation(Series, lagCount: 4);
        Console.WriteLine($"  partial lag 1: {Inv.F3(partial.Values[1])}");

        Console.WriteLine();
    }
}
