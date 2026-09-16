using Lodestar.Text.Vectorization;

namespace Lodestar.Sample;

/// <summary>The three documents the search samples share.</summary>
/// <remarks>
/// Small on purpose: "cat" is in one document and "sat" in two of three, which is what makes
/// the IDF variants visibly disagree without needing a corpus nobody can read.
/// </remarks>
internal static class Corpus
{
    /// <summary>Two series that move together, row-major in time, for the vector autoregression samples (#786).</summary>
    public static double[] VarSeries { get; } =
    [
        0.1968, -0.1307, 0.2167, -0.8291, 0.2534, 0.0921, 0.2934, -0.0748, -0.2623, 0.2421,
        0.4235, -0.3523, -0.2909, 0.4475, -0.2417, -0.9916, -0.3567, -0.3337, -1.0869, -0.5651,
        -0.5519, -1.0074, -1.2172, -0.1457, 0.2603, -0.7061, -0.7272, 0.3055, 0.3998, -0.6067,
    ];

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
