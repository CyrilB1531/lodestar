using Lodestar.Stats.Regression;
using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>What a vector autoregression refuses, and the least squares it reduces to equation by equation (#786).</summary>
public sealed class VectorAutoregressionEdgeTests
{
    private static readonly double[] Series =
    [
        0.1968, -0.1307, 0.2167, -0.8291, 0.2534, 0.0921, 0.2934, -0.0748, -0.2623, 0.2421,
        0.4235, -0.3523, -0.2909, 0.4475, -0.2417, -0.9916, -0.3567, -0.3337, -1.0869, -0.5651,
        -0.5519, -1.0074, -1.2172, -0.1457, 0.2603, -0.7061, -0.7272, 0.3055, 0.3998, -0.6067,
    ];

    [Fact]
    public void Each_equation_is_least_squares_on_the_stacked_lags()
    {
        const int variables = 2;
        VarSummary fit = VectorAutoregression.Fit(Series, variables, 1);

        // The same design by hand: a constant, then both variables' previous observation.
        int usable = (Series.Length / variables) - 1;
        var design = new double[usable * 2];
        var response = new double[usable];
        for (int row = 0; row < usable; row++)
        {
            design[row * 2] = Series[row * variables];
            design[(row * 2) + 1] = Series[(row * variables) + 1];
            response[row] = Series[((row + 1) * variables) + 1];
        }

        OlsSummary equation = OrdinaryLeastSquares.Fit(design, response, 2);

        for (int column = 0; column < 3; column++)
        {
            Assert.Equal(equation.Coefficients[column], fit.Coefficients[1][column], 1e-12);
            Assert.Equal(equation.StandardErrors[column], fit.StandardErrors[1][column], 1e-12);
        }
    }

    [Fact]
    public void The_residual_covariance_is_the_residuals_own()
    {
        const int variables = 2;
        VarSummary fit = VectorAutoregression.Fit(Series, variables, 1);
        int usable = fit.ObservationsUsed;

        // Residuals recomputed from the reported coefficients, then their cross-product over T − k.
        var residuals = new double[variables][];
        for (int equation = 0; equation < variables; equation++)
        {
            residuals[equation] = new double[usable];
            for (int row = 0; row < usable; row++)
            {
                double fitted = fit.Coefficients[equation][0]
                    + (fit.Coefficients[equation][1] * Series[row * variables])
                    + (fit.Coefficients[equation][2] * Series[(row * variables) + 1]);
                residuals[equation][row] = Series[((row + 1) * variables) + equation] - fitted;
            }
        }

        for (int i = 0; i < variables; i++)
        {
            for (int j = 0; j < variables; j++)
            {
                double total = 0.0;
                for (int row = 0; row < usable; row++)
                {
                    total += residuals[i][row] * residuals[j][row];
                }

                Assert.Equal(
                    total / (usable - fit.ModelDegreesOfFreedom), fit.ResidualCovariance[(i * variables) + j], 1e-12);
                Assert.Equal(total / usable, fit.ResidualCovarianceMaximumLikelihood[(i * variables) + j], 1e-12);
            }
        }
    }

    [Fact]
    public void One_variable_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => VectorAutoregression.Fit(Series, 1, 1));
    }

    [Fact]
    public void A_lag_order_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => VectorAutoregression.Fit(Series, 2, 0));
    }

    [Fact]
    public void A_series_that_is_not_whole_observations_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => VectorAutoregression.Fit(Series.AsSpan(0, 29), 2, 1));

        Assert.Equal("series", error.ParamName);
    }

    [Fact]
    public void A_non_finite_observation_is_refused()
    {
        double[] series = [.. Series];
        series[7] = double.NaN;

        ArgumentException error = Assert.Throws<ArgumentException>(() => VectorAutoregression.Fit(series, 2, 1));

        Assert.Equal("series", error.ParamName);
    }

    [Fact]
    public void A_lag_order_that_leaves_no_residual_degree_of_freedom_is_refused()
    {
        // Six observations of two variables at lag 2: four usable rows for five parameters per equation.
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => VectorAutoregression.Fit(Series.AsSpan(0, 12), 2, 2));

        Assert.Equal("series", error.ParamName);
    }
}
