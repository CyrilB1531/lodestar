using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Reshaping a skewed column, at the cost of everything but its order.</summary>
internal static class QuantileTransformerSample
{
    public static void Run()
    {
        Console.WriteLine("QuantileTransformer (Lodestar.Preprocessing)");

        // The last value is three times the one before it, and dominates every distance.
        double[] skew = [1.0, 1.0, 2.0, 3.0, 5.0, 8.0, 13.0, 21.0, 34.0, 100.0];

        QuantileTransformer uniform = QuantileTransformer.Fit(skew, featureCount: 1);
        Console.WriteLine($"  fitted on        : {uniform.SampleCount} rows x {uniform.FeatureCount} features");
        Console.WriteLine($"  uniform          : {Inv.List(uniform.Transform(skew))}");
        Console.WriteLine($"  back at 0.5      : {Inv.List(uniform.InverseTransform([0.5]))}");
        Console.WriteLine();
    }
}
