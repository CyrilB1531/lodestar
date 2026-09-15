using Lodestar.Stats.Regression;

namespace Lodestar.Sample;

/// <summary>A logistic fit, and the table that makes it inference.</summary>
internal static class GeneralizedLinearModelSample
{
    public static void Run()
    {
        Console.WriteLine("GeneralizedLinearModel (Lodestar.Stats.Regression)");

        // Row-major, one regressor per row. No constant column: WithIntercept adds it.
        double[] design = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0];
        double[] response = [0.0, 0.0, 1.0, 0.0, 1.0, 1.0];

        GlmSummary fit = GeneralizedLinearModel.Fit(design, response, 1, GlmFamily.Binomial);

        Console.WriteLine($"  slope            : {Inv.F4(fit.Coefficients[1])}");
        Console.WriteLine($"  its p-value      : {Inv.F4(fit.PValues[1])}");
        Console.WriteLine($"  deviance         : {Inv.F4(fit.Deviance)}");
        Console.WriteLine($"  converged in     : {fit.Iterations}");

        // Counts over unequal years: the exposure enters as log(years) with no coefficient, so the
        // intercept reads as a yearly rate.
        double[] claims = [1.0, 4.0, 1.0, 6.0, 3.0, 9.0];
        double[] years = [1.0, 2.5, 0.5, 4.0, 1.5, 3.0];
        GlmSummary rate = GeneralizedLinearModel.Fit(design, claims, [], years, 1, GlmFamily.Poisson);

        Console.WriteLine($"  claims a year    : {Inv.F4(Math.Exp(rate.Coefficients[0]))}");
        Console.WriteLine();
    }
}
