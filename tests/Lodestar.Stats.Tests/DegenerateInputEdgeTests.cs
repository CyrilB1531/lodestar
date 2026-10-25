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

    [Fact]
    public void The_F_density_at_two_subnormal_dfs_underflows_where_its_last_factor_does()
    {
        // Exactly 3.7e-329 and 7.4e-329, under the smallest double: the last factor of the lifted product, underflowing,
        // was skipped and both read Drop / f, 6.2e-66 (#1729's Review A).
        Assert.Equal(0.0, Distributions.FisherSnedecorPdf(1e5, 1.5e-323, 1.5e-323));
        Assert.Equal(0.0, Distributions.FisherSnedecorPdf(1e5, 0.3, 1.5e-323));
    }

    [Theory]
    [InlineData(1e-300, 3.8613505209794197e-25, 8.425811011254576e-47, 4.2129055056272877e253)]
    [InlineData(0.5, 1e-25, 1e-47, 1e-47)]
    public void The_F_density_at_a_tiny_shape_keeps_its_digits(double f, double dfn, double dfd, double expected)
    {
        // e^(−log B), log B some 100 nats, read these 4.2e-14 and 7.7e-15 off 1000-digit sums (#1729's Review A).
        Assert.Equal(expected, Distributions.FisherSnedecorPdf(f, dfn, dfd), 1e-15 * expected);
    }

    [Fact]
    public void The_F_density_past_a_long_exponent_keeps_its_digits_beside_the_product_shape()
    {
        // Front's exponential of 200 nats read it 4.2e-14 off its 700-digit value, where a df of 0.0019 took products.
        Assert.Equal(4.3930981577872537e-101, Distributions.FisherSnedecorPdf(1e10, 0.002, 30), 1e-15 * 4.3930981577872537e-101);
    }

    [Theory]
    [InlineData(1e-300, 4.46e-308, 3.0, 2.23e-8)]
    [InlineData(1e-310, 4.46e-308, 3.0, 223.00000000000068)]
    public void The_F_density_at_a_shape_beside_the_lift_takes_its_exponent_with_the_power(double f, double dfn, double dfd, double expected)
    {
        // s · value / f formed with s near 2^-1022 went subnormal before the scale: 6.6e-14 and 5.1e-14 off (#1729's Review A).
        Assert.Equal(expected, Distributions.FisherSnedecorPdf(f, dfn, dfd), 1e-15 * expected);
    }

    [Theory]
    [InlineData(1e-310, 1.0, 4.450147717014403e-308, 10.535855610481399)]
    [InlineData(5e-324, 1.0, 4.450147717014403e-308, 47453132.81212577)]
    [InlineData(1.340074e-318, 1.8375865931634674, 5.264668010984388e-308, 6.339902933979412)]
    public void The_F_density_beside_the_lift_threshold_stays_finite(double f, double dfn, double dfd, double expected)
    {
        // Its factors ordered by their logs pushed the product past the largest double: +∞ (#1729's Review A).
        Assert.Equal(expected, Distributions.FisherSnedecorPdf(f, dfn, dfd), 1e-15 * expected);
    }

    [Fact]
    public void An_F_quantile_beside_a_subnormal_complement_comes_back_to_its_tail()
    {
        double root = Distributions.FisherSnedecorQuantile(3.5e-318, 1, 1e-320);

        Assert.Equal(3.5e-318, Distributions.FisherSnedecorCdf(root, 1, 1e-320), 1e-9 * 3.5e-318);
    }

    [Fact]
    public void A_negative_value_scales_by_a_power_of_two_on_both_frameworks()
    {
        // Math.ScaleB scales a negative; the netstandard2.0 steps returned it unscaled (#1729's Review A).
        Assert.Equal(-3.0 / 1024.0, PowerOfTwo.Scale(-3.0, -10));
    }

    [Theory]
    [InlineData(1.002, 0, 1e6, 1.4226030101936747, 2882)]
    [InlineData(0.75, 1, 4818.6, 1.6248493634592563, 2818)]
    public void A_power_whose_mantissa_part_alone_leaves_the_doubles_keeps_its_digits(
        double mantissa, int exponent, double s, double expected, int expectedExponent)
    {
        // m^s by Math.Pow overflowed or underflowed though the whole power, some 2^2880, is a double times 2^k; 2 to the
        // rounded log2 then read it 3e-13 off; squared from m^(s/4) it holds a few ulps (#1729's Review A).
        (double value, int k) = PowerOfTwo.Power(mantissa, exponent, s);
        double scaled = value * Math.Pow(2.0, k - expectedExponent);
        Assert.Equal(expected, scaled, 1e-15 * expected);
    }

    [Theory]
    [InlineData(0.004855995613680737, 7.543351270323165e-12, 0.49999999996918245)]
    [InlineData(401.83943424247514, 7.73728374717664e-12, 0.49999999992463234)]
    public void A_Student_tail_below_the_tiny_shape_takes_the_normalizer_to_its_shape(double t, double df, double expected)
    {
        // Student's cached log(s B) holds 1e-15 only, and read these 12 and 10 ulps off (#1729's Review A).
        Assert.Equal(expected, Distributions.StudentSf(t, df), 3.3e-16);
    }

    [Theory]
    [InlineData(0.11059961137646561, 19.0)]
    [InlineData(0.034145041613094756, 19.0)]
    [InlineData(0.12190963100581795, 39.0)]
    [InlineData(0.11, 99.0)]
    public void A_chi_squared_upper_tail_far_below_its_shape_rounds_to_one(double x, double df)
    {
        // The closed form's sum near one read 0.9999999999999986 and 1.0000000000000007 (#1729's Review A).
        Assert.Equal(1.0, Distributions.ChiSquaredSf(x, df), 3.3e-16);
    }

    [Fact]
    public void A_power_of_two_past_the_doubles_is_zero_or_infinite_and_a_split_of_none_is_NaN()
    {
        // A negative shift is masked to six bits: 2^-1075 read -0, the sign bit (#1729's Review A).
        Assert.Equal(0.0, PowerOfTwo.Of(-1075));
        Assert.Equal(0.0, PowerOfTwo.Of(-1100));
        Assert.Equal(double.PositiveInfinity, PowerOfTwo.Of(1024));
        Assert.True(double.IsNaN(PowerOfTwo.Split(0.0).Mantissa));
        Assert.True(double.IsNaN(PowerOfTwo.Split(double.PositiveInfinity).Mantissa));
    }

    [Fact]
    public void A_scale_into_the_subnormals_rounds_once()
    {
        // 11/8 of the smallest double rounds to one granule; halved a step at a time it rounded 5.5 to 6, then 1.5 to 2,
        // as Kolmogorov's own loop did before it took this helper (#1729's Review A).
        Assert.Equal(double.Epsilon, PowerOfTwo.Scale(1.375, -1074));
    }

    [Fact]
    public void The_F_law_at_a_subnormal_f_beside_a_tiny_df_keeps_its_digits()
    {
        // 1/f past the largest double, and 1/B through e^(−log B) of some 690 nats: the density read +inf and the lower tail
        // 1.6e-14 off 60-digit sums, the factors once ordered nearest one and the tail exponentiated whole (#1729's Review A).
        Assert.Equal(224945689727.15982, Distributions.FisherSnedecorPdf(5e-324, 1.0, 1e-300), 1e-15 * 224945689727.15982);
        Assert.Equal(2.507371360371358e-306, Distributions.FisherSnedecorCdf(5e-324, 0.5, 1e-300), 1e-15 * 2.507371360371358e-306);
    }

    [Fact]
    public void The_F_law_at_a_subnormal_small_half_takes_its_power_from_the_odds_mantissa()
    {
        // e^(s log y), s log y of hundreds of nats, read these 2.7e-14 to 6.6e-14 off 1000-digit sums (#1729's Review A).
        Assert.Equal(2.2250738585072014e-308, Distributions.FisherSnedecorPdf(1.0, 4.450147717014403e-308, 3.0), 1e-15 * 2.2250738585072014e-308);
        Assert.Equal(7.351051938957217e-156, Distributions.FisherSnedecorCdf(1e-310, 1.0, 3.0), 1e-15 * 7.351051938957217e-156);
        Assert.Equal(6.366197723670929e-157, Distributions.FisherSnedecorCdf(1e-312, 1.0, 1.0), 1e-15 * 6.366197723670929e-157);
        Assert.Equal(1.046073029638586e-154, Distributions.FisherSnedecorPdf(1e-309, 3.0, 1.0), 1e-15 * 1.046073029638586e-154);
    }

    [Fact]
    public void An_F_quantile_whose_target_the_flat_tail_meets_at_the_smallest_double_is_a_root()
    {
        // At two tiny dfs the lower tail is flat to its last ulp and falls by one; read strictly, the end check answered +∞
        // for a target it met at the smallest double (#1729's Review A).
        double p = Distributions.FisherSnedecorCdf(1.44e-190, 1e-300, 3.395841653944e-312);
        double root = Distributions.FisherSnedecorQuantile(p, 1e-300, 3.395841653944e-312);
        Assert.True(root == 0.0 || Math.Abs(Distributions.FisherSnedecorCdf(root, 1e-300, 3.395841653944e-312) - p) <= 1e-9 * p, $"root {root}");

        // Past the plateau the tail never reaches the target, so the root lies past every double.
        Assert.Equal(double.PositiveInfinity, Distributions.FisherSnedecorQuantile(3.395841654271607e-12, 1e-300, 3.395841653944e-312));
    }

    [Fact]
    public void The_F_law_at_a_huge_df_beside_thousands_reads_zero_not_NaN()
    {
        // The power's mantissa overflowed to +∞ beside a factor already 0, and inf · 0 read NaN (#1729's Review A).
        Assert.Equal(0.0, Distributions.FisherSnedecorSf(1e40, 1e300, 5000.0));
        Assert.Equal(0.0, Distributions.FisherSnedecorPdf(1e40, 1e300, 5000.0));
        Assert.Equal(0.0, Distributions.FisherSnedecorCdf(1e-40, 5000.0, 1e300));
    }

    [Fact]
    public void A_chi_squared_tail_at_a_df_of_zero_is_refused_not_lifted()
    {
        // Lifted, zero stays zero and the lift recursed without end (#1729's Review A).
        Assert.Throws<ArgumentOutOfRangeException>(() => Gamma.ChiSquaredTail(0.0, 1.0, upper: true));
    }

    [Fact]
    public void The_lifted_chi_squared_tails_at_zero_are_one_and_zero()
    {
        // A df of 5e-324 halves to zero, and at x = 0 the tail took that half and the shape refused it (#1729's Review A).
        Assert.Equal(1.0, Gamma.ChiSquaredTail(5e-324, 0.0, upper: true));
        Assert.Equal(0.0, Gamma.ChiSquaredTail(5e-324, 0.0, upper: false));
        var lead = new Gamma.UnhalvedLead(1.5e-323);
        Assert.Equal(1.0 - Gamma.ChiSquaredTailBetween(lead, 0.0, 5e-324, upper: true), Gamma.ChiSquaredTailBetween(lead, 0.0, 5e-324, upper: false));
    }

    [Theory]
    [InlineData(3.846241e-317, 221477.18672150362)]
    [InlineData(1e-300, 5.383368601372556e21)]
    public void The_F_density_where_the_scaled_f_underflows_takes_y_from_the_odds(double dfd, double expected)
    {
        // d1 f, 3e-325, underflowed to zero and y read 0 for 7.8e-9: 1.4e-11 and 2.2e-13 off 60-digit values (#1729).
        Assert.Equal(expected, Distributions.FisherSnedecorPdf(8.4e-323, 0.003562409783441407, dfd), 1e-15 * expected);
    }

    [Theory]
    [InlineData(1e-10, 0.49999999942435375)]
    [InlineData(0.5, 0.4999999999826713)]
    public void The_F_lower_tail_at_two_equal_tiny_dfs_is_the_nearest_double(double f, double expected)
    {
        // One less the other side's series, near a half here, read these 2.5 ulps off 1000-digit sums (#1729's Review A).
        Assert.Equal(expected, Distributions.FisherSnedecorCdf(f, 1e-10, 1e-10));
    }

    [Theory]
    [InlineData(1e-318, 20, 1e-320)]
    [InlineData(1.05e-321, 44.5618638639256, 1e-322)]
    [InlineData(5.81995e-319, 19.84684195238273, 4.3023e-320)]
    public void The_F_upper_tail_at_a_subnormal_denominator_df_keeps_its_digits(double f, double dfn, double dfd)
    {
        // The fraction's first term quantised so small a d2/2: the upper tail read 0.99994, 0.954 and 0.99992 where the
        // exact one rounds to one, which this holds to two ulps (#1729).
        Assert.Equal(1.0, Distributions.FisherSnedecorSf(f, dfn, dfd), 3.3e-16);
    }

    [Fact]
    public void A_df_whose_half_is_not_a_double_enters_at_its_exact_half()
    {
        // 1.5e-323 is three of the smallest doubles, and its half rounds to two: each of these is proportional to that half
        // and read a third off 1000-digit sums, or for chi-squared its exact a e^(-x/2) / x at so small an a (#1729's Review A).
        Assert.Equal(1.4821969375235244e-13, Distributions.FisherSnedecorCdf(1e-5, 1e-310, 1.5e-323), 1e-12 * 1.4821969375235244e-13);
        Assert.Equal(7.410984687618699e-24, Distributions.FisherSnedecorPdf(1e-300, 1.5e-323, 3.7), 1e-12 * 7.410984687618699e-24);
        Assert.Equal(7.410984687618699e-24, Distributions.ChiSquaredPdf(1e-300, 1.5e-323), 1e-12 * 7.410984687618699e-24);

        // That lower tail is 1.48e-13 at every f, so a target above it has its root at the end of the support.
        Assert.Equal(double.PositiveInfinity, Distributions.FisherSnedecorQuantile(1.6e-13, 1e-310, 1.5e-323));
        Assert.Equal(double.PositiveInfinity, Distributions.FisherSnedecorIsf(0.99999999999985, 1e-310, 1.5e-323));
        Assert.Equal(double.PositiveInfinity, Distributions.FisherSnedecorQuantile(0.8, 1e-310, 1.5e-323));

        // At a denominator df of 1e-322 the lower tail stays of that order at every f, under 1e-300, so the root is +∞,
        // and 1 - p rounding to one there must not read as the upper tail's root at f = 0.
        Assert.Equal(double.PositiveInfinity, Distributions.FisherSnedecorQuantile(1e-300, 0.3, 1e-322));

        // A numerator df that small makes the lower tail one less a·G at every f, so a target of 1e-3 has its root at 0.
        Assert.Equal(0.0, Distributions.FisherSnedecorQuantile(1e-3, 1.5e-323, 3.7));

        // Both shapes that small, the upper tail is a/(a + b), 1.5/4 in units of the smallest double.
        Assert.Equal(0.375, Distributions.FisherSnedecorSf(1.0, 1.5e-323, 2.5e-323), 1e-15);

        // The chi-squared upper tail is a E1(x/2), a subnormal read to its own granularity, and its inverse returns it.
        Assert.Equal(5.52e-321, Distributions.ChiSquaredSf(5e-324, 1.5e-323), 5e-324);
        Assert.Equal(1e-321, Distributions.ChiSquaredSf(Distributions.ChiSquaredIsf(1e-321, 1.5e-323), 1.5e-323), 1e-323);
        // The root is judged against p itself, not the rounded half's target: it read 9.04e-321 back, 12 granules off.
        Assert.Equal(9.1e-321, Distributions.ChiSquaredSf(Distributions.ChiSquaredIsf(9.1e-321, 2.5e-323), 2.5e-323));
    }

    [Theory]
    [InlineData(2e20, 1e-20, 2000.0, 1.0987596822392125e-21)]
    [InlineData(1e3, 1e-20, 2000.0, 1.9630189089945256e-19)]
    public void A_shape_below_the_ulp_of_one_keeps_the_large_shape_expansion_terms(double f, double dfn, double dfd, double tail)
    {
        // 1000-digit sums: b + 1 rounded to one and every term past the first read zero, the first 1.4e-7 off.
        Assert.Equal(tail, Distributions.FisherSnedecorSf(f, dfn, dfd), 1e-14 * tail);
    }

    [Theory]
    [InlineData(2.0, 1e-12, 0.4999999999923991)]
    [InlineData(0.5, 1e-9, 0.49999999481918356)]
    [InlineData(1000.0, 1e-9, 0.4999999910187324)]
    public void A_student_tail_below_the_tiny_shape_is_one_less_the_other_series(double t, double df, double tail)
    {
        // A half-df under 1e-8 takes 1 - SmallShapeLowerTail(1/2, df/2, ...) with LogScaledBeta's normalizer; 1000-digit
        // sums, within three ulps of one half.
        Assert.Equal(tail, Distributions.StudentSf(t, df), 3.3e-16);
    }

    [Fact]
    public void A_half_integer_chi_squared_shape_takes_one_gamma_on_both_sides_of_the_power_switch()
    {
        // The two doubles straddle a log x = -4 at five degrees of freedom; 60-digit series, each within two ulps, and
        // the lower x no higher: Lanczos' log Γ on one side stepped the tail down 10 ulps as x rose.
        double below = Distributions.ChiSquaredCdf(0.4037930359893107, 5);
        double above = Distributions.ChiSquaredCdf(0.40379303598931077, 5);
        Assert.Equal(0.004775524675233816, below, 2e-18);
        Assert.Equal(0.004775524675233817, above, 2e-18);
        Assert.True(below <= above);
    }

    [Fact]
    public void A_tiny_probability_keeps_the_binomial_p_value_digits()
    {
        // 1 − (1 − 1e-300)^1000 is 1e-297 to 5e-298 of itself; e^(a log x) read it 4.9e-15 off.
        Assert.Equal(1e-297, Binomial.Test(1, 1000, 1e-300).PValue, 1e-310);
    }

    [Fact]
    public void A_tiny_shape_tail_takes_one_less_the_other_side_only_when_it_is_past_one_half()
    {
        // I_x(a, b) is past one half with b at least a, and near b/(a + b) below it; 1000-digit reference, 13 digits held.
        Assert.Equal(1.0, Distributions.FisherSnedecorSf(1e-300, 3.7, 1e-300), 3.3e-16);
        Assert.Equal(9.999999988486215e-291, Distributions.FisherSnedecorSf(1e300, 1e-300, 1e-10), 1e-303);
    }

    [Theory]
    [InlineData(0.41, 5, 3)]
    [InlineData(1e-3, 5, 18)]
    [InlineData(2.0, 3, 0)]
    public void A_subnormal_df_tail_rounds_once_to_the_nearest_double(double x, int dfGranules, int tailGranules)
    {
        // Q is (df/2) E1(x/2) at so small a df, by a 40-digit series: 3.006, 17.56 and 0.329 granules. A factor on the
        // rounded half's tail rounded twice, the first 2 granules for 3 (#1729's Review A).
        Assert.Equal(tailGranules * double.Epsilon, Distributions.ChiSquaredSf(x, dfGranules * double.Epsilon));
    }

    [Fact]
    public void A_subnormal_chi_squared_statistic_is_not_halved_before_its_tails()
    {
        // e^(-x/2)/2 at two degrees of freedom; Lanczos' log Γ(1) read it 4e-16 high.
        Assert.Equal(0.5, Distributions.ChiSquaredPdf(1e-300, 2));
        // The root is the double nearest it, decided at the midpoint: 4e-324 rounds up, 2.3e-324 down to zero.
        Assert.Equal(5e-324, Distributions.ChiSquaredQuantile(1.6e-162, 1));
        Assert.Equal(0.0, Distributions.ChiSquaredQuantile(1.2e-162, 1));
        // x/2 rounded 1.5e-323 up a third, and P(1/2, x/2) = sqrt(x/2) / Γ(3/2) read 15% high; 5e-324 halved to zero, whose
        // lower tail read 0 and upper 1 at a shape of 1e-300, for one less 7.4e-298 and 7.4e-298 (#1729's Review A).
        Assert.Equal(3.0718005745332645e-162, Distributions.ChiSquaredCdf(1.5e-323, 1.0), 1e-13 * 3.0718005745332645e-162);
        Assert.Equal(7.445560034370397e-298, Distributions.ChiSquaredSf(5e-324, 2e-300), 1e-13 * 7.445560034370397e-298);
        Assert.Equal(1.0, Distributions.ChiSquaredCdf(5e-324, 2e-300));

        // The inversion solves for x/2 and so answered only even multiples of the smallest double there.
        Assert.Equal(1.5e-323, Distributions.ChiSquaredQuantile(3.0718005745332645e-162, 1.0));
    }
}
