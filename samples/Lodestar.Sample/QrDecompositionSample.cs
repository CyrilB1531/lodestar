using Lodestar.Decomposition;

namespace Lodestar.Sample;

/// <summary>The thin QR, and the two properties that make it readable.</summary>
internal static class QrDecompositionSample
{
    public static void Run()
    {
        Console.WriteLine("Thin QR (Lodestar.Decomposition)");

        // A 3 x 2 matrix, row-major.
        double[] matrix = [1.0, 2.0, 3.0, 4.0, 5.0, 7.0];

        QrDecomposition qr = QrDecomposition.Householder(matrix, rowCount: 3, columnCount: 2);
        Console.WriteLine($"  shape            : {qr.RowCount}x{qr.ColumnCount}, Q holds {qr.Q.Count}, R holds {qr.R.Count}");

        // R is upper triangular, so the entry below its diagonal is zero.
        Console.WriteLine($"  R below diagonal = {Inv.F1(qr.R[2])}");

        // Q's columns are orthonormal: the first has unit length.
        double lengthSquared = (qr.Q[0] * qr.Q[0]) + (qr.Q[2] * qr.Q[2]) + (qr.Q[4] * qr.Q[4]);
        Console.WriteLine($"  |first column|^2 = {Inv.F5(lengthSquared)}");
        Console.WriteLine();
    }
}
