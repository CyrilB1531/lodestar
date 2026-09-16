using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>What a fitted one-hot encoder reports, and the layout it produces.</summary>
internal static class OneHotEncoderSample
{
    public static void Run()
    {
        Console.WriteLine("OneHotEncoder (Lodestar.Preprocessing)");

        string[] values = ["b", "a", "c", "a"];
        OneHotEncoder<string> encoder = Encoders.OneHot(values, featureCount: 1);

        Console.WriteLine($"  fitted on        : {encoder.SampleCount} rows x {encoder.FeatureCount} features");
        Console.WriteLine($"  categories       : [{string.Join(", ", encoder.Categories[0])}]");
        Console.WriteLine($"  columns          : {encoder.EncodedFeatureCount}");
        Console.WriteLine($"  encoded          : {Inv.List(encoder.Transform(values))}");
        Console.WriteLine();
    }
}
