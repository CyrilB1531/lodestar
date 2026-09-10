using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>One step of a curve: a time, the risk set before it, and what happened at it.</summary>
internal static class SurvivalStepSample
{
    public static void Run()
    {
        Console.WriteLine("SurvivalStep (Lodestar.Survival)");

        // The middle subject is censored, which leaves the risk set without moving the curve.
        KaplanMeierCurve curve = KaplanMeier.Estimate([1.0, 2.0, 3.0], [true, false, true]);

        foreach (SurvivalStep step in curve.Steps)
        {
            Console.WriteLine(
                $"  t={Inv.F1(step.Time)}  at risk={step.AtRisk}  events={step.Events}  censored={step.Censored}");
        }

        Console.WriteLine();
    }
}
