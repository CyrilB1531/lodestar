using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>What the infrequent-category options refuse, and the edges the corpus does not isolate (#1161).</summary>
public sealed class OneHotInfrequentEdgeTests
{
    private static readonly string[] Values = ["a", "a", "a", "b", "b", "c", "d"];

    [Fact]
    public void Settings_the_reference_refuses_are_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Encoders.OneHot<string>(Values, 1, new OneHotEncoderOptions { MinFrequency = 0 }));
        Assert.Throws<ArgumentOutOfRangeException>(() => Encoders.OneHot<string>(Values, 1, new OneHotEncoderOptions { MaxCategories = 0 }));
        Assert.Throws<ArgumentOutOfRangeException>(() => Encoders.OneHot<string>(Values, 1, new OneHotEncoderOptions { MinFrequencyShare = 0.0 }));
        Assert.Throws<ArgumentOutOfRangeException>(() => Encoders.OneHot<string>(Values, 1, new OneHotEncoderOptions { MinFrequencyShare = 1.0 }));
        Assert.Throws<ArgumentOutOfRangeException>(() => Encoders.OneHot<string>(Values, 1, new OneHotEncoderOptions { MinFrequencyShare = double.NaN }));
        Assert.Throws<ArgumentOutOfRangeException>(() => Encoders.OneHot<string>(Values, 1, new OneHotEncoderOptions { Unknown = (UnknownCategory)9 }));
        Assert.Throws<ArgumentException>(() =>
            Encoders.OneHot<string>(Values, 1, new OneHotEncoderOptions { MinFrequency = 2, MinFrequencyShare = 0.2 }));
    }

    [Fact]
    public void Feature_names_need_one_name_per_feature()
    {
        OneHotEncoder<string> encoder = Encoders.OneHot<string>(Values, 1, new OneHotEncoderOptions { MinFrequency = 2 });

        Assert.Equal(["colour_a", "colour_b", "colour_infrequent_sklearn"], encoder.FeatureNames(["colour"]));
        Assert.Throws<ArgumentException>(() => encoder.FeatureNames(["colour", "size"]));
    }

    /// <summary>With no infrequent column to take it, an unknown encodes as all zeros, as the reference's does.</summary>
    [Fact]
    public void An_unknown_with_no_infrequent_column_is_all_zeros()
    {
        OneHotEncoder<string> encoder = Encoders.OneHot<string>(Values, 1, new OneHotEncoderOptions { Unknown = UnknownCategory.Infrequent });
        OneHotEncoder<string> grouped = Encoders.OneHot<string>(
            Values, 1, new OneHotEncoderOptions { Unknown = UnknownCategory.Infrequent, MaxCategories = 2 });

        Assert.Equal([0.0, 0.0, 0.0, 0.0], encoder.Transform(["zzz"]));
        Assert.Equal([0.0, 1.0], grouped.Transform(["zzz"]));
        Assert.All(encoder.InfrequentCategories, Assert.Null);
    }

    /// <summary>The sparse output holds exactly the dense output's ones, and nothing for an ignored unknown.</summary>
    [Fact]
    public void The_sparse_output_is_the_dense_one()
    {
        OneHotEncoder<string> encoder = Encoders.OneHot<string>(
            Values, 1, new OneHotEncoderOptions { MinFrequency = 2, Drop = CategoryDrop.First, Unknown = UnknownCategory.Ignore });
        string[] rows = ["a", "b", "zzz", "d"];

        Assert.Equal(encoder.Transform(rows), encoder.TransformSparse(rows).ToDense().Cast<double>());
        Assert.Equal(2, encoder.TransformSparse(rows).NonZeroCount);
    }

    /// <summary>max_categories = 1 groups every category, and drop="first" then drops that single column.</summary>
    [Fact]
    public void One_category_groups_them_all()
    {
        OneHotEncoder<string> encoder = Encoders.OneHot<string>(Values, 1, new OneHotEncoderOptions { MaxCategories = 1 });
        OneHotEncoder<string> dropped = Encoders.OneHot<string>(
            Values, 1, new OneHotEncoderOptions { MaxCategories = 1, Drop = CategoryDrop.First });

        Assert.Equal(["x0_infrequent_sklearn"], encoder.FeatureNames());
        Assert.Equal(["a", "b", "c", "d"], encoder.InfrequentCategories[0]!);
        Assert.Equal(0, dropped.EncodedFeatureCount);
    }
}
