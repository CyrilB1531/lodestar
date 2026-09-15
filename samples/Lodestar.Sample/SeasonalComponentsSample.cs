using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>The three components add back to the series wherever the trend is defined.</summary>
internal static class SeasonalComponentsSample
{
    private static readonly double[] Quarterly = [10.0, 14.0, 8.0, 12.0, 11.0, 15.0, 9.0, 13.0, 12.0, 16.0, 10.0, 14.0];

    public static void Run()
    {
        Console.WriteLine("Seasonal components (Lodestar.Stats.TimeSeries)");

        SeasonalComponents parts = SeasonalDecomposition.Decompose(Quarterly, period: 4);
        double rebuilt = parts.Trend[5] + parts.Seasonal[5] + parts.Residual[5];

        Console.WriteLine($"  rebuilt Q2, yr 2 : {Inv.F3(rebuilt)} (observed {Inv.F3(Quarterly[5])})");
        Console.WriteLine();
    }
}
