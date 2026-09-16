namespace Lodestar.Preprocessing;

/// <summary>A single train and test split of the rows.</summary>
public sealed class TrainTestSplit
{
    internal TrainTestSplit(int[] trainIndices, int[] testIndices)
    {
        TrainIndices = trainIndices;
        TestIndices = testIndices;
    }

    /// <summary>The rows to fit on, ascending.</summary>
    public IReadOnlyList<int> TrainIndices { get; }

    /// <summary>The rows held out, ascending.</summary>
    public IReadOnlyList<int> TestIndices { get; }
}
