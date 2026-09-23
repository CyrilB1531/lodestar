using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>The degree, the interaction restriction, and the bias.</summary>
internal static class PolynomialFeaturesOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("PolynomialFeaturesOptions (Lodestar.Preprocessing)");

        // The count is a binomial coefficient, so ask before expanding a wide matrix.
        var cubic = new PolynomialFeaturesOptions { Degree = 3 };
        Console.WriteLine($"  4 features, d={cubic.Degree}  : {PolynomialFeatures.OutputFeatureCount(4, cubic)} columns");

        var interactions = new PolynomialFeaturesOptions
        {
            Degree = 2,
            InteractionOnly = true,
            IncludeBias = false,
        };
        Console.WriteLine($"  interactions     : {string.Join(" | ", PolynomialFeatures.FeatureNames(3, interactions))}");
        Console.WriteLine($"  bias {interactions.IncludeBias}, interaction-only {interactions.InteractionOnly}");
        Console.WriteLine();
    }
}
