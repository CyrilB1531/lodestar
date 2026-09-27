using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

namespace Lodestar.Text.Benchmarks;

/// <summary>
/// A <c>LodestarVectorStoreCollection</c> written and searched in alternation, one record replaced
/// before each search, and a hybrid search alone, at three collection sizes (#1214).
/// </summary>
/// <remarks>
/// <see cref="FilteredVectorSearchBenchmarks"/>' corpus. Each write replaces a held record with one of
/// 64 prepared replacements, so the size never moves. The write-then-search rows are where a store
/// rebuilding on the first read after a write pays for every record; <c>Hybrid</c> is the fusion alone.
/// </remarks>
// CA1001: BenchmarkDotNet owns this type's lifecycle, and its [GlobalCleanup] disposes the collection.
// Implementing IDisposable would advertise an ownership no caller ever takes.
#pragma warning disable CA1001
[MemoryDiagnoser]
[SimpleJob(
    RunStrategy.Throughput,
    launchCount: 1,
    warmupCount: 5,
    iterationCount: 20)]
public class VectorStoreChurnBenchmarks
{
    private const int Replacements = 64;
    private static readonly string[] Keyword = ["w7"];
    private LodestarVectorStoreCollection<int, TaggedVector> _collection = null!;
    private ReadOnlyMemory<float> _query;
    private TaggedVector[] _replacements = [];
    private int _next;

    /// <summary>How many records the collection holds.</summary>
    [Params(1_000, 10_000, 100_000)]
    public int Records { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        (TaggedVector[] records, _query) = TaggedVector.Corpus(Records);

        // S2245 / CA5394: a seeded Random makes the replacements reproducible; nothing here is security-sensitive.
#pragma warning disable S2245, CA5394
        var random = new Random(1214);
#pragma warning restore S2245, CA5394
        _replacements = new TaggedVector[Replacements];
        for (int i = 0; i < Replacements; i++)
        {
            _replacements[i] = new TaggedVector
            {
                Id = i * (Records / Replacements),
                Embedding = TaggedVector.Uniform(random),
                Text = TaggedVector.Words(random, "common"),
            };
        }

        _collection = new LodestarVectorStoreCollection<int, TaggedVector>("churn");
        _collection.UpsertAsync(records).GetAwaiter().GetResult();
        _ = Hybrid().GetAwaiter().GetResult();
    }

    [GlobalCleanup]
    public void Cleanup() => _collection.Dispose();

    [Benchmark]
    public async Task<int> UpsertThenSearch()
    {
        await Replace().ConfigureAwait(false);
        return await Count(_collection.SearchAsync(_query, 10)).ConfigureAwait(false);
    }

    [Benchmark]
    public async Task<int> UpsertThenHybrid()
    {
        await Replace().ConfigureAwait(false);
        return await Hybrid().ConfigureAwait(false);
    }

    [Benchmark]
    public Task<int> Hybrid() => Count(_collection.HybridSearchAsync(_query, Keyword, 10));

    private Task Replace() => _collection.UpsertAsync(_replacements[_next++ % Replacements]);

    private static Task<int> Count(IAsyncEnumerable<VectorSearchResult<TaggedVector>> hits) =>
        FilteredVectorSearchBenchmarks.Count(hits);
}
