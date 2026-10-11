using Lodestar.Abstractions;
using Lodestar.Preprocessing.Internal;

namespace Lodestar.Preprocessing;

/// <summary>
/// Scales each row to unit norm, at <c>sklearn.preprocessing.Normalizer</c> parity.
/// </summary>
/// <remarks>
/// The one member of this package that fits nothing: every row is scaled by its own norm, so
/// there is no statistic to learn and no state to carry — the reference is a transformer all the
/// same, with a <c>fit</c> that only validates, so this is static.
/// Rows, not features. Every other member here scales a column; this one scales a row, which is
/// what a distance or a dot product between two rows reads afterwards.
/// </remarks>
public static class Normalizer
{
    /// <summary>Scales each row of a row-major matrix to unit norm.</summary>
    /// <param name="samples">The matrix, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="norm">Which norm each row is scaled by.</param>
    /// <returns>A new matrix of the same shape; the input is never written to.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, or <paramref name="norm"/> is not a defined value.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    public static double[] Transform(
        ReadOnlySpan<double> samples, int featureCount, RowNorm norm = RowNorm.L2)
    {
        Guard.NotLessThan(featureCount, 1);
        RequireDefined(norm);
        int sampleCount = SampleMatrix.Rows(samples, featureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        // One loop per norm, the norm chosen once: a single loop holding all three inlined made the JIT's code for the
        // row loop half again as slow on short rows.
        return norm switch
        {
            RowNorm.L1 => Scaled(samples, featureCount, sampleCount, default(Manhattan)),
            RowNorm.L2 => Scaled(samples, featureCount, sampleCount, default(Euclidean)),
            _ => Scaled(samples, featureCount, sampleCount, default(Largest)),
        };
    }

    /// <summary>Scales each row of a sparse matrix to unit norm.</summary>
    /// <param name="matrix">The matrix to read; never written to.</param>
    /// <param name="norm">Which norm each row is scaled by.</param>
    /// <returns>A new matrix with the same stored positions and scaled values.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="norm"/> is not a defined value.</exception>
    /// <exception cref="ArgumentException"><paramref name="matrix"/> stores a non-finite value.</exception>
    /// <remarks>
    /// The stored positions are kept rather than recomputed: scaling a row by a positive number
    /// turns no stored value into a zero that was not one already, and the reference keeps its
    /// sparsity the same way. Unlike the dense overload, a norm is not floored: the reference's
    /// sparse kernels divide by any norm that is not exactly zero.
    /// </remarks>
    public static CsrMatrix Transform(CsrMatrix matrix, RowNorm norm = RowNorm.L2)
    {
        Guard.NotNull(matrix);
        RequireDefined(norm);
        SampleMatrix.RequireFiniteAsChecked(matrix.Values, nameof(matrix));

        double[] values = new double[matrix.Values.Length];
        for (int row = 0; row < matrix.RowCount; row++)
        {
            int start = matrix.RowPointers[row];
            int end = matrix.RowPointers[row + 1];
            double length = Length(matrix.Values.AsSpan(start, end - start), norm);

            // S1244: only a row whose every stored value is an exact zero is left undivided, as the reference does.
#pragma warning disable S1244
            double divisor = length == 0.0 ? 1.0 : length;
#pragma warning restore S1244
            for (int i = start; i < end; i++)
            {
                values[i] = matrix.Values[i] / divisor;
            }
        }

        return new CsrMatrix(
            matrix.RowCount,
            matrix.ColumnCount,
            values,
            (int[])matrix.ColumnIndices.Clone(),
            (int[])matrix.RowPointers.Clone());
    }

    private static double[] Scaled<TNorm>(ReadOnlySpan<double> samples, int featureCount, int sampleCount, TNorm norm)
        where TNorm : struct, IDenseNorm
    {
        var scaled = new double[samples.Length];
        for (int row = 0; row < sampleCount; row++)
        {
            int start = row * featureCount;
            // The reference floors a dense norm below 10 · eps to 1, as it floors a scale: a row of
            // [1e-17, 0] is left alone rather than blown up to [1, 0] (#1230).
            double divisor = ScaleFloor.Of(norm.Of(samples.Slice(start, featureCount)));
            for (int feature = 0; feature < featureCount; feature++)
            {
                scaled[start + feature] = samples[start + feature] / divisor;
            }
        }

        return scaled;
    }

    /// <summary>
    /// A dense row's norm in the reference's order: <c>np.abs(X).sum(axis=1)</c> pairwise for L1, <c>row_norms</c>'
    /// <c>einsum</c> for L2, so the quotient lands on scikit-learn's last bit — L2 against an x86-64 numpy, whose
    /// baseline kernel does not fuse its multiply-adds; an Arm build's does, and can land an ulp away.
    /// </summary>
    private interface IDenseNorm
    {
        double Of(ReadOnlySpan<double> row);
    }

    private readonly struct Manhattan : IDenseNorm
    {
        public double Of(ReadOnlySpan<double> row) => NumpyReduction.AbsSum(row);
    }

    private readonly struct Euclidean : IDenseNorm
    {
        public double Of(ReadOnlySpan<double> row) => Math.Sqrt(NumpyReduction.EinsumSquares(row));
    }

    private readonly struct Largest : IDenseNorm
    {
        public double Of(ReadOnlySpan<double> row) => LargestMagnitude(row);
    }

    /// <summary>The Max norm, dense or sparse alike: the largest magnitude, an exact pick in any order.</summary>
    private static double LargestMagnitude(ReadOnlySpan<double> row)
    {
        double largest = 0.0;
        foreach (double value in row)
        {
            // A select rather than a branch: on random rows the new largest is a coin toss the predictor loses.
            double magnitude = Math.Abs(value);
            largest = magnitude > largest ? magnitude : largest;
        }

        return largest;
    }

    /// <summary>A sparse row's norm, summed in order as the reference's <c>inplace_csr_row_normalize_*</c> kernels sum it.</summary>
    private static double Length(ReadOnlySpan<double> row, RowNorm norm)
    {
        double total = 0.0;
        switch (norm)
        {
            case RowNorm.L1:
                for (int i = 0; i < row.Length; i++)
                {
                    total += Math.Abs(row[i]);
                }

                return total;

            case RowNorm.Max:
                return LargestMagnitude(row);

            default:
                for (int i = 0; i < row.Length; i++)
                {
                    total += row[i] * row[i];
                }

                return Math.Sqrt(total);
        }
    }

    private static void RequireDefined(RowNorm norm)
    {
        if (norm is not (RowNorm.L1 or RowNorm.L2 or RowNorm.Max))
        {
            throw new ArgumentOutOfRangeException(nameof(norm), norm, "Not a defined row norm.");
        }
    }
}
