using BenchmarkDotNet.Attributes;
using Lodestar.Gpu.Compute;
using Lodestar.Text.Similarity;

namespace Lodestar.Gpu.Benchmarks;

// SonarLint S2245 / CA5394: a seeded Random builds a reproducible corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks.
#pragma warning disable CA1822

// CA1001 (owns disposable fields but is not IDisposable): BenchmarkDotNet owns the
// instance and calls the cleanup, which is where the accelerator is released.
#pragma warning disable CA1001

/// <summary>The MinHash kernel against <c>MinHash.Signature</c>, over the same coefficients.</summary>
/// <remarks>
/// The two sides do not divide the work the same way, and that is the measurement. The CPU path
/// hashes and minimises in one pass; the kernel takes hashes the host already computed and does
/// the minimisation alone. <c>GpuResident</c> is therefore the gate row for *the part the
/// accelerator took*, and <c>GpuWithHashing</c> sits beside it with the host pass inside the
/// measurement, which is what a caller starting from tokens actually pays.
/// </remarks>
[MemoryDiagnoser]
public class TiledMinHashSignaturesBenchmarks
{
    private const int TokensPerDocument = 24;
    private const int Vocabulary = 5_000;

    private GpuContext _context = null!;
    private TiledMinHashSignatures _kernel = null!;
    private DeviceTokenHashes _resident = null!;
    private MinHash _cpu = null!;
    private string[][] _tokens = [];
    private ulong[] _multipliers = [];
    private ulong[] _addends = [];

    /// <summary>Documents in the batch.</summary>
    [Params(5_000, 50_000)]
    public int Documents { get; set; }

    /// <summary>Signature length, which is also the estimate's resolution.</summary>
    [Params(64, 128)]
    public int Permutations { get; set; }

    /// <summary>The device each figure came from, which BenchmarkDotNet prints as a column.</summary>
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
        var random = new Random(4446);
        _tokens = [.. Enumerable.Range(0, Documents).Select(_ =>
            Enumerable.Range(0, TokensPerDocument)
                .Select(__ => $"term{random.Next(Vocabulary):D4}").ToArray())];

        _multipliers = new ulong[Permutations];
        _addends = new ulong[Permutations];
        for (int i = 0; i < Permutations; i++)
        {
            _multipliers[i] = (ulong)random.NextInt64(1, long.MaxValue);
            _addends[i] = (ulong)random.NextInt64(0, long.MaxValue);
        }

        _cpu = new MinHash(new MinHashPermutations(_multipliers, _addends));
        _context = GpuContext.Create();
        _kernel = new TiledMinHashSignatures(_context);
        _resident = DeviceTokenHashes.Upload(_context, Hash(_tokens));

        // Decision 0102, rule 3: ILGPU compiles on first launch, so this runs once on the
        // real corpus before anything is timed.
        _kernel.Signatures(_resident, _multipliers, _addends);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _resident.Dispose();
        _context.Dispose();
    }

    /// <summary>The CPU path, which hashes and minimises in one pass. The gate's baseline.</summary>
    [Benchmark(Baseline = true)]
    public int CpuBaseline()
    {
        int total = 0;
        foreach (string[] document in _tokens)
        {
            total += _cpu.Signature(document).Length;
        }

        return total;
    }

    /// <summary>The minimisation alone, over hashes the host already holds. The gate row.</summary>
    [Benchmark]
    public int GpuResident() => _kernel.Signatures(_resident, _multipliers, _addends).Count;

    /// <summary>Hashing on the host and uploading, which is what a caller starting from tokens pays.</summary>
    [Benchmark]
    public int GpuWithHashing()
    {
        using var resident = DeviceTokenHashes.Upload(_context, Hash(_tokens));
        return _kernel.Signatures(resident, _multipliers, _addends).Count;
    }

    /// <summary>The host's half: the same SHA-1 prefix the CPU path hashes with.</summary>
    private static uint[][] Hash(string[][] documents) =>
        [.. documents.Select(document => document.Select(TokenHash.Of).ToArray())];
}
