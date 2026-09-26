using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>The Breslow-Fleming-Harrington curve of Freireich's treatment arm, then with three late entrants.</summary>
internal static class BreslowFlemingHarringtonSample
{
    public static void Run()
    {
        Console.WriteLine("Breslow-Fleming-Harrington (Lodestar.Survival)");

        SurvivalCurve curve = BreslowFlemingHarrington.Estimate(Trial.TreatmentDurations, Trial.TreatmentObserved);
        Console.WriteLine($"  S(6)             : {Inv.F4(curve.Survival[1])} in [{Inv.F4(curve.Lower[1])}, {Inv.F4(curve.Upper[1])}]");

        double[] entries = [.. Trial.TreatmentDurations.Select((_, i) => i >= 18 ? 5.0 : 0.0)];
        SurvivalCurve entered = BreslowFlemingHarrington.Estimate(Trial.TreatmentDurations, Trial.TreatmentObserved, entries);
        Console.WriteLine($"  with entrants    : {entered.Steps.Length} steps, S(6) {Inv.F4(entered.Survival[2])}");
        Console.WriteLine();
    }
}
