using Lodestar.Metrics.Internal;

namespace Lodestar.Metrics;

/// <summary>
/// The two class likelihood ratios — the equivalent of
/// <c>sklearn.metrics.class_likelihood_ratios</c>.
/// </summary>
/// <remarks>
/// How much a prediction should move a belief, independently of how common the class
/// is: a positive result multiplies the prior odds by <see cref="Positive"/> and a
/// negative one by <see cref="Negative"/>. That prevalence-independence is what
/// separates them from precision, which moves with the base rate.
/// </remarks>
public sealed class LikelihoodRatios
{
    private LikelihoodRatios(double positive, double negative)
    {
        Positive = positive;
        Negative = negative;
    }

    /// <summary>The positive ratio, <c>LR+</c>: sensitivity over one minus specificity. Above <c>1</c> when a positive prediction is evidence for the class.</summary>
    public double Positive { get; }

    /// <summary>The negative ratio, <c>LR-</c>: one minus sensitivity, over specificity. Below <c>1</c> when a negative prediction is evidence against the class.</summary>
    public double Negative { get; }

    /// <summary>Both ratios — <c>class_likelihood_ratios(y_true, y_pred, sample_weight=…, replace_undefined_by=…)</c>.</summary>
    /// <param name="yTrue">The true labels, one per sample. Exactly two distinct values may occur.</param>
    /// <param name="yPred">The predicted labels, same length as <paramref name="yTrue"/>.</param>
    /// <param name="posLabel">The label counted as positive. scikit-learn takes the greater of the two through <c>labels</c>; this asks.</param>
    /// <param name="undefinedPositive">What <see cref="Positive"/> answers when it has no value. The default reproduces <c>replace_undefined_by=nan</c>.</param>
    /// <param name="undefinedNegative">What <see cref="Negative"/> answers when it has no value.</param>
    /// <param name="sampleWeight">A weight per sample. Omit to weight every sample by 1.</param>
    /// <returns>
    /// Both ratios. Either is replaced when it has no value: <see cref="Positive"/>
    /// when no sample was predicted into the class wrongly, <see cref="Negative"/>
    /// when none was predicted out of it rightly. With no positive sample at all, a
    /// ratio that is not replaced is <c>NaN</c>, as in scikit-learn.
    /// </returns>
    /// <exception cref="ArgumentException">The inputs disagree in length, are empty, the weights do not match, hold a non-finite value or are zero throughout, or other than two distinct labels occur, or two without <paramref name="posLabel"/>.</exception>
    public static LikelihoodRatios Compute(
        ReadOnlySpan<int> yTrue,
        ReadOnlySpan<int> yPred,
        int posLabel = 1,
        double undefinedPositive = double.NaN,
        double undefinedNegative = double.NaN,
        ReadOnlySpan<double> sampleWeight = default)
    {
        Inputs.Validate(yTrue, yPred, sampleWeight);
        RequireBinary(yTrue, yPred, posLabel);

        double truePositive = 0.0;
        double falseNegative = 0.0;
        double falsePositive = 0.0;
        double trueNegative = 0.0;

        for (int i = 0; i < yTrue.Length; i++)
        {
            double weight = sampleWeight.IsEmpty ? 1.0 : sampleWeight[i];
            bool actual = yTrue[i] == posLabel;
            bool guessed = yPred[i] == posLabel;

            if (actual)
            {
                if (guessed)
                {
                    truePositive += weight;
                }
                else
                {
                    falseNegative += weight;
                }
            }
            else if (guessed)
            {
                falsePositive += weight;
            }
            else
            {
                trueNegative += weight;
            }
        }

        // The reference's own arithmetic, straight from the counts, since one minus specificity cancels (#1252).
        // Each ratio is replaced only where its own count vanishes, and is otherwise NaN as there (#1250).
        double positives = truePositive + falseNegative;
        double negatives = trueNegative + falsePositive;

        return new LikelihoodRatios(
            Ratio(truePositive * negatives, falsePositive, falsePositive * positives, undefinedPositive),
            Ratio(falseNegative * negatives, trueNegative, trueNegative * positives, undefinedNegative));
    }

    // S1244: whether the count vanished, which is the reference's own test -- it warns that the
    // ratio is ill-defined and substitutes rather than dividing.
#pragma warning disable S1244
    private static double Ratio(double numerator, double count, double denominator, double undefined) =>
        count == 0.0 ? undefined : numerator / denominator;
#pragma warning restore S1244

    /// <summary>Refuses anything but two classes, as the reference does.</summary>
    /// <remarks>
    /// One class alone is refused too: <c>class_likelihood_ratios</c> unpacks four counts from a
    /// <c>1 × 1</c> matrix there and raises, measured on scikit-learn 1.9.1.
    /// </remarks>
    private static void RequireBinary(ReadOnlySpan<int> yTrue, ReadOnlySpan<int> yPred, int posLabel)
    {
        var seen = new SortedSet<int>();
        for (int i = 0; i < yTrue.Length; i++)
        {
            seen.Add(yTrue[i]);
            seen.Add(yPred[i]);
            if (seen.Count > 2)
            {
                throw new ArgumentException(
                    "class_likelihood_ratios only supports binary classification problems.",
                    nameof(yTrue));
            }
        }

        if (seen.Count < 2)
        {
            throw new ArgumentException(
                "class_likelihood_ratios needs both classes to occur in yTrue or yPred; only one does.",
                nameof(yTrue));
        }

        // The reference takes the greater label as positive; asked for one the data lacks, every
        // sample would count negative, so it is refused as the precision family refuses it.
        if (!seen.Contains(posLabel))
        {
            throw new ArgumentException(
                $"posLabel {posLabel} does not occur in the data, which holds {seen.Min} and {seen.Max}.",
                nameof(posLabel));
        }
    }
}
