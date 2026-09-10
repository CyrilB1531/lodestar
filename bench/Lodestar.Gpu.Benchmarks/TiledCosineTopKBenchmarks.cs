using BenchmarkDotNet.Attributes;
using Lodestar.Embeddings.Search;
using Lodestar.Gpu.Compute;

namespace Lodestar.Gpu.Benchmarks;

// SonarLint S2245 / CA5394: a seeded Random builds a reproducible corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, and the
// build succeeds either way -- so following the rule breaks the run, not the compile.
#pragma warning disable CA1822

// CA1001 (owns disposable fields but is not IDisposable): BenchmarkDotNet owns the
// instance and calls the cleanup, which is where the accelerator is released.
#pragma warning disable CA1001

/// <summary>The tiled cosine + top-k kernel against this repository's SIMD path.</summary>
/// <remarks>
/// Decision 0102's gate is <c>GpuResident</c> against <c>SimdBaseline</c>: transfers are
/// inside the measured region and the baseline is our own path in the same process.
/// <c>GpuKernelOnly</c> sits beside it, excluded from the gate, to show where the time
/// goes. Read <c>Accelerator</c> in the output before believing any of it — a run with no
/// GPU reports the CPU accelerator and answers a different question.
/// </remarks>
[MemoryDiagnoser]
public class TiledCosineTopKBenchmarks
{
    private const int Dimension = 384;
    private const int TopK = 10;

    private GpuContext _context = null!;
    private DeviceEmbeddingMatrix _resident = null!;
    private TiledCosineTopK _kernel = null!;
    private EmbeddingIndex _index = null!;
    private float[] _rows = [];
    private float[] _queries = [];

    /// <summary>Rows in the corpus swept by every query.</summary>
    [Params(10_000, 100_000)]
    public int Documents { get; set; }

    /// <summary>Queries in one batch. One is the worst case for a GPU; a batch is the workload.</summary>
    [Params(1, 256)]
    public int Queries { get; set; }

    /// <summary>Which accelerator produced the numbers, so a report cannot omit it.</summary>
    public string Accelerator => _context.IsCpuAccelerator ? "CPU (no GPU present)" : "GPU";

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(444);
        _rows = new float[Documents * Dimension];
        for (int i = 0; i < _rows.Length; i++)
        {
            _rows[i] = (float)((random.NextDouble() * 2.0) - 1.0);
        }

        _queries = new float[Queries * Dimension];
        for (int i = 0; i < _queries.Length; i++)
        {
            _queries[i] = (float)((random.NextDouble() * 2.0) - 1.0);
        }

        _index = new EmbeddingIndex(Dimension);
        for (int row = 0; row < Documents; row++)
        {
            _index.Add(_rows.AsSpan(row * Dimension, Dimension));
        }

        _context = GpuContext.Create();
        _kernel = new TiledCosineTopK(_context);
        _resident = DeviceEmbeddingMatrix.Upload(_context, _rows, Documents, Dimension);

        // Decision 0102, rule 3: ILGPU compiles a kernel on first launch, so this runs
        // both once on the real corpus before anything is timed.
        _kernel.Search(_resident, _queries, Queries, TopK);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _resident.Dispose();
        _context.Dispose();
    }

    /// <summary>The SIMD path, one search per query. This is the gate's baseline.</summary>
    [Benchmark(Baseline = true)]
    public int SimdBaseline()
    {
        int seen = 0;
        for (int query = 0; query < Queries; query++)
        {
            seen += _index.Search(_queries.AsSpan(query * Dimension, Dimension), TopK).Count;
        }

        return seen;
    }

    /// <summary>Upload, sweep and read back: what a caller pays with the matrix already resident.</summary>
    /// <remarks>The gate row. Query transfer and result read-back are both inside it.</remarks>
    [Benchmark]
    public int GpuResident() => _kernel.Search(_resident, _queries, Queries, TopK).Count;

    /// <summary>The corpus uploaded per call, which is what a GPU package must not do.</summary>
    /// <remarks>
    /// Not the gate — it is the row that says why residency exists. #444's own constraint is
    /// that one kernel per call is time spent in PCIe, and this is that claim priced.
    /// </remarks>
    [Benchmark]
    public int GpuFromHost()
    {
        using var matrix = DeviceEmbeddingMatrix.Upload(_context, _rows, Documents, Dimension);
        return _kernel.Search(matrix, _queries, Queries, TopK).Count;
    }
}
