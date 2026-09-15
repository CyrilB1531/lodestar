using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>Every option refuses where it is set, naming the value.</summary>
public sealed class OptionsValidationTests
{
    [Fact]
    public void A_negative_maximum_lag_is_refused() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new DickeyFullerOptions { MaxLag = -1 });

    [Fact]
    public void A_null_maximum_lag_means_the_default_rule() =>
        Assert.Null(new DickeyFullerOptions().MaxLag);

    [Theory]
    [InlineData(TrendTerms.None)]
    [InlineData(TrendTerms.ConstantAndQuadraticTrend)]
    public void Kpss_refuses_terms_it_does_not_define(TrendTerms terms) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new KpssOptions { Regression = terms });

    [Fact]
    public void A_negative_kpss_lag_count_is_refused() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new KpssOptions { LagCount = -1 });

    [Fact]
    public void A_negative_extrapolation_is_refused() =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new SeasonalDecompositionOptions { ExtrapolateTrend = -1 });

    [Fact]
    public void The_defaults_are_the_references() =>
        Assert.Equal(
            (TrendTerms.Constant, LagSelection.Akaike, TrendTerms.Constant, KpssLagRule.Automatic, SeasonalModel.Additive, true, 0),
            (new DickeyFullerOptions().Regression, new DickeyFullerOptions().LagSelection,
             new KpssOptions().Regression, new KpssOptions().LagRule,
             new SeasonalDecompositionOptions().Model, new SeasonalDecompositionOptions().TwoSided,
             new SeasonalDecompositionOptions().ExtrapolateTrend));
}
