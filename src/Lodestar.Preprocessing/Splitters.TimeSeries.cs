namespace Lodestar.Preprocessing;

public static partial class Splitters
{
    /// <summary>Cuts forward-chaining splits over time-ordered rows, as <c>TimeSeriesSplit</c> does.</summary>
    /// <param name="sampleCount">How many rows there are, oldest first.</param>
    /// <param name="splitCount">How many splits, at least two: scikit-learn's <c>n_splits</c>.</param>
    /// <param name="testSize">Rows per test block; <see langword="null"/> is <c>sampleCount / (splitCount + 1)</c>, the reference's default.</param>
    /// <param name="gap">Rows dropped between the end of each training block and its test block; a negative gap
    /// overlaps them, which the reference allows.</param>
    /// <param name="maxTrainSize">The most recent rows a training block keeps; <see langword="null"/> or zero keeps them
    /// all, as <c>max_train_size=0</c> does in the reference.</param>
    /// <returns>One <see cref="FoldSplit"/> per split, oldest first, each index list ascending.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A count is below its floor, or <c>splitCount + 1</c> is above <paramref name="sampleCount"/>.</exception>
    /// <exception cref="ArgumentException">The test blocks and the gap leave the first split no training row, or the
    /// test blocks need more rows than there are, which only a negative gap lets through.</exception>
    /// <remarks>
    /// The test blocks are the last <c>splitCount · testSize</c> rows, cut in order; each split trains on everything
    /// before its block less <paramref name="gap"/> rows, cut to the last <paramref name="maxTrainSize"/> of them.
    /// </remarks>
    public static IReadOnlyList<FoldSplit> TimeSeries(
        int sampleCount, int splitCount, int? testSize = null, int gap = 0, int? maxTrainSize = null)
    {
        Guard.NotLessThan(splitCount, 2);
        if (testSize is { } requested)
        {
            Guard.NotLessThan(requested, 1);
        }

        if (maxTrainSize is { } cap)
        {
            // The reference slices a negative cap into an empty training block without a word.
            Guard.NotLessThan(cap, 0);
        }

        if (splitCount + 1 > sampleCount)
        {
            throw new ArgumentOutOfRangeException(
                nameof(splitCount), splitCount, $"{splitCount + 1} blocks is more than the {sampleCount} rows there are.");
        }

        int blockSize = testSize ?? (sampleCount / (splitCount + 1));
        if ((long)sampleCount - gap - ((long)blockSize * splitCount) <= 0)
        {
            throw new ArgumentException(
                $"{splitCount} test blocks of {blockSize} rows and a gap of {gap} leave no training row out of {sampleCount}.",
                nameof(testSize));
        }

        // Reachable only through a negative gap: the reference would start a block at a negative index
        // and return an empty test fold without a word.
        long firstTest = sampleCount - ((long)splitCount * blockSize);
        if (firstTest < 0)
        {
            throw new ArgumentException(
                $"{splitCount} test blocks of {blockSize} rows need {splitCount * (long)blockSize} rows; there are {sampleCount}.",
                nameof(testSize));
        }

        var splits = new FoldSplit[splitCount];
        for (int split = 0; split < splitCount; split++)
        {
            int testStart = (int)firstTest + (split * blockSize);
            // indices[start:end] with both ends clamped to the rows there are, as a slice clamps a negative gap's.
            long end = (long)testStart - gap;
            long start = maxTrainSize is { } limit && limit > 0 && limit < end ? end - limit : 0;
            int trainEnd = (int)Math.Min(end, sampleCount);
            int trainStart = (int)Math.Min(start, trainEnd);
            splits[split] = new FoldSplit(
                new IndexRange(trainStart, trainEnd - trainStart), new IndexRange(testStart, blockSize));
        }

        return splits;
    }

    /// <summary>A run of consecutive row indices, held as its ends.</summary>
    /// <remarks>
    /// The reference hands back numpy views of one <c>arange</c>, which cost nothing; copying each block out cost
    /// 2.0 ms at a million rows where the reference took 0.2, so a block is described rather than filled.
    /// </remarks>
    private sealed class IndexRange(int start, int count) : IReadOnlyList<int>
    {
        public int Count => count;

        public int this[int index] =>
            (uint)index < (uint)count ? start + index : throw new ArgumentOutOfRangeException(nameof(index));

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return start + i;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
