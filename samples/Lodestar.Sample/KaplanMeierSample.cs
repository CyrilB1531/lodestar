using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>The survival curve of Freireich's treatment arm, censorings included.</summary>
internal static class KaplanMeierSample
{
    public static void Run()
    {
        Console.WriteLine("Kaplan-Meier (Lodestar.Survival)");

        KaplanMeierCurve curve = KaplanMeier.Estimate(Trial.TreatmentDurations, Trial.TreatmentObserved);

        Console.WriteLine($"  steps            : {curve.Steps.Length}");
        Console.WriteLine($"  S(6) / at risk   : {Inv.F4(curve.Survival[1])} / {curve.Steps[1].AtRisk}");
        Console.WriteLine($"  95% bounds at 6  : {Inv.F4(curve.Lower[1])} .. {Inv.F4(curve.Upper[1])}");
        Console.WriteLine();
    }
}
