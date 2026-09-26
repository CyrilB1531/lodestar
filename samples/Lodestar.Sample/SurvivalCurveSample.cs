using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>What a survival curve holds: its steps, the estimate and the bounds at a level.</summary>
internal static class SurvivalCurveSample
{
    public static void Run()
    {
        Console.WriteLine("SurvivalCurve (Lodestar.Survival)");

        SurvivalCurve curve = BreslowFlemingHarrington.Estimate(Trial.ControlDurations, Trial.ControlObserved, 0.9);
        SurvivalStep last = curve.Steps[^1];
        Console.WriteLine(
            $"  last step        : week {Inv.F0(last.Time)}, S {Inv.F4(curve.Survival[^1])} in "
            + $"[{Inv.F4(curve.Lower[^1])}, {Inv.F4(curve.Upper[^1])}] at {Inv.F3(curve.ConfidenceLevel)}");
        Console.WriteLine();
    }
}
