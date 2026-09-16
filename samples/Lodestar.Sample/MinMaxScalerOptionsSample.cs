using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>A range that is not the unit interval, and what clipping is for.</summary>
internal static class MinMaxScalerOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("MinMaxScalerOptions (Lodestar.Preprocessing)");

        double[] samples = [0.0, 2.0, 4.0];
        var options = new MinMaxScalerOptions { Low = -5.0, High = 3.0, Clip = true };
        MinMaxScaler scaler = MinMaxScaler.Fit(samples, featureCount: 1, options);

        Console.WriteLine($"  range            : [{Inv.F1(options.Low)}, {Inv.F1(options.High)}], clip {options.Clip}");
        Console.WriteLine($"  mapped           : {Inv.List(scaler.Transform(samples))}");

        // Clipping only shows on a value the fit never saw; the inverse never clips.
        Console.WriteLine($"  unseen 40        : {Inv.List(scaler.Transform([40.0]))}");
        Console.WriteLine($"  inverse of 6     : {Inv.List(scaler.InverseTransform([6.0]))}");
        Console.WriteLine();
    }
}
