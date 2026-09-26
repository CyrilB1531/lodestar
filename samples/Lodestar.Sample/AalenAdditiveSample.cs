using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>How much Freireich's treatment adds to the hazard, week by week, as Aalen's additive model reads it.</summary>
internal static class AalenAdditiveSample
{
    public static void Run()
    {
        Console.WriteLine("Aalen additive (Lodestar.Survival)");

        (double[] design, double[] weeks, bool[] observed) = AcceleratedFailureTimeSample.Arms();
        AalenSummary fit = AalenAdditive.Fit(design, weeks, observed, featureCount: 1);
        int last = fit.EventTimes.Count - 1;
        Console.WriteLine(
            $"  {fit.EventTimes.Count} event times  : treatment adds {Inv.F4(fit.CumulativeHazards[last * 2])} "
            + $"by week {Inv.F0(fit.EventTimes[last])}");

        double[] weights = [.. weeks.Select((_, i) => i % 2 == 0 ? 1.0 : 2.0)];
        AalenSummary weighted = AalenAdditive.Fit(design, weeks, observed, weights, 1);
        Console.WriteLine($"  weighted         : slope {Inv.F4(weighted.Slopes[0])}");
        Console.WriteLine();
    }
}
