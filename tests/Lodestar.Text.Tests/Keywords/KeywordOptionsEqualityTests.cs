using Xunit;
using Lodestar.Text.Keywords;

namespace Lodestar.Text.Tests.Keywords;

/// <summary>Decision 0112: StopWords compares as a set, and the hash carries presence only.</summary>
public sealed class KeywordOptionsEqualityTests
{
    [Fact]
    public void Rake_options_with_the_same_stop_words_in_separate_lists_are_equal()
    {
        RakeOptions left = new() { StopWords = ["the", "a"] };
        RakeOptions right = new() { StopWords = ["the", "a"] };

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void Rake_stop_words_compare_as_a_set_so_order_and_repetition_do_not_count()
    {
        RakeOptions left = new() { StopWords = ["the", "the", "a"] };
        RakeOptions right = new() { StopWords = ["a", "the"] };

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void Rake_options_differing_in_a_stop_word_are_unequal()
    {
        RakeOptions left = new() { StopWords = ["the"] };
        RakeOptions right = new() { StopWords = ["an"] };

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void Absent_rake_stop_words_equal_absent_ones_and_not_an_empty_set()
    {
        RakeOptions absent = new();
        RakeOptions alsoAbsent = new();
        RakeOptions empty = new() { StopWords = [] };

        Assert.Equal(absent, alsoAbsent);
        Assert.Equal(absent.GetHashCode(), alsoAbsent.GetHashCode());
        Assert.NotEqual(absent, empty);
    }

    [Fact]
    public void Rake_options_differing_in_a_scalar_are_unequal()
    {
        RakeOptions left = new() { MinLength = 1 };
        RakeOptions right = new() { MinLength = 2 };

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void TextRank_options_with_the_same_stop_words_in_separate_lists_are_equal()
    {
        TextRankOptions left = new() { StopWords = ["the", "a"] };
        TextRankOptions right = new() { StopWords = ["the", "a"] };

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void TextRank_stop_words_compare_as_a_set()
    {
        TextRankOptions left = new() { StopWords = ["the", "the"] };
        TextRankOptions right = new() { StopWords = ["the"] };

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void TextRank_options_differing_in_a_scalar_are_unequal()
    {
        TextRankOptions left = new() { Damping = 0.85 };
        TextRankOptions right = new() { Damping = 0.80 };

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void TextRank_options_differing_in_an_optional_word_count_are_unequal()
    {
        TextRankOptions left = new() { Words = 5 };
        TextRankOptions right = new();

        Assert.NotEqual(left, right);
    }
}
