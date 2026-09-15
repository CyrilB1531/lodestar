using Lodestar.Stats.Regression;

namespace Lodestar.Sample;

/// <summary>Three unordered categories on one regressor, and the table the fit carries.</summary>
internal static class MultinomialLogitSample
{
    public static void Run()
    {
        Console.WriteLine("MultinomialLogit (Lodestar.Stats.Regression)");

        double[] design = [-0.8, -1.32, -0.25, 0.42, 1.14, 0.11, -0.55, -0.78, 0.75, 1.63, 0.27, -1.23];
        int[] response = [2, 2, 0, 1, 0, 2, 2, 1, 0, 0, 1, 2];

        MultinomialLogitSummary fit = MultinomialLogit.Fit(
            design, response, 1, new MultinomialLogitOptions { ConfidenceLevel = 0.9 });

        Console.WriteLine($"  reference label  : {fit.Categories[0]}");
        Console.WriteLine($"  slope of label 2 : {Inv.F4(fit.Coefficients[1][1])}");
        Console.WriteLine($"  pseudo R2        : {Inv.F4(fit.PseudoRSquared)}");
        Console.WriteLine();
    }
}
