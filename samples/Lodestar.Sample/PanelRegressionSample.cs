using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Panel;

namespace Lodestar.Sample;

/// <summary>Four entities over four years, fitted by each of the four panel estimators.</summary>
internal static class PanelRegressionSample
{
    public static void Run()
    {
        Console.WriteLine("Panel regression (Lodestar.Stats.Regression)");

        int[] entities = [1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4];
        int[] periods = [2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023];
        double[] x = [0.5, 1.1, 1.9, 2.4, 1.2, 1.8, 2.9, 3.1, -0.3, 0.4, 0.8, 1.6, 2.0, 2.2, 3.1, 3.9];
        double[] y = [2.1, 3.0, 4.2, 4.9, 4.4, 5.1, 6.8, 7.0, 0.2, 1.3, 1.7, 3.1, 6.1, 6.3, 7.9, 9.2];
        int[] regions = [1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2];
        var design = new PanelDesign(y, x, 1, entities, periods);

        Console.WriteLine(
            $"  design              : {design.Response.Length} rows, {design.ExogenousCount} regressor ({design.Exogenous.Length} values), "
            + $"{design.Entities.Length} entity and {design.Periods.Length} period labels");

        var clustered = new PanelOptions { CovarianceType = PanelCovarianceType.Clustered };
        PanelSummary within = PanelRegression.FixedEffects(design, new PanelOptions { EntityEffects = true });
        PanelSummary byRegion = PanelRegression.FixedEffects(design, regions, clustered with { EntityEffects = true });
        PanelSummary between = PanelRegression.Between(design);
        PanelSummary betweenByRegion = PanelRegression.Between(design, regions, clustered);
        var noConstant = new PanelOptions { WithIntercept = false };
        PanelSummary differenced = PanelRegression.FirstDifference(design, noConstant);
        PanelSummary differencedByRegion = PanelRegression.FirstDifference(design, regions, clustered with { WithIntercept = false });
        PanelSummary random = PanelRegression.RandomEffects(design);
        PanelSummary randomByRegion = PanelRegression.RandomEffects(design, regions, clustered);

        Console.WriteLine($"  fixed effects       : {Inv.F4(within.Coefficients[1])}, s.e. {Inv.F4(within.StandardErrors[1])} ({Inv.F4(byRegion.StandardErrors[1])} by region)");
        Console.WriteLine($"  between             : {Inv.F4(between.Coefficients[1])}, s.e. {Inv.F4(between.StandardErrors[1])} ({Inv.F4(betweenByRegion.StandardErrors[1])} by region)");
        Console.WriteLine($"  first difference    : {Inv.F4(differenced.Coefficients[0])}, s.e. {Inv.F4(differenced.StandardErrors[0])} ({Inv.F4(differencedByRegion.StandardErrors[0])} by region)");
        Console.WriteLine($"  random effects      : {Inv.F4(random.Coefficients[1])}, s.e. {Inv.F4(random.StandardErrors[1])} ({Inv.F4(randomByRegion.StandardErrors[1])} by region)");
        Console.WriteLine();
    }
}
