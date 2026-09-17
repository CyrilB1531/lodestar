using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// What the corpus does not reach for #764: every refusal, the collision a row of zeros carries,
/// and the two rules the reference states in prose rather than in a number.
/// </summary>
public sealed class EncodersEdgeTests
{
    private static readonly string[] TwoByTwo = ["a", "x", "b", "y"];

    [Fact]
    public void A_feature_count_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Encoders.OneHot<string>(TwoByTwo, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Encoders.Ordinal<string>(TwoByTwo, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => SimpleImputer.Fit([1.0, 2.0], 0));
    }

    [Fact]
    public void A_span_that_is_not_a_whole_number_of_rows_is_refused()
    {
        Assert.Throws<ArgumentException>(() => Encoders.OneHot<string>(["a", "b", "c"], 2));
        Assert.Throws<ArgumentException>(() => Encoders.Ordinal<string>([], 2));
        Assert.Throws<ArgumentException>(() => SimpleImputer.Fit([1.0, 2.0, 3.0], 2));
    }

    /// <summary>A category is a value; the reference has no null level either.</summary>
    [Fact]
    public void A_null_category_is_refused()
    {
        Assert.Throws<ArgumentException>(() => Encoders.OneHot<string>(["a", null!], 1));
        Assert.Throws<ArgumentException>(() => Encoders.Ordinal<string>(["a", null!], 1));
    }

    [Fact]
    public void An_unseen_category_is_refused_by_default_and_encoded_as_zeros_when_ignored()
    {
        string[] fitted = ["a", "b", "c"];
        OneHotEncoder<string> strict = Encoders.OneHot<string>(fitted, 1);
        OneHotEncoder<string> lenient = Encoders.OneHot<string>(
            fitted, 1, new OneHotEncoderOptions { Unknown = UnknownCategory.Ignore });

        Assert.Throws<ArgumentException>(() => strict.Transform(["zzz"]));
        Assert.Equal([0.0, 0.0, 0.0], lenient.Transform(["zzz"]));

        // The ordinal encoder has no ignore: an index has to be some number, and the reference's
        // use_encoded_value needs one outside the codes that nothing here asks for yet.
        Assert.Throws<ArgumentException>(() => Encoders.Ordinal<string>(fitted, 1).Transform(["zzz"]));
    }

    /// <summary>
    /// A row of zeros means either "unknown" or "the dropped category", and nothing distinguishes
    /// them — the reference's own collision, pinned here so it is not mistaken for a defect.
    /// </summary>
    [Fact]
    public void An_ignored_unknown_and_a_dropped_first_category_encode_alike()
    {
        OneHotEncoder<string> encoder = Encoders.OneHot<string>(
            ["a", "b", "c"],
            1,
            new OneHotEncoderOptions { Drop = CategoryDrop.First, Unknown = UnknownCategory.Ignore });

        Assert.Equal(encoder.Transform(["a"]), encoder.Transform(["zzz"]));
        Assert.Equal([0.0, 0.0], encoder.Transform(["a"]));
    }

    /// <summary>
    /// `if_binary` drops the first category only where the feature has exactly two of them, which
    /// is what separates it from `first`: the three-category feature below keeps all three columns.
    /// </summary>
    [Fact]
    public void If_binary_drops_only_a_two_category_feature()
    {
        var options = new OneHotEncoderOptions { Drop = CategoryDrop.IfBinary };

        OneHotEncoder<string> binary = Encoders.OneHot<string>(["y", "n", "y"], 1, options);
        OneHotEncoder<string> three = Encoders.OneHot<string>(["a", "b", "c"], 1, options);

        Assert.Equal(1, binary.EncodedFeatureCount);
        Assert.Equal(3, three.EncodedFeatureCount);

        // Both features at once: the layout is the first's columns, then the second's.
        OneHotEncoder<string> both = Encoders.OneHot<string>(["y", "a", "n", "b", "y", "c"], 2, options);
        Assert.Equal(4, both.EncodedFeatureCount);
    }

    /// <summary>Strings sort by code point, as numpy's do, and not the way a culture would.</summary>
    [Fact]
    public void Categories_sort_by_code_point()
    {
        OneHotEncoder<string> encoder = Encoders.OneHot<string>(["B", "a", "b", "A"], 1);

        Assert.Equal(["A", "B", "a", "b"], encoder.Categories[0]);
    }

    /// <summary>And integers sort as numbers, which is the whole reason the encoders are generic.</summary>
    [Fact]
    public void Integer_categories_sort_as_numbers()
    {
        OrdinalEncoder<int> encoder = Encoders.Ordinal<int>([10, 2, 33, 2], 1);

        Assert.Equal([2, 10, 33], encoder.Categories[0]);
        Assert.Equal([1.0, 0.0, 2.0, 0.0], encoder.Transform([10, 2, 33, 2]));
    }

    /// <summary>A tie in <c>most_frequent</c> goes to the smaller value, as the reference's does.</summary>
    [Fact]
    public void The_most_frequent_strategy_breaks_a_tie_downward()
    {
        SimpleImputer imputer = SimpleImputer.Fit(
            [1.0, 1.0, 2.0, 2.0, double.NaN],
            1,
            new SimpleImputerOptions { Strategy = ImputationStrategy.MostFrequent });

        Assert.Equal(1.0, imputer.Statistics[0]);
    }

    /// <summary>
    /// A feature with nothing in it is refused, where the reference drops it and returns a narrower
    /// matrix. <see cref="SimpleImputerOptions.KeepEmptyFeatures"/> fills it with zero instead, and
    /// the width stays what the caller passed either way.
    /// </summary>
    [Fact]
    public void An_empty_feature_is_refused_unless_it_is_kept()
    {
        double[] samples = [1.0, double.NaN, 2.0, double.NaN];

        Assert.Throws<ArgumentException>(() => SimpleImputer.Fit(samples, 2));

        SimpleImputer kept = SimpleImputer.Fit(
            samples, 2, new SimpleImputerOptions { KeepEmptyFeatures = true });

        Assert.Equal(0.0, kept.Statistics[1]);
        Assert.Equal([1.0, 0.0, 2.0, 0.0], kept.Transform(samples));
    }

    /// <summary>
    /// Under <see cref="ImputationStrategy.Constant"/> a kept empty feature takes the fill value, not zero, as the
    /// reference's <c>keep_empty_features=True</c> does; without keeping it the feature is still refused (#894).
    /// </summary>
    [Fact]
    public void A_kept_empty_feature_takes_the_constant_fill_value()
    {
        double[] samples = [double.NaN, double.NaN];
        var constant = new SimpleImputerOptions { Strategy = ImputationStrategy.Constant, FillValue = 7.0 };

        Assert.Throws<ArgumentException>(() => SimpleImputer.Fit(samples, 1, constant));

        SimpleImputer kept = SimpleImputer.Fit(samples, 1, constant with { KeepEmptyFeatures = true });

        Assert.Equal(7.0, kept.Statistics[0]);
        Assert.Equal([7.0, 7.0], kept.Transform(samples));
    }

    /// <summary>An infinity marks nothing and would carry into every statistic, so it is refused.</summary>
    [Fact]
    public void An_infinity_is_refused_where_a_nan_is_a_missing_value()
    {
        Assert.Throws<ArgumentException>(() => SimpleImputer.Fit([1.0, double.PositiveInfinity], 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => SimpleImputer.Fit(
            [1.0, double.NaN],
            1,
            new SimpleImputerOptions { Strategy = ImputationStrategy.Constant, FillValue = double.NaN }));
    }

    /// <summary>A matrix with nothing missing comes back unchanged, whatever the strategy.</summary>
    [Fact]
    public void A_matrix_without_a_missing_value_is_returned_as_it_came()
    {
        double[] samples = [1.0, 2.0, 3.0, 4.0];

        Assert.Equal(samples, SimpleImputer.Fit(samples, 2).Transform(samples));
    }

    [Fact]
    public void Transforming_a_shape_the_encoder_was_not_fitted_for_is_refused()
    {
        Assert.Throws<ArgumentException>(() => Encoders.OneHot<string>(TwoByTwo, 2).Transform(["a"]));
        Assert.Throws<ArgumentException>(() => Encoders.Ordinal<string>(TwoByTwo, 2).Transform([]));
        Assert.Throws<ArgumentException>(() => SimpleImputer.Fit([1.0, 2.0, 3.0, 4.0], 2).Transform([1.0]));
    }

    /// <summary>An enum value outside the defined ones is refused, not read as the default it would fall through to (#912).</summary>
    [Fact]
    public void An_undefined_option_value_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SimpleImputer.Fit(
            [1.0, double.NaN], 1, new SimpleImputerOptions { Strategy = (ImputationStrategy)42 }));
        Assert.Throws<ArgumentOutOfRangeException>(() => Encoders.OneHot(
            ["a", "b"], 1, new OneHotEncoderOptions { Drop = (CategoryDrop)9 }));
        Assert.Throws<ArgumentOutOfRangeException>(() => Encoders.OneHot(
            ["a", "b"], 1, new OneHotEncoderOptions { Unknown = (UnknownCategory)7 }));
        Assert.Throws<ArgumentOutOfRangeException>(() => Encoders.OneHot(
            ["a", "b"], 1, new OneHotEncoderOptions { Unknown = (UnknownCategory)(-1) }));
    }
}
