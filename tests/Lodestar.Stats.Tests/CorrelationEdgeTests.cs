using Xunit;

namespace Lodestar.Stats.Tests;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible fixture; no security use.
#pragma warning disable S2245, CA5394

/// <summary>What the correlation corpora cannot hold: the refusals, and the cases scipy will not produce.</summary>
public sealed class CorrelationEdgeTests
{
    /// <summary>
    /// Decision 0007, first family — the reference is wrong in a way it does not state.
    /// Measured on scipy 1.18.1: <c>kendalltau([1, 2], [3, 4], method='asymptotic')</c> raises
    /// <c>ZeroDivisionError</c>, because the variance's last term divides <c>x0 * y0</c> by
    /// <c>9 n (n-1) (n-2)</c> and both are zero at two pairs. It is an unguarded division
    /// escaping, not a refusal scipy defends: nothing in its documentation says the asymptotic
    /// branch has a lower size bound. The IEEE answer is taken instead, so the statistic — which
    /// is perfectly well defined there — still comes back.
    /// </summary>
    [Fact]
    public void KendallTau_asymptotic_at_two_pairs_answers_nan()
    {
        TestResult result = KendallTau.Test(
            [1.0, 2.0], [3.0, 4.0], method: ExactMethod.Asymptotic);

        Assert.Equal(1.0, result.Statistic, 12);
        Assert.True(double.IsNaN(result.PValue), $"expected a NaN p-value, got {result.PValue}.");
    }

    /// <summary>
    /// Decision 0007, first family, and this one changes a conclusion rather than an edge case.
    /// Measured on scipy 1.18.1: <c>spearmanr(x, y, nan_policy='omit')</c> routes through
    /// <c>mstats_basic</c>'s masked arrays, which do not clip. On these five pairs it answers
    /// <c>rho=1.0000000000000002</c> and then <c>pvalue=1.0</c> — no evidence of association,
    /// for perfectly monotone data — because the Student argument divides by a negative
    /// <c>1 - rho</c> clipped to zero. The same four surviving pairs passed directly give
    /// <c>(1.0, 0.0)</c>, so scipy contradicts itself rather than defending a rule.
    /// </summary>
    [Fact]
    public void Spearman_under_omit_answers_the_filtered_correlation()
    {
        double[] x = [1.0, 2.0, double.NaN, 4.0, 5.0];
        double[] y = [2.0, 3.0, double.NaN, 5.0, 7.0];

        TestResult omitted = Spearman.Test(x, y, nanPolicy: NanPolicy.Omit);
        TestResult filtered = Spearman.Test([1.0, 2.0, 4.0, 5.0], [2.0, 3.0, 5.0, 7.0]);

        Assert.Equal(1.0, omitted.Statistic, 12);
        Assert.Equal(0.0, omitted.PValue, 12);
        Assert.Equal(filtered.Statistic, omitted.Statistic, 15);
        Assert.Equal(filtered.PValue, omitted.PValue, 15);
    }

    [Fact]
    public void Pearson_refuses_a_sample_shorter_than_two_pairs()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => Pearson.Test([1.0], [2.0]));

        Assert.Contains("at least two pairs", error.Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// The rank tests answer where <c>Pearson</c> refuses, because their references do:
    /// <c>pearsonr</c> raises <c>ValueError</c> below two pairs while <c>spearmanr</c> and
    /// <c>kendalltau</c> return <c>(nan, nan)</c>. The disagreement is upstream, and each side
    /// is matched rather than reconciled.
    /// </summary>
    [Fact]
    public void The_rank_tests_answer_nan_below_two_pairs_where_pearson_refuses()
    {
        TestResult spearman = Spearman.Test([1.0], [2.0]);
        TestResult kendall = KendallTau.Test([1.0], [2.0]);

        Assert.True(double.IsNaN(spearman.Statistic) && double.IsNaN(spearman.PValue));
        Assert.True(double.IsNaN(kendall.Statistic) && double.IsNaN(kendall.PValue));
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(-0.5)]
    [InlineData(double.NaN)]
    public void A_confidence_level_outside_the_open_unit_interval_is_refused(double level)
    {
        PearsonResult result = Pearson.Test([1.0, 2.0, 3.0, 4.0], [2.0, 1.0, 4.0, 3.0]);

        Assert.Throws<ArgumentOutOfRangeException>(() => result.ConfidenceInterval(level));
    }

    [Fact]
    public void All_three_refuse_samples_of_different_lengths()
    {
        Assert.Throws<ArgumentException>(() => Pearson.Test([1.0, 2.0, 3.0], [1.0, 2.0]));
        Assert.Throws<ArgumentException>(() => Spearman.Test([1.0, 2.0, 3.0], [1.0, 2.0]));
        Assert.Throws<ArgumentException>(() => KendallTau.Test([1.0, 2.0, 3.0], [1.0, 2.0]));
    }

    /// <summary>
    /// Omission drops the pair, not the value: dropping each sample on its own would leave
    /// four values against four and correlate observations that were never observed together.
    /// </summary>
    [Fact]
    public void Omit_drops_the_pair_rather_than_the_value()
    {
        double[] x = [1.0, 2.0, double.NaN, 4.0, 5.0];
        double[] y = [2.0, double.NaN, 3.0, 5.0, 7.0];
        double[] filteredX = [1.0, 4.0, 5.0];
        double[] filteredY = [2.0, 5.0, 7.0];

        PearsonResult omitted = Pearson.Test(x, y, nanPolicy: NanPolicy.Omit);
        PearsonResult listwise = Pearson.Test(filteredX, filteredY);

        Assert.Equal(listwise.Statistic, omitted.Statistic, 12);
        Assert.Equal(listwise.PValue, omitted.PValue, 12);
    }

    [Fact]
    public void A_nan_refuses_under_raise_in_all_three()
    {
        double[] x = [1.0, 2.0, double.NaN, 4.0];
        double[] y = [2.0, 3.0, 4.0, 5.0];

        Assert.Throws<ArgumentException>(() => Pearson.Test(x, y, nanPolicy: NanPolicy.Raise));
        Assert.Throws<ArgumentException>(() => Spearman.Test(x, y, nanPolicy: NanPolicy.Raise));
        Assert.Throws<ArgumentException>(() => KendallTau.Test(x, y, nanPolicy: NanPolicy.Raise));
    }

    /// <summary>
    /// The exact branch is refused on a tied sample, which is where it differs from
    /// <see cref="MannWhitney"/>: there scipy computes an exact p-value on tied data and this
    /// package matches it, here scipy refuses and this package matches that instead.
    /// </summary>
    [Fact]
    public void The_exact_method_is_refused_on_a_tied_sample()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => KendallTau.Test(
                [1.0, 1.0, 2.0, 3.0], [1.0, 2.0, 2.0, 3.0], method: ExactMethod.Exact));

        Assert.Contains("untied samples only", error.Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Past the table's measured ceiling <c>Exact</c> refuses and <c>Auto</c> falls back, which
    /// is the contract <see cref="MannWhitney"/> already has: a caller who never asked for an
    /// exact answer must not be handed an exception over one.
    /// </summary>
    [Fact]
    public void Auto_falls_back_past_the_table_ceiling_where_exact_refuses()
    {
        var random = new System.Random(1120);
        int n = 1_200;
        double[] x = new double[n];
        double[] y = new double[n];
        for (int i = 0; i < n; i++)
        {
            x[i] = i;
            y[i] = random.NextDouble();
        }

        Assert.Throws<ArgumentOutOfRangeException>(
            () => KendallTau.Test(x, y, method: ExactMethod.Exact));

        TestResult auto = KendallTau.Test(x, y, method: ExactMethod.Auto);
        TestResult asymptotic = KendallTau.Test(x, y, method: ExactMethod.Asymptotic);

        Assert.Equal(asymptotic.PValue, auto.PValue, 15);
    }

    /// <summary>
    /// Two pairs admit only ±1, so the two-sided p-value is one whatever the data — scipy
    /// documents the limit of the beta as its shape parameters reach zero rather than
    /// evaluating a density that is not defined there.
    /// </summary>
    [Fact]
    public void Pearson_answers_one_at_two_pairs()
    {
        PearsonResult rising = Pearson.Test([1.0, 2.0], [3.0, 9.0]);
        PearsonResult falling = Pearson.Test([1.0, 2.0], [9.0, 3.0]);

        Assert.Equal(1.0, rising.Statistic, 12);
        Assert.Equal(-1.0, falling.Statistic, 12);
        Assert.Equal(1.0, rising.PValue, 12);
        Assert.Equal(1.0, falling.PValue, 12);
    }

    [Fact]
    public void A_constant_sample_has_no_correlation_and_no_interval()
    {
        PearsonResult result = Pearson.Test([2.0, 2.0, 2.0, 2.0, 2.0], [1.0, 4.0, 2.0, 8.0, 3.0]);
        (double low, double high) = result.ConfidenceInterval();

        Assert.True(double.IsNaN(result.Statistic));
        Assert.True(double.IsNaN(result.PValue));
        Assert.True(double.IsNaN(low) && double.IsNaN(high));
    }

    /// <summary>Below four pairs the Fisher transform has no standard error, and the interval is the whole range.</summary>
    [Fact]
    public void The_interval_spans_the_whole_range_below_four_pairs()
    {
        (double low, double high) = Pearson.Test([1.0, 2.0, 3.0], [1.0, 3.0, 2.0])
            .ConfidenceInterval();

        Assert.Equal(-1.0, low, 12);
        Assert.Equal(1.0, high, 12);
    }

    /// <summary>A one-sided interval is half-open: the far bound is the correlation's own limit.</summary>
    [Theory]
    [InlineData(Alternative.Less, -1.0)]
    [InlineData(Alternative.Greater, 1.0)]
    public void A_one_sided_interval_is_half_open(Alternative alternative, double expectedBound)
    {
        double[] x = [1.0, 2.0, 3.0, 4.0, 5.0];
        double[] y = [2.0, 1.0, 4.0, 3.0, 5.0];

        (double low, double high) = Pearson.Test(x, y, alternative).ConfidenceInterval();

        Assert.Equal(expectedBound, alternative == Alternative.Less ? low : high, 12);
    }
}

#pragma warning restore S2245, CA5394
