using Lodestar.Gpu.Compute;

namespace Lodestar.Sample;

/// <summary>Token hashes held flat on the device, one row per document.</summary>
internal static class DeviceTokenHashesSample
{
    public static void Run()
    {
        Console.WriteLine("DeviceTokenHashes (Lodestar.Gpu.Compute)");

        using GpuContext context = GpuCorpus.Open();

        // Hashes rather than tokens: a kernel parameter has to be blittable, and the host
        // keeps the hashing because it is the cheap, sequential half.
        using var resident = DeviceTokenHashes.Upload(context, GpuCorpus.DocumentHashes);

        Console.WriteLine($"  documents held   : {resident.Count}");
        Console.WriteLine("  an empty document is legal; an empty batch is not");
        Console.WriteLine();
    }
}
