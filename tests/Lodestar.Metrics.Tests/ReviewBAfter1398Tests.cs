using Xunit;

namespace Lodestar.Metrics.Tests;

/// <summary>
/// The Review B findings of <c>Lodestar.Metrics</c> after #1398, one fact or theory each; the expected values
/// were measured on scikit-learn 1.9.1.
/// </summary>
public sealed class ReviewBAfter1398Tests
{
    [Theory]
    [InlineData(double.NaN, "Input contains NaN.")]
    [InlineData(double.PositiveInfinity, "Input contains infinity or a value too large for dtype('float64').")]
    public void A_non_finite_output_weight_is_refused(double weight, string message)
    {
        // mean_squared_error(..., multioutput=[nan, 1]) raises; this returned NaN (#1461).
        ArgumentException twoOutputs = Assert.Throws<ArgumentException>(
            () => MeanSquaredError.Score([1, 2, 3, 4], [1, 2, 3, 5], outputCount: 2, outputWeights: [weight, 1]));
        ArgumentException oneOutput = Assert.Throws<ArgumentException>(
            () => MeanSquaredError.Score([1, 2], [1, 3], outputWeights: [weight]));

        Assert.StartsWith(message, twoOutputs.Message, StringComparison.Ordinal);
        Assert.Equal("outputWeights", oneOutput.ParamName);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void A_non_finite_tweedie_power_is_refused(double power)
    {
        // scikit-learn's parameter check refuses all three (#1462).
        Assert.Throws<ArgumentOutOfRangeException>(() => TweedieDeviance.Score([1.0, 2.0], [1.5, 2.5], power));
        Assert.Throws<ArgumentOutOfRangeException>(() => D2Tweedie.Score([1.0, 2.0], [1.5, 2.5], power));
    }

    [Fact]
    public void A_NaN_probability_is_binned_last_as_the_reference_bins_it()
    {
        // calibration_curve([0,1,1,0], [0.3, nan, 0.8, 0.1], n_bins=5) (#1464).
        CalibrationCurve curve = CalibrationCurve.Compute([0, 1, 1, 0], [0.3, double.NaN, 0.8, 0.1]);

        Assert.Equal([0.0, 0.0, 1.0, 1.0], curve.ProbTrue);
        Assert.Equal([0.1, 0.3, 0.8, double.NaN], curve.ProbPred);
    }

    [Fact]
    public void An_infinity_beside_a_NaN_passes_the_range_test_as_numpy_extremes_let_it()
    {
        // The NaN makes numpy's min and max NaN, so the infinity is binned rather than refused (#1464).
        CalibrationCurve curve = CalibrationCurve.Compute([0, 1, 1, 0], [0.3, double.NaN, 0.8, double.PositiveInfinity]);

        Assert.Equal([0.0, 1.0, 0.5], curve.ProbTrue);
        Assert.Equal([0.3, 0.8, double.NaN], curve.ProbPred);
    }

    [Fact]
    public void A_NaN_probability_makes_every_quantile_edge_NaN()
    {
        CalibrationCurve curve = CalibrationCurve.Compute(
            [0, 1, 1, 0], [0.3, double.NaN, 0.8, 0.1], nBins: 3, strategy: BinStrategy.Quantile);

        Assert.Equal([0.5], curve.ProbTrue);
        Assert.Equal([double.NaN], curve.ProbPred);
    }

    [Fact]
    public void An_infinity_alone_is_still_outside_the_unit_interval() =>
        Assert.Throws<ArgumentException>(() => CalibrationCurve.Compute([0, 1], [0.3, double.PositiveInfinity]));

    [Fact]
    public void Binary_log_loss_refuses_a_single_label_and_the_matrix_form_scores_it()
    {
        // log_loss([0,0], [0.2,0.3]) raises; with labels=[0,1] the matrix form gives 0.2899092476264711 (#1465).
        ArgumentException error = Assert.Throws<ArgumentException>(() => LogLoss.Score([0, 0], [0.2, 0.3]));

        Assert.Equal("yTrue", error.ParamName);
        Assert.Equal(0.2899092476264711, LogLoss.MultiClass([0, 0], [0.8, 0.2, 0.7, 0.3], classCount: 2), 1e-12);
    }

    [Theory]
    [InlineData(Averaging.Macro, 0.4444444444444444)]
    [InlineData(Averaging.Micro, 0.3333333333333333)]
    [InlineData(Averaging.Weighted, 0.3333333333333333)]
    public void An_infinite_beta_scores_the_recall(Averaging average, double expected)
    {
        // fbeta_score(beta=inf) returns recall_score; this refused the beta (#1466).
        int[] yTrue = [0, 1, 1, 1, 2, 0];
        int[] yPred = [1, 1, 0, 0, 2, 2];

        Assert.Equal(expected, FBeta.Score(yTrue, yPred, double.PositiveInfinity, average), 1e-12);
        Assert.Equal(Recall.Score(yTrue, yPred, average: average), FBeta.Score(yTrue, yPred, double.PositiveInfinity, average));
    }

    [Fact]
    public void A_confusion_table_past_the_largest_array_is_refused_before_allocating()
    {
        // 65,536 labels wrapped m * m to 0 and failed on an index (#1467).
        int[] yTrue = [.. Enumerable.Range(0, 32_768)];
        int[] yPred = [.. Enumerable.Range(32_768, 32_768)];

        Assert.Throws<ArgumentException>(() => ConfusionMatrix.Compute(yTrue, yPred));
    }

    [Fact]
    public void A_cluster_table_past_the_largest_array_is_refused_before_allocating()
    {
        // 65,536 clusters wrapped clusters * clusters to 0 (#1468).
        int[] labels = [.. Enumerable.Range(0, 65_536), 0];
        double[] features = [.. Enumerable.Range(0, 65_537).Select(i => (double)i)];

        Assert.Throws<ArgumentException>(() => DaviesBouldin.Score(labels, features, 1));
    }

    [Fact]
    public void A_feature_block_whose_shape_wraps_in_int_is_refused()
    {
        // 65,536 × 65,537 wraps to 65,536 in int, the block's length: it passed and failed on an index (#1470).
        int[] labels = [.. Enumerable.Range(0, 65_536).Select(i => i % 2)];
        double[] features = new double[65_536];

        ArgumentException error = Assert.Throws<ArgumentException>(() => Silhouette.Score(labels, features, 65_537));
        Assert.Equal("features", error.ParamName);
    }

    [Fact]
    public void A_curve_cannot_be_rewritten_through_a_cast()
    {
        RocCurve curve = RocCurve.Compute([0, 1, 1, 0], [0.1, 0.8, 0.6, 0.3]);

        // Cast back to double[], the arrays let a caller rewrite the curve (#1473).
        Assert.False(curve.Thresholds is double[]);
        Assert.False(curve.FalsePositiveRate is double[]);
        Assert.False(PrecisionRecallCurve.Compute([0, 1, 1, 0], [0.1, 0.8, 0.6, 0.3]).Precision is double[]);
        Assert.False(DetCurve.Compute([0, 1, 1, 0], [0.1, 0.8, 0.6, 0.3]).FalseNegativeRate is double[]);
        Assert.False(CalibrationCurve.Compute([0, 1, 1, 0], [0.1, 0.8, 0.6, 0.3]).ProbPred is double[]);
    }
}
