using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>The baseline a Cox fit carries, per stratum, and what it is made of.</summary>
internal static class CoxBaselineSample
{
    public static void Run()
    {
        Console.WriteLine("CoxBaseline (Lodestar.Survival)");

        double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                           0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
        double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
        bool[] died = [true, true, false, true, true, true, true, false, true, true];

        CoxSummary fit = CoxProportionalHazards.Fit(design, months, died, [], [0, 0, 1, 1, 0, 0, 1, 1, 0, 0], [], 2);
        foreach (CoxBaseline baseline in fit.Baselines)
        {
            int last = baseline.Times.Length - 1;
            Console.WriteLine(
                $"  stratum {baseline.Stratum}        : H({Inv.F1(baseline.Times[last])}) {Inv.F3(baseline.CumulativeHazard[last])}, "
                + $"S {Inv.F3(baseline.Survival[last])}, first hazard {Inv.F3(baseline.Hazard[0])}");
        }

        var rebuilt = new CoxBaseline(0, fit.Baselines[0].Times, fit.Baselines[0].Hazard, fit.Baselines[0].CumulativeHazard, fit.Baselines[0].Survival);
        Console.WriteLine($"  equal by value   : {rebuilt == fit.Baselines[0]}");
        Console.WriteLine();
    }
}
