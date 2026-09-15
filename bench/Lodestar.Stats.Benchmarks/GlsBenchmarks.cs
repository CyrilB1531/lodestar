using BenchmarkDotNet.Attributes;
using Lodestar.Stats.Regression;
using MathNet.Numerics.LinearAlgebra;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, and the
// build succeeds either way -- so following the rule breaks the run, not the compile.
#pragma warning disable CA1822

/// <summary>
/// Generalized least squares under an AR(1) error covariance, against the GLS a Math.NET user writes by hand.
/// </summary>
/// <remarks>
/// Math.NET Numerics 5.0.0 exports no GLS: the incumbent is its Cholesky factor of the covariance and the
/// normal equations <c>(XᵀΣ⁻¹X)⁻¹ XᵀΣ⁻¹y</c> solved through it, which returns the coefficients and nothing else.
/// So the pair prices this package's whole table against an estimate alone (#771); <c>[GlobalSetup]</c> refuses
/// to time either side if their slopes differ by more than <c>1e-9</c> relative.
/// </remarks>
[MemoryDiagnoser]
public class GlsBenchmarks
{
    private const int Regressors = 4;

    private double[] _design = [];
    private double[] _response = [];
    private double[] _covariance = [];
    private Matrix<double> _exogenous = null!;
    private Vector<double> _endogenous = null!;
    private Matrix<double> _sigma = null!;

    /// <summary>Rows, which is also the order of the covariance, so the cost grows with its cube.</summary>
    [Params(50, 200, 500, 1_000)]
    public int SampleSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Random random = new(771);
        _design = new double[SampleSize * Regressors];
        _response = new double[SampleSize];
        _covariance = new double[SampleSize * SampleSize];
        _exogenous = Matrix<double>.Build.Dense(SampleSize, Regressors + 1);

        double shock = 0.0;
        for (int row = 0; row < SampleSize; row++)
        {
            _exogenous[row, 0] = 1.0;
            double signal = 1.0;
            for (int column = 0; column < Regressors; column++)
            {
                double value = random.NextDouble();
                _design[(row * Regressors) + column] = value;
                _exogenous[row, column + 1] = value;
                signal += (column + 1) * value;
            }

            // AR(1) errors at 0.6, the covariance handed to both sides below.
            shock = (0.6 * shock) + (random.NextDouble() - 0.5);
            _response[row] = signal + shock;
            for (int other = 0; other < SampleSize; other++)
            {
                _covariance[(row * SampleSize) + other] = Math.Pow(0.6, Math.Abs(row - other));
            }
        }

        _endogenous = Vector<double>.Build.Dense(_response);
        _sigma = Matrix<double>.Build.Dense(SampleSize, SampleSize, (row, column) => _covariance[(row * SampleSize) + column]);

        double lodestar = Lodestar_Gls();
        double mathNet = MathNet_Gls();
        // Negated, so a NaN on either side refuses too: every ordered comparison with NaN is false.
        if (!(Math.Abs(lodestar - mathNet) <= 1e-9 * Math.Abs(mathNet)))
        {
            throw new InvalidOperationException(
                $"The two slopes differ at n = {SampleSize}: {lodestar:R} against {mathNet:R}. Not timing them.");
        }
    }

    [Benchmark(Baseline = true)]
    public double Lodestar_Gls() =>
        GeneralizedLeastSquares.Fit(_design, _response, _covariance, Regressors).Coefficients[1];

    [Benchmark]
    public double MathNet_Gls()
    {
        var cholesky = _sigma.Cholesky();
        Matrix<double> whitenedDesign = cholesky.Solve(_exogenous);
        Vector<double> whitenedResponse = cholesky.Solve(_endogenous);
        Vector<double> coefficients = _exogenous.TransposeThisAndMultiply(whitenedDesign)
            .Solve(_exogenous.TransposeThisAndMultiply(whitenedResponse));
        return coefficients[1];
    }
}
