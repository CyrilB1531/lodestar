using Lodestar.Gpu.Compute;

namespace Lodestar.Sample;

/// <summary>The record a device sweep hands back.</summary>
internal static class GpuSearchResultSample
{
    public static void Run()
    {
        Console.WriteLine("GpuSearchResult (Lodestar.Gpu.Compute)");

        // An index, not an identifier: it means a row of the matrix that was uploaded,
        // and mapping it back to a document of your own is the caller's job.
        var hit = new GpuSearchResult(Index: 2, Score: 0.75f);
        Console.WriteLine($"  row {hit.Index} scored {Inv.F3(hit.Score)}");
        Console.WriteLine();
    }
}
