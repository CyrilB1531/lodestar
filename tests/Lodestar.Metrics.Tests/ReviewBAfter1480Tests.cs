using Xunit;

namespace Lodestar.Metrics.Tests;

/// <summary>
/// The Review B findings of <c>Lodestar.Metrics</c> after #1480, one fact or theory each; the expected values
/// were measured on scikit-learn 1.9.1.
/// </summary>
public sealed class ReviewBAfter1480Tests
{
    [Fact]
    public void Custom_weights_on_one_output_are_refused_except_by_the_root_mean_squared_error()
    {
        // mean_squared_error(..., multioutput=[2.0]) raises; root_mean_squared_error scores 0.5773502691896257 (#1533).
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => MeanSquaredError.Score([1, 2, 3], [1, 2, 4], outputWeights: [2.0]));
        Assert.Equal("Custom weights are useful only in multi-output cases. (Parameter 'outputWeights')", error.Message);
        Assert.Equal(error.Message, Assert.Throws<ArgumentException>(
            () => MedianAbsoluteError.Score([1, 2, 3], [1, 2, 4], outputWeights: [2.0])).Message);
        Assert.Equal(0.5773502691896257, RootMeanSquaredError.Score([1, 2, 3], [1, 2, 4], outputWeights: [2.0]), 1e-12);
    }

    [Fact]
    public void Sample_weights_summing_to_zero_are_refused_except_by_the_median()
    {
        // numpy.average raises; median_absolute_error never divides by the total and scores (#1273).
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => MeanSquaredError.Score([1, 2, 3], [1, 2, 4], sampleWeight: [1, -1, 0]));
        Assert.Equal("sampleWeight", error.ParamName);
        Assert.Equal(error.Message, Assert.Throws<ArgumentException>(
            () => R2.Score([1, 2, 3], [1, 2, 4], sampleWeight: [1, -1, 0])).Message);
        // median_absolute_error([1,2,3], [1,2,4], sample_weight=[1,-1,0]) is 1.0 (#1546).
        Assert.Equal(1.0, MedianAbsoluteError.Score([1, 2, 3], [1, 2, 4], sampleWeight: [1, -1, 0]));
    }

    [Fact]
    public void Weighted_average_precision_answers_zero_when_the_weighted_positives_vanish()
    {
        bool[] yTrue = [true, false, false, true];
        double[] yScore = [0.9, 0.2, 0.4, 0.6];

        // _average_binary_score returns 0 at isclose(total, 0), before scoring a label (#1534).
        Assert.Equal(0.0, AveragePrecision.Score(yTrue, yScore, 2, Averaging.Weighted, [0.0, 0.0]));
        Assert.Equal(0.0, AveragePrecision.Score(yTrue, yScore, 2, Averaging.Weighted, [1e-9, 1e-9]));
    }

    [Fact]
    public void Weighted_one_vs_rest_roc_auc_answers_zero_when_the_weights_vanish()
    {
        int[] yTrue = [0, 1, 2, 0, 1, 2];
        double[] yScore = [.7, .2, .1, .1, .8, .1, .2, .2, .6, .5, .3, .2, .3, .5, .2, .1, .3, .6];
        var options = new MultiClassRocOptions { Average = Averaging.Weighted, SampleWeight = [1e-9, 1e-9, 1e-9, 1e-9, 1e-9, 1e-9] };

        Assert.Equal(0.0, RocAuc.MultiClass(yTrue, yScore, 3, options));
    }

    [Fact]
    public void Binary_roc_auc_reads_the_labels_present()
    {
        double[] yScore = [0.1, 0.8, 0.6, 0.3];

        // A third label is refused, one label and a zero-weighted class are NaN, as roc_auc_score answers (#1277).
        Assert.Equal("yTrue", Assert.Throws<ArgumentException>(() => RocAuc.Score([0, 1, 2, 1], yScore)).ParamName);
        Assert.True(double.IsNaN(RocAuc.Score([1, 1, 1, 1], yScore)));
        Assert.True(double.IsNaN(RocAuc.Score([0, 1, 0, 1], yScore, 1, [1, 0, 1, 0])));
    }

    [Fact]
    public void A_two_label_target_without_posLabel_is_refused_where_the_reference_takes_no_explicit_one()
    {
        int[] yTrue = [0, 2, 2, 0];
        double[] p = [0.1, 0.9, 0.8, 0.3];

        // average_precision_score: "pos_label=1 is not a valid label. It should be one of [0 2]" (#1277, #1542).
        Assert.Equal("posLabel", Assert.Throws<ArgumentException>(() => AveragePrecision.Score(yTrue, p)).ParamName);
        Assert.Equal("posLabel", Assert.Throws<ArgumentException>(() => RocAuc.Score(yTrue, p)).ParamName);
        Assert.Equal("posLabel", Assert.Throws<ArgumentException>(() => LogLoss.Score(yTrue, p)).ParamName);

        // An explicit pos_label is accepted by brier_score_loss and calibration_curve: 0.3875 and all zeros.
        Assert.Equal(0.38750000000000007, BrierScore.Score(yTrue, p), 1e-12);
        Assert.Equal([0.0, 0.0], CalibrationCurve.Compute(yTrue, p, nBins: 2).ProbTrue);
        Assert.Equal(0.0, AveragePrecision.Score(yTrue, p, posLabel: 2) - 1.0, 1e-12);
    }

    [Fact]
    public void A_nBins_past_one_array_is_refused()
    {
        // nBins + 1 wrapped inside linspace (#1538).
        Assert.Equal("nBins", Assert.Throws<ArgumentOutOfRangeException>(
            () => CalibrationCurve.Compute([0, 1], [0.2, 0.8], nBins: int.MaxValue)).ParamName);
    }

    [Fact]
    public void A_finite_beta_whose_square_overflows_is_refused()
    {
        // fbeta_score raises OverflowError on beta**2 at 1e200 (#1540).
        Assert.Equal("beta", Assert.Throws<ArgumentOutOfRangeException>(() => FBeta.Score([0, 1, 1], [0, 1, 0], 1e200)).ParamName);
    }

    [Theory]
    [InlineData(double.NaN, "Input contains NaN.")]
    [InlineData(double.PositiveInfinity, "Input contains infinity or a value too large for dtype('float64').")]
    public void A_non_finite_probability_gets_the_reference_s_sentence(double value, string message)
    {
        // It read as a range violation (#1541).
        Assert.StartsWith(message, Assert.Throws<ArgumentException>(() => LogLoss.Score([0, 1], [0.2, value])).Message, StringComparison.Ordinal);
        Assert.StartsWith(message, Assert.Throws<ArgumentException>(() => BrierScore.Score([0, 1], [0.2, value])).Message, StringComparison.Ordinal);
    }

    [Fact]
    public void One_vs_one_pairs_past_one_array_are_refused_under_classCount()
    {
        // Reached only past 46,341 classes, which a public call would pay seconds to build; the bound is read directly.
        System.Reflection.MethodInfo pairs = typeof(RocAuc).Assembly
            .GetType("Lodestar.Metrics.Internal.MultiClassRoc", throwOnError: true)!
            .GetMethod("Pairs", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)!;

        // 65,537 classes make 2,147,516,416 pairs, past one array; k * (k - 1) overflowed int (#1469, #1544).
        var error = Assert.Throws<System.Reflection.TargetInvocationException>(() => pairs.Invoke(null, [65_537, "classCount"]));

        Assert.Equal("classCount", Assert.IsType<ArgumentException>(error.InnerException).ParamName);
    }

    [Fact]
    public void A_class_labels_name_and_no_sample_carries_is_left_out_as_the_reference_leaves_it()
    {
        int[] yTrue = [0, 1, 0, 1, 0, 1];
        double[] yScore = [.7, .2, .1, .2, .7, .1, .6, .3, .1, .3, .6, .1, .5, .4, .1, .4, .5, .1];

        // One-vs-one pairs only the classes present, and weighted one-vs-rest zeroes a class of no weight: 1.0 both.
        Assert.Equal(1.0, RocAuc.MultiClass(
            yTrue, yScore, 3, new MultiClassRocOptions { Strategy = MultiClassStrategy.OneVsOne, Labels = [0, 1, 2] }));
        Assert.Equal(1.0, RocAuc.MultiClass(
            yTrue, yScore, 3, new MultiClassRocOptions { Average = Averaging.Weighted, Labels = [0, 1, 2] }));
    }

    [Fact]
    public void Weighted_one_vs_rest_zeroes_a_class_of_no_weight()
    {
        int[] yTrue = [0, 1, 2, 0, 1, 2];
        double[] yScore = [.7, .2, .1, .2, .7, .1, .2, .2, .6, .5, .3, .2, .3, .5, .2, .1, .3, .6];

        Assert.Equal(1.0, RocAuc.MultiClass(
            yTrue, yScore, 3, new MultiClassRocOptions { Average = Averaging.Weighted, SampleWeight = [0, 0, 1, 1, 1, 1] }));
    }

    [Fact]
    public void A_NaN_score_is_refused_before_the_zero_weight_shortcut()
    {
        int[] yTrue = [0, 1, 2, 0, 1, 2];
        double[] yScore = [double.NaN, .2, .1, .2, .7, .1, .2, .2, .6, .5, .3, .2, .3, .5, .2, .1, .3, .6];

        // check_array raises "Input contains NaN." before _average_binary_score's shortcut.
        ArgumentException error = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass(
            yTrue, yScore, 3, new MultiClassRocOptions { Average = Averaging.Weighted, SampleWeight = [1e-9, 1e-9, 1e-9, 1e-9, 1e-9, 1e-9] }));
        // Refused up front in check_array's words since #1569, not by a class curve naming a compacted index.
        Assert.StartsWith("Input contains NaN.", error.Message, StringComparison.Ordinal);
        Assert.Equal("yScore", error.ParamName);
    }

    [Fact]
    public void The_root_mean_squared_error_averages_a_NaN_output_weight_into_NaN()
    {
        // root_mean_squared_error never checks multioutput: nan.
        Assert.True(double.IsNaN(RootMeanSquaredError.Score([1, 2, 3, 4], [1, 2, 3, 5], outputCount: 2, outputWeights: [double.NaN, 1])));
    }
}
