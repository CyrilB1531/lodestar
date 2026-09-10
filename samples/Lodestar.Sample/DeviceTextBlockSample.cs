using Lodestar.Gpu.Compute;

namespace Lodestar.Sample;

/// <summary>A batch of strings renamed to symbol codes and held on the device.</summary>
internal static class DeviceTextBlockSample
{
    public static void Run()
    {
        Console.WriteLine("DeviceTextBlock (Lodestar.Gpu.Compute)");

        using GpuContext context = GpuCorpus.Open();

        // A kernel parameter has to be blittable, so the strings are renamed on the host
        // against the pattern's own alphabet before anything is uploaded.
        using var block = DeviceTextBlock.Upload(context, "kitten", ["sitting", "mitten", ""]);

        Console.WriteLine($"  strings held     : {block.Count}");
        Console.WriteLine($"  pattern alphabet : at most {DeviceTextBlock.MaxPatternAlphabet} characters");
        Console.WriteLine();
    }
}
