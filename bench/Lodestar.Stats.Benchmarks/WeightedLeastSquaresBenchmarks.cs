using BenchmarkDotNet.Attributes;
using Lodestar.Stats.Regression;
using MathNet.Numerics.LinearRegression;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, and the
// build succeeds either way -- so following the rule breaks the run, not the compile.
#pragma warning disable CA1822

/// <summary>Weighted least squares against Math.NET Numerics' <c>WeightedRegression.Weighted</c> (#782).</summary>
/// <remarks>
/// Math.NET is MIT, maintained, and the one numerics library <c>src/</c> already reaches; its weighted regression
/// returns the coefficients and nothing else, solved through the normal equations. So the pair prices this
/// package's whole table against an estimate alone, and <c>[GlobalSetup]</c> refuses to time either side if their
/// slopes differ by more than <c>1e-9</c> relative.
/// </remarks>
[MemoryDiagnoser]
public class WeightedLeastSquaresBenchmarks
{
    private const int Regressors = 4;

    private double[] _design = [];
    private double[] _response = [];
    private double[] _weights = [];
    private double[][] _jagged = [];

    /// <summary>Rows. Four regressors throughout, as <c>OlsBenchmarks</c>.</summary>
    [Params(200, 2_000, 20_000, 200_000)]
    public int SampleSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Random random = new(768);
        _design = new double[SampleSize * Regressors];
        _response = new double[SampleSize];
        _weights = new double[SampleSize];
        _jagged = new double[SampleSize][];

        for (int row = 0; row < SampleSize; row++)
        {
            var line = new double[Regressors];
            double signal = 1.0;
            for (int column = 0; column < Regressors; column++)
            {
                double value = random.NextDouble();
                line[column] = value;
                _design[(row * Regressors) + column] = value;
                signal += (column + 1) * value;
            }

            // A variance that grows with the row's own signal, and the inverse-variance weight that answers it.
            double spread = 0.2 + (0.2 * signal);
            _jagged[row] = line;
            _response[row] = signal + (spread * (random.NextDouble() - 0.5));
            _weights[row] = 1.0 / (spread * spread);
        }

        double lodestar = Lodestar_Wls();
        double mathNet = MathNet_Wls();
        // Negated, so a NaN on either side refuses too: every ordered comparison with NaN is false.
        if (!(Math.Abs(lodestar - mathNet) <= 1e-9 * Math.Abs(mathNet)))
        {
            throw new InvalidOperationException(
                $"The two slopes differ at n = {SampleSize}: {lodestar:R} against {mathNet:R}. Not timing them.");
        }
    }

    [Benchmark(Baseline = true)]
    public double Lodestar_Wls() =>
        WeightedLeastSquares.Fit(_design, _response, _weights, Regressors).Coefficients[1];

    [Benchmark]
    public double MathNet_Wls() =>
        WeightedRegression.Weighted(_jagged, _response, _weights, intercept: true)[1];
}
