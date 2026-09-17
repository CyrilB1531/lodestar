using Lodestar.Abstractions;

namespace Lodestar.Decomposition.Internal;

/// <summary>Lee and Seung's multiplicative updates, in scikit-learn's <c>solver="mu"</c> form.</summary>
/// <remarks>
/// Each factor is scaled by a ratio rather than moved by a step, which keeps it non-negative with no
/// projection and no line search — and makes a zero permanent, so the initialisation decides the
/// sparsity. W is updated first and H second, against the already-updated W. H is held column-major,
/// <c>H[a, j]</c> at <c>j·k + a</c>: a sparse product reads one feature's k entries per non-zero, which a
/// row-major H scatters a feature count apart. Each cell sums in the row-major loops' order, to the bit.
/// </remarks>
internal static class MultiplicativeUpdates
{
    internal static void UpdateWeights(
        CsrMatrix matrix, double[] w, double[] h, int k, NmfBetaLoss loss, Workspace workspace)
    {
        double[] numerator = workspace.RowsByRank;

        if (loss == NmfBetaLoss.KullbackLeibler)
        {
            // WH is needed only where X is non-zero, and the ratio X/WH replaces it there.
            double[] ratio = SparseRatio(matrix, w, h, k, workspace.Ratio);
            SparsePatternTimesTranspose(matrix, ratio, h, k, numerator);
            ScaleByRank(w, numerator, SumsPerRank(h, k, workspace.Sums), k);
        }
        else
        {
            SparsePatternTimesTranspose(matrix, matrix.Values, h, k, numerator);   // X Hᵀ
            double[] hht = Gram(h, k, workspace.RankByRank);                              // H Hᵀ
            double[] denominator = workspace.RowsByRankDenominator;
            DenseProduct(w, k, hht, denominator);
            Scale(w, numerator, denominator);
        }
    }

    internal static void UpdateComponents(
        CsrMatrix matrix, double[] w, double[] h, int k, NmfBetaLoss loss, Workspace workspace)
    {
        double[] numerator = workspace.FeaturesByRank;

        if (loss == NmfBetaLoss.KullbackLeibler)
        {
            double[] ratio = SparseRatio(matrix, w, h, k, workspace.Ratio);
            TransposeTimesSparsePattern(matrix, ratio, w, k, numerator);
            ScaleByRank(h, numerator, SumsPerRank(w, k, workspace.Sums), k);

            // scikit-learn snaps H below machine epsilon to zero for β ≤ 1, and only there.
            for (int i = 0; i < h.Length; i++)
            {
                if (h[i] < BetaDivergence.MachineEpsilon)
                {
                    h[i] = 0;
                }
            }
        }
        else
        {
            TransposeTimesSparsePattern(matrix, matrix.Values, w, k, numerator);    // Wᵀ X
            double[] wtw = DenseBlock.TransposeGram(w, matrix.RowCount, k);
            double[] denominator = workspace.FeaturesByRankDenominator;
            GramTimesColumns(wtw, h, k, denominator);
            Scale(h, numerator, denominator);
        }
    }

    /// <summary><c>H Hᵀ</c> for a column-major <c>H</c>, its upper triangle summed and mirrored.</summary>
    /// <remarks><c>H[a, j]·H[b, j]</c> and <c>H[b, j]·H[a, j]</c> are the same product, summed over j in the same order.</remarks>
    internal static double[] Gram(double[] h, int k, double[] result)
    {
        Array.Clear(result, 0, k * k);
        for (int j = 0; j < h.Length; j += k)
        {
            ReadOnlySpan<double> feature = h.AsSpan(j, k);
            for (int a = 0; a < feature.Length; a++)
            {
                double left = feature[a];
                Span<double> row = result.AsSpan(a * k, k);
                for (int b = a; b < feature.Length; b++)
                {
                    row[b] += left * feature[b];
                }
            }
        }
        for (int a = 0; a < k; a++)
        {
            for (int b = a + 1; b < k; b++)
            {
                result[(b * k) + a] = result[(a * k) + b];
            }
        }
        return result;
    }

    /// <summary>The sum of each component's cells, over a block <paramref name="k"/> wide whose stride is k.</summary>
    /// <remarks>W's column sums and a column-major H's row sums alike, each summed in the order of its other index.</remarks>
    private static double[] SumsPerRank(double[] block, int k, double[] sums)
    {
        Array.Clear(sums, 0, k);
        for (int start = 0; start < block.Length; start += k)
        {
            for (int a = 0; a < k; a++)
            {
                sums[a] += block[start + a];
            }
        }
        return sums;
    }

    /// <summary><c>X / (W H)</c> at X's non-zeros, floored so the division cannot blow up.</summary>
    private static double[] SparseRatio(CsrMatrix matrix, double[] w, double[] h, int k, double[] ratio)
    {
        double[] values = matrix.Values;
        int[] columns = matrix.ColumnIndices;
        int[] pointers = matrix.RowPointers;
        for (int row = 0; row < matrix.RowCount; row++)
        {
            ReadOnlySpan<double> weights = w.AsSpan(row * k, k);
            for (int index = pointers[row]; index < pointers[row + 1]; index++)
            {
                ReadOnlySpan<double> feature = h.AsSpan(columns[index] * k, k);
                double product = 0;
                for (int a = 0; a < weights.Length; a++)
                {
                    product += weights[a] * feature[a];
                }
                ratio[index] = values[index] / Math.Max(product, BetaDivergence.MachineEpsilon);
            }
        }
        return ratio;
    }

    /// <summary><c>S Hᵀ</c> into <paramref name="result"/>, where S shares the matrix's sparsity and carries <paramref name="data"/>.</summary>
    private static void SparsePatternTimesTranspose(
        CsrMatrix matrix, double[] data, double[] h, int k, double[] result)
    {
        Array.Clear(result, 0, result.Length);
        int[] columns = matrix.ColumnIndices;
        int[] pointers = matrix.RowPointers;
        for (int row = 0; row < matrix.RowCount; row++)
        {
            Span<double> target = result.AsSpan(row * k, k);
            for (int index = pointers[row]; index < pointers[row + 1]; index++)
            {
                double value = data[index];
                ReadOnlySpan<double> feature = h.AsSpan(columns[index] * k, k);
                for (int a = 0; a < target.Length; a++)
                {
                    target[a] += value * feature[a];
                }
            }
        }
    }

    /// <summary><c>Wᵀ S</c>, column-major, into <paramref name="result"/>, where S shares the matrix's sparsity.</summary>
    private static void TransposeTimesSparsePattern(
        CsrMatrix matrix, double[] data, double[] w, int k, double[] result)
    {
        Array.Clear(result, 0, result.Length);
        int[] columns = matrix.ColumnIndices;
        int[] pointers = matrix.RowPointers;
        for (int row = 0; row < matrix.RowCount; row++)
        {
            ReadOnlySpan<double> weights = w.AsSpan(row * k, k);
            for (int index = pointers[row]; index < pointers[row + 1]; index++)
            {
                double value = data[index];
                Span<double> target = result.AsSpan(columns[index] * k, k);
                for (int a = 0; a < target.Length; a++)
                {
                    target[a] += weights[a] * value;
                }
            }
        }
    }

    /// <summary><c>W · (H Hᵀ)</c>, row-major, into <paramref name="result"/>.</summary>
    private static void DenseProduct(double[] w, int k, double[] hht, double[] result)
    {
        Array.Clear(result, 0, result.Length);
        for (int start = 0; start < w.Length; start += k)
        {
            Span<double> target = result.AsSpan(start, k);
            for (int t = 0; t < k; t++)
            {
                double value = w[start + t];
                ReadOnlySpan<double> source = hht.AsSpan(t * k, k);
                for (int j = 0; j < target.Length; j++)
                {
                    target[j] += value * source[j];
                }
            }
        }
    }

    /// <summary><c>(WᵀW) · H</c> for a column-major H, into a column-major <paramref name="result"/>.</summary>
    /// <remarks>Cell <c>(a, j)</c> sums over <c>t</c> in the order the row-major product did.</remarks>
    private static void GramTimesColumns(double[] wtw, double[] h, int k, double[] result)
    {
        for (int j = 0; j < h.Length; j += k)
        {
            ReadOnlySpan<double> feature = h.AsSpan(j, k);
            for (int a = 0; a < k; a++)
            {
                ReadOnlySpan<double> gramRow = wtw.AsSpan(a * k, k);
                double sum = 0;
                for (int t = 0; t < feature.Length; t++)
                {
                    sum += gramRow[t] * feature[t];
                }
                result[j + a] = sum;
            }
        }
    }

    /// <summary><c>factor *= numerator / denominator</c>, with a zero denominator floored.</summary>
    private static void Scale(double[] factor, double[] numerator, double[] denominator)
    {
        for (int i = 0; i < factor.Length; i++)
        {
            factor[i] *= numerator[i] / Floored(denominator[i]);
        }
    }

    /// <summary><see cref="Scale"/> where every cell of component <c>a</c> shares the denominator <c>sums[a]</c>.</summary>
    private static void ScaleByRank(double[] factor, double[] numerator, double[] sums, int k)
    {
        for (int start = 0; start < factor.Length; start += k)
        {
            for (int a = 0; a < k; a++)
            {
                factor[start + a] *= numerator[start + a] / Floored(sums[a]);
            }
        }
    }

    private static double Floored(double denominator)
    {
        // S1244: the comparison is exact on purpose, and so is scikit-learn's own
        // `denominator[denominator == 0] = EPSILON` — a denominator merely near zero
        // still divides, and replacing it would move the answer away from the reference.
#pragma warning disable S1244
        return denominator == 0 ? BetaDivergence.MachineEpsilon : denominator;
#pragma warning restore S1244
    }

    /// <summary>The buffers one fit's updates overwrite on every iteration, allocated once.</summary>
    /// <remarks>At a large vocabulary each is on the large-object heap, which a per-iteration allocation kept collecting.</remarks>
    internal sealed class Workspace(CsrMatrix matrix, int k, NmfBetaLoss loss)
    {
        internal double[] RowsByRank { get; } = new double[checked(matrix.RowCount * k)];

        internal double[] RowsByRankDenominator { get; } = new double[checked(matrix.RowCount * k)];

        internal double[] FeaturesByRank { get; } = new double[checked(matrix.ColumnCount * k)];

        internal double[] FeaturesByRankDenominator { get; } = new double[checked(matrix.ColumnCount * k)];

        internal double[] Ratio { get; } =
            loss == NmfBetaLoss.KullbackLeibler ? new double[matrix.Values.Length] : [];

        internal double[] RankByRank { get; } = new double[checked(k * k)];

        internal double[] Sums { get; } = new double[k];
    }
}
