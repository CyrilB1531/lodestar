using Lodestar.Stats.Regression;

namespace Lodestar.Sample;

/// <summary>The inference table over rows that are not equally reliable.</summary>
internal static class WeightedLeastSquaresSample
{
    public static void Run()
    {
        Console.WriteLine("Weighted least squares (Lodestar.Stats.Regression)");

        double[] dose = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0];
        double[] meanResponse = [2.3, 3.8, 6.4, 7.7, 11.6, 10.9];
        double[] groupSize = [40.0, 35.0, 30.0, 12.0, 5.0, 3.0];

        OlsSummary summary = WeightedLeastSquares.Fit(dose, meanResponse, groupSize, featureCount: 1);

        Console.WriteLine($"  slope / s.e. / p : {Inv.F4(summary.Coefficients[1])} / {Inv.F4(summary.StandardErrors[1])} / {Inv.F4(summary.PValues[1])}");
        Console.WriteLine($"  weighted R²      : {Inv.F4(summary.RSquared)} on {summary.ResidualDegreesOfFreedom} d.f.");
        Console.WriteLine();
    }
}
