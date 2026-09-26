using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>Everything a parametric fit reports: its parameter table, its curves and their bands.</summary>
internal static class ParametricFitSample
{
    public static void Run()
    {
        Console.WriteLine("ParametricFit (Lodestar.Survival)");

        ParametricFit fit = ParametricSurvival.Fit(ParametricModel.LogNormal, Trial.TreatmentDurations, Trial.TreatmentObserved);
        for (int j = 0; j < fit.Parameters.Count; j++)
        {
            Console.WriteLine(
                $"  {fit.ParameterNames[j],-16} : {Inv.F4(fit.Parameters[j])} se {Inv.F4(fit.StandardErrors[j])} "
                + $"z {Inv.F3(fit.ZStatistics[j])} p {Inv.F4(fit.PValues[j])} "
                + $"[{Inv.F3(fit.ConfidenceLower[j])}, {Inv.F3(fit.ConfidenceUpper[j])}]");
        }

        Console.WriteLine($"  {fit.Model} fit    : log-likelihood {Inv.F4(fit.LogLikelihood)}, AIC {Inv.F4(fit.Aic)} at {Inv.F3(fit.ConfidenceLevel)}");
        double[] weeks = [10.0, 20.0];
        double[] survival = fit.Survival(weeks);
        double[] cumulative = fit.CumulativeHazard(weeks);
        double[] hazard = fit.Hazard(weeks);
        Console.WriteLine($"  S, H, h at 10    : {Inv.F4(survival[0])}, {Inv.F4(cumulative[0])}, {Inv.F4(hazard[0])}");
        (double[] lower, double[] upper) = fit.SurvivalBounds(weeks);
        (double[] hazardLower, double[] hazardUpper) = fit.CumulativeHazardBounds(weeks);
        Console.WriteLine(
            $"  bands at 20      : S [{Inv.F4(lower[1])}, {Inv.F4(upper[1])}], H [{Inv.F4(hazardLower[1])}, {Inv.F4(hazardUpper[1])}]");
        Console.WriteLine($"  median / p75     : {Inv.F3(fit.MedianSurvivalTime)} / {Inv.F3(fit.Percentile(0.75))}");
        Console.WriteLine();
    }
}
