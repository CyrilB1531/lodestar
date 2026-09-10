using BenchmarkDotNet.Attributes;
using Lodestar.Gpu.Compute;
using Lodestar.Text.Distances;

namespace Lodestar.Gpu.Benchmarks;

// SonarLint S2245 / CA5394: a seeded Random builds a reproducible corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks.
#pragma warning disable CA1822

// CA1001 (owns disposable fields but is not IDisposable): BenchmarkDotNet owns the
// instance and calls the cleanup, which is where the accelerator is released.
#pragma warning disable CA1001

/// <summary>The Myers kernel against <c>Levenshtein.Distance</c>, which is bit-parallel too.</summary>
/// <remarks>
/// <strong>This is the row expected to miss decision 0102's gate, and publishing that is the
/// point.</strong> Myers collapses a dynamic-programming row into one machine word on either
/// side, so the CPU already spends tens of nanoseconds on a short pair and the accelerator has
/// to amortise a renaming, two transfers and a launch on top of that. A failed gate is a row in
/// performance.md and a kernel that does not ship, which is worth more to the next reader than
/// an absence. Read <c>Accelerator</c> before believing any figure here.
/// </remarks>
[MemoryDiagnoser]
public class BitParallelEditDistanceBenchmarks
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

    private GpuContext _context = null!;
    private DeviceTextBlock _resident = null!;
    private BitParallelEditDistance _kernel = null!;
    private string[] _texts = [];
    private string _pattern = string.Empty;

    /// <summary>Strings compared against the pattern in one call.</summary>
    [Params(10_000, 200_000)]
    public int Texts { get; set; }

    /// <summary>How long each string is. Myers costs one word per character of text.</summary>
    [Params(32, 256)]
    public int TextLength { get; set; }

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
        var random = new Random(4444);
        _pattern = new string([.. Enumerable.Range(0, 24).Select(_ => Alphabet[random.Next(Alphabet.Length)])]);
        _texts = [.. Enumerable.Range(0, Texts).Select(_ =>
            new string([.. Enumerable.Range(0, TextLength).Select(__ => Alphabet[random.Next(Alphabet.Length)])]))];

        _context = GpuContext.Create();
        _kernel = new BitParallelEditDistance(_context);
        _resident = DeviceTextBlock.Upload(_context, _pattern, _texts);

        // Decision 0102, rule 3: ILGPU compiles on first launch, so this runs once on the
        // real corpus before anything is timed.
        _kernel.Distance(_pattern, _resident);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _resident.Dispose();
        _context.Dispose();
    }

    /// <summary>The bit-parallel path this repository already ships, one call per string.</summary>
    [Benchmark(Baseline = true)]
    public long CpuBaseline()
    {
        long total = 0;
        foreach (string text in _texts)
        {
            total += Levenshtein.Distance(_pattern, text);
        }

        return total;
    }

    /// <summary>The kernel with the batch already renamed and resident. The gate row.</summary>
    /// <remarks>The equality table upload and the distance read-back are both inside it.</remarks>
    [Benchmark]
    public long GpuResident() => _kernel.Distance(_pattern, _resident).Length;

    /// <summary>Renaming and uploading per call, which is what a one-shot caller pays.</summary>
    /// <remarks>
    /// Not the gate. It prices the half a resident batch avoids, and on this kernel it is the
    /// half that decides the answer: renaming is a pass over every character on the host, in
    /// the language the CPU baseline is already written in.
    /// </remarks>
    [Benchmark]
    public long GpuFromHost()
    {
        using var block = DeviceTextBlock.Upload(_context, _pattern, _texts);
        return _kernel.Distance(_pattern, block).Length;
    }
}
