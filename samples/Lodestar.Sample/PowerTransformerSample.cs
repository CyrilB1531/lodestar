using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>One exponent, fitted by maximum likelihood, that straightens a skew.</summary>
internal static class PowerTransformerSample
{
    public static void Run()
    {
        Console.WriteLine("PowerTransformer (Lodestar.Preprocessing)");

        double[] income = [22.0, 25.0, 28.0, 31.0, 35.0, 42.0, 55.0, 78.0, 120.0, 260.0];

        PowerTransformer yeoJohnson = PowerTransformer.Fit(income, featureCount: 1);
        Console.WriteLine($"  fitted on        : {yeoJohnson.SampleCount} rows x {yeoJohnson.FeatureCount} features");
        Console.WriteLine($"  lambda           : {Inv.F4(yeoJohnson.Lambdas[0])}");
        Console.WriteLine($"  transformed      : {Inv.List(yeoJohnson.Transform(income))}");

        // The round trip is genuine here: the power is monotone and its inverse exact.
        Console.WriteLine($"  35 there and back: {Inv.F4(yeoJohnson.InverseTransform(yeoJohnson.Transform([35.0]))[0])}");
        Console.WriteLine();
    }
}
