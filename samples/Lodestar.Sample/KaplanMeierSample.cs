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

        // The mean survival up to week 10, and whether the two arms differ at that week alone.
        RestrictedMeanResult tenWeeks = KaplanMeier.RestrictedMean(curve, 10.0);
        KaplanMeierCurve control = KaplanMeier.Estimate(Trial.ControlDurations, Trial.ControlObserved);
        Console.WriteLine($"  RMST to 10       : {Inv.F4(tenWeeks.Mean)} (variance {Inv.F4(tenWeeks.Variance)})");
        Console.WriteLine($"  differ at 10     : p {Inv.F4(KaplanMeier.CompareAt(10.0, curve, control).PValue)}");
        Console.WriteLine();
    }
}
