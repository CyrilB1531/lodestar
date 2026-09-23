using Lodestar.Abstractions;
using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Scaling a row rather than a column — the one member here that works across a row.</summary>
internal static class NormalizerSample
{
    public static void Run()
    {
        Console.WriteLine("Normalizer (Lodestar.Preprocessing)");

        // Three documents as term counts; what matters is the direction, not the length.
        double[] counts = [2.0, 0.0, 1.0, 0.0, 4.0, 4.0, 1.0, 1.0, 1.0];

        foreach (RowNorm norm in (RowNorm[])[RowNorm.L1, RowNorm.L2, RowNorm.Max])
        {
            double[] unit = Normalizer.Transform(counts, featureCount: 3, norm);
            Console.WriteLine($"  {norm,-3}             : {Inv.List(unit)}");
        }

        // The sparse overload reads the stored values and returns a new matrix, leaving the
        // one it was given alone -- unlike CsrMatrix.NormalizeRows, which scales in place.
        var sparse = new CsrMatrix(
            rowCount: 3,
            columnCount: 3,
            values: [2.0, 1.0, 4.0, 4.0, 1.0, 1.0, 1.0],
            columnIndices: [0, 2, 1, 2, 0, 1, 2],
            rowPointers: [0, 2, 4, 7]);
        CsrMatrix scaled = Normalizer.Transform(sparse, RowNorm.L2);
        Console.WriteLine($"  sparse L2 stored : {Inv.List(scaled.Values)}");
        Console.WriteLine();
    }
}
