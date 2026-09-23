using Lodestar.Stats.Internal;
using Xunit;

namespace Lodestar.Stats.Tests.Internal;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible fixture; no security use.
#pragma warning disable S2245, CA5394

/// <summary>The merge sort's inversion count, against the quadratic definition it replaces.</summary>
public sealed class InversionsTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(17)]
    [InlineData(64)]
    [InlineData(257)]
    public void Matches_the_quadratic_definition_on_random_input(int length)
    {
        var random = new System.Random(1120 + length);
        int[] values = new int[length];
        for (int i = 0; i < length; i++)
        {
            // A small range on purpose: ties are where an inversion count goes wrong, and a
            // wide range would almost never produce one.
            values[i] = random.Next(0, 8);
        }

        long expected = Quadratic(values);
        Assert.Equal(expected, Inversions.Count([.. values], length, new int[length]));
    }

    [Fact]
    public void Counts_every_pair_of_a_reversed_sequence()
    {
        int[] values = [5, 4, 3, 2, 1];

        // Every one of the 5 * 4 / 2 pairs is out of order.
        Assert.Equal(10L, Inversions.Count(values, values.Length, new int[values.Length]));
    }

    [Fact]
    public void Counts_none_of_a_sequence_that_is_already_sorted()
    {
        int[] values = [1, 1, 2, 3, 3, 4];

        Assert.Equal(0L, Inversions.Count(values, values.Length, new int[values.Length]));
    }

    [Fact]
    public void Leaves_the_values_sorted()
    {
        int[] values = [3, 1, 2, 1];

        Inversions.Count(values, values.Length, new int[values.Length]);

        Assert.Equal([1, 1, 2, 3], values);
    }

    private static long Quadratic(int[] values)
    {
        long count = 0L;
        for (int i = 0; i < values.Length; i++)
        {
            for (int j = i + 1; j < values.Length; j++)
            {
                if (values[i] > values[j])
                {
                    count++;
                }
            }
        }

        return count;
    }
}

#pragma warning restore S2245, CA5394
