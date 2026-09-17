using Lodestar.Abstractions;

namespace Lodestar.Decomposition.Internal;

/// <summary>scikit-learn's <c>_beta_divergence(..., square_root=True)</c>.</summary>
/// <remarks>
/// Both branches avoid densifying <c>W H</c>: the Frobenius one expands the squared norm into
/// three traces, and the Kullback–Leibler one needs <c>W H</c> only where the matrix is non-zero
/// plus one rank-one correction for everywhere else. H is column-major, as
/// <see cref="MultiplicativeUpdates"/> holds it.
/// </remarks>
internal static class BetaDivergence
{
    /// <summary><c>double.Epsilon</c> is not this: it is numpy's <c>finfo(float64).eps</c>.</summary>
    internal const double MachineEpsilon = 2.220446049250313e-16;

    internal static double Compute(
        CsrMatrix matrix, double[] w, double[] h, int componentCount, NmfBetaLoss loss)
    {
        double residual = loss == NmfBetaLoss.KullbackLeibler
            ? KullbackLeibler(matrix, w, h, componentCount)
            : Frobenius(matrix, w, h, componentCount);

        // Rounding can push the residual just below zero on a near-perfect fit.
        return Math.Sqrt(2.0 * Math.Max(residual, 0));
    }

    private static double Frobenius(CsrMatrix matrix, double[] w, double[] h, int k)
    {
        // Expanded into three traces rather than a residual, so WH is never formed: the
        // squared norm of X, the trace of HᵀWᵀWH, and twice the trace of WᵀXHᵀ.
        double normX = 0;
        foreach (double value in matrix.Values)
        {
            normX += value * value;
        }

        return (normX + NormOfProduct(matrix, w, h, k) - (2.0 * Cross(matrix, w, h, k))) / 2.0;
    }

    /// <summary><c>tr(HᵀWᵀWH)</c>, through the two Gram matrices rather than through <c>W H</c>.</summary>
    private static double NormOfProduct(CsrMatrix matrix, double[] w, double[] h, int k)
    {
        double[] wtw = DenseBlock.TransposeGram(w, matrix.RowCount, k);
        double[] hht = MultiplicativeUpdates.Gram(h, k, new double[checked(k * k)]);

        double total = 0;
        for (int a = 0; a < k; a++)
        {
            for (int b = 0; b < k; b++)
            {
                total += wtw[(a * k) + b] * hht[(a * k) + b];
            }
        }
        return total;
    }

    /// <summary><c>tr(WᵀXHᵀ)</c>, over the matrix's non-zeros only.</summary>
    private static double Cross(CsrMatrix matrix, double[] w, double[] h, int k)
    {
        double[] values = matrix.Values;
        int[] columns = matrix.ColumnIndices;
        int[] pointers = matrix.RowPointers;
        double total = 0;
        for (int row = 0; row < matrix.RowCount; row++)
        {
            ReadOnlySpan<double> weights = w.AsSpan(row * k, k);
            for (int index = pointers[row]; index < pointers[row + 1]; index++)
            {
                double value = values[index];
                ReadOnlySpan<double> feature = h.AsSpan(columns[index] * k, k);
                for (int a = 0; a < weights.Length; a++)
                {
                    total += value * weights[a] * feature[a];
                }
            }
        }
        return total;
    }

    private static double KullbackLeibler(CsrMatrix matrix, double[] w, double[] h, int k)
    {

        double residual = 0;
        double dataSum = 0;
        for (int row = 0; row < matrix.RowCount; row++)
        {
            for (int index = matrix.RowPointers[row]; index < matrix.RowPointers[row + 1]; index++)
            {
                double value = matrix.Values[index];
                // A zero entry contributes nothing: 0 · log(0/x) is defined as 0 here, which
                // is what skipping it means.
                if (value <= MachineEpsilon)
                {
                    continue;
                }
                ReadOnlySpan<double> feature = h.AsSpan(matrix.ColumnIndices[index] * k, k);
                double product = 0;
                for (int a = 0; a < k; a++)
                {
                    product += w[(row * k) + a] * feature[a];
                }
                residual += value * Math.Log(value / Math.Max(product, MachineEpsilon));
                dataSum += value;
            }
        }

        // Σ WH over every cell, as (Σ columns of W) · (Σ rows of H) — a rank-one identity,
        // so the zeros cost nothing.
        double sumWh = 0;
        for (int a = 0; a < k; a++)
        {
            double columnSum = 0;
            for (int i = 0; i < matrix.RowCount; i++)
            {
                columnSum += w[(i * k) + a];
            }
            double rowSum = 0;
            for (int j = 0; j < h.Length; j += k)
            {
                rowSum += h[j + a];
            }
            sumWh += columnSum * rowSum;
        }

        return residual + sumWh - dataSum;
    }
}
