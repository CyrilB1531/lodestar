using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>How much Freireich's treatment stretches survival time, as a Weibull regression, censored three ways.</summary>
internal static class AcceleratedFailureTimeSample
{
    public static void Run()
    {
        Console.WriteLine("Accelerated failure time (Lodestar.Survival)");

        (double[] design, double[] weeks, bool[] observed) = Arms();
        AftSummary right = AcceleratedFailureTime.Fit(AftModel.Weibull, design, weeks, observed, featureCount: 1);
        Console.WriteLine($"  time ratio       : {Inv.F3(right.ExpCoefficients[0])} for the treatment arm");

        AftSummary left = AcceleratedFailureTime.FitLeftCensored(AftModel.Weibull, design, weeks, observed, 1);
        double[] lower = [.. weeks.Select((w, i) => observed[i] ? w : w - 1.0)];
        double[] upper = [.. weeks.Select((w, i) => observed[i] ? w : w + 1.0)];
        AftSummary interval = AcceleratedFailureTime.FitIntervalCensored(AftModel.Weibull, design, lower, upper, 1);
        Console.WriteLine($"  left / interval  : {Inv.F3(left.ExpCoefficients[0])} / {Inv.F3(interval.ExpCoefficients[0])}");

        double[] weights = [.. weeks.Select((_, i) => i % 2 == 0 ? 1.0 : 2.0)];
        double[] entries = new double[weeks.Length];
        AftSummary weighted = AcceleratedFailureTime.Fit(AftModel.Weibull, design, weeks, observed, weights, entries, 1);
        AftSummary leftWeighted = AcceleratedFailureTime.FitLeftCensored(AftModel.Weibull, design, weeks, observed, weights, entries, 1);
        AftSummary intervalWeighted = AcceleratedFailureTime.FitIntervalCensored(AftModel.Weibull, design, lower, upper, weights, entries, 1);
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
