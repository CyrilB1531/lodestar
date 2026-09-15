using Lodestar.Stats.Regression;

namespace Lodestar.Sample;

/// <summary>Everything a multinomial logit fit reports: the equations, their inference, and the whole-model numbers.</summary>
internal static class MultinomialLogitSummarySample
{
    public static void Run()
    {
        Console.WriteLine("The multinomial logit summary table (Lodestar.Stats.Regression)");

        double[] design = [-0.8, -1.32, -0.25, 0.42, 1.14, 0.11, -0.55, -0.78, 0.75, 1.63, 0.27, -1.23];
        int[] response = [2, 2, 0, 1, 0, 2, 2, 1, 0, 0, 1, 2];

        MultinomialLogitSummary fit = MultinomialLogit.Fit(design, response, 1);

        Console.WriteLine($"  categories       : {string.Join(", ", fit.Categories)}");
        Console.WriteLine(
            $"  label 2 slope    : {Inv.F4(fit.Coefficients[1][1])} ± {Inv.F4(fit.StandardErrors[1][1])}"
            + $", z {Inv.F4(fit.ZStatistics[1][1])}, p {Inv.F4(fit.PValues[1][1])}");
        Console.WriteLine(
            $"  its interval     : [{Inv.F4(fit.ConfidenceLower[1][1])}, {Inv.F4(fit.ConfidenceUpper[1][1])}]"
            + $" at {Inv.F1(fit.ConfidenceLevel)}");
        Console.WriteLine(
            $"  log-likelihood   : {Inv.F4(fit.LogLikelihood)} against the null's {Inv.F4(fit.NullLogLikelihood)}");
        Console.WriteLine(
            $"  pseudo R2 / LR   : {Inv.F4(fit.PseudoRSquared)} / {Inv.F4(fit.LikelihoodRatio)}"
            + $" (p {Inv.F4(fit.LikelihoodRatioPValue)})");
        Console.WriteLine($"  Akaike / Bayesian: {Inv.F4(fit.Akaike)} / {Inv.F4(fit.Bayesian)}");
        Console.WriteLine(
            $"  degrees of freedom: {fit.ModelDegreesOfFreedom} model, {fit.ResidualDegreesOfFreedom} residual"
            + $", intercept: {fit.HasIntercept}");
        Console.WriteLine($"  converged        : {fit.Converged} in {fit.Iterations}");
        Console.WriteLine();
    }
}
