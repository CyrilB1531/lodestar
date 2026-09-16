using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>Everything a vector autoregression reports beyond its coefficients.</summary>
internal static class VarSummarySample
{
    public static void Run()
    {
        Console.WriteLine("The VAR summary table (Lodestar.Stats.TimeSeries)");

        VarSummary fit = VectorAutoregression.Fit(Corpus.VarSeries, variableCount: 2, lagOrder: 1);

        Console.WriteLine(
            $"  first equation   : {Inv.F4(fit.Coefficients[0][0])} + {Inv.F4(fit.Coefficients[0][1])}·y1(-1)"
            + $" + {Inv.F4(fit.Coefficients[0][2])}·y2(-1)");
        Console.WriteLine(
            $"  its errors       : {Inv.F4(fit.StandardErrors[0][0])}, {Inv.F4(fit.StandardErrors[0][1])}"
            + $", {Inv.F4(fit.StandardErrors[0][2])}");
        Console.WriteLine(
            $"  first t / p      : {Inv.F4(fit.TStatistics[0][1])} / {Inv.F4(fit.PValues[0][1])}");
        Console.WriteLine(
            $"  residual var     : {Inv.F4(fit.ResidualCovariance[0])}"
            + $" (ML {Inv.F4(fit.ResidualCovarianceMaximumLikelihood[0])})");
        Console.WriteLine(
            $"  log-likelihood   : {Inv.F4(fit.LogLikelihood)}, AIC {Inv.F4(fit.Akaike)}, BIC {Inv.F4(fit.Bayesian)}");
        Console.WriteLine(
            $"  HQ / FPE         : {Inv.F4(fit.HannanQuinn)} / {Inv.F4(fit.FinalPredictionError)}");
        Console.WriteLine(
            $"  shape            : {fit.VariableCount} variables, {fit.ModelDegreesOfFreedom} parameters per equation"
            + $", {fit.ResidualDegreesOfFreedom} residual d.f., intercept {fit.HasIntercept}");
        Console.WriteLine();
    }
}
