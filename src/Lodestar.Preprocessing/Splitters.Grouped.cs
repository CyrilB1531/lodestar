using Lodestar.Preprocessing.Internal;

namespace Lodestar.Preprocessing;

public static partial class Splitters
{
    /// <summary>Cuts folds that never split a group, as <c>GroupKFold(n_splits=foldCount)</c> does.</summary>
    /// <param name="groups">One group label per row; any integers.</param>
    /// <param name="foldCount">How many folds to cut, at most the number of distinct groups.</param>
    /// <returns>One <see cref="FoldSplit"/> per fold, in order, each index list ascending.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The row count is below two, or <paramref name="foldCount"/> is below two or above it.</exception>
    /// <exception cref="ArgumentException"><paramref name="foldCount"/> is greater than the number of distinct groups.</exception>
    /// <remarks>
    /// The reference's greedy balance: groups by descending size, each to the fold holding the fewest rows so far, the
    /// first such fold on a tie. Groups of equal size go in descending label order, which is what reversing numpy's
    /// stable ascending sort gives.
    /// </remarks>
    public static IReadOnlyList<FoldSplit> GroupKFold(ReadOnlySpan<int> groups, int foldCount)
    {
        RequireFoldCount(groups.Length, foldCount);
        (int[] codes, int[] sizes) = SortedClasses(groups);
        RequireEnoughGroups(sizes.Length, foldCount, nameof(foldCount));

        // np.argsort(sizes, kind="stable")[::-1]: descending size, ties by descending group index.
        int[] byWeight = [.. Enumerable.Range(0, sizes.Length).OrderBy(group => sizes[group]).Reverse()];
        var load = new long[foldCount];
        var groupFold = new int[sizes.Length];
        foreach (int group in byWeight)
        {
            int lightest = 0;
            for (int fold = 1; fold < foldCount; fold++)
            {
                if (load[fold] < load[lightest])
                {
                    lightest = fold;
                }
            }

            load[lightest] += sizes[group];
            groupFold[group] = lightest;
        }

        return Folds(Assign(codes, groupFold), foldCount);
    }

    /// <summary>Cuts group folds from shuffled groups, as <c>GroupKFold(shuffle=True, random_state=randomState)</c> does.</summary>
    /// <param name="groups">One group label per row.</param>
    /// <param name="foldCount">How many folds to cut.</param>
    /// <param name="randomState">scikit-learn's <c>random_state</c>, in <c>[0, 2³² − 1]</c>.</param>
    /// <returns>One <see cref="FoldSplit"/> per fold, in order, each index list ascending.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The row count, <paramref name="foldCount"/> or <paramref name="randomState"/> is out of range.</exception>
    /// <exception cref="ArgumentException"><paramref name="foldCount"/> is greater than the number of distinct groups.</exception>
    /// <remarks>
    /// The distinct groups, ascending, are permuted and cut into <paramref name="foldCount"/> runs by
    /// <c>np.array_split</c>, the first <c>groupCount % foldCount</c> runs one group longer. Folds balance groups, not
    /// rows, as the reference's do.
    /// </remarks>
    public static IReadOnlyList<FoldSplit> GroupKFold(ReadOnlySpan<int> groups, int foldCount, long randomState)
    {
        RequireFoldCount(groups.Length, foldCount);
        NumpyRandomState.RequireSeed(randomState, nameof(randomState));
        (int[] codes, int[] sizes) = SortedClasses(groups);
        RequireEnoughGroups(sizes.Length, foldCount, nameof(foldCount));

        int[] shuffled = new NumpyRandomState(randomState).Permutation(sizes.Length);
        var groupFold = new int[sizes.Length];
        int baseRun = sizes.Length / foldCount;
        int longer = sizes.Length % foldCount;
        int position = 0;
        for (int fold = 0; fold < foldCount; fold++)
        {
            int run = baseRun + (fold < longer ? 1 : 0);
            for (int taken = 0; taken < run; taken++)
            {
                groupFold[shuffled[position++]] = fold;
            }
        }

        return Folds(Assign(codes, groupFold), foldCount);
    }

    /// <summary>Cuts group folds that keep each label's share, as <c>StratifiedGroupKFold(n_splits=foldCount)</c> does.</summary>
    /// <param name="labels">One class label per row.</param>
    /// <param name="groups">One group label per row, as many as <paramref name="labels"/>.</param>
    /// <param name="foldCount">How many folds to cut.</param>
    /// <returns>One <see cref="FoldSplit"/> per fold, in order, each index list ascending.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The row count or <paramref name="foldCount"/> is out of range.</exception>
    /// <exception cref="ArgumentException">The two spans differ in length, <paramref name="foldCount"/> is above every class's count, or above the number of groups.</exception>
    /// <remarks>
    /// The reference's greedy search: groups by descending spread of their class counts, each to the fold that
    /// minimises the mean, over classes, of the spread of the folds' shares of that class; a fold within
    /// <c>np.isclose</c> of the best wins if it holds fewer rows. Spreads are numpy's, to the bit, since a tie decides a
    /// fold. A fold count above the smallest class makes the reference warn, and is accepted here.
    /// </remarks>
    public static IReadOnlyList<FoldSplit> StratifiedGroupKFold(ReadOnlySpan<int> labels, ReadOnlySpan<int> groups, int foldCount) =>
        StratifiedGroups(labels, groups, foldCount, null);

    /// <summary>Cuts stratified group folds from shuffled groups, as <c>StratifiedGroupKFold(shuffle=True, random_state=randomState)</c> does.</summary>
    /// <param name="labels">One class label per row.</param>
    /// <param name="groups">One group label per row.</param>
    /// <param name="foldCount">How many folds to cut.</param>
    /// <param name="randomState">scikit-learn's <c>random_state</c>, in <c>[0, 2³² − 1]</c>.</param>
    /// <returns>One <see cref="FoldSplit"/> per fold, in order, each index list ascending.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The row count, <paramref name="foldCount"/> or <paramref name="randomState"/> is out of range.</exception>
    /// <exception cref="ArgumentException">As the unshuffled form.</exception>
    /// <remarks>The groups are visited in a permuted order before the same search, which is all the reference's shuffle does.</remarks>
    public static IReadOnlyList<FoldSplit> StratifiedGroupKFold(
        ReadOnlySpan<int> labels, ReadOnlySpan<int> groups, int foldCount, long randomState)
    {
        NumpyRandomState.RequireSeed(randomState, nameof(randomState));
        return StratifiedGroups(labels, groups, foldCount, new NumpyRandomState(randomState));
    }

    private static FoldSplit[] StratifiedGroups(
        ReadOnlySpan<int> labels, ReadOnlySpan<int> groups, int foldCount, NumpyRandomState? generator)
    {
        if (labels.Length != groups.Length)
        {
            throw new ArgumentException(
                $"{labels.Length} labels and {groups.Length} groups: each row needs one of each.", nameof(groups));
        }

        RequireFoldCount(labels.Length, foldCount);
        (int[] classOf, int[] classSizes) = SortedClasses(labels);
        RequireEnoughMembers(classSizes, foldCount);
        (int[] groupOf, int[] groupSizes) = SortedClasses(groups);
        RequireEnoughGroups(groupSizes.Length, foldCount, nameof(foldCount));

        int classCount = classSizes.Length;
        int groupCount = groupSizes.Length;

        // Group g's counts sit in row rank[g]: the identity, or the inverse of the generator's permutation.
        var rank = new int[groupCount];
        for (int group = 0; group < groupCount; group++)
        {
            rank[group] = group;
        }

        if (generator is not null)
        {
            int[] permutation = generator.Permutation(groupCount);
            for (int row = 0; row < groupCount; row++)
            {
                rank[permutation[row]] = row;
            }
        }

        var groupCounts = new double[groupCount * classCount];
        for (int row = 0; row < labels.Length; row++)
        {
            groupCounts[(rank[groupOf[row]] * classCount) + classOf[row]]++;
        }

        int[] visit = SpreadOrder(groupCounts, groupCount, classCount);
        int[] rankFold = GreedyFolds(groupCounts, visit, classSizes, foldCount);
        var groupFold = new int[groupCount];
        for (int group = 0; group < groupCount; group++)
        {
            groupFold[group] = rankFold[rank[group]];
        }

        return Folds(Assign(groupOf, groupFold), foldCount);
    }

    /// <summary><c>np.argsort(-np.std(groupCounts, axis=1), kind="stable")</c>.</summary>
    private static int[] SpreadOrder(double[] groupCounts, int groupCount, int classCount)
    {
        var spread = new double[groupCount];
        for (int row = 0; row < groupCount; row++)
        {
            spread[row] = -NumpyReduction.StandardDeviation(groupCounts.AsSpan(row * classCount, classCount));
        }

        return [.. Enumerable.Range(0, groupCount).OrderBy(row => spread[row])];
    }

    /// <summary>The reference's <c>_find_best_fold</c>, applied to each group in the order given.</summary>
    private static int[] GreedyFolds(double[] groupCounts, int[] visit, int[] classSizes, int foldCount)
    {
        int classCount = classSizes.Length;
        var foldCounts = new double[foldCount * classCount];
        var shares = new double[foldCount * classCount];
        var spreads = new double[classCount];
        var rowFold = new int[visit.Length];
        foreach (int row in visit)
        {
            ReadOnlySpan<double> counts = groupCounts.AsSpan(row * classCount, classCount);
            int best = -1;
            double bestEval = double.PositiveInfinity;
            double bestSamples = double.PositiveInfinity;
            for (int fold = 0; fold < foldCount; fold++)
            {
                double evaluation = SpreadWith(foldCounts, fold, counts, classSizes, shares, spreads);
                double samples = NumpyReduction.Sum(foldCounts.AsSpan(fold * classCount, classCount));
                if (evaluation < bestEval || (NumpyReduction.IsClose(evaluation, bestEval) && samples < bestSamples))
                {
                    bestEval = evaluation;
                    bestSamples = samples;
                    best = fold;
                }
            }

            Span<double> chosen = foldCounts.AsSpan(best * classCount, classCount);
            for (int cls = 0; cls < classCount; cls++)
            {
                chosen[cls] += counts[cls];
            }

            rowFold[row] = best;
        }

        return rowFold;
    }

    /// <summary>The mean spread of the folds' class shares were the group's counts added to fold <paramref name="fold"/>.</summary>
    private static double SpreadWith(
        double[] foldCounts, int fold, ReadOnlySpan<double> counts, int[] classSizes, double[] shares, double[] spreads)
    {
        int classCount = classSizes.Length;
        Span<double> candidate = foldCounts.AsSpan(fold * classCount, classCount);
        for (int cls = 0; cls < classCount; cls++)
        {
            candidate[cls] += counts[cls];
        }

        for (int i = 0; i < shares.Length; i++)
        {
            shares[i] = foldCounts[i] / classSizes[i % classCount];
        }

        // Counts are whole numbers, so taking the group back out restores the fold exactly.
        for (int cls = 0; cls < classCount; cls++)
        {
            candidate[cls] -= counts[cls];
        }

        NumpyReduction.ColumnStandardDeviations(shares, shares.Length / classCount, classCount, spreads);
        return NumpyReduction.Sum(spreads) / classCount;
    }

    /// <summary>Each row's fold, from its group's.</summary>
    private static int[] Assign(int[] groupOf, int[] groupFold)
    {
        var assignment = new int[groupOf.Length];
        for (int row = 0; row < groupOf.Length; row++)
        {
            assignment[row] = groupFold[groupOf[row]];
        }

        return assignment;
    }

    private static void RequireEnoughGroups(int groupCount, int foldCount, string parameterName)
    {
        if (foldCount > groupCount)
        {
            throw new ArgumentException(
                $"{foldCount} folds is more than the {groupCount} groups there are to deal out; the reference refuses it too.",
                parameterName);
        }
    }
}
