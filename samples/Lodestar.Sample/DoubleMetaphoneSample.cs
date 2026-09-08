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

        // The pair is constructible, and compares by value -- which is what lets a caller
        // group a corpus by sound with the code itself as the dictionary key.
        DoubleMetaphoneCode byHand = new("SM0", "XMT");
        Console.WriteLine($"  built by hand, equal to Smith  = {byHand == smith}");
    }
}
