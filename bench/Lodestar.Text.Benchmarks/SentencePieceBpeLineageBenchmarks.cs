using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;

namespace Lodestar.Text.Benchmarks;

/// <summary>Which SentencePiece-lineage BPE file a row of <see cref="SentencePieceBpeLineageBenchmarks"/> reads.</summary>
public enum BpeLineageModel
{
    /// <summary>Llama-2's <c>tokenizer.json</c>: the escape spelled as a normalizer.</summary>
    Llama2,

    /// <summary>Mistral v0.1's: the escape spelled as a pre-tokenizer.</summary>
    Mistral,
}

/// <summary>
/// The BPE lineage that declares no pre-tokenizer, over the corpus documents: each document is one
/// piece, too long for the piece cache, so every character is looked up in the vocabulary.
/// </summary>
[MemoryDiagnoser]
public class SentencePieceBpeLineageBenchmarks
{
    private string[] _documents = [];
    private BpeTokenizer _bpe = null!;

    /// <summary>The file this row encodes with.</summary>
    [Params(BpeLineageModel.Llama2, BpeLineageModel.Mistral)]
    public BpeLineageModel Model { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _documents = JsonSerializer.Deserialize<string[]>(File.ReadAllBytes(BenchCorpus.Path("documents.json")))!;
        string file = Model == BpeLineageModel.Llama2 ? "llama2_tokenizer.json" : "mistral_v01_tokenizer.json";
        _bpe = new BpeTokenizer(TokenizerJsonLoader.LoadBpe(
            Path.Combine(BenchCorpus.RepoRoot(), "tests", "oracles", file),
            new ArtifactLoadOptions { MaxTotalBytes = 32L * 1024 * 1024, MaxVocabularySize = 300_000, MaxArrayLength = 300_000 }));
    }

    [Benchmark]
    public int EncodeDocuments()
    {
        int total = 0;
        foreach (string document in _documents)
        {
            total += _bpe.Encode(document).Ids.Count;
        }
        return total;
    }
}
