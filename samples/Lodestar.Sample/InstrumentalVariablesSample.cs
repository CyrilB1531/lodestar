using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Instrumental;

namespace Lodestar.Sample;

/// <summary>A regressor correlated with the error, and the instruments that correct it.</summary>
internal static class InstrumentalVariablesSample
{
    public static void Run()
    {
        Console.WriteLine("Instrumental variables (Lodestar.Stats.Regression)");

        double[] response = [3.1, 4.0, 5.2, 4.4, 6.9, 7.1, 6.0, 8.8, 9.1, 8.2, 10.7, 11.3];
        double[] exogenous = [0.2, -1.0, 0.5, 1.3, -0.4, 0.9, -1.2, 0.1, 1.7, -0.6, 0.8, -0.3];
        double[] endogenous = [1.0, 1.4, 2.1, 1.8, 3.0, 3.3, 2.6, 3.9, 4.2, 3.7, 4.9, 5.4];
        double[] instruments =
            [0.9, 0.1, 1.5, -0.3, 2.2, 0.4, 1.7, 0.8, 3.1, -0.2, 3.3, 0.6,
             2.4, 1.1, 3.8, -0.5, 4.1, 0.9, 3.5, 0.2, 4.6, -0.1, 5.2, 0.7];
        var design = new IvDesign(response, exogenous, 1, endogenous, 1, instruments, 2);

        Console.WriteLine(
            $"  design               : {design.Response.Length} rows, {design.ExogenousCount} exogenous ({design.Exogenous.Length} values), "
            + $"{design.EndogenousCount} endogenous ({design.Endogenous.Length}), {design.InstrumentCount} instruments ({design.Instruments.Length})");

        IvSummary twoStage = InstrumentalVariables.TwoStageLeastSquares(design);
        IvSummary liml = InstrumentalVariables.Liml(design, new IvOptions { Fuller = 1.0 });
        IvSummary gmm = InstrumentalVariables.Gmm(
            design,
            new IvOptions { CovarianceType = IvCovarianceType.Kernel, Kernel = IvKernel.Parzen, Bandwidth = 2 });

        IvFirstStage first = twoStage.FirstStage[0];
        IvTest sargan = twoStage.Overidentification!;
        Console.WriteLine($"  2SLS effect / s.e.   : {Inv.F4(twoStage.Coefficients[2])} / {Inv.F4(twoStage.StandardErrors[2])}");
        Console.WriteLine($"  first-stage partial  : {Inv.F4(first.PartialRSquared)}, test {Inv.F1(first.InstrumentTest.Statistic)}");
        Console.WriteLine($"  Sargan (df {sargan.DegreesOfFreedom})       : {Inv.F4(sargan.Statistic)}, p {Inv.F4(sargan.PValue)}");
        Console.WriteLine($"  LIML-Fuller kappa    : {Inv.F4(liml.Kappa!.Value)}");
        Console.WriteLine($"  GMM effect, Hansen J : {Inv.F4(gmm.Coefficients[2])}, {Inv.F4(gmm.Overidentification!.Statistic)}");
        Console.WriteLine();
    }
}
