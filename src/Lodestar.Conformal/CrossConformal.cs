using Lodestar.Conformal.Internal;

namespace Lodestar.Conformal;

/// <summary>Cross-conformal regression: CV+, Jackknife+ and the jackknife-after-bootstrap, with no calibration set held out.</summary>
/// <remarks>
/// MAPIE 1.5.0's <c>CrossConformalRegressor</c> and <c>JackknifeAfterBootstrapRegressor</c>, over predictions rather
/// than models: model <c>m</c> is fitted without the samples its column of a held-out mask marks — a fold (CV+), a
/// sample (Jackknife+) or a bootstrap's out-of-bag samples. <b>The guarantee is <c>1 − 2α</c></b> (Barber et al.,
/// 2021), not split conformal's <c>1 − α</c>, and assumes exchangeability.
/// </remarks>
public static class CrossConformal
{
    /// <summary>Each training sample's out-of-sample prediction: the aggregate of the models fitted without it.</summary>
    /// <remarks>
    /// Score the result with <see cref="SplitConformal.AbsoluteResiduals"/> or <see cref="SplitConformal.GammaScores"/>.
    /// A sample no model holds out has no such prediction and gets <see cref="double.NaN"/>; the intervals skip it by
    /// the same mask, as MAPIE's <c>plus</c> does.
    /// </remarks>
    /// <param name="predictions">Every model's prediction at every training sample, row-major <c>n × M</c>.</param>
    /// <param name="heldOut">Row-major <c>n × M</c>: <see langword="true"/> where model <c>m</c> was fitted without sample <c>i</c>.</param>
    /// <param name="modelCount">The number of models, <c>M</c>.</param>
    /// <param name="aggregation">How several models' predictions combine; MAPIE's <c>aggregation_method</c>.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="modelCount"/> is not positive, or <paramref name="aggregation"/> is not declared.</exception>
    /// <exception cref="ArgumentException">A block's length is not a multiple of <paramref name="modelCount"/>, or the two differ.</exception>
    public static double[] OutOfSample(
        ReadOnlySpan<double> predictions,
        ReadOnlySpan<bool> heldOut,
        int modelCount,
        CrossConformalAggregation aggregation = CrossConformalAggregation.Mean)
    {
        CheckModels(modelCount);
        CheckAggregation(aggregation);
        if (predictions.Length % modelCount != 0 || heldOut.Length != predictions.Length)
        {
            throw new ArgumentException(
                $"{predictions.Length} predictions and {heldOut.Length} mask entries are not both n × {modelCount}.",
                nameof(heldOut));
        }

        int n = predictions.Length / modelCount;
        var outOfSample = new double[n];
        var buffer = new double[modelCount];
        for (int i = 0; i < n; i++)
        {
            outOfSample[i] = Aggregate(
                predictions.Slice(i * modelCount, modelCount), heldOut.Slice(i * modelCount, modelCount), aggregation, buffer);
        }

        return outOfSample;
    }

    /// <summary>The prediction interval at one test point for the absolute-residual score, the held-out models named by a mask.</summary>
    /// <remarks>
    /// <see cref="CrossConformalMethod.Plus"/> reads the <c>α</c> quantile of <c>ŷ₋ᵢ(x) − Rᵢ</c> and the <c>1 − α</c>
    /// quantile of <c>ŷ₋ᵢ(x) + Rᵢ</c> over the training samples, <c>ŷ₋ᵢ(x)</c> the aggregate at the test point of the
    /// models fitted without sample <c>i</c>; <see cref="CrossConformalMethod.MinMax"/> widens the smallest and largest
    /// of those by the scores' quantile. A sample no model holds out is skipped. A rank past the samples gives an
    /// infinite bound (decision 0007), where MAPIE raises.
    /// </remarks>
    /// <param name="testPredictions">Each model's prediction at the test point, <c>M</c> values.</param>
    /// <param name="heldOut">Row-major <c>n × M</c>, as <see cref="OutOfSample"/> takes it.</param>
    /// <param name="scores">The training samples' absolute residuals from their out-of-sample predictions.</param>
    /// <param name="alpha">Miscoverage level in <c>(0, 1)</c>.</param>
    /// <param name="method">Plus, the default, or min-max.</param>
    /// <param name="aggregation">How several held-out models' predictions combine at the test point.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alpha"/> is not in <c>(0, 1)</c>, or an enum value is not declared.</exception>
    /// <exception cref="ArgumentException">The lengths disagree, no sample is held out, a test prediction is not finite, or a held-out sample's score is NaN.</exception>
    public static (double Lower, double Upper) Interval(
        ReadOnlySpan<double> testPredictions,
        ReadOnlySpan<bool> heldOut,
        ReadOnlySpan<double> scores,
        double alpha,
        CrossConformalMethod method = CrossConformalMethod.Plus,
        CrossConformalAggregation aggregation = CrossConformalAggregation.Mean)
    {
        (double[] perSample, double[] kept) = FromMask(testPredictions, heldOut, scores, alpha, method, aggregation);
        return Absolute(perSample, kept, alpha, method);
    }

    /// <summary>The prediction interval at one test point for the absolute-residual score, one fold per training sample.</summary>
    /// <remarks>
    /// CV+ and Jackknife+, where each sample is held out by exactly one model: <paramref name="folds"/><c>[i]</c> names it.
    /// The same interval as the mask overload with one <see langword="true"/> per row.
    /// </remarks>
    /// <param name="testPredictions">Each model's prediction at the test point, <c>M</c> values.</param>
    /// <param name="folds">The model fitted without each training sample, in <c>[0, M)</c>.</param>
    /// <param name="scores">The training samples' absolute residuals from their out-of-sample predictions.</param>
    /// <param name="alpha">Miscoverage level in <c>(0, 1)</c>.</param>
    /// <param name="method">Plus, the default, or min-max.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alpha"/> is not in <c>(0, 1)</c>, a fold is outside <c>[0, M)</c>, or <paramref name="method"/> is not declared.</exception>
    /// <exception cref="ArgumentException">The lengths disagree, a test prediction is not finite, or a score is NaN.</exception>
    public static (double Lower, double Upper) Interval(
        ReadOnlySpan<double> testPredictions,
        ReadOnlySpan<int> folds,
        ReadOnlySpan<double> scores,
        double alpha,
        CrossConformalMethod method = CrossConformalMethod.Plus)
    {
        (double[] perSample, double[] kept) = FromFolds(testPredictions, folds, scores, alpha, method);
        return Absolute(perSample, kept, alpha, method);
    }

    /// <summary>The prediction interval at one test point for the gamma score, the held-out models named by a mask.</summary>
    /// <remarks>
    /// As the absolute overload, over <c>ŷ₋ᵢ(x)(1 + Rᵢ)</c>, each side at <c>α/2</c> since the score is not symmetric.
    /// Every held-out prediction at the test point must be strictly positive, as MAPIE requires.
    /// </remarks>
    /// <param name="testPredictions">Each model's prediction at the test point, <c>M</c> values.</param>
    /// <param name="heldOut">Row-major <c>n × M</c>, as <see cref="OutOfSample"/> takes it.</param>
    /// <param name="scores">The training samples' gamma scores from <see cref="SplitConformal.GammaScores"/>.</param>
    /// <param name="alpha">Miscoverage level in <c>(0, 1)</c>.</param>
    /// <param name="method">Plus, the default, or min-max.</param>
    /// <param name="aggregation">How several held-out models' predictions combine at the test point.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alpha"/> is not in <c>(0, 1)</c>, an enum value is not declared, or a held-out prediction is not strictly positive.</exception>
    /// <exception cref="ArgumentException">The lengths disagree, no sample is held out, a test prediction is not finite, or a held-out sample's score is NaN.</exception>
    public static (double Lower, double Upper) GammaInterval(
        ReadOnlySpan<double> testPredictions,
        ReadOnlySpan<bool> heldOut,
        ReadOnlySpan<double> scores,
        double alpha,
        CrossConformalMethod method = CrossConformalMethod.Plus,
        CrossConformalAggregation aggregation = CrossConformalAggregation.Mean)
    {
        (double[] perSample, double[] kept) = FromMask(testPredictions, heldOut, scores, alpha, method, aggregation);
        return Gamma(perSample, kept, alpha, method);
    }

    /// <summary>The prediction interval at one test point for the gamma score, one fold per training sample.</summary>
    /// <remarks>The fold overload of the gamma interval; see the absolute one.</remarks>
    /// <param name="testPredictions">Each model's prediction at the test point, <c>M</c> values.</param>
    /// <param name="folds">The model fitted without each training sample, in <c>[0, M)</c>.</param>
    /// <param name="scores">The training samples' gamma scores from <see cref="SplitConformal.GammaScores"/>.</param>
    /// <param name="alpha">Miscoverage level in <c>(0, 1)</c>.</param>
    /// <param name="method">Plus, the default, or min-max.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alpha"/> is not in <c>(0, 1)</c>, a fold is outside <c>[0, M)</c>, <paramref name="method"/> is not declared, or a prediction is not strictly positive.</exception>
    /// <exception cref="ArgumentException">The lengths disagree, a test prediction is not finite, or a score is NaN.</exception>
    public static (double Lower, double Upper) GammaInterval(
        ReadOnlySpan<double> testPredictions,
        ReadOnlySpan<int> folds,
        ReadOnlySpan<double> scores,
        double alpha,
        CrossConformalMethod method = CrossConformalMethod.Plus)
    {
        (double[] perSample, double[] kept) = FromFolds(testPredictions, folds, scores, alpha, method);
        return Gamma(perSample, kept, alpha, method);
    }

    private static (double Lower, double Upper) Absolute(double[] perSample, double[] scores, double alpha, CrossConformalMethod method)
    {
        int n = scores.Length;
        if (method == CrossConformalMethod.MinMax)
        {
            double q = RegressionQuantile.Upper(scores, 1.0 - alpha);
            return (perSample.Min() - q, perSample.Max() + q);
        }

        var low = new double[n];
        var high = new double[n];
        for (int i = 0; i < n; i++)
        {
            low[i] = perSample[i] + (-scores[i]);
            high[i] = perSample[i] + scores[i];
        }

        return (RegressionQuantile.Lower(low, alpha), RegressionQuantile.Upper(high, 1.0 - alpha));
    }

    private static (double Lower, double Upper) Gamma(double[] perSample, double[] scores, double alpha, CrossConformalMethod method)
    {
        for (int i = 0; i < perSample.Length; i++)
        {
            SplitConformal.GammaPositive(perSample[i], "testPredictions", i);
        }

        double beta = alpha / 2.0;
        double upperLevel = (1.0 - alpha) + beta;
        if (method == CrossConformalMethod.MinMax)
        {
            double low = RegressionQuantile.Lower((double[])scores.Clone(), beta);
            double high = RegressionQuantile.Upper(scores, upperLevel);
            return (perSample.Min() * (1.0 + low), perSample.Max() * (1.0 + high));
        }

        var distribution = new double[scores.Length];
        for (int i = 0; i < scores.Length; i++)
        {
            distribution[i] = perSample[i] * (1.0 + scores[i]);
        }

        return (RegressionQuantile.Lower((double[])distribution.Clone(), beta), RegressionQuantile.Upper(distribution, upperLevel));
    }

    /// <summary>Each held-out sample's aggregate prediction at the test point, and its score; samples no model holds out dropped.</summary>
    private static (double[] PerSample, double[] Scores) FromMask(
        ReadOnlySpan<double> testPredictions,
        ReadOnlySpan<bool> heldOut,
        ReadOnlySpan<double> scores,
        double alpha,
        CrossConformalMethod method,
        CrossConformalAggregation aggregation)
    {
        CheckCommon(testPredictions, alpha, method);
        CheckAggregation(aggregation);
        int modelCount = testPredictions.Length;
        int n = scores.Length;
        if (heldOut.Length != (long)n * modelCount)
        {
            throw new ArgumentException(
                $"The mask holds {heldOut.Length} entries where {n} scores and {modelCount} models need {(long)n * modelCount}.",
                nameof(heldOut));
        }

        var perSample = new List<double>(n);
        var kept = new List<double>(n);
        var buffer = new double[modelCount];
        for (int i = 0; i < n; i++)
        {
            ReadOnlySpan<bool> row = heldOut.Slice(i * modelCount, modelCount);
            if (row.IndexOf(true) < 0)
            {
                // No model was fitted without this sample: it has no out-of-sample score to calibrate with.
                continue;
            }

            perSample.Add(Aggregate(testPredictions, row, aggregation, buffer));
            kept.Add(Score(scores[i], i, nameof(scores)));
        }

        if (kept.Count == 0)
        {
            throw new ArgumentException("No training sample is held out by any model.", nameof(heldOut));
        }

        return ([.. perSample], [.. kept]);
    }

    private static (double[] PerSample, double[] Scores) FromFolds(
        ReadOnlySpan<double> testPredictions,
        ReadOnlySpan<int> folds,
        ReadOnlySpan<double> scores,
        double alpha,
        CrossConformalMethod method)
    {
        CheckCommon(testPredictions, alpha, method);
        if (folds.Length != scores.Length || scores.Length == 0)
        {
            throw new ArgumentException(
                $"There are {folds.Length} folds and {scores.Length} scores; both need one per training sample.",
                nameof(folds));
        }

        var perSample = new double[scores.Length];
        var kept = new double[scores.Length];
        for (int i = 0; i < scores.Length; i++)
        {
            int fold = folds[i];
            if (fold < 0 || fold >= testPredictions.Length)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(folds), fold, $"Fold {i} names model {fold} of {testPredictions.Length}.");
            }

            perSample[i] = testPredictions[fold];
            kept[i] = Score(scores[i], i, nameof(scores));
        }

        return (perSample, kept);
    }

    /// <summary>The aggregate of the predictions the mask row keeps; NaN when it keeps none.</summary>
    private static double Aggregate(
        ReadOnlySpan<double> predictions, ReadOnlySpan<bool> row, CrossConformalAggregation aggregation, double[] buffer)
    {
        int count = 0;
        double sum = 0.0;
        for (int m = 0; m < row.Length; m++)
        {
            if (row[m])
            {
                buffer[count++] = predictions[m];
                sum += predictions[m];
            }
        }

        if (count == 0)
        {
            return double.NaN;
        }

        if (aggregation == CrossConformalAggregation.Mean)
        {
            return sum / count;
        }

        Array.Sort(buffer, 0, count);
        int middle = count / 2;
        return count % 2 == 1 ? buffer[middle] : (buffer[middle - 1] + buffer[middle]) / 2.0;
    }

    private static double Score(double score, int index, string parameterName) =>
        double.IsNaN(score)
            ? throw new ArgumentException($"Score {index} is NaN, on a sample a model holds out.", parameterName)
            : score;

    private static void CheckCommon(ReadOnlySpan<double> testPredictions, double alpha, CrossConformalMethod method)
    {
        SplitConformal.CheckAlpha(alpha);
        CheckModels(testPredictions.Length);
        for (int m = 0; m < testPredictions.Length; m++)
        {
            if (double.IsNaN(testPredictions[m]) || double.IsInfinity(testPredictions[m]))
            {
                // A NaN, or an infinity less an infinite score, would reach the rank selection, whose comparisons are then no order.
                throw new ArgumentException($"Model {m}'s prediction at the test point is not finite.", nameof(testPredictions));
            }
        }

        if (method is not (CrossConformalMethod.Plus or CrossConformalMethod.MinMax))
        {
            throw new ArgumentOutOfRangeException(nameof(method), method, "The method must be Plus or MinMax.");
        }
    }

    private static void CheckModels(int modelCount)
    {
        if (modelCount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(modelCount), modelCount, "Cross-conformal prediction needs at least one model.");
        }
    }

    private static void CheckAggregation(CrossConformalAggregation aggregation)
    {
        if (aggregation is not (CrossConformalAggregation.Mean or CrossConformalAggregation.Median))
        {
            throw new ArgumentOutOfRangeException(nameof(aggregation), aggregation, "The aggregation must be Mean or Median.");
        }
    }
}
