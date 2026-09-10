using Lodestar.Gpu.Compute;

namespace Lodestar.Sample;

/// <summary>Two products chained, with the intermediate never leaving the accelerator.</summary>
internal static class DeviceDenseBlockSample
{
    public static void Run()
    {
        Console.WriteLine("DeviceDenseBlock (Lodestar.Gpu.Compute)");

        using GpuContext context = GpuCorpus.Open();
        using var matrix = DeviceSparseMatrix.Upload(
            context, GpuCorpus.RowPointers, GpuCorpus.ColumnIndices, GpuCorpus.Values, 2, 2);
        var kernel = new TiledSparseDenseProduct(context);

        using var operand = DeviceDenseBlock.Upload(context, [1.0, 0.0, 0.0, 1.0], 2, 2);
        using DeviceDenseBlock first = kernel.Multiply(matrix, operand);

        // The chain: first's result is the type the next product takes, so nothing crosses
        // the bus between the two. Download is where the chain ends.
        using DeviceDenseBlock second = kernel.Multiply(matrix, first);

        double[] values = second.Download();
        Console.WriteLine($"  chained shape    : {second.RowCount} x {second.ColumnCount}");
        Console.WriteLine($"  row 0            : {Inv.F1(values[0])} {Inv.F1(values[1])}");
        Console.WriteLine();
    }
}
