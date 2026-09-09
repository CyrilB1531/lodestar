using Lodestar.Text.Stemming;

namespace Lodestar.Sample;

/// <summary>The Snowball stemmer for hu.</summary>
internal static class HungarianSnowballStemmerSample
{
    public static void Run()
    {
        Console.WriteLine($"  hu gyűrűk                           = {HungarianSnowballStemmer.Stem("gyűrűk")}");
    }
}
