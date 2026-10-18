using Lodestar.Stats.Internal;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Degenerate inputs the Review B after #1702 found answered or refused unlike their reference (decision 0010).</summary>
public sealed class DegenerateInputEdgeTests
{
    [Theory]
    [InlineData(new[] { 0.1, 0.1, 0.1 })]
    [InlineData(new[] { 0.1, 0.1, 0.1, 0.1 })]
    [InlineData(new[] { 1.0, 1.0, 1.0 })]
    public void A_constant_sample_is_refused_by_Shapiro_Wilk_even_when_its_mean_rounds(double[] sample)
    {
        // The mean of three 0.1 rounds, the squares summed to 5.8e-34, and the test answered W = 0, p = 0 (#1706): a zero
        // range, which Royston's AS R94 refuses as its sixth fault.
        Assert.Throws<ArgumentException>(() => ShapiroWilk.Test(sample));
        Assert.Throws<ArgumentException>(() => ShapiroWilk.Test([.. sample, double.NaN], NanPolicy.Omit));
    }

    [Theory]
    [InlineData(new[] { 0.1, 0.1, 0.1 + 1.39e-17 }, 0.7500000000000001, 3.3306690738754696e-16)]
    [InlineData(new[] { 1.0, 1.0, 1.0 + 2.220446049250313e-16 }, 0.7500000000000001, 3.3306690738754696e-16)]
    [InlineData(new[] { 1e15 + 1, 1e15 + 2, 1e15 + 4, 1e15 + 8, 1e15 + 3, 1e15 + 1 }, 0.8468270492655245, 0.14834133508019082)]
    [InlineData(new[] { 0.0, 1e-8, 1.0 }, 0.7500000075000002, 1.653986725358436e-08)]
    public void A_near_constant_sample_keeps_its_digits_shifted_as_scipy_shifts_it(double[] sample, double w, double p)
    {
        // Unshifted, the first read W = 0 and the second 0.25, below the 3/4 three values bound W by (#1706's Review A).
        TestResult result = ShapiroWilk.Test(sample);
        Assert.Equal(w, result.Statistic, 1e-12);
        Assert.Equal(p, result.PValue, 1e-12 * p);
    }

    [Theory]
    [InlineData(double.PositiveInfinity, 3)]
    [InlineData(double.NegativeInfinity, 4)]
    public void A_constant_infinite_sample_answers_NaN_as_the_algorithm_leaves_it_undefined(double value, int count)
    {
        TestResult result = ShapiroWilk.Test([.. Enumerable.Repeat(value, count)]);
        Assert.True(double.IsNaN(result.Statistic));
        Assert.True(double.IsNaN(result.PValue));
    }

    [Theory]
    [InlineData(new[] { 1.0, double.PositiveInfinity, double.PositiveInfinity }, Center.Median)]
    [InlineData(new[] { 1.0, 2.0, double.PositiveInfinity }, Center.Mean)]
    [InlineData(new[] { 1.0, 2.0, double.PositiveInfinity, 4.0 }, Center.Trimmed)]
    public void Fligner_answers_NaN_when_a_group_s_centre_is_infinite(double[] group, Center center)
    {
        // inf - inf deviations reached the ranking and the test answered 0.1266, p 0.9386 for the first (#1707).
        TestResult result = Fligner.Test(center, 0.1, NanPolicy.Propagate, group, [1.0, 2.0, 3.0]);
        Assert.True(double.IsNaN(result.Statistic));
        Assert.True(double.IsNaN(result.PValue));
    }

    [Theory]
    [InlineData(new[] { 1e308, 1e308, 5.0 }, Center.Mean, 4.658401634418615, 0.030901945460422606)]
    [InlineData(new[] { 1e308, 1e308 }, Center.Median, 3.557501052526919, 0.059276914397861695)]
    public void Fligner_ranks_the_infinite_deviations_of_a_centre_that_only_overflowed(
        double[] group, Center center, double statistic, double pValue)
    {
        // Every deviation is +inf, not inf - inf: scipy ranks them, and a first fix for #1707 answered NaN.
        TestResult result = Fligner.Test(center, 0.05, NanPolicy.Propagate, group, [1.0, 2.0, 3.0]);
        Assert.Equal(statistic, result.Statistic, 1e-12);
        Assert.Equal(pValue, result.PValue, 1e-12);
    }

    [Fact]
    public void Fligner_still_ranks_an_infinite_value_around_a_finite_centre()
    {
        TestResult result = Fligner.Test(Center.Median, 0.05, NanPolicy.Propagate, [1.0, 2.0, double.PositiveInfinity], [1.0, 2.0, 3.0]);
        Assert.Equal(0.38911604418257967, result.Statistic, 1e-12);
        Assert.Equal(0.532764412704285, result.PValue, 1e-12);
    }

    [Theory]
    [InlineData(new[] { -3.0, -3.0, -2.0 }, -0.25)]
    [InlineData(new[] { -1.0, -2.0 }, -0.3333333333333333)]
    public void Negative_counts_answer_a_NaN_p_value_rather_than_an_internal_refusal(double[] observed, double statistic)
    {
        // RegularizedQ refused the negative statistic under its own "x" (#1709).
        TestResult result = ChiSquared.GoodnessOfFit(observed);
        Assert.Equal(statistic, result.Statistic, 1e-15);
        Assert.True(double.IsNaN(result.PValue));
    }

    [Fact]
    public void A_contingency_table_whose_marginals_overflow_answers_NaN()
    {
        // Every cell finite, the sums infinite: RegularizedQ refused the NaN statistic under "x" (#1710).
        ChiSquaredContingencyResult result = ChiSquared.Contingency([[1e308, 1e308], [1.0, 1.0]]);
        Assert.True(double.IsNaN(result.Statistic));
        Assert.True(double.IsNaN(result.PValue));
    }

    [Fact]
    public void Groups_of_one_value_each_answer_NaN_as_f_oneway_does()
    {
        // No within-group degree of freedom; this threw (#1712).
        TestResult result = OneWayAnova.Test([1.0], [2.0], [3.0]);
        Assert.True(double.IsNaN(result.Statistic));
        Assert.True(double.IsNaN(result.PValue));
    }

    [Theory]
    [InlineData(1e-9, 1e-9)]
    [InlineData(1e-17, 1e-17)]
    [InlineData(-0.3, -0.3095196042031117)]
    [InlineData(0.5, 0.5493061443340549)]
    [InlineData(0.999999, 7.254328619247669)]
    public void The_Fisher_transform_holds_the_function_on_both_target_frameworks(double r, double atanh)
    {
        // ln((1 + r) / (1 - r)) / 2 read 1e-9 8e-8 off and 1e-17 as 0 on netstandard2.0 (#1711); 60-digit sums at the
        // doubles themselves, within the two ulps the netstandard2.0 form holds.
        Assert.Equal(atanh, Fisher.Atanh(r), 4.5e-16 * Math.Abs(atanh));
    }

    [Fact]
    public void Bartlett_and_Friedman_answer_NaN_where_their_statistic_is_undefined()
    {
        // A group of one has no variance, and no block leaves Friedman's Q 0/0: NaN, given or left by omission (decision 0010).
        foreach (TestResult result in new[]
        {
            Bartlett.Test([1.0], [1.0, 2.0, 3.0]),
            Friedman.Test(NanPolicy.Omit, [double.NaN, 1.0], [1.0, double.NaN], [2.0, 3.0]),
            Friedman.Test([], [], []),
        })
        {
            Assert.True(double.IsNaN(result.Statistic) && double.IsNaN(result.PValue));
        }
    }

    [Fact]
    public void Shapiro_Wilk_follows_Royston_s_range_rule_and_scales_by_the_range()
    {
        // AS R94 refuses a range below 1e-19, and divides by the range, so 1e200 values do not overflow their squares.
        Assert.Throws<ArgumentException>(() => ShapiroWilk.Test([0.0, 0.0, 1e-20]));
        TestResult scaled = ShapiroWilk.Test([1e200, 2e200, 3.5e200]);
        TestResult plain = ShapiroWilk.Test([1.0, 2.0, 3.5]);
        Assert.Equal(plain.Statistic, scaled.Statistic, 1e-14);
        Assert.Equal(plain.PValue, scaled.PValue, 1e-12);
    }

    [Fact]
    public void An_undefined_option_is_refused_even_where_an_empty_sample_answers_NaN()
    {
        // The NaN answer came before the option was read, and (Alternative)99 went through (#1712's Review A).
        var bad = (Alternative)99;
        Assert.Throws<ArgumentOutOfRangeException>(() => TTest.Independent([], [1.0, 2.0], bad));
        Assert.Throws<ArgumentOutOfRangeException>(() => TTest.OneSample([], 0.0, bad));
        Assert.Throws<ArgumentOutOfRangeException>(() => TTest.Paired([], [], bad));
        Assert.Throws<ArgumentOutOfRangeException>(() => MannWhitney.Test([], [1.0], bad));
        Assert.Throws<ArgumentOutOfRangeException>(() => KolmogorovSmirnov.TwoSample([], [1.0], bad));
        Assert.Throws<ArgumentOutOfRangeException>(() => Wilcoxon.OneSample([0.0, 0.0], alternative: bad));
    }

    [Fact]
    public void Every_option_outside_its_enumeration_is_refused_whatever_the_samples()
    {
        // (NanPolicy)9 read as Omit in some members and Propagate in others; (Center)9 and a bad Kendall option ran on.
        var policy = (NanPolicy)9;
        Assert.Throws<ArgumentOutOfRangeException>(() => TTest.OneSample([1.0, 2.0], 0.0, nanPolicy: policy));
        Assert.Throws<ArgumentOutOfRangeException>(() => Friedman.Test(policy, [1.0, 2.0], [2.0, 3.0], [3.0, 1.0]));
        Assert.Throws<ArgumentOutOfRangeException>(() => Spearman.Test([1.0, 2.0, 3.0], [1.0, 3.0, 2.0], nanPolicy: policy));
        Assert.Throws<ArgumentOutOfRangeException>(() => Levene.Test((Center)9, 0.05, NanPolicy.Propagate, [1.0, 2.0, 3.0, 4.0], []));
        Assert.Throws<ArgumentOutOfRangeException>(() => Fligner.Test((Center)9, 0.05, NanPolicy.Propagate, [1.0, 2.0, 3.0, 4.0], []));
        Assert.Throws<ArgumentOutOfRangeException>(() => KendallTau.Test([1.0, 2.0, 3.0, 4.0], [2.0, 5.0, 1.0, 7.0], variant: (KendallVariant)9));
        Assert.Throws<ArgumentOutOfRangeException>(() => Spearman.Matrix([1.0, 2.0], 2, (Alternative)99));
    }

    [Fact]
    public void An_undefined_option_is_refused_after_main_s_own_refusals()
    {
        // Main refused these inputs for another reason first, and still does (#1712's Review A).
        ArgumentException raised = Assert.Throws<ArgumentException>(
            () => Spearman.Matrix([1.0, double.NaN, 3.0, 4.0, 5.0, 6.0], 2, (Alternative)99, NanPolicy.Raise));
        Assert.Equal("data", raised.ParamName);
        ArgumentException cell = Assert.Throws<ArgumentException>(() => ChiSquared.Contingency([[1.0, -1.0], [2.0, 3.0]], (Continuity)9));
        Assert.Equal("table", cell.ParamName);
        ArgumentOutOfRangeException trim = Assert.Throws<ArgumentOutOfRangeException>(
            () => Fligner.Test(Center.Trimmed, double.NaN, (NanPolicy)9, [1.0, 2.0, 3.0], [4.0, 5.0, 7.0]));
        Assert.Equal("proportionToCut", trim.ParamName);
        ArgumentException groups = Assert.Throws<ArgumentException>(() => Fligner.Test(Center.Median, 0.05, (NanPolicy)9, [1.0, 2.0, 3.0]));
        Assert.Equal("groups", groups.ParamName);
    }

    [Fact]
    public void An_undefined_Pearson_correlation_has_no_interval()
    {
        // Below two pairs r is NaN (decision 0010), and so is its interval, where the whole range read as one.
        (double low, double high) = Pearson.Test([1.0], [2.0]).ConfidenceInterval();
        Assert.True(double.IsNaN(low) && double.IsNaN(high));
    }

    [Fact]
    public void An_undefined_option_waits_for_every_refusal_its_input_meets_inside_the_computation()
    {
        // Main refused the alternative, the exact limit and the sample size where its computation reached them; the options
        // checked first refused (NanPolicy)99 or (ZeroMethod)99 instead (#1704's Review A).
        Assert.Equal("alternative", Assert.Throws<ArgumentOutOfRangeException>(
            () => TTest.OneSample([1.0, 2.0], 0.0, (Alternative)99, (NanPolicy)99)).ParamName);
        Assert.Equal("alternative", Assert.Throws<ArgumentOutOfRangeException>(
            () => Wilcoxon.OneSample([1.5], (ZeroMethod)99, (Alternative)99)).ParamName);
        double[] distinct = [.. Enumerable.Range(0, 600).Select(i => i + 0.37 * Math.Sin(i * 1.3))];
        Assert.Equal("method", Assert.Throws<ArgumentOutOfRangeException>(
            () => Wilcoxon.OneSample(distinct, alternative: (Alternative)99, method: ExactMethod.Exact)).ParamName);
        Assert.Equal("center", Assert.Throws<ArgumentOutOfRangeException>(
            () => Levene.Test((Center)99, 0.05, (NanPolicy)99, [1.0, 2.0], [3.0, 5.0])).ParamName);
        Assert.Equal("proportionToCut", Assert.Throws<ArgumentOutOfRangeException>(
            () => Fligner.Test(Center.Trimmed, 1.0, (NanPolicy)99, [1.0, 2.0], [3.0, 5.0])).ParamName);
        // An undefined policy omits, as it always did, so the sample omission leaves too short is refused for that.
        Assert.Equal("sample", Assert.Throws<ArgumentException>(
            () => ShapiroWilk.Test([1.0, 2.0, double.NaN], (NanPolicy)7)).ParamName);
    }

    [Fact]
    public void A_rank_test_without_variance_answers_a_NaN_p_value_as_scipy_s_normal_approximation_does()
    {
        // scipy's continuity correction carries the deviation's sign, none at zero, so a zero variance reads 0/0 (#1704's Review A).
        TestResult tied = MannWhitney.Test([1.0], [1.0]);
        Assert.Equal(0.5, tied.Statistic);
        Assert.True(double.IsNaN(tied.PValue));
        Assert.Equal(1.0, MannWhitney.Test([1.0, 1.0, 1.0], [1.0, 1.0], Alternative.Less).PValue);
        Assert.True(double.IsNaN(MannWhitney.Test([1.0, 1.0, 1.0], [1.0, 1.0], continuity: Continuity.None).PValue));

        // Every difference zero: the permutation and exact nulls answer 1, the normal approximation NaN.
        Assert.Equal(new TestResult(0.0, 1.0), Wilcoxon.OneSample([0.0, 0.0, 0.0]));
        Assert.Equal(new TestResult(0.0, 1.0), Wilcoxon.OneSample([0.0, 0.0, 0.0], method: ExactMethod.Exact));
        Assert.True(double.IsNaN(Wilcoxon.OneSample([0.0, 0.0, 0.0], method: ExactMethod.Asymptotic).PValue));
        Assert.True(double.IsNaN(Wilcoxon.OneSample(new double[30]).PValue));
        Assert.True(double.IsNaN(Wilcoxon.OneSample(new double[60]).PValue));
        Assert.True(double.IsNaN(Wilcoxon.OneSample(new double[30], ZeroMethod.Pratt).PValue));
        Assert.True(double.IsNaN(
            Wilcoxon.OneSample(new double[5], ZeroMethod.Pratt, Alternative.Less, Continuity.Applied, ExactMethod.Asymptotic).PValue));
        Assert.True(double.IsNaN(Wilcoxon.Paired([2.0, 2.0, 2.0], [2.0, 2.0, 2.0], method: ExactMethod.Asymptotic).PValue));

        // One value against one rounds the asymptotic law's size to none, which scipy's kstwo answers NaN.
        Assert.True(double.IsNaN(KolmogorovSmirnov.TwoSample([1.5], [1.5], method: ExactMethod.Asymptotic).PValue));
        Assert.Equal(1.0, KolmogorovSmirnov.TwoSample([1.5], [1.5]).PValue);

        // Every Fligner deviation tied leaves the scores no variance, 0/0: scipy's rounding read NaN for the first and 12 for
        // the second, this read 15 for the first (decision 0010).
        TestResult constant = Fligner.Test([5.0, 5.0, 5.0, 5.0], [5.0, 5.0, 5.0, 5.0]);
        Assert.True(double.IsNaN(constant.Statistic) && double.IsNaN(constant.PValue));
        Assert.True(double.IsNaN(Fligner.Test([5.0, 5.0, 5.0, 5.0], [0.0, 0.0, 0.0]).PValue));
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(-0.1)]
    [InlineData(0.5)]
    public void An_empty_group_answers_NaN_whatever_the_trim_as_scipy_s_levene_and_fligner_do(double proportionToCut)
    {
        // A propagated NaN too: Levene took the centres first and refused an out-of-range trim, where Fligner answered NaN.
        Assert.True(double.IsNaN(Levene.Test(Center.Trimmed, proportionToCut, NanPolicy.Propagate, [double.NaN], [1.5, 2.5]).PValue));
        Assert.True(double.IsNaN(Fligner.Test(Center.Trimmed, proportionToCut, NanPolicy.Propagate, [double.NaN], [1.5, 2.5]).PValue));
        Assert.True(double.IsNaN(Levene.Test(Center.Trimmed, proportionToCut, NanPolicy.Propagate, [], [1.5]).PValue));
        Assert.True(double.IsNaN(Fligner.Test(Center.Trimmed, proportionToCut, NanPolicy.Propagate, [], [1.5]).PValue));
    }

    [Fact]
    public void Degenerate_edges_answer_as_their_reference_does()
    {
        // A caller-built record carries no pair count, and keeps the whole range it read before (#1704's Review A).
        Assert.Equal((-1.0, 1.0), new PearsonResult(0.5, 0.01).ConfidenceInterval());

        // Below two pairs a one-sided interval keeps its fixed bound, as a NaN correlation's and a binomial's at no trial do.
        Assert.Equal((double.NaN, 1.0), Pearson.Test([0.0], [0.0], Alternative.Greater).ConfidenceInterval());
        Assert.Equal((-1.0, double.NaN), Pearson.Test([], [], Alternative.Less).ConfidenceInterval());

        // One row or one column leaves no degree of freedom, and the observed table is the expected one: scipy's (0, 1).
        ChiSquaredContingencyResult row = ChiSquared.Contingency([[1.0, 2.0, 3.0]]);
        Assert.Equal((0.0, 1.0, 0), (row.Statistic, row.PValue, row.DegreesOfFreedom));
        Assert.Equal(0, ChiSquared.Contingency([[2.5], [1.5]]).DegreesOfFreedom);
        Assert.True(double.IsNaN(ChiSquared.Contingency([[0.0, 2.0]]).PValue));
        // A zero total answers first: a row of zeros is 0/0 throughout (decision 0010), where scipy's zero test misses it.
        Assert.True(double.IsNaN(ChiSquared.Contingency([[0.0, 0.0]]).PValue));
        Assert.Equal(new TestResult(4.5, 1.0), MannWhitney.Test([1.0, 1.0, 1.0], [1.0, 1.0, 1.0], Alternative.Less));

        // y/f overflowed at a subnormal f and a tiny denominator df, and the density read +inf for scipy's 0.
        Assert.Equal(0.0, Distributions.FisherSnedecorPdf(double.Epsilon, 1e10, 1e-300));

        // trials - count + 1 overflowed past int.MaxValue trials, and an impossible count read p = 1.
        Assert.Equal(0.0, Binomial.Test(0, int.MaxValue, 1.0).PValue);
        Assert.Equal(0.0, Binomial.Test(3, 10, 1.0).PValue);

        // No trial: a one-sided interval keeps the limit it fixes, as scipy's does.
        Assert.Equal(0.0, Binomial.Test(0, 0, 0.5, Alternative.Less).ProportionConfidenceInterval().Low);
        Assert.Equal(1.0, Binomial.Test(0, 0, 0.5, Alternative.Greater).ProportionConfidenceInterval().High);
        Assert.Equal("method", Assert.Throws<ArgumentOutOfRangeException>(
            () => Binomial.Test(0, 0).ProportionConfidenceInterval(0.95, (ProportionInterval)99)).ParamName);
    }

    [Theory]
    [InlineData(5e-324, 3.7, 1e-300, 8.706831748732146e-20)]
    [InlineData(1e-323, 3.7, 1e-300, 1.5694072282839022e-19)]
    [InlineData(5e-324, 1.5, 1e-300, 454561.11786671437)]
    [InlineData(1e-323, 0.3, 1e-300, 1.4960487233718688e19)]
    [InlineData(1e-323, 3.7, 1e-308, 9.902290171660264e-13)]
    public void The_F_density_at_a_subnormal_scaled_f_keeps_its_digits(double f, double dfn, double dfd, double expected)
    {
        // A subnormal d1 f, quantised, carried into y: 3 to 8% off these 80-digit sums, and at d1/d2 past the largest double
        // too, where the first cut divided first (#1725's Review A).
        Assert.Equal(expected, Distributions.FisherSnedecorPdf(f, dfn, dfd), 1e-12 * expected);
    }

    [Fact]
    public void The_F_lower_tail_at_a_subnormal_scaled_f_keeps_its_digits()
    {
        // The tail read the same quantised y, 8% off a 120-digit series (#1725's Review A).
        Assert.Equal(9.855283716302596e-304, Distributions.FisherSnedecorCdf(1e-323, 0.3, 1e-300), 1e-12 * 9.855283716302596e-304);
    }
}
