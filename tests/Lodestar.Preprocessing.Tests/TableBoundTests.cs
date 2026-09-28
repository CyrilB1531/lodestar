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
}
