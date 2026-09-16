using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Encoding categories two ways, and what the column layout looks like.</summary>
internal static class EncodersSample
{
    public static void Run()
    {
        Console.WriteLine("Encoders (Lodestar.Preprocessing)");

        // Two features per row: three categories then two.
        string[] values = ["b", "x", "a", "y", "c", "x", "a", "y"];

        OneHotEncoder<string> oneHot = Encoders.OneHot(values, featureCount: 2);
        Console.WriteLine($"  fitted on        : {oneHot.SampleCount} rows x {oneHot.FeatureCount} features");
        Console.WriteLine($"  categories       : [{string.Join(", ", oneHot.Categories[0])}] and [{string.Join(", ", oneHot.Categories[1])}]");
        Console.WriteLine($"  columns          : {oneHot.EncodedFeatureCount}");
        Console.WriteLine($"  row a,y          : {Inv.List(oneHot.Transform(["a", "y"]))}");

        // Integers sort as numbers where strings sort by code point, which is why
        // the encoders are generic rather than taking everything as text.
        OrdinalEncoder<int> ordinal = Encoders.Ordinal<int>([10, 2, 33, 2], featureCount: 1);
        Console.WriteLine($"  integer codes    : {Inv.List(ordinal.Transform([10, 2, 33, 2]))}");
        Console.WriteLine($"  its categories   : [{string.Join(", ", ordinal.Categories[0])}]");
        Console.WriteLine($"  fitted on        : {ordinal.SampleCount} rows x {ordinal.FeatureCount} features");
        Console.WriteLine();
    }
}
