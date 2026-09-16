using Lodestar.Abstractions;
using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Centring and scaling a matrix, and what a constant feature does.</summary>
internal static class StandardScalerSample
{
    public static void Run()
    {
        Console.WriteLine("Standard scaling (Lodestar.Preprocessing)");

        // Row-major, two features per row. The second never varies.
        double[] samples = [1.0, 10.0, 2.0, 10.0, 4.0, 10.0];

        StandardScaler scaler = StandardScaler.Fit(samples, featureCount: 2);
        Console.WriteLine($"  fitted on        : {scaler.SampleCount} rows x {scaler.FeatureCount} features");
        Console.WriteLine($"  mean / variance  : {Inv.List(scaler.Mean!)} / {Inv.List(scaler.Variance!)}");

        // The constant feature scales by 1, not by 0: its spread is below what the
        // variance computation could resolve, so dividing by it would amplify noise.
        Console.WriteLine($"  scale            : {Inv.List(scaler.Scale!)}");

        double[] standardised = scaler.Transform(samples);
        Console.WriteLine($"  transformed      : {Inv.List(standardised)}");
        Console.WriteLine($"  and back         : {Inv.List(scaler.InverseTransform(standardised))}");

        // Fitted over batches instead, which gives the statistics one whole fit would.
        StandardScaler batched = StandardScaler.Fit(samples.AsSpan(0, 2), 2).PartialFit(samples.AsSpan(2));
        Console.WriteLine($"  over batches     : {Inv.List(batched.Mean!)} over {batched.SampleCount} rows");

        // And over a sparse matrix, where centring is refused rather than offered.
        var sparse = new CsrMatrix(3, 2, [1.0, 10.0, 2.0, 10.0, 4.0, 10.0], [0, 1, 0, 1, 0, 1], [0, 2, 4, 6]);
        Console.WriteLine($"  sparse scale     : {Inv.List(StandardScaler.Fit(sparse).Scale!)}");
        Console.WriteLine();
    }
}
