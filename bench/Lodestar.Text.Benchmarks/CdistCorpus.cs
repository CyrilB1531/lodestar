namespace Lodestar.Text.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

/// <summary>
/// The phrases the two <c>Cdist</c> benchmarks score, drawn once here so the fixed-width corpus
/// and the varied-width one differ in the draw rather than in two copies of it (#1134).
/// </summary>
internal static class CdistCorpus
{
    private static readonly string[] Words =
    [
        "new", "york", "boston", "atlanta", "brooklyn", "angeles", "lakers", "mets", "braves",
        "red", "sox", "knicks",
    ];

    /// <summary>Draws <paramref name="count"/> phrases of <paramref name="minWords"/> to <paramref name="maxWords"/> words.</summary>
    /// <remarks>
    /// Equal bounds draw no word count at all, which is what keeps the three-word corpus the
    /// same sequence it was before the varied one existed, and its recorded numbers comparable.
    /// </remarks>
    internal static string[] Phrases(int count, int seed, int minWords = 3, int maxWords = 3)
    {
        var random = new Random(seed);
        var phrases = new string[count];
        for (int i = 0; i < count; i++)
        {
            int words = minWords == maxWords ? minWords : random.Next(minWords, maxWords + 1);
            var phrase = new System.Text.StringBuilder();
            for (int w = 0; w < words; w++)
            {
                phrase.Append(Words[random.Next(Words.Length)]).Append(' ');
            }

            phrases[i] = phrase.Append(i.ToString(System.Globalization.CultureInfo.InvariantCulture)).ToString();
        }

        return phrases;
    }
}
