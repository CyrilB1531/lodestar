using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Decision 0113: the expected table compares by value, row by row.</summary>
public sealed class Chi2ContingencyResultEqualityTests
{
    private static Chi2ContingencyResult Result(double[][] expected) =>
        new(Statistic: 1.5, PValue: 0.2, Dof: 1, ExpectedFrequencies: expected);

    [Fact]
    public void Separate_tables_holding_the_same_frequencies_are_equal()
    {
        Chi2ContingencyResult left = Result([[1.0, 2.0], [3.0, 4.0]]);
        Chi2ContingencyResult right = Result([[1.0, 2.0], [3.0, 4.0]]);

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void A_table_differing_inside_a_row_is_unequal()
    {
        Chi2ContingencyResult left = Result([[1.0, 2.0], [3.0, 4.0]]);
        Chi2ContingencyResult right = Result([[1.0, 2.0], [3.0, 4.5]]);

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void A_table_with_a_differing_row_count_is_unequal()
    {
        Chi2ContingencyResult left = Result([[1.0, 2.0], [3.0, 4.0]]);
        Chi2ContingencyResult right = Result([[1.0, 2.0]]);

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void A_row_of_a_differing_width_is_unequal()
    {
        Chi2ContingencyResult left = Result([[1.0, 2.0]]);
        Chi2ContingencyResult right = Result([[1.0, 2.0, 3.0]]);

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void A_differing_statistic_is_unequal()
    {
        Chi2ContingencyResult left = Result([[1.0]]);
        Chi2ContingencyResult right = new(2.5, 0.2, 1, [[1.0]]);

        Assert.NotEqual(left, right);
    }
}
