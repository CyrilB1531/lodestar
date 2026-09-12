namespace Lodestar.Conformal;

/// <summary>Split conformal prediction: an interval instead of a point, a set instead of a class.</summary>
/// <remarks>
/// Reproduces MAPIE 1.5.0's <c>SplitConformalRegressor</c> and its
/// <c>SplitConformalClassifier</c> at <c>conformity_score="lac"</c>, both <c>prefit</c>;
/// every member is static and thread-safe. <b>The guarantee assumes exchangeability</b> —
/// it does not hold for time series, for drift, or for a split that leaks, the intervals
/// still come out, and nothing in the output says so. See <c>docs/guides/conformal.md</c>.
/// </remarks>
public static class SplitConformal
{
    /// <summary>The score a new point must not exceed to fall inside the prediction.</summary>
    /// <remarks>
    /// The <c>k</c>-th smallest score, <c>k = ceil((n + 1) · (1 − alpha))</c>, 1-based — the rule
    /// MAPIE was measured to follow, and not a numpy quantile. When <c>k</c> exceeds the score
    /// count the level asks for a score that does not exist, and the answer is
    /// <see cref="double.PositiveInfinity"/>: a trivial prediction with real coverage, carried
    /// through by <see cref="Interval"/> and <see cref="PredictionSet"/>. MAPIE raises there;
    /// decision 0070 says why this does not. <b>Exchangeability</b> — see the type's remarks.
    /// </remarks>
    /// <param name="scores">The calibration scores; not modified.</param>
    /// <param name="alpha">Miscoverage level in <c>(0, 1)</c>: 0.1 asks for 90 % coverage.</param>
    /// <exception cref="ArgumentException"><paramref name="scores"/> is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alpha"/> is not in <c>(0, 1)</c>.</exception>
    public static double Quantile(ReadOnlySpan<double> scores, double alpha)
    {
        if (scores.Length == 0)
        {
            throw new ArgumentException("Conformal calibration needs at least one score.", nameof(scores));
        }
        // NaN spelled out rather than left to a negated comparison: `!(a > 0 && a < 1)`
        // rejects it and `a <= 0 || a >= 1` accepts it, and the reader should not have to know.
        if (double.IsNaN(alpha) || alpha <= 0.0 || alpha >= 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(alpha), alpha, "The miscoverage level must lie strictly between 0 and 1.");
        }

        int n = scores.Length;
        int k = (int)Math.Ceiling((n + 1) * (1.0 - alpha));
        if (k > n)
        {
            return double.PositiveInfinity;
        }

        // Sorted rather than selected: n is the calibration size, which is small next to the
        // predictions the quantile is then applied to, and a copy keeps the caller's span intact.
        double[] sorted = scores.ToArray();
        Array.Sort(sorted);
        return sorted[k - 1];
    }

    /// <summary>The absolute-residual calibration scores of a regressor, <c>|y − ŷ|</c>.</summary>
    /// <remarks>
    /// MAPIE's <c>AbsoluteConformityScore</c>, which is what <c>SplitConformalRegressor</c>
    /// uses by default. Hand this to <see cref="Quantile"/>.
    /// </remarks>
    /// <param name="yTrue">The observed values.</param>
    /// <param name="yPredicted">The model's predictions, same length as <paramref name="yTrue"/>.</param>
    /// <exception cref="ArgumentException">The two spans have different lengths.</exception>
    public static double[] AbsoluteResiduals(ReadOnlySpan<double> yTrue, ReadOnlySpan<double> yPredicted)
    {
        if (yTrue.Length != yPredicted.Length)
        {
            throw new ArgumentException(
                $"There are {yTrue.Length} observed values but {yPredicted.Length} predictions.",
                nameof(yPredicted));
        }

        double[] residuals = new double[yTrue.Length];
        for (int i = 0; i < yTrue.Length; i++)
        {
            residuals[i] = Math.Abs(yTrue[i] - yPredicted[i]);
        }
        return residuals;
    }

    /// <summary>The normalised calibration scores of a regressor, <c>|y − ŷ| / r̂</c>.</summary>
    /// <remarks>
    /// MAPIE's <c>ResidualNormalisedScore</c>. <paramref name="residualEstimates"/> is a second
    /// model's prediction of <c>|y − ŷ|</c> at each calibration point, and dividing by it is what
    /// makes the interval width vary with the input — <see cref="AbsoluteResiduals"/> gives every
    /// point the same width, which is a real limitation and not a simplification. Hand the result
    /// to <see cref="Quantile"/> and the same estimates to <see cref="NormalisedInterval"/>.
    /// <b>Exchangeability</b> — see the type's remarks.
    /// </remarks>
    /// <param name="yTrue">The observed values.</param>
    /// <param name="yPredicted">The model's predictions, same length as <paramref name="yTrue"/>.</param>
    /// <param name="residualEstimates">The predicted absolute residual at each point, all strictly positive.</param>
    /// <exception cref="ArgumentException">The spans have different lengths.</exception>
    /// <exception cref="ArgumentOutOfRangeException">An estimate is not strictly positive, or is NaN.</exception>
    public static double[] NormalisedResiduals(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPredicted,
        ReadOnlySpan<double> residualEstimates)
    {
        if (yTrue.Length != yPredicted.Length || yTrue.Length != residualEstimates.Length)
        {
            throw new ArgumentException(
                $"There are {yTrue.Length} observed values, {yPredicted.Length} predictions and "
                + $"{residualEstimates.Length} residual estimates.",
                nameof(residualEstimates));
        }

        double[] scores = new double[yTrue.Length];
        for (int i = 0; i < yTrue.Length; i++)
        {
            scores[i] = Math.Abs(yTrue[i] - yPredicted[i]) / Positive(residualEstimates[i], i);
        }
        return scores;
    }

    /// <summary>The normalised interval <c>[ŷ − q·r̂, ŷ + q·r̂]</c> around a point prediction.</summary>
    /// <remarks>
    /// The quantile comes from <see cref="Quantile"/> over <see cref="NormalisedResiduals"/>, and
    /// <paramref name="residualEstimate"/> from the same second model, at the point being
    /// predicted. An infinite <paramref name="quantile"/> yields the whole line, as
    /// <see cref="Interval"/> does. <b>Exchangeability</b> — see the type's remarks.
    /// </remarks>
    /// <param name="prediction">The model's point prediction.</param>
    /// <param name="residualEstimate">The predicted absolute residual at this point, strictly positive.</param>
    /// <param name="quantile">The calibrated quantile from <see cref="Quantile"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="quantile"/> is negative or NaN, or <paramref name="residualEstimate"/> is not strictly positive.</exception>
    public static (double Lower, double Upper) NormalisedInterval(
        double prediction, double residualEstimate, double quantile)
    {
        if (double.IsNaN(quantile) || quantile < 0.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantile), quantile, "A calibrated quantile is a non-negative score.");
        }

        double width = quantile * Positive(residualEstimate, null);
        return (prediction - width, prediction + width);
    }

    /// <summary>A residual estimate, refused unless it is strictly positive.</summary>
    /// <remarks>
    /// MAPIE floors it at 1e-8 instead, because its own residual model may predict a negative
    /// and it has nowhere to send the complaint. Here the estimate is the caller's own argument,
    /// so flooring would turn their bug into an interval of width <c>q · 1e-8</c> — which reads
    /// as certainty. Decision 0118 has why this diverges.
    /// </remarks>
    private static double Positive(double estimate, int? index)
    {
        if (double.IsNaN(estimate) || estimate <= 0.0)
        {
            string where = index is null ? "The residual estimate" : $"Residual estimate {index}";
            throw new ArgumentOutOfRangeException(
                nameof(estimate), estimate,
                $"{where} is not strictly positive, and the score divides by it.");
        }

        return estimate;
    }

    /// <summary>The prediction interval <c>[ŷ − q, ŷ + q]</c> around a point prediction.</summary>
    /// <remarks>
    /// An infinite <paramref name="quantile"/> — see <see cref="Quantile"/> — yields the whole
    /// line, which is the trivial prediction the calibration size forced.
    /// <b>The guarantee assumes exchangeability</b>; see the type's remarks.
    /// </remarks>
    /// <param name="prediction">The model's point prediction.</param>
    /// <param name="quantile">The calibrated quantile from <see cref="Quantile"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="quantile"/> is negative or NaN.</exception>
    public static (double Lower, double Upper) Interval(double prediction, double quantile)
    {
        if (double.IsNaN(quantile) || quantile < 0.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantile), quantile, "A calibrated quantile is a non-negative score.");
        }

        return (prediction - quantile, prediction + quantile);
    }

    /// <summary>The LAC calibration scores of a classifier, <c>1 − p̂(true class)</c>.</summary>
    /// <remarks>
    /// MAPIE's <c>conformity_score="lac"</c>. <paramref name="probabilities"/> is row-major,
    /// one row per calibration sample and <paramref name="classCount"/> values each, in the
    /// same class order <see cref="PredictionSet"/> will be given.
    /// </remarks>
    /// <param name="probabilities">The predicted probabilities, row-major.</param>
    /// <param name="labels">The index of each sample's true class.</param>
    /// <param name="classCount">How many classes each row holds.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="classCount"/> is not positive.</exception>
    /// <exception cref="ArgumentException">The shapes disagree, or a label is outside the class range.</exception>
    public static double[] LeastAmbiguousScores(
        ReadOnlySpan<double> probabilities, ReadOnlySpan<int> labels, int classCount)
    {
        if (classCount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(classCount), classCount, "A classifier has at least one class.");
        }
        if (probabilities.Length != labels.Length * classCount)
        {
            throw new ArgumentException(
                $"{labels.Length} samples of {classCount} classes need {labels.Length * classCount} "
                    + $"probabilities, not {probabilities.Length}.",
                nameof(probabilities));
        }

        double[] scores = new double[labels.Length];
        for (int i = 0; i < labels.Length; i++)
        {
            int label = labels[i];
            if (label < 0 || label >= classCount)
            {
                throw new ArgumentException(
                    $"Sample {i} has class {label}, outside [0, {classCount}).", nameof(labels));
            }
            scores[i] = 1.0 - probabilities[(i * classCount) + label];
        }
        return scores;
    }

    /// <summary>The prediction set: every class whose probability clears <c>1 − q</c>.</summary>
    /// <remarks>
    /// MAPIE's LAC rule, edges included. <b>The set can be empty</b>, when no class clears
    /// the threshold; substituting the most likely class there would return something with
    /// no coverage guarantee under a name that promises one. An infinite
    /// <paramref name="quantile"/> returns every class, the trivial prediction.
    /// <b>The guarantee assumes exchangeability</b>; see the type's remarks.
    /// </remarks>
    /// <param name="probabilities">One sample's predicted probabilities, in calibration order.</param>
    /// <param name="quantile">The calibrated quantile from <see cref="Quantile"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="quantile"/> is negative or NaN.</exception>
    public static bool[] PredictionSet(ReadOnlySpan<double> probabilities, double quantile)
    {
        if (double.IsNaN(quantile) || quantile < 0.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantile), quantile, "A calibrated quantile is a non-negative score.");
        }

        double threshold = 1.0 - quantile;
        bool[] included = new bool[probabilities.Length];
        for (int i = 0; i < probabilities.Length; i++)
        {
            included[i] = probabilities[i] >= threshold;
        }
        return included;
    }
}
