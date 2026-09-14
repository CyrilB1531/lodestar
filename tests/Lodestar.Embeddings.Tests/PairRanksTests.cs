using Xunit;
using Lodestar.Embeddings.Tokenization;

namespace Lodestar.Embeddings.Tests;

/// <summary>
/// The open-addressing table <see cref="BpeTokenizer"/> resolves a pair of symbol ids to its merge
/// rank through, in place of a <c>Dictionary&lt;long, int&gt;</c> whose hash collided pairs (#673).
/// </summary>
public class PairRanksTests
{
    [Fact]
    public void A_pair_and_its_mirror_keep_separate_ranks()
    {
        // (1, 2), (2, 1) and (0, 3) share the exclusive or long.GetHashCode reduced them to.
        var ranks = new PairRanks(3);
        ranks.Set(1, 2, 0);
        ranks.Set(2, 1, 1);
        ranks.Set(0, 3, 2);

        Assert.True(ranks.TryGetRank(1, 2, out int first));
        Assert.True(ranks.TryGetRank(2, 1, out int second));
        Assert.True(ranks.TryGetRank(0, 3, out int third));
        Assert.Equal([0, 1, 2], new[] { first, second, third });
        Assert.False(ranks.TryGetRank(3, 0, out _));
    }

    [Fact]
    public void Setting_a_pair_again_replaces_its_rank()
    {
        var ranks = new PairRanks(2);
        ranks.Set(5, 7, 0);
        ranks.Set(5, 7, 1);

        Assert.True(ranks.TryGetRank(5, 7, out int rank));
        Assert.Equal(1, rank);
    }

    [Fact]
    public void A_table_filled_to_its_sizing_finds_every_pair_and_no_other()
    {
        const int Pairs = 5000;
        var ranks = new PairRanks(Pairs);
        for (int i = 0; i < Pairs; i++)
        {
            ranks.Set(i, Pairs - i, i);
        }

        for (int i = 0; i < Pairs; i++)
        {
            Assert.True(ranks.TryGetRank(i, Pairs - i, out int rank));
            Assert.Equal(i, rank);
            Assert.False(ranks.TryGetRank(Pairs - i, i + Pairs, out _));
        }
    }

    [Fact]
    public void An_empty_table_finds_nothing()
    {
        Assert.False(new PairRanks(0).TryGetRank(0, 0, out _));
    }
}
