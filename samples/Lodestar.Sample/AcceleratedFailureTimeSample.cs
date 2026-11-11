using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>How much Freireich's treatment stretches survival time, as a Weibull regression, censored three ways.</summary>
internal static class AcceleratedFailureTimeSample
{
    public static void Run()
    {
        Console.WriteLine("Accelerated failure time (Lodestar.Survival)");

        (double[] design, double[] weeks, bool[] observed) = Arms();
        AftSummary right = AcceleratedFailureTime.Fit(design, weeks, observed, featureCount: 1, AftModel.Weibull);
        Console.WriteLine($"  time ratio       : {Inv.F3(right.ExpCoefficients[0])} for the treatment arm");

        AftSummary left = AcceleratedFailureTime.FitLeftCensored(design, weeks, observed, 1, AftModel.Weibull);
        double[] lower = [.. weeks.Select((w, i) => observed[i] ? w : w - 1.0)];
        double[] upper = [.. weeks.Select((w, i) => observed[i] ? w : w + 1.0)];
        AftSummary interval = AcceleratedFailureTime.FitIntervalCensored(design, lower, upper, 1, AftModel.Weibull);
        Console.WriteLine($"  left / interval  : {Inv.F3(left.ExpCoefficients[0])} / {Inv.F3(interval.ExpCoefficients[0])}");

        double[] weights = [.. weeks.Select((_, i) => i % 2 == 0 ? 1.0 : 2.0)];
        double[] entries = new double[weeks.Length];
        AftSummary weighted = AcceleratedFailureTime.Fit(design, weeks, observed, weights, entries, 1, AftModel.Weibull);
        AftSummary leftWeighted = AcceleratedFailureTime.FitLeftCensored(design, weeks, observed, weights, entries, 1, AftModel.Weibull);
        AftSummary intervalWeighted = AcceleratedFailureTime.FitIntervalCensored(design, lower, upper, weights, entries, 1, AftModel.Weibull);
        Console.WriteLine(
            $"  weighted         : {Inv.F3(weighted.ExpCoefficients[0])}, {Inv.F3(leftWeighted.ExpCoefficients[0])}, "
            + $"{Inv.F3(intervalWeighted.ExpCoefficients[0])}");
        Console.WriteLine();
    }

    /// <summary>Both arms in one sample, the covariate one for the treatment arm.</summary>
    internal static (double[] Design, double[] Weeks, bool[] Observed) Arms() =>
        ([.. Trial.TreatmentDurations.Select(_ => 1.0), .. Trial.ControlDurations.Select(_ => 0.0)],
         [.. Trial.TreatmentDurations, .. Trial.ControlDurations],
         [.. Trial.TreatmentObserved, .. Trial.ControlObserved]);
}
