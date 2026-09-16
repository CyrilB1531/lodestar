using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Widening the percentile range, and turning each step off on its own.</summary>
internal static class RobustScalerOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("RobustScalerOptions (Lodestar.Preprocessing)");

        double[] samples = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0];

        var deciles = new RobustScalerOptions { LowerPercentile = 10.0, UpperPercentile = 90.0 };
        RobustScaler wide = RobustScaler.Fit(samples, featureCount: 1, deciles);
        Console.WriteLine(
            $"  percentiles      : {Inv.F1(deciles.LowerPercentile)} to {Inv.F1(deciles.UpperPercentile)}"
            + $", range {Inv.List(wide.Scale!)}");

        // Each switch decides exactly one statistic: no other scaler here is that simple.
        var uncentred = new RobustScalerOptions { WithCentring = false, WithScaling = true };
        RobustScaler scaled = RobustScaler.Fit(samples, 1, uncentred);
        Console.WriteLine($"  centring {uncentred.WithCentring}   : median is {(scaled.Centre is null ? "absent" : "present")}");

        var unscaled = new RobustScalerOptions { WithScaling = false };
        RobustScaler centred = RobustScaler.Fit(samples, 1, unscaled);
        Console.WriteLine($"  scaling {unscaled.WithScaling}    : range is {(centred.Scale is null ? "absent" : "present")}");

        // Unit variance divides the range again, by the normal quantiles of the two
        // percentiles, so a normal column comes out with a standard deviation of 1.
        var unit = new RobustScalerOptions { UnitVariance = true };
        RobustScaler standardised = RobustScaler.Fit(samples, 1, unit);
        Console.WriteLine(
            $"  unit variance {unit.UnitVariance} : range {Inv.List(RobustScaler.Fit(samples, 1).Scale!)}"
            + $" becomes {Inv.List(standardised.Scale!)}");
        Console.WriteLine();
    }
}
