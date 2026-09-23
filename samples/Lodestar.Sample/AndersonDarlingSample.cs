using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>Anderson-Darling — a normality test read against a table, not a p-value.</summary>
internal static class AndersonDarlingSample
{
    private static readonly double[] Heights =
        [172.0, 168.0, 175.0, 180.0, 169.0, 171.0, 177.0, 165.0, 173.0, 179.0];

    public static void Run()
    {
        Console.WriteLine("Anderson-Darling test");

        AndersonResult result = AndersonDarling.Test(Heights);

        Console.WriteLine($"  A squared             = {Inv.F4(result.Statistic)}");
        Console.WriteLine($"  critical at 5%        = {Inv.F4(result.CriticalValues[2])}");
        Console.WriteLine($"  p                     = {Inv.F4(result.PValue)}");
    }
}
