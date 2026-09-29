using Xunit;

namespace Lodestar.Metrics.Tests;

/// <summary>
/// The Review B findings of <c>Lodestar.Metrics</c> after #1575, one fact each; the expected values were measured
/// on scikit-learn 1.9.1.
/// </summary>
public sealed class ReviewBAfter1575Tests
{
    [Fact]
    public void Two_labels_and_no_scores_get_the_reference_s_sentence_too()
    {
        // roc_auc_score([0,1,1], []) raises check_array's sentence, not a length mismatch (#1585).
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.Score([0, 1, 1], []));

        Assert.StartsWith("Found array with 0 sample(s) (shape=(0,)) while a minimum of 1 is required.", error.Message, StringComparison.Ordinal);
        Assert.Equal("yScore", error.ParamName);
    }

    [Theory]
    [InlineData(new[] { 0, 1, 2 }, 1)]
    [InlineData(new[] { 3, 4, 4 }, 1)]
    public void No_scores_are_refused_before_the_labels_are_read(int[] yTrue, int posLabel)
    {
        // roc_auc_score([0,1,2], []) and ([3,4,4], []) raise check_array's sentence first (#1585).
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.Score(yTrue, [], posLabel));

        Assert.StartsWith("Found array with 0 sample(s)", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_non_finite_score_is_refused_before_the_labels_are_read()
    {
        // roc_auc_score([0,1,2], [nan,1,2]) raises "Input contains NaN."; this said to use MultiClass.
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.Score([0, 1, 2], [double.NaN, 1, 2]));

        Assert.StartsWith("Input contains NaN.", error.Message, StringComparison.Ordinal);
        Assert.Equal("yScore", error.ParamName);
    }

    [Theory]
    [InlineData(new double[0])]
    [InlineData(new[] { .1 })]
    public void No_labels_are_refused_with_the_reference_s_sentence(double[] yScore)
    {
        // roc_auc_score([], []) and ([], [.1]) both raise "Found array with 0 sample(s)".
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.Score([], yScore));

        Assert.StartsWith("Found array with 0 sample(s) (shape=(0,)) while a minimum of 1 is required.", error.Message, StringComparison.Ordinal);
        Assert.Equal("yTrue", error.ParamName);
    }

    [Fact]
    public void Eight_class_totals_are_summed_pairwise_as_numpy_sums_them()
    {
        // Totals 1e16, six 1, -1e16: in order 0, pairwise 4, so scikit-learn skips the shortcut and answers nan (#1586).
        int[] yTrue = [0, 1, 2, 3, 4, 5, 6, 7];
        double[] yScore = [.. Enumerable.Repeat(1.0 / 8, 64)];

        double score = RocAuc.MultiClass(yTrue, yScore, 8, new MultiClassRocOptions
        {
            Average = Averaging.Weighted,
            SampleWeight = [1e16, 1, 1, 1, 1, 1, 1, -1e16],
        });

        Assert.True(double.IsNaN(score));
    }

    [Fact]
    public void The_weighted_one_vs_rest_shortcut_reads_the_class_totals()
    {
        // The sample sum is 1, the class totals 1e16 and -1e16 cancel: scikit-learn returns 0; this threw (#1586).
        int[] yTrue = [0, 1, 0, 2];
        double[] yScore = [.5, .3, .2, .2, .5, .3, .3, .3, .4, .1, .1, .8];

        double score = RocAuc.MultiClass(yTrue, yScore, 3, new MultiClassRocOptions
        {
            Average = Averaging.Weighted,
            SampleWeight = [1e16, -1e16, 1, 0],
        });

        Assert.Equal(0.0, score);
    }

    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(-1.0, 1.0)]
    public void The_median_refuses_output_weights_summing_to_zero(double first, double second)
    {
        // The page left this refusal out (#1588); scikit-learn raises ZeroDivisionError with the same words.
        ArgumentException error = Assert.Throws<ArgumentException>(() => MedianAbsoluteError.Score(
            [1, 2, 1, 1], [1, 3, 2, 2], outputCount: 2, outputWeights: [first, second]));

        Assert.StartsWith("Weights sum to zero, can't be normalized.", error.Message, StringComparison.Ordinal);
        Assert.Equal("outputWeights", error.ParamName);
    }

    [Fact]
    public void Weighted_average_precision_reads_the_label_totals_too()
    {
        // Rows (0, 1e16), (1, -1e16), (0, 1): 1 row by row, 0 label by label; average_precision_score returns 0.
        bool[] yTrue = [true, false, false, true, true, false];
        double[] yScore = [.9, .1, .2, .8, .7, .3];

        double score = AveragePrecision.Score(yTrue, yScore, 2, Averaging.Weighted, sampleWeight: [1e16, -1e16, 1]);

        Assert.Equal(0.0, score);
    }

    [Theory]
    [InlineData(0, 3, "Found array with 0 sample(s) (shape=(0,)) while a minimum of 1 is required.", "yTrue")]
    [InlineData(3, 0, "Found array with 0 sample(s) (shape=(0, 3)) while a minimum of 1 is required.", "yScore")]
    public void Multiclass_empty_inputs_get_the_reference_s_sentence(int samples, int scores, string message, string paramName)
    {
        // roc_auc_score([], one row of three) and ([0,1,2], an empty (0, 3)) raise check_array's sentences (#1585).
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => RocAuc.MultiClass([.. Enumerable.Range(0, samples)], new double[scores], 3));

        Assert.StartsWith(message, error.Message, StringComparison.Ordinal);
        Assert.Equal(paramName, error.ParamName);
    }

    [Fact]
    public void Weighted_average_precision_over_one_label_is_the_binary_score()
    {
        // An (n, 1) matrix is binary to scikit-learn: no shortcut, no (s·w)/w, and all-zero weights refused.
        bool[] yTrue = [true, false, true, false];
        double[] yScore = [.9, .6, .3, .1];
        double[] weights = [.7, 1.3, 2.9, .4];

        double matrix = AveragePrecision.Score(yTrue, yScore, 1, Averaging.Weighted, weights);

        Assert.Equal(AveragePrecision.Score([1, 0, 1, 0], yScore, 1, weights), matrix);
        Assert.Equal(0.7862811791383221, matrix, 1e-15);
        Assert.StartsWith("Sample weights must contain at least one non-zero number.", Assert.Throws<ArgumentException>(
            () => AveragePrecision.Score(yTrue, yScore, 1, Averaging.Weighted, [0.0, 0.0, 0.0, 0.0])).Message, StringComparison.Ordinal);
    }
}
