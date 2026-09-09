using Lodestar.Text.Stemming;
using Xunit;

namespace Lodestar.Text.Tests.Stemming;

/// <summary>
/// What decision 0092 settled: the alphabet the tables are written in, and the two
/// shapes where this parts from <c>nltk</c> — neither of which the corpus can carry.
/// </summary>
public sealed class RomanianSnowballStemmerTests
{
    // The tables spell ş and ţ with a cedilla, as the description does, so a word
    // written the modern way keeps the endings they are part of. nltk agrees.
    [Theory]
    [InlineData("ştiinţă", "ştiinţ")]
    [InlineData("știință", "științ")]
    [InlineData("româneşte", "român")]
    [InlineData("românește", "româneșt")]
    public void The_two_spellings_of_s_and_t_are_two_different_words(string word, string expected)
    {
        Assert.Equal(expected, RomanianSnowballStemmer.Stem(word));
    }

    /// <summary>
    /// Step 3's region qualifies the search: "asem" reaches past RV, so the shorter
    /// "em" is what goes. Stopping at the longest match would leave the word whole.
    /// </summary>
    [Theory]
    [InlineData("casem", "cas")]
    [InlineData("cântasem", "cânt")]
    public void An_ending_past_RV_falls_through_to_a_shorter_one(string word, string expected)
    {
        Assert.Equal(expected, RomanianSnowballStemmer.Stem(word));
    }

    // Two shapes nltk reads differently, both needing an ending repeated or chained
    // in a way Romanian does not build. Decision 0092 measures and bounds them.
    [Fact]
    public void A_chained_derivational_suffix_measures_R2_against_the_word_it_left()
    {
        // nltk keeps the region it computed before step 1 rewrote the word, and
        // answers "dorm"; R2 is measured against what step 2 actually sees.
        Assert.Equal("dormat", RomanianSnowballStemmer.Stem("dormativitate"));
    }

    [Fact]
    public void A_doubled_verb_ending_is_read_where_it_ends_the_word()
    {
        // nltk tests the letter before the *first* occurrence in RV; this reads the
        // one before the occurrence that ends the word, and answers "posândând".
        Assert.Equal("posând", RomanianSnowballStemmer.Stem("posândând"));
    }

    /// <summary>An <c>i</c> or <c>u</c> between vowels is a consonant to the regions.</summary>
    [Theory]
    [InlineData("ploaie", "ploai")]
    [InlineData("femeia", "femei")]
    [InlineData("continuare", "continu")]
    public void An_i_or_u_between_vowels_is_marked_before_the_regions(string word, string expected)
    {
        Assert.Equal(expected, RomanianSnowballStemmer.Stem(word));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("o", "o")]
    [InlineData("om", "om")]
    public void A_word_too_short_for_a_region_comes_back_whole(string word, string expected)
    {
        Assert.Equal(expected, RomanianSnowballStemmer.Stem(word));
    }
}
