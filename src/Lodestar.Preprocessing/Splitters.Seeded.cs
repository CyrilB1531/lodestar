using Lodestar.Preprocessing.Internal;

namespace Lodestar.Preprocessing;

public static partial class Splitters
{
    /// <summary>Cuts shuffled folds, as <c>KFold(n_splits=foldCount, shuffle=True, random_state=randomState)</c> does.</summary>
    /// <param name="sampleCount">How many rows there are.</param>
    /// <param name="foldCount">How many folds to cut.</param>
    /// <param name="randomState">scikit-learn's <c>random_state</c>, in <c>[0, 2³² − 1]</c>.</param>
    /// <returns>One <see cref="FoldSplit"/> per fold, in order, each index list ascending.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="sampleCount"/>, <paramref name="foldCount"/> or <paramref name="randomState"/> is out of range.</exception>
    public static IReadOnlyList<FoldSplit> KFold(int sampleCount, int foldCount, long randomState)
    {
        RequireFoldCount(sampleCount, foldCount);
        NumpyRandomState.RequireSeed(randomState, nameof(randomState));
        return KFold(sampleCount, foldCount, new NumpyRandomState(randomState).Permutation(sampleCount));
    }

    /// <summary>Cuts shuffled stratified folds, as <c>StratifiedKFold(shuffle=True, random_state=randomState)</c> does.</summary>
    /// <param name="labels">One class label per row.</param>
    /// <param name="foldCount">How many folds to cut.</param>
    /// <param name="randomState">scikit-learn's <c>random_state</c>, in <c>[0, 2³² − 1]</c>.</param>
    /// <returns>One <see cref="FoldSplit"/> per fold, in order, each index list ascending.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The row count, <paramref name="foldCount"/> or <paramref name="randomState"/> is out of range.</exception>
    /// <exception cref="ArgumentException"><paramref name="foldCount"/> is greater than every class's count.</exception>
    /// <remarks>
    /// The reference shuffles each class's list of folds, not the rows: class <c>c</c>'s rows, ascending, take the
    /// folds of the unshuffled allocation in the order the generator deals them, one class after another.
    /// </remarks>
    public static IReadOnlyList<FoldSplit> StratifiedKFold(ReadOnlySpan<int> labels, int foldCount, long randomState)
    {
        RequireFoldCount(labels.Length, foldCount);
        NumpyRandomState.RequireSeed(randomState, nameof(randomState));
        return ShuffledStratified(labels, foldCount, new NumpyRandomState(randomState));
    }

    /// <summary>Holds out a shuffled share, as <c>train_test_split(test_size=testFraction, random_state=randomState)</c> does.</summary>
    /// <param name="sampleCount">How many rows there are.</param>
    /// <param name="testFraction">What share to hold out, strictly inside (0, 1).</param>
    /// <param name="randomState">scikit-learn's <c>random_state</c>, in <c>[0, 2³² − 1]</c>.</param>
    /// <returns>The training and test indices, ascending.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="sampleCount"/>, <paramref name="testFraction"/> or <paramref name="randomState"/> is out of range.</exception>
    public static TrainTestSplit TrainTest(int sampleCount, double testFraction, long randomState)
    {
        int testCount = TestCount(sampleCount, testFraction);
        NumpyRandomState.RequireSeed(randomState, nameof(randomState));
        int[] permutation = new NumpyRandomState(randomState).Permutation(sampleCount);

        // Mark the held-out head, then read both halves out ascending in one pass: sorting them put
        // a million rows at 57 ms, this at 15, and scikit-learn at 12 (performance.md, #1157).
        var held = new bool[sampleCount];
        for (int i = 0; i < testCount; i++)
        {
            held[permutation[i]] = true;
        }

        var train = new int[sampleCount - testCount];
        var test = new int[testCount];
        int inTrain = 0;
        int inTest = 0;
        for (int row = 0; row < sampleCount; row++)
        {
            if (held[row])
            {
                test[inTest++] = row;
            }
            else
            {
                train[inTrain++] = row;
            }
        }

        return new TrainTestSplit(train, test);
    }

    /// <summary>Repeats shuffled k-fold, as <c>RepeatedKFold(n_splits=foldCount, n_repeats=repeatCount, random_state=randomState)</c> does.</summary>
    /// <param name="sampleCount">How many rows there are.</param>
    /// <param name="foldCount">How many folds each repeat cuts.</param>
    /// <param name="repeatCount">How many times to repeat, at least one.</param>
    /// <param name="randomState">scikit-learn's <c>random_state</c>, in <c>[0, 2³² − 1]</c>.</param>
    /// <returns><c>repeatCount · foldCount</c> folds, repeat by repeat, as the reference yields them.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A count or <paramref name="randomState"/> is out of range.</exception>
    /// <remarks>One generator serves every repeat, so repeat <c>r</c> reads the rows in the generator's <c>r</c>-th permutation.</remarks>
    public static IReadOnlyList<FoldSplit> RepeatedKFold(int sampleCount, int foldCount, int repeatCount, long randomState)
    {
        RequireFoldCount(sampleCount, foldCount);
        Guard.NotLessThan(repeatCount, 1);
        NumpyRandomState.RequireSeed(randomState, nameof(randomState));
        var generator = new NumpyRandomState(randomState);
        var folds = new List<FoldSplit>(repeatCount * foldCount);
        for (int repeat = 0; repeat < repeatCount; repeat++)
        {
            folds.AddRange(KFold(sampleCount, foldCount, generator.Permutation(sampleCount)));
        }

        return folds;
    }

    /// <summary>Repeats shuffled stratified k-fold, as <c>RepeatedStratifiedKFold(…, random_state=randomState)</c> does.</summary>
    /// <param name="labels">One class label per row.</param>
    /// <param name="foldCount">How many folds each repeat cuts.</param>
    /// <param name="repeatCount">How many times to repeat, at least one.</param>
    /// <param name="randomState">scikit-learn's <c>random_state</c>, in <c>[0, 2³² − 1]</c>.</param>
    /// <returns><c>repeatCount · foldCount</c> folds, repeat by repeat.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A count or <paramref name="randomState"/> is out of range.</exception>
    /// <exception cref="ArgumentException"><paramref name="foldCount"/> is greater than every class's count.</exception>
    public static IReadOnlyList<FoldSplit> RepeatedStratifiedKFold(
        ReadOnlySpan<int> labels, int foldCount, int repeatCount, long randomState)
    {
        RequireFoldCount(labels.Length, foldCount);
        Guard.NotLessThan(repeatCount, 1);
        NumpyRandomState.RequireSeed(randomState, nameof(randomState));
        var generator = new NumpyRandomState(randomState);
        var folds = new List<FoldSplit>(repeatCount * foldCount);
        for (int repeat = 0; repeat < repeatCount; repeat++)
        {
            folds.AddRange(ShuffledStratified(labels, foldCount, generator));
        }

        return folds;
    }

    /// <summary>Holds out a share that keeps each label's proportion, as <c>train_test_split(stratify=labels, random_state=randomState)</c> does.</summary>
    /// <param name="labels">One class label per row; every class needs two rows.</param>
    /// <param name="testFraction">What share to hold out, strictly inside (0, 1).</param>
    /// <param name="randomState">scikit-learn's <c>random_state</c>, in <c>[0, 2³² − 1]</c>.</param>
    /// <returns>The training and test indices, ascending.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The row count, <paramref name="testFraction"/> or <paramref name="randomState"/> is out of range.</exception>
    /// <exception cref="ArgumentException">A class has one row, or either side would hold fewer rows than there are classes.</exception>
    /// <remarks>
    /// <c>StratifiedShuffleSplit</c>'s algorithm: each class's share of the training rows, then of the test rows, from
    /// the approximate mode of the multivariate hypergeometric, remainder ties broken by the generator; then each
    /// class's rows, ascending, read through a permutation and cut there. No unseeded form exists, because the
    /// reference refuses <c>stratify</c> without shuffling.
    /// </remarks>
    public static TrainTestSplit StratifiedTrainTest(ReadOnlySpan<int> labels, double testFraction, long randomState)
    {
        int sampleCount = labels.Length;
        int testCount = TestCount(sampleCount, testFraction);
        NumpyRandomState.RequireSeed(randomState, nameof(randomState));
        int trainCount = sampleCount - testCount;

        (int[] codes, int[] counts) = SortedClasses(labels);
        RequireStratifiable(counts, trainCount, testCount, nameof(labels), nameof(testFraction));
        int[][] members = Members(codes, counts);

        var generator = new NumpyRandomState(randomState);
        int[] trainShares = ApproximateMode(counts, trainCount, generator);
        var remaining = new int[counts.Length];
        for (int cls = 0; cls < counts.Length; cls++)
        {
            remaining[cls] = counts[cls] - trainShares[cls];
        }

        int[] testShares = ApproximateMode(remaining, testCount, generator);
        var train = new int[trainCount];
        var test = new int[testCount];
        int inTrain = 0;
        int inTest = 0;
        for (int cls = 0; cls < counts.Length; cls++)
        {
            int[] permutation = generator.Permutation(counts[cls]);
            for (int i = 0; i < trainShares[cls]; i++)
            {
                train[inTrain++] = members[cls][permutation[i]];
            }

            for (int i = trainShares[cls]; i < trainShares[cls] + testShares[cls]; i++)
            {
                test[inTest++] = members[cls][permutation[i]];
            }
        }

        Array.Sort(train);
        Array.Sort(test);
        return new TrainTestSplit(train, test);
    }

    /// <summary><c>StratifiedKFold._make_test_folds</c> with <c>shuffle=True</c>, drawing from the generator given.</summary>
    private static FoldSplit[] ShuffledStratified(ReadOnlySpan<int> labels, int foldCount, NumpyRandomState generator)
    {
        int sampleCount = labels.Length;
        int[] identity = Reading(sampleCount, ReadOnlySpan<int>.Empty);
        (int[] codes, int[] counts) = Classes(labels, identity);
        RequireEnoughMembers(counts, foldCount);
        int[] allocation = Allocation(counts, foldCount);
        int[][] members = Members(codes, counts);

        var assignment = new int[sampleCount];
        int classCount = counts.Length;
        for (int cls = 0; cls < classCount; cls++)
        {
            // np.arange(n_splits).repeat(allocation[:, cls]), shuffled, laid over the class's rows ascending.
            var deal = new int[counts[cls]];
            int position = 0;
            for (int fold = 0; fold < foldCount; fold++)
            {
                for (int taken = 0; taken < allocation[(fold * classCount) + cls]; taken++)
                {
                    deal[position++] = fold;
                }
            }

            generator.Shuffle(deal);
            for (int member = 0; member < deal.Length; member++)
            {
                assignment[members[cls][member]] = deal[member];
            }
        }

        return Folds(assignment, foldCount);
    }

    /// <summary><c>ceil(sampleCount · testFraction)</c>, refused unless it leaves a training row.</summary>
    private static int TestCount(int sampleCount, double testFraction)
    {
        Guard.NotLessThan(sampleCount, 2);
        if (double.IsNaN(testFraction) || testFraction <= 0.0 || testFraction >= 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(testFraction), testFraction, "A test fraction lies strictly inside (0, 1).");
        }

        int testCount = (int)Math.Ceiling(sampleCount * testFraction);
        if (testCount >= sampleCount)
        {
            throw new ArgumentOutOfRangeException(
                nameof(testFraction), testFraction, $"holding out {testCount} of {sampleCount} rows leaves nothing to fit on.");
        }

        return testCount;
    }

    /// <summary>Each row's class, numbered by ascending label as <c>np.unique</c> numbers them, and each class's count.</summary>
    /// <remarks>
    /// Hashed in first-seen order, then only the distinct values sorted: sorting every label made <c>GroupKFold</c>
    /// slower than the reference at a million rows (37 ms against 32).
    /// </remarks>
    private static (int[] Codes, int[] Counts) SortedClasses(ReadOnlySpan<int> labels)
    {
        var seen = new Dictionary<int, int>();
        var codes = new int[labels.Length];
        for (int row = 0; row < labels.Length; row++)
        {
            if (!seen.TryGetValue(labels[row], out int code))
            {
                code = seen.Count;
                seen.Add(labels[row], code);
            }

            codes[row] = code;
        }

        int[] values = [.. seen.Keys];
        int[] firstSeen = [.. seen.Values];
        Array.Sort(values, firstSeen);
        var rank = new int[values.Length];
        for (int sorted = 0; sorted < values.Length; sorted++)
        {
            rank[firstSeen[sorted]] = sorted;
        }

        var counts = new int[values.Length];
        for (int row = 0; row < codes.Length; row++)
        {
            int code = rank[codes[row]];
            codes[row] = code;
            counts[code]++;
        }

        return (codes, counts);
    }

    /// <summary>Each class's rows, ascending: <c>np.split(np.argsort(codes, kind="stable"), …)</c>.</summary>
    private static int[][] Members(int[] codes, int[] counts)
    {
        var members = new int[counts.Length][];
        var filled = new int[counts.Length];
        for (int cls = 0; cls < counts.Length; cls++)
        {
            members[cls] = new int[counts[cls]];
        }

        for (int row = 0; row < codes.Length; row++)
        {
            int cls = codes[row];
            members[cls][filled[cls]++] = row;
        }

        return members;
    }

    private static void RequireStratifiable(int[] counts, int trainCount, int testCount, string labelsName, string fractionName)
    {
        for (int cls = 0; cls < counts.Length; cls++)
        {
            if (counts[cls] < 2)
            {
                throw new ArgumentException(
                    "A class has a single row, which cannot sit on both sides of a stratified split; the reference refuses it too.",
                    labelsName);
            }
        }

        if (trainCount < counts.Length || testCount < counts.Length)
        {
            throw new ArgumentException(
                $"{trainCount} training and {testCount} test rows cannot each hold one row of {counts.Length} classes.",
                fractionName);
        }
    }

    /// <summary>scikit-learn's <c>_approximate_mode</c>: each class's share of <paramref name="draws"/>, ties broken by the generator.</summary>
    private static int[] ApproximateMode(int[] counts, int draws, NumpyRandomState generator)
    {
        long total = 0;
        foreach (int count in counts)
        {
            total += count;
        }

        var continuous = new double[counts.Length];
        var shares = new int[counts.Length];
        long floored = 0;
        for (int cls = 0; cls < counts.Length; cls++)
        {
            continuous[cls] = counts[cls] / (double)total * draws;
            shares[cls] = (int)Math.Floor(continuous[cls]);
            floored += shares[cls];
        }

        long needed = draws - floored;
        if (needed <= 0)
        {
            return shares;
        }

        var remainder = new double[counts.Length];
        for (int cls = 0; cls < counts.Length; cls++)
        {
            remainder[cls] = continuous[cls] - shares[cls];
        }

        // np.sort(np.unique(remainder))[::-1], each value's classes ascending as np.where lists them: a stable
        // descending sort puts every tie in one run, in class order.
        int[] byRemainder = [.. Enumerable.Range(0, counts.Length).OrderByDescending(cls => remainder[cls])];
        int start = 0;
        while (needed > 0)
        {
            int end = start + 1;
            while (end < byRemainder.Length && !(remainder[byRemainder[end]] < remainder[byRemainder[start]]))
            {
                end++;
            }

            int addNow = (int)Math.Min(end - start, needed);
            foreach (int cls in generator.Choice(byRemainder.AsSpan(start, end - start), addNow))
            {
                shares[cls]++;
            }

            needed -= addNow;
            start = end;
        }

        return shares;
    }
}
