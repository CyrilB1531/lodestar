using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>Dropping a category, and the row of zeros two settings can both produce.</summary>
internal static class OneHotEncoderOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("OneHotEncoderOptions (Lodestar.Preprocessing)");

        var options = new OneHotEncoderOptions
        {
            Drop = CategoryDrop.First,
            Unknown = UnknownCategory.Ignore,
        };
        OneHotEncoder<string> encoder = Encoders.OneHot(["a", "b", "c"], 1, options);

        Console.WriteLine($"  drop / unknown   : {options.Drop} / {options.Unknown}");
        Console.WriteLine($"  dropped 'a'      : {Inv.List(encoder.Transform(["a"]))}");

        // The same row: nothing distinguishes an ignored unknown from the dropped
        // category, which is the reference's own collision.
        Console.WriteLine($"  unknown 'zzz'    : {Inv.List(encoder.Transform(["zzz"]))}");

        // if_binary drops only where a single column says everything.
        var ifBinary = new OneHotEncoderOptions { Drop = CategoryDrop.IfBinary, Unknown = UnknownCategory.Refuse };
        Console.WriteLine(
            $"  {ifBinary.Drop} on 2 / 3  : "
            + $"{Encoders.OneHot(["y", "n"], 1, ifBinary).EncodedFeatureCount} / "
            + $"{Encoders.OneHot(["a", "b", "c"], 1, ifBinary).EncodedFeatureCount} columns");
        Console.WriteLine();
    }
}
