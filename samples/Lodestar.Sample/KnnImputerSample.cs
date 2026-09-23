using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Filling a gap from the rows that resemble the one it is in.</summary>
internal static class KnnImputerSample
{
    public static void Run()
    {
        Console.WriteLine("KnnImputer (Lodestar.Preprocessing)");

        double[] rows =
        [
            1.0, 2.0, double.NaN,
            3.0, 4.0, 3.0,
            double.NaN, 6.0, 5.0,
            8.0, 8.0, 7.0,
        ];

        KnnImputer imputer = KnnImputer.Fit(rows, featureCount: 3);
        Console.WriteLine($"  fitted on        : {imputer.SampleCount} rows x {imputer.FeatureCount} features");
        Console.WriteLine($"  filled           : {Inv.List(imputer.Transform(rows))}");

        // A feature missing from every row has nothing to impute it from, and is dropped.
        double[] empty = [1.0, double.NaN, 2.0, 3.0, double.NaN, 4.0];
        KnnImputer narrower = KnnImputer.Fit(empty, 3);
        Console.WriteLine($"  dropped          : {narrower.OutputFeatureCount} of 3 kept ({string.Join(",", narrower.KeptFeatures)})");
        Console.WriteLine();
    }
}
