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
    /// <summary>The score a new point must not exceed to fall inside the prediction, by the ceiling rule.</summary>
    /// <remarks>
    /// <see cref="Quantile(ReadOnlySpan{double}, double, ConformalQuantileRule)"/> at
    /// <see cref="ConformalQuantileRule.Ceiling"/>, the rule MAPIE's regressors follow.
    /// </remarks>
    /// <param name="scores">The calibration scores; not modified.</param>
    /// <param name="alpha">Miscoverage level in <c>(0, 1)</c>: 0.1 asks for 90 % coverage.</param>
    /// <exception cref="ArgumentException"><paramref name="scores"/> is empty or holds a NaN.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alpha"/> is not in <c>(0, 1)</c>.</exception>
    public static double Quantile(ReadOnlySpan<double> scores, double alpha) =>
        Quantile(scores, alpha, ConformalQuantileRule.Ceiling);

    /// <summary>The score a new point must not exceed to fall inside the prediction.</summary>
    /// <remarks>
    /// <see cref="ConformalQuantileRule.Ceiling"/> reads the <c>k</c>-th smallest score, <c>k = ceil((n + 1) · (1 − alpha))</c>;
    /// <see cref="ConformalQuantileRule.MapieClassification"/> the <c>(ceil((n − 1) · level) + 1)</c>-th, <c>level = (n + 1)(1 − alpha) / n</c>,
    /// which is what MAPIE's prediction sets read. When the rule asks for a score that does not exist the answer is
    /// <see cref="double.PositiveInfinity"/>: a trivial prediction with real coverage, carried through by
    /// <see cref="Interval"/> and <see cref="PredictionSet"/>. MAPIE raises there; decision 0070 says why this does not.
    /// <b>Exchangeability</b> — see the type's remarks.
    /// </remarks>
    /// <param name="scores">The calibration scores; not modified.</param>
    /// <param name="alpha">Miscoverage level in <c>(0, 1)</c>: 0.1 asks for 90 % coverage.</param>
    /// <param name="rule">Which order statistic to read.</param>
    /// <exception cref="ArgumentException"><paramref name="scores"/> is empty or holds a NaN.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alpha"/> is not in <c>(0, 1)</c>, or <paramref name="rule"/> is not a declared value.</exception>
    public static double Quantile(ReadOnlySpan<double> scores, double alpha, ConformalQuantileRule rule)
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
        if (rule is not (ConformalQuantileRule.Ceiling or ConformalQuantileRule.MapieClassification))
        {
            throw new ArgumentOutOfRangeException(
                nameof(rule), rule, "The quantile rule must be Ceiling or MapieClassification.");
        }

        // NaN sorts first and would move every rank down one; MAPIE's regressor drops it, this refuses it (#889).
        for (int i = 0; i < scores.Length; i++)
        {
            if (double.IsNaN(scores[i]))
            {
                throw new ArgumentException($"Calibration score {i} is NaN.", nameof(scores));
            }
        }

        int n = scores.Length;
        int k = rule == ConformalQuantileRule.Ceiling ? CeilingRank(n, alpha) : HigherRank(n, alpha);
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

    /// <summary>The 1-based rank of the ceiling rule.</summary>
    private static int CeilingRank(int n, double alpha) => (int)Math.Ceiling((n + 1) * (1.0 - alpha));

    /// <summary>The 1-based rank numpy's <c>higher</c> quantile reads at MAPIE's classification level, past <paramref name="n"/> where the level passes 1.</summary>
    /// <remarks>Evaluated in numpy's order, level first, so a product near an integer rounds as it does there.</remarks>
    private static int HigherRank(int n, double alpha)
    {
        double level = (n + 1) * (1.0 - alpha) / n;
        return level > 1.0 ? n + 1 : (int)Math.Ceiling((n - 1) * level) + 1;
    }

    /// <summary>The absolute-residual calibration scores of a regressor, <c>|y − ŷ|</c>.</summary>
    /// <remarks>
    /// MAPIE's <c>AbsoluteConformityScore</c>, which is what <c>SplitConformalRegressor</c>
    /// uses by default. Hand this to <see cref="Quantile(ReadOnlySpan{double}, double)"/>.
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
    /// to <see cref="Quantile(ReadOnlySpan{double}, double)"/> and the same estimates to <see cref="NormalisedInterval"/>.
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
    /// The quantile comes from <see cref="Quantile(ReadOnlySpan{double}, double)"/> over <see cref="NormalisedResiduals"/>, and
    /// <paramref name="residualEstimate"/> from the same second model, at the point being
    /// predicted. An infinite <paramref name="quantile"/> yields the whole line, as
    /// <see cref="Interval"/> does. <b>Exchangeability</b> — see the type's remarks.
    /// </remarks>
    /// <param name="prediction">The model's point prediction.</param>
    /// <param name="residualEstimate">The predicted absolute residual at this point, strictly positive.</param>
    /// <param name="quantile">The calibrated quantile from <see cref="Quantile(ReadOnlySpan{double}, double)"/>.</param>
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
    /// An infinite <paramref name="quantile"/> — see <see cref="Quantile(ReadOnlySpan{double}, double)"/> — yields the whole
    /// line, which is the trivial prediction the calibration size forced.
    /// <b>The guarantee assumes exchangeability</b>; see the type's remarks.
    /// </remarks>
    /// <param name="prediction">The model's point prediction.</param>
    /// <param name="quantile">The calibrated quantile from <see cref="Quantile(ReadOnlySpan{double}, double)"/>.</param>
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

    /// <summary>How far below <c>1 − q</c> a class may fall and stay in the set: MAPIE's <c>EPSILON</c>, from <c>mapie/_machine_precision.py</c>.</summary>
    private const double InclusionTolerance = 1e-8;

    /// <summary>The prediction set: every class whose probability clears <c>1 − q</c>, to within 1e-8.</summary>
    /// <remarks>
    /// MAPIE's LAC rule, edges included, when the quantile was taken at
    /// <see cref="ConformalQuantileRule.MapieClassification"/>; the default rule can read one rank lower.
    /// <b>The set can be empty</b>, when no class clears the threshold; substituting the most likely class there would return something with
    /// no coverage guarantee under a name that promises one. An infinite
    /// <paramref name="quantile"/> returns every class, the trivial prediction.
    /// <b>The guarantee assumes exchangeability</b>; see the type's remarks.
    /// </remarks>
    /// <param name="probabilities">One sample's predicted probabilities, in calibration order.</param>
    /// <param name="quantile">The calibrated quantile from <see cref="Quantile(ReadOnlySpan{double}, double, ConformalQuantileRule)"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="quantile"/> is negative or NaN.</exception>
    public static bool[] PredictionSet(ReadOnlySpan<double> probabilities, double quantile)
    {
        if (double.IsNaN(quantile) || quantile < 0.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantile), quantile, "A calibrated quantile is a non-negative score.");
        }

        bool[] included = new bool[probabilities.Length];
        for (int i = 0; i < probabilities.Length; i++)
        {
            // MAPIE's own comparison, so a class 1e-8 short of 1 - q is in the set as it is there.
            included[i] = (1.0 - probabilities[i]) - quantile <= InclusionTolerance;
        }
        return included;
    }
}
