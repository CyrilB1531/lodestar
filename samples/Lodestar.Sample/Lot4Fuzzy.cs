using Lodestar.Fuzzy;
using Lodestar.Text;

namespace Lodestar.Sample;

// SonarLint S1192: "apple pie" and its misspelling are the data of the
// demonstration, not a magic value. Each call shows the exact pair being scored,
// and hoisting them into constants would make a reader open two definitions to
// find out what is being compared to what — for a file whose only job is to be
// read.
#pragma warning disable S1192

/// <summary>
/// Lot 4 — Lodestar.Fuzzy, the rapidfuzz-compatible surface.
/// </summary>
internal static class Lot4Fuzzy
{
    private static readonly string[] Candidates = ["apple pie", "banana bread", "cherry tart", "apple tart"];

    public static void Run()
    {
        Console.WriteLine("lot 4 — fuzzy matching");

        // The seven scorers. Each is a rapidfuzz `fuzz.*` function by the same name.
        Console.WriteLine($"  Ratio                 = {Inv.F1(Fuzz.Ratio("apple pie", "appel pie"))}");
        // Over code points, as rapidfuzz counts: two emoji sharing a high surrogate share nothing.
        Console.WriteLine($"  Ratio, code points    = {Inv.F1(Fuzz.Ratio("\U0001F600", "\U0001F601", TextElement.CodePoint))}");
        Console.WriteLine($"  PartialRatio          = {Inv.F1(Fuzz.PartialRatio("apple", "an apple a day"))}");
        Console.WriteLine($"  TokenSortRatio        = {Inv.F1(Fuzz.TokenSortRatio("pie apple", "apple pie"))}");
        Console.WriteLine($"  TokenSetRatio         = {Inv.F1(Fuzz.TokenSetRatio("apple pie apple", "apple pie"))}");
        Console.WriteLine($"  PartialTokenSortRatio = {Inv.F1(Fuzz.PartialTokenSortRatio("pie apple", "the apple pie"))}");
        Console.WriteLine($"  PartialTokenSetRatio  = {Inv.F1(Fuzz.PartialTokenSetRatio("apple pie", "the apple pie today"))}");
        Console.WriteLine($"  WRatio                = {Inv.F1(Fuzz.WRatio("apple pie", "appel pie"))}");

        // Process: the same scorers applied across a collection of choices.
        ExtractResult? best = Process.ExtractOne("appel pie", Candidates, scorer: Fuzz.WRatio, scoreCutoff: 0);
        Console.WriteLine($"  ExtractOne            = {best?.Choice} ({Inv.F1(best?.Score)}) at index {best?.Index}");
        IReadOnlyList<ExtractResult> top = Process.Extract("appel pie", Candidates, scorer: Fuzz.WRatio, limit: 2, scoreCutoff: 0);
        foreach (ExtractResult hit in top)
        {
            Console.WriteLine($"    Extract #{hit.Index} {hit.Choice} ({Inv.F1(hit.Score)})");
        }

        // Cdist: two lists rather than one query, every pair scored into a matrix. Its default
        // scorer is Ratio where Extract's is WRatio, which is how the reference defaults them.
        string[] queries = ["appel pie", "banana bred"];
        ScoreMatrix scores = Process.Cdist(queries, Candidates);
        Console.WriteLine($"  Cdist                 = {scores.Rows} x {scores.Columns}, [0,0] = {Inv.F1(scores[0, 0])}");
        for (int row = 0; row < scores.Rows; row++)
        {
            ReadOnlySpan<double> window = scores.Row(row);
            Console.WriteLine($"    row {row} \"{queries[row]}\" best = {Inv.F1(Best(window))}");
        }

        // A cutoff zeroes a cell rather than dropping it: a matrix has a cell for every pair.
        double[] flat = Process.Cdist(queries, Candidates, scoreCutoff: 80.0).ToArray();
        Console.WriteLine($"  Cdist, cutoff 80      = {Inv.List(flat)}");

        // The matrix is a result to read, so equality asks whether two handles are the same one:
        // a copy of the handle is, a second call with the same inputs is not.
        ScoreMatrix sameHandle = scores;
        ScoreMatrix scoredAgain = Process.Cdist(queries, Candidates);
        Console.WriteLine($"  same handle           = {sameHandle == scores}, scored again = {scoredAgain == scores}");
        Console.WriteLine($"  equal as object       = {sameHandle.Equals((object)scores)}, hash = {sameHandle.GetHashCode() == scores.GetHashCode()}");

        // Deduplicator: blocked pairwise clustering, so the scorer never sees the
        // full cross product.
        string[] records = ["apple pie", "appel pie", "banana bread", "banana bred"];
        IReadOnlyList<IReadOnlyList<int>> clusters = Deduplicator.FindClusters(
            records,
            blockingKey: static record => record[..1],
            similarity: static (a, b) => Fuzz.Ratio(a, b),
            threshold: 80);
        Console.WriteLine($"  FindClusters          = {clusters.Count} clusters: "
            + string.Join(" | ", clusters.Select(c => "{" + string.Join(",", c) + "}")));
        Console.WriteLine();
    }
    /// <summary>The best score in one row of the matrix.</summary>
    private static double Best(ReadOnlySpan<double> row)
    {
        double best = 0.0;
        for (int i = 0; i < row.Length; i++)
        {
            best = Math.Max(best, row[i]);
        }

        return best;
    }
}
