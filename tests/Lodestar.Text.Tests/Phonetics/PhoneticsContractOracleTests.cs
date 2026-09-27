using System.Text.Json.Serialization;
using Lodestar.Text.Phonetics;
using Lodestar.Text.Tests.Oracles;
using Xunit;

namespace Lodestar.Text.Tests.Phonetics;

/// <summary>A word beyond ASCII letters and what each jellyfish encoder answers for it.</summary>
public sealed record PhoneticContractCase
{
    [JsonPropertyName("id")] public int Id { get; init; }
    [JsonPropertyName("word")] public string Word { get; init; } = "";
    [JsonPropertyName("soundex")] public string Soundex { get; init; } = "";
    [JsonPropertyName("metaphone")] public string Metaphone { get; init; } = "";
    [JsonPropertyName("nysiis")] public string Nysiis { get; init; } = "";

    /// <summary>Null where jellyfish's codex raises <c>ValueError</c>.</summary>
    [JsonPropertyName("codex")] public string? Codex { get; init; }
}

/// <summary>
/// Replays <c>phonetics_contract.json</c> and its pairs: apostrophes, spaces, the full uppercase
/// mapping, decomposition and grapheme clusters, answered as jellyfish 1.2.1 answers them
/// (decision 0009).
/// </summary>
public sealed class PhoneticsContractOracleTests
{
    private static readonly OracleFile<PhoneticContractCase> Corpus =
        OracleCorpus.Load<PhoneticContractCase>("phonetics_contract.json");

    private static readonly OracleFile<MatchRatingComparisonCase> Pairs =
        OracleCorpus.Load<MatchRatingComparisonCase>("phonetics_contract_pairs.json");

    [Fact]
    public void The_corpora_are_the_ones_that_were_committed()
    {
        Assert.Equal(330, Corpus.Cases.Count);
        Assert.Equal(330, Pairs.Cases.Count);
        Assert.Equal("1.2.1", Corpus.Metadata.LibraryVersion);
        Assert.Equal("1.2.1", Pairs.Metadata.LibraryVersion);
    }

    [Fact]
    public void Soundex_matches_jellyfish()
    {
        OracleAsserts.ExactString(Corpus.Cases, c => c.Soundex, c => Soundex.Encode(c.Word), Label);
    }

    [Fact]
    public void Metaphone_matches_jellyfish()
    {
        OracleAsserts.ExactString(Corpus.Cases, c => c.Metaphone, c => Metaphone.Encode(c.Word), Label);
    }

    [Fact]
    public void Nysiis_matches_jellyfish()
    {
        OracleAsserts.ExactString(Corpus.Cases, c => c.Nysiis, c => Nysiis.Encode(c.Word), Label);
    }

    [Fact]
    public void Codex_matches_jellyfish_and_refuses_what_it_refuses()
    {
        OracleAsserts.ExactString(Corpus.Cases,
            c => c.Codex ?? "refused",
            c =>
            {
                try
                {
                    return MatchRatingApproach.Codex(c.Word);
                }
                catch (ArgumentException)
                {
                    return "refused";
                }
            },
            Label);
    }

    [Fact]
    public void Compare_matches_jellyfish()
    {
        OracleAsserts.ExactNullableBool(Pairs.Cases,
            c => c.Comparison,
            c => MatchRatingApproach.Compare(c.A, c.B),
            c => $"[#{c.Id}] {OracleAsserts.Escape(c.A)} / {OracleAsserts.Escape(c.B)}");
    }

    private static string Label(PhoneticContractCase c) => $"[#{c.Id}] {OracleAsserts.Escape(c.Word)}";
}
