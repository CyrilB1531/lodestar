using Lodestar.Preprocessing.Internal;

namespace Lodestar.Preprocessing;

/// <summary>
/// Encodes one column of labels as their positions among the sorted distinct ones, at
/// <c>sklearn.preprocessing.LabelEncoder</c> parity.
/// </summary>
/// <typeparam name="T">The label type; <see cref="string"/> and <see cref="int"/> are the two the reference takes.</typeparam>
/// <remarks>
/// What <see cref="OrdinalEncoder{T}"/> does to a matrix of features, this does to a single column
/// of targets — and the reference keeps them apart for that reason rather than a computational
/// one. The class order is the sort order of <typeparamref name="T"/> on both sides.
/// </remarks>
public sealed class LabelEncoder<T>
    where T : IComparable<T>, IEquatable<T>
{
    private readonly T[] _classes;
    private readonly CategoryIndex<T> _index;

    private LabelEncoder(T[] classes, int sampleCount)
    {
        _classes = classes;
        SampleCount = sampleCount;
        _index = new CategoryIndex<T>([classes]);
    }

    /// <summary>How many labels the encoder was fitted on.</summary>
    public int SampleCount { get; }

    /// <summary>The distinct labels, sorted — the reference's <c>classes_</c>.</summary>
    public IReadOnlyList<T> Classes => _classes;

    /// <summary>What <see cref="Encoders.Label"/> calls; the factory is there so this type carries no public static.</summary>
    internal static LabelEncoder<T> FitCore(ReadOnlySpan<T> labels)
    {
        if (labels.Length == 0)
        {
            throw new ArgumentException("A label encoder needs at least one label.", nameof(labels));
        }

        CategoryMatrix.RequireNoNull(labels);

        // One column, so the table the encoders share builds exactly one row of categories.
        return new LabelEncoder<T>(CategoryTable.Build(labels, 1, labels.Length)[0], labels.Length);
    }

    /// <summary>Encodes each label as its position in <see cref="Classes"/>.</summary>
    /// <param name="labels">The labels to encode; each must be one the encoder was fitted on.</param>
    /// <returns>One code per label.</returns>
    /// <exception cref="ArgumentException">A label is null, or is not among <see cref="Classes"/>.</exception>
    /// <remarks>
    /// An unseen label is refused rather than mapped anywhere, which is the reference's own
    /// behaviour — there is no <c>handle_unknown</c> on this transformer, unlike on
    /// <see cref="OneHotEncoder{T}"/> and <see cref="OrdinalEncoder{T}"/>.
    /// </remarks>
    public int[] Transform(ReadOnlySpan<T> labels)
    {
        CategoryMatrix.RequireNoNull(labels);

        var codes = new int[labels.Length];
        for (int i = 0; i < labels.Length; i++)
        {
            int position = _index.IndexOf(0, labels[i]);
            if (position < 0)
            {
                throw new ArgumentException(
                    $"labels[{i}] is not one of the {_classes.Length} labels this encoder was fitted on.",
                    nameof(labels));
            }

            codes[i] = position;
        }

        return codes;
    }

    /// <summary>Reads each code back as the label it stands for.</summary>
    /// <param name="codes">The codes; each in <c>[0, Classes.Count)</c>.</param>
    /// <returns>One label per code.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A code is outside the range of <see cref="Classes"/>.</exception>
    public T[] InverseTransform(ReadOnlySpan<int> codes)
    {
        var labels = new T[codes.Length];
        for (int i = 0; i < codes.Length; i++)
        {
            if (codes[i] < 0 || codes[i] >= _classes.Length)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(codes), codes[i], $"A code must lie in [0, {_classes.Length}).");
            }

            labels[i] = _classes[codes[i]];
        }

        return labels;
    }
}
