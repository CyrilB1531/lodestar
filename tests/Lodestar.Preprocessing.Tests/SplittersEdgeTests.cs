using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// What the corpus does not reach: every refusal, and the three properties a caller relies on
/// that no single frozen case states — the partition, the identity permutation, and the class
/// shares a stratified fold keeps (#762).
/// </summary>
public sealed class SplittersEdgeTests
{
    private static readonly int[] ThreeClasses = [0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2];

    [Fact]
    public void A_fold_count_below_two_or_above_the_rows_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.KFold(10, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.KFold(10, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.KFold(10, 11));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.KFold(1, 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.StratifiedKFold([0, 0, 1, 1], 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.StratifiedKFold([0, 0, 1, 1], 5));
    }

    /// <summary>
    /// The reference refuses only when <em>every</em> class is short, and warns when one is. Warning is
    /// not a return value, so the page says what the short class costs instead: a fold without it.
    /// </summary>
    [Fact]
    public void A_fold_count_above_every_class_count_is_refused_and_above_one_of_them_is_not()
    {
        Assert.Throws<ArgumentException>(() => Splitters.StratifiedKFold([0, 0, 1, 1, 2, 2], 3));

        IReadOnlyList<FoldSplit> folds = Splitters.StratifiedKFold([0, 0, 0, 1], 3);

        Assert.Equal([0, 3], folds[0].TestIndices);
        Assert.Equal([1], folds[1].TestIndices);
        Assert.Equal([2], folds[2].TestIndices);
    }

    [Fact]
    public void A_test_fraction_outside_the_open_unit_interval_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.TrainTest(10, 0.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.TrainTest(10, 1.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.TrainTest(10, -0.1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.TrainTest(10, double.NaN));
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.TrainTest(1, 0.5));
    }

    /// <summary>A fraction inside the interval can still round up to everything, and two rows is where it does.</summary>
    [Fact]
    public void A_fraction_that_rounds_up_to_every_row_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.TrainTest(2, 0.99));
        Assert.Equal([1], Splitters.TrainTest(2, 0.5).TestIndices);
    }

    [Fact]
    public void An_order_that_is_not_a_permutation_is_refused()
    {
        Assert.Throws<ArgumentException>(() => Splitters.KFold(4, 2, [0, 1, 2]));
        Assert.Throws<ArgumentException>(() => Splitters.KFold(4, 2, [0, 1, 2, 2]));
        Assert.Throws<ArgumentException>(() => Splitters.KFold(4, 2, [0, 1, 2, 4]));
        Assert.Throws<ArgumentException>(() => Splitters.KFold(4, 2, [0, 1, 2, -1]));
        Assert.Throws<ArgumentException>(() => Splitters.StratifiedKFold([0, 0, 1, 1], 2, [0, 0, 1, 1]));
        Assert.Throws<ArgumentException>(() => Splitters.TrainTest(4, 0.5, [3, 2, 1]));
    }

    /// <summary>
    /// The permutation is an argument, so the one that reorders nothing changes no fold. A train/test split is the
    /// exception the reference makes: unshuffled it holds out the tail, and <c>ShuffleSplit</c> the permutation's head (#893).
    /// </summary>
    [Fact]
    public void The_identity_permutation_gives_the_unshuffled_split()
    {
        int[] identity = [.. Enumerable.Range(0, ThreeClasses.Length)];

        IReadOnlyList<FoldSplit> plain = Splitters.KFold(ThreeClasses.Length, 3);
        IReadOnlyList<FoldSplit> read = Splitters.KFold(ThreeClasses.Length, 3, identity);
        IReadOnlyList<FoldSplit> plainStratified = Splitters.StratifiedKFold(ThreeClasses, 3);
        IReadOnlyList<FoldSplit> readStratified = Splitters.StratifiedKFold(ThreeClasses, 3, identity);

        for (int fold = 0; fold < 3; fold++)
        {
            Assert.Equal(plain[fold].TestIndices, read[fold].TestIndices);
            Assert.Equal(plainStratified[fold].TestIndices, readStratified[fold].TestIndices);
        }

        Assert.Equal([9, 10, 11], Splitters.TrainTest(ThreeClasses.Length, 0.25).TestIndices);
        Assert.Equal([0, 1, 2], Splitters.TrainTest(ThreeClasses.Length, 0.25, identity).TestIndices);
    }

    /// <summary>
    /// The property a cross-validation loop depends on: each row is scored exactly once and fitted on
    /// <c>k − 1</c> times. Asserted on both splitters and on a permuted read.
    /// </summary>
    [Theory]
    [InlineData(3, false)]
    [InlineData(5, false)]
    [InlineData(3, true)]
    public void Every_row_is_held_out_once_and_trained_on_the_rest(int foldCount, bool stratified)
    {
        int[] order = [11, 0, 5, 2, 9, 4, 7, 1, 10, 3, 8, 6];
        IReadOnlyList<FoldSplit> folds = stratified
            ? Splitters.StratifiedKFold(ThreeClasses, foldCount, order)
            : Splitters.KFold(ThreeClasses.Length, foldCount);
        var heldOut = new int[ThreeClasses.Length];
        var fitted = new int[ThreeClasses.Length];

        foreach (FoldSplit fold in folds)
        {
            Assert.Equal(ThreeClasses.Length, fold.TrainIndices.Count + fold.TestIndices.Count);
            foreach (int row in fold.TestIndices)
            {
                heldOut[row]++;
            }

            foreach (int row in fold.TrainIndices)
            {
                fitted[row]++;
            }
        }

        Assert.All(heldOut, count => Assert.Equal(1, count));
        Assert.All(fitted, count => Assert.Equal(foldCount - 1, count));
    }

    /// <summary>
    /// What "stratified" buys, stated as a bound rather than an equality: a class of <c>m</c> rows over
    /// <c>k</c> folds gives every fold <c>floor(m/k)</c> or <c>ceil(m/k)</c> of them, which a plain
    /// <see cref="Splitters.KFold(int, int)"/> on these labels does not — its first fold is four rows of class 0
    /// and nothing else.
    /// </summary>
    [Fact]
    public void A_stratified_fold_keeps_each_class_within_one_row_of_its_share()
    {
        IReadOnlyList<FoldSplit> folds = Splitters.StratifiedKFold(ThreeClasses, 3);

        foreach (FoldSplit fold in folds)
        {
            int[] present = [0, 1, 2];
            foreach (int label in present)
            {
                int members = ThreeClasses.Count(value => value == label);
                int held = fold.TestIndices.Count(row => ThreeClasses[row] == label);

                Assert.InRange(held, members / 3, (members + 2) / 3);
            }
        }

        IReadOnlyList<int> unstratified = Splitters.KFold(ThreeClasses.Length, 3)[0].TestIndices;

        Assert.Equal(4, unstratified.Count(row => ThreeClasses[row] == 0));
        Assert.DoesNotContain(unstratified, row => ThreeClasses[row] != 0);
    }

    /// <summary>The held-out rows are the last ones, and the two sides partition the rows between them.</summary>
    [Fact]
    public void The_train_test_split_holds_out_the_last_rows()
    {
        TrainTestSplit split = Splitters.TrainTest(10, 0.25);

        Assert.Equal([7, 8, 9], split.TestIndices);
        Assert.Equal([0, 1, 2, 3, 4, 5, 6], split.TrainIndices);
        Assert.DoesNotContain(split.TrainIndices, row => split.TestIndices.Contains(row));
    }
}
