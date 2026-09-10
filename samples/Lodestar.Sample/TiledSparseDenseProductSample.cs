using Lodestar.Gpu.Compute;

namespace Lodestar.Sample;

/// <summary>A sparse matrix times a dense block, on the accelerator.</summary>
internal static class TiledSparseDenseProductSample
{
    public static void Run()
    {
        Console.WriteLine("TiledSparseDenseProduct (Lodestar.Gpu.Compute)");

        using GpuContext context = GpuCorpus.Open();
        using var matrix = DeviceSparseMatrix.Upload(
            context, GpuCorpus.RowPointers, GpuCorpus.ColumnIndices, GpuCorpus.Values, 2, 2);
        var kernel = new TiledSparseDenseProduct(context);

        double[] identity = [1.0, 0.0, 0.0, 1.0];
        double[] product = kernel.Multiply(matrix, identity, width: 2);

        Console.WriteLine($"  row 0            : {Inv.F1(product[0])} {Inv.F1(product[1])}");
        Console.WriteLine($"  row 1            : {Inv.F1(product[2])} {Inv.F1(product[3])}");
        Console.WriteLine();
    }
}
