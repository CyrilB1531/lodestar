using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>An Aalen fit penalised both ways, at a narrower level, and one without an intercept.</summary>
internal static class AalenOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("Aalen fitting options (Lodestar.Survival)");

        (double[] design, double[] weeks, bool[] observed) = AcceleratedFailureTimeSample.Arms();
        var penalised = new AalenOptions { CoefficientPenalizer = 0.5, SmoothingPenalizer = 1.0, ConfidenceLevel = 0.9 };
        AalenSummary fit = AalenAdditive.Fit(design, weeks, observed, 1, penalised);
        Console.WriteLine(
            $"  penalties        : {Inv.F3(penalised.CoefficientPenalizer)} and {Inv.F3(penalised.SmoothingPenalizer)}, "
            + $"slope {Inv.F4(fit.Slopes[0])} at {Inv.F3(fit.ConfidenceLevel)}");

        var through = new AalenOptions { FitIntercept = false };
        AalenSummary origin = AalenAdditive.Fit(design, weeks, observed, 1, through);
        Console.WriteLine($"  no intercept     : {origin.CovariateIndices.Count} coefficient (intercept {through.FitIntercept})");
        Console.WriteLine();
    }
}
