using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Panel;

namespace Lodestar.Sample;

/// <summary>The whole-model half of a panel table: the three R², the tests and the variance components.</summary>
internal static class PanelSummarySample
{
    public static void Run()
    {
        Console.WriteLine("Panel summary (Lodestar.Stats.Regression)");

        int[] entities = [1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4];
        int[] periods = [2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023];
        double[] x = [0.5, 1.1, 1.9, 2.4, 1.2, 1.8, 2.9, 3.1, -0.3, 0.4, 0.8, 1.6, 2.0, 2.2, 3.1, 3.9];
        double[] y = [2.1, 3.0, 4.2, 4.9, 4.4, 5.1, 6.8, 7.0, 0.2, 1.3, 1.7, 3.1, 6.1, 6.3, 7.9, 9.2];
        var design = new PanelDesign(y, x, 1, entities, periods);

        PanelSummary fixedEffects = PanelRegression.FixedEffects(
            design, new PanelOptions { EntityEffects = true, CovarianceType = PanelCovarianceType.Kernel });
        WaldTest model = fixedEffects.ModelTest!;
        WaldTest robust = fixedEffects.RobustModelTest!;
        WaldTest pooled = fixedEffects.PoolabilityTest!;
        Console.WriteLine(
            $"  R² / within / between / overall : {Inv.F4(fixedEffects.RSquared)} / {Inv.F4(fixedEffects.RSquaredWithin)} / "
            + $"{Inv.F4(fixedEffects.RSquaredBetween)} / {Inv.F4(fixedEffects.RSquaredOverall)}");
        Console.WriteLine(
            $"  F / robust / poolability        : {Inv.F1(model.Statistic)} / {Inv.F1(robust.Statistic)} / {Inv.F1(pooled.Statistic)}");
        Console.WriteLine(
            $"  {fixedEffects.CovarianceType} at {fixedEffects.Bandwidth} lags, debiased {fixedEffects.Debiased}: "
            + $"t {Inv.F4(fixedEffects.TStatistics[1])}, p {Inv.F4(fixedEffects.PValues[1])}");
        Console.WriteLine(
            $"  {Inv.F1(fixedEffects.ConfidenceLevel * 100)}% interval             : "
            + $"[{Inv.F4(fixedEffects.ConfidenceLower[1])}, {Inv.F4(fixedEffects.ConfidenceUpper[1])}]");
        Console.WriteLine(
            $"  rows / entities / periods / d.f. : {fixedEffects.ObservationCount} / {fixedEffects.EntityCount} / "
            + $"{fixedEffects.PeriodCount} / {fixedEffects.ResidualDegreesOfFreedom}, constant {fixedEffects.HasConstant}");

        PanelSummary random = PanelRegression.RandomEffects(design);
        Console.WriteLine(
            $"  σ²e / σ²u / ρ / θ₁              : {Inv.F4(random.ResidualVariance!.Value)} / {Inv.F4(random.EffectsVariance!.Value)} / "
            + $"{Inv.F4(random.Rho!.Value)} / {Inv.F4(random.Theta![0])}");
        Console.WriteLine();
    }
}
