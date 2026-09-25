using Lodestar.Abstractions;
using System.Globalization;
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
    private const string InfrequentName = "infrequent_sklearn";

    private readonly T[][] _categories;
    private readonly CategoryIndex<T> _index;
    private readonly int[]?[] _grouped;
    private readonly int[] _frequentCount;
    private readonly int[] _dropped;
    private readonly int[] _offsets;
    private readonly UnknownCategory _unknown;

    private OneHotEncoder(
        int featureCount, int sampleCount, T[][] categories, CategoryIndex<T> index, int[]?[] grouped, int[] dropped, UnknownCategory unknown)
    {
        FeatureCount = featureCount;
        SampleCount = sampleCount;
        _categories = categories;
        _index = index;
        _grouped = grouped;
        _dropped = dropped;
        _unknown = unknown;
        _frequentCount = new int[featureCount];
        _offsets = new int[featureCount];
        var infrequent = new IReadOnlyList<T>?[featureCount];
        int width = 0;
        for (int feature = 0; feature < featureCount; feature++)
        {
            int[]? map = grouped[feature];
            _frequentCount[feature] = map is null ? categories[feature].Length : map.Max();
            infrequent[feature] = map is null ? null : [.. categories[feature].Where((_, i) => map[i] == _frequentCount[feature])];
            _offsets[feature] = width;
            width += GroupedCount(feature) - (dropped[feature] < 0 ? 0 : 1);
        }

        EncodedFeatureCount = width;
        InfrequentCategories = infrequent;
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

    /// <summary>
    /// Each feature's infrequent categories, sorted, sharing its block's last column; <see langword="null"/> for a
    /// feature with none — the reference's <c>infrequent_categories_</c>.
    /// </summary>
    public IReadOnlyList<IReadOnlyList<T>?> InfrequentCategories { get; }

    /// <summary>What <see cref="Encoders.OneHot"/> calls; the factory is there so this type carries no public static.</summary>
    internal static OneHotEncoder<T> FitCore(
        ReadOnlySpan<T> values, int featureCount, OneHotEncoderOptions? options)
    {
        Guard.NotLessThan(featureCount, 1);
        OneHotEncoderOptions settings = options ?? new OneHotEncoderOptions();
        CheckOptions(settings, nameof(options));

        int sampleCount = CategoryMatrix.Rows(values, featureCount);
        CategoryMatrix.RequireNoNull(values);

        T[][] categories = CategoryTable.Build(values, featureCount, sampleCount);
        var index = new CategoryIndex<T>(categories);
        var grouped = new int[]?[featureCount];
        var dropped = new int[featureCount];
        bool grouping = settings.MinFrequency.HasValue || settings.MinFrequencyShare.HasValue || settings.MaxCategories.HasValue;
        for (int feature = 0; feature < featureCount; feature++)
        {
            // Counting costs a lookup per value, so a fit that groups nothing does not pay it.
            grouped[feature] = grouping
                ? InfrequentGrouping.Map(
                    Counts(values, featureCount, sampleCount, feature, index, categories[feature].Length),
                    sampleCount, settings.MinFrequency, settings.MinFrequencyShare, settings.MaxCategories)
                : null;
            int groupedCount = grouped[feature] is { } map ? map.Max() + 1 : categories[feature].Length;

            // The reference drops after grouping: the first grouped column, and if_binary counts grouped columns.
            dropped[feature] = Dropped(settings.Drop, groupedCount);
        }

        return new OneHotEncoder<T>(featureCount, sampleCount, categories, index, grouped, dropped, settings.Unknown);
    }

    private static int[] Counts(
        ReadOnlySpan<T> values, int featureCount, int sampleCount, int feature, CategoryIndex<T> index, int categoryCount)
    {
        var counts = new int[categoryCount];
        for (int row = 0; row < sampleCount; row++)
        {
            counts[index.IndexOf(feature, values[(row * featureCount) + feature])]++;
        }

        return counts;
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
                int column = ColumnOf(feature, values[(row * FeatureCount) + feature], nameof(values));
                if (column >= 0)
                {
                    encoded[(row * EncodedFeatureCount) + column] = 1.0;
                }
            }
        }

        return encoded;
    }

    /// <summary>Encodes a row-major matrix of categories into a sparse matrix, the reference's default output.</summary>
    /// <param name="values">The categories to encode, row-major, with <see cref="FeatureCount"/> per row.</param>
    /// <returns>A <c>rows × <see cref="EncodedFeatureCount"/></c> matrix holding the ones <see cref="Transform"/> sets, and nothing else.</returns>
    /// <exception cref="ArgumentException">As <see cref="Transform"/>.</exception>
    /// <remarks>
    /// At most one stored value per feature and row, in ascending column order: a dropped category or an ignored
    /// unknown stores none, as <c>sparse_output=True</c> stores none.
    /// </remarks>
    public CsrMatrix TransformSparse(ReadOnlySpan<T> values)
    {
        int rows = CategoryMatrix.Rows(values, FeatureCount);
        CategoryMatrix.RequireNoNull(values);

        var columns = new List<int>(rows * FeatureCount);
        var pointers = new int[rows + 1];
        for (int row = 0; row < rows; row++)
        {
            for (int feature = 0; feature < FeatureCount; feature++)
            {
                int column = ColumnOf(feature, values[(row * FeatureCount) + feature], nameof(values));
                if (column >= 0)
                {
                    columns.Add(column);
                }
            }

            pointers[row + 1] = columns.Count;
        }

        double[] ones = [.. Enumerable.Repeat(1.0, columns.Count)];
        return CsrMatrix.CreateUnchecked(rows, EncodedFeatureCount, ones, [.. columns], pointers);
    }

    /// <summary>The name of each column <see cref="Transform"/> produces — the reference's <c>get_feature_names_out</c>.</summary>
    /// <param name="inputFeatures">One name per input feature; <see langword="null"/> names them <c>x0</c>, <c>x1</c>, …</param>
    /// <returns><c>feature_category</c> for each column, and <c>feature_infrequent_sklearn</c> for an infrequent one.</returns>
    /// <exception cref="ArgumentException"><paramref name="inputFeatures"/> does not hold <see cref="FeatureCount"/> names.</exception>
    /// <remarks>A category is written as Python's <c>str</c> writes it: a string as it is, an integer in invariant digits, a floating value as <c>1.0</c> or <c>1e-05</c>.</remarks>
    public string[] FeatureNames(IReadOnlyList<string>? inputFeatures = null)
    {
        if (inputFeatures is not null && inputFeatures.Count != FeatureCount)
        {
            throw new ArgumentException(
                $"{inputFeatures.Count} feature names were given for {FeatureCount} features.", nameof(inputFeatures));
        }

        var names = new List<string>(EncodedFeatureCount);
        for (int feature = 0; feature < FeatureCount; feature++)
        {
            string prefix = inputFeatures?[feature] ?? "x" + feature.ToString(CultureInfo.InvariantCulture);
            string[] grouped = GroupedNames(feature);
            for (int g = 0; g < grouped.Length; g++)
            {
                if (g != _dropped[feature])
                {
                    names.Add(prefix + "_" + grouped[g]);
                }
            }
        }

        return [.. names];
    }

    private static void CheckOptions(OneHotEncoderOptions settings, string parameterName)
    {
        // An undefined value is refused, where the switches below would read it as no drop or as ignoring (#912).
        if (settings.Drop is < CategoryDrop.None or > CategoryDrop.IfBinary)
        {
            throw new ArgumentOutOfRangeException(parameterName, settings.Drop, "Not a defined category drop.");
        }

        if (settings.Unknown is < UnknownCategory.Refuse or > UnknownCategory.Infrequent)
        {
            throw new ArgumentOutOfRangeException(parameterName, settings.Unknown, "Not a defined unknown-category handling.");
        }

        if (settings.MinFrequency is < 1 || settings.MaxCategories is < 1)
        {
            throw new ArgumentOutOfRangeException(parameterName, "MinFrequency and MaxCategories are counts of at least 1.");
        }

        if (settings.MinFrequencyShare is { } share && !(share > 0.0 && share < 1.0))
        {
            throw new ArgumentOutOfRangeException(parameterName, share, "MinFrequencyShare lies strictly between 0 and 1.");
        }

        if (settings.MinFrequency.HasValue && settings.MinFrequencyShare.HasValue)
        {
            throw new ArgumentException("MinFrequency and MinFrequencyShare are one setting: give a count or a share.", parameterName);
        }
    }

    /// <summary>Which grouped column a feature drops, or <c>-1</c> when it drops none.</summary>
    private static int Dropped(CategoryDrop drop, int groupedCount) => drop switch
    {
        CategoryDrop.First => 0,
        CategoryDrop.IfBinary => groupedCount == 2 ? 0 : -1,
        _ => -1,
    };

    private int GroupedCount(int feature) =>
        _frequentCount[feature] + (_grouped[feature] is null ? 0 : 1);

    private string[] GroupedNames(int feature)
    {
        int[]? map = _grouped[feature];
        var names = new string[GroupedCount(feature)];
        for (int i = 0; i < _categories[feature].Length; i++)
        {
            int g = map is null ? i : map[i];
            names[g] = map is not null && g == _frequentCount[feature] ? InfrequentName : Format(_categories[feature][i]);
        }

        return names;
    }

    private static string Format(T category) => category switch
    {
        double number => PythonRepr.Double(number),
        float number => PythonRepr.Single(number),
        IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
        _ => category.ToString() ?? string.Empty,
    };

    /// <summary>The column a value lands in, or <c>-1</c> when it sets none.</summary>
    private int ColumnOf(int feature, T value, string parameterName)
    {
        int index = _index.IndexOf(feature, value);
        int grouped;
        if (index >= 0)
        {
            grouped = _grouped[feature] is { } map ? map[index] : index;
        }
        else if (_unknown == UnknownCategory.Refuse)
        {
            throw new ArgumentException(
                $"feature {feature} has no category {value}. Fit saw "
                + $"{_categories[feature].Length} of them; pass UnknownCategory.Ignore to "
                + "encode an unseen value as all zeros instead.",
                parameterName);
        }
        else if (_unknown == UnknownCategory.Infrequent && _grouped[feature] is not null)
        {
            grouped = _frequentCount[feature];
        }
        else
        {
            // Ignored, or no infrequent column to take it: every column of this feature stays zero, which is also
            // what a dropped first category looks like. The reference accepts that collision.
            return -1;
        }

        return Column(feature, grouped);
    }

    /// <summary>The column a grouped category lands in, or <c>-1</c> when it is the dropped one.</summary>
    private int Column(int feature, int grouped)
    {
        int dropped = _dropped[feature];
        if (dropped < 0)
        {
            return _offsets[feature] + grouped;
        }

        if (grouped == dropped)
        {
            return -1;
        }

        return _offsets[feature] + (grouped > dropped ? grouped - 1 : grouped);
    }
}
