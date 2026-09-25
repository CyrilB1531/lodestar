namespace Lodestar.Preprocessing;

/// <summary>Fits the categorical encoders, at <c>sklearn.preprocessing</c> parity.</summary>
/// <remarks>
/// A static factory rather than a <c>Fit</c> on each encoder: a public static on a generic type is
/// what CA1000 refuses, and inference reads better — the shape <see cref="Splitters"/> already has.
/// <strong>One element type per call</strong>, as a 2-D array carries one dtype there too.
/// </remarks>
public static class Encoders
{
    /// <summary>Fits a one-hot encoder on a row-major matrix of categories.</summary>
    /// <typeparam name="T">The category type; <see cref="string"/> and <see cref="int"/> are the two the reference takes.</typeparam>
    /// <param name="values">The categories, row-major: <paramref name="featureCount"/> per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="options">Which category to drop and what to do with an unseen value; <see langword="null"/> drops none and refuses.</param>
    /// <returns>A fitted <see cref="OneHotEncoder{T}"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, or <paramref name="options"/> holds a <see cref="CategoryDrop"/> or <see cref="UnknownCategory"/> that is not a defined value, a <see cref="OneHotEncoderOptions.MinFrequency"/> or <see cref="OneHotEncoderOptions.MaxCategories"/> below 1, or a <see cref="OneHotEncoderOptions.MinFrequencyShare"/> outside <c>(0, 1)</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="values"/> holds no row, a partial one, or a null; or <paramref name="options"/> sets both <see cref="OneHotEncoderOptions.MinFrequency"/> and <see cref="OneHotEncoderOptions.MinFrequencyShare"/>.</exception>
    public static OneHotEncoder<T> OneHot<T>(
        ReadOnlySpan<T> values, int featureCount, OneHotEncoderOptions? options = null)
        where T : IComparable<T>, IEquatable<T> =>
        OneHotEncoder<T>.FitCore(values, featureCount, options);

    /// <summary>Fits an ordinal encoder on a row-major matrix of categories.</summary>
    /// <typeparam name="T">The category type; <see cref="string"/> and <see cref="int"/> are the two the reference takes.</typeparam>
    /// <param name="values">The categories, row-major: <paramref name="featureCount"/> per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <returns>A fitted <see cref="OrdinalEncoder{T}"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive.</exception>
    /// <exception cref="ArgumentException"><paramref name="values"/> holds no row, a partial one, or a null.</exception>
    public static OrdinalEncoder<T> Ordinal<T>(ReadOnlySpan<T> values, int featureCount)
        where T : IComparable<T>, IEquatable<T> =>
        OrdinalEncoder<T>.FitCore(values, featureCount);

    /// <summary>Fits a label encoder on one column of labels.</summary>
    /// <typeparam name="T">The label type; <see cref="string"/> and <see cref="int"/> are the two the reference takes.</typeparam>
    /// <param name="labels">The labels, one column.</param>
    /// <returns>A fitted <see cref="LabelEncoder{T}"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="labels"/> is empty or holds a null.</exception>
    public static LabelEncoder<T> Label<T>(ReadOnlySpan<T> labels)
        where T : IComparable<T>, IEquatable<T> =>
        LabelEncoder<T>.FitCore(labels);
}
