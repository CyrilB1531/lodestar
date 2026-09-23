using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>Pearson's r — a linear relationship, and the tail that says whether to believe it.</summary>
internal static class PearsonSample
{
    // Eight tasting panels: how much sugar the recipe carried, against the
    // sweetness score the panel gave it.
    private static readonly double[] Sugar = [4.0, 6.5, 5.0, 9.0, 7.5, 3.0, 8.0, 6.0];
    private static readonly double[] Sweetness = [3.1, 5.9, 4.0, 7.4, 6.8, 2.2, 6.1, 5.2];

    public static void Run()
    {
        Console.WriteLine("Pearson correlation");

        PearsonResult twoSided = Pearson.Test(Sugar, Sweetness);
        PearsonResult greater = Pearson.Test(Sugar, Sweetness, Alternative.Greater);

        Console.WriteLine($"  r                     = {Inv.F4(twoSided.Statistic)}");
        Console.WriteLine($"  two-sided p           = {Inv.E3(twoSided.PValue)}");
        Console.WriteLine($"  one-sided p           = {Inv.E3(greater.PValue)}");
    }
}
