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

/// <summary>What residency buys across two operations, which is the only place it shows.</summary>
/// <remarks>
/// Decision 0102 deferred these types until three kernels existed, because chainability is a
/// claim about two operations sharing a residency. <c>RoundTripped</c> is the baseline on
/// purpose: the question is not whether an accelerator beats a processor — section 24 asks
/// that — but what a caller loses by letting an intermediate cross the bus, and putting the
/// round-tripped version in the denominator makes the ratio read as what residency is worth.
/// <c>CpuBaseline</c> is there so that ratio cannot be read in a vacuum.
/// </remarks>
[MemoryDiagnoser]
public class ChainedProductBenchmarks
{
    private const int Inner = 4_000;
    private const double Density = 0.004;

    private GpuContext _context = null!;
    private TiledSparseDenseProduct _kernel = null!;
    private DeviceSparseMatrix _first = null!;
    private DeviceSparseMatrix _second = null!;
    private CsrMatrix _hostFirst = null!;
    private CsrMatrix _hostSecond = null!;
    private double[] _block = [];

    /// <summary>Rows of the outer matrix, and so of the answer.</summary>
    [Params(2_000, 20_000)]
    public int Rows { get; set; }

    /// <summary>Columns of the dense operand, carried through both products.</summary>
    [Params(32, 128)]
    public int Width { get; set; }

    /// <summary>The device each figure came from, which BenchmarkDotNet prints as a column.</summary>
    /// <remarks>
    /// A property alone was not enough: BenchmarkDotNet reports [Params] and columns, not
    /// arbitrary properties, so the first run of these published numbers with no way to
    /// tell which device produced them.
    /// </remarks>
    [ParamsSource(nameof(Devices))]
    public string Device { get; set; } = string.Empty;

    /// <summary>The one device this run uses, named so the report carries it.</summary>
    public static IEnumerable<string> Devices() => [Describe()];

    private static string Describe()
    {
        using var probe = GpuContext.Create();
        return probe.IsHardwareGpu ? probe.DeviceName : $"{probe.DeviceName} (NOT a GPU)";
    }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(4445);
        _hostFirst = Sparse(random, Rows, Inner);
        _hostSecond = Sparse(random, Inner, Inner);
        _block = new double[Inner * Width];
        for (int i = 0; i < _block.Length; i++)
        {
            _block[i] = (random.NextDouble() * 2.0) - 1.0;
        }

        _context = GpuContext.Create();
        _kernel = new TiledSparseDenseProduct(_context);
        _first = Resident(_hostFirst);
        _second = Resident(_hostSecond);

        // Decision 0102, rule 3: ILGPU compiles on first launch, so this runs once on the
        // real corpus before anything is timed.
        using DeviceDenseBlock warm = DeviceDenseBlock.Upload(_context, _block, Inner, Width);
        using DeviceDenseBlock product = _kernel.Multiply(_second, warm);
        _kernel.Multiply(_first, product).Dispose();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _first.Dispose();
        _second.Dispose();
        _context.Dispose();
    }

    /// <summary>Two products with a download and re-upload between them. The baseline.</summary>
    [Benchmark(Baseline = true)]
    public int RoundTripped()
    {
        double[] inner = _kernel.Multiply(_second, _block, Width);
        return _kernel.Multiply(_first, inner, Width).Length;
    }

    /// <summary>The same two products with the intermediate never leaving the accelerator.</summary>
    [Benchmark]
    public int Chained()
    {
        using DeviceDenseBlock operand = DeviceDenseBlock.Upload(_context, _block, Inner, Width);
        using DeviceDenseBlock inner = _kernel.Multiply(_second, operand);
        using DeviceDenseBlock outer = _kernel.Multiply(_first, inner);
        return outer.Download().Length;
    }

    /// <summary>The CPU path composed twice, for the absolute scale the ratio sits on.</summary>
    [Benchmark]
    public int CpuBaseline() => _hostFirst.Multiply(_hostSecond.Multiply(_block, Width), Width).Length;

    private static CsrMatrix Sparse(Random random, int rows, int columns)
    {
        var values = new List<double>();
        var indices = new List<int>();
        var pointers = new int[rows + 1];
        int perRow = Math.Max(1, (int)(columns * Density));
        for (int row = 0; row < rows; row++)
        {
            pointers[row] = values.Count;
            int column = 0;
            for (int taken = 0; taken < perRow && column < columns; taken++)
            {
                column += 1 + random.Next(columns / perRow);
                if (column >= columns)
                {
                    break;
                }

                indices.Add(column);
                values.Add(random.NextDouble());
            }
        }

        pointers[rows] = values.Count;
        return new CsrMatrix(rows, columns, [.. values], [.. indices], pointers);
    }

    private DeviceSparseMatrix Resident(CsrMatrix matrix) => DeviceSparseMatrix.Upload(
        _context, matrix.RowPointers, matrix.ColumnIndices, matrix.Values,
        matrix.RowCount, matrix.ColumnCount);
}
