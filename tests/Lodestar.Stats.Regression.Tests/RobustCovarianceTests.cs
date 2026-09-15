using Lodestar.Decomposition;
using Lodestar.Stats.Regression.Internal;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>The leverages the HC2 and HC3 covariances weight their rows by.</summary>
/// <remarks>
/// <c>Leverages</c> reads them as rows of <c>X R⁻¹</c> since the solve stopped forming Q (#782). These hold them to
/// the thin Q of <c>QrDecomposition.Householder</c> — the same projection reached the long way — while the OLS
/// corpus holds HC2 and HC3 at 1e-9.
/// </remarks>
public sealed class RobustCovarianceTests
{
    /// <summary>Six rows of an intercept and two regressors, row-major.</summary>
    private static readonly double[] Design =
    [
        1.0, 0.5, 2.0,
        1.0, 1.5, -1.0,
        1.0, -0.3, 0.7,
        1.0, 2.2, 1.1,
        1.0, 0.9, -0.4,
        1.0, -1.1, 0.2,
    ];

    private static double[] Leverages()
    {
        (_, double[] inverseUpper) = LeastSquares.Solve(Design, 6, 3, withIntercept: false, new double[6]);
        return RobustCovariance.Leverages(Design, inverseUpper, rowCount: 6, parameterCount: 3);
    }

    [Fact]
    public void Each_leverage_is_its_row_of_the_thin_Q_against_itself()
    {
        QrDecomposition qr = QrDecomposition.Householder(Design, rowCount: 6, columnCount: 3);

        double[] leverages = Leverages();

        for (int row = 0; row < 6; row++)
        {
            double expected = 0.0;
            for (int column = 0; column < 3; column++)
            {
                double value = qr.Q[(row * 3) + column];
                expected += value * value;
            }

            Assert.Equal(expected, leverages[row], 1e-14);
        }
    }

    [Fact]
    public void The_leverages_sum_to_the_parameter_count()
    {
        // The hat matrix is a projection of rank p, so its trace is exactly p up to rounding.
        double[] leverages = Leverages();

        Assert.Equal(3.0, leverages.Sum(), 1e-12);
        Assert.All(leverages, value => Assert.InRange(value, 0.0, 1.0));
    }
}
