using BenchmarkDotNet.Attributes;
using MathNet.Numerics.Distributions;
using MetaDistributions = Meta.Numerics.Statistics.Distributions;

namespace Lodestar.Stats.Benchmarks;

// CA1822: BenchmarkDotNet rejects static benchmarks — see StatsBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// The lower tail and the quantile of the t, F, chi-squared and normal laws against Math.NET
/// Numerics 5.0.0 and Meta.Numerics 4.2.0, the two .NET libraries that publish them (#1158).
/// </summary>
/// <remarks>
/// Every value is checked against both before anything is timed, and a difference past 1e-9 is
/// printed. Meta.Numerics' laws are objects built once in the setup; Math.NET's and
/// <see cref="Distributions"/>' are static calls. bench/README.md §60 has the method.
/// </remarks>
[MemoryDiagnoser]
public class DistributionIncumbentBenchmarks
{
    // Instance fields, not literals: with constant arguments the JIT folds a closed-form tail into
    // its answer, and the row times nothing (DistributionTailBenchmarks measured it).
    private readonly double _t = -2.0;
    private readonly double _tDf = 10.0;
    private readonly double _f = 4.0;
    private readonly double _fNumerator = 2.0;
    private readonly double _fDenominator = 20.0;
    private readonly double _chi = 9.488;
    private readonly double _chiDf = 4.0;
    private readonly double _z = -1.96;
    private readonly double _upper = 0.975;
    private readonly double _critical = 0.95;

    private MetaDistributions.StudentDistribution _metaStudent = null!;
    private MetaDistributions.FisherDistribution _metaFisher = null!;
    private MetaDistributions.ChiSquaredDistribution _metaChi = null!;
    private MetaDistributions.NormalDistribution _metaNormal = null!;

    [GlobalSetup]
    public void Setup()
    {
        _metaStudent = new MetaDistributions.StudentDistribution(_tDf);
        _metaFisher = new MetaDistributions.FisherDistribution(_fNumerator, _fDenominator);
        _metaChi = new MetaDistributions.ChiSquaredDistribution((int)_chiDf);
        _metaNormal = new MetaDistributions.NormalDistribution();
        CheckAgreement();
    }

    [Benchmark(Baseline = true)]
    public double Lodestar_StudentCdf() => Distributions.StudentCdf(_t, _tDf);

    [Benchmark]
    public double MathNet_StudentCdf() => StudentT.CDF(0.0, 1.0, _tDf, _t);

    [Benchmark]
    public double MetaNumerics_StudentCdf() => _metaStudent.LeftProbability(_t);

    [Benchmark]
    public double Lodestar_StudentQuantile() => Distributions.StudentQuantile(_upper, _tDf);

    [Benchmark]
    public double MathNet_StudentQuantile() => StudentT.InvCDF(0.0, 1.0, _tDf, _upper);

    [Benchmark]
    public double MetaNumerics_StudentQuantile() => _metaStudent.InverseLeftProbability(_upper);

    [Benchmark]
    public double Lodestar_FisherCdf() => Distributions.FisherCdf(_f, _fNumerator, _fDenominator);

    [Benchmark]
    public double MathNet_FisherCdf() => FisherSnedecor.CDF(_fNumerator, _fDenominator, _f);

    [Benchmark]
    public double MetaNumerics_FisherCdf() => _metaFisher.LeftProbability(_f);

    [Benchmark]
    public double Lodestar_FisherQuantile() => Distributions.FisherQuantile(_critical, _fNumerator, _fDenominator);

    [Benchmark]
    public double MathNet_FisherQuantile() => FisherSnedecor.InvCDF(_fNumerator, _fDenominator, _critical);

    [Benchmark]
    public double MetaNumerics_FisherQuantile() => _metaFisher.InverseLeftProbability(_critical);

    [Benchmark]
    public double Lodestar_ChiSquaredCdf() => Distributions.ChiSquaredCdf(_chi, _chiDf);

    [Benchmark]
    public double MathNet_ChiSquaredCdf() => ChiSquared.CDF(_chiDf, _chi);

    [Benchmark]
    public double MetaNumerics_ChiSquaredCdf() => _metaChi.LeftProbability(_chi);

    [Benchmark]
    public double Lodestar_ChiSquaredQuantile() => Distributions.ChiSquaredQuantile(_critical, _chiDf);

    [Benchmark]
    public double MathNet_ChiSquaredQuantile() => ChiSquared.InvCDF(_chiDf, _critical);

    [Benchmark]
    public double MetaNumerics_ChiSquaredQuantile() => _metaChi.InverseLeftProbability(_critical);

    [Benchmark]
    public double Lodestar_NormalCdf() => Distributions.NormalCdf(_z);

    [Benchmark]
    public double MathNet_NormalCdf() => Normal.CDF(0.0, 1.0, _z);

    [Benchmark]
    public double MetaNumerics_NormalCdf() => _metaNormal.LeftProbability(_z);

    [Benchmark]
    public double Lodestar_NormalQuantile() => Distributions.NormalQuantile(_upper);

    [Benchmark]
    public double MathNet_NormalQuantile() => Normal.InvCDF(0.0, 1.0, _upper);

    [Benchmark]
    public double MetaNumerics_NormalQuantile() => _metaNormal.InverseLeftProbability(_upper);

    /// <summary>Records every value that differs from Lodestar's past 1e-9, for bench/README.md to quote.</summary>
    private void CheckAgreement()
    {
        (string Name, double Ours, double MathNet, double Meta)[] rows =
        [
            ("t cdf", Lodestar_StudentCdf(), MathNet_StudentCdf(), MetaNumerics_StudentCdf()),
            ("t quantile", Lodestar_StudentQuantile(), MathNet_StudentQuantile(), MetaNumerics_StudentQuantile()),
            ("F cdf", Lodestar_FisherCdf(), MathNet_FisherCdf(), MetaNumerics_FisherCdf()),
            ("F quantile", Lodestar_FisherQuantile(), MathNet_FisherQuantile(), MetaNumerics_FisherQuantile()),
            ("chi-squared cdf", Lodestar_ChiSquaredCdf(), MathNet_ChiSquaredCdf(), MetaNumerics_ChiSquaredCdf()),
            ("chi-squared quantile", Lodestar_ChiSquaredQuantile(), MathNet_ChiSquaredQuantile(), MetaNumerics_ChiSquaredQuantile()),
            ("normal cdf", Lodestar_NormalCdf(), MathNet_NormalCdf(), MetaNumerics_NormalCdf()),
            ("normal quantile", Lodestar_NormalQuantile(), MathNet_NormalQuantile(), MetaNumerics_NormalQuantile()),
        ];
        foreach ((string name, double ours, double mathNet, double meta) in rows)
        {
            MetaNumericsAgreement.Record($"{name}, Math.NET", ours, mathNet);
            MetaNumericsAgreement.Record($"{name}, Meta.Numerics", ours, meta);
        }
        MetaNumericsAgreement.Report(nameof(DistributionIncumbentBenchmarks));
    }
}
