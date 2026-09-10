using Lodestar.Gpu.Compute;

namespace Lodestar.Sample;

/// <summary>Cosine similarity and top-k, for a batch of queries at once.</summary>
internal static class TiledCosineTopKSample
{
    public static void Run()
    {
        Console.WriteLine("TiledCosineTopK (Lodestar.Gpu.Compute)");

        using GpuContext context = GpuCorpus.Open();
        using var matrix = DeviceEmbeddingMatrix.Upload(
            context, GpuCorpus.Vectors, count: 3, dimension: 2);
        var kernel = new TiledCosineTopK(context);

        // Two queries in one call. A batch is what makes the kernel worth its transfers.
        float[] queries = [1f, 0f, 0f, 1f];
        IReadOnlyList<IReadOnlyList<GpuSearchResult>> hits = kernel.Search(matrix, queries, 2, 2);

        for (int query = 0; query < hits.Count; query++)
        {
            IReadOnlyList<GpuSearchResult> best = hits[query];
            Console.WriteLine($"  query {query} best : doc {best[0].Index} at {Inv.F3(best[0].Score)}");
        }

        Console.WriteLine();
    }
}
