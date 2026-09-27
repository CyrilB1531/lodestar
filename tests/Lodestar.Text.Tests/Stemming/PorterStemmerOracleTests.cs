using System.Text.Json.Serialization;
using Lodestar.Text.Stemming;
using Lodestar.Text.Tests.Oracles;
using Xunit;

namespace Lodestar.Text.Tests.Stemming;

public sealed record PorterCase
{
    [JsonPropertyName("id")] public int Id { get; init; }
    [JsonPropertyName("word")] public string Word { get; init; } = "";
    [JsonPropertyName("stem")] public string Stem { get; init; } = "";
}

public sealed class PorterStemmerOracleTests
{
    private static readonly OracleFile<PorterCase> Corpus = OracleCorpus.Load<PorterCase>("porter.json");

    private static readonly OracleFile<PorterCase> Martin = OracleCorpus.Load<PorterCase>("porter_martin.json");

    [Fact]
    public void Metadata_is_nltk()
    {
        Assert.Equal("nltk", Corpus.Metadata.Library);
        Assert.NotEmpty(Corpus.Cases);
    }

    [Fact]
    public void Stem_matches_nltk()
    {
        OracleAsserts.ExactString(Corpus.Cases,
            c => c.Stem,
            c => PorterStemmer.Stem(c.Word),
            c => $"[#{c.Id}] \"{c.Word}\"");
    }

    [Fact]
    public void Stem_under_martins_extensions_matches_nltk()
    {
        Assert.Equal(Corpus.Cases.Count, Martin.Cases.Count);
        OracleAsserts.ExactString(Martin.Cases,
            c => c.Stem,
            c => PorterStemmer.Stem(c.Word, PorterStemmerMode.MartinExtensions),
            c => $"[#{c.Id}] \"{c.Word}\"");
    }

    [Theory]
    [InlineData("analogy", "analogi", "analog")]
    [InlineData("accessibly", "accessibli", "access")]
    [InlineData("as", "a", "as")]
    [InlineData("IS", "i", "is")]
    [InlineData("", "", "")]
    public void The_two_modes_part_where_nltk_parts_them(string word, string original, string martin)
    {
        Assert.Equal(original, PorterStemmer.Stem(word));
        Assert.Equal(original, PorterStemmer.Stem(word, PorterStemmerMode.OriginalAlgorithm));
        Assert.Equal(martin, PorterStemmer.Stem(word, PorterStemmerMode.MartinExtensions));
    }

    [Fact]
    public void Stem_refuses_an_undefined_mode()
    {
        Assert.Throws<ArgumentException>(() => PorterStemmer.Stem("word", (PorterStemmerMode)2));
    }

    [Theory]
    [InlineData("caresses", "caress")]
    [InlineData("ponies", "poni")]
    [InlineData("relational", "relat")]
    [InlineData("running", "run")]
    [InlineData("happy", "happi")]
    public void Stem_known_values(string word, string expected)
    {
        Assert.Equal(expected, PorterStemmer.Stem(word));
    }

    [Fact]
    public void Stem_NullArgument_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PorterStemmer.Stem(null!));
    }
}
