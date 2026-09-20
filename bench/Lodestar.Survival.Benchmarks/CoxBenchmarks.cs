using BenchmarkDotNet.Attributes;

namespace Lodestar.Survival.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, and the
// build succeeds either way -- so following the rule breaks the run, not the compile.
#pragma warning disable CA1822

/// <summary>What a Cox fit costs against sample size and covariate count.</summary>
/// <remarks>
/// No incumbent: decision 0002 found no .NET survival package. The likelihood pass is linear in
/// the sample and quadratic in the covariates, per Newton iteration; the concordance is n log n
/// and ignores the covariates, so a row flat across Covariates means it dominates again.
/// </remarks>
[MemoryDiagnoser]
public class CoxBenchmarks
{
    private double[] _design = [];
    private double[] _durations = [];
    private bool[] _observed = [];

    /// <summary>Subjects: a trial, and a small registry.</summary>
    [Params(1_000, 10_000)]
    public int SampleSize { get; set; }

    /// <summary>Covariates per subject.</summary>
    [Params(2, 8)]
    public int Covariates { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Random random = new(684);
        _design = new double[SampleSize * Covariates];
        _durations = new double[SampleSize];
        _observed = new bool[SampleSize];
        for (int i = 0; i < SampleSize; i++)
        {
            double logHazard = 0.0;
            for (int j = 0; j < Covariates; j++)
            {
                double value = (random.NextDouble() * 2.0) - 1.0;
                _design[(i * Covariates) + j] = value;
                logHazard += (j % 2 == 0 ? 0.5 : -0.5) * value;
            }

            // Exponential durations rounded up to whole months, so ties are ordinary.
            _durations[i] = Math.Ceiling(-Math.Log(1.0 - random.NextDouble()) / Math.Exp(logHazard) * 24.0);
            // Roughly a third censored, as SurvivalBenchmarks has it.
            _observed[i] = random.Next(3) != 0;
        }
    }

    [Benchmark]
    public double Fit() =>
        CoxProportionalHazards.Fit(_design, _durations, _observed, Covariates).LogLikelihood;
}
