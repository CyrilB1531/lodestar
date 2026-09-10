using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>The record a Nelson-Aalen estimate comes back in.</summary>
internal static class NelsonAalenCurveSample
{
    public static void Run()
    {
        Console.WriteLine("NelsonAalenCurve (Lodestar.Survival)");

        NelsonAalenCurve curve = NelsonAalen.Estimate([1.0, 2.0, 3.0], [true, true, true]);

        Console.WriteLine($"  steps            : {curve.Steps.Length}");
        // 1/3 + 1/2 + 1/1, where Kaplan-Meier on the same sample has reached zero.
        Console.WriteLine($"  hazard at the end: {Inv.F4(curve.CumulativeHazard[3])}");
        Console.WriteLine();
    }
}
