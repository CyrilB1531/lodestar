using Accord.Statistics.Models.Regression.Fitting;
using BenchmarkDotNet.Attributes;
using Lodestar.Stats.Regression;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, as GlmBenchmarks records.
#pragma warning disable CA1822

/// <summary>The multinomial logit against Accord's <c>MultinomialLogisticRegression</c>, the one .NET incumbent (#788).</summary>
/// <remarks>
/// Accord is LGPL-2.1 and archived, so it is raced rather than delegated to (decision 0004). Its
/// <c>LowerBoundNewtonRaphson</c> runs at a tolerance of <c>1e-10</c>, where its coefficients agree with this fit's to
/// <c>1e-8</c>; at its looser default a race would time a different answer. Its standard errors are not compared: they read
/// the lower-bound Hessian its algorithm iterates on, and measured 33% to 43% from the ones this fit and statsmodels report.
/// </remarks>
[MemoryDiagnoser]
public class MultinomialLogitBenchmarks
{
    private const int Regressors = 3;
    private double[] _design = [];
    private double[][] _inputs = [];
    private int[] _labels = [];

    [Params(200, 2_000)]
    public int SampleSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(788);
        _design = new double[SampleSize * Regressors];
        _inputs = new double[SampleSize][];
        _labels = new int[SampleSize];
        for (int row = 0; row < SampleSize; row++)
        {
            _inputs[row] = new double[Regressors];
            double first = 0.3;
            double second = -0.2;
            for (int column = 0; column < Regressors; column++)
            {
                double value = (random.NextDouble() * 2.0) - 1.0;
                _design[(row * Regressors) + column] = value;
                _inputs[row][column] = value;
                first += (0.8 - (0.5 * column)) * value;
                second += (-0.4 + (0.6 * column)) * value;
            }

            double total = 1.0 + Math.Exp(first) + Math.Exp(second);
            double draw = random.NextDouble() * total;
            int label = 2;
            if (draw < 1.0)
            {
                label = 0;
            }
            else if (draw < 1.0 + Math.Exp(first))
            {
                label = 1;
            }

            _labels[row] = label;
        }

        double ours = Lodestar().Coefficients[1][2];
        double theirs = Accord().Coefficients[1][2];
        if (!(Math.Abs(ours - theirs) <= 1e-8 * Math.Abs(ours)))
        {
            throw new InvalidOperationException($"Accord's coefficient is {theirs:R}, this fit's {ours:R}.");
        }
    }

    [Benchmark(Baseline = true)]
    public MultinomialLogitSummary Lodestar() => MultinomialLogit.Fit(_design, _labels, Regressors);

    [Benchmark]
    public Accord.Statistics.Models.Regression.MultinomialLogisticRegression Accord() =>
        new LowerBoundNewtonRaphson { MaxIterations = 1_000, Tolerance = 1e-10, ComputeStandardErrors = true }
            .Learn(_inputs, _labels);
}
