using Lodestar.Text.Stemming;

namespace Lodestar.Sample;

/// <summary>The Snowball stemmer for fi.</summary>
internal static class FinnishSnowballStemmerSample
{
    public static void Run()
    {
        Console.WriteLine($"  fi talossaan                        = {FinnishSnowballStemmer.Stem("talossaan")}");
    }
}
