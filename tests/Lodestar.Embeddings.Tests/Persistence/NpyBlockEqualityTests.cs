using Lodestar.Embeddings.Persistence;
using Xunit;

namespace Lodestar.Embeddings.Tests.Persistence;

/// <summary>A block compares its values and its shape element by element, not the arrays holding them.</summary>
public sealed class NpyBlockEqualityTests
{
    private static NpyBlock Block(int rows, int columns) =>
        new(new float[rows * columns], [rows, columns]);

    [Fact]
    public void Blocks_holding_the_same_values_and_shape_in_separate_arrays_are_equal()
    {
        Assert.Equal(Block(1, 2), Block(1, 2));
        Assert.Equal(Block(1, 2).GetHashCode(), Block(1, 2).GetHashCode());
    }

    [Fact]
    public void Blocks_differing_in_one_dimension_are_unequal()
    {
        Assert.NotEqual(Block(1, 2), Block(2, 1));
    }

    [Fact]
    public void A_default_block_equals_another_and_not_a_shaped_one()
    {
        Assert.Equal(default, default(NpyBlock));
        Assert.NotEqual(default, Block(1, 1));
        Assert.NotEqual(Block(1, 1), default);
    }
}
