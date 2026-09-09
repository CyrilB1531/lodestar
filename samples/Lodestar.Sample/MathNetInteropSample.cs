using Lodestar.Abstractions;
using Lodestar.Extensions.MathNet;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Lodestar.Sample;

/// <summary>The bridge to Math.NET, for a caller who already holds its types.</summary>
internal static class MathNetInteropSample
{
    public static void Run()
    {
        Console.WriteLine("Math.NET interop (Lodestar.Extensions.MathNet)");

        // Columns deliberately out of order: CsrMatrix accepts it and Math.NET searches
        // the row, so this is the case the conversion has to repair rather than pass on.
        CsrMatrix unsorted = new(1, 5, [9.0, 7.0, 8.0], [4, 0, 2], [0, 3]);

        SparseMatrix sparse = MathNetInterop.ToSparseMatrix(unsorted);
        Console.WriteLine($"  out of order -> [0,0]/[0,2]/[0,4] = {Inv.F1(sparse[0, 0])} / {Inv.F1(sparse[0, 2])} / {Inv.F1(sparse[0, 4])}");

        Matrix<double> dense = DenseMatrix.OfRowArrays(
            [0.0, 3.0, 0.0],
            [0.0, 0.0, 0.0],
            [4.0, 0.0, 5.0]);

        CsrMatrix back = MathNetInterop.ToCsrMatrix(dense);
        Console.WriteLine($"  dense 3x3    -> CSR {back.RowCount}x{back.ColumnCount}, {back.NonZeroCount} stored");
        Console.WriteLine();
    }
}
