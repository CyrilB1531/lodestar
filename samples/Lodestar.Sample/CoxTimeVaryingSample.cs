using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>A covariate that changes during follow-up, one row per interval it held.</summary>
internal static class CoxTimeVaryingSample
{
    public static void Run()
    {
        Console.WriteLine("CoxTimeVarying (Lodestar.Survival)");

        // Six subjects; three change their exposure partway, and each row is the interval (start, stop] it held.
        double[] exposure = [0.5, 1.5, -0.3, 0.0, 0.8, -1.2, 1.1, 2.0, -0.5];
        double[] starts = [0, 4, 0, 0, 6, 0, 0, 3, 0];
        double[] stops = [4, 9, 7, 6, 12, 10, 3, 8, 5];
        bool[] ends = [false, true, true, false, true, false, false, true, true];

        CoxSummary fit = CoxTimeVarying.Fit(exposure, starts, stops, ends, featureCount: 1);
        CoxSummary weighted = CoxTimeVarying.Fit(exposure, starts, stops, ends, [1, 1, 2, 1, 1, 1, 1, 1, 2], [], 1);
        Console.WriteLine($"  coefficient      : {Inv.F4(fit.Coefficients[0])} (se {Inv.F4(fit.StandardErrors[0])})");
        Console.WriteLine($"  weighted         : {Inv.F4(weighted.Coefficients[0])}");
        Console.WriteLine();
    }
}
