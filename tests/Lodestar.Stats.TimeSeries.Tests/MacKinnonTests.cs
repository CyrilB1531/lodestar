using Lodestar.Stats.TimeSeries.Internal;
using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>What holds of the response surface for every input, not only the corpus's.</summary>
public sealed class MacKinnonTests
{
    [Theory]
    [InlineData(TrendTerms.None)]
    [InlineData(TrendTerms.Constant)]
    [InlineData(TrendTerms.ConstantAndTrend)]
    [InlineData(TrendTerms.ConstantAndQuadraticTrend)]
    public void The_p_value_never_decreases_in_the_statistic_beyond_the_references_own_turn(TrendTerms regression)
    {
        // Under ct and ctt the large-p cubic turns over by about 3e-8 just below the table's maximum,
        // in statsmodels too, before the jump to 1; a switch-point mix-up moves p by far more than that.
        const double TheReferencesOwnTurn = 1e-7;
        double previous = 0.0;
        for (int step = 0; step <= 23_000; step++)
        {
            double statistic = -20.0 + (step * 0.001);
            double p = MacKinnon.PValue(statistic, regression);
            Assert.True(p >= previous - TheReferencesOwnTurn, $"{regression}: p({statistic}) = {p} fell below {previous}.");
            previous = p;
        }
    }

    [Fact]
    public void The_normal_cdf_is_symmetric_and_halves_at_zero()
    {
        Assert.Equal(0.5, MacKinnon.NormalCdf(0.0), 15);
        Assert.Equal(1.0, MacKinnon.NormalCdf(1.3) + MacKinnon.NormalCdf(-1.3), 15);
    }
}
