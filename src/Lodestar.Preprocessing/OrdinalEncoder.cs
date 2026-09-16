using Lodestar.Preprocessing.Internal;

namespace Lodestar.Preprocessing;

/// <summary>
/// Encodes each categorical feature as its category's index, at
/// <c>sklearn.preprocessing.OrdinalEncoder</c> parity.
/// </summary>
/// <typeparam name="T">The category type — <see cref="string"/> and <see cref="int"/> are the two the reference takes.</typeparam>
/// <remarks>
/// One column in, one column out, where <see cref="OneHotEncoder{T}"/> gives each category a column
/// of its own. The index is into <see cref="Categories"/>, which is sorted — so the codes carry the
/// sort order, and a model that reads them as numbers reads that order as distance.
/// </remarks>
public sealed class OrdinalEncoder<T>
    where T : IComparable<T>, IEquatable<T>
{
    private readonly T[][] _categories;

    private OrdinalEncoder(int featureCount, int sampleCount, T[][] categories)
    {
        FeatureCount = featureCount;
        SampleCount = sampleCount;
        _categories = categories;
    }

    /// <summary>How many values each row carries.</summary>
    public int FeatureCount { get; }

    /// <summary>How many rows the encoder was fitted on.</summary>
    public int SampleCount { get; }

    /// <summary>Each feature's categories, sorted — the reference's <c>categories_</c>.</summary>
    public IReadOnlyList<IReadOnlyList<T>> Categories => _categories;

    /// <summary>What <see cref="Encoders.Ordinal"/> calls; the factory is there so this type carries no public static.</summary>
    internal static OrdinalEncoder<T> FitCore(ReadOnlySpan<T> values, int featureCount)
    {
        Guard.NotLessThan(featureCount, 1);
        int sampleCount = CategoryMatrix.Rows(values, featureCount);
        CategoryMatrix.RequireNoNull(values);

        return new OrdinalEncoder<T>(
            featureCount, sampleCount, CategoryTable.Build(values, featureCount, sampleCount));
    }

    /// <summary>Encodes a row-major matrix as category indices.</summary>
    /// <param name="values">The categories to encode, row-major, with <see cref="FeatureCount"/> per row.</param>
    /// <returns>A new array of the same length, each value the index of its category.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="values"/> holds no row, a partial one, a null, or a category the fit never saw.
    /// </exception>
    /// <remarks>
    /// An unseen category is refused rather than encoded: the reference's own default raises, and its
    /// <c>use_encoded_value</c> needs a value outside the codes that this package has no caller for
    /// yet (decision 0095's rule).
    /// </remarks>
    public double[] Transform(ReadOnlySpan<T> values)
    {
        int rows = CategoryMatrix.Rows(values, FeatureCount);
        CategoryMatrix.RequireNoNull(values);

        var encoded = new double[values.Length];
        for (int row = 0; row < rows; row++)
        {
            for (int feature = 0; feature < FeatureCount; feature++)
            {
                int position = (row * FeatureCount) + feature;
                int index = CategoryTable.IndexOf(_categories[feature], values[position]);
                if (index < 0)
                {
                    throw new ArgumentException(
                        $"feature {feature} has no category {values[position]}. Fit saw "
                        + $"{_categories[feature].Length} of them.",
                        nameof(values));
                }

                encoded[position] = index;
            }
        }

        return encoded;
    }
}
