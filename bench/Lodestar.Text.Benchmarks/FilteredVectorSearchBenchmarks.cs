using System.Globalization;
using System.Text;
using BenchmarkDotNet.Attributes;
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

namespace Lodestar.Text.Benchmarks;

/// <summary>A record for <see cref="FilteredVectorSearchBenchmarks"/>: a key, a filterable tag and a 384-wide vector.</summary>
public sealed class TaggedVector
{
    /// <summary>The key.</summary>
    [VectorStoreKey]
    public int Id { get; set; }

    /// <summary>What the filter reads: half the records are even.</summary>
    [VectorStoreData]
    public int Tag { get; set; }

    /// <summary>What the keyword half indexes: eight words from 2,000, plus <c>common</c> in every record.</summary>
    [VectorStoreData(IsFullTextIndexed = true)]
    public string Text { get; set; } = string.Empty;

    /// <summary>The embedding, MiniLM's width.</summary>
    [VectorStoreVector(384)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>
/// A vector search through <c>LodestarVectorStoreCollection</c> with a filter admitting half the
/// records, top 10: the filter reads every record and only those it admits are scored.
/// </summary>
/// <remarks>
/// The collection is built and its indexes warmed once, so the rows time the search alone. The
/// unfiltered row is the reference the filtered one is read against. The two hybrid rows fuse the
/// same vector ranking with a keyword one record holds and one every record holds, the two shapes
/// #993 and #1036 moved in opposite directions.
/// </remarks>
// CA1001 (owns a disposable field but is not IDisposable): BenchmarkDotNet owns
// this type's lifecycle and calls [GlobalCleanup] below, which disposes
// _collection. IDisposable would advertise an ownership no caller ever takes.
#pragma warning disable CA1001
[MemoryDiagnoser]
public class FilteredVectorSearchBenchmarks
{
    private const int Dimension = 384;
    private LodestarVectorStoreCollection<int, TaggedVector> _collection = null!;
    private ReadOnlyMemory<float> _query;
    private VectorSearchOptions<TaggedVector> _filtered = null!;

    /// <summary>How many records the collection holds.</summary>
    [Params(10_000, 100_000)]
    public int Records { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        // S2245 / CA5394: a seeded Random makes the collection reproducible; nothing here is security-sensitive.
#pragma warning disable S2245, CA5394
        var random = new Random(682);
        var records = new TaggedVector[Records];
        for (int i = 0; i < Records; i++)
        {
            var vector = new float[Dimension];
            for (int j = 0; j < Dimension; j++)
            {
                vector[j] = (float)((random.NextDouble() * 2) - 1);
            }
            records[i] = new TaggedVector { Id = i, Tag = random.Next(), Embedding = vector };
        }

        var query = new float[Dimension];
        for (int j = 0; j < Dimension; j++)
        {
            query[j] = (float)((random.NextDouble() * 2) - 1);
        }

        // A second generator, so the vectors above are drawn exactly as before the text existed.
        var words = new Random(1036);
        var text = new StringBuilder();
        for (int i = 0; i < Records; i++)
        {
            text.Clear().Append(i == Records / 2 ? "common needle" : "common");
            for (int w = 0; w < 8; w++)
            {
                text.Append(" w").Append(words.Next(2_000).ToString(CultureInfo.InvariantCulture));
            }
            records[i].Text = text.ToString();
        }
#pragma warning restore S2245, CA5394
        _query = query;

        _collection = new LodestarVectorStoreCollection<int, TaggedVector>("bench");
        _collection.UpsertAsync(records).GetAwaiter().GetResult();
        _filtered = new VectorSearchOptions<TaggedVector> { Filter = record => record.Tag % 2 == 0 };
        _ = Unfiltered().GetAwaiter().GetResult();
    }

    [GlobalCleanup]
    public void Cleanup() => _collection.Dispose();

    [Benchmark]
    public Task<int> FilteredTop10() => Count(_collection.SearchAsync(_query, 10, _filtered));

    [Benchmark]
    public Task<int> Unfiltered() => Count(_collection.SearchAsync(_query, 10));

    /// <summary>A keyword one record holds: the ranking #993 made cheap.</summary>
    [Benchmark]
    public Task<int> HybridSelective() => Count(_collection.HybridSearchAsync(_query, ["needle"], 10));

    /// <summary>A keyword every record holds, as <c>the</c> is over prose with no stop words: the ranking #993 made dearer (#1036).</summary>
    [Benchmark]
    public Task<int> HybridBroad() => Count(_collection.HybridSearchAsync(_query, ["common"], 10));

    private static async Task<int> Count(IAsyncEnumerable<VectorSearchResult<TaggedVector>> hits)
    {
        int count = 0;
        await foreach (VectorSearchResult<TaggedVector> _ in hits.ConfigureAwait(false))
        {
            count++;
        }
        return count;
    }
}
