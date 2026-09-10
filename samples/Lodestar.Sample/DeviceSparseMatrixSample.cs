using Lodestar.Gpu.Compute;

namespace Lodestar.Sample;

/// <summary>A CSR matrix held on the accelerator across many products.</summary>
internal static class DeviceSparseMatrixSample
{
    public static void Run()
    {
        Console.WriteLine("DeviceSparseMatrix (Lodestar.Gpu.Compute)");

        using GpuContext context = GpuCorpus.Open();

        // The three CSR arrays go in as spans: no Lodestar type crosses into this package.
        using var matrix = DeviceSparseMatrix.Upload(
            context, GpuCorpus.RowPointers, GpuCorpus.ColumnIndices, GpuCorpus.Values,
            rowCount: 2, columnCount: 2);

        Console.WriteLine($"  shape            : {matrix.RowCount} x {matrix.ColumnCount}");
        Console.WriteLine($"  stored values    : {matrix.NonZeroCount}");
        Console.WriteLine();
    }
}
