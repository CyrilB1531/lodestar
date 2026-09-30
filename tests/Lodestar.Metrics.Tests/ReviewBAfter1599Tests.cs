using Xunit;

namespace Lodestar.Metrics.Tests;

/// <summary>
/// The Review B finding of <c>Lodestar.Metrics</c> after #1599: <c>RocAuc.MultiClass</c> refuses in
/// <c>_multiclass_roc_auc_score</c>'s order and words; each sentence was measured on scikit-learn 1.9.1.
/// </summary>
public sealed class ReviewBAfter1599Tests
{
    private static readonly int[] YTrue = [0, 1, 2, 0];

    private static readonly double[] Good = [.6, .3, .1, .2, .5, .3, .1, .2, .7, .5, .3, .2];

    /// <summary><see cref="Good"/> with its first row summing to 1.3.</summary>
    private static readonly double[] Bad = [.9, .2, .2, .2, .5, .3, .1, .2, .7, .5, .3, .2];

    private const string RowSums =
        "Target scores need to be probabilities for multiclass roc_auc, i.e. they should sum up to 1.0 over classes";

    [Fact]
    public void The_row_sums_come_before_the_labels()
    {
        // roc_auc_score(..., multi_class='ovr', labels=[2,1,0]) on a row summing to 1.3 names the row sums (#1601).
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass(YTrue, Bad, 3, new MultiClassRocOptions { Labels = [2, 1, 0] }));

        Assert.StartsWith(RowSums, error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void The_row_sums_come_before_a_one_vs_one_weight()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass(YTrue, Bad, 3, new MultiClassRocOptions
        {
            Strategy = MultiClassStrategy.OneVsOne,
            SampleWeight = [1, 1, 1, 1],
        }));

        Assert.StartsWith(RowSums, error.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(new[] { 2, 1, 0 }, "Parameter 'labels' must be ordered")]
    [InlineData(new[] { 0, 0, 1 }, "Parameter 'labels' must be unique")]
    [InlineData(new[] { 0, 1, 2, 3 }, "Number of given labels, 4, not equal to the number of columns in 'y_score', 3")]
    public void Labels_are_refused_in_the_reference_s_order_and_words(int[] labels, string message)
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass(YTrue, Good, 3, new MultiClassRocOptions { Labels = labels }));

        Assert.StartsWith(message, error.Message, StringComparison.Ordinal);
        Assert.Equal("labels", error.ParamName);
    }

    [Fact]
    public void The_labels_come_before_a_one_vs_one_weight()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass(YTrue, Good, 3, new MultiClassRocOptions
        {
            Strategy = MultiClassStrategy.OneVsOne,
            Labels = [0, 1, 3],
            SampleWeight = [1, 1, 1, 1],
        }));

        Assert.StartsWith("'y_true' contains labels not in parameter 'labels'", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void The_row_sums_come_before_the_averaging()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => RocAuc.MultiClass(YTrue, Bad, 3, new MultiClassRocOptions { Average = Averaging.Micro }));

        Assert.StartsWith(RowSums, error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void The_averaging_comes_before_the_labels()
    {
        // Micro under one-vs-one is refused at the averaging step, before labels=[2,1,0] is read.
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass(YTrue, Good, 3, new MultiClassRocOptions
        {
            Strategy = MultiClassStrategy.OneVsOne,
            Average = Averaging.Micro,
            Labels = [2, 1, 0],
        }));

        Assert.StartsWith("average must be one of ('macro', 'weighted', None) for multiclass problems", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Binary_averaging_is_refused_before_any_array_is_read()
    {
        // validate_params refuses average='binary' ahead of the row sums a 1.3 row would fail.
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => RocAuc.MultiClass(YTrue, Bad, 3, new MultiClassRocOptions { Average = Averaging.Binary }));

        Assert.StartsWith(
            "The 'average' parameter of roc_auc_score must be a str among {'macro', 'micro', 'samples', 'weighted'} or None. Got 'binary' instead.",
            error.Message,
            StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(false, 0.71875)]
    [InlineData(true, 0.6802685950413223)]
    public void Micro_averaging_one_vs_rest_scores_the_raveled_matrix(bool weighted, double expected)
    {
        // roc_auc_score(..., multi_class='ovr', average='micro'), which this refused.
        int[] yTrue = [0, 1, 2, 0, 1, 2, 2, 1];
        double[] yScore = [.5, .3, .2, .3, .3, .4, .1, .6, .3, .4, .4, .2, .2, .5, .3, .3, .3, .4, .2, .2, .6, .6, .2, .2];
        double[] weights = weighted ? [1, 2, .5, 1, 3, 1, .5, 2] : [];

        foreach (int workers in new[] { 1, 4 })
        {
            double score = RocAuc.MultiClass(yTrue, yScore, 3, new MultiClassRocOptions
            {
                Average = Averaging.Micro,
                SampleWeight = weights,
                MaxDegreeOfParallelism = workers,
            });

            Assert.Equal(expected, score, 1e-15);
        }
    }

    [Fact]
    public void The_labels_come_before_a_weight_of_the_wrong_length()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass(
            YTrue, Good, 3, new MultiClassRocOptions { Labels = [2, 1, 0], SampleWeight = [1, 1] }));

        Assert.StartsWith("Parameter 'labels' must be ordered", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_false_positive_rate_that_turns_back_is_refused_as_auc_refuses_it()
    {
        // roc_auc_score([0,0,1,1,0], [.9,.1,.4,.8,.3], sample_weight=[2,-3,1,1,4]) raises "x is neither increasing
        // nor decreasing"; the area was returned.
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => RocAuc.Score([0, 0, 1, 1, 0], [.9, .1, .4, .8, .3], 1, [2, -3, 1, 1, 4]));

        Assert.StartsWith("x is neither increasing nor decreasing", error.Message, StringComparison.Ordinal);
        Assert.Equal("sampleWeight", error.ParamName);
    }

    [Fact]
    public void A_positive_total_at_or_below_zero_is_NaN_as_roc_curve_makes_it()
    {
        // roc_auc_score([0,1,0,1], [.1,.4,.35,.8], sample_weight=[1,-2,3,1]) is nan: the positives total -1.
        Assert.True(double.IsNaN(RocAuc.Score([0, 1, 0, 1], [.1, .4, .35, .8], 1, [1, -2, 3, 1])));
    }

    [Fact]
    public void Micro_over_a_declared_class_absent_from_y_true_scores_the_raveled_matrix()
    {
        double score = RocAuc.MultiClass([0, 1, 0, 1], [.5, .3, .2, .3, .3, .4, .4, .4, .2, .2, .5, .3], 3, new MultiClassRocOptions
        {
            Average = Averaging.Micro,
            Labels = [0, 1, 2],
        });

        Assert.Equal(0.859375, score, 1e-15);
    }

    [Fact]
    public void Micro_refuses_all_zero_weights_and_answers_NaN_on_cancelling_ones()
    {
        // Weighted's zero-total shortcut is not micro's: [0,0,0,0] is refused and [1,-1,0,0] is nan, as scikit-learn does.
        double[] yScore = [.6, .3, .1, .2, .5, .3, .5, .3, .2, .2, .6, .2];
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass([0, 1, 0, 1], yScore, 3, new MultiClassRocOptions
        {
            Average = Averaging.Micro,
            Labels = [0, 1, 2],
            SampleWeight = [0, 0, 0, 0],
        }));

        Assert.StartsWith("Sample weights must contain at least one non-zero number.", error.Message, StringComparison.Ordinal);
        Assert.True(double.IsNaN(RocAuc.MultiClass([0, 1, 0, 1], yScore, 3, new MultiClassRocOptions
        {
            Average = Averaging.Micro,
            Labels = [0, 1, 2],
            SampleWeight = [1, -1, 0, 0],
        })));
    }

    [Fact]
    public void A_class_absent_from_y_true_is_NaN_before_its_curve_reads_a_negative_weight()
    {
        // _binary_roc_auc_score stops on the one-label column: roc_auc_score(..., labels=[0,1,2]) is nan, not a refusal.
        double[] yScore = [.5, .3, .2, .2, .6, .2, .3, .4, .3, .6, .2, .2, .5, .4, .1, .1, .7, .2];

        Assert.True(double.IsNaN(RocAuc.MultiClass([1, 1, 1, 0, 0, 1], yScore, 3, new MultiClassRocOptions
        {
            Labels = [0, 1, 2],
            SampleWeight = [1, 1, 1, -1, 1, 1],
        })));
    }

    [Fact]
    public void A_class_curve_refused_in_a_worker_surfaces_as_the_sequential_refusal()
    {
        // roc_auc_score(..., multi_class='ovr', labels=[0,1,2], sample_weight=[-0.5,1,3,2,0.5,1]) raises "x is neither
        // increasing nor decreasing" in a class curve; four workers must throw that ArgumentException, not an AggregateException.
        int[] yTrue = [0, 1, 0, 0, 0, 1];
        double[] yScore =
        [
            4 / 13.0, 5 / 13.0, 4 / 13.0, .5, .25, .25, 5 / 12.0, 3 / 12.0, 4 / 12.0,
            1 / 8.0, 3 / 8.0, 4 / 8.0, 2 / 9.0, 4 / 9.0, 3 / 9.0, 5 / 11.0, 2 / 11.0, 4 / 11.0,
        ];
        double[] weights = [-0.5, 1.0, 3.0, 2.0, 0.5, 1.0];

        ArgumentException sequential = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass(
            yTrue, yScore, 3, new MultiClassRocOptions { Labels = [0, 1, 2], SampleWeight = weights }));
        ArgumentException parallel = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass(
            yTrue, yScore, 3, new MultiClassRocOptions { Labels = [0, 1, 2], SampleWeight = weights, MaxDegreeOfParallelism = 4 }));

        Assert.StartsWith("x is neither increasing nor decreasing", sequential.Message, StringComparison.Ordinal);
        Assert.Equal(sequential.Message, parallel.Message);
    }

    [Fact]
    public void A_tie_under_negative_weights_is_summed_in_roc_curve_s_order()
    {
        // roc_auc_score([0,0,0,1,0], [.9,.9,.9,.5,.1], sample_weight=[1,1e16,-1e16,1,1]) is 1.0: argsort(stable=True,
        // descending=True) sums the tie's 1, 1e16, -1e16 in that order; reversed, it gives 0.5.
        Assert.Equal(1.0, RocAuc.Score([0, 0, 0, 1, 0], [.9, .9, .9, .5, .1], 1, [1, 1e16, -1e16, 1, 1]));
    }

    [Theory]
    [InlineData(Averaging.Macro, 0.0)]
    [InlineData(Averaging.Macro, double.NaN)]
    [InlineData(Averaging.Weighted, double.NaN)]
    public void Weights_are_not_checked_when_no_class_curve_reads_them(Averaging average, double first)
    {
        // roc_auc_score([0,0,0], ..., labels=[0,1,2]) is nan with weights [0,0,0] or [nan,1,1]: every column is
        // one label, so no roc_curve reads a weight.
        double[] weights = double.IsNaN(first) ? [double.NaN, 1, 1] : [0, 0, 0];

        Assert.True(double.IsNaN(RocAuc.MultiClass([0, 0, 0], [.6, .3, .1, .5, .3, .2, .7, .2, .1], 3, new MultiClassRocOptions
        {
            Average = average,
            Labels = [0, 1, 2],
            SampleWeight = weights,
        })));
    }
}
