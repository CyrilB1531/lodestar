using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Instrumental;

namespace Lodestar.Sample;

/// <summary>Sargan's and Hansen's overidentification tests on one design.</summary>
internal static class WaldTestSample
{
    public static void Run()
    {
        Console.WriteLine("IV tests (Lodestar.Stats.Regression)");

        double[] response = [3.1, 4.0, 5.2, 4.4, 6.9, 7.1, 6.0, 8.8, 9.1, 8.2, 10.7, 11.3];
        double[] exogenous = [0.2, -1.0, 0.5, 1.3, -0.4, 0.9, -1.2, 0.1, 1.7, -0.6, 0.8, -0.3];
        double[] endogenous = [1.0, 1.4, 2.1, 1.8, 3.0, 3.3, 2.6, 3.9, 4.2, 3.7, 4.9, 5.4];
        double[] instruments =
            [0.9, 0.1, 1.5, -0.3, 2.2, 0.4, 1.7, 0.8, 3.1, -0.2, 3.3, 0.6,
             2.4, 1.1, 3.8, -0.5, 4.1, 0.9, 3.5, 0.2, 4.6, -0.1, 5.2, 0.7];
        var design = new IvDesign(response, exogenous, 1, endogenous, 1, instruments, 2);

        WaldTest sargan = InstrumentalVariables.TwoStageLeastSquares(design).Overidentification!;
        WaldTest hansen = InstrumentalVariables.Gmm(design).Overidentification!;
        Console.WriteLine($"  Sargan (df {sargan.DegreesOfFreedom})       : {Inv.F4(sargan.Statistic)}, p {Inv.F4(sargan.PValue)}");
        Console.WriteLine($"  Hansen J            : {Inv.F4(hansen.Statistic)}, chi-squared {hansen.DenominatorDegreesOfFreedom is null}");
        Console.WriteLine();
    }
}
