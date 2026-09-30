using Lodestar.Preprocessing.Internal;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>scipy's <c>_log_var</c> behind <see cref="PowerTransformer"/> on the inputs a fit rarely reaches (#1227).</summary>
public sealed class PowerSearchInternalsTests
{
    /// <summary>scipy.stats._morestats._log_var on two log-values, measured on scipy 1.18.1.</summary>
    [Theory]
    [InlineData(0.0, double.NegativeInfinity, -1.3862943611198906)]
    [InlineData(double.PositiveInfinity, 0.0, double.NaN)]
    [InlineData(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity)]
    [InlineData(1.0, 1.0, double.NegativeInfinity)]
    [InlineData(800.0, 800.0, double.NaN)]
    public void The_log_variance_keeps_scipy_s_answer_on_infinite_and_equal_terms(double a, double b, double expected)
    {
        double[] values = [a, b];

        Assert.Equal(expected, PowerLikelihood.LogVariance(values, new double[2]));
    }

    /// <summary>scipy.special.boxcox and inv_boxcox, and scikit-learn's _yeo_johnson_inverse_transform, at λ 0 and 2.</summary>
    [Fact]
    public void The_degenerate_exponents_take_the_reference_s_own_branches()
    {
        Assert.Equal(0.6931471805599453, PowerLikelihood.BoxCox(2.0, 0.0));
        Assert.Equal(0.6931471805599453, PowerLikelihood.BoxCox(2.0, 1e-20));
        Assert.Equal(1.6487212707001282, PowerLikelihood.InverseBoxCox(0.5, 0.0), 15);
        Assert.Equal(0.6487212707001282, PowerLikelihood.InverseYeoJohnson(0.5, 0.0), 15);
        Assert.Equal(-0.41421356237309515, PowerLikelihood.InverseYeoJohnson(-0.5, 0.0), 15);
        Assert.Equal(0.41421356237309515, PowerLikelihood.InverseYeoJohnson(0.5, 2.0), 15);
        Assert.Equal(-0.6487212707001282, PowerLikelihood.InverseYeoJohnson(-0.5, 2.0), 15);
    }

    [Fact]
    public void Inverted_Yeo_Johnson_bounds_are_refused_in_fminbound_s_words()
    {
        // PowerTransformer().fit([[1e146], [-1e146], [3e145], [5e145]]) raises "The lower bound exceeds the upper bound."
        ArgumentException error = Assert.Throws<ArgumentException>(() => PowerTransformer.Fit([1e146, -1e146, 3e145, 5e145], 1));

        Assert.Equal("The lower bound exceeds the upper bound. (Parameter 'samples')", error.Message);
    }

    /// <summary>-boxcox_llf(0, x), -yeojohnson_llf(0, x) and -yeojohnson_llf(2, -x) on scipy 1.18.1: the logarithm branches.</summary>
    [Fact]
    public void The_objectives_take_the_logarithm_where_the_exponent_vanishes()
    {
        double[] column = [0.5, 1.0, 2.0, 4.0, 3.0];
        double[] negated = [-0.5, -1.0, -2.0, -4.0, -3.0];

        Assert.Equal(1.0837417553229172, new BoxCoxObjective(column).Negative(0.0), 12);
        Assert.Equal(1.094621405496457, new YeoJohnsonObjective(column).Negative(0.0), 12);
        Assert.Equal(1.094621405496457, new YeoJohnsonObjective(negated).Negative(2.0), 12);
    }
}
