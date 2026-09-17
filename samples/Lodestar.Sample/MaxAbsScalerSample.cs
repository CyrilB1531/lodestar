using Lodestar.Abstractions;
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

        // The running maximum absolute value, and the sparse matrix this scaler suits.
        MaxAbsScaler folded = scaler.PartialFit([-32.0, 1.0]);
        Console.WriteLine($"  after a batch    : {Inv.List(folded.MaximumAbsolute)} over {folded.SampleCount} rows");

        var sparse = new CsrMatrix(3, 2, [2.0, -4.0, 1.0], [0, 1, 0], [0, 2, 3, 3]);
        MaxAbsScaler sparseScaler = MaxAbsScaler.Fit(sparse);
        CsrMatrix sparseScaled = sparseScaler.Transform(sparse);
        Console.WriteLine($"  sparse scale     : {Inv.List(sparseScaler.Scale)}");
        Console.WriteLine($"  sparse scaled    : {Inv.List(sparseScaled.Values)} ({sparseScaled.NonZeroCount} stored)");
        Console.WriteLine($"  and back         : {Inv.List(sparseScaler.InverseTransform(sparseScaled).Values)}");
        Console.WriteLine();
    }
}
