using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>An AFT fit with the shape modelled too, a ridge penalty, the sandwich variance and no intercept.</summary>
internal static class AftOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("AFT fitting options (Lodestar.Survival)");

        (double[] design, double[] weeks, bool[] observed) = AcceleratedFailureTimeSample.Arms();
        var options = new AftOptions { Ancillary = true, Penalizer = 0.1, Robust = true, ConfidenceLevel = 0.9, MaximumIterations = 50 };
        AftSummary fit = AcceleratedFailureTime.Fit(AftModel.LogNormal, design, weeks, observed, 1, options);
        Console.WriteLine(
            $"  {fit.Coefficients.Count} coefficients   : penalizer {Inv.F3(options.Penalizer)}, robust {options.Robust}, "
            + $"ancillary {options.Ancillary}, {options.MaximumIterations} steps at most, level {Inv.F3(options.ConfidenceLevel)}");

        var through = new AftOptions { FitIntercept = false };
        AftSummary origin = AcceleratedFailureTime.Fit(AftModel.LogNormal, design, weeks, observed, 1, through);
        Console.WriteLine($"  no intercept     : {origin.Coefficients.Count} coefficients (intercept {through.FitIntercept})");
        Console.WriteLine();
    }
}
