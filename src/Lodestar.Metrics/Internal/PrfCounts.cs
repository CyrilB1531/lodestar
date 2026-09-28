using System.Numerics;
using System.Runtime.InteropServices;

namespace Lodestar.Metrics.Internal;

/// <summary>
/// What precision, recall, F-beta, Jaccard and the report read: per requested label, the correctly
/// predicted weight, the predicted weight and the true weight — scikit-learn's <c>tp_sum</c>,
/// <c>pred_sum</c> and <c>true_sum</c>.
/// </summary>
/// <remarks>
/// Weights sum in sample order, <c>np.bincount</c>'s, in <c>O(n + k)</c> memory: the <c>m × m</c>
/// matrix it replaces overflowed past 46,341 labels (#1200). <see cref="FromMatrix"/> reads a built one.
/// </remarks>
internal sealed class PrfCounts
{
    private PrfCounts(int[] labels, double[] truePositives, double[] predicted, double[] support)
    {
        Labels = labels;
        TruePositives = truePositives;
        Predicted = predicted;
        Support = support;
    }

    /// <summary>The requested labels, in the order the other arrays follow.</summary>
    public int[] Labels { get; }

    public double[] TruePositives { get; }

    public double[] Predicted { get; }

    public double[] Support { get; }

    /// <summary>The weight of the samples whose true and predicted labels were both requested.</summary>
    public double KeptTotal { get; private init; }

    public bool AnySampleCorrect { get; private init; }

    public bool IsWeighted { get; private init; }

    /// <summary>Whether labels were supplied and some observed label is not among them.</summary>
    public bool DropsObservedLabels { get; private init; }

    /// <summary>The counts over every requested label, or every observed one when none is supplied.</summary>
    /// <exception cref="ArgumentException">The inputs disagree in length or are empty, <paramref name="labels"/> repeats a label, or the weights are invalid.</exception>
    public static PrfCounts Compute(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<int> yPred, ReadOnlySpan<int> labels, ReadOnlySpan<double> sampleWeight)
    {
        Inputs.Validate(yTrue, yPred, sampleWeight);
        LabelIndex index = LabelIndex.Create(yTrue, yPred, labels);
        // Few labels, unweighted: the matrix path, one increment a sample and exact integer sums; many:
        // per-label sums, where that table would be 8·m² bytes (#1200). Weights sum in sample order.
        if (sampleWeight.IsEmpty && index.Count <= DenseLabelLimit)
        {
            return FromMatrix(ConfusionMatrix.Count(yTrue, yPred, index, sampleWeight, out _));
        }

        if (sampleWeight.IsEmpty && index.TryGetDirect(out int[] table, out int min))
        {
            return CountUnweighted(yTrue, yPred, index, table, min);
        }

        return CountByIndex(yTrue, yPred, sampleWeight, index);
    }

    /// <summary>The counts through the index's lookup, weighted or over labels too sparse for a table.</summary>
    private static PrfCounts CountByIndex(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<int> yPred, ReadOnlySpan<double> sampleWeight, LabelIndex index)
    {
        int k = index.RequestedCount;
        bool weighted = !sampleWeight.IsEmpty;
        var tp = new double[k];
        var predicted = new double[k];
        var support = new double[k];
        double kept = 0.0;
        bool anyCorrect = false;
        for (int i = 0; i < yTrue.Length; i++)
        {
            int row = index.IndexOf(yTrue[i]);
            int col = index.IndexOf(yPred[i]);
            double weight = weighted ? sampleWeight[i] : 1.0;
            if (row < k)
            {
                support[row] += weight;
                tp[row] += row == col ? weight : 0.0;
            }

            if (col < k)
            {
                predicted[col] += weight;
                kept += row < k ? weight : 0.0;
            }

            anyCorrect |= row == col;
        }

        int[] requested = new int[k];
        Array.Copy(index.Labels, requested, k);
        return new PrfCounts(requested, tp, predicted, support)
        {
            KeptTotal = kept,
            AnySampleCorrect = anyCorrect,
            IsWeighted = weighted,
            DropsObservedLabels = index.Count > k,
        };
    }

    // Past this many labels the m × m table no longer pays for itself.
    private const int DenseLabelLimit = 64;

    /// <summary>The unweighted counts over many labels, as integers, whose sums are exact in any order.</summary>
    private static PrfCounts CountUnweighted(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<int> yPred, LabelIndex index, int[] table, int min)
    {
        int k = index.RequestedCount;
        int m = index.Count;
        long[] tp = new long[m];
        long[] predicted = new long[m];
        long[] support = new long[m];
        long kept = CountPerLabel(yTrue, yPred, table, min, k, tp, predicted, support);

        bool anyCorrect = Array.Exists(tp, static count => count > 0);
        int[] requested = new int[k];
        Array.Copy(index.Labels, requested, k);
        return new PrfCounts(requested, Requested(tp, k), Requested(predicted, k), Requested(support, k))
        {
            KeptTotal = kept,
            AnySampleCorrect = anyCorrect,
            IsWeighted = false,
            DropsObservedLabels = m > k,
        };
    }

    // S107: the four outputs are filled in place, which keeps the loop free of a result type.
#pragma warning disable S107
    private static long CountPerLabel(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<int> yPred, int[] table, int min, int k,
        long[] tp, long[] predicted, long[] support)
    {
        long kept = 0;
        for (int i = 0; i < yTrue.Length; i++)
        {
            int row = table[yTrue[i] - min];
            int col = table[yPred[i] - min];
            support[row]++;
            predicted[col]++;
            tp[row] += row == col ? 1 : 0;
            kept += row < k && col < k ? 1 : 0;
        }

        return kept;
    }
#pragma warning restore S107

    private static double[] Requested(long[] counts, int k)
    {
        var result = new double[k];
        for (int i = 0; i < k; i++)
        {
            result[i] = counts[i];
        }

        return result;
    }

    /// <summary>The counts for <paramref name="posLabel"/> alone, as <c>average="binary"</c> takes them.</summary>
    /// <remarks>
    /// <c>_check_set_wise_labels</c> replaces <c>labels</c> with <c>[pos_label]</c> on a binary target,
    /// so the ones supplied are not read (#1249), and refuses only a target with more than two classes
    /// or a <c>pos_label</c> absent from one that has two. An absent positive class otherwise counts
    /// zero everywhere and takes the zero-division value (#1201).
    /// </remarks>
    /// <exception cref="ArgumentException">The target has more than two classes, or two without <paramref name="posLabel"/>, or the inputs are invalid.</exception>
    public static PrfCounts Binary(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<int> yPred, int posLabel, ReadOnlySpan<double> sampleWeight)
    {
        Inputs.Validate(yTrue, yPred, sampleWeight);
        RequireBinaryTarget(LabelIndex.SortedUnion(yTrue, yPred), posLabel);

        bool weighted = !sampleWeight.IsEmpty;
        double tp = 0.0;
        double predicted = 0.0;
        double support = 0.0;
        for (int i = 0; i < yTrue.Length; i++)
        {
            double weight = weighted ? sampleWeight[i] : 1.0;
            bool actual = yTrue[i] == posLabel;
            bool guessed = yPred[i] == posLabel;
            support += actual ? weight : 0.0;
            predicted += guessed ? weight : 0.0;
            tp += actual && guessed ? weight : 0.0;
        }

        return new PrfCounts([posLabel], [tp], [predicted], [support]) { IsWeighted = weighted };
    }

    /// <summary>The same binary counts read off a matrix, whose observed labels stand for the target's.</summary>
    /// <exception cref="ArgumentException">The matrix's data has more than two classes, or two without <paramref name="posLabel"/>.</exception>
    public static PrfCounts Binary(ConfusionMatrix cm, int posLabel)
    {
        RequireBinaryTarget(cm.Observed, posLabel);

        ReadOnlySpan<int> all = cm.AllLabels;
        int stride = cm.Stride;
        ReadOnlySpan<double> cells = cm.Cells;
        int at = all.IndexOf(posLabel);
        double tp = 0.0;
        double predicted = 0.0;
        double support = 0.0;
        if (at >= 0)
        {
            tp = cells[(at * stride) + at];
            for (int other = 0; other < stride; other++)
            {
                predicted += cells[(other * stride) + at];
            }

            support = at < cm.Size ? cm.TrueSum[at] : RowSum(cells, at, stride);
        }

        return new PrfCounts([posLabel], [tp], [predicted], [support]) { IsWeighted = cm.IsWeighted };
    }

    /// <summary>The counts over a matrix's requested labels.</summary>
    /// <remarks>
    /// Predicted weight is summed down each column over every observed row, so a sample predicted
    /// into a requested class from outside the request still counts, as scikit-learn counts it.
    /// </remarks>
    public static PrfCounts FromMatrix(ConfusionMatrix cm)
    {
        int k = cm.Size;
        int stride = cm.Stride;
        ReadOnlySpan<double> cells = cm.Cells;
        var tp = new double[k];
        var predicted = new double[k];
        for (int row = 0; row < stride; row++)
        {
            AddRow(predicted, cells.Slice(row * stride, k));
        }

        for (int i = 0; i < k; i++)
        {
            tp[i] = cells[(i * stride) + i];
        }

        int[] labels = new int[k];
        for (int i = 0; i < k; i++)
        {
            labels[i] = cm.Labels[i];
        }

        return new PrfCounts(labels, tp, predicted, cm.TrueSum.ToArray())
        {
            KeptTotal = cm.TotalWeight,
            AnySampleCorrect = !cm.NoSampleCorrect,
            IsWeighted = cm.IsWeighted,
            DropsObservedLabels = cm.ExplicitLabels && cm.DroppedSamples,
        };
    }

    // Adds a row into the column sums a vector at a time: each column still sums its rows in order,
    // so the result is the scalar loop's to the bit, several columns at once.
    private static void AddRow(double[] sums, ReadOnlySpan<double> row)
    {
        int col = 0;
        if (Vector.IsHardwareAccelerated)
        {
            col = row.Length - (row.Length % Vector<double>.Count);
            Span<Vector<double>> target = MemoryMarshal.Cast<double, Vector<double>>(sums.AsSpan(0, col));
            ReadOnlySpan<Vector<double>> source = MemoryMarshal.Cast<double, Vector<double>>(row.Slice(0, col));
            for (int i = 0; i < target.Length; i++)
            {
                target[i] += source[i];
            }
        }

        for (; col < row.Length; col++)
        {
            sums[col] += row[col];
        }
    }

    private static void RequireBinaryTarget(ReadOnlySpan<int> observed, int posLabel)
    {
        if (observed.Length > 2)
        {
            throw new ArgumentException(
                "Averaging.Binary needs a two-class target. Use Micro, Macro or Weighted, or PerClass.",
                nameof(posLabel));
        }

        if (observed.Length == 2 && observed.IndexOf(posLabel) < 0)
        {
            throw new ArgumentException(
                $"posLabel {posLabel} does not occur in the data, which holds {observed[0]} and {observed[1]}.",
                nameof(posLabel));
        }
    }

    private static double RowSum(ReadOnlySpan<double> cells, int row, int stride)
    {
        double total = 0.0;
        for (int col = 0; col < stride; col++)
        {
            total += cells[(row * stride) + col];
        }

        return total;
    }
}
