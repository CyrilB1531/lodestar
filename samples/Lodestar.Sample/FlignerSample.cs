using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>Fligner-Killeen: equal spreads, asked of ranks.</summary>
internal static class FlignerSample
{
    public static void Run()
    {
        Console.WriteLine("Fligner-Killeen test");

        double[] steady = [20.1, 19.8, 20.3, 20.0, 19.9, 20.2, 20.1, 19.7];
        double[] erratic = [20.4, 18.9, 21.2, 19.1, 21.0, 18.7, 20.8, 19.4];
        TestResult median = Fligner.Test(steady, erratic);
        TestResult mean = Fligner.Test(Center.Mean, 0.05, NanPolicy.Propagate, steady, erratic);

        Console.WriteLine($"  median centre         : statistic {Inv.F4(median.Statistic)}, p {Inv.E3(median.PValue)}");
        Console.WriteLine($"  mean centre           : statistic {Inv.F4(mean.Statistic)}, p {Inv.E3(mean.PValue)}");
    }
}
