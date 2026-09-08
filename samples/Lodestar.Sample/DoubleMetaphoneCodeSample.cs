using Lodestar.Text.Phonetics;

namespace Lodestar.Sample;

/// <summary>
/// <see cref="DoubleMetaphoneCode"/> — the pair of codes an encode returns, and the
/// empty <c>Secondary</c> that says a word has only one reading.
/// </summary>
internal static class DoubleMetaphoneCodeSample
{
    public static void Run()
    {
        Console.WriteLine("DoubleMetaphoneCode");

        DoubleMetaphoneCode two = DoubleMetaphone.Encode("Knuth");
        Console.WriteLine($"  Knuth, two readings   = {two.Primary} / {two.Secondary}");

        // Empty means "no alternate", not "no code": compare against Primary.
        DoubleMetaphoneCode one = DoubleMetaphone.Encode("Wright");
        Console.WriteLine($"  Wright, one reading   = {one.Primary}");
        Console.WriteLine($"  has an alternate      = {one.Secondary.Length > 0}");

        // The pair is constructible and compares by value, which is what lets a
        // caller group a corpus by sound with the code itself as the key.
        DoubleMetaphoneCode byHand = new("N0", "NT");
        Console.WriteLine($"  built by hand, equal  = {byHand == two}");
    }
}
