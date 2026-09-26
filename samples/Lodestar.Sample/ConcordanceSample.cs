using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>How well a score orders survival, censored subjects included.</summary>
internal static class ConcordanceSample
{
    public static void Run()
    {
        Console.WriteLine("Concordance (Lodestar.Survival)");

        double[] times = [1, 2, 3, 4, 5];
        double[] scores = [1.5, 1.0, 3.5, 4.0, 6.0];

        Console.WriteLine($"  all observed     : {Inv.F3(Concordance.Index(times, scores))}");
        Console.WriteLine($"  one censored     : {Inv.F3(Concordance.Index(times, scores, [true, true, false, true, true]))}");
        Console.WriteLine();
    }
}
