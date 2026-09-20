using Xunit;

namespace Lodestar.Metrics.Tests;

/// <summary>
/// What scikit-learn 1.9.1 refuses in a classification metric's weights and decisions,
/// each sentence measured against the reference rather than read off its source (#890).
/// </summary>
public sealed class ClassificationInputValidationTests
{
    private static readonly int[] Labels = [0, 1];
    private static readonly bool[] TrueMatrix = [true, false, false, true];
    private static readonly bool[] PredMatrix = [true, false, true, true];
    private static readonly double[] Proba = [0.2, 0.7];
    private static readonly double[] ProbaMatrix = [0.8, 0.2, 0.3, 0.7];

    /// <summary>Every entry point reaching <c>_check_sample_weight</c>, by name.</summary>
    private static readonly Dictionary<string, Action<double[]>> CheckedMetrics = new()
    {
        ["accuracy"] = w => Accuracy.Score(Labels, Labels, sampleWeight: w),
        ["accuracy_count"] = w => Accuracy.Score(Labels, Labels, normalize: false, sampleWeight: w),
        ["zero_one"] = w => ZeroOneLoss.Score(Labels, Labels, sampleWeight: w),
        ["zero_one_matrix"] = w => ZeroOneLoss.Score(TrueMatrix, PredMatrix, 2, sampleWeight: w),
        ["hamming"] = w => HammingLoss.Score(Labels, Labels, w),
        ["hamming_matrix"] = w => HammingLoss.Score(TrueMatrix, PredMatrix, 2, w),
        ["confusion_matrix"] = w => ConfusionMatrix.Compute(Labels, Labels, sampleWeight: w),
        ["multilabel_confusion_matrix"] = w => MultilabelConfusionMatrix.Compute(Labels, Labels, sampleWeight: w),
        ["multilabel_confusion_matrix_matrix"] =
            w => MultilabelConfusionMatrix.Compute(TrueMatrix, PredMatrix, 2, sampleWeight: w),
        ["matthews"] = w => MatthewsCorrelation.Score(Labels, Labels, sampleWeight: w),
        ["precision"] = w => Precision.Score(Labels, Labels, sampleWeight: w),
        ["recall"] = w => Recall.Score(Labels, Labels, sampleWeight: w),
        ["f1"] = w => F1.Score(Labels, Labels, sampleWeight: w),
        ["jaccard"] = w => JaccardScore.Score(Labels, Labels, sampleWeight: w),
        ["cohen_kappa"] = w => CohenKappa.Score(Labels, Labels, sampleWeight: w),
        ["balanced_accuracy"] = w => BalancedAccuracy.Score(Labels, Labels, sampleWeight: w),
        ["likelihood_ratios"] = w => LikelihoodRatios.Compute(Labels, Labels, sampleWeight: w),
        ["classification_report"] = w => ClassificationReport.Compute(Labels, Labels, sampleWeight: w),
        ["log_loss"] = w => LogLoss.Score(Labels, Proba, sampleWeight: w),
        ["log_loss_total"] = w => LogLoss.Score(Labels, Proba, normalize: false, sampleWeight: w),
        ["log_loss_matrix"] = w => LogLoss.MultiClass(Labels, ProbaMatrix, 2, sampleWeight: w),
        ["brier"] = w => BrierScore.Score(Labels, Proba, sampleWeight: w),
        ["brier_matrix"] = w => BrierScore.MultiClass(Labels, ProbaMatrix, 2, sampleWeight: w),
    };

    public static TheoryData<string> CheckedNames() => [.. CheckedMetrics.Keys];

    [Theory]
    [MemberData(nameof(CheckedNames))]
    public void A_weight_vector_that_is_zero_throughout_is_refused(string metric)
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => CheckedMetrics[metric]([0.0, 0.0]));

        Assert.Equal("sampleWeight", error.ParamName);
        Assert.StartsWith(
            "Sample weights must contain at least one non-zero number.", error.Message, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(CheckedNames))]
    public void A_non_finite_weight_is_refused_in_the_words_naming_sample_weight(string metric)
    {
        ArgumentException nan = Assert.Throws<ArgumentException>(() => CheckedMetrics[metric]([double.NaN, 1.0]));
        ArgumentException inf = Assert.Throws<ArgumentException>(
            () => CheckedMetrics[metric]([1.0, double.NegativeInfinity]));

        Assert.Equal("sampleWeight", nan.ParamName);
        Assert.StartsWith("Input sample_weight contains NaN.", nan.Message, StringComparison.Ordinal);
        Assert.Equal("sampleWeight", inf.ParamName);
        Assert.StartsWith("Input sample_weight contains infinity", inf.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("accuracy")]
    [InlineData("zero_one")]
    [InlineData("zero_one_matrix")]
    [InlineData("hamming")]
    [InlineData("log_loss")]
    [InlineData("log_loss_matrix")]
    [InlineData("brier")]
    [InlineData("brier_matrix")]
    public void Weights_summing_to_zero_are_refused_where_numpy_average_divides_by_them(string metric)
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => CheckedMetrics[metric]([1.0, -1.0]));

        Assert.Equal("sampleWeight", error.ParamName);
        Assert.StartsWith("Weights sum to zero, can't be normalized.", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Weights_summing_to_zero_are_scored_where_the_reference_never_divides_by_them()
    {
        // Measured on [0, 1] against itself with weights [1, -1]: each value below is scikit-learn's.
        double[] weights = [1.0, -1.0];

        Assert.Equal(0.0, Accuracy.Score(Labels, Labels, normalize: false, sampleWeight: weights));
        Assert.Equal(0.0, ZeroOneLoss.Score(Labels, Labels, normalize: false, sampleWeight: weights));
        Assert.Equal(-1.0, MatthewsCorrelation.Score(Labels, Labels, sampleWeight: weights));
        Assert.Equal(1.0, F1.Score(Labels, Labels, sampleWeight: weights));
        Assert.Equal(1.0, BalancedAccuracy.Score(Labels, Labels, sampleWeight: weights));
        Assert.True(double.IsNaN(CohenKappa.Score(Labels, Labels, sampleWeight: weights)));
        ConfusionMatrix cm = ConfusionMatrix.Compute(Labels, Labels, sampleWeight: weights);
        Assert.Equal(1.0, cm[0, 0]);
        Assert.Equal(-1.0, cm[1, 1]);
        // hamming_loss on a label matrix divides with numpy's `/`, not numpy.average.
        Assert.Equal(double.NegativeInfinity, HammingLoss.Score(TrueMatrix, PredMatrix, 2, weights));
    }

    [Fact]
    public void Hinge_loss_refuses_only_the_zero_sum_because_it_never_checks_its_weights()
    {
        // hinge_loss skips _check_sample_weight: all-zero reaches numpy.average's refusal,
        // and a NaN weight is scored, nan on both sides.
        int[] yTrue = [-1, 1];
        double[] decision = [-0.5, 0.3];

        double[][] refused = [[0.0, 0.0], [1.0, -1.0]];
        foreach (double[] weights in refused)
        {
            ArgumentException binary = Assert.Throws<ArgumentException>(
                () => HingeLoss.Score(yTrue, decision, sampleWeight: weights));
            ArgumentException multiclass = Assert.Throws<ArgumentException>(
                () => HingeLoss.MultiClass(Labels, ProbaMatrix, 2, weights));

            Assert.Equal("sampleWeight", binary.ParamName);
            Assert.StartsWith("Weights sum to zero", binary.Message, StringComparison.Ordinal);
            Assert.Equal("sampleWeight", multiclass.ParamName);
        }

        Assert.True(double.IsNaN(HingeLoss.Score(yTrue, decision, sampleWeight: [double.NaN, 1.0])));
        Assert.True(double.IsNaN(HingeLoss.MultiClass(Labels, ProbaMatrix, 2, [double.NaN, 1.0])));
    }

    [Theory]
    [InlineData(double.NaN, "Input contains NaN.")]
    [InlineData(double.PositiveInfinity, "Input contains infinity or a value too large for dtype('float64').")]
    public void Hinge_loss_refuses_a_decision_that_is_not_finite(double bad, string expected)
    {
        // Before #890 a NaN margin failed `cost > 0` and scored 0.
        ArgumentException binary = Assert.Throws<ArgumentException>(() => HingeLoss.Score([1], [bad]));
        ArgumentException multiclass = Assert.Throws<ArgumentException>(
            () => HingeLoss.MultiClass(Labels, [0.0, 1.0, bad, 0.0], 2));

        Assert.Equal("predDecision", binary.ParamName);
        Assert.StartsWith(expected, binary.Message, StringComparison.Ordinal);
        Assert.Equal("predDecision", multiclass.ParamName);
        Assert.StartsWith(expected, multiclass.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void The_binary_losses_refuse_a_third_label_rather_than_count_it_negative()
    {
        int[] yTrue = [0, 1, 2];
        double[] scores = [0.2, 0.7, 0.5];

        ArgumentException log = Assert.Throws<ArgumentException>(() => LogLoss.Score(yTrue, scores));
        ArgumentException brier = Assert.Throws<ArgumentException>(() => BrierScore.Score(yTrue, scores));
        ArgumentException hinge = Assert.Throws<ArgumentException>(() => HingeLoss.Score(yTrue, scores));

        Assert.All(new[] { log, brier, hinge }, e => Assert.Equal("yTrue", e.ParamName));
        Assert.StartsWith(
            "y_true and y_prob contain different number of classes: 3 vs 2.", log.Message, StringComparison.Ordinal);
        Assert.StartsWith(
            "The type of the target inferred from y_true is multiclass", brier.Message, StringComparison.Ordinal);
        Assert.StartsWith(
            "The shape of pred_decision cannot be 1d array with a multiclass target.",
            hinge.Message,
            StringComparison.Ordinal);
    }

    [Fact]
    public void Two_labels_neither_of_which_is_the_positive_one_are_still_binary()
    {
        // The refusal counts labels, not whether posLabel is among them: against posLabel 1
        // every sample is negative, costing 1.5 and 0.5 in turn.
        Assert.Equal(1.0, HingeLoss.Score([0, 2, 0, 2], [0.5, -0.5, 0.5, -0.5]), 12);
        Assert.Equal(0.0, BrierScore.Score([5, 5, 7], [0.0, 0.0, 0.0], posLabel: 1));
    }
}
