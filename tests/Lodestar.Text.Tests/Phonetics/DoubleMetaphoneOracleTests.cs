using Lodestar.Text.Phonetics;
using Lodestar.Text.Tests.Oracles;
using Xunit;

namespace Lodestar.Text.Tests.Phonetics;

/// <summary>Replays <c>double_metaphone.json</c> against doublemetaphone 1.2's own values.</summary>
/// <remarks>
/// Every expectation here comes from the frozen corpus. The encoder's behaviour on the input
/// contract — accents, non-Latin scripts, digits, punctuation, the empty word — is pinned by the
/// corpus's own fixed points rather than by assertions written on this side, so what the reference
/// does stays the reference's to say (decision 0075).
/// </remarks>
public sealed class DoubleMetaphoneOracleTests
{
    private static readonly OracleFile<DoubleMetaphoneCase> Corpus =
        OracleCorpus.Load<DoubleMetaphoneCase>("double_metaphone.json");

    [Fact]
    public void Primary_matches_doublemetaphone()
    {
        OracleAsserts.ExactString(Corpus.Cases,
            c => c.Primary,
            c => DoubleMetaphone.Encode(c.Word).Primary,
            c => $"[#{c.Id}] {OracleAsserts.Escape(c.Word)}");
    }

    [Fact]
    public void Secondary_matches_doublemetaphone()
    {
        OracleAsserts.ExactString(Corpus.Cases,
            c => c.Secondary,
            c => DoubleMetaphone.Encode(c.Word).Secondary,
            c => $"[#{c.Id}] {OracleAsserts.Escape(c.Word)}");
    }

    [Fact]
    public void The_corpus_is_the_one_that_was_committed()
    {
        // Pins the case count and the library version so an empty `cases` array -- which asserts
        // nothing and reports green -- cannot pass unnoticed.
        Assert.Equal(423, Corpus.Cases.Count);
        Assert.Equal("doublemetaphone", Corpus.Metadata.Library);
        Assert.Equal("1.2", Corpus.Metadata.LibraryVersion);
    }

    [Fact]
    public void The_corpus_exercises_both_codes()
    {
        // A corpus of words that all encode the same way twice would let a one-code
        // implementation pass both facts above.
        Assert.Equal(102, Corpus.Cases.Count(c => c.Secondary.Length > 0));
    }

    [Fact]
    public void Encode_refuses_a_null_string()
    {
        Assert.Throws<ArgumentNullException>(() => DoubleMetaphone.Encode(null!));
    }

    [Fact]
    public void The_span_overload_agrees_with_the_string_one()
    {
        foreach (DoubleMetaphoneCase c in Corpus.Cases)
        {
            Assert.Equal(DoubleMetaphone.Encode(c.Word), DoubleMetaphone.Encode(c.Word.AsSpan()));
        }
    }
}
