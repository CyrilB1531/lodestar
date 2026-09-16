using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Mapping features onto a range, and what a constant feature does there.</summary>
internal static class MinMaxScalerSample
{
    public static void Run()
    {
        Console.WriteLine("Min-max scaling (Lodestar.Preprocessing)");

        // Row-major, two features per row. The second never varies.
        double[] samples = [1.0, 10.0, 3.0, 10.0, 5.0, 10.0];

        MinMaxScaler scaler = MinMaxScaler.Fit(samples, featureCount: 2);
        Console.WriteLine($"  fitted on        : {scaler.SampleCount} rows x {scaler.FeatureCount} features");
        Console.WriteLine($"  min / max        : {Inv.List(scaler.DataMinimum)} / {Inv.List(scaler.DataMaximum)}");

        // The constant feature has a range of 0 and is scaled by 1 rather than divided by nothing.
        Console.WriteLine($"  range            : {Inv.List(scaler.DataRange)}");
        Console.WriteLine($"  scale / offset   : {Inv.List(scaler.Scale)} / {Inv.List(scaler.Minimum)}");

        double[] mapped = scaler.Transform(samples);
        Console.WriteLine($"  mapped           : {Inv.List(mapped)}");
        Console.WriteLine($"  and back         : {Inv.List(scaler.InverseTransform(mapped))}");
        Console.WriteLine();
    }
}
