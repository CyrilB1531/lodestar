using Lodestar.Gpu.Compute;

namespace Lodestar.Sample;

/// <summary>A corpus uploaded once and swept many times.</summary>
internal static class DeviceEmbeddingMatrixSample
{
    public static void Run()
    {
        Console.WriteLine("DeviceEmbeddingMatrix (Lodestar.Gpu.Compute)");

        using GpuContext context = GpuCorpus.Open();
        using var matrix = DeviceEmbeddingMatrix.Upload(
            context, GpuCorpus.Vectors, count: 3, dimension: 2);

        Console.WriteLine($"  rows / dimension : {matrix.Count} / {matrix.Dimension}");
        Console.WriteLine("  rows are L2-normalized on upload, which makes cosine a dot product");
        Console.WriteLine();
    }
}
