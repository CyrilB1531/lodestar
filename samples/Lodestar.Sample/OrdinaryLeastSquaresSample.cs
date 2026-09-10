using Lodestar.Stats.Regression;

namespace Lodestar.Sample;

/// <summary>Fitting a line, and reading how sure the fit is of it.</summary>
internal static class OrdinaryLeastSquaresSample
{
    public static void Run()
    {
        Console.WriteLine("Ordinary least squares (Lodestar.Stats.Regression)");

        // Row-major, one regressor per row. No constant column: WithIntercept adds it.
        double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0];
        double[] response = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1];

        OlsSummary summary = OrdinaryLeastSquares.Fit(design, response, featureCount: 1);

        Console.WriteLine($"  intercept        : {Inv.F4(summary.Coefficients[0])}");
        Console.WriteLine($"  slope            : {Inv.F4(summary.Coefficients[1])}");

        // The estimate is the cheap half. These three are what "inference" means.
        Console.WriteLine($"  standard error   : {Inv.F4(summary.StandardErrors[1])}");
        Console.WriteLine($"  t / p            : {Inv.F3(summary.TStatistics[1])} / {Inv.E3(summary.PValues[1])}");
        Console.WriteLine(
            $"  95% interval     : [{Inv.F4(summary.ConfidenceLower[1])}, {Inv.F4(summary.ConfidenceUpper[1])}]");
        Console.WriteLine();
    }
}
