using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>The record a Kaplan-Meier estimate comes back in.</summary>
internal static class KaplanMeierCurveSample
{
    public static void Run()
    {
        Console.WriteLine("KaplanMeierCurve (Lodestar.Survival)");

        // Every duration observed, so the curve walks down to zero.
        KaplanMeierCurve curve = KaplanMeier.Estimate([1.0, 2.0, 3.0], [true, true, true]);

        Console.WriteLine($"  confidence level : {Inv.F3(curve.ConfidenceLevel)}");
        Console.WriteLine($"  survival         : {string.Join(", ", curve.Survival.Select(Inv.F3))}");
        // The transform is undefined at zero, so both bounds collapse onto the estimate.
        Console.WriteLine($"  bounds at zero   : {Inv.F3(curve.Lower[3])} .. {Inv.F3(curve.Upper[3])}");
        Console.WriteLine();
    }
}
