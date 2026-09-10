using BenchmarkDotNet.Attributes;

namespace Lodestar.Survival.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, and the
// build succeeds either way -- so following the rule breaks the run, not the compile.
#pragma warning disable CA1822

/// <summary>
/// The three survival estimators against sample size, and against each other.
/// </summary>
/// <remarks>
/// There is no incumbent to race: decision 0099 records the empty NuGet searches. What
/// is measured instead is the shape of the cost. Ties are a parameter rather than an
/// accident — the two curves walk one step table, and they part on it: Kaplan-Meier
/// multiplies once per step, Nelson-Aalen's tie correction sums once per event.
/// </remarks>
[MemoryDiagnoser]
public class SurvivalBenchmarks
{
    private double[] _durations = [];
    private bool[] _observed = [];
    private double[] _armA = [];
    private bool[] _armAObserved = [];
    private double[] _armB = [];
    private bool[] _armBObserved = [];

    /// <summary>Subjects. A trial is hundreds; a registry study is hundreds of thousands.</summary>
    [Params(1_000, 100_000)]
    public int SampleSize { get; set; }

    /// <summary>
    /// How many distinct durations the sample holds, as a fraction of its size.
    /// </summary>
    /// <remarks>
    /// 100 means every duration distinct; 4 means heavy ties. The step table is what
    /// both estimators walk, so this is the parameter that moves them, not SampleSize alone.
    /// </remarks>
    [Params(100, 4)]
    public int DistinctPercent { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Random random = new(569);
        int distinct = Math.Max(1, SampleSize * DistinctPercent / 100);

        _durations = new double[SampleSize];
        _observed = new bool[SampleSize];
        for (int i = 0; i < SampleSize; i++)
        {
            _durations[i] = random.Next(distinct) + 1;
            // Roughly a third censored, which is an ordinary trial's shape.
            _observed[i] = random.Next(3) != 0;
        }

        int half = SampleSize / 2;
        _armA = _durations.AsSpan(0, half).ToArray();
        _armAObserved = _observed.AsSpan(0, half).ToArray();
        _armB = _durations.AsSpan(half).ToArray();
        _armBObserved = _observed.AsSpan(half).ToArray();
    }

    [Benchmark(Baseline = true)]
    public KaplanMeierCurve KaplanMeierEstimate() =>
        KaplanMeier.Estimate(_durations, _observed);

    [Benchmark]
    public NelsonAalenCurve NelsonAalenEstimate() =>
        NelsonAalen.Estimate(_durations, _observed);

    [Benchmark]
    public LogRankResult LogRankTest() =>
        LogRank.Test(_armA, _armAObserved, _armB, _armBObserved);
}
