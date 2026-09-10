using BenchmarkDotNet.Attributes;
using Lodestar.Abstractions;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;
using Lucene.Analysis.Standard;
using Lucene.Document;
using Lucene.Index;
using Lucene.Search;
using Lucene.Search.Similarities;
using Lucene.Store;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, and the
// build succeeds either way -- so following the rule breaks the run, not the compile.
#pragma warning disable CA1822

// CA1001 (owns a disposable field but is not IDisposable): BenchmarkDotNet owns
// the instance and calls the cleanup, so the directory and the reader are released
// there. IDisposable would advertise an ownership no caller ever takes.
#pragma warning disable CA1001

/// <summary>BM25 against LuceneSharp.Core, priced at the level a caller actually pays.</summary>
/// <remarks>
/// <strong>These are not like-for-like, and that is the measurement.</strong> Lucene needs an
/// index; this scores a matrix a caller already built for other reasons. Pricing only the query
/// flatters this package, pricing only the build flatters Lucene, so both are rows and the ratio
/// to read depends on how many queries one corpus answers. bench/README.md section 21 has the
/// rest, including what Lucene answers that this does not.
/// </remarks>
[MemoryDiagnoser]
public class Bm25Benchmarks
{
    private string[] _documents = [];
    private Bm25Index _index = null!;
    private int[] _query = [];
    private ByteBuffersDirectory _directory = null!;
    private DirectoryReader _reader = null!;
    private IndexSearcher _searcher = null!;
    private Query _luceneQuery = null!;

    /// <summary>Documents in the corpus.</summary>
    [Params(1_000, 20_000)]
    public int Documents { get; set; }

    private const int TopK = 10;
    private const string Body = "body";
    private const string QueryTerm = "term001";

    [GlobalSetup]
    public void Setup()
    {
        Random random = new(573);
        string[] vocabulary = [.. Enumerable.Range(0, 500).Select(i => $"term{i:D3}")];
        _documents = [.. Enumerable.Range(0, Documents).Select(_ =>
            string.Join(' ', Enumerable.Range(0, 40).Select(__ => vocabulary[random.Next(vocabulary.Length)])))];

        var vectorizer = new CountVectorizer();
        _index = new Bm25Index(vectorizer.FitTransform(_documents));

        IReadOnlyList<string> names = vectorizer.GetFeatureNames();
        // One term on both sides: a single term is the same shape of work for either,
        // and what is being priced is the index rather than the query language.
        _query = [IndexOf(names, QueryTerm)];

        _directory = new ByteBuffersDirectory();
        BuildLuceneIndex(_directory, _documents);
        _reader = DirectoryReader.Open(_directory);
        _searcher = new IndexSearcher(_reader);
        _searcher.SetSimilarity(new BM25Similarity());
        _luceneQuery = new TermQuery(new Term(Body, QueryTerm));
    }

    /// <summary>The reader and the directory outlive an iteration, so they are released here.</summary>
    [GlobalCleanup]
    public void Cleanup()
    {
        _reader.Dispose();
        _directory.Dispose();
    }

    private static int IndexOf(IReadOnlyList<string> names, string term)
    {
        for (int i = 0; i < names.Count; i++)
        {
            if (string.Equals(names[i], term, StringComparison.Ordinal))
            {
                return i;
            }
        }

        return -1;
    }

    private static void BuildLuceneIndex(Lucene.Store.Directory directory, string[] documents)
    {
        using StandardAnalyzer analyzer = new();
        using IndexWriter writer = new(directory, new IndexWriterConfig(analyzer));
        foreach (string text in documents)
        {
            Document document = new();
            document.Add(new TextField(Body, text, Field.Store.No));
            writer.AddDocument(document);
        }

        writer.Commit();
    }

    /// <summary>The query alone, over a matrix the caller already had.</summary>
    [Benchmark(Baseline = true)]
    public IReadOnlyList<SearchHit> LodestarQuery() => _index.Top(_query, TopK);

    /// <summary>The query alone, over an index Lucene already built.</summary>
    [Benchmark]
    public TopDocs LuceneQuery() => _searcher.Search(_luceneQuery, TopK);

    /// <summary>Text in, ranking out: what this package pays from nothing.</summary>
    [Benchmark]
    public IReadOnlyList<SearchHit> LodestarFromText()
    {
        CsrMatrix counts = new CountVectorizer().FitTransform(_documents);
        return new Bm25Index(counts).Top(_query, TopK);
    }

    /// <summary>Text in, ranking out: what Lucene pays from nothing, index included.</summary>
    [Benchmark]
    public TopDocs LuceneFromText()
    {
        using ByteBuffersDirectory directory = new();
        BuildLuceneIndex(directory, _documents);
        using DirectoryReader reader = DirectoryReader.Open(directory);
        IndexSearcher searcher = new(reader);
        searcher.SetSimilarity(new BM25Similarity());
        return searcher.Search(_luceneQuery, TopK);
    }
}
