using Lodestar.Stats.TimeSeries;
using Xunit;

namespace Lodestar.Stats.Tests;

public sealed class SerialCorrelationEdgeTests
{
    private static readonly double[] Series =
        [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0];

    [Fact]
    public void Lag_zero_is_exactly_one_and_its_interval_is_a_point()
    {
        AutocorrelationResult result = SerialCorrelation.Autocorrelation(Series, lagCount: 4);

        Assert.Equal(1.0, result.Values[0]);
        Assert.Equal(1.0, result.ConfidenceLower[0]);
        Assert.Equal(1.0, result.ConfidenceUpper[0]);
        Assert.Equal(5, result.Values.Count);
    }

    [Fact]
    public void A_series_shorter_than_two_points_is_refused()
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => SerialCorrelation.Autocorrelation([1.0], lagCount: 1));

        Assert.Equal("series", refusal.ParamName);
        Assert.Contains("two points", refusal.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_constant_series_is_refused_rather_than_divided_by_zero()
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => SerialCorrelation.Autocorrelation([2.0, 2.0, 2.0, 2.0], lagCount: 2));

        Assert.Equal("series", refusal.ParamName);
        Assert.Contains("every value is", refusal.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void A_non_finite_value_is_refused(double value)
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => SerialCorrelation.Autocorrelation([1.0, 2.0, value, 4.0], lagCount: 2));

        Assert.Equal("series", refusal.ParamName);
        Assert.Contains("row 2", refusal.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void A_lag_count_below_one_is_refused(int lagCount)
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => SerialCorrelation.Autocorrelation(Series, lagCount));

        Assert.Equal("lagCount", refusal.ParamName);
    }

    [Fact]
    public void A_lag_count_reaching_the_series_length_is_refused()
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => SerialCorrelation.Autocorrelation(Series, lagCount: 10));

        Assert.Equal("lagCount", refusal.ParamName);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(double.NaN)]
    public void A_confidence_level_outside_the_open_unit_interval_is_refused(double level)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new AutocorrelationOptions { ConfidenceLevel = level });
    }

    [Fact]
    public void The_adjusted_estimator_divides_by_a_smaller_denominator()
    {
        AutocorrelationResult biased = SerialCorrelation.Autocorrelation(Series, 4);
        AutocorrelationResult adjusted = SerialCorrelation.Autocorrelation(
            Series, 4, new AutocorrelationOptions { Adjusted = true });

        // n / (n - k) > 1 at every lag past zero, and the ratio is exactly that factor.
        for (int lag = 1; lag <= 4; lag++)
        {
            Assert.Equal(
                biased.Values[lag] * (Series.Length / (double)(Series.Length - lag)),
                adjusted.Values[lag],
                12);
        }
    }

    [Fact]
    public void The_flat_band_is_the_same_width_at_every_lag()
    {
        AutocorrelationResult result = SerialCorrelation.Autocorrelation(
            Series, 4, new AutocorrelationOptions { BartlettConfidenceInterval = false });

        double width = result.ConfidenceUpper[1] - result.ConfidenceLower[1];
        for (int lag = 2; lag <= 4; lag++)
        {
            Assert.Equal(width, result.ConfidenceUpper[lag] - result.ConfidenceLower[lag], 12);
        }
    }

    [Fact]
    public void The_bartlett_band_widens_with_the_lag()
    {
        AutocorrelationResult result = SerialCorrelation.Autocorrelation(Series, 4);

        double previous = result.ConfidenceUpper[1] - result.ConfidenceLower[1];
        for (int lag = 2; lag <= 4; lag++)
        {
            double width = result.ConfidenceUpper[lag] - result.ConfidenceLower[lag];
            Assert.True(width >= previous, $"lag {lag}: {width} is not at least {previous}.");
            previous = width;
        }
    }

    [Fact]
    public void The_partial_function_reports_lag_zero_as_one()
    {
        AutocorrelationResult result =
            SerialCorrelation.PartialAutocorrelation(Series, lagCount: 4);

        Assert.Equal(1.0, result.Values[0]);
        Assert.Equal(1.0, result.ConfidenceLower[0]);
        Assert.Equal(1.0, result.ConfidenceUpper[0]);
    }

    [Fact]
    public void The_first_partial_coefficient_equals_the_first_autocorrelation()
    {
        // Levinson-Durbin's first reflection coefficient is r1/r0 by construction, and the
        // adjusted autocorrelation is the same ratio: numerator over n - 1, denominator over n.
        AutocorrelationResult partial =
            SerialCorrelation.PartialAutocorrelation(Series, lagCount: 3);
        AutocorrelationResult adjusted = SerialCorrelation.Autocorrelation(
            Series, 3, new AutocorrelationOptions { Adjusted = true });

        Assert.Equal(adjusted.Values[1], partial.Values[1], 12);
    }

    [Fact]
    public void The_partial_band_is_flat_even_though_the_autocorrelation_band_is_not()
    {
        AutocorrelationResult result =
            SerialCorrelation.PartialAutocorrelation(Series, lagCount: 4);

        double width = result.ConfidenceUpper[1] - result.ConfidenceLower[1];
        for (int lag = 2; lag <= 4; lag++)
        {
            Assert.Equal(width, result.ConfidenceUpper[lag] - result.ConfidenceLower[lag], 12);
        }
    }

    [Fact]
    public void A_partial_lag_count_past_half_the_series_is_refused()
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => SerialCorrelation.PartialAutocorrelation(Series, lagCount: 6));

        Assert.Equal("lagCount", refusal.ParamName);
    }

    [Fact]
    public void A_partial_lag_count_at_half_the_series_is_allowed()
    {
        AutocorrelationResult result =
            SerialCorrelation.PartialAutocorrelation(Series, lagCount: 5);

        Assert.Equal(6, result.Values.Count);
    }
}
