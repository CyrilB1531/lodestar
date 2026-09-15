using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>Two series that move together, each explained by both series' past.</summary>
internal static class VectorAutoregressionSample
{
    public static void Run()
    {
        Console.WriteLine("VectorAutoregression (Lodestar.Stats.TimeSeries)");

        double[] series = Corpus.VarSeries;

        VarSummary fit = VectorAutoregression.Fit(series, variableCount: 2, lagOrder: 1);

        Console.WriteLine($"  cross lag        : {Inv.F4(fit.Coefficients[1][1])}");
        Console.WriteLine($"  its p-value      : {Inv.F4(fit.PValues[1][1])}");
        Console.WriteLine($"  used             : {fit.ObservationsUsed} observations at lag {fit.LagOrder}");
        Console.WriteLine();
    }
}
