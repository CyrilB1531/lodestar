using Lodestar.Decomposition;

namespace Lodestar.Sample;

/// <summary>How many principal components a dense block is worth, read off the cumulative curve.</summary>
internal static class PrincipalComponentVarianceSample
{
    public static void Run()
    {
        Console.WriteLine("Explained variance (Lodestar.Decomposition)");

        // Five samples of three features; the third is nearly twice the first.
        double[] matrix =
        [
            1.0, 2.0, 2.1,
            2.0, 1.0, 3.9,
            3.0, 4.0, 6.2,
            4.0, 3.0, 7.9,
            5.0, 5.0, 10.1,
        ];

        PrincipalComponentVariance variance =
            PrincipalComponentVariance.Compute(matrix, rowCount: 5, columnCount: 3);
        Console.WriteLine($"  shape            : {variance.SampleCount}x{variance.FeatureCount}, {variance.ComponentCount} components, total {Inv.F3(variance.TotalVariance)}");
        Console.WriteLine($"  first component  : variance {Inv.F3(variance.ExplainedVariance[0])}, ratio {Inv.F3(variance.ExplainedVarianceRatio[0])}");

        // The smallest count whose cumulative share reaches 99 %.
        int keep = 0;
        while (variance.CumulativeExplainedVarianceRatio[keep] < 0.99)
        {
            keep++;
        }
        Console.WriteLine($"  keep for 99%     : {keep + 1}");
        Console.WriteLine();
    }
}
