using BenchmarkDotNet.Attributes;
using Lodestar.Embeddings.Search;
using Lodestar.Gpu.Compute;

namespace Lodestar.Gpu.Benchmarks;

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, and the
// build succeeds either way -- so following the rule breaks the run, not the compile.
#pragma warning disable CA1822

// CA1001 (owns disposable fields but is not IDisposable): BenchmarkDotNet owns the
// instance and calls the cleanup, which is where the accelerator is released.
#pragma warning disable CA1001

/// <summary>The cosine kernel as <c>k</c> grows, which is where its selection costs.</summary>
/// <remarks>
/// <see cref="TiledCosineTopKBenchmarks"/> holds k at 10, where scoring dominates. This holds the
/// batch at 16 queries and moves k to 1,000, the range the selection's own cost is written in
/// (#1214). Read <c>Device</c> before any figure, as there.
/// </remarks>
[MemoryDiagnoser]
public class CosineTopKSelectionBenchmarks
{
    private const int Dimension = 384;
    private const int Queries = 16;

    private GpuContext _context = null!;
    private DeviceEmbeddingMatrix _resident = null!;
    private TiledCosineTopK _kernel = null!;
    private EmbeddingIndex _index = null!;
    private float[] _queries = [];

    /// <summary>Rows in the corpus swept by every query.</summary>
    [Params(10_000, 100_000)]
    public int Documents { get; set; }

    /// <summary>Hits per query.</summary>
    [Params(10, 100, 1_000)]
    public int K { get; set; }

    /// <summary>The device each figure came from, which BenchmarkDotNet prints as a column.</summary>
    [ParamsSource(nameof(Devices))]
    public string Device { get; set; } = string.Empty;

    /// <summary>The one device this run uses, named so the report carries it.</summary>
    public static IEnumerable<string> Devices() => [SeededVectors.Device()];

    [GlobalSetup]
    public void Setup()
    {
        Random random = SeededVectors.Seeded(1214);
        float[] rows = SeededVectors.Uniform(random, Documents * Dimension);
        _queries = SeededVectors.Uniform(random, Queries * Dimension);
        _index = SeededVectors.Index(rows, Documents, Dimension);

        _context = GpuContext.Create();
        _kernel = new TiledCosineTopK(_context);
        _resident = DeviceEmbeddingMatrix.Upload(_context, rows, Documents, Dimension);

        // bench/README.md's GPU gate, rule 3: ILGPU compiles on first launch, so warm up first.
        _kernel.Search(_resident, _queries, Queries, K);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _resident.Dispose();
        _context.Dispose();
    }

    /// <summary>The SIMD path, one search per query.</summary>
    [Benchmark(Baseline = true)]
    public int SimdBaseline()
    {
        int seen = 0;
        for (int query = 0; query < Queries; query++)
        {
            seen += _index.Search(_queries.AsSpan(query * Dimension, Dimension), K).Count;
        }

        return seen;
    }

    /// <summary>The batch against the resident matrix, transfers of the queries and hits included.</summary>
    [Benchmark]
    public int GpuResident() => _kernel.Search(_resident, _queries, Queries, K).Count;
}
