using Xunit;

namespace Lodestar.Metrics.Tests;

/// <summary>
/// The Review B findings of <c>Lodestar.Metrics</c> after #1560, one fact or theory each; the expected values
/// were measured on scikit-learn 1.9.1.
/// </summary>
public sealed class ReviewBAfter1560Tests
{
    [Theory]
    [InlineData(double.NaN, "Input contains NaN.")]
    [InlineData(double.PositiveInfinity, "Input contains infinity or a value too large for dtype('float64').")]
    public void The_median_refuses_a_non_finite_output_weight(double weight, string message)
    {
        // median_absolute_error(..., multioutput=[nan, 1]) raises; #1560 dropped the check and it scored NaN (#1564).
        ArgumentException error = Assert.Throws<ArgumentException>(() => MedianAbsoluteError.Score(
            [1, 2, 1, 1], [1, 3, 2, 2], outputCount: 2, outputWeights: [weight, 1]));

        Assert.StartsWith(message, error.Message, StringComparison.Ordinal);
        Assert.Equal("outputWeights", error.ParamName);
    }

    [Fact]
    public void The_median_checks_finiteness_before_the_single_output_rule()
    {
        // median_absolute_error([1,2], [1,3], multioutput=[nan]) raises "Input contains NaN." first.
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => MedianAbsoluteError.Score([1, 2], [1, 3], outputWeights: [double.NaN]));

        Assert.StartsWith("Input contains NaN.", error.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(new[] { 0.0, 0.0, 0.0 })]
    [InlineData(new[] { double.NaN, 1.0, 1.0 })]
    [InlineData(new[] { 1.0, 1.0 })]
    public void One_class_scores_NaN_whatever_the_weights(double[] weights)
    {
        // roc_auc_score([1,1,1], [.1,.2,.3], sample_weight=...) answers nan before roc_curve reads a weight (#1565).
        Assert.True(double.IsNaN(RocAuc.Score([1, 1, 1], [.1, .2, .3], 1, weights)));
    }

    [Fact]
    public void One_class_scores_NaN_whatever_the_score_length_but_refuses_no_scores()
    {
        // roc_auc_score([1,1,1], [.1,.2]) is nan; with [] check_array raises.
        Assert.True(double.IsNaN(RocAuc.Score([1, 1, 1], [.1, .2])));
        Assert.True(double.IsNaN(RocAuc.Score([1, 1, 1], [.1, .2, .3, .4])));
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.Score([1, 1, 1], []));
        Assert.Equal("Found array with 0 sample(s) (shape=(0,)) while a minimum of 1 is required. (Parameter 'yScore')", error.Message);
    }

    [Fact]
    public void One_class_still_refuses_a_NaN_score()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => RocAuc.Score([1, 1, 1], [.1, double.NaN, .3], 1, [0.0, 0.0, 0.0]));

        Assert.StartsWith("Input contains NaN.", error.Message, StringComparison.Ordinal);
        Assert.Equal("yScore", error.ParamName);
    }

    [Fact]
    public void One_vs_one_weighted_over_one_present_class_is_refused_as_the_reference_refuses_it()
    {
        // roc_auc_score([0,0,0], ..., multi_class='ovo', average='weighted', labels=[0,1,2]) raises ZeroDivisionError (#1566).
        ArgumentException error = Assert.Throws<ArgumentException>(() => OneClassOneVsOne(Averaging.Weighted, 1));

        Assert.Equal("Weights sum to zero, can't be normalized. (Parameter 'yTrue')", error.Message);
        Assert.Throws<ArgumentException>(() => OneClassOneVsOne(Averaging.Weighted, 2));
        Assert.True(double.IsNaN(OneClassOneVsOne(Averaging.Macro, 1)));
        Assert.True(double.IsNaN(OneClassOneVsOne(Averaging.Macro, 2)));
    }

    /// <summary>One-vs-one over <c>[0, 0, 0]</c> with three labels declared: no pair holds two present classes.</summary>
    private static double OneClassOneVsOne(Averaging average, int workers) => RocAuc.MultiClass(
        [0, 0, 0],
        [.6, .3, .1, .5, .3, .2, .7, .2, .1],
        3,
        new MultiClassRocOptions
        {
            Strategy = MultiClassStrategy.OneVsOne,
            Average = average,
            Labels = [0, 1, 2],
            MaxDegreeOfParallelism = workers,
        });
}
