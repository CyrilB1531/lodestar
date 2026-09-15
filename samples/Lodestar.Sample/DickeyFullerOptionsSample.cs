using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>The trend terms, the lag rule and the maximum lag of an augmented Dickey-Fuller test.</summary>
internal static class DickeyFullerOptionsSample
{
    private static readonly double[] Walk =
        [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8, 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

    public static void Run()
    {
        Console.WriteLine("Dickey-Fuller options (Lodestar.Stats.TimeSeries)");

        var options = new DickeyFullerOptions
        {
            Regression = TrendTerms.ConstantAndTrend,
            LagSelection = LagSelection.Fixed,
            MaxLag = 1,
        };
        DickeyFullerResult result = Stationarity.AugmentedDickeyFuller(Walk, options);

        Console.WriteLine($"  {options.Regression}, {options.LagSelection}, lag {options.MaxLag}");
        Console.WriteLine($"  statistic        : {Inv.F3(result.Statistic)}");
        Console.WriteLine();
    }
}
