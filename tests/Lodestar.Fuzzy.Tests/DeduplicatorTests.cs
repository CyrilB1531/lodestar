using Lodestar.Fuzzy;
using Xunit;

namespace Lodestar.Fuzzy.Tests;

public sealed class DeduplicatorTests
{
    // Similarity used throughout: token-set ratio (order-insensitive).
    private static double Sim(string a, string b) => Fuzz.TokenSetRatio(a, b);

    [Fact]
    public void Groups_typos_within_the_same_block()
    {
        var records = new[]
        {
            "John Smith",   // 0
            "Jon Smith",    // 1  (dup of 0)
            "Jane Doe",     // 2
            "Jayne Doe",    // 3  (dup of 2)
            "Bob Brown",    // 4
        };

        // Block by first letter of the surname's initial letter of the whole string.
        IReadOnlyList<IReadOnlyList<int>> clusters =
            Deduplicator.FindClusters(records, r => r[..1], Sim, threshold: 80);

        // Expect {0,1}, {2,3}, {4} — every record placed once.
        Assert.Equal(records.Length, clusters.Sum(c => c.Count));
        Assert.Contains(clusters, c => c.Count == 2 && c.Contains(0) && c.Contains(1));
        Assert.Contains(clusters, c => c.Count == 2 && c.Contains(2) && c.Contains(3));
        Assert.Contains(clusters, c => c.Count == 1 && c.Contains(4));
    }

    [Fact]
    public void Blocking_prevents_cross_block_matches()
    {
        // Identical names but different declared blocks: never compared, so they
        // stay separate — the documented recall/speed trade-off.
        var records = new (string Name, string Block)[]
        {
            ("apple", "A"),
            ("apple", "B"),
        };
        IReadOnlyList<IReadOnlyList<int>> clusters =
            Deduplicator.FindClusters(records, r => r.Block, (x, y) => Fuzz.Ratio(x.Name, y.Name), threshold: 90);
        Assert.Equal(2, clusters.Count);

        // Same block: the identical names merge.
        var sameBlock = new (string Name, string Block)[] { ("apple", "A"), ("apple", "A") };
        IReadOnlyList<IReadOnlyList<int>> merged =
            Deduplicator.FindClusters(sameBlock, r => r.Block, (x, y) => Fuzz.Ratio(x.Name, y.Name), threshold: 90);
        Assert.Single(merged);
    }

    [Fact]
    public void Transitive_closure_merges_chained_duplicates()
    {
        // a~b and b~c but a≁c directly: all three should still cluster.
        var records = new[] { "aaaaa", "aaaab", "aaabb" };
        IReadOnlyList<IReadOnlyList<int>> clusters =
            Deduplicator.FindClusters(records, _ => "block", (a, b) => Fuzz.Ratio(a, b), threshold: 80);
        IReadOnlyList<int> cluster = Assert.Single(clusters);
        Assert.Equal(3, cluster.Count);
    }

    [Fact]
    public void A_null_blocking_key_is_refused_naming_blocking_key()
    {
        // It threw from Dictionary.TryGetValue, naming its parameter key (#1612).
        string[] records = ["a", "b"];

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => Deduplicator.FindClusters(records, r => r == "b" ? null : r, (x, y) => Fuzz.Ratio(x, y), 80));
        Assert.Equal("blockingKey", error.ParamName);
        Assert.StartsWith("The blocking key of record 1 is null.", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Null_arguments_are_refused_by_name()
    {
        string[] records = ["a"];
        Func<string, string> key = r => r;
        Func<string, string, double> similarity = (x, y) => Fuzz.Ratio(x, y);

        Assert.Equal("records", Assert.Throws<ArgumentNullException>(
            () => Deduplicator.FindClusters(null!, key, similarity, 80)).ParamName);
        Assert.Equal("blockingKey", Assert.Throws<ArgumentNullException>(
            () => Deduplicator.FindClusters(records, null!, similarity, 80)).ParamName);
        Assert.Equal("similarity", Assert.Throws<ArgumentNullException>(
            () => Deduplicator.FindClusters(records, key, null!, 80)).ParamName);
    }
}
