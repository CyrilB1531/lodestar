using Lodestar.Stats.Regression;

namespace Lodestar.Sample;

/// <summary>What the whole-model half of the table says, and what a VIF adds to it.</summary>
internal static class OlsSummarySample
{
    public static void Run()
    {
        Console.WriteLine("The OLS summary table (Lodestar.Stats.Regression)");

        // Two regressors that say almost the same thing: x2 is x1 plus a hundredth.
        double[] design = [1.0, 1.01, 2.0, 2.02, 3.0, 2.99, 4.0, 4.01, 5.0, 5.02,
                           6.0, 5.99, 7.0, 7.01, 8.0, 8.02, 9.0, 8.99, 10.0, 10.01];
        double[] response = [2.2, 4.1, 6.3, 7.9, 10.2, 12.1, 14.3, 15.9, 18.2, 20.1];

        OlsSummary summary = OrdinaryLeastSquares.Fit(design, response, featureCount: 2);

        Console.WriteLine($"  R2 / adjusted    : {Inv.F4(summary.RSquared)} / {Inv.F4(summary.AdjustedRSquared)}");
        Console.WriteLine($"  F / p            : {Inv.F1(summary.FStatistic)} / {Inv.E3(summary.FPValue)}");
        Console.WriteLine(
            $"  residual s.e.    : {Inv.F4(summary.ResidualStandardError)} on {summary.ResidualDegreesOfFreedom} d.f.");

        // The model explains 99.95% of the variance and neither slope is significant:
        // the fit cannot tell which of two interchangeable regressors earns the credit.
        Console.WriteLine($"  slope p-values   : {Inv.E3(summary.PValues[1])}, {Inv.E3(summary.PValues[2])}");
        Console.WriteLine($"  VIF per regressor: {Inv.List(summary.VarianceInflationFactors)}");
        Console.WriteLine();
    }
}
