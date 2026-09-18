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

    /// <summary>
    /// The support of each requested class — scikit-learn's <c>true_sum</c>,
    /// counted against every observed label; see <see cref="ConfusionMatrix.Stride"/>.
    /// </summary>
    /// <remarks>
    /// Returns <see cref="ConfusionMatrix.TrueSum"/> directly rather than
    /// summing <see cref="ConfusionMatrix.Cells"/> — the two totals agree
    /// mathematically but not always in the last bit; see <see cref="ConfusionMatrix.TrueSum"/>'s own remarks.
    /// </remarks>
    public static double[] Support(ConfusionMatrix cm) => cm.TrueSum.ToArray();

    /// <summary>
    /// Column sums: how much weight was predicted into each requested class,
    /// again counted against every observed label — scikit-learn's
    /// <c>pred_sum</c>. Runs over <see cref="ConfusionMatrix.Stride"/> rows
    /// rather than just the requested <see cref="ConfusionMatrix.Size"/> for the
    /// same reason <see cref="ConfusionMatrix.Stride"/>'s own remarks give: a
    /// predicted label outside the request still belongs in a requested true
    /// label's column sum, the same as scikit-learn counts it.
    /// </summary>
    public static double[] PredictedSum(ConfusionMatrix cm)
    {
        int k = cm.Size;
        int stride = cm.Stride;
        ReadOnlySpan<double> cells = cm.Cells;
        double[] predicted = new double[k];
        for (int row = 0; row < stride; row++)
        {
            int offset = row * stride;
            for (int col = 0; col < k; col++)
            {
                predicted[col] += cells[offset + col];
            }
        }
        return predicted;
    }

    /// <summary>The diagonal: correctly predicted weight per requested class.</summary>
    public static double[] TruePositives(ConfusionMatrix cm)
    {
        int k = cm.Size;
        int stride = cm.Stride;
        ReadOnlySpan<double> cells = cm.Cells;
        double[] tp = new double[k];
        for (int i = 0; i < k; i++)
        {
            tp[i] = cells[(i * stride) + i];
        }
        return tp;
    }

    public static double[] PerClass(ConfusionMatrix cm, PrfMetric metric, double beta, ZeroDivision zeroDivision) =>
        PerClass(cm, metric, beta, zeroDivision, out _);

    // Same computation, support out for Aggregate's Weighted branch to reuse
    // instead of a second O(k*stride) pass.
    private static double[] PerClass(
        ConfusionMatrix cm, PrfMetric metric, double beta, ZeroDivision zeroDivision, out double[] support)
    {
        support = Support(cm);
        return PerClass(TruePositives(cm), PredictedSum(cm), support, metric, beta, zeroDivision);
    }

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
        ConfusionMatrix cm, PrfMetric metric, double beta, Averaging average, int posLabel, ZeroDivision zeroDivision)
    {
        if (average == Averaging.Micro)
        {
            return Micro(cm, metric, beta, zeroDivision);
        }

        double[] perClass = PerClass(cm, metric, beta, zeroDivision, out double[] support);

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
                return perClass[BinaryOrdinal(cm, posLabel)];

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

    private static double Micro(ConfusionMatrix cm, PrfMetric metric, double beta, ZeroDivision zeroDivision)
    {
        double[] tp = TruePositives(cm);
        double[] predicted = PredictedSum(cm);
        double[] support = Support(cm);

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
    // recall — see docs/decisions/0032 for why the two diverge.
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

    private static int BinaryOrdinal(ConfusionMatrix cm, int posLabel)
    {
        // scikit-learn refuses average="binary" once the *observed* target has
        // more than two classes — which a dropped-samples matrix also implies.
        if (cm.Size > 2 || (cm.ExplicitLabels && cm.DroppedSamples))
        {
            throw new ArgumentException(
                "Averaging.Binary needs a two-class target. Use Micro, Macro or Weighted, or PerClass.",
                nameof(posLabel));
        }

        for (int i = 0; i < cm.Labels.Count; i++)
        {
            if (cm.Labels[i] == posLabel)
            {
                return i;
            }
        }

        throw new ArgumentException(
            $"posLabel {posLabel} does not occur in the data.", nameof(posLabel));
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
