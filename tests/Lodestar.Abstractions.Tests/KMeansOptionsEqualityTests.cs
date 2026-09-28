using System.Reflection;
using Lodestar.Cluster;
using Xunit;

namespace Lodestar.Abstractions.Tests;

/// <summary>
/// <c>KMeansOptions</c> compares its starting blocks through <c>ValueEquality</c> alone (#1329): decision 0003 leaves a
/// moved data type its structural equality and that helper, and no private member of its own.
/// </summary>
public sealed class KMeansOptionsEqualityTests
{
    [Fact]
    public void Sets_holding_the_same_blocks_compare_equal()
    {
        var left = new KMeansOptions { InitialCentreSets = [[1.0, 2.0], [3.0, double.NaN]] };
        var right = new KMeansOptions { InitialCentreSets = [[1.0, 2.0], [3.0, double.NaN]] };

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void Sets_differing_in_a_value_a_count_or_presence_compare_unequal()
    {
        var sets = new KMeansOptions { InitialCentreSets = [[1.0, 2.0]] };

        Assert.NotEqual(sets, new KMeansOptions { InitialCentreSets = [[1.0, 2.5]] });
        Assert.NotEqual(sets, new KMeansOptions { InitialCentreSets = [[1.0, 2.0], [1.0, 2.0]] });
        Assert.NotEqual(sets, new KMeansOptions());
        Assert.NotEqual(new KMeansOptions(), sets);
        Assert.Equal(new KMeansOptions(), new KMeansOptions());
    }

    [Fact]
    public void The_type_declares_no_static_helper_and_no_closure()
    {
        const BindingFlags Declared = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly;

        Assert.Empty(typeof(KMeansOptions).GetMethods(Declared));
        Assert.Empty(typeof(KMeansOptions).GetNestedTypes(BindingFlags.NonPublic));
    }
}
