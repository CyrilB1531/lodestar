using Lodestar.Text.Stemming;
using Xunit;

namespace Lodestar.Text.Tests.Stemming;

/// <summary>
/// What decision 0086 settled, which the frozen corpus cannot carry: the one
/// place the Russian stemmer is deliberately not <c>nltk</c>, and the three
/// alphabet questions it answers.
/// </summary>
public sealed class RussianSnowballStemmerTests
{
    // nltk stems these to "подь" and "обь": its ь is one apostrophe and its ъ is
    // two, so step 4 removes half of a final ъ. Decision 0086 does not follow it.
    [Theory]
    [InlineData("подъём", "подъ")]
    [InlineData("объём", "объ")]
    public void A_stem_left_ending_in_a_hard_sign_keeps_it(string word, string expected)
    {
        Assert.Equal(expected, RussianSnowballStemmer.Stem(word));
    }

    /// <summary>The oblique forms agree with nltk: step 4 never fires on them.</summary>
    [Theory]
    [InlineData("подъёма", "подъем")]
    [InlineData("объёмы", "объем")]
    public void The_oblique_forms_of_the_same_words_agree(string word, string expected)
    {
        Assert.Equal(expected, RussianSnowballStemmer.Stem(word));
    }

    // The other half of decision 0086, which the corpus does carry and which is
    // asserted here too because the two halves are one reading of one table.
    [Fact]
    public void The_uyushchaya_pair_follows_nltks_table_over_the_description()
    {
        Assert.Equal("рискующ", RussianSnowballStemmer.Stem("рискующая"));
        Assert.Equal("риск", RussianSnowballStemmer.Stem("рискующий"));
    }

    [Theory]
    [InlineData("ёлка")]
    [InlineData("Ёлка")]
    [InlineData("елка")]
    // A decomposed ё, which NFC composes before the fold can reach it.
    [InlineData("ёлка")]
    public void Yo_folds_to_ye_whatever_case_or_form_it_arrives_in(string word)
    {
        Assert.Equal("елк", RussianSnowballStemmer.Stem(word));
    }

    [Theory]
    [InlineData("ГОРОДА", "город")]
    [InlineData("Москва", "москв")]
    public void Uppercase_Cyrillic_is_lowercased_like_every_sibling(string word, string expected)
    {
        Assert.Equal(expected, RussianSnowballStemmer.Stem(word));
    }

    /// <summary>
    /// A word with no Russian vowel has an empty RV and no letter any table can
    /// reach, so it needs no guard of its own to come back whole.
    /// </summary>
    [Theory]
    [InlineData("", "")]
    [InlineData("в", "в")]
    [InlineData("hello", "hello")]
    public void A_word_the_alphabet_cannot_reach_comes_back_whole(string word, string expected)
    {
        Assert.Equal(expected, RussianSnowballStemmer.Stem(word));
    }

    /// <summary>Step 4 still runs on a one-letter word, which is why there is no length guard.</summary>
    [Fact]
    public void A_lone_soft_sign_is_removed_by_step_4()
    {
        Assert.Equal("", RussianSnowballStemmer.Stem("ь"));
    }
}
