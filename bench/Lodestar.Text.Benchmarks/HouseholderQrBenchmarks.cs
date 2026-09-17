using BenchmarkDotNet.Attributes;
using Lodestar.Decomposition;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark block; no security use.
#pragma warning disable S2245, CA5394

/// <summary><see cref="QrDecomposition.Householder"/> on a tall block, the shape a range finder hands it.</summary>
/// <remarks>
/// Every reflector is applied down all rows of a row-major block, so its cost is the walk across rows; 20,000
/// rows by 30 is a randomized SVD's basis at a large corpus. No incumbent: the package against itself, A/B.
/// </remarks>
[MemoryDiagnoser]
public class HouseholderQrBenchmarks
{
    private double[] _block = [];

    /// <summary>Rows of the block, each 30 wide.</summary>
    [Params(2_000, 20_000)]
    public int Rows { get; set; }

    /// <summary>Columns of the block.</summary>
    public const int Columns = 30;

    [GlobalSetup]
    public void Setup()
    {
        var rng = new Random(20260917);
        _block = new double[Rows * Columns];
        for (int i = 0; i < _block.Length; i++)
        {
            _block[i] = (rng.NextDouble() * 2) - 1;
        }
    }

    [Benchmark]
    public int Householder() => QrDecomposition.Householder(_block, Rows, Columns).Q.Count;
}
