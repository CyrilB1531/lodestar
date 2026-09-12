using Lodestar.Stats.Regression;

namespace Lodestar.Sample;

/// <summary>The three choices a fit takes, and what each one changes.</summary>
internal static class OlsOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("Fitting options (Lodestar.Stats.Regression)");

        double[] design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0];
        double[] response = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1];

        OlsSummary ninetyFive = OrdinaryLeastSquares.Fit(design, response, featureCount: 1);
        OlsSummary ninetyNine = OrdinaryLeastSquares.Fit(
            design, response, featureCount: 1, new OlsOptions { ConfidenceLevel = 0.99 });

        Console.WriteLine(
            $"  95%              : [{Inv.F4(ninetyFive.ConfidenceLower[1])}, {Inv.F4(ninetyFive.ConfidenceUpper[1])}]");
        Console.WriteLine(
            $"  99%              : [{Inv.F4(ninetyNine.ConfidenceLower[1])}, {Inv.F4(ninetyNine.ConfidenceUpper[1])}]");

        // Dropping the intercept scores the model against zero rather than against the
        // response's mean, so the uncentred R2 is larger without the fit being better.
        OlsSummary noIntercept = OrdinaryLeastSquares.Fit(
            design, response, featureCount: 1, new OlsOptions { WithIntercept = false });

        Console.WriteLine($"  R2 centred       : {Inv.F4(ninetyFive.RSquared)}");
        Console.WriteLine($"  R2 uncentred     : {Inv.F4(noIntercept.RSquared)}");

        // A robust covariance leaves the estimate alone and widens what it claims to know.
        // It also moves the tests to the normal, which OlsSummary.CovarianceType is what says.
        OlsSummary robust = OrdinaryLeastSquares.Fit(
            design, response, featureCount: 1,
            new OlsOptions { CovarianceType = CovarianceType.Hc3 });

        Console.WriteLine($"  SE ordinary      : {Inv.F4(ninetyFive.StandardErrors[1])}");
        Console.WriteLine($"  SE {robust.CovarianceType,-14}: {Inv.F4(robust.StandardErrors[1])}");
        Console.WriteLine();
    }
}
