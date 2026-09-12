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
        Console.WriteLine();
    }
}
