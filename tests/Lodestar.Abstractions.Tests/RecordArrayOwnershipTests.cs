using Lodestar.Cluster;
using Lodestar.Decomposition;
using Lodestar.Stats;
using Lodestar.Survival;
using Xunit;

namespace Lodestar.Abstractions.Tests;

/// <summary>
/// What each record's remark promises of its arrays (#1305): taken and exposed as they are, so a write through the
/// caller's reference changes the record and what it equals, and its hash, which reads lengths only, stays put.
/// </summary>
public sealed class RecordArrayOwnershipTests
{
    [Fact]
    public void Truncated_svd_options_hold_the_callers_random_matrix()
    {
        double[] omega = [1.0, 2.0];
        var options = new TruncatedSvdOptions { RandomMatrix = omega };
        var copy = new TruncatedSvdOptions { RandomMatrix = [1.0, 2.0] };

        AssertWriteReaches(omega, options.RandomMatrix, () => options.Equals(copy), () => options.GetHashCode());
    }

    [Fact]
    public void Nmf_options_hold_the_callers_random_matrix()
    {
        double[] omega = [1.0, 2.0];
        var options = new NmfOptions { RandomMatrix = omega };
        var copy = new NmfOptions { RandomMatrix = [1.0, 2.0] };

        AssertWriteReaches(omega, options.RandomMatrix, () => options.Equals(copy), () => options.GetHashCode());
    }

    [Fact]
    public void K_means_options_hold_the_callers_centres()
    {
        double[] centres = [1.0, 2.0];
        double[] set = [3.0, 4.0];
        var options = new KMeansOptions { InitialCentres = centres };
        var copy = new KMeansOptions { InitialCentres = [1.0, 2.0] };
        var sets = new KMeansOptions { InitialCentreSets = [set] };
        var setsCopy = new KMeansOptions { InitialCentreSets = [[3.0, 4.0]] };

        AssertWriteReaches(centres, options.InitialCentres, () => options.Equals(copy), () => options.GetHashCode());
        AssertWriteReaches(set, sets.InitialCentreSets[0], () => sets.Equals(setsCopy), () => sets.GetHashCode());
    }

    [Fact]
    public void An_anderson_result_holds_its_tables()
    {
        double[] critical = [0.5, 0.6];
        double[] levels = [15.0, 10.0];
        var result = new AndersonResult(1.0, 0.1, critical, levels);
        var copy = new AndersonResult(1.0, 0.1, [0.5, 0.6], [15.0, 10.0]);

        AssertWriteReaches(critical, result.CriticalValues, () => result.Equals(copy), () => result.GetHashCode());
        Assert.Same(levels, result.SignificanceLevels);
    }

    [Fact]
    public void A_contingency_result_holds_its_expected_table()
    {
        double[] row = [1.0, 2.0];
        var result = new ChiSquaredContingencyResult(1.0, 0.5, 1, [row, [3.0, 4.0]]);
        var copy = new ChiSquaredContingencyResult(1.0, 0.5, 1, [[1.0, 2.0], [3.0, 4.0]]);

        AssertWriteReaches(row, result.ExpectedFrequencies[0], () => result.Equals(copy), () => result.GetHashCode());
    }

    [Fact]
    public void A_correlation_matrix_holds_its_matrices()
    {
        double[] statistics = [1.0, 0.5, 0.5, 1.0];
        double[] pValues = [0.0, 0.2, 0.2, 0.0];
        var matrix = new CorrelationMatrix(2, statistics, pValues);
        var copy = new CorrelationMatrix(2, [1.0, 0.5, 0.5, 1.0], [0.0, 0.2, 0.2, 0.0]);

        AssertWriteReaches(statistics, matrix.Statistics, () => matrix.Equals(copy), () => matrix.GetHashCode());
        Assert.Same(pValues, matrix.PValues);
    }

    [Fact]
    public void A_kaplan_meier_curve_holds_its_arrays()
    {
        SurvivalStep[] steps = [new(0.0, 2, 0, 0), new(1.0, 2, 1, 0)];
        double[] survival = [1.0, 0.5];
        double[] lower = [1.0, 0.1];
        double[] upper = [1.0, 0.9];
        var curve = new KaplanMeierCurve(steps, survival, lower, upper, 0.95);
        var copy = new KaplanMeierCurve([.. steps], [1.0, 0.5], [1.0, 0.1], [1.0, 0.9], 0.95);

        AssertWriteReaches(survival, curve.Survival, () => curve.Equals(copy), () => curve.GetHashCode());
        Assert.Same(steps, curve.Steps);
        Assert.Same(lower, curve.Lower);
        Assert.Same(upper, curve.Upper);
    }

    [Fact]
    public void A_nelson_aalen_curve_holds_its_arrays()
    {
        SurvivalStep[] steps = [new(0.0, 2, 0, 0), new(1.0, 2, 1, 0)];
        double[] hazard = [0.0, 0.5];
        var curve = new NelsonAalenCurve(steps, hazard);
        var copy = new NelsonAalenCurve([.. steps], [0.0, 0.5]);

        AssertWriteReaches(hazard, curve.CumulativeHazard, () => curve.Equals(copy), () => curve.GetHashCode());
        Assert.Same(steps, curve.Steps);
    }

    [Fact]
    public void A_survival_curve_holds_its_arrays()
    {
        SurvivalStep[] steps = [new(0.0, 2, 0, 0), new(1.0, 2, 1, 0)];
        double[] survival = [1.0, 0.5];
        double[] lower = [1.0, 0.1];
        double[] upper = [1.0, 0.9];
        var curve = new SurvivalCurve(steps, survival, lower, upper, 0.95);
        var copy = new SurvivalCurve([.. steps], [1.0, 0.5], [1.0, 0.1], [1.0, 0.9], 0.95);

        AssertWriteReaches(survival, curve.Survival, () => curve.Equals(copy), () => curve.GetHashCode());
        Assert.Same(steps, curve.Steps);
        Assert.Same(lower, curve.Lower);
        Assert.Same(upper, curve.Upper);
    }

    /// <summary>The member is the caller's array, and a write through it breaks equality with a copy, not the hash.</summary>
    private static void AssertWriteReaches(double[] given, double[] exposed, Func<bool> equalsCopy, Func<int> hash)
    {
        Assert.Same(given, exposed);
        Assert.True(equalsCopy());
        int before = hash();

        given[0] += 1.0;

        Assert.False(equalsCopy());
        Assert.Equal(before, hash());
    }
}
