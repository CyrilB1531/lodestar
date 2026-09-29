namespace Lodestar.Metrics.Internal;

/// <summary>Which of the three related scores is being computed.</summary>
internal enum PrfMetric
{
    Precision,
    Recall,
    FScore,
    Jaccard,
}

/// <summary>
/// The arithmetic behind precision, recall and F-beta, kept in one place because
/// scikit-learn's zero-division and averaging rules are the whole difficulty and
/// are identical across the three.
/// </summary>
internal static class Prf
{
    /// <summary>
    /// scikit-learn's <c>_prf_divide</c>: the zero-division policy applies to a
    /// zero denominator per class, before any averaging.
    /// </summary>
    public static double Divide(double numerator, double denominator, ZeroDivision zeroDivision, string metric)
    {
        // SonarLint S1244 warns against comparing floating point for exact
        // equality, which is right for arithmetic and wrong here: this is
        // scikit-learn's own _prf_divide test, deciding between a real
        // division and the zero-division policy. A tolerance would silently
        // reroute a legitimate small-but-nonzero denominator into the
        // undefined branch and change the result.
#pragma warning disable S1244
        if (denominator != 0.0)
        {
#pragma warning restore S1244
            return numerator / denominator;
        }

        return Undefined(zeroDivision, metric);
    }

    public static double Undefined(ZeroDivision zeroDivision, string metric) => zeroDivision switch
    {
        ZeroDivision.Zero => 0.0,
        ZeroDivision.One => 1.0,
        ZeroDivision.NaN => double.NaN,
        _ => throw new UndefinedMetricException(
            $"{metric} is undefined here: no sample contributes to its denominator. "
            + "Pass ZeroDivision.Zero, One or NaN to get a value instead."),
    };

    /// <summary>The score of a matrix the caller built: the binary counts for <see cref="Averaging.Binary"/>, its requested labels otherwise.</summary>
    public static double Score(
        ConfusionMatrix cm, PrfMetric metric, double beta, Averaging average, int posLabel, ZeroDivision zeroDivision) =>
        Aggregate(
            average == Averaging.Binary ? PrfCounts.Binary(cm, posLabel) : PrfCounts.FromMatrix(cm),
            metric, beta, average, zeroDivision);

    /// <summary>The score of the samples themselves, counted per label without a matrix (#1200).</summary>
    // S107: the public overloads' own parameters, passed through; the five metric types each call this once.
#pragma warning disable S107
    public static double Score(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<int> yPred, ReadOnlySpan<int> labels, ReadOnlySpan<double> sampleWeight,
        PrfMetric metric, double beta, Averaging average, int posLabel, ZeroDivision zeroDivision) =>
        Aggregate(
            average == Averaging.Binary
                ? PrfCounts.Binary(yTrue, yPred, posLabel, sampleWeight)
                : PrfCounts.Compute(yTrue, yPred, labels, sampleWeight),
            metric, beta, average, zeroDivision);
#pragma warning restore S107

    /// <summary>One score per requested label.</summary>
    public static double[] PerClass(PrfCounts counts, PrfMetric metric, double beta, ZeroDivision zeroDivision) =>
        PerClass(counts.TruePositives, counts.Predicted, counts.Support, metric, beta, zeroDivision);

    /// <summary>The per-class scores from sums already read off a matrix, so several scores can share one read.</summary>
    public static double[] PerClass(
        double[] tp, double[] predicted, double[] support, PrfMetric metric, double beta, ZeroDivision zeroDivision)
    {
        double[] result = new double[tp.Length];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = metric switch
            {
                PrfMetric.Precision => Divide(tp[i], predicted[i], zeroDivision, "Precision"),
                PrfMetric.Recall => Divide(tp[i], support[i], zeroDivision, "Recall"),
                PrfMetric.Jaccard => Jaccard(tp[i], predicted[i], support[i], zeroDivision),
                _ => FScore(tp[i], predicted[i], support[i], beta, zeroDivision),
            };
        }

        return result;
    }

    public static double Aggregate(
        PrfCounts counts, PrfMetric metric, double beta, Averaging average, ZeroDivision zeroDivision)
    {
        if (average == Averaging.Micro)
        {
            return Micro(counts, metric, beta, zeroDivision);
        }

        double[] perClass = PerClass(counts, metric, beta, zeroDivision);
        double[] support = counts.Support;

        // jaccard_score averages through numpy.average and the other three through
        // _nanaverage, which catches its zero-sum error (#988 corrects #861).
        if (average == Averaging.Weighted && metric == PrfMetric.Jaccard)
        {
            return JaccardWeighted(perClass, support);
        }

        switch (average)
        {
            case Averaging.Macro:
            case Averaging.Weighted:
                return Average(perClass, support, average);

            case Averaging.Binary:
                // The counts hold the positive label alone; see PrfCounts.Binary.
                return perClass[0];

            default:
                throw new ArgumentOutOfRangeException(nameof(average), average, "Unknown averaging mode.");
        }
    }

    /// <summary>
    /// The macro or support-weighted mean of per-class scores — scikit-learn's
    /// <c>_nanaverage</c>, in <c>sklearn/utils/extmath.py</c>.
    /// </summary>
    /// <remarks>
    /// A <see cref="double.NaN"/> class leaves the mean with its weight, and only
    /// every class being <see cref="double.NaN"/> makes the result one. Weights that
    /// sum to zero once those are gone fall back to the unweighted mean, because
    /// <c>_nanaverage</c> catches <c>numpy.average</c>'s refusal; <see cref="JaccardWeighted"/> does not (#861, #988).
    /// </remarks>
    public static double Average(double[] perClass, double[] support, Averaging average)
    {
        Accumulate(perClass, support, out double total, out double weighted, out double weightSum, out int defined);

        if (defined == 0)
        {
            return double.NaN;
        }

        // SonarLint S1244: an exact zero is numpy.average's own ZeroDivisionError
        // test, and a tolerance would drop the weights of a small real support.
#pragma warning disable S1244
        return average == Averaging.Macro || weightSum == 0.0 ? total / defined : weighted / weightSum;
#pragma warning restore S1244
    }

    /// <summary>
    /// The support-weighted mean for Jaccard — <c>jaccard_score</c>'s own averaging,
    /// which is <c>numpy.average</c> where its three siblings use <c>_nanaverage</c>.
    /// </summary>
    /// <remarks>
    /// The reference drops its weights on <c>not numpy.any(weights)</c>, so only supports that
    /// are every one zero fall back to the unweighted mean; a set that merely sums to zero keeps
    /// them and raises, where <c>_nanaverage</c> catches that error. Only a negative sample weight
    /// reaches the refusal: non-negative weights make every support non-negative (#988).
    /// </remarks>
    /// <exception cref="ArgumentException">The supports sum to zero without all being zero.</exception>
    private static double JaccardWeighted(double[] perClass, double[] support)
    {
        Accumulate(perClass, support, out double total, out double weighted, out double weightSum, out int defined);

        if (defined == 0)
        {
            return double.NaN;
        }

        if (!AnyNonZero(support))
        {
            return total / defined;
        }

        Weights.RequireNonZeroSum(weightSum, "sampleWeight");
        return weighted / weightSum;
    }

    /// <summary><c>numpy.any</c> over the supports: is a single one of them not zero.</summary>
    private static bool AnyNonZero(double[] support) =>
        // SonarLint S1244: numpy.any tests each element against zero exactly, and a
        // tolerance would drop the weights of a support the reference keeps.
#pragma warning disable S1244
        Array.Exists(support, static weight => weight != 0.0);
#pragma warning restore S1244

    /// <summary>The running totals both averaging rules read, over the defined classes alone.</summary>
    private static void Accumulate(
        double[] perClass, double[] support, out double total, out double weighted, out double weightSum, out int defined)
    {
        total = 0.0;
        weighted = 0.0;
        weightSum = 0.0;
        defined = 0;
        for (int i = 0; i < perClass.Length; i++)
        {
            double value = perClass[i];
            if (double.IsNaN(value))
            {
                continue;
            }

            defined++;
            total += value;
            weighted += value * support[i];
            weightSum += support[i];
        }
    }

    /// <summary>
    /// The intersection over the union — precision's numerator over the size of both
    /// sets together.
    /// </summary>
    /// <remarks>
    /// Predicted plus support double-counts the true positives, so one copy comes back
    /// out. Undefined when neither set holds anything, which is the same
    /// <see cref="ZeroDivision"/> case precision and recall answer.
    /// </remarks>
    private static double Jaccard(double tp, double predicted, double support, ZeroDivision zeroDivision) =>
        Divide(tp, predicted + support - tp, zeroDivision, "Jaccard");

    public static double Micro(PrfCounts counts, PrfMetric metric, double beta, ZeroDivision zeroDivision)
    {
        double[] tp = counts.TruePositives;
        double[] predicted = counts.Predicted;
        double[] support = counts.Support;

        double tpSum = 0.0;
        double predictedSum = 0.0;
        double supportSum = 0.0;
        for (int i = 0; i < tp.Length; i++)
        {
            tpSum += tp[i];
            predictedSum += predicted[i];
            supportSum += support[i];
        }

        return metric switch
        {
            PrfMetric.Precision => Divide(tpSum, predictedSum, zeroDivision, "Precision"),
            PrfMetric.Recall => Divide(tpSum, supportSum, zeroDivision, "Recall"),
            PrfMetric.Jaccard => Jaccard(tpSum, predictedSum, supportSum, zeroDivision),
            _ => FScore(tpSum, predictedSum, supportSum, beta, zeroDivision),
        };
    }

    // Derived from the raw counts, not the already-divided precision and
    // recall — see decision 0032 at 53af23c2 for why the two diverge.
    private static double FScore(double tp, double predicted, double support, double beta, ZeroDivision zeroDivision)
    {
        // SonarLint S1244 warns against comparing floating point for exact
        // equality, which does not apply here: this is not a numerical
        // guard at all. beta == 0 selects a documented, discrete behaviour —
        // scikit-learn defines fbeta_score(beta=0) as precision — the same
        // way a switch selects a case. There is no "close to zero" beta that
        // should also take this branch.
#pragma warning disable S1244
        if (beta == 0.0)
        {
#pragma warning restore S1244
            return Divide(tp, predicted, zeroDivision, "Precision");
        }

        double beta2 = beta * beta;
        double numerator = (1.0 + beta2) * tp;
        double denominator = predicted + (beta2 * support);
        return Divide(numerator, denominator, zeroDivision, "F-score");
    }

    public static void ValidateBeta(double beta)
    {
        if (double.IsNaN(beta) || double.IsInfinity(beta) || beta < 0.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(beta), beta, "beta must be a finite number greater than or equal to zero.");
        }
    }
}
