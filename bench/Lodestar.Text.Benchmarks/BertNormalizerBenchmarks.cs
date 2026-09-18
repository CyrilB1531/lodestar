using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;

namespace Lodestar.Text.Benchmarks;

/// <summary>Which flavour of text a row of the BERT normalizer table measures.</summary>
public enum NormalizerText
{
    /// <summary>The corpus documents as generated, lowercase ASCII, which the normalizer does not change.</summary>
    Ascii,

    /// <summary>The same documents with every <c>e</c> replaced by <c>é</c>, so an uncased model decomposes.</summary>
    Accented,

    /// <summary>The same documents with every <c>a</c> replaced by <c>中</c>, so the CJK padding runs.</summary>
    Cjk,
}

/// <summary>
/// The <c>vocab.txt</c> route, whose normalizer and pre-tokenizer are what BERT's BasicTokenizer adds
/// to a WordPiece encode: `BertNormalizer` then `BertPreTokenizer`, cased and uncased.
/// </summary>
/// <remarks>
/// The three flavours are the cases #992 changed and #1048 measured, and they select different branches
/// of the normalizer: ASCII text it leaves alone, accented text an uncased model decomposes and strips,
/// ideographs it pads. The encode behind them is the same in all six rows, so a row moving is the
/// normalizer moving.
/// </remarks>
[MemoryDiagnoser]
public class BertNormalizerBenchmarks
{
    private readonly Dictionary<NormalizerText, string[]> _texts = [];
    private WordPieceTokenizer _cased = null!;
    private WordPieceTokenizer _uncased = null!;

    /// <summary>The flavour of text this row encodes.</summary>
    [Params(NormalizerText.Ascii, NormalizerText.Accented, NormalizerText.Cjk)]
    public NormalizerText Text { get; set; }

    /// <summary>Whether the vocabulary is the uncased one, which is what runs the accent strip.</summary>
    [Params(false, true)]
    public bool Lowercase { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        string[] documents = JsonSerializer.Deserialize<string[]>(
            File.ReadAllBytes(BenchCorpus.Path("documents.json")))!;
        _texts[NormalizerText.Ascii] = documents;
        _texts[NormalizerText.Accented] = [.. documents.Select(text => text.Replace('e', 'é'))];
        _texts[NormalizerText.Cjk] = [.. documents.Select(text => text.Replace('a', '中'))];

        var bounds = new ArtifactLoadOptions
        {
            MaxTotalBytes = 32L * 1024 * 1024,
            MaxVocabularySize = 300_000,
            MaxArrayLength = 300_000,
        };
        _cased = new WordPieceTokenizer(
            VocabTxtLoader.Load(BenchCorpus.Path("vocab_30k.txt"), bounds, lowercase: false));
        _uncased = new WordPieceTokenizer(
            VocabTxtLoader.Load(BenchCorpus.Path("vocab_30k.txt"), bounds, lowercase: true));
    }

    [Benchmark]
    public int Encode()
    {
        WordPieceTokenizer tokenizer = Lowercase ? _uncased : _cased;
        int total = 0;
        foreach (string text in _texts[Text])
        {
            total += tokenizer.Encode(text).Ids.Count;
        }
        return total;
    }
}
