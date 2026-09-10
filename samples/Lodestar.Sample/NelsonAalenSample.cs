using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>The cumulative hazard of the same arm, which keeps rising where survival cannot fall.</summary>
internal static class NelsonAalenSample
{
    public static void Run()
    {
        Console.WriteLine("Nelson-Aalen (Lodestar.Survival)");

        NelsonAalenCurve curve = NelsonAalen.Estimate(Trial.TreatmentDurations, Trial.TreatmentObserved);

        // Three events tied at time 6: the increment is 1/21 + 1/20 + 1/19, not 3/21.
        Console.WriteLine($"  H(6)             : {Inv.F4(curve.CumulativeHazard[1])}");
        Console.WriteLine($"  H(7)             : {Inv.F4(curve.CumulativeHazard[2])}");
        Console.WriteLine();
    }
}
