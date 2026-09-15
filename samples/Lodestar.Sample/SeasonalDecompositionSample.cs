using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>Three years of quarterly figures, split into trend and season.</summary>
internal static class SeasonalDecompositionSample
{
    private static readonly double[] Quarterly = [10.0, 14.0, 8.0, 12.0, 11.0, 15.0, 9.0, 13.0, 12.0, 16.0, 10.0, 14.0];

    public static void Run()
    {
        Console.WriteLine("Seasonal decomposition (Lodestar.Stats.TimeSeries)");

        SeasonalComponents parts = SeasonalDecomposition.Decompose(Quarterly, period: 4);

        Console.WriteLine($"  trend at Q3      : {Inv.F3(parts.Trend[2])}");
        Console.WriteLine($"  season at Q2     : {Inv.F3(parts.Seasonal[1])}");
        Console.WriteLine();
    }
}
