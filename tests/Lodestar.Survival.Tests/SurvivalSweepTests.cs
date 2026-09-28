using Lodestar.Survival.Internal;
using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>The invariant sweep after #1306: option equality, prediction table sizes (#1307, #1308).</summary>
public sealed class SurvivalSweepTests
{
    [Fact]
    public void Options_built_from_the_same_breakpoints_are_equal_and_hash_alike()
    {
        var left = new ParametricOptions { Breakpoints = [2.0, 5.0] };
        var right = new ParametricOptions { Breakpoints = [2.0, 5.0] };

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
        Assert.NotEqual(left, new ParametricOptions { Breakpoints = [2.0, 6.0] });
        Assert.NotEqual(left, left with { ConfidenceLevel = 0.9 });
    }

    [Fact]
    public void A_breakpoint_written_after_validation_does_not_reach_the_options()
    {
        var options = new ParametricOptions { Breakpoints = [2.0, 5.0] };

        options.Breakpoints[0] = -1.0;

        Assert.Equal([2.0, 5.0], options.Breakpoints);
    }

    [Fact]
    public void A_prediction_table_past_the_largest_array_is_refused_by_name()
    {
        // 46,341 squared is 2,147,488,281: past Array.MaxLength, and past int.MaxValue, where it wrapped.
        ArgumentException refused = Assert.Throws<ArgumentException>(() => ResultTable.Length(46_341, 46_341, "design"));

        Assert.Equal("design", refused.ParamName);
        Assert.Equal(6, ResultTable.Length(2, 3, "design"));
    }
}
