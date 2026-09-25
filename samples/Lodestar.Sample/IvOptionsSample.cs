using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Instrumental;

namespace Lodestar.Sample;

/// <summary>A kernel covariance at the automatic bandwidth, then at a chosen one.</summary>
internal static class IvOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("IV options (Lodestar.Stats.Regression)");

        double[] response = [3.1, 4.0, 5.2, 4.4, 6.9, 7.1, 6.0, 8.8, 9.1, 8.2, 10.7, 11.3];
        double[] exogenous = [0.2, -1.0, 0.5, 1.3, -0.4, 0.9, -1.2, 0.1, 1.7, -0.6, 0.8, -0.3];
        double[] endogenous = [1.0, 1.4, 2.1, 1.8, 3.0, 3.3, 2.6, 3.9, 4.2, 3.7, 4.9, 5.4];
        double[] instruments =
            [0.9, 0.1, 1.5, -0.3, 2.2, 0.4, 1.7, 0.8, 3.1, -0.2, 3.3, 0.6,
             2.4, 1.1, 3.8, -0.5, 4.1, 0.9, 3.5, 0.2, 4.6, -0.1, 5.2, 0.7];
        var design = new IvDesign(response, exogenous, 1, endogenous, 1, instruments, 2);

        IvSummary automatic = InstrumentalVariables.TwoStageLeastSquares(
            design, new IvOptions { CovarianceType = IvCovarianceType.Kernel });
        IvSummary chosen = InstrumentalVariables.TwoStageLeastSquares(
            design, new IvOptions { CovarianceType = IvCovarianceType.Kernel, Kernel = IvKernel.QuadraticSpectral, Bandwidth = 2 });
        Console.WriteLine($"  automatic bandwidth : {automatic.Bandwidth}");
        Console.WriteLine($"  s.e. at bandwidth 2 : {Inv.F4(chosen.StandardErrors[2])}");

        var weighted = new IvOptions
        {
            WithIntercept = true,
            ConfidenceLevel = 0.9,
            GmmWeightType = IvCovarianceType.Kernel,
            GmmWeightKernel = IvKernel.Bartlett,
            GmmWeightBandwidth = 3,
        };
        IvSummary gmm = InstrumentalVariables.Gmm(design, weighted);
        Console.WriteLine(
            $"  GMM weight          : {weighted.GmmWeightType}, {weighted.GmmWeightKernel} at {weighted.GmmWeightBandwidth} lags, "
            + $"intercept {weighted.WithIntercept}, level {Inv.F1(weighted.ConfidenceLevel)}");
        Console.WriteLine($"  GMM effect          : {Inv.F4(gmm.Coefficients[2])}");
        Console.WriteLine();
    }
}
