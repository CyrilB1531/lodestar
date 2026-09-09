using Lodestar.Text.Stemming;

namespace Lodestar.Sample;

/// <summary>The Snowball stemmer for ru.</summary>
internal static class RussianSnowballStemmerSample
{
    public static void Run()
    {
        Console.WriteLine($"  ru городами                         = {RussianSnowballStemmer.Stem("городами")}");
    }
}
