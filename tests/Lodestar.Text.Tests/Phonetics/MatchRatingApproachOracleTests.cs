using Lodestar.Text.Phonetics;
using Lodestar.Text.Tests.Oracles;
using Xunit;

namespace Lodestar.Text.Tests.Phonetics;

/// <summary>Replays <c>match_rating_codex.json</c> and <c>match_rating_comparison.json</c>
/// against jellyfish's own values.</summary>
public sealed class MatchRatingApproachOracleTests
{
    private static readonly OracleFile<MatchRatingCodexCase> CodexCorpus =
        OracleCorpus.Load<MatchRatingCodexCase>("match_rating_codex.json");

    private static readonly OracleFile<MatchRatingComparisonCase> ComparisonCorpus =
        OracleCorpus.Load<MatchRatingComparisonCase>("match_rating_comparison.json");

    [Fact]
    public void Codex_matches_jellyfish()
    {
        OracleAsserts.ExactString(CodexCorpus.Cases,
            c => c.Codex,
            c => MatchRatingApproach.Codex(c.Word),
            c => $"[#{c.Id}] {OracleAsserts.Escape(c.Word)}");
    }

    [Fact]
    public void Compare_matches_jellyfish()
    {
        OracleAsserts.ExactNullableBool(ComparisonCorpus.Cases,
            c => c.Comparison,
            c => MatchRatingApproach.Compare(c.A, c.B),
            c => $"[#{c.Id}] {OracleAsserts.Escape(c.A)} / {OracleAsserts.Escape(c.B)}");
    }

    [Fact]
    public void The_corpora_are_the_ones_that_were_committed()
    {
        // Pins the case count and the library version so an empty `cases` array -- which
        // runs zero theories and reports green -- cannot pass unnoticed.
        Assert.Equal(420, CodexCorpus.Cases.Count);
        Assert.Equal(212, ComparisonCorpus.Cases.Count);
        Assert.Equal("jellyfish", CodexCorpus.Metadata.Library);
        Assert.Equal("jellyfish", ComparisonCorpus.Metadata.Library);
        Assert.Equal("1.2.1", CodexCorpus.Metadata.LibraryVersion);
        Assert.Equal("1.2.1", ComparisonCorpus.Metadata.LibraryVersion);
    }

    [Theory]
    [InlineData("Smith", "SMTH")]
    [InlineData("Byrne", "BYRN")]
    [InlineData("Mississippi", "MSSP")]
    [InlineData("Bhattacharya", "BHTHRY")]
    [InlineData("aeiou", "A")]
    [InlineData("", "")]
    public void Codex_known_values(string word, string expected)
    {
        Assert.Equal(expected, MatchRatingApproach.Codex(word));
    }

    [Theory]
    [InlineData("Byrne", "Boern", true)]
    [InlineData("Tim", "Timothy", null)]
    [InlineData("Smith", "", null)]
    [InlineData("", "", true)]
    public void Compare_known_values(string a, string b, bool? expected)
    {
        Assert.Equal(expected, MatchRatingApproach.Compare(a, b));
    }

    // jellyfish measures a codex's length in UTF-8 bytes (decision 0009, amending 0007), so the
    // six-character truncation fires on a four-character CJK word and repeats its middle.
    [Fact]
    public void Codex_truncates_on_utf8_bytes_as_jellyfish_does()
    {
        // jellyfish.match_rating_codex("並丝七世") == "並丝七丝七世": 12 bytes, past 6.
        Assert.Equal("並丝七丝七世", MatchRatingApproach.Codex("並丝七世"));
    }

    [Fact]
    public void Compare_measures_the_length_gap_in_utf8_bytes()
    {
        // jellyfish.match_rating_comparison("日本", "AB") is None: 6 bytes against 2.
        Assert.Null(MatchRatingApproach.Compare("日本", "AB"));
    }

    [Fact]
    public void Codex_uppercases_by_the_full_case_mapping()
    {
        // jellyfish.match_rating_codex("ßa") == "S": "ß" uppercases to "SS", which collapses.
        Assert.Equal("S", MatchRatingApproach.Codex("ßa"));
        Assert.True(MatchRatingApproach.Compare("ßa", "Sa"));
    }

    [Theory]
    [InlineData("O'Brien")]
    [InlineData("Anne-Marie")]
    [InlineData("a1b")]
    [InlineData("a.b")]
    [InlineData("a_b")]
    [InlineData("a\tb")]
    [InlineData("-")]
    public void Codex_refuses_a_non_letter_non_space_character(string value)
    {
        Assert.Throws<ArgumentException>(() => MatchRatingApproach.Codex(value));
    }

    [Theory]
    [InlineData("O'Brien")]
    [InlineData("Anne-Marie")]
    [InlineData("a1b")]
    [InlineData("a.b")]
    [InlineData("a_b")]
    [InlineData("a\tb")]
    [InlineData("-")]
    public void Compare_answers_null_for_a_non_letter_non_space_character_in_either_operand(string value)
    {
        // jellyfish.match_rating_comparison("O'Brien", "Smith") is None, where the codex raises (#1198).
        Assert.Null(MatchRatingApproach.Compare(value, "Smith"));
        Assert.Null(MatchRatingApproach.Compare("Smith", value));
    }

    [Fact]
    public void Codex_NullArgument_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => MatchRatingApproach.Codex(null!));
    }

    [Fact]
    public void Compare_NullArgument_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => MatchRatingApproach.Compare(null!, "Smith"));
        Assert.Throws<ArgumentNullException>(() => MatchRatingApproach.Compare("Smith", null!));
    }
}
