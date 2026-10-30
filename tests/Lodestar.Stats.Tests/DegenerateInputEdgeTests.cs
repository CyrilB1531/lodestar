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
    public void An_F_quantile_on_a_flat_tail_is_an_end_unless_its_target_is_the_tail()
    {
        // The lower tail is b/(a + b) at every f; a target an ulp below, met at ε, read 1.0000007 (#1743). Before rounding
        // it lies above its rounding at every f, so ε's tail is past that and the quantile is 0 (#1745's Review A).
        double p = Distributions.FisherSnedecorCdf(1.44e-190, 1e-300, 3.395841653944e-312);
        Assert.Equal(p, Distributions.FisherSnedecorCdf(double.Epsilon, 1e-300, 3.395841653944e-312));
        Assert.Equal(p, Distributions.FisherSnedecorCdf(double.MaxValue, 1e-300, 3.395841653944e-312));
        Assert.True(ExactQuantileContract.FisherLowerReaches(double.Epsilon, 1e-300, 3.395841653944e-312, p));
        Assert.Equal(0.0, Distributions.FisherSnedecorQuantile(p, 1e-300, 3.395841653944e-312));
        Assert.Equal(0.0, Distributions.FisherSnedecorQuantile(3.3958416539320224e-12, 1e-300, 3.395841653944e-312));

        // Above the tail the target is never met, so the root lies past every double.
        Assert.Equal(double.PositiveInfinity, Distributions.FisherSnedecorQuantile(3.395841653932023e-12, 1e-300, 3.395841653944e-312));
        Assert.Equal(double.PositiveInfinity, Distributions.FisherSnedecorQuantile(3.395841654271607e-12, 1e-300, 3.395841653944e-312));

        // Past the first order's reach the inversion's end check reads a flat tail met at ε as 0, where it read +∞ (#1729's
        // Review A).
        double flat = Distributions.FisherSnedecorCdf(double.Epsilon, 1e-20, 1.1e-11);
        Assert.Equal(flat, Distributions.FisherSnedecorCdf(double.MaxValue, 1e-20, 1.1e-11));
        Assert.Equal(0.0, Distributions.FisherSnedecorQuantile(flat, 1e-20, 1.1e-11));

        // Judged on the caller's own side past one half, its plateau before rounding is below its rounding at every f, so no
        // double reaches it (#1745's Review A); the largest, whose tail rounds to it, answers (#1746).
        double large = Distributions.FisherSnedecorCdf(1.0, 3.395841653944e-312, 1e-300);
        Assert.Equal(double.MaxValue, Distributions.FisherSnedecorQuantile(large, 3.395841653944e-312, 1e-300));
        double upper = Distributions.FisherSnedecorSf(1.0, 1.9e-23, 1.9e-26);
        double upperRoot = Distributions.FisherSnedecorIsf(upper, 1.9e-23, 1.9e-26);
        Assert.Equal(ExactQuantileContract.FisherUpperReaches(double.MaxValue, 1.9e-23, 1.9e-26, upper), upperRoot <= double.MaxValue);
        Assert.True(double.IsPositiveInfinity(upperRoot) || ExactQuantileContract.FisherUpperReaches(Math.Max(upperRoot, double.Epsilon), 1.9e-23, 1.9e-26, upper));
        // One less a target an ulp below the tail rounds to one less the tail itself: only the caller's side parts them.
        Assert.Equal(0.15254237288135594, Distributions.FisherSnedecorCdf(1.0, 1e-300, 1.8e-301));
        Assert.Equal(0.0, Distributions.FisherSnedecorQuantile(0.15254237288135591, 1e-300, 1.8e-301));

        // One df below 2^-1021 alone: the flat share, not the lifted tail scaled back.
        foreach (double f in new[] { double.Epsilon, 1.0, double.MaxValue })
        {
            Assert.Equal(9.999999999999969e-286, Distributions.FisherSnedecorSf(f, 1e-310, 1e-25));
        }
        Assert.Equal(0.0, Distributions.FisherSnedecorIsf(1e-285, 1e-310, 1e-25));
    }

    [Fact]
    public void The_F_law_at_shapes_below_the_first_orders_reach_is_monotone_and_inverts_to_its_tail()
    {
        // The fraction's two sides fell 3 ulps apart across y = ½, and main's tail here was 3 ulps off the 400-digit series'
        // 3.671581579456564e-268 (#1743).
        Assert.True(Distributions.FisherSnedecorCdf(0.138, 9.829125705138842e-20, 1.6085286591331236e-20)
            <= Distributions.FisherSnedecorCdf(0.3236, 9.829125705138842e-20, 1.6085286591331236e-20));
        Assert.Equal(3.671581579456564e-268, Distributions.FisherSnedecorCdf(1.9826540529802225e-268, 1.2061636154805542e-10, 4.428528112573702e-278));

        // The inversion stopped within RootTolerance anywhere on a tail moving by a few ulps: the median of equal dfs read
        // 0.99956, below the 0.99978 an ulp lower read, and a target the tail meets at ε read +∞.
        Assert.Equal(0.5, Distributions.FisherSnedecorCdf(1.0, 1e-12, 1e-12));
        Assert.Equal(1.0, Distributions.FisherSnedecorQuantile(0.5, 1e-12, 1e-12));
        Assert.True(Distributions.FisherSnedecorQuantile(0.49999999999999994, 1e-12, 1e-12) <= 1.0);
        Assert.True(Distributions.FisherSnedecorQuantile(0.50000000000000011, 1e-12, 1e-12) > 1.0);
        // The smallest double whose tail before rounding reaches the target; the rounded tail at 1 sat above its own.
        double met = Distributions.FisherSnedecorCdf(1.0, 1.9e-32, 1.9e-17);
        double metRoot = Distributions.FisherSnedecorQuantile(met, 1.9e-32, 1.9e-17);
        // No double reaches it before rounding; the largest, whose tail rounds to it, answers (Cyril, 2026-10-29, #1746).
        Assert.Equal(double.MaxValue, metRoot);
        Assert.False(metRoot > 0.0 && ExactQuantileContract.FisherLowerReaches(Math.BitDecrement(metRoot), 1.9e-32, 1.9e-17, met));
        double upper = Distributions.FisherSnedecorSf(1.0, 1.9e-17, 1.9e-29);
        double upperRoot = Distributions.FisherSnedecorIsf(upper, 1.9e-17, 1.9e-29);
        Assert.True(double.IsPositiveInfinity(upperRoot) || ExactQuantileContract.FisherUpperReaches(Math.Max(upperRoot, double.Epsilon), 1.9e-17, 1.9e-29, upper));
        Assert.False(upperRoot > 0.0 && ExactQuantileContract.FisherUpperReaches(Math.BitDecrement(upperRoot), 1.9e-17, 1.9e-29, upper));

        // A subnormal df's rounded half moved the guess's share by far more than the slope, and the inversion it fell back to
        // answered a double whose tail fell short (#1743's Review A).
        double lone = Distributions.FisherSnedecorCdf(7.242828225307288e122, 1.0390523104750727e-11, 1.1403317e-317);
        double loneRoot = Distributions.FisherSnedecorQuantile(lone, 1.0390523104750727e-11, 1.1403317e-317);
        Assert.Equal(lone, Distributions.FisherSnedecorCdf(loneRoot, 1.0390523104750727e-11, 1.1403317e-317));
        Assert.True(Distributions.FisherSnedecorCdf(BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(loneRoot) - 1), 1.0390523104750727e-11, 1.1403317e-317) < lone);

        // Searched from the first order's root, the search's ends went untested: a target ε's tail is past read ε, not 0,
        // and one past the largest double's MaxValue, not +∞.
        double atSmallest = Distributions.FisherSnedecorCdf(double.Epsilon, 1e-12, 1e-12);
        Assert.Equal(0.0, Distributions.FisherSnedecorQuantile(BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(atSmallest) - 1), 1e-12, 1e-12));
        double atLargest = Distributions.FisherSnedecorCdf(double.MaxValue, 1e-12, 1e-12);
        Assert.Equal(double.PositiveInfinity, Distributions.FisherSnedecorQuantile(BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(atLargest) + 1), 1e-12, 1e-12));
    }

    [Theory]
    [InlineData(0.01, 0.01)]
    [InlineData(1.06997e-5, 0.214987)]
    [InlineData(0.5, 0.5)]
    public void An_F_inverse_upper_tail_whose_target_is_the_tail_at_the_largest_double_is_a_double(double numeratorDf, double denominatorDf)
    {
        // The inversion overflowed past the largest double, whose upper tail is the target, and read +∞ (#1746).
        double target = Distributions.FisherSnedecorSf(double.MaxValue, numeratorDf, denominatorDf);
        double root = Distributions.FisherSnedecorIsf(target, numeratorDf, denominatorDf);
        // The largest double too where its tail before rounding lies above its rounding, as at dfs (0.5, 0.5) (Cyril,
        // 2026-10-29).
        Assert.Equal(target, Distributions.FisherSnedecorSf(root, numeratorDf, denominatorDf));

        // An ulp above it, the root's tail is the target, or, where no double's is, the root is the first reaching it.
        double above = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(target) + 1);
        double next = Distributions.FisherSnedecorIsf(above, numeratorDf, denominatorDf);
        double atNext = Distributions.FisherSnedecorSf(next, numeratorDf, denominatorDf);
        double before = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(next) - 1);
        Assert.True(atNext <= above, $"root {next}");
        Assert.True(atNext >= above || Distributions.FisherSnedecorSf(before, numeratorDf, denominatorDf) > above, $"root {next}");
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
    [InlineData(1e300, 1.9e5, 2e304, 1.0)]
    [InlineData(1e300, 1e5, 1e305, 1.0)]
    [InlineData(5e-324, 1.7e308, 1.0, 0.0)]
    [InlineData(1.7e308, 10.0, 1.7e308, 1.0)]
    [InlineData(1.7e308, 1000.0, 1.7e308, 1.0)]
    public void An_overflowing_shape_product_leaves_the_f_tails_at_their_end(double f, double dfn, double dfd, double lower)
    {
        // a b past the largest double, and a shape times a logarithm, overflowed in double-double, whose ∞ − ∞ error read
        // NaN where main's doubles flowed through exp to 0 or 1 (#1745's Review A).
        Assert.Equal(lower, Distributions.FisherSnedecorCdf(f, dfn, dfd));
        Assert.Equal(1.0 - lower, Distributions.FisherSnedecorSf(f, dfn, dfd));
    }

    [Fact]
    public void A_df_whose_half_rounds_to_zero_keeps_the_inversion_root_at_a_subnormal_target()
    {
        // The Newton step's density took log B(0, b), which threw; a subnormal target keeps the inversion's root, inside a
        // run of doubles at the target whose start the search took milliseconds to reach (#1745's Review A).
        Assert.Equal(9.8663389555306772e147, Distributions.FisherSnedecorIsf(1e-321, double.Epsilon, 5));
        Assert.Equal(1.0135471774354963e-148, Distributions.FisherSnedecorQuantile(1e-321, 5, double.Epsilon));
    }

    [Fact]
    public void A_subnormal_target_searches_past_a_root_whose_tail_falls_short()
    {
        // The inversion's root, one, has an upper tail of one half; the next double's is zero.
        Assert.Equal(1.0000000000000002, Distributions.FisherSnedecorIsf(5e-324, 1e300, 1e300));
    }

    [Theory]
    [InlineData(3e-307, 0.3, 1e-307)]
    [InlineData(2.7e-308, 3.7, 1e-307)]
    [InlineData(1.3e-308, 3.7, 4.5e-308)]
    public void The_F_tail_rises_near_the_smallest_normal_doubles(double from, double dfn, double dfd)
    {
        // The product's residual rounded to the subnormals there, and b times the front fell below them: over 20,000 ulps
        // the tail fell 552 to 2,142 times (#1745's Review A).
        double f = from;
        double previous = Distributions.FisherSnedecorCdf(f, dfn, dfd);
        for (int i = 0; i < 4000; i++)
        {
            f = Math.BitIncrement(f);
            double tail = Distributions.FisherSnedecorCdf(f, dfn, dfd);
            Assert.True(tail >= previous, $"fell at {f:R}");
            previous = tail;
        }
    }

    [Theory]
    [InlineData(1.0, 0.5, 0.698878389158678)]
    [InlineData(1.0, 3.7, 0.8109293001197738)]
    [InlineData(0.5, 2.0, 0.6666666666666666)]
    public void Student_larger_tail_is_rounded_once(double t, double df, double lower)
    {
        // One less the smaller tail before its rounding, against a 60-digit sum; rounded twice it read up to 4 ulps off.
        Assert.Equal(lower, Distributions.StudentCdf(t, df));
        Assert.Equal(lower, Distributions.StudentSf(-t, df));
    }

    [Fact]
    public void Past_a_harmonic_shape_of_1000_the_F_quantile_keeps_the_inversion_root()
    {
        // The double fraction steps some 1e4 ulps between runs of f there; the search for the run took 59 us, scipy's 29.
        Assert.Equal(1.0000793419837652, Distributions.FisherSnedecorQuantile(0.5094683656978739, 165557.4344487868, 8597663.01685013));
    }

    [Theory]
    [InlineData(8.38769461692256, 52.34046127357577, 446.97820450138437, 2.2608091918231524e-39)]
    [InlineData(0.6818319510262985, 329.278608780484, 743.0123651982178, 0.002246340662753896)]
    public void The_F_density_takes_its_front_from_the_exact_split(double f, double dfn, double dfd, double density)
    {
        // A 50-digit density at the doubles themselves; from the rounded split the front read 442 and 424 ulps off,
        // main 110 and 265 (#1745's Review A).
        Assert.Equal(density, Distributions.FisherSnedecorPdf(f, dfn, dfd), 2e-16 * density);
    }

    [Theory]
    [InlineData(0.5420006953841079, 0.019859971225212675)]
    [InlineData(0.4210632003010545, 0.053)]
    [InlineData(0.975, 1999.0)]
    public void A_student_root_far_from_its_crossing_is_settled_by_further_newton_steps(double p, double df)
    {
        // 129 and 196 ulps off at the small dfs, past the window, and at df 1999 past one step's reach: the root was kept
        // with a tail short of p, or 14 exact tails searched (#1745's Review A). Judged on the smaller tail.
        double t = Distributions.StudentQuantile(p, df);
        Func<double, bool> reaches = x => p <= 0.5
            ? ExactQuantileContract.StudentLowerReaches(x, df, p)
            : ExactQuantileContract.StudentUpperReaches(x, df, 1.0 - p);
        Assert.True(reaches(t));
        Assert.False(reaches(Math.BitDecrement(t)));
    }

    [Fact]
    public void An_f_root_past_the_window_is_settled_by_further_newton_steps()
    {
        // 142 ulps off at dfs (0.0649, 0.240), whose root was kept with a tail short of p (#1745's Review A).
        const double P = 0.6456600677434635;
        double f = Distributions.FisherSnedecorQuantile(P, 0.0649, 0.240);
        Assert.True(ExactQuantileContract.FisherUpperReaches(f, 0.0649, 0.240, 1.0 - P));
        Assert.False(ExactQuantileContract.FisherUpperReaches(Math.BitDecrement(f), 0.0649, 0.240, 1.0 - P));
    }

    [Theory]
    [InlineData(0.4998268366311668, 0.001, 0.001)]
    [InlineData(0.2945541156514044, 0.001, 20.0)]
    [InlineData(6.828857148687927e-78, 400.0, 0.5)]
    [InlineData(6.832047447591412e-78, 1e5, 0.5)]
    [InlineData(1e-10, 1e-12, 1e6)]
    [InlineData(0.5, 0.05, 0.3)]
    public void An_f_inverse_tail_in_a_wide_run_or_near_the_largest_double_is_the_smallest_reaching(double p, double dfn, double dfd)
    {
        // Runs of doubles sharing the tail wider than the window, and a density underflowing near the largest double, kept
        // roots whose tail was past p, 1.9999999999996552 at the first (#1745's Review A).
        double f = Distributions.FisherSnedecorIsf(p, dfn, dfd);
        Assert.True(ExactQuantileContract.FisherUpperReaches(f, dfn, dfd, p));
        Assert.False(ExactQuantileContract.FisherUpperReaches(Math.BitDecrement(f), dfn, dfd, p));
    }

    [Theory]
    [InlineData(0.4996010777327141, 5000.0)]
    [InlineData(0.4996, 0.7)]
    [InlineData(0.49999999, 2000.0)]
    public void A_student_inverse_tail_in_a_wide_run_or_near_the_largest_double_is_the_smallest_reaching(double p, double df)
    {
        // As the F's (#1745's Review A).
        double t = Distributions.StudentIsf(p, df);
        Assert.True(ExactQuantileContract.StudentUpperReaches(t, df, p));
        Assert.False(ExactQuantileContract.StudentUpperReaches(Math.BitDecrement(t), df, p));
    }

    [Theory]
    [InlineData(0.3, 1e300, 7.41691286169067e-08)]
    [InlineData(30.0, 1e307, 0.007416912861689144)]
    [InlineData(1.0, 1e307, 0.05562684646268078)]
    public void The_F_chi_squared_limit_at_a_subnormal_half_rises_with_f(double dfn, double dfd, double f)
    {
        // As a double it fell up to 34 ulps between neighbouring f, and 2 where it met the 106-bit forms (#1746's Review A).
        for (int i = 0; i < 64; i++)
        {
            double next = Math.BitIncrement(f);
            Assert.True(Distributions.FisherSnedecorCdf(next, dfn, dfd) >= Distributions.FisherSnedecorCdf(f, dfn, dfd), $"f {f}");
            f = next;
        }
    }

    [Fact]
    public void An_F_inverse_tail_of_the_largest_doubles_tail_past_one_half_is_that_double()
    {
        // Judged on the smaller tail alone, one less a target past one half missed the rounded lower tail, +∞ (Cyril,
        // 2026-10-29, #1746's Review A).
        const double a = 0.49610036947034547, b = 0.00038124907557019193;
        Assert.Equal(double.MaxValue, Distributions.FisherSnedecorIsf(Distributions.FisherSnedecorSf(double.MaxValue, a, b), a, b));
    }

    [Fact]
    public void The_F_law_below_the_first_orders_shapes_takes_one_form_on_both_sides_and_its_tie_on_the_callers()
    {
        // The own side alone in 106 bits jumped at ProductFloor, and the upper tail rose an ulp (#1746's Review A).
        const double d = 1.2564049194572966e-11;
        Assert.True(Distributions.FisherSnedecorSf(4.989600773836801e291, d, d) <= Distributions.FisherSnedecorSf(4.9896007738368e291, d, d));
        Assert.True(Distributions.FisherSnedecorCdf(2.004168360008973e-292, d, d) >= Distributions.FisherSnedecorCdf(2.0041683600089726e-292, d, d));

        // At one half the end judged the side the inversion does not keep: Isf(Sf(MaxValue)) read +∞, and the quantile
        // MaxValue though the lower tail there is short of one half.
        Assert.Equal(0.5, Distributions.FisherSnedecorSf(double.MaxValue, 1e-6, 9.992908456791932e-7));
        Assert.Equal(double.MaxValue, Distributions.FisherSnedecorIsf(0.5, 1e-6, 9.992908456791932e-7));
        Assert.Equal(double.PositiveInfinity, Distributions.FisherSnedecorQuantile(0.5, 1e-6, 9.992908456791932e-7));
    }

    [Fact]
    public void Log_beta_at_a_ratio_of_shapes_below_1e_minus_16_keeps_its_106_bits()
    {
        // log(1 + ratio) read on 1 + ratio lost the ratio's low part: 4e-17 off (#1746's Review A); log Γ(½) − ½ log b.
        var value = Lodestar.Stats.Internal.DoubleDouble.LogBeta(0.5, 5e250);
        Assert.Equal(-288.0554906375481, value.High);
        Assert.Equal(1.4856592530925077e-14, value.Low, 1e-28);
    }

    [Fact]
    public void The_F_law_at_shapes_past_2e305_keeps_its_ends_and_its_mirror()
    {
        // log Γ(s) and s log(s + o) each overflowed, and odds rounded to 0 at ε passed the limit's bound: NaN where main
        // answered 0 and 1; a subnormal target kept a root 2 ulps short of 1, whose tail is 0 (#1746's Review A).
        Assert.Equal(0.0, Distributions.FisherSnedecorCdf(double.Epsilon, 1e307, 1e308));
        Assert.Equal(1.0, Distributions.FisherSnedecorSf(double.Epsilon, 1e307, double.MaxValue));
        Assert.Equal(-1.6754985354208095e307, Lodestar.Stats.Internal.DoubleDouble.LogBeta(5e306, 5e307).High, 1e-14 * 1.7e307);
        Assert.Equal(1.0, Distributions.FisherSnedecorQuantile(1e-310, 1e40, 1e41));
        Assert.Equal(1.0, Distributions.FisherSnedecorQuantile(2.2e-308, 1e307, 2e307));
        // The 106-bit chi-squared limit at a shape near 1e307 formed ∞ − ∞ and read NaN (#1746's Review A).
        Assert.Equal(1.0, Distributions.FisherSnedecorSf(1e-300, 1e308, 1e300));
        Assert.Equal(1.0, Distributions.FisherSnedecorCdf(1e300, 1e300, 1e308));

        // The caller's side near one rounds 1e-16 wide: its round trip holds, and its mirror, judged on a finer tail, is ±∞
        // (Cyril, 2026-10-29).
        Assert.Equal(double.MaxValue, Distributions.FisherSnedecorQuantile(0.9999999999999999, 4.519e-12, 0.0392));
        Assert.Equal(double.PositiveInfinity, Distributions.FisherSnedecorIsf(1.1102230246251565e-16, 4.519e-12, 0.0392));
        Assert.Equal(double.MaxValue, Distributions.StudentQuantile(0.9999999999999999, 0.0506));
        Assert.Equal(double.NegativeInfinity, Distributions.StudentQuantile(1.1102230246251565e-16, 0.0506));
    }

    [Theory]
    [InlineData(0.05)]
    [InlineData(0.1)]
    [InlineData(0.7)]
    public void A_student_quantile_whose_target_is_the_largest_doubles_rounded_tail_is_that_double(double df)
    {
        // That tail before rounding lies short of the target, the root past every double: ±∞, where main answered the
        // largest double and the round trip expects it (Cyril, 2026-10-29, #1746). An ulp further, ±∞ stands.
        double upper = Distributions.StudentSf(double.MaxValue, df);
        double lower = Distributions.StudentCdf(-double.MaxValue, df);
        Assert.False(ExactQuantileContract.StudentUpperReaches(double.MaxValue, df, upper));
        Assert.Equal(double.MaxValue, Distributions.StudentIsf(upper, df));
        Assert.Equal(-double.MaxValue, Distributions.StudentQuantile(lower, df));
        Assert.Equal(double.PositiveInfinity, Distributions.StudentIsf(Math.BitDecrement(upper), df));
        Assert.Equal(double.NegativeInfinity, Distributions.StudentQuantile(Math.BitDecrement(lower), df));
    }

    [Theory]
    [InlineData(0.7, 0.001, 1e6)]
    [InlineData(0.3, 10.0, 0.001)]
    public void An_f_quantile_on_a_double_form_tail_is_searched_past_the_window(double p, double dfn, double dfd)
    {
        // A subnormal half's limit, whose tail is a double: the root was kept 1,522 and 1,397 ulps past the crossing.
        double f = Distributions.FisherSnedecorQuantile(p, dfn, dfd);
        Func<double, bool> reaches = x => p <= 0.5
            ? ExactQuantileContract.FisherLowerReaches(x, dfn, dfd, p)
            : ExactQuantileContract.FisherUpperReaches(x, dfn, dfd, 1.0 - p);
        Assert.True(reaches(f));
        Assert.False(reaches(Math.BitDecrement(f)));
    }

    [Fact]
    public void An_f_tail_an_ulp_from_the_mean_at_large_dfs_keeps_its_deviation()
    {
        // ½ − 9.9e-15 by the log-F normal limit, off by order 1/d of the deviation; it read one half (#1745's Review A).
        Assert.Equal(0.4999999999999901, Distributions.FisherSnedecorCdf(Math.BitDecrement(1.0), 2e5, 2e5));
        Assert.Equal(0.5000000000000198, Distributions.FisherSnedecorCdf(Math.BitIncrement(1.0), 2e5, 2e5));
    }

    [Fact]
    public void An_f_split_rounded_to_one_half_off_the_mean_keeps_its_deviation()
    {
        // At dfs of 1e308 the sum overflows and the split rounds to one half an ulp below f = 1, about 1e138 standard
        // deviations from the mean: the symmetry's one half read there, where main read 0 and 1 (#1745's Review A).
        Assert.Equal(0.0, Distributions.FisherSnedecorCdf(Math.BitDecrement(1.0), 1e308, 1e308));
        Assert.Equal(1.0, Distributions.FisherSnedecorSf(Math.BitDecrement(1.0), 1e308, 1e308));
    }

    [Theory]
    [InlineData(1e20)]
    [InlineData(1e280)]
    [InlineData(1e296)]
    public void An_f_tail_at_a_huge_denominator_df_is_its_chi_squared_limit(double dfd)
    {
        // erfc(√5), 0.00156540225800254968: from the point's low part alone −log x drifted 4 ulps either way (#1745's Review A).
        Assert.Equal(0.0015654022580025497, Distributions.FisherSnedecorSf(10, 1, dfd));
    }

    [Fact]
    public void A_far_student_tail_is_rounded_once_and_falls_with_t()
    {
        // 1/(π 1e100); one double exponential of a rounded log read 25 ulps off and held flat for 150 ulps (#1745's Review A).
        Assert.Equal(3.1830988618379067e-101, Distributions.StudentSf(1e100, 1));
        double t = 1e100;
        double previous = Distributions.StudentSf(t, 1);
        for (int i = 0; i < 1000; i++)
        {
            t = Math.BitIncrement(t);
            double tail = Distributions.StudentSf(t, 1);
            Assert.True(tail <= previous);
            previous = tail;
        }
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
