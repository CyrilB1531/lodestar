using Lodestar.Text.Stemming;

namespace Lodestar.Sample;

/// <summary>The Snowball stemmer for ro.</summary>
internal static class RomanianSnowballStemmerSample
{
    public static void Run()
    {
        Console.WriteLine($"  ro copilului                        = {RomanianSnowballStemmer.Stem("copilului")}");
    }
}
