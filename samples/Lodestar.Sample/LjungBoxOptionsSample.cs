using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>The model's parameter count, and whether Box-Pierce comes beside Ljung-Box.</summary>
internal static class LjungBoxOptionsSample
{
    private static readonly double[] Series =
        [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

    public static void Run()
    {
        Console.WriteLine("Ljung-Box options (Lodestar.Stats.TimeSeries)");

        LjungBoxResult raw = SerialCorrelation.LjungBox(Series, lagCount: 4);
        LjungBoxResult residuals = SerialCorrelation.LjungBox(
            Series, lagCount: 4, new LjungBoxOptions { ModelDegreesOfFreedom = 2 });

        // The same lag costs two degrees of freedom against a fitted model, so its p-value
        // moves even though the statistic itself does not.
        Console.WriteLine($"  raw p(4)         : {Inv.F3(raw.PValues[3])}");
        Console.WriteLine($"  residual p(4)    : {Inv.F3(residuals.PValues[3])}");

        LjungBoxResult withBoxPierce = SerialCorrelation.LjungBox(
            Series, lagCount: 4, new LjungBoxOptions { BoxPierce = true });

        Console.WriteLine($"  Ljung-Box(4)     : {Inv.F3(withBoxPierce.Statistics[3])}");
        Console.WriteLine($"  Box-Pierce(4)    : {Inv.F3(withBoxPierce.BoxPierceStatistics[3])}");
        Console.WriteLine();
    }
}
