using Lodestar.Preprocessing;

namespace Lodestar.Sample;

/// <summary>A code per label, for the target column a classifier reports back.</summary>
internal static class LabelEncoderSample
{
    public static void Run()
    {
        Console.WriteLine("LabelEncoder (Lodestar.Preprocessing)");

        string[] cities = ["paris", "lyon", "paris", "nice"];

        LabelEncoder<string> encoder = Encoders.Label<string>(cities);
        Console.WriteLine($"  fitted on        : {encoder.SampleCount} labels");
        Console.WriteLine($"  classes          : {string.Join(",", encoder.Classes)}");
        Console.WriteLine($"  codes            : {string.Join(",", encoder.Transform(cities))}");

        // The round trip is exact: nothing was approximated on the way in.
        Console.WriteLine($"  predictions back : {string.Join(",", encoder.InverseTransform([2, 0]))}");
        Console.WriteLine();
    }
}
