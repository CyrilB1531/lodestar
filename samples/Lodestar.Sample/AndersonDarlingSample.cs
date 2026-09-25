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

        // The k-sample form: do two groups of heights share a distribution at all?
        double[] others = [181.0, 185.0, 179.0, 190.0, 183.0, 176.0, 188.0, 184.0];
        AndersonResult midrank = AndersonDarling.KSample(Heights, others);
        AndersonResult continuous = AndersonDarling.KSample(AndersonKSampleVariant.Continuous, Heights, others);
        Console.WriteLine($"  k-sample midrank      = {Inv.F4(midrank.Statistic)}, p {Inv.F4(midrank.PValue)}");
        Console.WriteLine($"  k-sample continuous   = {Inv.F4(continuous.Statistic)}, p {Inv.F4(continuous.PValue)}");
    }
}
