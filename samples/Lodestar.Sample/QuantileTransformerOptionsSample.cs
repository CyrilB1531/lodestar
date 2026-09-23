using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>How many quantiles are fitted, and what they map onto.</summary>
internal static class QuantileTransformerOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("QuantileTransformerOptions (Lodestar.Preprocessing)");

        double[] skew = [1.0, 1.0, 2.0, 3.0, 5.0, 8.0, 13.0, 21.0, 34.0, 100.0];

        // A coarse fit is a piecewise-linear approximation of the distribution.
        var coarse = new QuantileTransformerOptions { QuantileCount = 5 };
        QuantileTransformer approximate = QuantileTransformer.Fit(skew, 1, coarse);
        Console.WriteLine($"  {coarse.QuantileCount} levels        : {Inv.List(approximate.References)}");
        Console.WriteLine($"  at those levels  : {Inv.List(approximate.Quantiles[0])}");

        // The normal output stops near +/-5.2: the quantile of 0 is infinite, so it is clipped.
        var normal = new QuantileTransformerOptions { Output = QuantileOutput.Normal };
        Console.WriteLine($"  {normal.Output} ends     : {Inv.List(QuantileTransformer.Fit(skew, 1, normal).Transform([1.0, 100.0]))}");
        Console.WriteLine();
    }
}
