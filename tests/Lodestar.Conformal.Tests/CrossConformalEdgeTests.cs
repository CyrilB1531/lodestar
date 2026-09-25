using Xunit;

namespace Lodestar.Conformal.Tests;

/// <summary>What <see cref="CrossConformal"/> and the gamma score refuse, and the edges MAPIE raises at or returns NaN for (#1159).</summary>
public sealed class CrossConformalEdgeTests
{
    // Three models, six training samples, two per fold.
    private static readonly int[] Folds = [0, 0, 1, 1, 2, 2];
    private static readonly double[] TestPredictions = [10.0, 11.0, 12.0];
    private static readonly double[] Scores = [0.5, 1.0, 0.2, 0.8, 0.3, 0.6];

    private static bool[] MaskOf(int[] folds, int models)
    {
        var mask = new bool[folds.Length * models];
        for (int i = 0; i < folds.Length; i++)
        {
            mask[(i * models) + folds[i]] = true;
        }

        return mask;
    }

    [Fact]
    public void Arguments_out_of_range_are_refused()
    {
        bool[] mask = MaskOf(Folds, 3);
        Assert.Throws<ArgumentOutOfRangeException>(() => CrossConformal.Interval(TestPredictions, Folds, Scores, 0.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => CrossConformal.Interval(TestPredictions, Folds, Scores, double.NaN));
        Assert.Throws<ArgumentOutOfRangeException>(() => CrossConformal.Interval(TestPredictions, Folds, Scores, 0.1, (CrossConformalMethod)9));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            CrossConformal.Interval(TestPredictions, mask, Scores, 0.1, CrossConformalMethod.Plus, (CrossConformalAggregation)9));
        Assert.Throws<ArgumentOutOfRangeException>(() => CrossConformal.Interval(TestPredictions, [0, 0, 1, 1, 2, 3], Scores, 0.1));
        Assert.Throws<ArgumentOutOfRangeException>(() => CrossConformal.Interval([], Folds, Scores, 0.1));
        Assert.Throws<ArgumentOutOfRangeException>(() => CrossConformal.OutOfSample([1.0], [true], 0));
    }

    [Fact]
    public void Mismatched_lengths_and_NaN_scores_are_refused()
    {
        bool[] mask = MaskOf(Folds, 3);
        Assert.Throws<ArgumentException>(() => CrossConformal.Interval(TestPredictions, Folds.AsSpan(1), Scores, 0.1));
        Assert.Throws<ArgumentException>(() => CrossConformal.Interval(TestPredictions, mask.AsSpan(1), Scores, 0.1));
        Assert.Throws<ArgumentException>(() => CrossConformal.OutOfSample([1.0, 2.0, 3.0], [true, false], 3));
        Assert.Throws<ArgumentException>(() => CrossConformal.Interval(TestPredictions, Folds, [0.5, double.NaN, 0.2, 0.8, 0.3, 0.6], 0.1));
        Assert.Throws<ArgumentException>(() => CrossConformal.Interval(TestPredictions, new bool[18], Scores, 0.1));
        Assert.Throws<ArgumentException>(() => CrossConformal.Interval([10.0, double.NaN, 12.0], Folds, Scores, 0.3));
        Assert.Throws<ArgumentException>(() => CrossConformal.Interval([10.0, double.PositiveInfinity, 12.0], Folds, Scores, 0.3));
        Assert.Throws<ArgumentException>(() =>
            CrossConformal.Interval([10.0, double.NaN, 12.0], mask, Scores, 0.3, CrossConformalMethod.MinMax));
    }

    [Fact]
    public void The_gamma_score_refuses_a_non_positive_target_or_prediction()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SplitConformal.GammaScores([1.0, 0.0], [1.0, 1.0]));
        Assert.Throws<ArgumentOutOfRangeException>(() => SplitConformal.GammaScores([1.0, 2.0], [1.0, -1.0]));
        Assert.Throws<ArgumentException>(() => SplitConformal.GammaScores([1.0, 2.0], [1.0]));
        Assert.Throws<ArgumentOutOfRangeException>(() => SplitConformal.GammaInterval(0.0, Scores, 0.1));
        Assert.Throws<ArgumentOutOfRangeException>(() => CrossConformal.GammaInterval([10.0, -1.0, 12.0], Folds, Scores, 0.3));
    }

    /// <summary>A sample no model holds out has no score; every method skips it, where MAPIE's minmax returns NaN.</summary>
    [Fact]
    public void A_sample_no_model_holds_out_is_skipped()
    {
        bool[] mask = MaskOf(Folds, 3);
        bool[] withOrphan = [.. mask, false, false, false];
        double[] withNaN = [.. Scores, double.NaN];

        foreach (CrossConformalMethod method in new[] { CrossConformalMethod.Plus, CrossConformalMethod.MinMax })
        {
            Assert.Equal(
                CrossConformal.Interval(TestPredictions, mask, Scores, 0.3, method),
                CrossConformal.Interval(TestPredictions, withOrphan, withNaN, 0.3, method));
        }

        Assert.True(double.IsNaN(CrossConformal.OutOfSample([1.0, 2.0, 3.0, 4.0], [true, false, false, false], 2)[1]));
    }

    /// <summary>The mask and fold overloads agree when each sample is held out by one model.</summary>
    [Fact]
    public void The_fold_overload_is_the_mask_with_one_model_per_row()
    {
        bool[] mask = MaskOf(Folds, 3);
        foreach (CrossConformalMethod method in new[] { CrossConformalMethod.Plus, CrossConformalMethod.MinMax })
        {
            Assert.Equal(
                CrossConformal.Interval(TestPredictions, mask, Scores, 0.3, method),
                CrossConformal.Interval(TestPredictions, Folds, Scores, 0.3, method));
        }
    }

    /// <summary>A rank past the samples is an infinite bound (decision 0007), where MAPIE raises.</summary>
    [Fact]
    public void Too_few_samples_for_the_level_give_infinite_bounds()
    {
        (double lower, double upper) = CrossConformal.Interval(TestPredictions, Folds, Scores, 0.1);
        (double gammaLower, double gammaUpper) = SplitConformal.GammaInterval(10.0, Scores, 0.1);

        Assert.Equal(double.NegativeInfinity, lower);
        Assert.Equal(double.PositiveInfinity, upper);
        Assert.Equal(double.NegativeInfinity, gammaLower);
        Assert.Equal(double.PositiveInfinity, gammaUpper);
    }

    /// <summary>The median of an even count is the mean of the two middle predictions, as numpy's.</summary>
    [Fact]
    public void The_median_of_an_even_count_averages_the_middle_two()
    {
        double[] outOfSample = CrossConformal.OutOfSample(
            [1.0, 4.0, 2.0, 10.0], [true, true, true, true], 4, CrossConformalAggregation.Median);

        Assert.Equal(3.0, outOfSample[0]);
    }

    /// <summary>MAPIE evaluates the lower level as (1 − 2a) + a, 0.9500000000000001 at a = 0.05, one rank further at n = 59.</summary>
    [Fact]
    public void The_lower_bound_reads_the_rank_the_reference_reads()
    {
        double[] scores = [.. Enumerable.Range(1, 59).Select(v => v / 100.0)];

        (double lower, _) = SplitConformal.GammaInterval(1.0, scores, 0.1);

        // ⌈0.9500000000000001 · 60⌉ = 58, the second smallest score; 1 − 0.05 would read 57, the third.
        Assert.Equal(1.02, lower, 1e-15);
    }
}
