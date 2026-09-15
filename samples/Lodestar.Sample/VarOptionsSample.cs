using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>The one choice a vector autoregression takes: whether each equation carries a constant.</summary>
internal static class VarOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("VAR options (Lodestar.Stats.TimeSeries)");

        var options = new VarOptions { WithIntercept = false };

        VarSummary withConstant = VectorAutoregression.Fit(Corpus.VarSeries, 2, 1);
        VarSummary without = VectorAutoregression.Fit(Corpus.VarSeries, 2, 1, options);

        Console.WriteLine($"  intercept fitted : {withConstant.HasIntercept} against {options.WithIntercept}");
        Console.WriteLine(
            $"  parameters       : {withConstant.ModelDegreesOfFreedom} against {without.ModelDegreesOfFreedom}");
        Console.WriteLine($"  AIC              : {Inv.F4(withConstant.Akaike)} against {Inv.F4(without.Akaike)}");
        Console.WriteLine();
    }
}
