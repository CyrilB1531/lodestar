using Lodestar.Gpu.Compute;

namespace Lodestar.Sample;

/// <summary>Myers' edit distance from one pattern to a whole batch.</summary>
internal static class BitParallelEditDistanceSample
{
    public static void Run()
    {
        Console.WriteLine("BitParallelEditDistance (Lodestar.Gpu.Compute)");

        using GpuContext context = GpuCorpus.Open();
        string[] texts = ["sitting", "kitten", "mitten", ""];
        using var block = DeviceTextBlock.Upload(context, "kitten", texts);
        var kernel = new BitParallelEditDistance(context);

        int[] distances = kernel.Distance("kitten", block);
        for (int i = 0; i < texts.Length; i++)
        {
            string shown = texts[i].Length == 0 ? "(empty)" : texts[i];
            Console.WriteLine($"  kitten -> {shown,-8}: {distances[i]}");
        }

        Console.WriteLine($"  longest pattern  : {BitParallelEditDistance.MaxPatternLength} characters");
        Console.WriteLine();
    }
}
