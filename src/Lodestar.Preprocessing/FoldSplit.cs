namespace Lodestar.Preprocessing;

/// <summary>One fold of a cross-validation split: which rows train, and which are held out.</summary>
/// <remarks>
/// Indices rather than rows. A splitter that copied the data would decide the caller's layout for them, and the
/// indices are what a fit and a score both take.
/// </remarks>
public sealed class FoldSplit
{
    internal FoldSplit(int[] trainIndices, int[] testIndices)
    {
        TrainIndices = trainIndices;
        TestIndices = testIndices;
    }

    /// <summary>The rows this fold fits on, ascending.</summary>
    public IReadOnlyList<int> TrainIndices { get; }

    /// <summary>The rows this fold holds out, ascending.</summary>
    public IReadOnlyList<int> TestIndices { get; }
}
