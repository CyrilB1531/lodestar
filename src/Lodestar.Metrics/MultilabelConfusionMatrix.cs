using Lodestar.Metrics.Internal;

namespace Lodestar.Metrics;

/// <summary>
/// One 2×2 confusion matrix per label, or per sample — the equivalent of
/// <c>sklearn.metrics.multilabel_confusion_matrix</c>.
/// </summary>
/// <remarks>
/// A stack of <see cref="ConfusionMatrix"/> rather than a type of its own: an entry is
/// one class counted against everything else, which a two-label matrix already is. Its
/// labels are <c>0</c> and <c>1</c> in that order, so the cells land where the
/// reference puts them — true negative, false positive, false negative, true positive.
/// </remarks>
public static class MultilabelConfusionMatrix
{
    /// <summary>One matrix per class, each the class against everything else — <c>multilabel_confusion_matrix(y_true, y_pred, labels=…, sample_weight=…)</c>.</summary>
    /// <param name="yTrue">The true labels, one per sample.</param>
    /// <param name="yPred">The predicted labels, same length as <paramref name="yTrue"/>.</param>
    /// <param name="labels">The classes to report and their order. Omit for the sorted union of both inputs.</param>
    /// <param name="sampleWeight">A weight per sample. Omit to weight every sample by 1.</param>
    /// <returns>One matrix per class, in label order.</returns>
    /// <exception cref="ArgumentException">The inputs disagree in length, are empty, or the weights do not match, hold a non-finite value or are zero throughout.</exception>
    public static ConfusionMatrix[] Compute(
        ReadOnlySpan<int> yTrue,
        ReadOnlySpan<int> yPred,
        ReadOnlySpan<int> labels = default,
        ReadOnlySpan<double> sampleWeight = default)
    {
        Inputs.Validate(yTrue, yPred, sampleWeight);

        int[] classes = labels.IsEmpty ? LabelIndex.SortedUnion(yTrue, yPred) : labels.ToArray();
        var stack = new ConfusionMatrix[classes.Length];
        bool weighted = !sampleWeight.IsEmpty;

        for (int i = 0; i < classes.Length; i++)
        {
            int label = classes[i];
            var tally = new Tally(weighted);
            for (int sample = 0; sample < yTrue.Length; sample++)
            {
                int cell = Cell(yTrue[sample] == label, yPred[sample] == label);
                if (weighted)
                {
                    tally.Add(cell, sampleWeight[sample]);
                }
                else
                {
                    tally.Count(cell);
                }
            }

            stack[i] = tally.ToMatrix();
        }

        return stack;
    }

    /// <summary>One matrix per label of a matrix, or per sample when <paramref name="samplewise"/> holds.</summary>
    /// <param name="yTrue">Whether each label holds, row-major: one row per sample, <paramref name="labelCount"/> values each.</param>
    /// <param name="yPred">The predicted labels, same shape as <paramref name="yTrue"/>.</param>
    /// <param name="labelCount">How many labels each row holds.</param>
    /// <param name="samplewise">Count one matrix per <em>sample</em> instead of per label. The reference offers this on a matrix only, and refuses it on single-label input.</param>
    /// <param name="sampleWeight">A weight per <em>sample</em> — per row, not per label.</param>
    /// <returns>One matrix per label in column order, or per sample in row order.</returns>
    /// <exception cref="ArgumentException">The shapes disagree, or the weights do not match the row count, hold a non-finite value or are zero throughout.</exception>
    public static ConfusionMatrix[] Compute(
        ReadOnlySpan<bool> yTrue,
        ReadOnlySpan<bool> yPred,
        int labelCount,
        bool samplewise = false,
        ReadOnlySpan<double> sampleWeight = default)
    {
        int rows = Multilabel.Validate(yTrue, yPred, labelCount, sampleWeight);
        return samplewise
            ? PerSample(yTrue, yPred, labelCount, rows, sampleWeight)
            : PerLabel(yTrue, yPred, labelCount, rows, sampleWeight);
    }

    /// <summary>One matrix per label column, counting samples.</summary>
    private static ConfusionMatrix[] PerLabel(
        ReadOnlySpan<bool> yTrue, ReadOnlySpan<bool> yPred, int labelCount, int rows,
        ReadOnlySpan<double> sampleWeight)
    {
        var stack = new ConfusionMatrix[labelCount];
        bool weighted = !sampleWeight.IsEmpty;

        for (int label = 0; label < labelCount; label++)
        {
            var tally = new Tally(weighted);
            for (int row = 0; row < rows; row++)
            {
                int at = (row * labelCount) + label;
                int cell = Cell(yTrue[at], yPred[at]);
                if (weighted)
                {
                    tally.Add(cell, sampleWeight[row]);
                }
                else
                {
                    tally.Count(cell);
                }
            }

            stack[label] = tally.ToMatrix();
        }

        return stack;
    }

    /// <summary>One matrix per row, counting that row's labels under its own weight.</summary>
    private static ConfusionMatrix[] PerSample(
        ReadOnlySpan<bool> yTrue, ReadOnlySpan<bool> yPred, int labelCount, int rows,
        ReadOnlySpan<double> sampleWeight)
    {
        var stack = new ConfusionMatrix[rows];
        bool weighted = !sampleWeight.IsEmpty;

        for (int row = 0; row < rows; row++)
        {
            var tally = new Tally(weighted);
            int start = row * labelCount;
            for (int label = 0; label < labelCount; label++)
            {
                int cell = Cell(yTrue[start + label], yPred[start + label]);

                // The row's weight once per label, added as the repeated weight vector was.
                if (weighted)
                {
                    tally.Add(cell, sampleWeight[row]);
                }
                else
                {
                    tally.Count(cell);
                }
            }

            stack[row] = tally.ToMatrix();
        }

        return stack;
    }

    /// <summary>The row-major cell of a two-label matrix over <c>[0, 1]</c>, rows true.</summary>
    private static int Cell(bool isTrue, bool isPredicted) => (isTrue ? 2 : 0) + (isPredicted ? 1 : 0);

    /// <summary>
    /// One two-label matrix counted in a single pass, adding to each total in sample order as
    /// <see cref="ConfusionMatrix.Compute"/> does, so a weighted sum lands on the same bits.
    /// </summary>
    /// <remarks>
    /// Unweighted, the cells are counted as integers: <see cref="ConfusionMatrix.Compute"/> adds
    /// 1.0 per sample, which is exact below 2^53, so the conversion gives the same doubles.
    /// </remarks>
    private sealed class Tally(bool weighted)
    {
        private readonly double[] _cells = new double[4];
        private readonly double[] _trueSum = new double[2];
        private readonly int[] _counts = new int[4];
        private double _total;

        public void Count(int cell) => _counts[cell]++;

        public void Add(int cell, double weight)
        {
            _trueSum[cell >> 1] += weight;
            _cells[cell] += weight;
            _total += weight;
            _counts[cell]++;
        }

        public ConfusionMatrix ToMatrix()
        {
            bool anyCorrect = _counts[0] > 0 || _counts[3] > 0;
            if (weighted)
            {
                return ConfusionMatrix.FromBinaryCells(_cells, _trueSum, _total, anyCorrect, weighted: true);
            }

            for (int cell = 0; cell < 4; cell++)
            {
                _cells[cell] = _counts[cell];
            }

            _trueSum[0] = (double)_counts[0] + _counts[1];
            _trueSum[1] = (double)_counts[2] + _counts[3];
            return ConfusionMatrix.FromBinaryCells(
                _cells, _trueSum, (double)_counts[0] + _counts[1] + _counts[2] + _counts[3], anyCorrect, weighted: false);
        }
    }
}
