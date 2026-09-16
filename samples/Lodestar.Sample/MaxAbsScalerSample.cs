using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Scaling without ever subtracting, which is what keeps a zero a zero.</summary>
internal static class MaxAbsScalerSample
{
    public static void Run()
    {
        Console.WriteLine("Max-abs scaling (Lodestar.Preprocessing)");

        // Two features; the second is negative throughout, so its largest absolute
        // value is not its largest value.
        double[] samples = [2.0, -4.0, 1.0, -8.0];

        MaxAbsScaler scaler = MaxAbsScaler.Fit(samples, featureCount: 2);
        Console.WriteLine($"  fitted on        : {scaler.SampleCount} rows x {scaler.FeatureCount} features");
        Console.WriteLine($"  largest absolute : {Inv.List(scaler.MaximumAbsolute)}");
        Console.WriteLine($"  scale            : {Inv.List(scaler.Scale)}");

        double[] scaled = scaler.Transform(samples);
        Console.WriteLine($"  scaled           : {Inv.List(scaled)}");
        Console.WriteLine($"  and back         : {Inv.List(scaler.InverseTransform(scaled))}");
        Console.WriteLine();
    }
}
