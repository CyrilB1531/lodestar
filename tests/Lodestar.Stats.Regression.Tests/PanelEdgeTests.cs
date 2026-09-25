using Lodestar.Stats.Regression.Panel;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>What <see cref="PanelRegression"/> refuses, and what it does with rows in any order (#1156).</summary>
public sealed class PanelEdgeTests
{
    private const int Entities = 8;
    private const int Periods = 5;

    private static readonly int[] EntityLabels = [.. Enumerable.Range(0, Entities * Periods).Select(row => 10 + (7 * (row / Periods)))];
    private static readonly int[] PeriodLabels = [.. Enumerable.Range(0, Entities * Periods).Select(row => 1990 + (row % Periods))];
    private static readonly double[] Regressor = [.. Enumerable.Range(0, Entities * Periods).Select(row => Math.Sin(row * 1.3) + (0.1 * (row / Periods)))];
    private static readonly double[] Response = [.. Enumerable.Range(0, Entities * Periods).Select(row => (2.0 * Regressor[row]) + Math.Cos(row * 0.7) + (row / Periods))];
    private static readonly int[] Groups = [.. EntityLabels.Select(label => label % 3)];

    private static PanelDesign Design() => new(Response, Regressor, 1, EntityLabels, PeriodLabels);

    [Fact]
    public void Options_no_estimator_reads_are_refused()
    {
        Assert.Throws<ArgumentException>(() => PanelRegression.FixedEffects(Design(), new PanelOptions { Bandwidth = 1 }));
        Assert.Throws<ArgumentException>(() => PanelRegression.FixedEffects(Design(), new PanelOptions { Kernel = KernelType.Parzen }));
        Assert.Throws<ArgumentException>(() => PanelRegression.FixedEffects(Design(), new PanelOptions { ClusterEntity = true }));
        Assert.Throws<ArgumentException>(() => PanelRegression.FixedEffects(Design(), Groups, new PanelOptions()));
        Assert.Throws<ArgumentException>(() => PanelRegression.RandomEffects(Design(), new PanelOptions { EntityEffects = true }));
        Assert.Throws<ArgumentException>(() => PanelRegression.Between(Design(), new PanelOptions { TimeEffects = true }));
        Assert.Throws<ArgumentException>(() => PanelRegression.FixedEffects(Design(), new PanelOptions { CovarianceType = (PanelCovarianceType)9 }));
        Assert.Throws<ArgumentException>(() => PanelRegression.FixedEffects(Design(), new PanelOptions { CovarianceType = PanelCovarianceType.Kernel, Kernel = (KernelType)9 }));
        Assert.Throws<ArgumentException>(() => PanelRegression.FixedEffects(Design(), new PanelOptions { CovarianceType = PanelCovarianceType.Kernel, Bandwidth = -1 }));
        Assert.Throws<ArgumentOutOfRangeException>(() => PanelRegression.FixedEffects(Design(), new PanelOptions { ConfidenceLevel = 0.0 }));
        Assert.Throws<ArgumentNullException>(() => PanelRegression.FixedEffects(Design(), Groups, null!));
        Assert.Throws<ArgumentNullException>(() => PanelRegression.FirstDifference(Design(), null!));
    }

    [Fact]
    public void Clusterings_the_reference_does_not_run_are_refused()
    {
        var clustered = new PanelOptions { CovarianceType = PanelCovarianceType.Clustered };
        Assert.Throws<ArgumentException>(() =>
            PanelRegression.FixedEffects(Design(), Groups, clustered with { ClusterEntity = true, ClusterTime = true }));
        Assert.Throws<ArgumentException>(() => PanelRegression.Between(Design(), clustered with { ClusterEntity = true }));
        Assert.Throws<ArgumentException>(() =>
            PanelRegression.Between(Design(), new PanelOptions { CovarianceType = PanelCovarianceType.Kernel }));
        Assert.Throws<ArgumentException>(() =>
            PanelRegression.FirstDifference(Design(), clustered with { WithIntercept = false, ClusterTime = true }));
    }

    [Fact]
    public void Cluster_labels_an_estimator_cannot_read_are_refused()
    {
        var clustered = new PanelOptions { CovarianceType = PanelCovarianceType.Clustered };
        int[] varying = [.. Enumerable.Range(0, Entities * Periods).Select(row => row % 4)];

        Assert.Throws<ArgumentException>(() => PanelRegression.Between(Design(), varying, clustered));
        Assert.Throws<ArgumentException>(() => PanelRegression.FirstDifference(Design(), varying, clustered with { WithIntercept = false }));
    }

    [Fact]
    public void A_first_difference_refuses_a_constant_either_way_it_arrives()
    {
        double[] withConstant = [.. Regressor.SelectMany(value => new[] { 1.0, value })];

        Assert.Throws<ArgumentException>(() => PanelRegression.FirstDifference(Design(), new PanelOptions()));
        Assert.Throws<ArgumentException>(() => PanelRegression.FirstDifference(
            new PanelDesign(Response, withConstant, 2, EntityLabels, PeriodLabels), new PanelOptions { WithIntercept = false }));
    }

    [Fact]
    public void Malformed_panels_are_refused()
    {
        int[] repeated = [.. PeriodLabels];
        repeated[1] = repeated[0];

        Assert.Throws<ArgumentException>(() =>
            PanelRegression.FixedEffects(new PanelDesign(Response, Regressor, 1, EntityLabels, repeated)));
        Assert.Throws<ArgumentException>(() =>
            PanelRegression.FixedEffects(new PanelDesign(Response, Regressor, 1, EntityLabels, PeriodLabels.AsSpan(1))));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            PanelRegression.FixedEffects(new PanelDesign(Response, [], 0, EntityLabels, PeriodLabels)));
        Assert.Throws<ArgumentException>(() =>
            PanelRegression.FixedEffects(Design(), Groups.AsSpan(1), new PanelOptions { CovarianceType = PanelCovarianceType.Clustered }));
    }

    /// <summary>Where the effects or the variance components leave no degree of freedom the reference divides by zero.</summary>
    [Fact]
    public void Fits_with_no_degree_of_freedom_left_are_refused()
    {
        Assert.Throws<ArgumentException>(() => PanelRegression.FixedEffects(
            new PanelDesign([1.0, 2.0, 1.5, 2.2, 0.3], [0.1, 0.5, 0.3, 0.9, 1.2], 1, [1, 1, 2, 3, 4], [1, 2, 1, 1, 1]),
            new PanelOptions { WithIntercept = false, EntityEffects = true }));
        Assert.Throws<ArgumentException>(() => PanelRegression.RandomEffects(
            new PanelDesign([1.0, 2.0, 1.5, 2.2, 0.3], [0.1, 0.5, 0.3, 0.9, 1.2], 1, [1, 2, 3, 4, 5], [1, 1, 1, 1, 1])));
        Assert.Throws<ArgumentException>(() => PanelRegression.FixedEffects(
            new PanelDesign([1.0, 2.0, 1.5, 2.2, 0.3], [0.1, 0.5, 0.3, 0.9, 1.2], 1, [1, 1, 1, 1, 1], [1, 2, 3, 4, 5]),
            new PanelOptions { EntityEffects = true }));
        Assert.Throws<ArgumentException>(() => PanelRegression.RandomEffects(
            new PanelDesign([1.0, 2.0, 1.5, 2.2, 0.3, 1.1], [0.1, 0.5, 0.3, 0.9, 1.2, 0.7], 1, [1, 1, 1, 2, 2, 2], [1, 2, 3, 1, 2, 3])));
    }

    /// <summary>An exact fit's tests are infinite, as the reference's ratios are, rather than a refused inversion.</summary>
    [Theory]
    [InlineData(PanelCovarianceType.Unadjusted)]
    [InlineData(PanelCovarianceType.Robust)]
    public void An_exact_fit_has_infinite_model_tests(PanelCovarianceType covariance)
    {
        double[] regressor = [1.0, 2.0, 3.0, 4.0, 2.0, 3.0, 5.0, 7.0];
        double[] exact = [.. regressor.Select(value => 1.0 + (2.0 * value))];
        int[] entities = [1, 1, 1, 1, 2, 2, 2, 2];
        int[] periods = [1, 2, 3, 4, 1, 2, 3, 4];
        var options = new PanelOptions { CovarianceType = covariance };

        PanelSummary pooled = PanelRegression.FixedEffects(new PanelDesign(exact, regressor, 1, entities, periods), options);
        PanelSummary within = PanelRegression.FixedEffects(
            new PanelDesign(exact, regressor, 1, entities, periods), options with { EntityEffects = true });

        foreach (WaldTest test in new[] { pooled.ModelTest!, pooled.RobustModelTest!, within.PoolabilityTest! })
        {
            Assert.True(double.IsPositiveInfinity(test.Statistic) || test.Statistic > 1e20, $"{test.Statistic}");
            Assert.Equal(0.0, test.PValue, 1e-15);
        }
    }

    [Fact]
    public void A_truncating_kernel_past_the_periods_is_refused()
    {
        var kernel = new PanelOptions { CovarianceType = PanelCovarianceType.Kernel, Bandwidth = Periods };

        Assert.Throws<ArgumentException>(() => PanelRegression.FixedEffects(Design(), kernel));
        Assert.Equal(Periods - 1, PanelRegression.FixedEffects(Design(), kernel with { Bandwidth = Periods - 1 }).Bandwidth);
        Assert.Equal(Periods, PanelRegression.FixedEffects(Design(), kernel with { Kernel = KernelType.QuadraticSpectral }).Bandwidth);
    }

    /// <summary>Driscoll-Kraay's default is <c>⌊4(T/100)^(2/9)⌋</c>: two lags for five periods; no other covariance reports one.</summary>
    [Fact]
    public void The_kernel_bandwidth_is_reported_and_chosen_from_the_periods()
    {
        PanelSummary kernel = PanelRegression.RandomEffects(Design(), new PanelOptions { CovarianceType = PanelCovarianceType.Kernel });
        PanelSummary robust = PanelRegression.RandomEffects(Design(), new PanelOptions { CovarianceType = PanelCovarianceType.Robust });

        Assert.Equal(2, kernel.Bandwidth);
        Assert.Null(robust.Bandwidth);
        Assert.Equal(PanelCovarianceType.Kernel, kernel.CovarianceType);
    }

    /// <summary>The rows are sorted before anything reads them, so their order changes no number, θ's order included.</summary>
    [Fact]
    public void Row_order_changes_nothing()
    {
        int[] order = [.. Enumerable.Range(0, Entities * Periods).OrderBy(row => (row * 17) % 23).ThenBy(row => row)];
        var shuffled = new PanelDesign(
            [.. order.Select(row => Response[row])],
            [.. order.Select(row => Regressor[row])],
            1,
            [.. order.Select(row => EntityLabels[row])],
            [.. order.Select(row => PeriodLabels[row])]);
        var options = new PanelOptions { CovarianceType = PanelCovarianceType.Kernel };

        AssertSame(PanelRegression.RandomEffects(Design(), options), PanelRegression.RandomEffects(shuffled, options));
        AssertSame(
            PanelRegression.FirstDifference(Design(), options with { WithIntercept = false }),
            PanelRegression.FirstDifference(shuffled, options with { WithIntercept = false }));
        AssertSame(
            PanelRegression.FixedEffects(Design(), options with { EntityEffects = true, TimeEffects = true }),
            PanelRegression.FixedEffects(shuffled, options with { EntityEffects = true, TimeEffects = true }));
    }

    /// <summary>A first difference is taken between adjacent periods only; a gap leaves its pair out.</summary>
    [Fact]
    public void A_first_difference_skips_a_gap()
    {
        int[] kept = [.. Enumerable.Range(0, Entities * Periods).Where(row => row != 2)];
        var gapped = new PanelDesign(
            [.. kept.Select(row => Response[row])],
            [.. kept.Select(row => Regressor[row])],
            1,
            [.. kept.Select(row => EntityLabels[row])],
            [.. kept.Select(row => PeriodLabels[row])]);

        PanelSummary full = PanelRegression.FirstDifference(Design(), new PanelOptions { WithIntercept = false });
        PanelSummary gap = PanelRegression.FirstDifference(gapped, new PanelOptions { WithIntercept = false });

        Assert.Equal(Entities * (Periods - 1), full.ObservationCount);
        Assert.Equal((Entities * (Periods - 1)) - 2, gap.ObservationCount);
        Assert.False(gap.HasConstant);
    }

    private static void AssertSame(PanelSummary expected, PanelSummary actual)
    {
        Close(expected.Coefficients, actual.Coefficients);
        Close(expected.StandardErrors, actual.StandardErrors);
        Assert.Equal(expected.RSquaredWithin, actual.RSquaredWithin, 1e-12);
        if (expected.Theta is not null)
        {
            Close(expected.Theta, actual.Theta!);
        }
    }

    private static void Close(IReadOnlyList<double> expected, IReadOnlyList<double> actual)
    {
        Assert.Equal(expected.Count, actual.Count);
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i], actual[i], 1e-12);
        }
    }
}
