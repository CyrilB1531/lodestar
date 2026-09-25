using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>The point-biserial correlation: a binary variable against a continuous one.</summary>
internal static class PointBiserialSample
{
    public static void Run()
    {
        Console.WriteLine("Point-biserial correlation");

        bool[] passed = [true, false, true, true, false, true, false, false, true, true];
        double[] hours = [12.0, 4.5, 9.0, 14.0, 6.0, 10.5, 3.0, 7.5, 11.0, 8.0];
        TestResult result = PointBiserial.Test(passed, hours, NanPolicy.Propagate);

        Console.WriteLine($"  r                     = {Inv.F4(result.Statistic)}");
        Console.WriteLine($"  p                     = {Inv.E3(result.PValue)}");
    }
}
