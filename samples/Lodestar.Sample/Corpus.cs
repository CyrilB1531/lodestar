using Lodestar.Text.Vectorization;

namespace Lodestar.Sample;

/// <summary>The three documents the search samples share.</summary>
/// <remarks>
/// Small on purpose: "cat" is in one document and "sat" in two of three, which is what makes
/// the IDF variants visibly disagree without needing a corpus nobody can read.
/// </remarks>
internal static class Corpus
{
    public static string[] Documents { get; } =
        ["the cat sat", "the dog sat sat", "a bird flew far away today"];

    /// <summary>The column a term occupies in the fitted vocabulary.</summary>
    public static int Column(CountVectorizer vectorizer, string term)
    {
        IReadOnlyList<string> names = vectorizer.GetFeatureNames();
        for (int i = 0; i < names.Count; i++)
        {
            if (string.Equals(names[i], term, StringComparison.Ordinal))
            {
                return i;
            }
        }

        return -1;
    }
}
