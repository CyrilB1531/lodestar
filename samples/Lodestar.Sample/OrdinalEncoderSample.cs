using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>One code per category, and the order those codes carry.</summary>
internal static class OrdinalEncoderSample
{
    public static void Run()
    {
        Console.WriteLine("OrdinalEncoder (Lodestar.Preprocessing)");

        string[] values = ["b", "a", "c", "a"];
        OrdinalEncoder<string> encoder = Encoders.Ordinal(values, featureCount: 1);

        Console.WriteLine($"  fitted on        : {encoder.SampleCount} rows x {encoder.FeatureCount} features");
        Console.WriteLine($"  categories       : [{string.Join(", ", encoder.Categories[0])}]");

        // The codes are the sorted positions, so a model reading them as numbers
        // reads that order as distance -- which is what one-hot avoids.
        Console.WriteLine($"  codes            : {Inv.List(encoder.Transform(values))}");
        Console.WriteLine();
    }
}
