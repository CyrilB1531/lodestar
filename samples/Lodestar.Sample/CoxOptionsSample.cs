using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>The interval level a Cox fit is read at, and an iteration budget too small to converge.</summary>
internal static class CoxOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("Cox fitting options (Lodestar.Survival)");

        double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                           0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
        double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
        bool[] died = [true, true, false, true, true, true, true, false, true, true];

        var ninety = new CoxOptions { ConfidenceLevel = 0.9 };
        CoxSummary narrow = CoxProportionalHazards.Fit(design, months, died, 2, ninety);
        Console.WriteLine(
            $"  90% interval     : [{Inv.F4(narrow.ConfidenceLower[0])}, {Inv.F4(narrow.ConfidenceUpper[0])}] at {Inv.F3(ninety.ConfidenceLevel)}");

        // One Newton step from zero does not reach the maximum, and the fit says so rather than report.
        var tight = new CoxOptions { MaximumIterations = 1 };
        try
        {
            CoxProportionalHazards.Fit(design, months, died, 2, tight);
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine($"  budget of {tight.MaximumIterations}      : refused, not converged");
        }

        Console.WriteLine();
    }
}
