using Lodestar.Stats.Regression;

namespace Lodestar.Sample;

/// <summary>The estimate without the table, for a caller fitting many regressions.</summary>
internal static class OlsEstimateSample
{
    public static void Run()
    {
        Console.WriteLine("The OLS estimate alone (Lodestar.Stats.Regression)");

        double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0];
        double[] response = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1];

        OlsEstimate estimate = OrdinaryLeastSquares.Estimate(design, response, featureCount: 1, withIntercept: true);

        Console.WriteLine($"  slope / s.e. / t : {Inv.F4(estimate.Coefficients[1])} / {Inv.F4(estimate.StandardErrors[1])} / {Inv.F1(estimate.TStatistics[1])}");
        Console.WriteLine($"  residual SS      : {Inv.F4(estimate.ResidualSumOfSquares)} on {estimate.ResidualDegreesOfFreedom} d.f.");
        Console.WriteLine($"  intercept fitted : {estimate.HasIntercept}");
        Console.WriteLine();
    }
}
