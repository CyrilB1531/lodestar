using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>The piecewise exponential's breakpoints, a narrower interval, and an iteration budget too small to converge.</summary>
internal static class ParametricOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("Parametric fitting options (Lodestar.Survival)");

        var pieces = new ParametricOptions { Breakpoints = [10.0, 20.0], ConfidenceLevel = 0.9 };
        ParametricFit fit = ParametricSurvival.Fit(
            ParametricModel.PiecewiseExponential, Trial.TreatmentDurations, Trial.TreatmentObserved, pieces);
        Console.WriteLine(
            $"  {pieces.Breakpoints.Length} breakpoints    : lambdas {string.Join(", ", fit.Parameters.Select(Inv.F3))} at {Inv.F3(fit.ConfidenceLevel)}");

        var tight = new ParametricOptions { MaximumIterations = 1 };
        try
        {
            ParametricSurvival.Fit(ParametricModel.GeneralizedGamma, Trial.TreatmentDurations, Trial.TreatmentObserved, tight);
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine($"  budget of {tight.MaximumIterations}      : refused, not converged");
        }

        foreach (ParametricModel model in new[] { ParametricModel.Exponential, ParametricModel.LogLogistic })
        {
            Console.WriteLine($"  {model,-16} : AIC {Inv.F3(ParametricSurvival.Fit(model, Trial.TreatmentDurations, Trial.TreatmentObserved).Aic)}");
        }

        Console.WriteLine();
    }
}
