using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>What a Pearson result carries past its two numbers: an interval, at whichever level is asked for.</summary>
internal static class PearsonResultSample
{
    private static readonly double[] Sugar = [4.0, 6.5, 5.0, 9.0, 7.5, 3.0, 8.0, 6.0];
    private static readonly double[] Sweetness = [3.1, 5.9, 4.0, 7.4, 6.8, 2.2, 6.1, 5.2];

    public static void Run()
    {
        Console.WriteLine("Pearson confidence interval");

        PearsonResult result = Pearson.Test(Sugar, Sweetness);
        (double low95, double high95) = result.ConfidenceInterval();
        (double low99, double high99) = result.ConfidenceInterval(0.99);

        // Not symmetric about r: the Fisher z transform builds the interval where the
        // sampling distribution is normal, and there is little room above 0.98.
        Console.WriteLine($"  r                     = {Inv.F4(result.Statistic)}");
        Console.WriteLine($"  95% interval          = [{Inv.F4(low95)}, {Inv.F4(high95)}]");
        Console.WriteLine($"  99% interval          = [{Inv.F4(low99)}, {Inv.F4(high99)}]");
    }
}
