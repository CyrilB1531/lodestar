using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// What the seeded corpus does not reach: every refusal of the #1157 splitters, each the reference's own, and the
/// properties a caller relies on — a group never split, a time-series fold never training on its future.
/// </summary>
public sealed class SeededSplittersEdgeTests
{
    [Fact]
    public void A_seed_outside_numpys_range_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.KFold(10, 2, -1L));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.KFold(10, 2, 1L << 32));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.TrainTest(10, 0.5, -1L));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.GroupKFold([0, 1, 2], 2, -1L));
        Assert.Equal(2, Splitters.KFold(10, 2, uint.MaxValue).Count);
    }

    [Fact]
    public void More_folds_than_groups_is_refused()
    {
        Assert.Throws<ArgumentException>(() => Splitters.GroupKFold([0, 0, 1, 1], 3));
        Assert.Throws<ArgumentException>(() => Splitters.GroupKFold([0, 0, 1, 1], 3, 0L));
        Assert.Throws<ArgumentException>(() => Splitters.StratifiedGroupKFold([0, 1, 0, 1], [0, 0, 1, 1], 3));
    }

    [Fact]
    public void Labels_and_groups_of_different_lengths_are_refused()
    {
        Assert.Throws<ArgumentException>(() => Splitters.StratifiedGroupKFold([0, 1, 0], [0, 0, 1, 1], 2));
    }

    [Fact]
    public void A_group_never_straddles_two_test_folds()
    {
        int[] groups = [3, 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5, 8, 9, 7, 9];
        foreach (IReadOnlyList<FoldSplit> folds in new[]
                 {
                     Splitters.GroupKFold(groups, 3),
                     Splitters.GroupKFold(groups, 3, 42L),
                     Splitters.StratifiedGroupKFold([.. groups.Select(g => g % 2)], groups, 3),
                 })
        {
            var foldOf = new Dictionary<int, int>();
            for (int fold = 0; fold < folds.Count; fold++)
            {
                foreach (int row in folds[fold].TestIndices)
                {
                    if (!foldOf.TryGetValue(groups[row], out int seen))
                    {
                        foldOf[groups[row]] = seen = fold;
                    }

                    Assert.Equal(fold, seen);
                }
            }

            Assert.Equal(groups.Length, folds.Sum(fold => fold.TestIndices.Count));
        }
    }

    [Fact]
    public void A_time_series_split_trains_only_on_rows_before_its_test_block_less_the_gap()
    {
        IReadOnlyList<FoldSplit> splits = Splitters.TimeSeries(20, 3, gap: 2, maxTrainSize: 5);

        foreach (FoldSplit split in splits)
        {
            Assert.True(split.TrainIndices[^1] + 2 < split.TestIndices[0]);
            Assert.True(split.TrainIndices.Count <= 5);
        }
    }

    [Fact]
    public void A_time_series_split_that_leaves_no_training_row_is_refused()
    {
        Assert.Throws<ArgumentException>(() => Splitters.TimeSeries(13, 2, testSize: 5, gap: 3));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.TimeSeries(5, 5));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.TimeSeries(10, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.TimeSeries(10, 2, testSize: 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.TimeSeries(10, 2, maxTrainSize: -1));

        // A negative gap passes the reference's check and leaves it an empty test fold; refused here.
        Assert.Throws<ArgumentException>(() => Splitters.TimeSeries(15, 4, testSize: 4, gap: -3));
    }

    [Fact]
    public void A_repeat_count_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.RepeatedKFold(10, 2, 0, 0L));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.RepeatedStratifiedKFold([0, 0, 1, 1], 2, 0, 0L));
    }

    /// <summary>The three refusals <c>StratifiedShuffleSplit</c> itself raises, in its order.</summary>
    [Fact]
    public void A_stratified_split_the_reference_refuses_is_refused()
    {
        Assert.Throws<ArgumentException>(() => Splitters.StratifiedTrainTest([0, 0, 0, 1], 0.5, 0L));
        Assert.Throws<ArgumentException>(() => Splitters.StratifiedTrainTest([0, 0, 1, 1, 2, 2, 3, 3], 0.25, 0L));
        Assert.Throws<ArgumentException>(() => Splitters.StratifiedTrainTest([0, 0, 1, 1, 2, 2, 3, 3], 0.75, 0L));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.StratifiedTrainTest([0, 0, 1, 1], 1.0, 0L));
    }

    [Fact]
    public void A_stratified_split_keeps_each_class_on_both_sides()
    {
        int[] labels = [0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2];
        TrainTestSplit split = Splitters.StratifiedTrainTest(labels, 0.5, 7L);

        Assert.Equal(6, split.TestIndices.Count);
        Assert.Equal([3, 2, 1], [.. split.TestIndices.GroupBy(row => labels[row]).OrderBy(g => g.Key).Select(g => g.Count())]);
    }
}
