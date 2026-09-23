using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>The whole table an Anderson-Darling result carries, and why it is the useful half.</summary>
internal static class AndersonResultSample
{
    private static readonly double[] Skewed = [1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 40.0];

    public static void Run()
    {
        Console.WriteLine("Anderson-Darling table");

        AndersonResult result = AndersonDarling.Test(Skewed);
        for (int i = 0; i < result.SignificanceLevels.Length; i++)
        {
            Console.WriteLine(
                $"  at {Inv.F1(result.SignificanceLevels[i])}%{new string(' ', 16)}= {Inv.F4(result.CriticalValues[i])}");
        }

        // The interpolation clamps at the ends of that table, so no sample can be
        // reported outside [0.01, 0.15] however far from normal it is.
        Console.WriteLine($"  A squared             = {Inv.F4(result.Statistic)}");
        Console.WriteLine($"  p, clamped            = {Inv.F4(result.PValue)}");
        Console.WriteLine($"  Shapiro-Wilk's p      = {Inv.E3(ShapiroWilk.Test(Skewed).PValue)}");
    }
}
