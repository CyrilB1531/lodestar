using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// Strings and integers find their category by hash; every other type keeps the sorted search,
/// because a comparison of zero is not a hash match for it: 0.0 and -0.0 are one category.
/// </summary>
public sealed class CategoryLookupTests
{
    [Fact]
    public void A_floating_category_is_found_by_comparison_so_a_signed_zero_matches_its_twin()
    {
        double[] fitted = [2.5, 0.0, -1.0, 2.5];
        OrdinalEncoder<double> ordinal = Encoders.Ordinal<double>(fitted, 1);
        OneHotEncoder<double> oneHot = Encoders.OneHot<double>(fitted, 1);

        Assert.Equal([1.0, 1.0, 0.0, 2.0], ordinal.Transform([0.0, -0.0, -1.0, 2.5]));
        Assert.Equal([0.0, 1.0, 0.0], oneHot.Transform([-0.0]));
        Assert.Throws<ArgumentException>(() => ordinal.Transform([7.0]));
    }

    [Fact]
    public void An_integral_category_is_found_by_hash_in_every_feature()
    {
        long[] fitted = [30L, 1L, 10L, 2L, 20L, 1L];
        OrdinalEncoder<long> ordinal = Encoders.Ordinal<long>(fitted, 2);

        Assert.Equal([2.0, 1.0, 0.0, 0.0], ordinal.Transform([30L, 2L, 10L, 1L]));
        Assert.Throws<ArgumentException>(() => ordinal.Transform([1L, 1L]));
    }
}
