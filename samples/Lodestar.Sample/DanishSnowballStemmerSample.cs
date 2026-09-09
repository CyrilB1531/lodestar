using Lodestar.Text.Stemming;

namespace Lodestar.Sample;

/// <summary>The Snowball stemmer for da.</summary>
internal static class DanishSnowballStemmerSample
{
    public static void Run()
    {
        Console.WriteLine($"  da bestemmelsen                     = {DanishSnowballStemmer.Stem("bestemmelsen")}");
    }
}
