using Lodestar.Stats.Regression;

namespace Lodestar.Sample;

/// <summary>What a multinomial logit fit may be told, and what a spent budget looks like.</summary>
internal static class MultinomialLogitOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("Multinomial logit options (Lodestar.Stats.Regression)");

        double[] design = [-0.8, -1.32, -0.25, 0.42, 1.14, 0.11, -0.55, -0.78, 0.75, 1.63, 0.27, -1.23];
        int[] response = [2, 2, 0, 1, 0, 2, 2, 1, 0, 0, 1, 2];

        var options = new MultinomialLogitOptions
        {
            WithIntercept = true,
            ConfidenceLevel = 0.9,
            MaximumIterations = 35,
            Tolerance = 1e-8,
            ThrowOnNonConvergence = false,
        };

        MultinomialLogitSummary fit = MultinomialLogit.Fit(design, response, 1, options);
        MultinomialLogitSummary once = MultinomialLogit.Fit(design, response, 1, options with { MaximumIterations = 1 });

        Console.WriteLine($"  intercept fitted : {options.WithIntercept}, at level {Inv.F1(options.ConfidenceLevel)}");
        Console.WriteLine(
            $"  budget           : {options.MaximumIterations} iterations, tight tolerance: {options.Tolerance < 1e-6}"
            + $", throws: {options.ThrowOnNonConvergence}");
        Console.WriteLine($"  converged in     : {fit.Iterations}, one step only: {once.Converged}");
        Console.WriteLine();
    }
}
