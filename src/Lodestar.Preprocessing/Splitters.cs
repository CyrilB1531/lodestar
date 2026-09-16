namespace Lodestar.Preprocessing;

/// <summary>Cross-validation and train/test splits, at <c>sklearn.model_selection</c> parity where it is deterministic.</summary>
/// <remarks>
/// The unshuffled splitters are scikit-learn's fold for fold. A shuffled one takes the permutation as an argument
/// rather than a seed: the rows are read in that order, so a caller who passes scikit-learn's own permutation gets
/// scikit-learn's folds, and one who passes their own gets a split this package can describe without claiming a
/// generator it does not share (decision 0132).
/// </remarks>
public static class Splitters
{
    /// <summary>Cuts the rows into contiguous folds, as <c>KFold(n_splits=foldCount)</c> does without shuffling.</summary>
    /// <param name="sampleCount">How many rows there are.</param>
    /// <param name="foldCount">How many folds to cut, at least two and at most <paramref name="sampleCount"/>.</param>
    /// <returns>One <see cref="FoldSplit"/> per fold, in order.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="sampleCount"/> is below two, or <paramref name="foldCount"/> is below two or above it.</exception>
    /// <remarks>The first <c>sampleCount % foldCount</c> folds take one extra row, which is where the reference puts them.</remarks>
    public static IReadOnlyList<FoldSplit> KFold(int sampleCount, int foldCount) =>
        KFold(sampleCount, foldCount, default);

    /// <summary>Cuts the rows into folds, reading them in the order given.</summary>
    /// <param name="sampleCount">How many rows there are.</param>
    /// <param name="foldCount">How many folds to cut.</param>
    /// <param name="order">A permutation of <c>0..sampleCount-1</c> the rows are read in; empty reads them in order.</param>
    /// <returns>One <see cref="FoldSplit"/> per fold, in order, each index list ascending.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="sampleCount"/> or <paramref name="foldCount"/> is out of range.</exception>
    /// <exception cref="ArgumentException"><paramref name="order"/> is neither empty nor a permutation of the rows.</exception>
    public static IReadOnlyList<FoldSplit> KFold(int sampleCount, int foldCount, ReadOnlySpan<int> order)
    {
        RequireFoldCount(sampleCount, foldCount);
        int[] reading = Reading(sampleCount, order);
        var assignment = new int[sampleCount];
        int baseSize = sampleCount / foldCount;
        int larger = sampleCount % foldCount;
        int position = 0;
        for (int fold = 0; fold < foldCount; fold++)
        {
            int size = baseSize + (fold < larger ? 1 : 0);
            for (int taken = 0; taken < size; taken++)
            {
                assignment[reading[position++]] = fold;
            }
        }

        return Folds(assignment, foldCount);
    }

    /// <summary>Cuts the rows into folds that keep each label's share, as <c>StratifiedKFold</c> does without shuffling.</summary>
    /// <param name="labels">One class label per row; any integers, at least two distinct.</param>
    /// <param name="foldCount">How many folds to cut.</param>
    /// <returns>One <see cref="FoldSplit"/> per fold, in order.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The row count is below two, or <paramref name="foldCount"/> is below two or above it.</exception>
    /// <exception cref="ArgumentException"><paramref name="foldCount"/> is greater than every class's count, which leaves a fold with nothing to hold out.</exception>
    public static IReadOnlyList<FoldSplit> StratifiedKFold(ReadOnlySpan<int> labels, int foldCount) =>
        StratifiedKFold(labels, foldCount, default);

    /// <summary>Cuts stratified folds, reading the rows in the order given.</summary>
    /// <param name="labels">One class label per row.</param>
    /// <param name="foldCount">How many folds to cut.</param>
    /// <param name="order">A permutation of the rows to read them in; empty reads them in order.</param>
    /// <returns>One <see cref="FoldSplit"/> per fold, in order, each index list ascending.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The row count or <paramref name="foldCount"/> is out of range.</exception>
    /// <exception cref="ArgumentException"><paramref name="order"/> is not a permutation, or <paramref name="foldCount"/> is greater than every class's count.</exception>
    /// <remarks>
    /// Fold <c>i</c> takes as many rows of class <c>c</c> as the sorted labels hold at positions <c>i</c>, <c>i +
    /// foldCount</c>, and so on — the reference's own allocation, which is why a class of two rows over three folds
    /// lands in folds 0 and 2 rather than 0 and 1.
    /// </remarks>
    public static IReadOnlyList<FoldSplit> StratifiedKFold(ReadOnlySpan<int> labels, int foldCount, ReadOnlySpan<int> order)
    {
        int sampleCount = labels.Length;
        RequireFoldCount(sampleCount, foldCount);
        int[] reading = Reading(sampleCount, order);
        (Dictionary<int, int> encoding, int[] counts) = Classes(labels);
        RequireEnoughMembers(counts, foldCount);

        int[] allocation = Allocation(counts, foldCount);
        var assignment = new int[sampleCount];
        var placed = new int[counts.Length];
        for (int position = 0; position < sampleCount; position++)
        {
            int row = reading[position];
            int cls = encoding[labels[row]];
            int fold = 0;
            int remaining = placed[cls];
            while (remaining >= allocation[(fold * counts.Length) + cls])
            {
                remaining -= allocation[(fold * counts.Length) + cls];
                fold++;
            }

            placed[cls]++;
            assignment[row] = fold;
        }

        return Folds(assignment, foldCount);
    }

    /// <summary>Holds out the last rows, as <c>train_test_split(shuffle=False)</c> does.</summary>
    /// <param name="sampleCount">How many rows there are.</param>
    /// <param name="testFraction">What share to hold out, strictly inside (0, 1).</param>
    /// <returns>The training and test indices, ascending.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="sampleCount"/> is below two, or <paramref name="testFraction"/> is outside (0, 1).</exception>
    public static TrainTestSplit TrainTest(int sampleCount, double testFraction) =>
        TrainTest(sampleCount, testFraction, default);

    /// <summary>Holds out rows, reading them in the order given.</summary>
    /// <param name="sampleCount">How many rows there are.</param>
    /// <param name="testFraction">What share to hold out, strictly inside (0, 1).</param>
    /// <param name="order">A permutation of the rows to read them in; empty reads them in order.</param>
    /// <returns>The training and test indices, ascending.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="sampleCount"/> or <paramref name="testFraction"/> is out of range.</exception>
    /// <exception cref="ArgumentException"><paramref name="order"/> is neither empty nor a permutation of the rows.</exception>
    /// <remarks>
    /// The held-out count is <c>ceil(sampleCount · testFraction)</c>, the reference's, and it must leave at least one
    /// training row.
    /// </remarks>
    public static TrainTestSplit TrainTest(int sampleCount, double testFraction, ReadOnlySpan<int> order)
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

        var train = new int[sampleCount - testCount];
        var test = new int[testCount];
        if (order.IsEmpty)
        {
            // Straight into the two halves: no order array, and nothing to sort, since both come
            // out ascending. At a million rows that is 6.1 ms through the general path, 0.6 ms here.
            for (int row = 0; row < train.Length; row++)
            {
                train[row] = row;
            }

            for (int row = 0; row < test.Length; row++)
            {
                test[row] = train.Length + row;
            }

            return new TrainTestSplit(train, test);
        }

        int[] reading = Reading(sampleCount, order);
        Array.Copy(reading, train, train.Length);
        Array.Copy(reading, train.Length, test, 0, test.Length);
        Array.Sort(train);
        Array.Sort(test);
        return new TrainTestSplit(train, test);
    }

    /// <summary>Fold <c>i</c>'s share of each class: what the sorted labels hold at positions <c>i</c>, <c>i + foldCount</c>, …</summary>
    private static int[] Allocation(int[] counts, int foldCount)
    {
        var allocation = new int[foldCount * counts.Length];
        int position = 0;
        for (int cls = 0; cls < counts.Length; cls++)
        {
            for (int member = 0; member < counts[cls]; member++)
            {
                allocation[((position % foldCount) * counts.Length) + cls]++;
                position++;
            }
        }

        return allocation;
    }

    /// <summary>The distinct labels in the order they first appear, and how many rows each holds.</summary>
    /// <remarks>
    /// First appearance, not ascending value: the reference encodes its classes by ranking each label's first index
    /// (<c>np.unique(y_idx)</c>), and the allocation below reads that order — measured, on labels whose smallest value
    /// is not the first one seen the two orders give different folds.
    /// </remarks>
    private static (Dictionary<int, int> Encoding, int[] Counts) Classes(ReadOnlySpan<int> labels)
    {
        var encoding = new Dictionary<int, int>();
        var counts = new List<int>();
        for (int row = 0; row < labels.Length; row++)
        {
            if (encoding.TryGetValue(labels[row], out int cls))
            {
                counts[cls]++;
            }
            else
            {
                encoding.Add(labels[row], counts.Count);
                counts.Add(1);
            }
        }

        return (encoding, [.. counts]);
    }

    private static void RequireEnoughMembers(int[] counts, int foldCount)
    {
        for (int cls = 0; cls < counts.Length; cls++)
        {
            if (counts[cls] >= foldCount)
            {
                return;
            }
        }

        throw new ArgumentException(
            $"{foldCount} folds is more than every class holds, so a fold would have no row to hold out. The "
            + "reference refuses the same input, and warns rather than refusing when only the smallest class is short.",
            nameof(foldCount));
    }

    private static void RequireFoldCount(int sampleCount, int foldCount)
    {
        Guard.NotLessThan(sampleCount, 2);
        Guard.NotLessThan(foldCount, 2);
        if (foldCount > sampleCount)
        {
            throw new ArgumentOutOfRangeException(
                nameof(foldCount), foldCount, $"{foldCount} folds is more than the {sampleCount} rows there are to cut.");
        }
    }

    /// <summary>The order the rows are read in: the caller's permutation, or <c>0..n-1</c>.</summary>
    private static int[] Reading(int sampleCount, ReadOnlySpan<int> order)
    {
        var reading = new int[sampleCount];
        if (order.IsEmpty)
        {
            for (int i = 0; i < sampleCount; i++)
            {
                reading[i] = i;
            }

            return reading;
        }

        if (order.Length != sampleCount)
        {
            throw new ArgumentException(
                $"order holds {order.Length} positions for {sampleCount} rows.", nameof(order));
        }

        var seen = new bool[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            int row = order[i];
            if (row < 0 || row >= sampleCount || seen[row])
            {
                throw new ArgumentException(
                    $"order[{i}] is {row}, and an order is a permutation of 0..{sampleCount - 1} with no repeats.",
                    nameof(order));
            }

            seen[row] = true;
            reading[i] = row;
        }

        return reading;
    }

    /// <summary>The folds an assignment describes, each index list ascending.</summary>
    private static FoldSplit[] Folds(int[] assignment, int foldCount)
    {
        var sizes = new int[foldCount];
        for (int row = 0; row < assignment.Length; row++)
        {
            sizes[assignment[row]]++;
        }

        var folds = new FoldSplit[foldCount];
        for (int fold = 0; fold < foldCount; fold++)
        {
            var test = new int[sizes[fold]];
            var train = new int[assignment.Length - sizes[fold]];
            int inTest = 0;
            int inTrain = 0;
            for (int row = 0; row < assignment.Length; row++)
            {
                if (assignment[row] == fold)
                {
                    test[inTest++] = row;
                }
                else
                {
                    train[inTrain++] = row;
                }
            }

            folds[fold] = new FoldSplit(train, test);
        }

        return folds;
    }
}
