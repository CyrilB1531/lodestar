using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;

namespace Lodestar.Text.Benchmarks;

/// <summary>
/// BPE's piece cache in a long-lived tokenizer: GPT-2's vocabulary over the paragraphs of the
/// immutable decisions 0001 to 0100, warmed on the first half, timed on the second.
/// </summary>
/// <remarks>
/// Rebuilt and warmed before every iteration: the first half fills fewer than the 10,000
/// entries, so warmed once, the timed half would be read from a cache it filled itself.
/// </remarks>
[MemoryDiagnoser]
public class BpeWordCacheBenchmarks
{
    private BpeVocabulary _vocabulary = null!;
    private BpeTokenizer _bpe = null!;
    private string[] _warm = [];
    private string[] _timed = [];

    [GlobalSetup]
    public void Setup()
    {
        string root = BenchCorpus.RepoRoot();
        string oracles = Path.Combine(root, "tests", "oracles");
        _vocabulary = BpeFilesLoader.Load(
            Path.Combine(oracles, "gpt2_vocab.json"), Path.Combine(oracles, "gpt2_merges.txt"));

        string[] paragraphs = Directory.GetFiles(Path.Combine(root, "docs", "decisions"), "0*.md")
            .Where(path => int.Parse(Path.GetFileName(path).AsSpan(0, 4), provider: null) <= 100)
            .Order(StringComparer.Ordinal)
            .SelectMany(path => Regex.Split(File.ReadAllText(path).Replace("\r\n", "\n", StringComparison.Ordinal), @"\n\s*\n", RegexOptions.None, TimeSpan.FromSeconds(1)))
            .Select(paragraph => paragraph.Trim())
            .Where(paragraph => paragraph.Length > 0)
            .ToArray();
        _warm = paragraphs[..(paragraphs.Length / 2)];
        _timed = paragraphs[(paragraphs.Length / 2)..];
    }

    [IterationSetup]
    public void Warm()
    {
        _bpe = new BpeTokenizer(_vocabulary);
        foreach (string paragraph in _warm)
        {
            _bpe.Encode(paragraph);
        }
    }

    [Benchmark]
    public int EncodeUnseenProse()
    {
        int total = 0;
        foreach (string paragraph in _timed)
        {
            total += _bpe.Encode(paragraph).Ids.Count;
        }
        return total;
    }
}
