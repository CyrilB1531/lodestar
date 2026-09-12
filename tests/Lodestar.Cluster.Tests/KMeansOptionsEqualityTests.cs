using Xunit;

namespace Lodestar.Cluster.Tests;

/// <summary>Decision 0112: the centres compare by value, and the hash agrees.</summary>
public sealed class KMeansOptionsEqualityTests
{
    [Fact]
    public void Separate_arrays_holding_the_same_centres_are_equal()
    {
        KMeansOptions left = new() { InitialCentres = [1.0, 2.0], Seed = 3 };
        KMeansOptions right = new() { InitialCentres = [1.0, 2.0], Seed = 3 };

        Assert.Equal(left, right);
        Assert.True(left == right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void One_differing_centre_is_unequal()
    {
        KMeansOptions left = new() { InitialCentres = [1.0, 2.0] };
        KMeansOptions right = new() { InitialCentres = [1.0, 2.5] };

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void A_differing_length_is_unequal()
    {
        KMeansOptions left = new() { InitialCentres = [1.0, 2.0] };
        KMeansOptions right = new() { InitialCentres = [1.0] };

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void Absent_centres_equal_absent_centres_and_nothing_else()
    {
        KMeansOptions absent = new() { Seed = 7 };
        KMeansOptions alsoAbsent = new() { Seed = 7 };
        KMeansOptions present = new() { Seed = 7, InitialCentres = [] };

        Assert.Equal(absent, alsoAbsent);
        Assert.Equal(absent.GetHashCode(), alsoAbsent.GetHashCode());
        Assert.NotEqual(absent, present);
    }

    [Fact]
    public void A_differing_scalar_is_unequal()
    {
        KMeansOptions left = new() { InitialCentres = [1.0], Tolerance = 1e-4 };
        KMeansOptions right = new() { InitialCentres = [1.0], Tolerance = 1e-5 };

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void A_NaN_tolerance_equals_itself_so_equality_stays_reflexive()
    {
        KMeansOptions left = new() { Tolerance = double.NaN };
        KMeansOptions right = new() { Tolerance = double.NaN };

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void A_NaN_centre_equals_itself_too()
    {
        KMeansOptions left = new() { InitialCentres = [double.NaN, 1.0] };
        KMeansOptions right = new() { InitialCentres = [double.NaN, 1.0] };

        Assert.Equal(left, right);
    }
}
