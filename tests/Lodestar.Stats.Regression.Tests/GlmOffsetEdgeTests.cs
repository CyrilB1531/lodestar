using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>What an offset and an exposure reduce to at their boundaries, and what each refuses (#787).</summary>
/// <remarks>
/// <c>stats_glm.json</c> holds the fits at 1e-9 against statsmodels; these hold the identities a corpus does not
/// state: an exposure is an offset of its logarithm, and two empty spans are the overload without them.
/// </remarks>
public sealed class GlmOffsetEdgeTests
{
    private static readonly double[] Design = [0.0, 1.0, 2.0, 3.0, 0.5, 1.5, 2.5, 3.5, 0.2, 1.2];

    private static readonly double[] Claims = [1.0, 4.0, 1.0, 12.0, 3.0, 4.0, 7.0, 3.0, 2.0, 3.0];

    private static readonly double[] Exposure = [1.0, 2.5, 0.5, 4.0, 3.0, 1.5, 2.0, 0.75, 3.5, 1.25];

    [Fact]
    public void An_exposure_is_an_offset_of_its_logarithm()
    {
        double[] logged = [.. Exposure.Select(value => Math.Log(value))];

        GlmSummary exposed = GeneralizedLinearModel.Fit(Design, Claims, [], Exposure, 1, GlmFamily.Poisson);
        GlmSummary offset = GeneralizedLinearModel.Fit(Design, Claims, logged, [], 1, GlmFamily.Poisson);

        Assert.Equal(offset.Coefficients, exposed.Coefficients);
        Assert.Equal(offset.StandardErrors, exposed.StandardErrors);
        Assert.Equal(offset.NullDeviance, exposed.NullDeviance);
    }

    [Fact]
    public void Two_empty_spans_are_the_fit_without_them()
    {
        GlmSummary plain = GeneralizedLinearModel.Fit(Design, Claims, 1, GlmFamily.Poisson);
        GlmSummary empty = GeneralizedLinearModel.Fit(Design, Claims, [], [], 1, GlmFamily.Poisson);

        Assert.Equal(plain.Coefficients, empty.Coefficients);
        Assert.Equal(plain.StandardErrors, empty.StandardErrors);
        Assert.Equal(plain.NullDeviance, empty.NullDeviance);
    }

    [Fact]
    public void An_offset_of_zeros_fits_the_same_model()
    {
        // The null deviance comes from a refit rather than the mean, so it agrees to the refit's convergence only.
        GlmSummary plain = GeneralizedLinearModel.Fit(Design, Claims, 1, GlmFamily.Poisson);
        GlmSummary zeros = GeneralizedLinearModel.Fit(Design, Claims, new double[10], [], 1, GlmFamily.Poisson);

        for (int j = 0; j < plain.Coefficients.Count; j++)
        {
            Assert.Equal(plain.Coefficients[j], zeros.Coefficients[j], 1e-12);
            Assert.Equal(plain.StandardErrors[j], zeros.StandardErrors[j], 1e-12);
        }

        Assert.Equal(plain.NullDeviance, zeros.NullDeviance, 1e-9);
    }

    [Theory]
    [InlineData(GlmFamily.Binomial, GlmLink.Default)]
    [InlineData(GlmFamily.Gamma, GlmLink.Inverse)]
    public void An_exposure_without_a_log_link_is_refused(GlmFamily family, GlmLink link)
    {
        double[] response = family == GlmFamily.Binomial
            ? [0.0, 1.0, 0.0, 1.0, 1.0, 0.0, 1.0, 0.0, 1.0, 1.0]
            : Exposure;

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => GeneralizedLinearModel.Fit(Design, response, [], Exposure, 1, family, new GlmOptions { Link = link }));

        Assert.Equal("exposure", error.ParamName);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void An_exposure_that_is_not_positive_and_finite_is_refused(double bad)
    {
        double[] exposure = [.. Exposure];
        exposure[3] = bad;

        ArgumentOutOfRangeException error = Assert.Throws<ArgumentOutOfRangeException>(
            () => GeneralizedLinearModel.Fit(Design, Claims, [], exposure, 1, GlmFamily.Poisson));

        Assert.Equal("exposure", error.ParamName);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.NegativeInfinity)]
    public void A_non_finite_offset_is_refused(double bad)
    {
        double[] offset = new double[10];
        offset[5] = bad;

        ArgumentOutOfRangeException error = Assert.Throws<ArgumentOutOfRangeException>(
            () => GeneralizedLinearModel.Fit(Design, Claims, offset, [], 1, GlmFamily.Poisson));

        Assert.Equal("offset", error.ParamName);
    }

    [Fact]
    public void Spans_of_the_wrong_length_are_refused()
    {
        Assert.Equal(
            "offset",
            Assert.Throws<ArgumentException>(
                () => GeneralizedLinearModel.Fit(Design, Claims, [0.1, 0.2], [], 1, GlmFamily.Poisson)).ParamName);
        Assert.Equal(
            "exposure",
            Assert.Throws<ArgumentException>(
                () => GeneralizedLinearModel.Fit(Design, Claims, [], [1.0, 2.0], 1, GlmFamily.Poisson)).ParamName);
    }
}
