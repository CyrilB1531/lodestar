using BenchmarkDotNet.Attributes;
using Lodestar.Text.Similarity;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245 / CA5394: a seeded Random builds a reproducible corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks.
#pragma warning disable CA1822

/// <summary>The sketches against the exact measure they exist to replace.</summary>
/// <remarks>
/// <strong>These do not compute the same thing, and that is the measurement.</strong>
/// <c>ExactPairwise</c> answers every pair correctly and costs a quadratic number of set
/// comparisons; <c>SketchThenVerify</c> answers most pairs and costs one signature per document
/// plus a lookup. Comparing only the query would hide the signatures; comparing only the
/// signatures would hide what they buy on the millionth pair.
/// </remarks>
[MemoryDiagnoser]
public class SimilaritySketchBenchmarks
{
    private const int TokensPerDocument = 24;
    private const int Vocabulary = 2_000;

    private string[][] _documents = [];
    private MinHashPermutations _permutations = null!;
    private LshBanding _banding;

    /// <summary>Documents in the corpus. The pair count is this squared over two.</summary>
    [Params(500, 2_000)]
    public int Documents { get; set; }

    /// <summary>Signature length, which is also the estimate's resolution.</summary>
    [Params(64, 128)]
    public int Permutations { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(602);
        _documents = new string[Documents][];
        for (int document = 0; document < Documents; document++)
        {
            var tokens = new string[TokensPerDocument];
            for (int token = 0; token < TokensPerDocument; token++)
            {
                tokens[token] = $"term{random.Next(Vocabulary):D4}";
            }

            _documents[document] = tokens;
        }

        ulong[] multipliers = new ulong[Permutations];
        ulong[] addends = new ulong[Permutations];
        for (int i = 0; i < Permutations; i++)
        {
            multipliers[i] = (ulong)random.NextInt64(1, long.MaxValue);
            addends[i] = (ulong)random.NextInt64(0, long.MaxValue);
        }

        _permutations = new MinHashPermutations(multipliers, addends);
        _banding = LshBanding.Solve(0.5, Permutations);
    }

    /// <summary>Every pair compared exactly. The baseline, and what does not scale.</summary>
    [Benchmark(Baseline = true)]
    public int ExactPairwise()
    {
        int found = 0;
        for (int left = 0; left < _documents.Length; left++)
        {
            var first = new HashSet<string>(_documents[left], StringComparer.Ordinal);
            for (int right = left + 1; right < _documents.Length; right++)
            {
                int shared = _documents[right].Count(first.Contains);
                int union = first.Count + _documents[right].Length - shared;
                if (union > 0 && (double)shared / union >= 0.5)
                {
                    found++;
                }
            }
        }

        return found;
    }

    /// <summary>One signature per document, then a banded lookup and a verification.</summary>
    /// <remarks>Signature construction is inside the measurement: it is what the sketch costs.</remarks>
    [Benchmark]
    public int SketchThenVerify()
    {
        var hasher = new MinHash(_permutations);
        var index = new LshIndex(_banding);
        uint[][] signatures = new uint[_documents.Length][];
        for (int document = 0; document < _documents.Length; document++)
        {
            signatures[document] = hasher.Signature(_documents[document]);
            index.Add(document.ToString(System.Globalization.CultureInfo.InvariantCulture),
                signatures[document]);
        }

        int found = 0;
        for (int document = 0; document < signatures.Length; document++)
        {
            foreach (string candidate in index.Query(signatures[document]))
            {
                int other = int.Parse(candidate, System.Globalization.CultureInfo.InvariantCulture);
                if (other > document && MinHash.Jaccard(signatures[document], signatures[other]) >= 0.5)
                {
                    found++;
                }
            }
        }

        return found;
    }

    /// <summary>The signatures alone, which is what a caller pays once and reuses.</summary>
    [Benchmark]
    public int SignaturesOnly()
    {
        var hasher = new MinHash(_permutations);
        int total = 0;
        foreach (string[] document in _documents)
        {
            total += hasher.Signature(document).Length;
        }

        return total;
    }

    /// <summary>One fingerprint per document, the cheapest sketch of the three.</summary>
    [Benchmark]
    public int FingerprintsOnly()
    {
        int total = 0;
        foreach (string[] document in _documents)
        {
            total += SimHash.Fingerprint(document) == 0UL ? 0 : 1;
        }

        return total;
    }
}
