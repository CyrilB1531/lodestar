using Lodestar.Text.Stemming;

namespace Lodestar.Sample;

/// <summary>The 1980 original, still the baseline every other is compared to, and Martin's extensions to it.</summary>
internal static class PorterStemmerSample
{
    public static void Run()
    {
        Console.WriteLine($"  Porter(running)                     = {PorterStemmer.Stem("running")}");
        Console.WriteLine($"  Porter(analogy)                     = {PorterStemmer.Stem("analogy", PorterStemmerMode.OriginalAlgorithm)}");
        Console.WriteLine($"  Porter(analogy, Martin)             = {PorterStemmer.Stem("analogy", PorterStemmerMode.MartinExtensions)}");
    }
}
