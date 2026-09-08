using Lodestar.Text.Phonetics;

namespace Lodestar.Sample;

/// <summary>Two codes rather than one, so Smith and Schmidt meet on the alternate.</summary>
internal static class DoubleMetaphoneSample
{
    public static void Run()
    {
        DoubleMetaphoneCode smith = DoubleMetaphone.Encode("Smith");
        DoubleMetaphoneCode schmidt = DoubleMetaphone.Encode("Schmidt");
        Console.WriteLine($"  DoubleMetaphone(Smith)         = {smith.Primary} / {smith.Secondary}");
        Console.WriteLine($"  DoubleMetaphone(Schmidt)       = {schmidt.Primary} / {schmidt.Secondary}");
        Console.WriteLine($"  they meet on                   = {smith.Secondary == schmidt.Primary}");
    }
}
