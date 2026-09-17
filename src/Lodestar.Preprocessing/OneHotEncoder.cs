using Lodestar.Preprocessing.Internal;

namespace Lodestar.Preprocessing;

/// <summary>
/// Encodes each categorical feature as one column per category, at
/// <c>sklearn.preprocessing.OneHotEncoder</c> parity.
/// </summary>
/// <typeparam name="T">The category type — <see cref="string"/> and <see cref="int"/> are the two the reference takes.</typeparam>
/// <remarks>
/// Row-major in, row-major out, and one element type per call, which is what a 2-D array carries
/// there too: a caller with a string column and an integer column makes two calls and concatenates.
/// </remarks>
public sealed class OneHotEncoder<T>
    where T : IComparable<T>, IEquatable<T>
{
    private readonly T[][] _categories;
    private readonly CategoryIndex<T> _index;
    private readonly int[] _dropped;
    private readonly int[] _offsets;
    private readonly UnknownCategory _unknown;

    private OneHotEncoder(
        int featureCount,
        int sampleCount,
        T[][] categories,
        int[] dropped,
        int[] offsets,
        int encodedFeatureCount,
        UnknownCategory unknown)
    {
        FeatureCount = featureCount;
        SampleCount = sampleCount;
        EncodedFeatureCount = encodedFeatureCount;
        _categories = categories;
        _index = new CategoryIndex<T>(categories);
        _dropped = dropped;
        _offsets = offsets;
        _unknown = unknown;
    }

    /// <summary>How many values each row of the input carries.</summary>
    public int FeatureCount { get; }

    /// <summary>How many rows the encoder was fitted on.</summary>
    public int SampleCount { get; }

    /// <summary>How many columns <see cref="Transform"/> produces, dropped categories excluded.</summary>
    public int EncodedFeatureCount { get; }

    /// <summary>Each feature's categories, sorted — the reference's <c>categories_</c>.</summary>
    /// <remarks>Strings sort by code point, which is numpy's order rather than a culture's.</remarks>
    public IReadOnlyList<IReadOnlyList<T>> Categories => _categories;

    /// <summary>What <see cref="Encoders.OneHot"/> calls; the factory is there so this type carries no public static.</summary>
    internal static OneHotEncoder<T> FitCore(
        ReadOnlySpan<T> values, int featureCount, OneHotEncoderOptions? options)
    {
        Guard.NotLessThan(featureCount, 1);
        OneHotEncoderOptions settings = options ?? new OneHotEncoderOptions();

        // An undefined value is refused, where the switches below would read it as no drop or as ignoring (#912).
        if (settings.Drop is < CategoryDrop.None or > CategoryDrop.IfBinary)
        {
            throw new ArgumentOutOfRangeException(nameof(options), settings.Drop, "Not a defined category drop.");
        }

        if (settings.Unknown is < UnknownCategory.Refuse or > UnknownCategory.Ignore)
        {
            throw new ArgumentOutOfRangeException(nameof(options), settings.Unknown, "Not a defined unknown-category handling.");
        }

        int sampleCount = CategoryMatrix.Rows(values, featureCount);
        CategoryMatrix.RequireNoNull(values);

        T[][] categories = CategoryTable.Build(values, featureCount, sampleCount);
        var dropped = new int[featureCount];
        var offsets = new int[featureCount];
        int width = 0;
        for (int feature = 0; feature < featureCount; feature++)
        {
            dropped[feature] = Dropped(settings.Drop, categories[feature].Length);
            offsets[feature] = width;
            width += categories[feature].Length - (dropped[feature] < 0 ? 0 : 1);
        }

        return new OneHotEncoder<T>(
            featureCount, sampleCount, categories, dropped, offsets, width, settings.Unknown);
    }

    /// <summary>Encodes a row-major matrix of categories.</summary>
    /// <param name="values">The categories to encode, row-major, with <see cref="FeatureCount"/> per row.</param>
    /// <returns>A new array of <c>rows × <see cref="EncodedFeatureCount"/></c> values, each 0 or 1.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="values"/> holds no row, a partial one, or a null; or it holds a category the
    /// fit never saw and <see cref="OneHotEncoderOptions.Unknown"/> is <see cref="UnknownCategory.Refuse"/>.
    /// </exception>
    /// <remarks>
    /// An ignored unknown encodes to all zeros, and so does a dropped first category: a row of zeros
    /// means either, which is the reference's own collision.
    /// </remarks>
    public double[] Transform(ReadOnlySpan<T> values)
    {
        int rows = CategoryMatrix.Rows(values, FeatureCount);
        CategoryMatrix.RequireNoNull(values);

        var encoded = new double[rows * EncodedFeatureCount];
        for (int row = 0; row < rows; row++)
        {
            for (int feature = 0; feature < FeatureCount; feature++)
            {
                T value = values[(row * FeatureCount) + feature];
                int index = _index.IndexOf(feature, value);
                if (index < 0)
                {
                    if (_unknown == UnknownCategory.Refuse)
                    {
                        throw new ArgumentException(
                            $"feature {feature} has no category {value}. Fit saw "
                            + $"{_categories[feature].Length} of them; pass UnknownCategory.Ignore to "
                            + "encode an unseen value as all zeros instead.",
                            nameof(values));
                    }

                    // Ignored: every column of this feature stays zero, which is also what a dropped
                    // first category looks like. The reference accepts that collision.
                    continue;
                }

                int column = Column(feature, index);
                if (column >= 0)
                {
                    encoded[(row * EncodedFeatureCount) + column] = 1.0;
                }
            }
        }

        return encoded;
    }

    /// <summary>Which category index a feature drops, or <c>-1</c> when it drops none.</summary>
    private static int Dropped(CategoryDrop drop, int categoryCount) => drop switch
    {
        CategoryDrop.First => 0,
        CategoryDrop.IfBinary => categoryCount == 2 ? 0 : -1,
        _ => -1,
    };

    /// <summary>The column a category lands in, or <c>-1</c> when it is the dropped one.</summary>
    private int Column(int feature, int index)
    {
        int dropped = _dropped[feature];
        if (dropped < 0)
        {
            return _offsets[feature] + index;
        }

        if (index == dropped)
        {
            return -1;
        }

        return _offsets[feature] + (index > dropped ? index - 1 : index);
    }
}
