using BenchmarkDotNet.Attributes;
using Lodestar.Abstractions;
using Lodestar.Decomposition;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

/// <summary>
/// <see cref="TruncatedSvd"/> and <see cref="Nmf"/> at a vocabulary's width, where a row-major walk
/// down a column strides the whole feature count per element.
/// </summary>
/// <remarks>
/// <see cref="DecompositionBenchmarks"/> is fixed at 500 columns by the ML.NET row type it shares, a
/// width at which that stride still fits the cache; 20,000 is where the column-major kernels differ.
/// No incumbent: this class prices the package against itself, A/B.
/// </remarks>
[MemoryDiagnoser]
public class DecompositionWidthBenchmarks
{
    private const int Rows = 2000;
    private const int NonZerosPerRow = 40;
    private const int Rank = 20;
    private const int Seed = 20260917;

    private CsrMatrix _matrix = null!;
    private TruncatedSvd _fitted = null!;

    /// <summary>Features of the corpus.</summary>
    [Params(500, 20_000)]
    public int Columns { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var rng = new Random(Seed);
        var values = new double[Rows * NonZerosPerRow];
        var columnIndices = new int[Rows * NonZerosPerRow];
        var rowPointers = new int[Rows + 1];
        int cursor = 0;
        for (int row = 0; row < Rows; row++)
        {
            var chosen = new SortedSet<int>();
            while (chosen.Count < NonZerosPerRow)
            {
                chosen.Add(rng.Next(Columns));
            }
            foreach (int column in chosen)
            {
                columnIndices[cursor] = column;
                values[cursor] = 1 + (rng.NextDouble() * 4);
                cursor++;
            }
            rowPointers[row + 1] = cursor;
        }

        _matrix = new CsrMatrix(Rows, Columns, values, columnIndices, rowPointers);
        _fitted = TruncatedSvd.Fit(_matrix, Rank, new TruncatedSvdOptions { Seed = Seed });
    }

    [Benchmark]
    public int TruncatedSvd_Rank20() =>
        TruncatedSvd.Fit(_matrix, Rank, new TruncatedSvdOptions { Seed = Seed }).ComponentCount;

    [Benchmark]
    public int TruncatedSvd_Transform() => _fitted.Transform(_matrix).Length;

    [Benchmark]
    public int Nmf_Frobenius_Rank20() =>
        Nmf.Fit(_matrix, Rank, new NmfOptions { Seed = Seed, MaxIterations = 50, Tolerance = 0 }).Iterations;

    [Benchmark]
    public int Nmf_KullbackLeibler_Rank20() =>
        Nmf.Fit(
            _matrix,
            Rank,
            new NmfOptions { Seed = Seed, MaxIterations = 50, Tolerance = 0, BetaLoss = NmfBetaLoss.KullbackLeibler })
        .Iterations;
}
