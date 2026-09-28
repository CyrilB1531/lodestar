using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>A table sized by two caller counts is refused past the largest array, not wrapped (#1314).</summary>
public sealed class TableBoundTests
{
    [Fact]
    public void An_encoding_past_the_largest_array_is_refused_before_it_is_allocated()
    {
        // 50,000 categories by 43,000 rows is 2.15e9 cells, past int.MaxValue, where it wrapped.
        string[] categories = [.. Enumerable.Range(0, 50_000).Select(i => $"c{i}")];
        OneHotEncoder<string> encoder = Encoders.OneHot<string>(categories, 1);

        ArgumentException refused = Assert.Throws<ArgumentException>(
            () => encoder.Transform([.. Enumerable.Repeat("c0", 43_000)]));

        Assert.Equal("values", refused.ParamName);
    }

    [Fact]
    public void Repeated_splits_past_the_largest_list_are_refused_by_the_repeat_count()
    {
        // 2 folds by 2^30 repeats wrapped negative and threw under "capacity" (#1318).
        ArgumentException refused = Assert.Throws<ArgumentException>(() => Splitters.RepeatedKFold(4, 2, 1 << 30, 0));

        Assert.Equal("repeatCount", refused.ParamName);
    }
}
