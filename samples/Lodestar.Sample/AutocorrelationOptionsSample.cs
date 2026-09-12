using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>The estimator, the band shape, and the level -- three independent choices.</summary>
internal static class AutocorrelationOptionsSample
{
    private static readonly double[] Series =
        [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

    public static void Run()
    {
        Console.WriteLine("Autocorrelation options (Lodestar.Stats.TimeSeries)");

        AutocorrelationResult biased = SerialCorrelation.Autocorrelation(Series, lagCount: 3);
        AutocorrelationResult adjusted = SerialCorrelation.Autocorrelation(
            Series, lagCount: 3, new AutocorrelationOptions { Adjusted = true });

        // The adjusted estimator divides by a smaller denominator at every lag past
        // zero, so it reports a slightly larger correlation than the biased default.
        Console.WriteLine($"  r(3) biased      : {Inv.F3(biased.Values[3])}");
        Console.WriteLine($"  r(3) adjusted    : {Inv.F3(adjusted.Values[3])}");

        AutocorrelationResult flatBand = SerialCorrelation.Autocorrelation(
            Series, lagCount: 3, new AutocorrelationOptions { BartlettConfidenceInterval = false });
        AutocorrelationResult narrower = SerialCorrelation.Autocorrelation(
            Series, lagCount: 3, new AutocorrelationOptions { ConfidenceLevel = 0.80 });

        Console.WriteLine(
            $"  flat band width  : {Inv.F3(flatBand.ConfidenceUpper[1] - flatBand.ConfidenceLower[1])}");
        Console.WriteLine(
            $"  80% band width   : {Inv.F3(narrower.ConfidenceUpper[1] - narrower.ConfidenceLower[1])}");
        Console.WriteLine();
    }
}
