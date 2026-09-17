using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;

namespace Lodestar.Text.Benchmarks;

/// <summary>
/// SentencePiece's compiled normalizer alone, XLM-R's <c>nmt_nfkc_cf</c> map over the corpus documents,
/// and the unigram encode it runs inside.
/// </summary>
[MemoryDiagnoser]
public class PrecompiledNormalizerBenchmarks
{
    private string[] _documents = [];
    private PrecompiledNormalizer _normalizer = null!;
    private SentencePieceTokenizer _sentencePiece = null!;

    [GlobalSetup]
    public void Setup()
    {
        _documents = JsonSerializer.Deserialize<string[]>(File.ReadAllBytes(BenchCorpus.Path("documents.json")))!;
        _normalizer = SentencePieceModelLoader
            .Load(Path.Combine(BenchCorpus.RepoRoot(), "tests", "oracles", "nmt_nfkc_cf.model"))
            .Normalizer!;
        var bounds = new ArtifactLoadOptions { MaxTotalBytes = 32L * 1024 * 1024, MaxVocabularySize = 300_000, MaxArrayLength = 300_000 };
        _sentencePiece = new SentencePieceTokenizer(
            SentencePieceModelLoader.Load(BenchCorpus.Path("spiece_30k.model"), bounds) with { Normalizer = _normalizer });
    }

    [Benchmark]
    public int NormalizeDocuments()
    {
        int total = 0;
        foreach (string document in _documents)
        {
            total += _normalizer.Normalize(document).Length;
        }
        return total;
    }

    [Benchmark]
    public int EncodeDocuments()
    {
        int total = 0;
        foreach (string document in _documents)
        {
            total += _sentencePiece.Encode(document).Ids.Count;
        }
        return total;
    }
}
