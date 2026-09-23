using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Turning a measurement into a category, three ways.</summary>
internal static class KBinsDiscretizerSample
{
    public static void Run()
    {
        Console.WriteLine("KBinsDiscretizer (Lodestar.Preprocessing)");

        double[] ages = [19.0, 22.0, 25.0, 31.0, 38.0, 44.0, 52.0, 61.0, 67.0, 74.0];

        var ordinal = new KBinsDiscretizerOptions { BinCount = 4, Encoding = BinEncoding.Ordinal };
        foreach (BinStrategy strategy in (BinStrategy[])
                 [BinStrategy.Quantile, BinStrategy.Uniform, BinStrategy.KMeans])
        {
            KBinsDiscretizer bins = KBinsDiscretizer.Fit(ages, 1, ordinal with { Strategy = strategy });
            Console.WriteLine($"  {strategy,-8} edges   : {Inv.List(bins.BinEdges[0])}");
            Console.WriteLine($"  {strategy,-8} codes   : {Inv.List(bins.Transform(ages))}");
        }

        KBinsDiscretizer quartiles = KBinsDiscretizer.Fit(ages, 1, ordinal);
        Console.WriteLine($"  fitted on        : {quartiles.SampleCount} rows x {quartiles.FeatureCount} features");
        Console.WriteLine($"  bins per feature : {string.Join(",", quartiles.BinCounts)}");

        // The inverse answers with the bin's centre: the value itself is gone.
        Console.WriteLine($"  centres          : {Inv.List(quartiles.InverseTransform([0.0, 1.0, 2.0, 3.0]))}");
        Console.WriteLine();
    }
}
