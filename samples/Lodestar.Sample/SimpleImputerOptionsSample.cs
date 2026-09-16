using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>The four strategies, the tie rule, and the feature with nothing in it.</summary>
internal static class SimpleImputerOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("SimpleImputerOptions (Lodestar.Preprocessing)");

        // 1 and 2 both appear twice: the reference fills with the smaller.
        double[] tied = [1.0, 1.0, 2.0, 2.0, double.NaN];
        foreach (ImputationStrategy strategy in (ImputationStrategy[])
                 [ImputationStrategy.Mean, ImputationStrategy.Median, ImputationStrategy.MostFrequent])
        {
            var options = new SimpleImputerOptions { Strategy = strategy };
            Console.WriteLine($"  {strategy,-13}: {Inv.List(SimpleImputer.Fit(tied, 1, options).Statistics)}");
        }

        var constant = new SimpleImputerOptions
        {
            Strategy = ImputationStrategy.Constant,
            FillValue = -1.0,
        };
        Console.WriteLine($"  Constant {Inv.F1(constant.FillValue)}   : {Inv.List(SimpleImputer.Fit(tied, 1, constant).Statistics)}");

        // A feature with no value at all is refused unless it is kept, where the
        // reference drops it and returns a narrower matrix.
        var kept = new SimpleImputerOptions { KeepEmptyFeatures = true };
        double[] empty = [1.0, double.NaN, 2.0, double.NaN];
        Console.WriteLine($"  KeepEmpty {kept.KeepEmptyFeatures}  : {Inv.List(SimpleImputer.Fit(empty, 2, kept).Statistics)}");
        Console.WriteLine();
    }
}
