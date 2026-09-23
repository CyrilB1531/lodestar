using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>Spearman's rho — the same question as Pearson, asked of the ranks.</summary>
internal static class SpearmanSample
{
    // The dose rises with the effect at every step, but the last one jumps: a measure of
    // order sees a perfect relationship where a measure of distance sees two thirds of one.
    private static readonly double[] Dose = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 40.0];
    private static readonly double[] Effect = [2.0, 3.5, 4.0, 5.5, 6.0, 7.5, 8.0, 9.0];

    public static void Run()
    {
        Console.WriteLine("Spearman correlation");

        TestResult monotone = Spearman.Test(Dose, Effect);

        Console.WriteLine($"  rho                   = {Inv.F4(monotone.Statistic)}");
        Console.WriteLine($"  p                     = {Inv.E3(monotone.PValue)}");
        Console.WriteLine($"  Pearson r, same data  = {Inv.F4(Pearson.Test(Dose, Effect).Statistic)}");
    }
}
