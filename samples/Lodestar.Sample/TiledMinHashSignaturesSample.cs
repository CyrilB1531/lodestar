using Lodestar.Gpu.Compute;

namespace Lodestar.Sample;

/// <summary>MinHash signatures for a whole batch, one thread per permutation.</summary>
internal static class TiledMinHashSignaturesSample
{
    public static void Run()
    {
        Console.WriteLine("TiledMinHashSignatures (Lodestar.Gpu.Compute)");

        using GpuContext context = GpuCorpus.Open();
        using var resident = DeviceTokenHashes.Upload(context, GpuCorpus.DocumentHashes);
        var kernel = new TiledMinHashSignatures(context);

        IReadOnlyList<uint[]> signatures =
            kernel.Signatures(resident, GpuCorpus.Multipliers, GpuCorpus.Addends);

        Console.WriteLine($"  documents        : {signatures.Count}");
        Console.WriteLine($"  signature length : {signatures[0].Length}");

        // The third document is empty, so every slot keeps the maximum a minimum starts from.
        bool empty = signatures[2].All(slot => slot == uint.MaxValue);
        Console.WriteLine($"  empty doc at max : {empty}");
        Console.WriteLine();
    }
}
