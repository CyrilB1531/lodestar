using System.Text;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;

namespace Lodestar.Text.Benchmarks;

// CA1822: see LevenshteinIncumbentBenchmarks.
#pragma warning disable CA1822

/// <summary>Which sub-word model a row of the incumbent table measures.</summary>
public enum TokenizerModel
{
    /// <summary>WordPiece, both sides reading <c>vocab_30k.txt</c>'s vocabulary.</summary>
    WordPiece,

    /// <summary>SentencePiece unigram, both sides reading <c>spiece_30k.model</c>.</summary>
    SentencePiece,

    /// <summary>GPT-2 byte-level BPE, both sides reading <c>tokenizer_30k_bpe.json</c>'s vocabulary and merges.</summary>
    ByteLevelBpe,
}

/// <summary>
/// Our three sub-word tokenizers against `Microsoft.ML.Tokenizers`, the first-party
/// incumbent issue #438 names for this package.
/// </summary>
/// <remarks>
/// Both sides encode the same documents from the same artefact and were checked to
/// return identical ids first; bench/README.md section 15 has that check, and why the
/// model is a parameter rather than four methods.
/// </remarks>
[MemoryDiagnoser]
public class TokenizerIncumbentBenchmarks
{
    private string[] _documents = [];
    private WordPieceTokenizer _wordPiece = null!;
    private SentencePieceTokenizer _sentencePiece = null!;
    private Microsoft.ML.Tokenizers.WordPieceTokenizer _theirWordPiece = null!;
    private Microsoft.ML.Tokenizers.SentencePieceTokenizer _theirSentencePiece = null!;
    private BpeTokenizer _bpe = null!;
    private Microsoft.ML.Tokenizers.CodeGenTokenizer _theirBpe = null!;

    /// <summary>The model this row measures on both libraries.</summary>
    [Params(TokenizerModel.WordPiece, TokenizerModel.SentencePiece, TokenizerModel.ByteLevelBpe)]
    public TokenizerModel Model { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var bounds = new ArtifactLoadOptions
        {
            MaxTotalBytes = 32L * 1024 * 1024,
            MaxVocabularySize = 300_000,
            MaxArrayLength = 300_000,
        };
        _documents = JsonSerializer.Deserialize<string[]>(
            File.ReadAllBytes(BenchCorpus.Path("documents.json")))!;

        _wordPiece = new WordPieceTokenizer(
            TokenizerJsonLoader.LoadWordPiece(BenchCorpus.Path("tokenizer_30k_wordpiece.json"), bounds));
        _sentencePiece = new SentencePieceTokenizer(
            SentencePieceModelLoader.Load(BenchCorpus.Path("spiece_30k.model"), bounds));

        using (var vocabulary = File.OpenRead(BenchCorpus.Path("vocab_30k.txt")))
        {
            _theirWordPiece = Microsoft.ML.Tokenizers.WordPieceTokenizer.Create(vocabulary);
        }

        using (var model = File.OpenRead(BenchCorpus.Path("spiece_30k.model")))
        {
            // Positionally: addBeginOfSentence, addEndOfSentence. Left on, every document
            // would carry a leading <s> ours does not emit, and the ids would not compare.
            _theirSentencePiece = Microsoft.ML.Tokenizers.SentencePieceTokenizer.Create(model, false, false);
        }

        _bpe = new BpeTokenizer(TokenizerJsonLoader.LoadBpe(BenchCorpus.Path("tokenizer_30k_bpe.json"), bounds));
        _theirBpe = LoadTheirBpe(BenchCorpus.Path("tokenizer_30k_bpe.json"));
    }

    /// <summary>Hands <c>CodeGenTokenizer</c>, the incumbent's GPT-2 byte-level BPE, the file's vocabulary and merges as <c>vocab.json</c> and <c>merges.txt</c>.</summary>
    /// <remarks>
    /// Its <c>Create</c> refuses a vocabulary without <c>&lt;|endoftext|&gt;</c>, its default unknown
    /// token. The file has none, so one is appended past the last id: a byte-level model never
    /// produces the unknown token, and no id this class encodes moves.
    /// </remarks>
    private static Microsoft.ML.Tokenizers.CodeGenTokenizer LoadTheirBpe(string path)
    {
        using JsonDocument file = JsonDocument.Parse(File.ReadAllBytes(path));
        JsonElement model = file.RootElement.GetProperty("model");
        var vocabulary = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (JsonProperty entry in model.GetProperty("vocab").EnumerateObject())
        {
            vocabulary[entry.Name] = entry.Value.GetInt32();
        }
        vocabulary["<|endoftext|>"] = vocabulary.Count;

        var merges = new StringBuilder("#version: 0.2\n");
        foreach (JsonElement pair in model.GetProperty("merges").EnumerateArray())
        {
            merges.Append(pair[0].GetString()).Append(' ').Append(pair[1].GetString()).Append('\n');
        }

        using var vocabularyStream = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(vocabulary));
        using var mergesStream = new MemoryStream(Encoding.UTF8.GetBytes(merges.ToString()));
        return Microsoft.ML.Tokenizers.CodeGenTokenizer.Create(vocabularyStream, mergesStream);
    }

    [Benchmark(Baseline = true)]
    public int Lodestar()
    {
        int total = 0;
        foreach (string document in _documents)
        {
            total += Model switch
            {
                TokenizerModel.WordPiece => _wordPiece.Encode(document).Ids.Count,
                TokenizerModel.SentencePiece => _sentencePiece.Encode(document).Ids.Count,
                _ => _bpe.Encode(document).Ids.Count,
            };
        }
        return total;
    }

    [Benchmark]
    public int MlTokenizers()
    {
        int total = 0;
        foreach (string document in _documents)
        {
            total += Model switch
            {
                TokenizerModel.WordPiece => _theirWordPiece.EncodeToIds(document).Count,
                TokenizerModel.SentencePiece => _theirSentencePiece.EncodeToIds(document).Count,
                _ => _theirBpe.EncodeToIds(document).Count,
            };
        }
        return total;
    }
}
