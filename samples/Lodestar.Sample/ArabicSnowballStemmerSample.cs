using Lodestar.Text.Stemming;

namespace Lodestar.Sample;

/// <summary>The Snowball stemmer for ar.</summary>
internal static class ArabicSnowballStemmerSample
{
    public static void Run()
    {
        Console.WriteLine($"  ar بالكتاب                          = {ArabicSnowballStemmer.Stem("بالكتاب")}");
    }
}
