using Lodestar.Abstractions;
using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>The scaler for data with outliers, beside what StandardScaler makes of the same column.</summary>
internal static class RobustScalerSample
{
    public static void Run()
    {
        Console.WriteLine("Robust scaling (Lodestar.Preprocessing)");

        // One column, one value three decades out.
        double[] samples = [1.0, 2.0, 3.0, 4.0, 5000.0];

        RobustScaler scaler = RobustScaler.Fit(samples, featureCount: 1);
        Console.WriteLine($"  fitted on        : {scaler.SampleCount} rows x {scaler.FeatureCount} features");
        Console.WriteLine($"  median / range   : {Inv.List(scaler.Centre!)} / {Inv.List(scaler.Scale!)}");

        double[] robust = scaler.Transform(samples);
        Console.WriteLine($"  robust           : {Inv.List(robust)}");
        Console.WriteLine($"  and back         : {Inv.List(scaler.InverseTransform(robust))}");

        // The mean and the deviation both move with the outlier, so every ordinary
        // value lands in the same place and the shape of the column is lost.
        Console.WriteLine($"  standardised     : {Inv.List(StandardScaler.Fit(samples, 1).Transform(samples))}");

        // Over a sparse matrix the percentiles read the absent zeros too, and centring is refused.
        var sparse = new CsrMatrix(4, 2, [1.0, 5.0, 3.0, 4.0], [0, 1, 0, 0], [0, 2, 3, 4, 4]);
        RobustScaler sparseScaler = RobustScaler.Fit(sparse);
        CsrMatrix sparseScaled = sparseScaler.Transform(sparse);
        Console.WriteLine($"  sparse range     : {Inv.List(sparseScaler.Scale!)}");
        Console.WriteLine($"  sparse scaled    : {Inv.List(sparseScaled.Values)}");
        Console.WriteLine($"  and back         : {Inv.List(sparseScaler.InverseTransform(sparseScaled).Values)}");
        Console.WriteLine();
    }
}
