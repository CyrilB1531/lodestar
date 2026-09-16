using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Clipping a value the fit never saw into the unit interval.</summary>
internal static class MaxAbsScalerOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("MaxAbsScalerOptions (Lodestar.Preprocessing)");

        var options = new MaxAbsScalerOptions { Clip = true };
        MaxAbsScaler scaler = MaxAbsScaler.Fit([1.0, 2.0], featureCount: 1, options);

        Console.WriteLine($"  clip             : {options.Clip}");
        Console.WriteLine($"  unseen 8         : {Inv.List(scaler.Transform([8.0]))}");
        Console.WriteLine($"  inverse of 1     : {Inv.List(scaler.InverseTransform([1.0]))}");
        Console.WriteLine();
    }
}
