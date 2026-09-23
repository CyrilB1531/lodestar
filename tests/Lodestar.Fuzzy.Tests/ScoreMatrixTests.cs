using Xunit;

namespace Lodestar.Fuzzy.Tests;

/// <summary>The refusals and the equality <c>ScoreMatrix</c> carries, which no corpus can state.</summary>
/// <remarks>
/// A frozen corpus holds values; a refusal has none, and equality on a handle is this package's
/// own reading rather than the reference's. Both belong here rather than in the replay (#1123).
/// </remarks>
public sealed class ScoreMatrixTests
{
    private static readonly string[] Queries = ["new york", "boston"];
    private static readonly string[] Choices = ["new york mets", "boston red sox", "atlanta braves"];

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(2, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 3)]
    public void An_index_outside_the_matrix_is_refused(int row, int column)
    {
        ScoreMatrix scores = Process.Cdist(Queries, Choices);

        Assert.Throws<ArgumentOutOfRangeException>(() => scores[row, column]);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(2)]
    public void A_row_outside_the_matrix_is_refused(int row)
    {
        ScoreMatrix scores = Process.Cdist(Queries, Choices);

        Assert.Throws<ArgumentOutOfRangeException>(() => scores.Row(row).Length);
    }

    /// <summary>Equality asks whether two handles are the same matrix, not whether the cells agree.</summary>
    [Fact]
    public void Equality_is_on_the_handle_rather_than_the_cells()
    {
        ScoreMatrix scores = Process.Cdist(Queries, Choices);
        ScoreMatrix sameHandle = scores;
        ScoreMatrix scoredAgain = Process.Cdist(Queries, Choices);

        Assert.True(sameHandle == scores);
        Assert.True(sameHandle.Equals(scores));
        Assert.True(sameHandle.Equals((object)scores));
        Assert.Equal(scores.GetHashCode(), sameHandle.GetHashCode());

        // The same scores, computed twice: equal cell by cell, and not the same matrix.
        Assert.Equal(scores.ToArray(), scoredAgain.ToArray());
        Assert.True(scoredAgain != scores);
        Assert.False(scoredAgain.Equals((object)scores));
        Assert.False(scores.Equals("not a matrix"));
    }

    /// <summary>A shape that differs is not equal either, which is what the hash is built from.</summary>
    [Fact]
    public void A_different_shape_is_a_different_matrix()
    {
        ScoreMatrix wide = Process.Cdist(Queries, Choices);
        ScoreMatrix narrow = Process.Cdist(Queries, [Choices[0]]);

        Assert.True(wide != narrow);
        Assert.NotEqual(wide.GetHashCode(), narrow.GetHashCode());
    }

    /// <summary>The empty shapes the reference answers rather than refuses.</summary>
    [Fact]
    public void An_empty_side_gives_an_empty_matrix_of_that_shape()
    {
        ScoreMatrix noChoices = Process.Cdist(Queries, []);
        ScoreMatrix noQueries = Process.Cdist([], Choices);

        Assert.Equal(2, noChoices.Rows);
        Assert.Equal(0, noChoices.Columns);
        Assert.Empty(noChoices.ToArray());
        Assert.True(noChoices.Row(0).IsEmpty);

        Assert.Equal(0, noQueries.Rows);
        Assert.Equal(3, noQueries.Columns);
        Assert.Empty(noQueries.ToArray());
    }

    /// <summary>A default handle carries nothing, and says so rather than dereferencing null.</summary>
    [Fact]
    public void A_default_matrix_is_empty_rather_than_broken()
    {
        ScoreMatrix empty = default;

        Assert.Equal(0, empty.Rows);
        Assert.Equal(0, empty.Columns);
        Assert.Empty(empty.ToArray());
    }

    [Fact]
    public void A_null_list_is_refused()
    {
        Assert.Throws<ArgumentNullException>(() => Process.Cdist(null!, Choices));
        Assert.Throws<ArgumentNullException>(() => Process.Cdist(Queries, null!));
    }

    /// <summary>Past <c>int.MaxValue</c> cells the call is refused rather than overflowing the array.</summary>
    /// <remarks>
    /// The lists are counted, never read: <see cref="IReadOnlyList{T}"/> lets a fake report a
    /// count it does not hold, so the refusal can be proven without 46,341 strings a side.
    /// </remarks>
    [Fact]
    public void A_matrix_past_int_MaxValue_is_refused()
    {
        var huge = new CountOnly(46_341);

        Assert.Throws<ArgumentOutOfRangeException>(() => Process.Cdist(huge, huge));
    }

    /// <summary>A list that reports a count and refuses to be read, which is all the guard needs.</summary>
    private sealed class CountOnly(int count) : IReadOnlyList<string>
    {
        public int Count { get; } = count;

        public string this[int index] => throw new NotSupportedException("counted, never read");

        public IEnumerator<string> GetEnumerator() => throw new NotSupportedException("counted, never read");

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
