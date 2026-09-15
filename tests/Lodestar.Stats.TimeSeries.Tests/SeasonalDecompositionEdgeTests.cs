using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>The decomposition's refusals, and what holds of it for every input.</summary>
public sealed class SeasonalDecompositionEdgeTests
{
    private static readonly double[] Quarterly = [10.0, 14.0, 8.0, 12.0, 11.0, 15.0, 9.0, 13.0, 12.0, 16.0, 10.0, 14.0];

    [Fact]
    public void A_period_below_two_is_refused() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => SeasonalDecomposition.Decompose(Quarterly, 1));

    [Fact]
    public void Fewer_than_two_full_periods_are_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => SeasonalDecomposition.Decompose(Quarterly, 7));
        Assert.Equal("series", error.ParamName);
        Assert.Contains("14", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Multiplicative_refuses_a_value_at_or_below_zero()
    {
        double[] series = (double[])Quarterly.Clone();
        series[5] = 0.0;

        ArgumentException error = Assert.Throws<ArgumentException>(() => SeasonalDecomposition.Decompose(
            series, 4, new SeasonalDecompositionOptions { Model = SeasonalModel.Multiplicative }));
        Assert.Equal("series", error.ParamName);
        Assert.Contains("row 5", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Additive_components_add_back_to_the_series_wherever_the_trend_is_defined()
    {
        SeasonalComponents components = SeasonalDecomposition.Decompose(Quarterly, 4);

        for (int i = 0; i < Quarterly.Length; i++)
        {
            if (!double.IsNaN(components.Trend[i]))
            {
                Assert.Equal(Quarterly[i], components.Trend[i] + components.Seasonal[i] + components.Residual[i], 12);
            }
        }
    }

    [Theory]
    [InlineData(SeasonalModel.Additive, 0.0)]
    [InlineData(SeasonalModel.Multiplicative, 1.0)]
    public void The_seasonal_pattern_centres_on_its_identity(SeasonalModel model, double identity)
    {
        SeasonalComponents components = SeasonalDecomposition.Decompose(
            Quarterly, 4, new SeasonalDecompositionOptions { Model = model });

        double mean = (components.Seasonal[0] + components.Seasonal[1] + components.Seasonal[2] + components.Seasonal[3]) / 4.0;
        Assert.Equal(identity, mean, 12);
    }
}
