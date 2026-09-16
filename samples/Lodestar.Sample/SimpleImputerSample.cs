using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Filling what is missing, per feature, from what is present.</summary>
internal static class SimpleImputerSample
{
    public static void Run()
    {
        Console.WriteLine("SimpleImputer (Lodestar.Preprocessing)");

        // NaN marks a missing value, as it does in the reference.
        double[] samples = [1.0, 10.0, 2.0, double.NaN, double.NaN, 30.0];

        SimpleImputer imputer = SimpleImputer.Fit(samples, featureCount: 2);
        Console.WriteLine($"  fitted on        : {imputer.SampleCount} rows x {imputer.FeatureCount} features");
        Console.WriteLine($"  means            : {Inv.List(imputer.Statistics)}");
        Console.WriteLine($"  filled           : {Inv.List(imputer.Transform(samples))}");
        Console.WriteLine();
    }
}
