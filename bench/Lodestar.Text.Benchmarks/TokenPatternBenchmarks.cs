using System.Globalization;
using BenchmarkDotNet.Attributes;
using Lodestar.Text.Vectorization;

namespace Lodestar.Text.Benchmarks;

/// <summary>
/// A fit over text of short words through each kind of token pattern: the default one, scanned by hand, and the ones
/// that go through the Python translation, which ran five to eight times slower than 0.7.0 before #1645.
/// </summary>
/// <remarks>
/// 4,000,000 characters of words from a 5,000-word vocabulary, one document, so the time is the tokenizer's and the
/// vocabulary's, not a corpus's. The text holds no surrogate, as most text does; one that holds one takes the
/// pair-aware spelling, which this does not measure.
/// </remarks>
[MemoryDiagnoser]
public class TokenPatternBenchmarks
{
    private string _text = "";

    /// <summary>The token pattern, as a scikit-learn user would write it.</summary>
    [Params(@"\b\w\w+\b", "[a-z]+", @"\S+", "[^ ]+", @"[A-Za-z]\w+")]
    public string Pattern { get; set; } = "";

    [GlobalSetup]
    public void Setup() =>
        _text = string.Join(" ", Enumerable.Range(0, 500_000).Select(i => "word" + (i * 7 % 5_000).ToString(CultureInfo.InvariantCulture)));

    /// <summary>One document of 4 MB fitted with <see cref="Pattern"/>.</summary>
    [Benchmark]
    public CountVectorizer Fit() => new CountVectorizer(new CountVectorizerOptions { TokenPattern = Pattern }).Fit([_text]);
}
