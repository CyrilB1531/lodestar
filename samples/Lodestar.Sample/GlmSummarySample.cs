using Lodestar.Stats.Regression;

namespace Lodestar.Sample;

/// <summary>What the whole-model half of the table says, fitted through the log link.</summary>
internal static class GlmSummarySample
{
    public static void Run()
    {
        Console.WriteLine("The GLM summary table (Lodestar.Stats.Regression)");

        double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0];
        double[] response = [2.0, 3.0, 6.0, 8.0, 10.0, 13.0, 14.0, 17.0];

        GlmSummary fit = GeneralizedLinearModel.Fit(design, response, 1, GlmFamily.Poisson);

        Console.WriteLine($"  deviance / null  : {Inv.F4(fit.Deviance)} / {Inv.F4(fit.NullDeviance)}");
        Console.WriteLine($"  log-likelihood   : {Inv.F4(fit.LogLikelihood)}");
        Console.WriteLine($"  Akaike           : {Inv.F4(fit.Akaike)}");
        Console.WriteLine($"  dispersion       : {Inv.F1(fit.Dispersion)}");
        Console.WriteLine(
            $"  has intercept    : {fit.HasIntercept}, on {fit.ResidualDegreesOfFreedom} residual d.f.");
        Console.WriteLine();
    }
}
