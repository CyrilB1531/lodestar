namespace Lodestar.Metrics.Internal;

/// <summary>The binary scores' checks on which label is positive, read off the labels present rather than their weights.</summary>
internal static class PositiveLabel
{
    /// <summary>Refuses a target of exactly two labels, neither of them <paramref name="posLabel"/>.</summary>
    /// <remarks>
    /// Every sample would count negative. <c>posLabel</c> is read as scikit-learn's explicit <c>pos_label</c>, which
    /// <c>average_precision_score</c> refuses here; <c>roc_auc_score</c> and <c>log_loss</c> take none and infer the
    /// greater label, which this cannot, so they refuse too (#1277, #1542). The curves, the calibration and the Brier
    /// score accept an explicit one, and do not call this. A target of one label or of three is its caller's rule.
    /// </remarks>
    public static void RequireAmongTwo(ReadOnlySpan<int> yTrue, int posLabel, string paramName)
    {
        if (yTrue.IsEmpty)
        {
            return;
        }

        int first = yTrue[0];
        int? second = null;
        foreach (int label in yTrue)
        {
            if (label == first || label == second)
            {
                continue;
            }

            if (second is not null)
            {
                return;
            }

            second = label;
        }

        if (second is int other && first != posLabel && other != posLabel)
        {
            throw new ArgumentException(
                $"posLabel {posLabel} does not occur in the data, which holds {Math.Min(first, other)} and {Math.Max(first, other)}.",
                paramName);
        }
    }

    /// <summary>Refuses a target of more than two labels, which a binary score would count as negatives.</summary>
    public static void RequireAtMostTwo(ReadOnlySpan<int> yTrue, string use, string paramName)
    {
        if (Probabilities.LabelCountBeyondTwo(yTrue) is int count and > 0)
        {
            throw new ArgumentException($"yTrue holds {count} labels; a binary score takes two. {use}", paramName);
        }
    }
}
