namespace Lodestar.Preprocessing;

/// <summary>One fold of a cross-validation split: which rows train, and which are held out.</summary>
/// <remarks>
/// Indices rather than rows. A splitter that copied the data would decide the caller's layout for them, and the
/// indices are what a fit and a score both take.
/// </remarks>
public sealed class FoldSplit
{
    internal FoldSplit(IReadOnlyList<int> trainIndices, IReadOnlyList<int> testIndices)
    {
        TrainIndices = trainIndices;
        TestIndices = testIndices;
    }

    /// <summary>Holds two index arrays behind read-only views, so a caller cannot cast one back and edit the fold (#1232).</summary>
    internal FoldSplit(int[] trainIndices, int[] testIndices)
        : this(Array.AsReadOnly(trainIndices), Array.AsReadOnly(testIndices))
    {
    }

    /// <summary>The rows this fold fits on, ascending.</summary>
    public IReadOnlyList<int> TrainIndices { get; }

    /// <summary>The rows this fold holds out, ascending.</summary>
    public IReadOnlyList<int> TestIndices { get; }
}
