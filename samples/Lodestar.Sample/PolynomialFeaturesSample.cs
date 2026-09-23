using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Giving a linear model the curvature and the interactions it cannot see.</summary>
internal static class PolynomialFeaturesSample
{
    public static void Run()
    {
        Console.WriteLine("PolynomialFeatures (Lodestar.Preprocessing)");

        double[] row = [3.0, 4.0];

        Console.WriteLine($"  names            : {string.Join(" | ", PolynomialFeatures.FeatureNames(2))}");
        Console.WriteLine($"  values           : {Inv.List(PolynomialFeatures.Transform(row, featureCount: 2))}");
        Console.WriteLine();
    }
}
