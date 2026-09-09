using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>The two steps, switched independently — and what that does to the statistics.</summary>
internal static class StandardScalerOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("Standard scaling options (Lodestar.Preprocessing)");

        double[] samples = [1.0, 10.0, 2.0, 10.0, 4.0, 10.0];

        var scaleOnly = new StandardScalerOptions { WithMean = false, WithStd = true };
        StandardScaler scaler = StandardScaler.Fit(samples, featureCount: 2, scaleOnly);

        // The asymmetry worth knowing: with_mean off still fits a mean, and only turning
        // both steps off drops it.
        Console.WriteLine($"  WithMean={scaleOnly.WithMean} WithStd={scaleOnly.WithStd} -> mean fitted: {scaler.Mean is not null}");
        Console.WriteLine($"  not subtracted   : {Inv.List(scaler.Transform(samples))}");

        var neither = new StandardScalerOptions { WithMean = false, WithStd = false };
        StandardScaler identity = StandardScaler.Fit(samples, featureCount: 2, neither);
        Console.WriteLine($"  both off         -> mean fitted: {identity.Mean is not null}, transform is the identity");
        Console.WriteLine();
    }
}
