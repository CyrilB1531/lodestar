using Lodestar.Metrics.Internal;

namespace Lodestar.Metrics;

/// <summary>
/// Area under the receiver-operating-characteristic curve — the equivalent of
/// <c>sklearn.metrics.roc_auc_score</c>.
/// </summary>
/// <remarks>
/// Two entry points rather than scikit-learn's single overloaded function: their
/// parameter lists would be indistinguishable to the C# compiler, and a call
/// like <c>Score(y, s, 3)</c> would fail to compile in consumer code.
/// </remarks>
public static class RocAuc
{
    /// <summary>
    /// The binary case — <c>roc_auc_score(y_true, y_score, sample_weight=…)</c>.
    /// </summary>
    /// <param name="yTrue">The true labels, of at most two distinct values; one alone, or a class weighted zero throughout, scores <see cref="double.NaN"/>, as the reference answers.</param>
    /// <param name="yScore">A score per sample: the higher, the more the model believes <paramref name="posLabel"/>.</param>
    /// <param name="posLabel">The label counted as positive. scikit-learn infers this; 1 is what it infers for 0/1 labels.</param>
    /// <param name="sampleWeight">A weight per sample. Omit to weight every sample by 1.</param>
    /// <exception cref="ArgumentException">An input is empty or a score is not finite, refused first as <c>check_array</c> refuses them (#1585); <paramref name="yTrue"/> holds more than two labels, or two without <paramref name="posLabel"/> (#1277); or, over two labels, the inputs disagree in length or a weight is not finite. One label alone answers NaN before either is checked (#1584).; or, over two labels, a negative sample weight turns the false-positive rate back, as <c>auc</c> refuses it (#1601)</exception>
    public static double Score(
        ReadOnlySpan<int> yTrue,
        ReadOnlySpan<double> yScore,
        int posLabel = 1,
        ReadOnlySpan<double> sampleWeight = default)
    {
        // roc_auc_score checks y_true, then y_score, for samples and finiteness before any label is counted (#1585).
        if (yTrue.IsEmpty || yScore.IsEmpty)
        {
            throw new ArgumentException(
                "Found array with 0 sample(s) (shape=(0,)) while a minimum of 1 is required.",
                yTrue.IsEmpty ? nameof(yTrue) : nameof(yScore));
        }

        Inputs.RequireFinite(yScore, nameof(yScore));

        // Read off the labels present, not their weights, as roc_auc_score reads them (#1277).
        PositiveLabel.RequireAtMostTwo(yTrue, "Score several with RocAuc.MultiClass.", nameof(yTrue));
        PositiveLabel.RequireAmongTwo(yTrue, posLabel, nameof(posLabel));

        // roc_auc_score answers NaN before roc_curve reads a weight or compares lengths (#1565).
        return Probabilities.HoldsOneLabel(yTrue) ? double.NaN : BinaryRoc.Score(yTrue, yScore, posLabel, sampleWeight, weightsChecked: false, nameof(sampleWeight));
    }

    /// <summary>
    /// The multiclass case —
    /// <c>roc_auc_score(y_true, y_score, multi_class=…, average=…, labels=…)</c>.
    /// </summary>
    /// <param name="yTrue">The true labels, one per sample.</param>
    /// <param name="yScore">Class probabilities, row-major: sample 0's classes, then sample 1's. Length must be <paramref name="classCount"/> times the sample count, and each row must sum to 1, except for one or two columns over a <paramref name="yTrue"/> of two labels or one, which take scikit-learn's binary path: no sum is checked there, one column over two labels is scored, two are refused, and over one label no row or weight count is checked, the answer being NaN (#1605).</param>
    /// <param name="classCount">How many classes each row scores.</param>
    /// <param name="options">Strategy, averaging, labels, sample weights and worker count. <c>default</c> is scikit-learn's own defaults, on one thread.</param>
    /// <exception cref="ArgumentException">Any of the rules above is broken.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="MultiClassRocOptions.MaxDegreeOfParallelism"/> is negative, or <paramref name="classCount"/> is below one.</exception>
    /// <remarks>
    /// A <c>catch</c> written for the sequential path keeps working above one worker: the inputs are
    /// refused before a worker starts, in scikit-learn's order (#1569, #1601), and a class curve that
    /// refuses its rates under a negative weight is rethrown as the instance it is, the lowest class's,
    /// never as an <see cref="AggregateException"/>.
    /// </remarks>
    public static double MultiClass(
        ReadOnlySpan<int> yTrue,
        ReadOnlySpan<double> yScore,
        int classCount,
        MultiClassRocOptions options = default) =>
        MultiClassRoc.Score(yTrue, yScore, classCount, options);
}
