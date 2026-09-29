using BenchmarkDotNet.Attributes;
using Lodestar.Abstractions;
using Lodestar.Extensions.MathNet;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark block; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks.
#pragma warning disable CA1822

/// <summary><c>MathNetInterop.ToCsrMatrix</c> on the storages it walks: a diagonal and a mostly-zero dense matrix.</summary>
/// <remarks>
/// No incumbent: Math.NET has no CSR type of Lodestar's to convert to, so the package against itself, A/B (#1220).
/// The diagonal is the case the per-cell walk made quadratic; the dense one is the case it did not.
/// </remarks>
[MemoryDiagnoser]
public class MathNetInteropBenchmarks
{
    private Matrix<double> _diagonal = null!;
    private Matrix<double> _dense = null!;

    /// <summary>The side of both matrices.</summary>
    [Params(1_000, 4_000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var rng = new Random(20260929);
        _diagonal = DiagonalMatrix.Create(Size, Size, i => i + 1.0);
        _dense = DenseMatrix.Create(Size, Size, (_, _) => rng.NextDouble() < 0.01 ? rng.NextDouble() : 0.0);
    }

    [Benchmark]
    public int Diagonal() => MathNetInterop.ToCsrMatrix(_diagonal).NonZeroCount;

    [Benchmark]
    public int SparseDense() => MathNetInterop.ToCsrMatrix(_dense).NonZeroCount;
}
