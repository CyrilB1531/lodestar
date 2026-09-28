using Xunit;

namespace Lodestar.Metrics.Tests;

/// <summary>
/// The review findings of 2026-09-26 and 2026-09-27 the oracle corpora cannot carry: refusals,
/// which a JSON case has no value for, and label counts too large to freeze.
/// </summary>
/// <remarks>Each expected message and value was read off scikit-learn 1.9.1 in <c>.venv-oracles</c>.</remarks>
public sealed class ReviewFindingTests
{
    [Fact]
    public void Macro_f1_over_65_536_labels_is_scored_rather_than_overflowing()
    {
        // m * m wrapped to 0 at 65,536 labels and threw; the counts are O(n + k) now (#1200).
        int[] labels = [.. Enumerable.Range(0, 65_536)];

        Assert.Equal(1.0, F1.Score(labels, labels, Averaging.Macro));
        Assert.Equal(1.0, Precision.Score(labels, labels, Averaging.Weighted));
        Assert.Equal(1.0, JaccardScore.Score(labels, labels, Averaging.Micro));
        Assert.Equal(65_536, ClassificationReport.Compute(labels, labels).Classes.Count);
    }

    [Fact]
    public void Binary_averaging_on_a_batch_without_the_positive_class_takes_the_zero_division_value()
    {
        // precision_score([0, 0, 0], [0, 0, 0]) is 0.0 through zero_division (#1201).
        int[] negatives = [0, 0, 0];

        Assert.Equal(0.0, Precision.Score(negatives, negatives));
        Assert.Equal(1.0, Recall.Score(negatives, negatives, zeroDivision: ZeroDivision.One));
        Assert.Equal(0.0, JaccardScore.Score(negatives, negatives));
    }

    [Fact]
    public void Binary_averaging_refuses_a_two_class_target_without_the_positive_label()
    {
        var error = Assert.Throws<ArgumentException>(() => Precision.Score([0, 2, 2], [0, 2, 0]));

        Assert.StartsWith("posLabel 1 does not occur in the data, which holds 0 and 2.", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Binary_averaging_ignores_the_labels_on_a_binary_target()
    {
        // precision_score([0,1,1,0], [0,1,0,1], labels=[1]) is 0.5, and so is labels=[0, 1, 2] (#1249).
        int[] yTrue = [0, 1, 1, 0];
        int[] yPred = [0, 1, 0, 1];

        Assert.Equal(0.5, Precision.Score(yTrue, yPred, labels: [1]));
        Assert.Equal(0.5, Precision.Score(yTrue, yPred, labels: [0, 1, 2]));
        Assert.Equal(0.8, F1.Score(yTrue, [0, 1, 1, 1], labels: [1, 2]), 12);
    }

    [Fact]
    public void A_requested_label_absent_from_the_truth_is_scored_by_the_precision_family()
    {
        // precision_recall_fscore_support scores it; confusion_matrix alone refuses it (#1202).
        int[] labels = [0, 1];

        Assert.Equal(0.0, Precision.Score(labels, labels, Averaging.Macro, labels: [5]));
        Assert.Throws<ArgumentException>(() => ConfusionMatrix.Compute(labels, labels, [5]));
        Assert.Throws<ArgumentException>(() => CohenKappa.Score(labels, labels, labels: [5]));
    }

    [Fact]
    public void A_single_class_truth_is_refused_by_the_detection_curve()
    {
        var error = Assert.Throws<ArgumentException>(() => DetCurve.Compute([0, 0, 0], [0.1, 0.5, 0.3]));

        Assert.StartsWith("Only one class is present in y_true.", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_truth_with_three_classes_is_refused_by_the_detection_curve()
    {
        // det_curve's test is len(np.unique(y_true)) != 2, so three classes are refused as one is.
        var error = Assert.Throws<ArgumentException>(() => DetCurve.Compute([0, 1, 2, 1], [0.1, 0.9, 0.5, 0.7]));

        Assert.StartsWith("Only one class is present in y_true.", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Curves_and_the_area_refuse_weights_that_are_zero_throughout()
    {
        int[] yTrue = [0, 1, 0];
        double[] scores = [0.1, 0.5, 0.3];
        double[] zeros = [0.0, 0.0, 0.0];

        foreach (Action score in new Action[]
        {
            () => RocCurve.Compute(yTrue, scores, sampleWeight: zeros),
            () => RocAuc.Score(yTrue, scores, sampleWeight: zeros),
        })
        {
            var error = Assert.Throws<ArgumentException>(score);
            Assert.StartsWith("Sample weights must contain at least one non-zero number.", error.Message, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Likelihood_ratios_refuse_a_two_class_target_without_the_positive_label()
    {
        var error = Assert.Throws<ArgumentException>(() => LikelihoodRatios.Compute([0, 2, 2, 0], [0, 2, 0, 2]));

        Assert.StartsWith("posLabel 1 does not occur in the data, which holds 0 and 2.", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void The_label_ranking_scores_refuse_a_score_that_is_not_finite()
    {
        bool[] yTrue = [true, false, true];
        double[] yScore = [double.NaN, 0.2, 0.3];

        foreach (Action score in new Action[]
        {
            () => CoverageError.Score(yTrue, yScore, 3),
            () => LabelRankingLoss.Score(yTrue, yScore, 3),
            () => LabelRankingAveragePrecision.Score(yTrue, yScore, 3),
        })
        {
            var error = Assert.Throws<ArgumentException>(score);
            Assert.StartsWith("Input contains NaN.", error.Message, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Reciprocal_rank_still_ranks_a_negative_infinity_last()
    {
        // No scikit-learn counterpart refuses it, and -inf is how a caller ranks a document out.
        Assert.Equal(1.0, ReciprocalRank.Score([0.0, 1.0, 0.0], [double.NegativeInfinity, 2.0, 1.0], 3));
    }

    [Fact]
    public void The_internal_validity_scores_refuse_a_feature_that_is_not_finite()
    {
        double[] features = [0.0, 1.0, double.NaN, 3.0];
        int[] labels = [0, 0, 1, 1];

        foreach (Action score in new Action[]
        {
            () => Silhouette.Score(labels, features, 1),
            () => CalinskiHarabasz.Score(labels, features, 1),
            () => DaviesBouldin.Score(labels, features, 1),
        })
        {
            var error = Assert.Throws<ArgumentException>(score);
            Assert.StartsWith("Input X contains NaN.", error.Message, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void A_precomputed_silhouette_refuses_a_diagonal_beyond_a_hundred_ulps()
    {
        double[] distances = [0, 1, 2, 3, 1, 1e-13, 1, 1, 2, 1, 0, 1, 3, 1, 1, 0];
        int[] labels = [0, 0, 1, 1];

        var error = Assert.Throws<ArgumentException>(() => Silhouette.ScoreFromDistances(labels, distances));
        Assert.StartsWith("The precomputed distance matrix contains non-zero elements on the diagonal.", error.Message, StringComparison.Ordinal);

        distances[5] = 0.0;
        distances[0] = 1e-14;
        Assert.Equal(0.35833333333333234, Silhouette.ScoreFromDistances(labels, distances), 12);
    }

    [Fact]
    public void Ndcg_refuses_a_score_that_is_not_finite()
    {
        var error = Assert.Throws<ArgumentException>(() => Ndcg.Score([1.0, 0.0, 2.0], [double.NaN, 1.0, 2.0], 3));

        Assert.StartsWith("Input contains NaN.", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Mean_squared_log_error_does_not_overflow_near_the_top_of_the_range()
    {
        // log(1 + x) * x overflowed past about 2.5e305 before dividing (#1206).
        Assert.Equal(0.0, MeanSquaredLogError.Score([1e306], [1e306]));
        Assert.Equal(190.86833197722387, MeanSquaredLogError.Score([1e306], [1e300]), 9);
        Assert.Equal(79.60764323266923, MeanSquaredLogError.Score([3e305, 1.0], [1e300, 2.0]), 9);
    }
}
