using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>How many donors are averaged, and how they are weighted.</summary>
internal static class KnnImputerOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("KnnImputerOptions (Lodestar.Preprocessing)");

        double[] rows =
        [
            1.0, 2.0, double.NaN,
            3.0, 4.0, 3.0,
            double.NaN, 6.0, 5.0,
            8.0, 8.0, 7.0,
        ];

        var options = new KnnImputerOptions { NeighbourCount = 2 };
        Console.WriteLine($"  {options.NeighbourCount} donors, {options.Weights}: {Inv.List(KnnImputer.Fit(rows, 3, options).Transform(rows))}");

        // A nearer donor counts for more, which pulls the first gap from 4 down to 3.67.
        var weighted = options with { Weights = NeighbourWeights.Distance };
        Console.WriteLine($"  {weighted.NeighbourCount} donors, {weighted.Weights}: {Inv.List(KnnImputer.Fit(rows, 3, weighted).Transform(rows))}");
        Console.WriteLine();
    }
}
