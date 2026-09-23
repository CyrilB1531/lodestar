using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Where the edges go, what a row carries, and which percentile convention decides.</summary>
internal static class KBinsDiscretizerOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("KBinsDiscretizerOptions (Lodestar.Preprocessing)");

        double[] six = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0];
        var ordinal = new KBinsDiscretizerOptions { BinCount = 3, Encoding = BinEncoding.Ordinal };

        // The percentile convention changes the edges, not merely how they are reached.
        foreach (QuantileMethod method in (QuantileMethod[])
                 [QuantileMethod.AveragedInvertedCdf, QuantileMethod.Linear])
        {
            KBinsDiscretizer bins = KBinsDiscretizer.Fit(six, 1, ordinal with { QuantileMethod = method });
            Console.WriteLine($"  {method,-20}: {Inv.List(bins.BinEdges[0])}");
        }

        var oneHot = new KBinsDiscretizerOptions { BinCount = ordinal.BinCount, Strategy = BinStrategy.Uniform };
        KBinsDiscretizer hot = KBinsDiscretizer.Fit(six, 1, oneHot);
        Console.WriteLine($"  {oneHot.Strategy}, {oneHot.Encoding}  : width {hot.OutputFeatureCount}, row for 4: {Inv.List(hot.Transform([4.0]))}");
        Console.WriteLine();
    }
}
