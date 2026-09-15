using Lodestar.Stats.Regression;

namespace Lodestar.Sample;

/// <summary>The inference table when neighbouring errors are correlated.</summary>
internal static class GeneralizedLeastSquaresSample
{
    public static void Run()
    {
        Console.WriteLine("Generalized least squares (Lodestar.Stats.Regression)");

        double[] time = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0];
        double[] reading = [2.4, 3.6, 6.9, 7.1, 10.8, 11.2, 15.1, 14.6, 19.3, 19.9];
        var covariance = new double[100];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                covariance[(i * 10) + j] = Math.Pow(0.6, Math.Abs(i - j));
            }
        }

        OlsSummary fit = GeneralizedLeastSquares.Fit(time, reading, covariance, featureCount: 1);
        OlsSummary ordinary = OrdinaryLeastSquares.Fit(time, reading, featureCount: 1);

        Console.WriteLine($"  slope / s.e.     : {Inv.F4(fit.Coefficients[1])} / {Inv.F4(fit.StandardErrors[1])}");
        Console.WriteLine($"  ordinary s.e.    : {Inv.F4(ordinary.StandardErrors[1])}");
        Console.WriteLine();
    }
}
