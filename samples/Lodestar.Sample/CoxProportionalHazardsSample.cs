using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>By how much a dose and a treatment move the hazard, and the design the fit refuses.</summary>
internal static class CoxProportionalHazardsSample
{
    public static void Run()
    {
        Console.WriteLine("Cox proportional hazards (Lodestar.Survival)");

        // One row per patient: dose in mg, then 1 if on the new treatment.
        double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                           0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
        double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
        bool[] died = [true, true, false, true, true, true, true, false, true, true];

        CoxSummary summary = CoxProportionalHazards.Fit(design, months, died, featureCount: 2);
        Console.WriteLine($"  hazard ratio/mg  : {Inv.F3(summary.HazardRatios[0])} (p {Inv.F4(summary.PValues[0])})");

        // The second column repeats the first, so neither coefficient is identified.
        double[] duplicated = [0.2, 0.2, 1.1, 1.1, -0.4, -0.4, 0.9, 0.9, -1.3, -1.3, 0.5, 0.5];
        try
        {
            CoxProportionalHazards.Fit(duplicated, [5, 3, 8, 3, 9, 6], [true, true, false, true, true, true], 2);
        }
        catch (ArgumentException error)
        {
            Console.WriteLine($"  collinear design : refused on '{error.ParamName}'");
        }

        Console.WriteLine();
    }
}
