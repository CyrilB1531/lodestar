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
        Console.WriteLine();
    }
}
