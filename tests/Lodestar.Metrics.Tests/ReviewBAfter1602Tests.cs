using Xunit;

namespace Lodestar.Metrics.Tests;

/// <summary>
/// The Review B findings of <c>Lodestar.Metrics</c> after #1602 and what their Review A added: <c>RocAuc.MultiClass</c>'s
/// shape checks where scikit-learn 1.9.1 makes them, and its binary path for one or two columns; each value and
/// sentence was measured there.
/// </summary>
public sealed class ReviewBAfter1602Tests
{
    private static readonly double[] ThreeRows = [.6, .3, .1, .2, .5, .3, .1, .2, .7];

    private static readonly double[] FourRowsTwoColumns = [.6, .4, .3, .7, .5, .5, .8, .2];

    private static readonly int[] OneColumnLabels = [3, 5, 3, 5, 5];

    private static readonly double[] OneColumn = [.2, .7, .4, .9, .3];

    [Fact]
    public void Too_few_rows_are_counted_after_the_row_sums()
    {
        // roc_auc_score([0,1,2,2], 3 rows with one summing to 1.5) names the row sums first (#1604).
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => RocAuc.MultiClass([0, 1, 2, 2], [.6, .3, .1, .5, .5, .5, .1, .2, .7], 3));

        Assert.StartsWith("Target scores need to be probabilities", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Too_few_rows_are_counted_after_the_labels()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => RocAuc.MultiClass([0, 1, 2, 2], ThreeRows, 3, new MultiClassRocOptions { Labels = [2, 1, 0] }));

        Assert.StartsWith("Parameter 'labels' must be ordered", error.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(new double[0], "Found input variables with inconsistent numbers of samples: [4, 3]")]
    [InlineData(new[] { 1.0, 1.0 }, "Found input variables with inconsistent numbers of samples: [4, 3, 2]")]
    [InlineData(new[] { 1.0, 1.0, 1.0, 1.0 }, "Found input variables with inconsistent numbers of samples: [4, 3, 4]")]
    public void The_sample_count_is_refused_in_check_consistent_length_s_words(double[] weights, string message)
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => RocAuc.MultiClass([0, 1, 2, 2], ThreeRows, 3, new MultiClassRocOptions { SampleWeight = weights }));

        Assert.StartsWith(message, error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_weight_count_alone_is_refused_in_the_same_words()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass(
            [0, 1, 2, 2], [.6, .3, .1, .2, .5, .3, .1, .2, .7, .5, .3, .2], 3, new MultiClassRocOptions { SampleWeight = [1, 1] }));

        Assert.StartsWith("Found input variables with inconsistent numbers of samples: [4, 4, 2]", error.Message, StringComparison.Ordinal);
        Assert.Equal("options", error.ParamName);
    }

    [Theory]
    [InlineData(MultiClassStrategy.OneVsRest, new double[0])]
    [InlineData(MultiClassStrategy.OneVsOne, new[] { 1.0, 1.0, 1.0, 1.0 })]
    public void Two_columns_over_two_labels_are_refused_as_the_binary_path_refuses_them(MultiClassStrategy strategy, double[] weights)
    {
        // roc_auc_score([0,1,1,0], shape (4, 2)) raises, before the row sums a [.9, .4] row would fail and ignoring
        // multi_class='ovo''s weight refusal; this returned a number (#1605).
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass(
            [0, 1, 1, 0], [.9, .4, .3, .7, .5, .5, .8, .2], 2, new MultiClassRocOptions { Strategy = strategy, SampleWeight = weights }));

        Assert.StartsWith("y should be a 1d array, got an array of shape (4, 2) instead.", error.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(6, new double[0], "Found input variables with inconsistent numbers of samples: [4, 3]")]
    [InlineData(8, new[] { 1.0, 1.0 }, "Found input variables with inconsistent numbers of samples: [4, 4, 2]")]
    public void Two_columns_over_two_labels_count_their_lengths_first(int values, double[] weights, string message)
    {
        // _binary_clf_curve's check_consistent_length runs before column_or_1d.
        double[] yScore = [.. FourRowsTwoColumns.Take(values)];

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => RocAuc.MultiClass([0, 1, 1, 0], yScore, 2, new MultiClassRocOptions { SampleWeight = weights }));

        Assert.StartsWith(message, error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Two_columns_over_one_label_are_NaN_whatever_else_is_wrong()
    {
        // roc_auc_score([1,1,1,1], shape (3, 2), sample_weight=[1,1]) is nan: the binary path's one-class answer comes first.
        Assert.True(double.IsNaN(RocAuc.MultiClass(
            [1, 1, 1, 1], [.6, .4, .3, .7, .5, .5], 2, new MultiClassRocOptions { SampleWeight = [1, 1] })));
    }

    [Fact]
    public void Two_columns_over_three_labels_take_the_multiclass_path()
    {
        // roc_auc_score([0,1,2,0], shape (4, 2), multi_class='ovr') reaches the class-count refusal instead.
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => RocAuc.MultiClass([0, 1, 2, 0], [.6, .4, .3, .7, .5, .5, .8, .2], 2));

        Assert.StartsWith("Number of classes in y_true not equal to the number of columns in 'y_score'", error.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(new double[0], 0.8333333333333333)]
    [InlineData(new[] { 1, 2, 1, .5, 1 }, 0.8571428571428572)]
    public void One_column_over_two_labels_is_the_binary_score_of_the_greater_label(double[] weights, double expected)
    {
        // roc_auc_score([3,5,3,5,5], shape (5, 1)) is roc_auc_score([3,5,3,5,5], the column), 5 positive; this refused it.
        double score = RocAuc.MultiClass([3, 5, 3, 5, 5], [.2, .7, .4, .9, .3], 1, new MultiClassRocOptions { SampleWeight = weights });

        Assert.Equal(expected, score, 1e-15);
    }

    [Fact]
    public void One_column_over_three_labels_takes_the_multiclass_path()
    {
        // roc_auc_score([1,2,3,1,2], ones((5, 1)), multi_class='ovr') refuses the class count; with a column of .2 … the row sums.
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass([1, 2, 3, 1, 2], [1.0, 1, 1, 1, 1], 1));

        Assert.StartsWith("Number of classes in y_true not equal to the number of columns in 'y_score'", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void One_column_ignores_the_options_the_binary_path_does_not_read()
    {
        // roc_auc_score(..., multi_class='ovo', average='micro', labels=[3,5], sample_weight=ones) is 0.8333333333333333.
        double score = RocAuc.MultiClass(OneColumnLabels, OneColumn, 1, new MultiClassRocOptions
        {
            Strategy = MultiClassStrategy.OneVsOne,
            Average = Averaging.Micro,
            Labels = [3, 5],
            SampleWeight = [1, 1, 1, 1, 1],
        });

        Assert.Equal(0.8333333333333333, score, 1e-15);
    }

    [Theory]
    [InlineData(double.NaN, "Input sample_weight contains NaN.")]
    [InlineData(0.0, "Sample weights must contain at least one non-zero number.")]
    public void One_column_refuses_its_weights_as_roc_curve_does_under_options(double first, string message)
    {
        double[] weights = double.IsNaN(first) ? [double.NaN, 1, 1, 1, 1] : [0, 0, 0, 0, 0];

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => RocAuc.MultiClass(OneColumnLabels, OneColumn, 1, new MultiClassRocOptions { SampleWeight = weights }));

        Assert.StartsWith(message, error.Message, StringComparison.Ordinal);
        Assert.Equal("options", error.ParamName);
    }

    [Fact]
    public void One_column_refuses_a_rate_that_turns_back_and_is_NaN_on_a_positive_total_below_zero()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass(
            [0, 0, 1, 1, 0], [.9, .1, .4, .8, .3], 1, new MultiClassRocOptions { SampleWeight = [2, -3, 1, 1, 4] }));

        Assert.StartsWith("x is neither increasing nor decreasing", error.Message, StringComparison.Ordinal);
        Assert.Equal("options", error.ParamName);
        Assert.True(double.IsNaN(RocAuc.MultiClass(
            [0, 1, 0, 1], [.1, .4, .35, .8], 1, new MultiClassRocOptions { SampleWeight = [1, -2, 3, 1] })));
    }

    [Theory]
    [InlineData(4, new double[0], "Found input variables with inconsistent numbers of samples: [5, 4]")]
    [InlineData(5, new[] { 1.0, 1.0 }, "Found input variables with inconsistent numbers of samples: [5, 5, 2]")]
    public void One_column_counts_its_lengths_as_the_binary_path_does(int rows, double[] weights, string message)
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass(
            OneColumnLabels, [.. OneColumn.Take(rows)], 1, new MultiClassRocOptions { SampleWeight = weights }));

        Assert.StartsWith(message, error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void An_empty_single_column_names_its_shape()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass([0, 1], [], 1));

        Assert.StartsWith("Found array with 0 sample(s) (shape=(0, 1)) while a minimum of 1 is required.", error.Message, StringComparison.Ordinal);
    }
}
