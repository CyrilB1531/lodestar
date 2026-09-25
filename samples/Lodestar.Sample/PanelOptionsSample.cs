using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Panel;

namespace Lodestar.Sample;

/// <summary>Two-way effects clustered by entity, then Driscoll-Kraay at a chosen bandwidth.</summary>
internal static class PanelOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("Panel options (Lodestar.Stats.Regression)");

        int[] entities = [1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4];
        int[] periods = [2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023];
        double[] x = [0.5, 1.1, 1.9, 2.4, 1.2, 1.8, 2.9, 3.1, -0.3, 0.4, 0.8, 1.6, 2.0, 2.2, 3.1, 3.9];
        double[] y = [2.1, 3.0, 4.2, 4.9, 4.4, 5.1, 6.8, 7.0, 0.2, 1.3, 1.7, 3.1, 6.1, 6.3, 7.9, 9.2];
        var design = new PanelDesign(y, x, 1, entities, periods);

        var twoWay = new PanelOptions
        {
            WithIntercept = true,
            EntityEffects = true,
            TimeEffects = true,
            CovarianceType = PanelCovarianceType.Clustered,
            ClusterEntity = true,
            ClusterTime = false,
            Debiased = true,
            ConfidenceLevel = 0.9,
        };
        PanelSummary clustered = PanelRegression.FixedEffects(design, twoWay);
        Console.WriteLine(
            $"  two-way             : entity {twoWay.EntityEffects}, time {twoWay.TimeEffects}, intercept {twoWay.WithIntercept}, "
            + $"{twoWay.CovarianceType} by entity {twoWay.ClusterEntity}, by time {twoWay.ClusterTime}");
        Console.WriteLine(
            $"  slope, s.e.         : {Inv.F4(clustered.Coefficients[1])}, {Inv.F4(clustered.StandardErrors[1])}, "
            + $"debiased {twoWay.Debiased}, level {Inv.F1(twoWay.ConfidenceLevel)}");

        var kernel = new PanelOptions
        {
            EntityEffects = true,
            CovarianceType = PanelCovarianceType.Kernel,
            Kernel = KernelType.Parzen,
            Bandwidth = 2,
        };
        PanelSummary driscollKraay = PanelRegression.FixedEffects(design, kernel);
        Console.WriteLine($"  Driscoll-Kraay      : {kernel.Kernel} at {kernel.Bandwidth} lags, s.e. {Inv.F4(driscollKraay.StandardErrors[1])}");
        Console.WriteLine();
    }
}
