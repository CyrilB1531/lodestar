using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>Every figure a Cox fit reports, per covariate and for the model.</summary>
internal static class CoxSummarySample
{
    public static void Run()
    {
        Console.WriteLine("CoxSummary (Lodestar.Survival)");

        double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                           0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
        double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
        bool[] died = [true, true, false, true, true, true, true, false, true, true];

        CoxSummary s = CoxProportionalHazards.Fit(design, months, died, featureCount: 2);

        for (int j = 0; j < s.Coefficients.Count; j++)
        {
            Console.WriteLine(
                $"  covariate {j}      : coef {Inv.F4(s.Coefficients[j])} se {Inv.F4(s.StandardErrors[j])} "
                + $"z {Inv.F3(s.ZStatistics[j])} p {Inv.F4(s.PValues[j])} "
                + $"[{Inv.F3(s.ConfidenceLower[j])}, {Inv.F3(s.ConfidenceUpper[j])}] "
                + $"HR {Inv.F3(s.HazardRatios[j])} [{Inv.F3(s.HazardRatioLower[j])}, {Inv.F3(s.HazardRatioUpper[j])}]");
        }

        Console.WriteLine($"  log-likelihood   : {Inv.F4(s.LogLikelihood)} against {Inv.F4(s.NullLogLikelihood)} with none");
        Console.WriteLine(
            $"  likelihood ratio : {Inv.F3(s.LikelihoodRatioStatistic)} on {s.LikelihoodRatioDegreesOfFreedom} df, "
            + $"p {Inv.F4(s.LikelihoodRatioPValue)}");
        Console.WriteLine($"  concordance      : {Inv.F3(s.ConcordanceIndex)} at level {Inv.F3(s.ConfidenceLevel)}");
        Console.WriteLine();
    }
}
