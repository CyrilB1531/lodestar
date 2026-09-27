using BenchmarkDotNet.Attributes;
using Lodestar.Text.Distances;
using Lodestar.Text.Indexing;

namespace Lodestar.Text.Benchmarks;

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks.
#pragma warning disable CA1822

/// <summary>
/// <c>BkTree.Nearest</c> at a small and a large count, against the scan a caller writes instead:
/// every distance, then the nearest by a sort.
/// </summary>
/// <remarks>
/// The large count is #1199's: each hit used to be inserted into a sorted list, shifting every entry
/// behind it, where a bounded heap now holds them. Both arms are checked to agree before timing.
/// </remarks>
[MemoryDiagnoser]
public class BkTreeNearestBenchmarks
{
    private string[] words = [];
    private string[] queries = [];
    private BkTree tree = BkTree.OverLevenshtein();

    /// <summary>How many nearest items each query asks for.</summary>
    [Params(10, 2_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        // The tree keeps a word once, so the scan does too, in the same first-seen order.
        this.words = [.. BkTreeBenchmarks.Dictionary("uniform").Distinct(StringComparer.Ordinal)];
        this.tree = BkTree.OverLevenshtein();
        this.tree.AddRange(this.words);
        this.queries = [.. Enumerable.Range(0, 20).Select(i => this.words[i * 997 % this.words.Length])];

        foreach (string query in this.queries)
        {
            BkTreeMatch[] fromTree = [.. this.tree.Nearest(query, this.Count)];
            BkTreeMatch[] fromScan = [.. Scan(query)];
            if (!fromTree.SequenceEqual(fromScan))
            {
                throw new InvalidOperationException($"The two arms disagree on '{query}'.");
            }
        }
    }

    [Benchmark(Baseline = true)]
    public int FullScan()
    {
        int found = 0;
        foreach (string query in this.queries)
        {
            found += Scan(query).Count;
        }
        return found;
    }

    [Benchmark]
    public int TreeNearest()
    {
        int found = 0;
        foreach (string query in this.queries)
        {
            found += this.tree.Nearest(query, this.Count).Count;
        }
        return found;
    }

    // Insertion order breaks ties, as the tree's answer does, so the two lists compare equal.
    private List<BkTreeMatch> Scan(string query)
    {
        var all = new List<(BkTreeMatch Match, int Order)>(this.words.Length);
        for (int i = 0; i < this.words.Length; i++)
        {
            all.Add((new BkTreeMatch(this.words[i], Levenshtein.Distance(this.words[i], query)), i));
        }
        all.Sort(static (x, y) => x.Match.Distance != y.Match.Distance
            ? x.Match.Distance.CompareTo(y.Match.Distance)
            : x.Order.CompareTo(y.Order));
        return [.. all.Take(this.Count).Select(static e => e.Match)];
    }
}
