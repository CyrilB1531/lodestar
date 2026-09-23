using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>Levene's test — the assumption a one-way ANOVA has already made.</summary>
internal static class LeveneSample
{
    // Three machines filling the same bottle. They agree on where they aim and
    // disagree on how well they hold it, which is what the ANOVA cannot see.
    private static readonly double[] First = [20.1, 19.8, 20.3, 20.0, 19.9, 20.2, 20.1, 19.7];
    private static readonly double[] Second = [20.4, 18.9, 21.2, 19.1, 21.0, 18.7, 20.8, 19.4];
    private static readonly double[] Third = [20.0, 20.1, 19.9, 20.2, 19.8, 20.1, 20.0, 19.9];

    public static void Run()
    {
        Console.WriteLine("Levene's test for equal variance");

        TestResult median = Levene.Test(First, Second, Third);
        TestResult mean = Levene.Test(Center.Mean, 0.05, NanPolicy.Propagate, First, Second, Third);

        Console.WriteLine($"  W, around the median  = {Inv.F4(median.Statistic)}");
        Console.WriteLine($"  p                     = {Inv.E3(median.PValue)}");
        Console.WriteLine($"  W, around the mean    = {Inv.F4(mean.Statistic)}");
        Console.WriteLine($"  the ANOVA's own p     = {Inv.F4(OneWayAnova.Test(First, Second, Third).PValue)}");
    }
}
