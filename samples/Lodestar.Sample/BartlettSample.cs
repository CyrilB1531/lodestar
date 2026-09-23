using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>Bartlett's test — the same question as Levene's, under normality.</summary>
internal static class BartlettSample
{
    private static readonly double[] First = [20.1, 19.8, 20.3, 20.0, 19.9, 20.2, 20.1, 19.7];
    private static readonly double[] Second = [20.4, 18.9, 21.2, 19.1, 21.0, 18.7, 20.8, 19.4];
    private static readonly double[] Third = [20.0, 20.1, 19.9, 20.2, 19.8, 20.1, 20.0, 19.9];

    public static void Run()
    {
        Console.WriteLine("Bartlett's test for equal variance");

        TestResult result = Bartlett.Test(First, Second, Third);

        Console.WriteLine($"  T                     = {Inv.F4(result.Statistic)}");
        Console.WriteLine($"  p                     = {Inv.E3(result.PValue)}");
    }
}
