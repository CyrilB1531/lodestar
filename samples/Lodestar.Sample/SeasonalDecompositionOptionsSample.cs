using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>The model, the filter's sides and the extrapolated trend ends.</summary>
internal static class SeasonalDecompositionOptionsSample
{
    private static readonly double[] Quarterly = [10.0, 14.0, 8.0, 12.0, 11.0, 15.0, 9.0, 13.0, 12.0, 16.0, 10.0, 14.0];

    public static void Run()
    {
        Console.WriteLine("Seasonal decomposition options (Lodestar.Stats.TimeSeries)");

        var options = new SeasonalDecompositionOptions
        {
            Model = SeasonalModel.Multiplicative,
            TwoSided = true,
            ExtrapolateTrend = 1,
        };
        SeasonalComponents parts = SeasonalDecomposition.Decompose(Quarterly, 4, options);

        Console.WriteLine($"  {options.Model}, two-sided {options.TwoSided}, extrapolate {options.ExtrapolateTrend}");
        Console.WriteLine($"  first trend      : {Inv.F3(parts.Trend[0])}");
        Console.WriteLine();
    }
}
