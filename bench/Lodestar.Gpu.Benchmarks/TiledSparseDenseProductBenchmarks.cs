using BenchmarkDotNet.Attributes;
using Lodestar.Abstractions;
using Lodestar.Gpu.Compute;

namespace Lodestar.Gpu.Benchmarks;

// SonarLint S2245 / CA5394: a seeded Random builds a reproducible corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks.
#pragma warning disable CA1822

// CA1001 (owns disposable fields but is not IDisposable): BenchmarkDotNet owns the
// instance and calls the cleanup, which is where the accelerator is released.
#pragma warning disable CA1001

/// <summary>The SpMM kernel against <c>CsrMatrix.Multiply</c>, at a vectorizer's shape.</summary>
/// <remarks>
/// Decision 0102's gate is <c>GpuResident</c> against <c>CpuBaseline</c>, with the dense
/// operand and the result transferred inside the measured region. <c>GpuFromHost</c> prices
/// the matrix upload residency exists to avoid. Read <c>Accelerator</c> before believing any
/// of it, and note both sides are double precision because <c>CsrMatrix</c> is.
/// </remarks>
[MemoryDiagnoser]
public class TiledSparseDenseProductBenchmarks
{
    private const int Terms = 20_000;
    private const double Density = 0.002;

    private GpuContext _context = null!;
    private DeviceSparseMatrix _resident = null!;
    private TiledSparseDenseProduct _kernel = null!;
    private CsrMatrix _matrix = null!;
    private double[] _block = [];

    /// <summary>Rows in the sparse matrix: documents, at a vectorizer's shape.</summary>
    [Params(5_000, 50_000)]
    public int Documents { get; set; }

    /// <summary>Columns in the dense operand. 64 is a projection, 256 a wide one.</summary>
    [Params(64, 256)]
    public int Width { get; set; }

    /// <summary>Which accelerator produced the numbers, so a report cannot omit it.</summary>
    public string Accelerator => _context.IsCpuAccelerator ? "CPU (no GPU present)" : "GPU";

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(4442);
        var values = new List<double>();
        var indices = new List<int>();
        var pointers = new int[Documents + 1];
        int perRow = Math.Max(1, (int)(Terms * Density));
        for (int row = 0; row < Documents; row++)
        {
            pointers[row] = values.Count;
            int column = 0;
            for (int taken = 0; taken < perRow && column < Terms; taken++)
            {
                column += 1 + random.Next(Terms / perRow);
                if (column >= Terms)
                {
                    break;
                }

                indices.Add(column);
                values.Add(random.NextDouble());
            }
        }

        pointers[Documents] = values.Count;
        _matrix = new CsrMatrix(Documents, Terms, [.. values], [.. indices], pointers);

        _block = new double[Terms * Width];
        for (int i = 0; i < _block.Length; i++)
        {
            _block[i] = (random.NextDouble() * 2.0) - 1.0;
        }

        _context = GpuContext.Create();
        _kernel = new TiledSparseDenseProduct(_context);
        _resident = DeviceSparseMatrix.Upload(
            _context, _matrix.RowPointers, _matrix.ColumnIndices, _matrix.Values,
            _matrix.RowCount, _matrix.ColumnCount);

        // Decision 0102, rule 3: ILGPU compiles on first launch, so this runs once on the
        // real corpus before anything is timed.
        _kernel.Multiply(_resident, _block, Width);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _resident.Dispose();
        _context.Dispose();
    }

    /// <summary>The dense-block product this repository already ships. The gate's baseline.</summary>
    [Benchmark(Baseline = true)]
    public int CpuBaseline() => _matrix.Multiply(_block, Width).Length;

    /// <summary>The kernel with the matrix already resident. The gate row.</summary>
    [Benchmark]
    public int GpuResident() => _kernel.Multiply(_resident, _block, Width).Length;

    /// <summary>The matrix uploaded per call, which is what residency exists to avoid.</summary>
    [Benchmark]
    public int GpuFromHost()
    {
        using var matrix = DeviceSparseMatrix.Upload(
            _context, _matrix.RowPointers, _matrix.ColumnIndices, _matrix.Values,
            _matrix.RowCount, _matrix.ColumnCount);
        return _kernel.Multiply(matrix, _block, Width).Length;
    }
}
