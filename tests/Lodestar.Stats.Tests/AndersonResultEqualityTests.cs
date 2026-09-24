using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Two results holding the same statistic and tables are equal, whichever arrays hold them.</summary>
public sealed class AndersonResultEqualityTests
{
    private static AndersonResult Result(double statistic = 0.4, double pValue = 0.15, double critical = 0.787) =>
        new(statistic, pValue, [0.576, 0.656, critical], [15.0, 10.0, 5.0]);

    [Fact]
    public void Separate_tables_holding_the_same_numbers_are_equal()
    {
        AndersonResult left = Result();
        AndersonResult right = Result();

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void A_result_equals_itself_and_not_null()
    {
        AndersonResult result = Result();

        Assert.True(result.Equals(result));
        Assert.False(result.Equals(null));
    }

    [Fact]
    public void A_differing_statistic_or_p_value_is_unequal()
    {
        Assert.NotEqual(Result(), Result(statistic: 0.5));
        Assert.NotEqual(Result(), Result(pValue: 0.1));
    }

    [Fact]
    public void A_differing_critical_value_is_unequal()
    {
        Assert.NotEqual(Result(), Result(critical: 0.9));
    }

    [Fact]
    public void A_differing_significance_level_is_unequal()
    {
        AndersonResult left = Result();
        AndersonResult right = new(0.4, 0.15, [0.576, 0.656, 0.787], [15.0, 10.0, 2.5]);

        Assert.NotEqual(left, right);
    }

    /// <summary>A stored NaN compares equal to a stored NaN, as <c>double.Equals</c> has it.</summary>
    [Fact]
    public void A_NaN_statistic_equals_a_NaN_statistic()
    {
        Assert.Equal(Result(statistic: double.NaN), Result(statistic: double.NaN));
    }
}
