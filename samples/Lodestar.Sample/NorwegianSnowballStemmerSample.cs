using Lodestar.Text.Stemming;

namespace Lodestar.Sample;

/// <summary>The Snowball stemmer for no.</summary>
internal static class NorwegianSnowballStemmerSample
{
    public static void Run()
    {
        Console.WriteLine($"  no mulighetene                      = {NorwegianSnowballStemmer.Stem("mulighetene")}");
    }
}
