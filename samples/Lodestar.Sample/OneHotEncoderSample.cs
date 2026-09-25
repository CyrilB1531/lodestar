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

        // Infrequent categories share one column, and the sparse output stores only the ones.
        OneHotEncoder<string> grouped = Encoders.OneHot(
            ["a", "a", "a", "b", "b", "c", "d"], 1, new OneHotEncoderOptions { MinFrequency = 2 });
        Lodestar.Abstractions.CsrMatrix sparse = grouped.TransformSparse(["a", "d"]);
        Console.WriteLine($"  infrequent       : [{string.Join(", ", grouped.InfrequentCategories[0]!)}]");
        Console.WriteLine($"  names            : {string.Join(", ", grouped.FeatureNames())}");
        Console.WriteLine($"  sparse           : {sparse.NonZeroCount} stored, columns {string.Join(",", sparse.ColumnIndices)}");
        Console.WriteLine();
    }
}
