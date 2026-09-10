using Lodestar.Text.Similarity;
using Xunit;

namespace Lodestar.Text.Tests.Similarity;

/// <summary>What the banded index promises, and what it deliberately does not.</summary>
/// <remarks>
/// No oracle here: the reference's index is backed by a key-value store and returns its keys
/// in whatever order that store yields, so freezing its output would freeze a storage
/// detail. What is checkable is the banding contract, and that is what these state.
/// </remarks>
public sealed class LshIndexTests
{
    private static uint[] Signature(params uint[] slots) => slots;

    [Fact]
    public void A_signature_finds_itself()
    {
        var index = new LshIndex(new LshBanding(2, 2));
        uint[] signature = Signature(1, 2, 3, 4);
        index.Add("only", signature);

        Assert.Equal(["only"], index.Query(signature));
    }

    [Fact]
    public void One_matching_band_is_enough()
    {
        // The second band differs entirely and the first agrees, which is the whole point:
        // banding trades exactness for a lookup, and a candidate is not yet a match.
        var index = new LshIndex(new LshBanding(2, 2));
        index.Add("stored", Signature(1, 2, 3, 4));

        Assert.Equal(["stored"], index.Query(Signature(1, 2, 99, 99)));
    }

    [Fact]
    public void A_band_that_differs_in_one_slot_does_not_collide()
    {
        var index = new LshIndex(new LshBanding(2, 2));
        index.Add("stored", Signature(1, 2, 3, 4));

        Assert.Empty(index.Query(Signature(1, 99, 3, 99)));
    }

    [Fact]
    public void A_candidate_matching_several_bands_is_returned_once()
    {
        var index = new LshIndex(new LshBanding(3, 2));
        uint[] signature = Signature(1, 1, 1, 1, 1, 1);
        index.Add("stored", signature);

        Assert.Equal(["stored"], index.Query(signature));
    }

    [Fact]
    public void Two_bands_holding_the_same_slots_do_not_collide_with_each_other()
    {
        // long-comment: both bands of the stored signature hold the same two values. A
        // bucket key without the band index would merge them, so a query matching on its
        // first band would be answered through the stored second one. The index is in the key.
        var index = new LshIndex(new LshBanding(2, 2));
        index.Add("stored", Signature(7, 7, 7, 7));

        Assert.Equal(["stored"], index.Query(Signature(7, 7, 0, 0)));
        Assert.Equal(["stored"], index.Query(Signature(0, 0, 7, 7)));
        Assert.Empty(index.Query(Signature(0, 0, 0, 0)));
    }

    [Fact]
    public void Candidates_come_back_in_the_order_they_were_added()
    {
        var index = new LshIndex(new LshBanding(1, 2));
        index.Add("first", Signature(5, 5));
        index.Add("second", Signature(5, 5));
        index.Add("third", Signature(5, 5));

        Assert.Equal(["first", "second", "third"], index.Query(Signature(5, 5)));
    }

    [Fact]
    public void A_longer_signature_than_the_banding_needs_is_accepted()
    {
        // Solve returns a banding that usually consumes fewer slots than the signature
        // holds, so refusing the remainder would make its own answer unusable.
        var index = new LshIndex(new LshBanding(2, 2));
        index.Add("stored", Signature(1, 2, 3, 4, 5, 6));

        Assert.Equal(["stored"], index.Query(Signature(1, 2, 3, 4, 9, 9)));
    }

    [Fact]
    public void A_signature_shorter_than_the_banding_is_refused()
    {
        var index = new LshIndex(new LshBanding(2, 2));

        Assert.Throws<ArgumentException>(() => index.Add("short", Signature(1, 2, 3)));
    }

    [Fact]
    public void A_key_added_twice_is_refused()
    {
        var index = new LshIndex(new LshBanding(1, 2));
        index.Add("stored", Signature(1, 2));

        Assert.Throws<ArgumentException>(() => index.Add("stored", Signature(3, 4)));
    }

    [Fact]
    public void Count_tracks_the_keys_and_not_the_buckets()
    {
        var index = new LshIndex(new LshBanding(4, 1));
        index.Add("one", Signature(1, 2, 3, 4));
        index.Add("two", Signature(1, 2, 3, 4));

        Assert.Equal(2, index.Count);
    }

    [Fact]
    public void The_solved_banding_consumes_what_it_says_it_does()
    {
        LshBanding banding = LshBanding.Solve(0.95, 128);

        Assert.Equal(banding.Bands * banding.RowsPerBand, banding.Permutations);
        Assert.True(banding.Permutations <= 128);
    }

    [Fact]
    public void The_collision_curve_rises_with_similarity()
    {
        LshBanding banding = LshBanding.Solve(0.8, 128);

        Assert.True(banding.CollisionProbability(0.9) > banding.CollisionProbability(0.7));
        Assert.Equal(0.0, banding.CollisionProbability(0.0));
        Assert.Equal(1.0, banding.CollisionProbability(1.0));
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(double.NaN)]
    public void A_threshold_outside_the_open_unit_interval_is_refused(double threshold)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => LshBanding.Solve(threshold, 64));
    }

    [Fact]
    public void A_negative_error_weight_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => LshBanding.Solve(0.8, 64, falsePositiveWeight: -1.0));
    }
}
