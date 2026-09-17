using System.Text;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;

namespace Lodestar.Text.Benchmarks;

/// <summary>
/// GPT-2's 30k byte-level vocabulary with Llama-3's shape of added-token table, 256 entries opening
/// on the same two characters, over chat-template text dense with them and over prose holding none.
/// </summary>
/// <remarks>The table is what the scanner walks; the vocabulary behind it only has to encode the gaps.</remarks>
[MemoryDiagnoser]
public class AddedTokenScanBenchmarks
{
    private BpeTokenizer _bpe = null!;
    private string[] _chat = [];
    private string[] _prose = [];

    [GlobalSetup]
    public void Setup()
    {
        BpeVocabulary vocabulary = TokenizerJsonLoader.LoadBpe(
            BenchCorpus.Path("tokenizer_30k_bpe.json"),
            new ArtifactLoadOptions { MaxTotalBytes = 32L * 1024 * 1024, MaxVocabularySize = 300_000, MaxArrayLength = 300_000 });
        int next = vocabulary.Vocab.Values.Max() + 1;
        string[] named = ["<|begin_of_text|>", "<|end_of_text|>", "<|start_header_id|>", "<|end_header_id|>", "<|eot_id|>"];
        AddedToken[] added =
        [
            .. named.Select((content, i) => new AddedToken(content, next + i) { Special = true }),
            .. Enumerable.Range(0, 256 - named.Length)
                .Select(i => new AddedToken($"<|reserved_special_token_{i}|>", next + named.Length + i) { Special = true }),
        ];
        _bpe = new BpeTokenizer(vocabulary with { AddedTokens = added });

        _prose = JsonSerializer.Deserialize<string[]>(File.ReadAllBytes(BenchCorpus.Path("documents.json")))!;
        _chat = [.. _prose.Select(document =>
        {
            var turn = new StringBuilder("<|begin_of_text|>");
            foreach (string sentence in document.Split('.'))
            {
                turn.Append("<|start_header_id|>user<|end_header_id|>\n\n").Append(sentence).Append("<|eot_id|>");
            }
            return turn.ToString();
        })];
    }

    [Benchmark]
    public int ChatTemplate() => Encode(_chat);

    [Benchmark]
    public int ProseWithoutAddedTokens() => Encode(_prose);

    private int Encode(string[] texts)
    {
        int total = 0;
        foreach (string text in texts)
        {
            total += _bpe.Encode(text).Ids.Count;
        }
        return total;
    }
}
